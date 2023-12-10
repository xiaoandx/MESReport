using System;
using System.ComponentModel.DataAnnotations;

namespace MR.Models
{

    /// <summary>
    /// 内容摘要: 审核状态 mr_review_status 实体类
    /// </summary>
    public class ReviewStatus
    {
        /// <summary>
        /// 审核状态
        /// </summary>
        [Key]
        public int  status_id { get; set; }
        /// <summary>
        /// 审核状态名称
        /// </summary>
        public string?  status_name { get; set; }

    }
}
