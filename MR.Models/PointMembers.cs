using System;
using System.ComponentModel.DataAnnotations;

namespace MR.Models
{

    /// <summary>
    /// 内容摘要: 会员积分 mr_point_members 实体类
    /// </summary>
    public class PointMembers
    {
        /// <summary>
        /// 会员积分
        /// </summary>
        [Key]
        public long  point_member_id { get; set; }
        /// <summary>
        /// 会员
        /// </summary>
        public long  member_id { get; set; }
        /// <summary>
        /// 会员总积分
        /// </summary>
        public int  total_point { get; set; }
        /// <summary>
        /// 可用积分
        /// </summary>
        public int  available_point { get; set; }
        /// <summary>
        /// 冻结积分
        /// </summary>
        public int  froze_point { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        [DataType(DataType.DateTime)]
        public DateTime  created_at { get; set; }

    }
}
