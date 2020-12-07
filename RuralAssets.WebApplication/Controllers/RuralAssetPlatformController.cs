using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using AElf;
using AElf.Client.Service;
using AElf.Contracts.Assets;
using AElf.Cryptography;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace RuralAssets.WebApplication.Controllers
{
    [ApiController]
    [Route("api/v1.0/asset")]
    public class RuralAssetPlatformController : ControllerBase
    {
        private readonly ILogger<RuralAssetPlatformController> _logger;
        private readonly IValidationService _validationService;
        private readonly AccountManager _accountManager;
        private readonly ConfigOptions _configOptions;

        public RuralAssetPlatformController(ILogger<RuralAssetPlatformController> logger,
            IValidationService validationService, IOptionsSnapshot<ConfigOptions> configOptions,
            AccountManager accountManager)
        {
            _logger = logger;
            _validationService = validationService;
            _accountManager = accountManager;
            _configOptions = configOptions.Value;
        }

        [HttpPost("check")]
        public async Task<ResponseDto> CheckAsync(CheckInput input)
        {
            MessageHelper.Message message;
            if (!_validationService.ValidateIdCard(input.IdCard))
            {
                message = MessageHelper.Message.WrongIdCard;
                return new ResponseDto
                {
                    Code = MessageHelper.GetCode(message),
                    Msg = MessageHelper.GetMessage(message)
                };
            }

            if (string.IsNullOrEmpty(input.IdCard) || string.IsNullOrEmpty(input.Name))
            {
                message = MessageHelper.Message.ParameterMissed;
                return new ResponseDto
                {
                    Code = MessageHelper.GetCode(message),
                    Msg = MessageHelper.GetMessage(message)
                };
            }

            if (!string.IsNullOrEmpty(input.Year) && !_validationService.ValidateYear(input.Year))
            {
                message = MessageHelper.Message.ParameterTypeNotMatch;
                return new ResponseDto
                {
                    Code = MessageHelper.GetCode(message),
                    Msg = MessageHelper.GetMessage(message)
                };
            }

            message = MessageHelper.Message.Success;
            var sql = SqlStatementHelper.GetCheckSql(input.Name, input.IdCard, input.Year);
            var dataReader =
                await MySqlHelper.ExecuteReaderAsync(_configOptions.RuralAssetsConnectString, sql);
            dataReader.Read();
            var result = dataReader[0].ToString();
            string description;
            switch (result)
            {
                case "1":
                    description = "核验通过";
                    break;
                case "2":
                    description = "用户不存在";
                    break;
                case "3":
                    description = "资产不存在";
                    break;
                case "4":
                    description = "资产不符合要求";
                    break;
                default:
                    description = "系统异常";
                    break;
            }

            return new ResponseDto
            {
                Code = MessageHelper.GetCode(message),
                Msg = MessageHelper.GetMessage(message),
                Result = result,
                Description = description
            };
        }

        [HttpPost("query_credit")]
        public async Task<QueryCreditResponseDto> QueryCreditAsync(QueryCreditInput input)
        {
            MessageHelper.Message message;
            if (!_validationService.ValidateIdCard(input.IdCard))
            {
                message = MessageHelper.Message.WrongIdCard;
                return new QueryCreditResponseDto
                {
                    Code = MessageHelper.GetCode(message),
                    Msg = MessageHelper.GetMessage(message)
                };
            }

            if (string.IsNullOrEmpty(input.IdCard) || string.IsNullOrEmpty(input.Name))
            {
                message = MessageHelper.Message.ParameterMissed;
                return new QueryCreditResponseDto
                {
                    Code = MessageHelper.GetCode(message),
                    Msg = MessageHelper.GetMessage(message)
                };
            }

            var sql = SqlStatementHelper.GetQueryCreditSql(input.Name, input.IdCard);
            var dataReader =
                await MySqlHelper.ExecuteReaderAsync(_configOptions.RuralAssetsConnectString, sql);
            var assetRequestList = new List<AssetRequest>();
            while (dataReader.Read())
            {
                assetRequestList.Add(new AssetRequest
                {
                    AssetId = ParseToInt(dataReader, 3),
                    BCZJE = ParseToDouble(dataReader, 34)
                });
            }

            var client = new HttpClient();
            var request = new QueryCreditRequest
            {
                AssetList = assetRequestList
            };
            var options = new JsonSerializerOptions();
            var text = JsonSerializer.Serialize(request, options);
            var responseMessage = await client.PostAsync(_configOptions.CreditUri,
                new StringContent(text));
            var response = await responseMessage.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<QueryCreditResponseDto>(response);
        }

        [HttpPost("change_status")]
        public async Task<ResponseDto> ChangeStatusAsync(ChangeStatusInput input)
        {
            MessageHelper.Message message;
            if (!_validationService.ValidateIdCard(input.IdCard))
            {
                message = MessageHelper.Message.WrongIdCard;
                return new ResponseDto
                {
                    Code = MessageHelper.GetCode(message),
                    Msg = MessageHelper.GetMessage(message)
                };
            }

            if (string.IsNullOrEmpty(input.IdCard) || string.IsNullOrEmpty(input.Name) || !input.AssetList.Any() ||
                (input.AssetType != 1 && input.AssetType != 2))
            {
                message = MessageHelper.Message.ParameterMissed;
                return new ResponseDto
                {
                    Code = MessageHelper.GetCode(message),
                    Msg = MessageHelper.GetMessage(message)
                };
            }

            var nodeManager = new NodeManager(_configOptions.BlockChainEndpoint);
            var txId = nodeManager.SendTransaction(_configOptions.AccountAddress, _configOptions.RuralContractAddress,
                "SetAssetInfo", new AssetInfo
                {
                    Name = input.Name,
                    IdCard = input.IdCard,
                    AssetType = input.AssetType,
                    AssetIdList = {input.AssetList.Select(a => a.AssetId)},
                    AssetList =
                    {
                        input.AssetList.Select(a => new Asset
                        {
                            AssetId = a.AssetId,
                            Status = a.Status,
                            BankId = a.BankId,
                            LoanAmount = DoubleToLong(a.LoanAmount),
                            LoanAgreement = ByteString.CopyFrom(a.LoanAgreement),
                            DueDate = Timestamp.FromDateTime(DateTime.Parse(a.DueDate)),
                            LoanRate = DoubleToLong(a.LoanRate),
                            IdCard = input.IdCard
                        })
                    }
                });
            var result = nodeManager.CheckTransactionResult(txId);

            var sql = SqlStatementHelper.GetQueryCreditSql(input.Name, input.IdCard);
            var row = await MySqlHelper.ExecuteNonQueryAsync(_configOptions.RuralAssetsConnectString, sql);
            return new ResponseDto
            {
                Code = MessageHelper.GetCode(MessageHelper.Message.Success),
                Msg = MessageHelper.GetMessage(MessageHelper.Message.Success),
                Result = row > 0 ? "1" : "0",
                Description = $"{(row > 0 ? "成功" : "失败")}\n交易Id：{txId}\n交易状态：{result.Status}"
            };
        }

        private long DoubleToLong(double origin)
        {
            return (long) origin * 10000;
        }

        [HttpPost("list")]
        public async Task<ListResponseDto> ListAsync(ListInput input)
        {
            var message = MessageHelper.Message.Success;
            if (!string.IsNullOrEmpty(input.Idcard) && !_validationService.ValidateIdCard(input.Idcard))
            {
                message = MessageHelper.Message.WrongIdCard;
                return new ListResponseDto
                {
                    Code = MessageHelper.GetCode(message),
                    Msg = MessageHelper.GetMessage(message)
                };
            }

            if (input.AssetType != 1 && input.AssetType != 2)
            {
                message = MessageHelper.Message.ParameterMissed;
                return new ListResponseDto
                {
                    Code = MessageHelper.GetCode(message),
                    Msg = MessageHelper.GetMessage(message)
                };
            }

            // TODO: Type check.

            var response = new ListResponseDto
            {
                Code = MessageHelper.GetCode(message),
                Msg = MessageHelper.GetMessage(message),
                List = new List<AssetDto>()
            };
            var sql = SqlStatementHelper.GetListSql(input.Name, input.Idcard, input.AssetId, input.BFZT, input.LSX,
                input.LSXZ, input.LSC);
            var dataReader =
                await MySqlHelper.ExecuteReaderAsync(_configOptions.RuralAssetsConnectString, sql);
            while (dataReader.Read())
            {
                response.List.Add(new AssetDto
                {
                    AssetId = ParseToInt(dataReader, 0),
                    PC = dataReader[1]?.ToString() ?? string.Empty,
                    PCH = dataReader[2]?.ToString() ?? string.Empty,
                    XMMCId = ParseToInt(dataReader, 3),
                    XMMCMS = dataReader[4]?.ToString() ?? string.Empty,
                    Name = dataReader[5]?.ToString() ?? string.Empty,
                    SFZH = dataReader[6]?.ToString() ?? string.Empty,
                    LXFS = dataReader[7]?.ToString() ?? string.Empty,
                    KHYH = dataReader[8]?.ToString() ?? string.Empty,
                    ZHXZId = ParseToInt(dataReader, 9),
                    ZHXZMS = dataReader[10]?.ToString() ?? string.Empty,
                    ZCHMJ = dataReader[11]?.ToString() ?? string.Empty,
                    BCZJE = dataReader[12]?.ToString() ?? string.Empty,
                    BFZTMS = dataReader[14]?.ToString() ?? string.Empty,
                    LSX = dataReader[16]?.ToString() ?? string.Empty,
                    LSXZ = dataReader[18]?.ToString() ?? string.Empty,
                    LSC = dataReader[20]?.ToString() ?? string.Empty
                });
            }

            return response;
        }

        [HttpPost("details")]
        public async Task<DetailsResponseDto> DetailsAsync(DetailsInput input)
        {
            var message = MessageHelper.Message.Success;
            if (!string.IsNullOrEmpty(input.Idcard) && !_validationService.ValidateIdCard(input.Idcard))
            {
                message = MessageHelper.Message.WrongIdCard;
                return new DetailsResponseDto
                {
                    Code = MessageHelper.GetCode(message),
                    Msg = MessageHelper.GetMessage(message)
                };
            }

            if (string.IsNullOrEmpty(input.Name) || string.IsNullOrEmpty(input.Idcard) || input.AssetId == 0 ||
                (input.AssetType != 1 && input.AssetType != 2))
            {
                message = MessageHelper.Message.ParameterMissed;
                return new DetailsResponseDto
                {
                    Code = MessageHelper.GetCode(message),
                    Msg = MessageHelper.GetMessage(message)
                };
            }

            var sql = SqlStatementHelper.GetDetailSql(input.Name, input.Idcard, input.AssetId);
            var dataReader =
                await MySqlHelper.ExecuteReaderAsync(_configOptions.RuralAssetsConnectString, sql);
            dataReader.Read();
            return new DetailsResponseDto
            {
                Code = MessageHelper.GetCode(message),
                Msg = MessageHelper.GetMessage(message),
                AssetId = input.AssetId,
                BlockId = dataReader[1]?.ToString() ?? string.Empty,
                Id = ParseToInt(dataReader, 2),
                SKR = dataReader[3]?.ToString() ?? string.Empty,
                SFZH = dataReader[4]?.ToString() ?? string.Empty,
                LXFS = dataReader[5]?.ToString() ?? string.Empty,
                YHLX = dataReader[6]?.ToString() ?? string.Empty,
                KHSZYH = dataReader[7]?.ToString() ?? string.Empty,
                KHYH = dataReader[8]?.ToString() ?? string.Empty,
                LHH = dataReader[9]?.ToString() ?? string.Empty,
                YHZH = dataReader[10]?.ToString() ?? string.Empty,
                SFKH = dataReader[11]?.ToString() ?? string.Empty,
                ZHXZ = dataReader[12]?.ToString() ?? string.Empty,
                NTFW = dataReader[13]?.ToString() ?? string.Empty,
                DZ = dataReader[14]?.ToString() ?? string.Empty,
                XZ = dataReader[15]?.ToString() ?? string.Empty,
                NZ = dataReader[16]?.ToString() ?? string.Empty,
                BF = dataReader[17]?.ToString() ?? string.Empty,
                ZCHMJ = ParseToDouble(dataReader, 18),
                DTZW = ParseToDouble(dataReader, 19),
                SC = ParseToDouble(dataReader, 20),
                JJZW = ParseToDouble(dataReader, 21),
                SM = ParseToDouble(dataReader, 22),
                SMZ = ParseToInt(dataReader, 23),
                DPZW = ParseToDouble(dataReader, 24),
                QT = dataReader[25]?.ToString() ?? string.Empty,
                TDSYJ = ParseToDouble(dataReader, 26),
                DTZWJE = ParseToDouble(dataReader, 27),
                SCJE = ParseToDouble(dataReader, 28),
                QTZWJE = ParseToDouble(dataReader, 29),
                DPZWJE = ParseToDouble(dataReader, 30),
                QTFZWJE = ParseToDouble(dataReader, 31),
                BCZJE = ParseToDouble(dataReader, 32),
                ZJEDX = dataReader[33]?.ToString() ?? string.Empty,
                FJ = dataReader[34]?.ToString() ?? string.Empty,
                BZ = dataReader[35]?.ToString() ?? string.Empty,
                CJSJ = dataReader[36]?.ToString() ?? string.Empty,
                CJR = dataReader[37]?.ToString() ?? string.Empty,
                BFZT = dataReader[38]?.ToString() ?? string.Empty,
                SFSC = ParseToDouble(dataReader, 39),
                HTFJ = dataReader[40]?.ToString() ?? string.Empty,
                TDBCMXPC = ParseToDouble(dataReader, 41),
                Dealno = dataReader[42]?.ToString() ?? string.Empty,
                XMMC = dataReader[43]?.ToString() ?? string.Empty,
                XMLX = dataReader[44]?.ToString() ?? string.Empty,
                SZZW = dataReader[45]?.ToString() ?? string.Empty,
                LSX = dataReader[46]?.ToString() ?? string.Empty,
                LSXZ = dataReader[47]?.ToString() ?? string.Empty,
                LSC = dataReader[48]?.ToString() ?? string.Empty,
                SFXX = dataReader[49]?.ToString() ?? string.Empty,
            };
        }

        private int ParseToInt(MySqlDataReader dataReader, int index)
        {
            return int.Parse(
                string.IsNullOrEmpty(dataReader[index]?.ToString()) ? "0" : dataReader[index].ToString());
        }

        private double ParseToDouble(MySqlDataReader dataReader, int index)
        {
            return double.Parse(
                string.IsNullOrEmpty(dataReader[index]?.ToString()) ? "0" : dataReader[index].ToString());
        }
    }
}