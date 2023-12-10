using System;
using System.ComponentModel.DataAnnotations;

namespace MR.Models
{

    /// <summary>
    /// 内容摘要: 消息 mr_messages 实体类
    /// </summary>
    public class Messages
    {
        /// <summary>
        /// 消息
        /// </summary>
        [Key]
        public long  message_id { get; set; }
        /// <summary>
        /// 对象
        /// </summary>
        public long  object_id { get; set; }
        /// <summary>
        /// 对象类型
        /// </summary>
        public int  object_type_id { get; set; }
        /// <summary>
        /// 消息类别
        /// </summary>
        public int  type_id { get; set; }
        /// <summary>
        /// 会员
        /// </summary>
        public long  member_id { get; set; }
        /// <summary>
        /// 接收消息会员
        /// </summary>
        public long  to_member_id { get; set; }
        /// <summary>
        /// 消息标题
        /// </summary>
        public string?  message_title { get; set; }
        /// <summary>
        /// 消息内容
        /// </summary>
        public string?  message_content { get; set; }
        /// <summary>
        /// 图片路径
        /// </summary>
        public string?  message_img { get; set; }
        /// <summary>
        /// 是否已读
        /// </summary>
        public bool  is_read { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string?  message_remark { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        [DataType(DataType.DateTime)]
        public DateTime  created_at { get; set; }

    }
}
