using System;
using System.ComponentModel.DataAnnotations;

namespace MR.Models
{

    /// <summary>
    /// 内容摘要: 活动类型 mr_event_types 实体类
    /// </summary>
    public class EventTypes
    {
        /// <summary>
        /// 活动类型
        /// </summary>
        [Key]
        public int  type_id { get; set; }
        /// <summary>
        /// 活动类型名称
        /// </summary>
        public string?  type_name { get; set; }

    }
}
