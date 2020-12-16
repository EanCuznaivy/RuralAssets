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
        /// 贷款协议
        /// </summary>
        [JsonPropertyName("loan_agreement_hash")]
        public string LoanAgreementHash { get; set; }
    }
}