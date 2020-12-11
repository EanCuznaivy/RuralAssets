using System.Text.Json.Serialization;

namespace RuralAssets.WebApplication
{
    public class ListInput
    {
        [JsonPropertyName("name")] public string Name { get; set; }

        [JsonPropertyName("idcard")] public string IdCard { get; set; }

        [JsonPropertyName("asset_type")] public int AssetType { get; set; }

        [JsonPropertyName("pageno")] public int PageNo { get; set; }

        [JsonPropertyName("pagesize")] public int PageSize { get; set; }

        /// <summary>
        /// 资产Id
        /// </summary>
        [JsonPropertyName("asset_id")]
        public int AssetId { get; set; }

        /// <summary>
        /// 拨付状态
        /// 1 - 拨付中
        /// 2 - 拨付成功
        /// 3 - 拨付失败
        /// 4 - 未发放
        /// 5 - 撤销成功
        /// </summary>
        [JsonPropertyName("bfzt")]
        public double BFZT { get; set; }

        /// <summary>
        /// 隶属县编码
        /// </summary>
        [JsonPropertyName("lsx")]
        public string LSX { get; set; }

        /// <summary>
        /// 隶属乡镇编码
        /// </summary>
        [JsonPropertyName("lsxz")]
        public string LSXZ { get; set; }

        /// <summary>
        /// 隶属村编码
        /// </summary>
        [JsonPropertyName("lsc")]
        public string LSC { get; set; }
    }
}