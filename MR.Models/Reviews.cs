using System;
using System.ComponentModel.DataAnnotations;

namespace MR.Models
{

    /// <summary>
    /// 内容摘要: 审核意见 mr_reviews 实体类
    /// </summary>
    public class Reviews
    {
        /// <summary>
        /// 审核意见
        /// </summary>
        [Key]
        public long  review_id { get; set; }
        /// <summary>
        /// 对象
        /// </summary>
        public long  object_id { get; set; }
        /// <summary>
        /// 审核类型
        /// </summary>
        public int  type_id { get; set; }
        /// <summary>
        /// 审核状态
        /// </summary>
        public int  status_id { get; set; }
        /// <summary>
        /// 意见内容
        /// </summary>
        public string?  review_content { get; set; }
        /// <summary>
        /// 后台审核人
        /// </summary>
        public long  user_id { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string?  review_remark { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        [DataType(DataType.DateTime)]
        public DateTime  created_at { get; set; }

    }
}
