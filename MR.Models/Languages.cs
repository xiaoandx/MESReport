using System;
using System.ComponentModel.DataAnnotations;

namespace MR.Models
{

    /// <summary>
    /// 内容摘要: 多语言 mr_languages 实体类
    /// </summary>
    public class Languages
    {
        /// <summary>
        /// 多语言
        /// </summary>
        [Key]
        public int  lang_id { get; set; }
        /// <summary>
        /// 多语言
        /// </summary>
        public string?  lang_name { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        public string?  lang_code { get; set; }
        /// <summary>
        /// 国旗路径
        /// </summary>
        public string?  lang_flag { get; set; }
        /// <summary>
        /// 交易币种
        /// </summary>
        public string?  lang_currency { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string?  lang_remark { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        [DataType(DataType.DateTime)]
        public DateTime  created_at { get; set; }

    }
}
