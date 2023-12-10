using System;
using System.ComponentModel.DataAnnotations;

namespace MR.Models
{

    /// <summary>
    /// 内容摘要: 审核类型 mr_review_types 实体类
    /// </summary>
    public class ReviewTypes
    {
        /// <summary>
        /// 审核类型
        /// </summary>
        [Key]
        public int  type_id { get; set; }
        /// <summary>
        /// 审核类型名称
        /// </summary>
        public string?  type_name { get; set; }

    }
}
