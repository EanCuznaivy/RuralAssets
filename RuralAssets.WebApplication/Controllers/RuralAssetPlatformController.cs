using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;
using AElf;
using AElf.Client.Dto;
using AElf.Contracts.Assets;
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
        private readonly IChangeStatusService _changeStatusService;
        private readonly IFileValidationService _fileValidationService;
        private readonly ConfigOptions _configOptions;

        public RuralAssetPlatformController(ILogger<RuralAssetPlatformController> logger,
            IValidationService validationService, IOptionsSnapshot<ConfigOptions> configOptions,
            IChangeStatusService changeStatusService, IFileValidationService fileValidationService)
        {
            _logger = logger;
            _validationService = validationService;
            _changeStatusService = changeStatusService;
            _configOptions = configOptions.Value;
            _fileValidationService = fileValidationService;
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
            var sql = input.AssetType == "1"
                ? SqlStatementHelper.GetCheckSql(input.Name, input.IdCard, input.Year)
                : SqlStatementHelper.GetConstructionCheckSql(input.Name, input.IdCard, input.Year);
            var dataReader =
                await MySqlHelper.ExecuteReaderAsync(_configOptions.RuralAssetsConnectString, sql);
            dataReader.Read();
            var result = dataReader[0].ToString();
            GetCheckDescription(result, out var description);

            return new ResponseDto
            {
                Code = MessageHelper.GetCode(message),
                Msg = MessageHelper.GetMessage(message),
                Result = result,
                Description = description
            };
        }

        private void GetCheckDescription(string result, out string description)
        {
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
                    AssetId = CommonHelper.ParseToInt(dataReader, 3),
                    BCZJE = CommonHelper.ParseToDouble(dataReader, 34)
                });
            }
            
            sql = SqlStatementHelper.GetQueryConstructionCreditSql(input.Name, input.IdCard);
            dataReader =
                await MySqlHelper.ExecuteReaderAsync(_configOptions.RuralAssetsConnectString, sql);
            while (dataReader.Read())
            {
                assetRequestList.Add(new AssetRequest
                {
                    AssetId = CommonHelper.ParseToInt(dataReader, 0),
                    BCZJE = CommonHelper.ParseToDouble(dataReader, 3)
                });
            }
            
            var client = new HttpClient();
            var request = new QueryCreditRequest
            {
                Name = input.Name,
                IdCard = input.IdCard,
                AssetList = assetRequestList
            };
            var options = new JsonSerializerOptions
            {
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(UnicodeRanges.All)
            };
            var text = JsonSerializer.Serialize(request, options);
            _logger.LogInformation(text);

            var content = new StringContent(text);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var responseMessage = await client.PostAsync(_configOptions.CreditUri, content);
            var response = await responseMessage.Content.ReadAsStringAsync();

            var res = JsonSerializer.Deserialize<QueryCreditResponseDto>(response);

            if (res.Code != "0000")
            {
                res.Msg += $" 请求Credit使用参数：{text}";
            }

            return res;
        }

        [HttpPost("change_status")]
        public async Task<ResponseDto> ChangeStatusAsync(ChangeStatusInput input)
        {
            return await _changeStatusService.ChangeStatusAsync(input);
        }

        [HttpPost("list")]
        public async Task<ListResponseDto> ListAsync(ListInput input)
        {
            var message = MessageHelper.Message.Success;
            if (!string.IsNullOrEmpty(input.IdCard) && !_validationService.ValidateIdCard(input.IdCard))
            {
                message = MessageHelper.Message.WrongIdCard;
                return new ListResponseDto
                {
                    Code = MessageHelper.GetCode(message),
                    Msg = MessageHelper.GetMessage(message)
                };
            }

            if (input.AssetType != "1" && input.AssetType != "2")
            {
                message = MessageHelper.Message.ParameterMissed;
                return new ListResponseDto
                {
                    Code = MessageHelper.GetCode(message),
                    Msg = MessageHelper.GetMessage(message)
                };
            }

            var pageNo = Convert.ToInt32(input.PageNo);
            var pageSize = Convert.ToInt32(input.PageSize);
            if (pageNo < 0 || pageSize < 0)
            {
                message = MessageHelper.Message.ParameterTypeNotMatch;
                return new ListResponseDto
                {
                    Code = MessageHelper.GetCode(message),
                    Msg = MessageHelper.GetMessage(message),
                    Description = "分页数据不可为负数"
                };
            }

            if (pageSize == 0)
                pageSize = 100;

            // TODO: Type check.

            var response = new ListResponseDto
            {
                Code = MessageHelper.GetCode(message),
                Msg = MessageHelper.GetMessage(message),
                List = new List<AssetDto>()
            };
            int.TryParse(input.BFZT, out var bfzt);
            var sql = input.AssetType == "1"
                ? SqlStatementHelper.GetListSql(input.Name, input.IdCard, Convert.ToInt32(input.AssetId),
                    bfzt, input.LSX, input.LSXZ, input.LSC, pageNo, pageSize)
                : SqlStatementHelper.GetListOfConstructionSql(input.Name, input.IdCard, Convert.ToInt32(input.AssetId),
                    pageNo, pageSize);
            var dataReader =
                await MySqlHelper.ExecuteReaderAsync(_configOptions.RuralAssetsConnectString, sql);
            if (input.AssetType == "1")
            {
                while (dataReader.Read())
                {
                    response.List.Add(new AssetDto
                    {
                        AssetId = CommonHelper.ParseToInt(dataReader, 0),
                        PC = dataReader[1]?.ToString() ?? string.Empty,
                        PCH = dataReader[2]?.ToString() ?? string.Empty,
                        XMMCId = CommonHelper.ParseToInt(dataReader, 3),
                        XMMCMS = dataReader[4]?.ToString() ?? string.Empty,
                        Name = dataReader[5]?.ToString() ?? string.Empty,
                        SFZH = dataReader[6]?.ToString() ?? string.Empty,
                        LXFS = dataReader[7]?.ToString() ?? string.Empty,
                        KHYH = dataReader[8]?.ToString() ?? string.Empty,
                        ZHXZId = CommonHelper.ParseToInt(dataReader, 9),
                        ZHXZMS = dataReader[10]?.ToString() ?? string.Empty,
                        ZCHMJ = dataReader[11]?.ToString() ?? string.Empty,
                        BCZJE = dataReader[12]?.ToString() ?? string.Empty,
                        BFZTMS = dataReader[14]?.ToString() ?? string.Empty,
                        LSX = dataReader[16]?.ToString() ?? string.Empty,
                        LSXZ = dataReader[18]?.ToString() ?? string.Empty,
                        LSC = dataReader[20]?.ToString() ?? string.Empty
                    });
                }
            }
            else
            {
                while (dataReader.Read())
                {
                    response.List.Add(new AssetDto
                    {
                        AssetId = CommonHelper.ParseToInt(dataReader, 0),
                        PC = dataReader[1]?.ToString() ?? string.Empty,
                        PCH = dataReader[2]?.ToString() ?? string.Empty,
                        XMMCId = CommonHelper.ParseToInt(dataReader, 3),
                        XMMCMS = dataReader[4]?.ToString() ?? string.Empty,
                        Name = dataReader[5]?.ToString() ?? string.Empty,
                        SFZH = dataReader[6]?.ToString() ?? string.Empty,
                        LXFS = dataReader[7]?.ToString() ?? string.Empty,
                        KHYH = dataReader[8]?.ToString() ?? string.Empty,
                        ZHXZId = CommonHelper.ParseToInt(dataReader, 9),
                        ZHXZMS = dataReader[10]?.ToString() ?? string.Empty,
                        ZCHMJ = dataReader[11]?.ToString() ?? string.Empty,
                        BCZJE = dataReader[12]?.ToString() ?? string.Empty,
                        BFZTMS = dataReader[13]?.ToString() ?? string.Empty,
                        LSX = dataReader[14]?.ToString() ?? string.Empty,
                        LSXZ = dataReader[15]?.ToString() ?? string.Empty,
                        LSC = dataReader[16]?.ToString() ?? string.Empty
                    });
                }
            }

            if (!response.List.Any())
            {
                response.List = null;
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

            var assetId = Convert.ToInt32(input.AssetId);
            if (string.IsNullOrEmpty(input.Name) || string.IsNullOrEmpty(input.Idcard) || assetId == 0 ||
                (input.AssetType != "1" && input.AssetType != "2"))
            {
                message = MessageHelper.Message.ParameterMissed;
                return new DetailsResponseDto
                {
                    Code = MessageHelper.GetCode(message),
                    Msg = MessageHelper.GetMessage(message)
                };
            }

            var sql = input.AssetType == "1"
                ? SqlStatementHelper.GetDetailSql(input.Name, input.Idcard, assetId)
                : SqlStatementHelper.GetDetailOfConstructionSql(input.Name, input.Idcard, assetId);
            var dataReader =
                await MySqlHelper.ExecuteReaderAsync(_configOptions.RuralAssetsConnectString, sql);
            dataReader.Read();
            var detailInfo = new DetailsResponseDto
            {
                Code = MessageHelper.GetCode(message),
                Msg = MessageHelper.GetMessage(message),
                AssetId = assetId,
                BlockId = dataReader[1]?.ToString() ?? string.Empty,
                Id = CommonHelper.ParseToInt(dataReader, 2),
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
                ZCHMJ = CommonHelper.ParseToDouble(dataReader, 18),
                DTZW = CommonHelper.ParseToDouble(dataReader, 19),
                SC = CommonHelper.ParseToDouble(dataReader, 20),
                JJZW = CommonHelper.ParseToDouble(dataReader, 21),
                SM = CommonHelper.ParseToDouble(dataReader, 22),
                SMZ = CommonHelper.ParseToInt(dataReader, 23),
                DPZW = CommonHelper.ParseToDouble(dataReader, 24),
                QT = dataReader[25]?.ToString() ?? string.Empty,
                TDSYJ = CommonHelper.ParseToDouble(dataReader, 26),
                DTZWJE = CommonHelper.ParseToDouble(dataReader, 27),
                SCJE = CommonHelper.ParseToDouble(dataReader, 28),
                QTZWJE = CommonHelper.ParseToDouble(dataReader, 29),
                DPZWJE = CommonHelper.ParseToDouble(dataReader, 30),
                QTFZWJE = CommonHelper.ParseToDouble(dataReader, 31),
                BCZJE = CommonHelper.ParseToDouble(dataReader, 32),
                ZJEDX = dataReader[33]?.ToString() ?? string.Empty,
                FJ = dataReader[34]?.ToString() ?? string.Empty,
                BZ = dataReader[35]?.ToString() ?? string.Empty,
                CJSJ = dataReader[36]?.ToString() ?? string.Empty,
                CJR = dataReader[37]?.ToString() ?? string.Empty,
                BFZT = dataReader[38]?.ToString() ?? string.Empty,
                SFSC = CommonHelper.ParseToDouble(dataReader, 39),
                HTFJ = dataReader[40]?.ToString() ?? string.Empty,
                TDBCMXPC = CommonHelper.ParseToDouble(dataReader, 41),
                Dealno = dataReader[42]?.ToString() ?? string.Empty,
                XMMC = dataReader[43]?.ToString() ?? string.Empty
            };
            if (input.AssetType == "1")
            {
                detailInfo.XMLX = dataReader[44]?.ToString() ?? string.Empty;
                detailInfo.SZZW = dataReader[45]?.ToString() ?? string.Empty;
                detailInfo.LSX = dataReader[46]?.ToString() ?? string.Empty;
                detailInfo.LSXZ = dataReader[47]?.ToString() ?? string.Empty;
                detailInfo.LSC = dataReader[48]?.ToString() ?? string.Empty;
                detailInfo.SFXX = dataReader[49]?.ToString() ?? string.Empty;
            }
            else
            {
                detailInfo.XMLX = string.Empty;
                detailInfo.SZZW = dataReader[44]?.ToString() ?? string.Empty;
                detailInfo.LSX = dataReader[45]?.ToString() ?? string.Empty;
                detailInfo.LSXZ = dataReader[46]?.ToString() ?? string.Empty;
                detailInfo.LSC = dataReader[47]?.ToString() ?? string.Empty;
                detailInfo.SFXX = dataReader[48]?.ToString() ?? string.Empty;
            }
            return detailInfo;
        }

        [HttpPost("upload")]
        public async Task<UploadResponseDto> UploadAsync([FromForm] UploadInput input)
        {
            var message = MessageHelper.Message.Success;
            var errorMsg = _fileValidationService.ValidateFile(input);
            if (errorMsg != string.Empty)
            {
                message = MessageHelper.Message.FailToUploadFile;
                return new UploadResponseDto
                {
                    Code = MessageHelper.GetCode(message),
                    Msg = MessageHelper.GetMessage(message),
                    Description = errorMsg
                };
            }

            try
            {
                var fileInfo = await _fileValidationService.SaveFileInfoAsync(input);
                var result = _fileValidationService.RecordTransactionToBlockChain(fileInfo);
                if (input.file_hash != fileInfo.FileHash)
                {
                    message = MessageHelper.Message.FailToUploadFile;
                    return new UploadResponseDto
                    {
                        Code = MessageHelper.GetCode(message),
                        Msg = MessageHelper.GetMessage(message),
                        Description = $"文件Hash不匹配，实际上是{fileInfo.FileHash}"
                    };
                }

                return new UploadResponseDto
                {
                    Code = MessageHelper.GetCode(message),
                    Msg = MessageHelper.GetMessage(message),
                    Description =
                        $"交易Id：{result.TransactionId} 交易状态：{result.Status} 交易打包区块高度：{result.BlockNumber}",
                    FileId = fileInfo.FileId,
                    FileHash = fileInfo.FileHash,
                    TransactionId = result.TransactionId
                };
            }
            catch (Exception e)
            {
                message = MessageHelper.Message.FailToUploadFile;
                return new UploadResponseDto
                {
                    Code = MessageHelper.GetCode(message),
                    Msg = MessageHelper.GetMessage(message),
                    Description = $"文件上传失败：{e.Message}"
                };
            }
        }

        [HttpPost("record_json")]
        public ResponseDto RecordJsonAsync(RecordJsonInput input)
        {
            var nodeManager = new NodeManager(_configOptions.BlockChainEndpoint);
            var txId = nodeManager.SendTransaction(_configOptions.AccountAddress, _configOptions.RuralContractAddress,
                "RecordJsonMessage", new JsonMessage
                {
                    Key = input.Key,
                    Message = input.JsonMessage
                });
            var result = nodeManager.CheckTransactionResult(txId);
            var message = MessageHelper.Message.Success;
            return new ResponseDto
            {
                Code = MessageHelper.GetCode(message),
                Msg = MessageHelper.GetMessage(message),
                Description =
                    $"交易Id：{result.TransactionId} 交易状态：{result.Status} 交易打包区块高度：{result.BlockNumber}"
            };
        }

        [HttpGet("get_json")]
        public async Task<ResponseDto> GetJsonAsync(string input)
        {
            var nodeManager = new NodeManager(_configOptions.BlockChainEndpoint);
            var tx = nodeManager.GenerateRawTransaction(_configOptions.AccountAddress,
                _configOptions.RuralContractAddress,
                "GetJsonMessage", new StringValue {Value = input});
            var result = await nodeManager.ApiClient.ExecuteTransactionAsync(new ExecuteTransactionDto
            {
                RawTransaction = tx
            });
            var message = MessageHelper.Message.Success;
            var json = new StringValue();
            json.MergeFrom(ByteString.CopyFrom(ByteArrayHelper.HexStringToByteArray(result)));
            return new ResponseDto
            {
                Code = MessageHelper.GetCode(message),
                Msg = MessageHelper.GetMessage(message),
                Result = json.Value,
                Description = $"Key为{input}的链上数据为{json.Value}"
            };
        }
    }
}