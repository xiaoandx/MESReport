using System;
using System.ComponentModel.DataAnnotations;

namespace MR.Models
{

    /// <summary>
    /// 内容摘要: 意见类别表 mr_feedback_types 实体类
    /// </summary>
    public class FeedbackTypes
    {
        /// <summary>
        /// 意见类别表
        /// </summary>
        [Key]
        public int  type_id { get; set; }
        /// <summary>
        /// 意见名称
        /// </summary>
        public string?  type_name { get; set; }

    }
}
