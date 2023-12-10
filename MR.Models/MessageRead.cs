using System;
using System.ComponentModel.DataAnnotations;

namespace MR.Models
{

    /// <summary>
    /// 内容摘要: 阅读记录表 mr_message_read 实体类
    /// </summary>
    public class MessageRead
    {
        /// <summary>
        /// 阅读记录表
        /// </summary>
        [Key]
        public long  read_id { get; set; }
        /// <summary>
        /// 会员
        /// </summary>
        public long  member_id { get; set; }
        /// <summary>
        /// 消息
        /// </summary>
        public long  message_id { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string?  read_remark { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        [DataType(DataType.DateTime)]
        public DateTime  created_at { get; set; }

    }
}
