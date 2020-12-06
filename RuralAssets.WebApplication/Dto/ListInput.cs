using System.Text.Json.Serialization;

namespace RuralAssets.WebApplication
{
    public class ListInput
    {
        public string Name { get; set; }
        public string Idcard { get; set; }
        
        [JsonPropertyName("asset_type")]
        public int AssetType { get; set; }

        /// <summary>
        /// 资产Id
        /// </summary>
        public int AssetId { get; set; }

        /// <summary>
        /// 拨付状态
        /// 1 - 拨付中
        /// 2 - 拨付成功
        /// 3 - 拨付失败
        /// 4 - 未发放
        /// 5 - 撤销成功
        /// </summary>
        public double BFZT { get; set; }

        /// <summary>
        /// 隶属县编码
        /// </summary>
        public string LSX { get; set; }

        /// <summary>
        /// 隶属乡镇编码
        /// </summary>
        public string LSXZ { get; set; }

        /// <summary>
        /// 隶属村编码
        /// </summary>
        public string LSC { get; set; }
    }
}