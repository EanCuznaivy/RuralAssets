using System.Collections.Generic;

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
        public int AssetId { get; set; }

        /// <summary>
        /// 批次
        /// </summary>
        public string PC { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>
        public string PCH { get; set; }

        /// <summary>
        /// 项目名称id
        /// </summary>
        public int XMMCId { get; set; }

        /// <summary>
        /// 项目名称描述
        /// </summary>
        public string XMMCMS { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 身份证号
        /// </summary>
        public string SFZH { get; set; }

        /// <summary>
        /// 联系方式
        /// </summary>
        public string LXFS { get; set; }

        /// <summary>
        /// 开户银行
        /// </summary>
        public string KHYH { get; set; }

        /// <summary>
        /// 账号性质Id
        /// </summary>
        public int ZHXZId { get; set; }

        /// <summary>
        /// 账号性质描述
        /// </summary>
        public string ZHXZMS { get; set; }

        /// <summary>
        /// 总测绘面积
        /// </summary>
        public string ZCHMJ { get; set; }

        /// <summary>
        /// 补偿总金额
        /// </summary>
        public string BCZJE { get; set; }

        /// <summary>
        /// 拨付状态（描述）
        /// </summary>
        public string BFZTMS { get; set; }

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
    }
}