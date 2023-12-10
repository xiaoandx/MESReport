using System;
using System.ComponentModel.DataAnnotations;

namespace MR.Models
{

    /// <summary>
    /// 内容摘要: 广告 mr_ads 实体类
    /// </summary>
    public class Ads
    {
        /// <summary>
        /// 广告
        /// </summary>
        [Key]
        public int  ads_id { get; set; }
        /// <summary>
        /// 多语言
        /// </summary>
        public int  lang_id { get; set; }
        /// <summary>
        /// 广告位置
        /// </summary>
        public int  position_id { get; set; }
        /// <summary>
        /// 链接类型
        /// </summary>
        public int  link_id { get; set; }
        /// <summary>
        /// 广告名称
        /// </summary>
        public string?  ads_name { get; set; }
        /// <summary>
        /// 广告图片
        /// </summary>
        public string?  ads_image { get; set; }
        /// <summary>
        /// OSS链接
        /// </summary>
        public string?  oss_url { get; set; }
        /// <summary>
        /// 发布人
        /// </summary>
        public long  user_id { get; set; }
        /// <summary>
        /// 链接地址
        /// </summary>
        public string?  page_url { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int  sort_num { get; set; }
        /// <summary>
        /// 是否显示
        /// </summary>
        public bool  is_show { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string?  ads_remark { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        [DataType(DataType.DateTime)]
        public DateTime  created_at { get; set; }

    }
}
