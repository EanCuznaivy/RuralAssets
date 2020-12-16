using System;
using System.Linq;
using System.Threading.Tasks;
using AElf.Client.Dto;
using AElf.Contracts.Assets;
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
        private readonly IValidationService _validationService;
        private readonly ConfigOptions _configOptions;

        public ChangeStatusService(IOptionsSnapshot<ConfigOptions> configOptions, IValidationService validationService)
        {
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
                (input.AssetType != 1 && input.AssetType != 2))
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
                    if (string.IsNullOrEmpty(assetInChain.BankId) || assetInChain.LoanAmount < 0.0001 ||
                        string.IsNullOrEmpty(assetInChain.DueDate) || !assetInChain.LoanAgreementHash.Any() ||
                        assetInChain.LoanRate < 0.0001)
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

            var result = await RecordTransactionToBlockchain(input);

            var success = true;
            foreach (var assetInChain in input.AssetList)
            {
                var status = int.Parse(assetInChain.Status);
                var sql = SqlStatementHelper.GetChangeStatusSql(input.Name, input.IdCard, assetInChain.AssetId, status);
                var row = await MySqlHelper.ExecuteNonQueryAsync(_configOptions.RuralAssetsConnectString, sql);
                if (row == 0)
                {
                    success = false;
                }
            }

            return new ResponseDto
            {
                Code = MessageHelper.GetCode(MessageHelper.Message.Success),
                Msg = MessageHelper.GetMessage(MessageHelper.Message.Success),
                Result = success ? "1" : "0",
                Description =
                    $"{(success ? "成功" : "失败")} 交易Id：{result.TransactionId} 交易状态：{result.Status} 交易打包区块高度：{result.BlockNumber}"
            };
        }

        private async Task<TransactionResultDto> RecordTransactionToBlockchain(ChangeStatusInput input)
        {
            var nodeManager = new NodeManager(_configOptions.BlockChainEndpoint);
            var assetInfo = new AssetInfo
            {
                Name = input.Name,
                IdCard = input.IdCard,
                AssetType = input.AssetType,
                AssetIdList = {input.AssetList.Select(a => a.AssetId)},
                AssetList =
                {
                    input.AssetList.Select(a =>
                    {
                        var asset = new Asset
                        {
                            AssetId = a.AssetId,
                            Status = a.Status,
                            BankId = a.BankId ?? string.Empty,
                            LoanAmount = CommonHelper.DoubleToLong(a.LoanAmount),
                            LoanAgreementHash = a.LoanAgreementHash ?? string.Empty,
                            DueDate = a.DueDate ?? string.Empty,
                            LoanRate = CommonHelper.DoubleToLong(a.LoanRate)
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