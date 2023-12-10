using System;
using System.ComponentModel.DataAnnotations;

namespace MR.Models
{

    /// <summary>
    /// 内容摘要: 活动图片 mr_events_photos 实体类
    /// </summary>
    public class EventsPhotos
    {
        /// <summary>
        /// 活动图片
        /// </summary>
        [Key]
        public long  photo_id { get; set; }
        /// <summary>
        /// 活动
        /// </summary>
        public long  event_id { get; set; }
        /// <summary>
        /// 图片路径
        /// </summary>
        public string?  photo_path { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string?  photo_remark { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        [DataType(DataType.DateTime)]
        public DateTime  created_at { get; set; }

    }
}
