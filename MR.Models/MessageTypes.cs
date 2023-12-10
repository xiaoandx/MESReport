using System;
using System.ComponentModel.DataAnnotations;

namespace MR.Models
{

    /// <summary>
    /// 内容摘要: 消息类型 mr_message_types 实体类
    /// </summary>
    public class MessageTypes
    {
        /// <summary>
        /// 消息类型
        /// </summary>
        [Key]
        public int  type_id { get; set; }
        /// <summary>
        /// 消息类型名称
        /// </summary>
        public string?  type_name { get; set; }

    }
}
