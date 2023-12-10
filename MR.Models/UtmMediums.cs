using System;
using System.ComponentModel.DataAnnotations;

namespace MR.Models
{

    /// <summary>
    /// 内容摘要: 推广媒介 mr_utm_mediums 实体类
    /// </summary>
    public class UtmMediums
    {
        /// <summary>
        /// 推广媒介
        /// </summary>
        [Key]
        public int  medium_id { get; set; }
        /// <summary>
        /// 媒介名称
        /// </summary>
        public string?  medium_name { get; set; }

    }
}
