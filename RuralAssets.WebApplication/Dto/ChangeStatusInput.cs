using System.Collections.Generic;
using System.Text.Json.Serialization;
using AElf.Contracts.Assets;

namespace RuralAssets.WebApplication
{
    public class ChangeStatusInput
    {
        [JsonPropertyName("name")] public string Name { get; set; }
        [JsonPropertyName("idcard")] public string IdCard { get; set; }
        [JsonPropertyName("asset_type")] public int AssetType { get; set; }
        [JsonPropertyName("asset_list")] public List<AssetInChain> AssetList { get; set; }
    }

    public class AssetInChain
    {
        [JsonPropertyName("asset_id")] public int AssetId { get; set; }

        /// <summary>
        /// 状态
        /// 1 - 冻结
        /// 2 - 逾期
        /// 3 - 正常
        /// </summary>
        [JsonPropertyName("status")]
        public string Status { get; set; }

        /// <summary>
        /// 银行标识
        /// </summary>
        [JsonPropertyName("bank_id")]
        public string BankId { get; set; }
        
        /// <summary>
        /// 银行贷款编号
        /// </summary>
        [JsonPropertyName("loan_id")]
        public string LoanId { get; set; }

        /// <summary>
        /// 放款金额
        /// </summary>
        [JsonPropertyName("loan_amount")]
        public double LoanAmount { get; set; }

        /// <summary>
        /// 到期日
        /// </summary>
        [JsonPropertyName("due_date")]
        public string DueDate { get; set; }

        /// <summary>
        /// 贷款利率
        /// </summary>
        [JsonPropertyName("loan_rate")]
        public double LoanRate { get; set; }
        
        /// <summary>
        /// 贷款相关文件列表
        /// </summary>
        [JsonPropertyName("loan_file")]
        public List<LoanFile> LoanFiles { get; set; }
        
        
    }

    public class LoanFile
    {
        /// <summary>
        /// file_type
        /// </summary>
        [JsonPropertyName("file_type")]
        public string FileType { get; set; }
        
        /// <summary>
        /// 贷款文件类型
        /// </summary>
        [JsonPropertyName("file_id")] 
        public string FileId { get; set; }
        
        /// <summary>
        /// 文件hash值
        /// </summary>
        [JsonPropertyName("file_hash")] 
        public string FileHash { get; set; }
        
        /// <summary>
        /// 文件上传后的区块链交易ID
        /// </summary>
        [JsonPropertyName("transaction_id")] 
        public string TransactionId { get; set; }
    }
}