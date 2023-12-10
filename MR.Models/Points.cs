using System;
using System.ComponentModel.DataAnnotations;

namespace MR.Models
{

    /// <summary>
    /// 内容摘要: 积分 mr_points 实体类
    /// </summary>
    public class Points
    {
        /// <summary>
        /// 积分
        /// </summary>
        [Key]
        public long  point_id { get; set; }
        /// <summary>
        /// 积分名称
        /// </summary>
        public string?  point_name { get; set; }
        /// <summary>
        /// 积分数量
        /// </summary>
        public int  point_num { get; set; }
        /// <summary>
        /// 是否循环
        /// </summary>
        public bool  is_loop { get; set; }
        /// <summary>
        /// 每日最大累加上限积分
        /// </summary>
        public int  max_point { get; set; }
        /// <summary>
        /// 积分备注
        /// </summary>
        public string?  point_remark { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        [DataType(DataType.DateTime)]
        public DateTime  created_at { get; set; }

    }
}
