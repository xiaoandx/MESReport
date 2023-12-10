using System;
using System.ComponentModel.DataAnnotations;

namespace MR.Models
{

    /// <summary>
    /// 内容摘要: 基本信息 mr_setting_bases 实体类
    /// </summary>
    public class SettingBases
    {
        /// <summary>
        /// 基本信息
        /// </summary>
        [Key]
        public int  base_id { get; set; }
        /// <summary>
        /// 网站名称
        /// </summary>
        public string?  website_name { get; set; }
        /// <summary>
        /// 网站网址
        /// </summary>
        public string?  website_url { get; set; }
        /// <summary>
        /// 缓存时间
        /// </summary>
        public int  cache_time { get; set; }
        /// <summary>
        /// 最大上传文件
        /// </summary>
        public int  file_maxsize { get; set; }
        /// <summary>
        /// 上传文件类型
        /// </summary>
        public string?  file_type { get; set; }
        /// <summary>
        /// 首页标题
        /// </summary>
        public string?  home_title { get; set; }
        /// <summary>
        /// META关键字
        /// </summary>
        public string?  meta_keyword { get; set; }
        /// <summary>
        /// META描述
        /// </summary>
        public string?  meta_description { get; set; }
        /// <summary>
        /// 版权信息
        /// </summary>
        public string?  copyright_info { get; set; }
        /// <summary>
        /// ICP备案
        /// </summary>
        public string?  icp_no { get; set; }

    }
}
