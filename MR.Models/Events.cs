using System;
using System.ComponentModel.DataAnnotations;

namespace MR.Models
{

    /// <summary>
    /// 内容摘要: 活动 mr_events 实体类
    /// </summary>
    public class Events
    {
        /// <summary>
        /// 活动
        /// </summary>
        [Key]
        public long  event_id { get; set; }
        /// <summary>
        /// 多语言
        /// </summary>
        public int  lang_id { get; set; }
        /// <summary>
        /// 活动标题
        /// </summary>
        public string?  event_title { get; set; }
        /// <summary>
        /// 活动封面图片
        /// </summary>
        public string?  cover_img { get; set; }
        /// <summary>
        /// 活动详情
        /// </summary>
        public string?  event_content { get; set; }
        /// <summary>
        /// 活动类型
        /// </summary>
        public int  type_id { get; set; }
        /// <summary>
        /// 开始日期
        /// </summary>
        [DataType(DataType.DateTime)]
        public DateTime  start_at { get; set; }
        /// <summary>
        /// 结束日期
        /// </summary>
        [DataType(DataType.DateTime)]
        public DateTime  end_at { get; set; }
        /// <summary>
        /// 是否免费
        /// </summary>
        public bool  is_free { get; set; }
        /// <summary>
        /// 活动地点
        /// </summary>
        public string?  event_address { get; set; }
        /// <summary>
        /// 目标人群
        /// </summary>
        public string?  event_crowd { get; set; }
        /// <summary>
        /// 活动人数
        /// </summary>
        public int  people_num { get; set; }
        /// <summary>
        /// 是否显示
        /// </summary>
        public bool  is_show { get; set; }
        /// <summary>
        /// 访问量
        /// </summary>
        public int  visit_num { get; set; }
        /// <summary>
        /// 发布人
        /// </summary>
        public long  user_id { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string?  event_remark { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        [DataType(DataType.DateTime)]
        public DateTime  created_at { get; set; }

    }
}
