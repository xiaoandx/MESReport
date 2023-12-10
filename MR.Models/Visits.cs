using System;
using System.ComponentModel.DataAnnotations;

namespace MR.Models
{

    /// <summary>
    /// 内容摘要: 访问量 mr_visits 实体类
    /// </summary>
    public class Visits
    {
        /// <summary>
        /// 访问量
        /// </summary>
        [Key]
        public long  visit_id { get; set; }
        /// <summary>
        /// 来源网址
        /// </summary>
        public string?  referrer_url { get; set; }
        /// <summary>
        /// 搜索引擎
        /// </summary>
        public string?  search_engine { get; set; }
        /// <summary>
        /// 客户端访问浏览器
        /// </summary>
        public string?  client_browser { get; set; }
        /// <summary>
        /// 被访问页面
        /// </summary>
        public string?  visit_path { get; set; }
        /// <summary>
        /// 客户端信息
        /// </summary>
        public string?  client_info { get; set; }
        /// <summary>
        /// 客户端Cookies
        /// </summary>
        public string?  client_cookies { get; set; }
        /// <summary>
        /// 设备操作系统
        /// </summary>
        public string?  device_system { get; set; }
        /// <summary>
        /// 搜客户端平台
        /// </summary>
        public string?  device_platform { get; set; }
        /// <summary>
        /// 设备品牌
        /// </summary>
        public string?  device_brand { get; set; }
        /// <summary>
        /// 设备型号
        /// </summary>
        public string?  device_model { get; set; }
        /// <summary>
        /// 设备语言
        /// </summary>
        public string?  device_language { get; set; }
        /// <summary>
        /// 系统版本号
        /// </summary>
        public string?  device_version { get; set; }
        /// <summary>
        /// 设备信息
        /// </summary>
        public string?  device_info { get; set; }
        /// <summary>
        /// 临时信息
        /// </summary>
        public string?  temp_info { get; set; }
        /// <summary>
        /// 会员
        /// </summary>
        public long  member_id { get; set; }
        /// <summary>
        /// 公司
        /// </summary>
        public long  company_id { get; set; }
        /// <summary>
        /// 用户唯一标识
        /// </summary>
        public string?  client_id { get; set; }
        /// <summary>
        /// 客户端IP
        /// </summary>
        public string?  client_ip { get; set; }
        /// <summary>
        /// 客户端来源
        /// </summary>
        public int  source_id { get; set; }
        /// <summary>
        /// 频道
        /// </summary>
        public int  channel_id { get; set; }
        /// <summary>
        /// 客户端版本号
        /// </summary>
        public string?  client_version { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string?  visit_remark { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        [DataType(DataType.DateTime)]
        public DateTime  created_at { get; set; }

    }
}
