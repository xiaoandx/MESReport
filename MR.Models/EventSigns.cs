using System;
using System.ComponentModel.DataAnnotations;

namespace MR.Models
{

    /// <summary>
    /// 内容摘要: 签到 mr_event_signs 实体类
    /// </summary>
    public class EventSigns
    {
        /// <summary>
        /// 签到
        /// </summary>
        [Key]
        public long  sign_id { get; set; }
        /// <summary>
        /// 活动
        /// </summary>
        public long  event_id { get; set; }
        /// <summary>
        /// 会员
        /// </summary>
        public long  member_id { get; set; }
        /// <summary>
        /// 签到姓名
        /// </summary>
        public string?  member_name { get; set; }
        /// <summary>
        /// 签到电话
        /// </summary>
        public string?  member_phone { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        public string?  member_email { get; set; }
        /// <summary>
        /// 签到地址
        /// </summary>
        public string?  member_address { get; set; }
        /// <summary>
        /// 签到经度
        /// </summary>
        public double  member_lng { get; set; }
        /// <summary>
        /// 签到维度
        /// </summary>
        public double  member_lat { get; set; }
        /// <summary>
        /// 签到方式
        /// </summary>
        public string?  sign_style { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string?  sign_remark { get; set; }
        /// <summary>
        /// 签到时间
        /// </summary>
        [DataType(DataType.DateTime)]
        public DateTime  created_at { get; set; }

    }
}
