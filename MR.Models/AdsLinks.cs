using System;
using System.ComponentModel.DataAnnotations;

namespace MR.Models
{

    /// <summary>
    /// 内容摘要: 链接类型 mr_ads_links 实体类
    /// </summary>
    public class AdsLinks
    {
        /// <summary>
        /// 链接类型
        /// </summary>
        [Key]
        public int  link_id { get; set; }
        /// <summary>
        /// 链接类型名称
        /// </summary>
        public string?  link_name { get; set; }
        /// <summary>
        /// 是否显示
        /// </summary>
        public bool  is_show { get; set; }

    }
}
