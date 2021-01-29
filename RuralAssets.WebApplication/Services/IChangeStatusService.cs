using System;
using System.Linq;
using System.Threading.Tasks;
using AElf.Client.Dto;
using AElf.Contracts.Assets;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace RuralAssets.WebApplication
{
    public interface IChangeStatusService
    {
        Task<ResponseDto> ChangeStatusAsync(ChangeStatusInput input);
    }

    public class ChangeStatusService : IChangeStatusService
    {
        private readonly ILogger<ChangeStatusService> _logger;
        private readonly IValidationService _validationService;
        private readonly ConfigOptions _configOptions;

        public ChangeStatusService(ILogger<ChangeStatusService> logger, IOptionsSnapshot<ConfigOptions> configOptions,
            IValidationService validationService)
        {
            _logger = logger;
            _configOptions = configOptions.Value;
            _validationService = validationService;
        }

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
                (input.AssetType != "1" && input.AssetType != "2"))
            {
                message = MessageHelper.Message.ParameterMissed;
                return new ResponseDto
                {
                    Code = MessageHelper.GetCode(message),
                    Msg = MessageHelper.GetMessage(message)
                };
            }

            foreach (var assetInChain in input.AssetList)
            {
                if (assetInChain.Status == "1")
                {
                    var loanAmount = double.Parse(assetInChain.LoanAmount);
                    var loanRate = double.Parse(assetInChain.LoanRate);
                    if (string.IsNullOrEmpty(assetInChain.BankId) || loanAmount < 0.0001 ||
                        string.IsNullOrEmpty(assetInChain.DueDate) || loanRate < 0.0001)
                    {
                        message = MessageHelper.Message.ParameterMissed;
                        return new ResponseDto
                        {
                            Code = MessageHelper.GetCode(message),
                            Msg = MessageHelper.GetMessage(message),
                            Description = "状态为冻结时，银行标识、放款金额、到期日、贷款利率、贷款协议为必填项"
                        };
                    }

                    if (!DateTime.TryParse(assetInChain.DueDate, out _))
                    {
                        message = MessageHelper.Message.ParameterTypeNotMatch;
                        return new ResponseDto
                        {
                            Code = MessageHelper.GetCode(message),
                            Msg = MessageHelper.GetMessage(message),
                            Description = "到期日转换失败"
                        };
                    }

                    if (!int.TryParse(assetInChain.Status, out _))
                    {
                        message = MessageHelper.Message.ParameterTypeNotMatch;
                        return new ResponseDto
                        {
                            Code = MessageHelper.GetCode(message),
                            Msg = MessageHelper.GetMessage(message),
                            Description = "资产状态不正确，1：冻结，2：逾期，3：正常"
                        };
                    }
                }
            }

            var result = RecordTransactionToBlockchain(input);
            var success = await UpdateStatusInTransactionAsync(input, result.TransactionId);
            return new ResponseDto
            {
                Code = MessageHelper.GetCode(MessageHelper.Message.Success),
                Msg = MessageHelper.GetMessage(MessageHelper.Message.Success),
                Result = success ? "1" : "0",
                Description =
                    $"{(success ? "成功" : "失败")} 交易Id：{result.TransactionId} 交易状态：{result.Status} 交易打包区块高度：{result.BlockNumber}"
            };
        }

        private async Task<bool> UpdateStatusInTransactionAsync(ChangeStatusInput input, string transactionId)
        {
            await using var conn = new MySqlConnection(_configOptions.RuralAssetsConnectString);
            conn.Open();
            var isSuccess = true;
            foreach (var assetInChain in input.AssetList)
            {
                await using var transaction = conn.BeginTransaction();
                try
                {
                    var cmd = conn.CreateCommand();
                    cmd.Transaction = transaction;
                    var changeStatusSql = input.AssetType == "1"?
                        SqlStatementHelper.GetChangeStatusSql(input.Name, input.IdCard,
                            assetInChain.AssetId,
                            assetInChain.Status):
                        SqlStatementHelper.GetChangeStatusForConstructionSql(input.Name, input.IdCard,
                            assetInChain.AssetId,
                            assetInChain.Status);
                    _logger.LogInformation(changeStatusSql);
                    cmd.CommandText = changeStatusSql;
                    await cmd.ExecuteNonQueryAsync();

                    var dueDate = assetInChain.DueDate ?? string.Empty;
                    var insertToEntityTdbcLoanSql = SqlStatementHelper.GetInsertToEntityTdbcLoanSql(input.Name,
                        input.IdCard, input.AssetType, assetInChain.AssetId, assetInChain.Status, assetInChain.LoanId,
                        assetInChain.BankId, assetInChain.LoanAmount, dueDate, assetInChain.LoanRate, transactionId);
                    _logger.LogInformation(insertToEntityTdbcLoanSql);
                    cmd.CommandText = insertToEntityTdbcLoanSql;
                    await cmd.ExecuteNonQueryAsync();

                    foreach (var loanFile in assetInChain.LoanFiles)
                    {
                        var insertFileInfoSql = SqlStatementHelper.GetInsertFileInfoSql(input.Name, input.IdCard,
                            input.AssetType, assetInChain.AssetId, loanFile.FileId, loanFile.FileType,
                            loanFile.FileHash,
                            loanFile.TransactionId);
                        _logger.LogInformation(insertFileInfoSql);
                        cmd.CommandText = insertFileInfoSql;
                        await cmd.ExecuteNonQueryAsync();
                    }

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    transaction.Rollback();
                    isSuccess = false;
                }
            }

            return isSuccess;
        }

        private TransactionResultDto RecordTransactionToBlockchain(ChangeStatusInput input)
        {
            var nodeManager = new NodeManager(_configOptions.BlockChainEndpoint);
            var assetInfo = new AssetInfo
            {
                Name = input.Name,
                IdCard = input.IdCard,
                AssetType = int.Parse(input.AssetType),
                AssetList =
                {
                    input.AssetList.Select(a =>
                    {
                        var asset = new Asset
                        {
                            AssetId = int.Parse(a.AssetId),
                            Status = a.Status,
                            BankId = a.BankId ?? string.Empty,
                            LoanAmount = CommonHelper.DoubleToLong(double.Parse(a.LoanAmount)),
                            LoanAgreementHash = string.Empty,
                            DueDate = a.DueDate ?? string.Empty,
                            LoanRate = CommonHelper.DoubleToLong(double.Parse(a.LoanRate))
                        };
                        return asset;
                    })
                }
            };
            var txId = nodeManager.SendTransaction(_configOptions.AccountAddress, _configOptions.RuralContractAddress,
                "RecordJsonMessage", new JsonMessage
                {
                    Key = assetInfo.IdCard,
                    Message = assetInfo.ToString()
                });
            return nodeManager.CheckTransactionResult(txId);
        }
    }
}