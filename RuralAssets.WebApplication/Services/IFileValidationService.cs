using System.IO;
using System.Text;
using System.Threading.Tasks;
using AElf;
using AElf.Client.Dto;
using AElf.Contracts.Assets;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;


namespace RuralAssets.WebApplication
{
    public interface IFileValidationService
    {
        string ValidateFile(UploadInput loanFile);
        Task<FileSavedInfo> SaveFileInfoAsync(UploadInput loanFile);
        TransactionResultDto RecordTransactionToBlockChain(FileSavedInfo fileInfo);
    }

    public class FileValidationService : IFileValidationService
    {
        private readonly ConfigOptions _configOptions;

        public FileValidationService(IOptions<ConfigOptions> configOptions)
        {
            _configOptions = configOptions.Value;
        }

        public string ValidateFile(UploadInput input)
        {
            var errorMsg = string.Empty;
            var file = input.LoanFile;
            if (file == null)
            {
                errorMsg = "文件为空";
            }
            else if (string.IsNullOrEmpty(input.FileType))
            {
                errorMsg = "文件类型";
            }
            else if (string.IsNullOrEmpty(input.LoanFile.Name))
            {
                errorMsg = "文件名为空";
            }

            return errorMsg;
        }

        public async Task<FileSavedInfo> SaveFileInfoAsync(UploadInput input)
        {
            MakeSureSaveDirExist();
            var file = input.LoanFile;
            var fileId = GenerateFileId(input.IdCard, input.LoanId, Path.GetFileName(file.FileName), input.FileType, input.AssetId,
                input.AssetType);
            var fileName = fileId.TrimStart('"').TrimEnd('"');
            var path = Path.Combine(_configOptions.FileSaveDir, fileName);
            var fileHash = await SaveAndCalculateMD5Async(path, file);
            return new FileSavedInfo
            {
                FileId = fileId,
                FileHash = fileHash
            };
        }

        private static async Task<string> SaveAndCalculateMD5Async(string path, IFormFile file){
            await using var stream = file.OpenReadStream();

            var calculator = System.Security.Cryptography.MD5.Create();
            var buffer = calculator.ComputeHash(stream);
            calculator.Clear();
            var stringBuilder = new StringBuilder();
            foreach (var t in buffer)
            {
                stringBuilder.Append(t.ToString("x2"));
            }
            var hashMD5 = stringBuilder.ToString();
            stream.Seek(0, SeekOrigin.Begin);
            await using var fs = new FileStream(path, FileMode.Create);
            await stream.CopyToAsync(fs); 
            return hashMD5;
        }

        public TransactionResultDto RecordTransactionToBlockChain(FileSavedInfo fileInfo)
        {
            var nodeManager = new NodeManager(_configOptions.BlockChainEndpoint);
            var txId = nodeManager.SendTransaction(_configOptions.AccountAddress, _configOptions.RuralContractAddress,
                "RecordJsonMessage", new JsonMessage
                {
                    Key = fileInfo.FileId,
                    Message = new StringValue
                    {
                        Value = fileInfo.FileHash
                    }.ToString()
                });
            return nodeManager.CheckTransactionResult(txId);
        }

        private void MakeSureSaveDirExist()
        {
            if (!Directory.Exists(_configOptions.FileSaveDir))
                Directory.CreateDirectory(_configOptions.FileSaveDir);
        }

        private static string GenerateFileId(string idCard, string loanId, string fileName, string fileType,
            int assetId, int assetType)
        {
            var fileInfo = new StringValue
            {
                Value = idCard + loanId + fileName + fileType + assetId + assetType
            };
            return HashHelper.ComputeFrom(fileInfo).ToString();
        }
    }
}