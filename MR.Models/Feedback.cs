using System;
using System.ComponentModel.DataAnnotations;

namespace MR.Models
{

    /// <summary>
    /// 内容摘要: 意见反馈 mr_feedback 实体类
    /// </summary>
    public class Feedback
    {
        /// <summary>
        /// 意见反馈
        /// </summary>
        [Key]
        public long  feedback_id { get; set; }
        /// <summary>
        /// 意见类型
        /// </summary>
        public int  type_id { get; set; }
        /// <summary>
        /// 会员
        /// </summary>
        public long  member_id { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string?  feedback_name { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        public string?  feedback_email { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string?  feedback_phone { get; set; }
        /// <summary>
        /// 反馈内容
        /// </summary>
        public string?  feedback_content { get; set; }
        /// <summary>
        /// 客户端IP
        /// </summary>
        public string?  client_ip { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string?  feedback_remark { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        [DataType(DataType.DateTime)]
        public DateTime  created_at { get; set; }

    }
}
