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
        public string LXFS { get; set; }

        /// <summary>
        /// 银行类型
        /// </summary>
        public string YHLX { get; set; }

        /// <summary>
        /// 开户所在银行
        /// </summary>
        public string KHSZYH { get; set; }

        /// <summary>
        /// 开户银行
        /// </summary>
        public string KHYH { get; set; }

        /// <summary>
        /// 联行号
        /// </summary>
        public string LHH { get; set; }

        /// <summary>
        /// 银行账号
        /// </summary>
        public string YHZH { get; set; }

        /// <summary>
        /// 是否跨行
        /// 同行/跨行
        /// </summary>
        public string SFKH { get; set; }

        /// <summary>
        /// 账号性质
        /// 对私账号/对公账号
        /// </summary>
        public string ZHXZ { get; set; }

        /// <summary>
        /// 土地方位
        /// </summary>
        public string NTFW { get; set; }

        /// <summary>
        /// 东至
        /// </summary>
        public string DZ { get; set; }

        /// <summary>
        /// 西至
        /// </summary>
        public string XZ { get; set; }

        /// <summary>
        /// 南至
        /// </summary>
        public string NZ { get; set; }

        /// <summary>
        /// 北至
        /// </summary>
        public string BF { get; set; }

        /// <summary>
        /// 总测绘面积（亩）
        /// </summary>
        public double ZCHMJ { get; set; }

        /// <summary>
        /// 大田作物
        /// </summary>
        public double DTZW { get; set; }

        /// <summary>
        /// 蔬菜
        /// </summary>
        public double SC { get; set; }

        /// <summary>
        /// 经济作物
        /// </summary>
        public double JJZW { get; set; }

        /// <summary>
        /// 树木
        /// </summary>
        public double SM { get; set; }

        /// <summary>
        /// 树木株数
        /// </summary>
        public int SMZ { get; set; }

        /// <summary>
        /// 大棚作物面积
        /// </summary>
        public double DPZW { get; set; }

        /// <summary>
        /// 其他
        /// </summary>
        public string QT { get; set; }

        /// <summary>
        /// 土地收益金
        /// </summary>
        public double TDSYJ { get; set; }

        /// <summary>
        /// 大田作物金额
        /// </summary>
        public double DTZWJE { get; set; }

        /// <summary>
        /// 蔬菜金额
        /// </summary>
        public double SCJE { get; set; }

        /// <summary>
        /// 其他经济作物金额
        /// </summary>
        public double QTZWJE { get; set; }

        /// <summary>
        /// 大棚作物金额
        /// </summary>
        public double DPZWJE { get; set; }

        /// <summary>
        /// 其他附着物金额
        /// </summary>
        public double QTFZWJE { get; set; }

        /// <summary>
        /// 补偿总金额
        /// </summary>
        public double BCZJE { get; set; }

        /// <summary>
        /// 总金额大写
        /// </summary>
        public string ZJEDX { get; set; }

        /// <summary>
        /// 附件
        /// </summary>
        public string FJ { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string BZ { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public string CJSJ { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CJR { get; set; }

        /// <summary>
        /// 拨付状态
        /// 拨付中/拨付成功/拨付失败/未发放/撤销成功
        /// </summary>
        public string BFZT { get; set; }

        /// <summary>
        /// 是否删除
        /// 1：已删除；2：未删除
        /// </summary>
        public double SFSC { get; set; }

        /// <summary>
        /// 合同附件
        /// </summary>
        public string HTFJ { get; set; }

        /// <summary>
        /// 土地补偿明细批次
        /// </summary>
        public double TDBCMXPC { get; set; }

        /// <summary>
        /// 银行付款编号
        /// </summary>
        public string Dealno { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string XMMC { get; set; }
        
        /// <summary>
        /// 项目类型
        /// </summary>
        public string XMLX { get; set; }

        /// <summary>
        /// 数字指纹
        /// </summary>
        public string SZZW { get; set; }

        /// <summary>
        /// 隶属县
        /// </summary>
        public string LSX { get; set; }

        /// <summary>
        /// 隶属乡镇
        /// </summary>
        public string LSXZ { get; set; }

        /// <summary>
        /// 隶属村
        /// </summary>
        public string LSC { get; set; }

        /// <summary>
        /// 是否村委会
        /// 村委会/村民
        /// </summary>
        public string SFXX { get; set; }
    }
}