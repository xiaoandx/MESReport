using System;
using System.ComponentModel.DataAnnotations;

namespace MR.Models
{

    /// <summary>
    /// 内容摘要: 访问频道 mr_visit_channels 实体类
    /// </summary>
    public class VisitChannels
    {
        /// <summary>
        /// 访问频道
        /// </summary>
        [Key]
        public int  channel_id { get; set; }
        /// <summary>
        /// 频道名称
        /// </summary>
        public string?  channel_name { get; set; }
        /// <summary>
        /// 频道路径
        /// </summary>
        public string?  channel_path { get; set; }

    }
}
