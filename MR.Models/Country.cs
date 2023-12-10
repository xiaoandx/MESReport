using System;
using System.ComponentModel.DataAnnotations;

namespace MR.Models
{

    /// <summary>
    /// 内容摘要: 国家 mr_country 实体类n
    /// </summary>
    public class Country
    {
        /// <summary>
        /// 国家
        /// </summary>
        [Key]
        public long  country_id { get; set; }
        /// <summary>
        /// 父
        /// </summary>
        public long  parent_id { get; set; }
        /// <summary>
        /// 路径
        /// </summary>
        public string?  country_path { get; set; }
        /// <summary>
        /// 层级
        /// </summary>
        public int  area_deep { get; set; }
        /// <summary>
        /// 中文名称
        /// </summary>
        public string?  area_name { get; set; }
        /// <summary>
        /// 英文名称
        /// </summary>
        public string?  name_en { get; set; }
        /// <summary>
        /// 俄文名称
        /// </summary>
        public string?  name_ru { get; set; }
        /// <summary>
        /// 中文拼音
        /// </summary>
        public string?  name_pinyin { get; set; }
        /// <summary>
        /// 代码
        /// </summary>
        public string?  country_code { get; set; }

    }
}
