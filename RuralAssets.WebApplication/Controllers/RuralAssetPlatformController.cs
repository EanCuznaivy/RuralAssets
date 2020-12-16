using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
        private readonly ConfigOptions _configOptions;

        public RuralAssetPlatformController(ILogger<RuralAssetPlatformController> logger,
            IValidationService validationService, IOptionsSnapshot<ConfigOptions> configOptions,
            IChangeStatusService changeStatusService)
        {
            _logger = logger;
            _validationService = validationService;
            _changeStatusService = changeStatusService;
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
                    AssetId = CommonHelper.ParseToInt(dataReader, 3),
                    BCZJE = CommonHelper.ParseToDouble(dataReader, 34)
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

            if (input.AssetType != 1 && input.AssetType != 2)
            {
                message = MessageHelper.Message.ParameterMissed;
                return new ListResponseDto
                {
                    Code = MessageHelper.GetCode(message),
                    Msg = MessageHelper.GetMessage(message)
                };
            }

            if (input.PageNo < 0 || input.PageSize < 0)
            {
                message = MessageHelper.Message.ParameterTypeNotMatch;
                return new ListResponseDto
                {
                    Code = MessageHelper.GetCode(message),
                    Msg = MessageHelper.GetMessage(message),
                    Description = "分页数据不可为负数"
                };
            }

            // TODO: Type check.

            var response = new ListResponseDto
            {
                Code = MessageHelper.GetCode(message),
                Msg = MessageHelper.GetMessage(message),
                List = new List<AssetDto>()
            };
            var sql = SqlStatementHelper.GetListSql(input.Name, input.IdCard, input.AssetId, input.BFZT, input.LSX,
                input.LSXZ, input.LSC, input.PageNo, input.PageSize == 0 ? 100 : input.PageSize);
            var dataReader =
                await MySqlHelper.ExecuteReaderAsync(_configOptions.RuralAssetsConnectString, sql);
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
                XMMC = dataReader[43]?.ToString() ?? string.Empty,
                XMLX = dataReader[44]?.ToString() ?? string.Empty,
                SZZW = dataReader[45]?.ToString() ?? string.Empty,
                LSX = dataReader[46]?.ToString() ?? string.Empty,
                LSXZ = dataReader[47]?.ToString() ?? string.Empty,
                LSC = dataReader[48]?.ToString() ?? string.Empty,
                SFXX = dataReader[49]?.ToString() ?? string.Empty,
            };
        }

        [HttpPost("upload")]
        public async Task<ResponseDto> UploadAsync([FromForm] UploadInput input)
        {
            var file = input.LoanFile;
            if (file == null)
            {
                return new ResponseDto
                {
                    Msg = "文件为空"
                };
            }

            var path = Path.Combine(CommonHelper.GetDefaultDataDir(), file.FileName);
            var stream = file.OpenReadStream();
            var bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            stream.Seek(0, SeekOrigin.Begin);
            var fs = new FileStream(path, FileMode.Create);
            var bw = new BinaryWriter(fs);
            bw.Write(bytes);
            bw.Close();
            fs.Close();
            return new ResponseDto();
        }
    }
}