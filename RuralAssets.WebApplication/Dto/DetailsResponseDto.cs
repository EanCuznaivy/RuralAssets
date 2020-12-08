using System.Text.Json.Serialization;

namespace RuralAssets.WebApplication
{
    public class DetailsResponseDto
    {
        [JsonPropertyName("code")]
        public string Code { get; set; }
        [JsonPropertyName("msg")]
        public string Msg { get; set; }
        [JsonPropertyName("asset_id")]
        public int AssetId { get; set; }

        /// <summary>
        /// 数字证书唯一标识
        /// </summary>
        [JsonPropertyName("block_id")]
        public string BlockId { get; set; }

        [JsonPropertyName("id")]
        public double Id { get; set; }

        /// <summary>
        /// 姓名（收款人）
        /// </summary>
        [JsonPropertyName("skr")]
        public string SKR { get; set; }

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
        /// 银行类型
        /// </summary>
        [JsonPropertyName("yhlx")]
        public string YHLX { get; set; }

        /// <summary>
        /// 开户所在银行
        /// </summary>
        [JsonPropertyName("khszyh")]
        public string KHSZYH { get; set; }

        /// <summary>
        /// 开户银行
        /// </summary>
        [JsonPropertyName("khyh")]
        public string KHYH { get; set; }

        /// <summary>
        /// 联行号
        /// </summary>
        [JsonPropertyName("lhh")]
        public string LHH { get; set; }

        /// <summary>
        /// 银行账号
        /// </summary>
        [JsonPropertyName("yhzh")]
        public string YHZH { get; set; }

        /// <summary>
        /// 是否跨行
        /// 同行/跨行
        /// </summary>
        [JsonPropertyName("sfkh")]
        public string SFKH { get; set; }

        /// <summary>
        /// 账号性质
        /// 对私账号/对公账号
        /// </summary>
        [JsonPropertyName("zhxz")]
        public string ZHXZ { get; set; }

        /// <summary>
        /// 土地方位
        /// </summary>
        [JsonPropertyName("ntfw")]
        public string NTFW { get; set; }

        /// <summary>
        /// 东至
        /// </summary>
        [JsonPropertyName("dz")]
        public string DZ { get; set; }

        /// <summary>
        /// 西至
        /// </summary>
        [JsonPropertyName("xz")]
        public string XZ { get; set; }

        /// <summary>
        /// 南至
        /// </summary>
        [JsonPropertyName("nz")]
        public string NZ { get; set; }

        /// <summary>
        /// 北至
        /// </summary>
        [JsonPropertyName("bf")]
        public string BF { get; set; }

        /// <summary>
        /// 总测绘面积（亩）
        /// </summary>
        [JsonPropertyName("zchmj")]
        public double ZCHMJ { get; set; }

        /// <summary>
        /// 大田作物
        /// </summary>
        [JsonPropertyName("dtzw")]
        public double DTZW { get; set; }

        /// <summary>
        /// 蔬菜
        /// </summary>
        [JsonPropertyName("sc")]
        public double SC { get; set; }

        /// <summary>
        /// 经济作物
        /// </summary>
        [JsonPropertyName("jjzw")]
        public double JJZW { get; set; }

        /// <summary>
        /// 树木
        /// </summary>
        [JsonPropertyName("sm")]
        public double SM { get; set; }

        /// <summary>
        /// 树木株数
        /// </summary>
        [JsonPropertyName("smz")]
        public int SMZ { get; set; }

        /// <summary>
        /// 大棚作物面积
        /// </summary>
        [JsonPropertyName("dpzw")]
        public double DPZW { get; set; }

        /// <summary>
        /// 其他
        /// </summary>
        [JsonPropertyName("qt")]
        public string QT { get; set; }

        /// <summary>
        /// 土地收益金
        /// </summary>
        [JsonPropertyName("tdsyj")]
        public double TDSYJ { get; set; }

        /// <summary>
        /// 大田作物金额
        /// </summary>
        [JsonPropertyName("dtzwje")]
        public double DTZWJE { get; set; }

        /// <summary>
        /// 蔬菜金额
        /// </summary>
        [JsonPropertyName("scje")]
        public double SCJE { get; set; }

        /// <summary>
        /// 其他经济作物金额
        /// </summary>
        [JsonPropertyName("qtzwje")]
        public double QTZWJE { get; set; }

        /// <summary>
        /// 大棚作物金额
        /// </summary>
        [JsonPropertyName("dpzwje")]
        public double DPZWJE { get; set; }

        /// <summary>
        /// 其他附着物金额
        /// </summary>
        [JsonPropertyName("qtfzwje")]
        public double QTFZWJE { get; set; }

        /// <summary>
        /// 补偿总金额
        /// </summary>
        [JsonPropertyName("bczje")]
        public double BCZJE { get; set; }

        /// <summary>
        /// 总金额大写
        /// </summary>
        [JsonPropertyName("zjedx")]
        public string ZJEDX { get; set; }

        /// <summary>
        /// 附件
        /// </summary>
        [JsonPropertyName("fj")]
        public string FJ { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [JsonPropertyName("bz")]
        public string BZ { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [JsonPropertyName("cjsj")]
        public string CJSJ { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        [JsonPropertyName("cjr")]
        public string CJR { get; set; }

        /// <summary>
        /// 拨付状态
        /// 拨付中/拨付成功/拨付失败/未发放/撤销成功
        /// </summary>
        [JsonPropertyName("bfzt")]
        public string BFZT { get; set; }

        /// <summary>
        /// 是否删除
        /// 1：已删除；2：未删除
        /// </summary>
        [JsonPropertyName("sfsc")]
        public double SFSC { get; set; }

        /// <summary>
        /// 合同附件
        /// </summary>
        [JsonPropertyName("htfj")]
        public string HTFJ { get; set; }

        /// <summary>
        /// 土地补偿明细批次
        /// </summary>
        [JsonPropertyName("tdbcmxpc")]
        public double TDBCMXPC { get; set; }

        /// <summary>
        /// 银行付款编号
        /// </summary>
        [JsonPropertyName("dealno")]
        public string Dealno { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        [JsonPropertyName("xmmc")]
        public string XMMC { get; set; }
        
        /// <summary>
        /// 项目类型
        /// </summary>
        [JsonPropertyName("xmlx")]
        public string XMLX { get; set; }

        /// <summary>
        /// 数字指纹
        /// </summary>
        [JsonPropertyName("szzw")]
        public string SZZW { get; set; }

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

        /// <summary>
        /// 是否村委会
        /// 村委会/村民
        /// </summary>
        [JsonPropertyName("sfxx")]
        public string SFXX { get; set; }
    }
}