using System;
using System.ComponentModel.DataAnnotations;

namespace MR.Models
{

    /// <summary>
    /// 内容摘要: 推广来源 mr_utm_sources 实体类
    /// </summary>
    public class UtmSources
    {
        /// <summary>
        /// 推广来源
        /// </summary>
        [Key]
        public int  source_id { get; set; }
        /// <summary>
        /// 来源名称
        /// </summary>
        public string?  source_name { get; set; }
        /// <summary>
        /// 是否显示
        /// </summary>
        public bool  is_show { get; set; }

    }
}
