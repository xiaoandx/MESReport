using System;
using System.ComponentModel.DataAnnotations;

namespace MR.Models
{

    /// <summary>
    /// 内容摘要: 平台 mr_platforms 实体类
    /// </summary>
    public class Platforms
    {
        /// <summary>
        /// 平台
        /// </summary>
        [Key]
        public int  platform_id { get; set; }
        /// <summary>
        /// 平台名称
        /// </summary>
        public string?  platform_name { get; set; }

    }
}
