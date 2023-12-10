using System;
using System.ComponentModel.DataAnnotations;

namespace MR.Models
{

    /// <summary>
    /// 内容摘要: 区域 mr_areas 实体类
    /// </summary>
    public class Areas
    {
        /// <summary>
        /// 区域
        /// </summary>
        [Key]
        public int  area_id { get; set; }
        /// <summary>
        /// 区域名称
        /// </summary>
        public string?  area_name { get; set; }
        /// <summary>
        /// 父级
        /// </summary>
        public string?  parent_id { get; set; }
        /// <summary>
        /// 简称
        /// </summary>
        public string?  short_name { get; set; }
        /// <summary>
        /// 级别
        /// </summary>
        public string?  level_type { get; set; }
        /// <summary>
        /// 城市代码
        /// </summary>
        public string?  city_code { get; set; }
        /// <summary>
        /// 邮政编码
        /// </summary>
        public string?  zip_code { get; set; }
        /// <summary>
        /// 全称
        /// </summary>
        public string?  merger_name { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public string?  area_lng { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        public string?  area_lat { get; set; }
        /// <summary>
        /// 拼音
        /// </summary>
        public string?  area_pinyin { get; set; }

    }
}
