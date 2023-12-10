using System;
using System.ComponentModel.DataAnnotations;

namespace MR.Models
{

    /// <summary>
    /// 内容摘要: 活动报名 mr_event_enroll 实体类
    /// </summary>
    public class EventEnroll
    {
        /// <summary>
        /// 活动报名
        /// </summary>
        [Key]
        public long  enroll_id { get; set; }
        /// <summary>
        /// 活动
        /// </summary>
        public long  event_id { get; set; }
        /// <summary>
        /// 会员
        /// </summary>
        public long  member_id { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string?  member_name { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string?  mobile_phone { get; set; }
        /// <summary>
        /// 报名人数
        /// </summary>
        public int  enroll_people_num { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string?  enroll_remark { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        [DataType(DataType.DateTime)]
        public DateTime  created_at { get; set; }

    }
}
