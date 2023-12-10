using System;
using System.ComponentModel.DataAnnotations;

namespace MR.Models
{

    /// <summary>
    /// 内容摘要: 积分记录 mr_point_records 实体类
    /// </summary>
    public class PointRecords
    {
        /// <summary>
        /// 积分记录
        /// </summary>
        [Key]
        public long  record_id { get; set; }
        /// <summary>
        /// 会员
        /// </summary>
        public long  member_id { get; set; }
        /// <summary>
        /// 积分类型
        /// </summary>
        public int  point_id { get; set; }
        /// <summary>
        /// 积分数量
        /// </summary>
        public int  point_num { get; set; }
        /// <summary>
        /// 积分说明
        /// </summary>
        public string?  point_action { get; set; }
        /// <summary>
        /// TOKEN校验
        /// </summary>
        public string?  point_token { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string?  point_remark { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        [DataType(DataType.DateTime)]
        public DateTime  created_at { get; set; }

    }
}
