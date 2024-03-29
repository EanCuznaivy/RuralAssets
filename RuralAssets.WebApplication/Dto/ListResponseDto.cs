using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RuralAssets.WebApplication
{
    public class ListResponseDto
    {
        public string Code { get; set; }
        public string Msg { get; set; }
        public string Result { get; set; }
        public string Description { get; set; }
        public List<AssetDto> List { get; set; }
    }

    public class AssetDto
    {
        /// <summary>
        /// 资产id
        /// </summary>
        [JsonPropertyName("asset_id")]
        public int AssetId { get; set; }

        /// <summary>
        /// 批次
        /// </summary>
        [JsonPropertyName("pc")]
        public string PC { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>
        [JsonPropertyName("pch")]
        public string PCH { get; set; }

        /// <summary>
        /// 项目名称id
        /// </summary>
        [JsonPropertyName("xmmcid")]
        public int XMMCId { get; set; }

        /// <summary>
        /// 项目名称描述
        /// </summary>
        [JsonPropertyName("xmmcms")]
        public string XMMCMS { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// 身份证号
        /// </summary>
        [JsonPropertyName("sfzh")]
        public string SFZH { get; set; }

        /// <summary>
        /// 联系方式
        /// </summary>
        [JsonPropertyName("lxfs")]
        public string LXFS { get; set; }

        /// <summary>
        /// 开户银行
        /// </summary>
        [JsonPropertyName("khyh")]
        public string KHYH { get; set; }

        /// <summary>
        /// 账号性质Id
        /// </summary>
        [JsonPropertyName("zhxzid")]
        public int ZHXZId { get; set; }

        /// <summary>
        /// 账号性质描述
        /// </summary>
        [JsonPropertyName("zhxzms")]
        public string ZHXZMS { get; set; }

        /// <summary>
        /// 总测绘面积
        /// </summary>
        [JsonPropertyName("zchmj")]
        public string ZCHMJ { get; set; }

        /// <summary>
        /// 补偿总金额
        /// </summary>
        [JsonPropertyName("bczje")]
        public string BCZJE { get; set; }

        /// <summary>
        /// 拨付状态（描述）
        /// </summary>
        [JsonPropertyName("bfztms")]
        public string BFZTMS { get; set; }

        /// <summary>
        /// 隶属县
        /// </summary>
        [JsonPropertyName("lsx")]
        public string LSX { get; set; }

        /// <summary>
        /// 隶属乡镇
        /// </summary>
        [JsonPropertyName("lsxz")]
        public string LSXZ { get; set; }

        /// <summary>
        /// 隶属村
        /// </summary>
        [JsonPropertyName("lsc")]
        public string LSC { get; set; }
    }
}