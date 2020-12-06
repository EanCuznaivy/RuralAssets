using System.Collections.Generic;

namespace RuralAssets.WebApplication
{
    public class ChangeStatusInput
    {
        public string Name { get; set; }
        public string Idcard { get; set; }
        public int AssetType { get; set; }
        public List<AssetInChain> AssetList { get; set; }
    }

    public class AssetInChain
    {
        public int AssetId { get; set; }

        /// <summary>
        /// 状态
        /// 1 - 冻结
        /// 2 - 逾期
        /// 3 - 正常
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 银行标识
        /// </summary>
        public string BankId { get; set; }

        /// <summary>
        /// 放款金额
        /// </summary>
        public double LoanAmount { get; set; }

        /// <summary>
        /// 到期日
        /// </summary>
        public string DueDate { get; set; }

        /// <summary>
        /// 贷款利率
        /// </summary>
        public double LoanRate { get; set; }

        /// <summary>
        /// 贷款协议
        /// </summary>
        public byte[] LoanAgreement { get; set; }
    }
}