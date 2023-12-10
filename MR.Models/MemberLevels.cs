using System;
using System.ComponentModel.DataAnnotations;

namespace MR.Models
{

    /// <summary>
    /// 内容摘要: 会员等级 mr_member_levels 实体类
    /// </summary>
    public class MemberLevels
    {
        /// <summary>
        /// 会员等级
        /// </summary>
        [Key]
        public int  level_id { get; set; }
        /// <summary>
        /// 会员等级名称
        /// </summary>
        public string?  level_name { get; set; }
        /// <summary>
        /// ICON
        /// </summary>
        public string?  level_icon { get; set; }
        /// <summary>
        /// 享受折扣
        /// </summary>
        public int  off_rate { get; set; }
        /// <summary>
        /// 达标积分数量
        /// </summary>
        public int  max_point { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string?  level_remark { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        [DataType(DataType.DateTime)]
        public DateTime  created_at { get; set; }

    }
}
