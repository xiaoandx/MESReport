using System;
using System.ComponentModel.DataAnnotations;

namespace MR.Models
{

    /// <summary>
    /// 内容摘要: 日志 mr_logs 实体类
    /// </summary>
    public class Logs
    {
        /// <summary>
        /// 日志
        /// </summary>
        [Key]
        public long  log_id { get; set; }
        /// <summary>
        /// 日志类型
        /// </summary>
        public int  type_id { get; set; }
        /// <summary>
        /// 后台用户
        /// </summary>
        public long  user_id { get; set; }
        /// <summary>
        /// 会员
        /// </summary>
        public long  member_id { get; set; }
        /// <summary>
        /// 日志标识码
        /// </summary>
        public string?  log_code { get; set; }
        /// <summary>
        /// 平台类型
        /// </summary>
        public int  platform_id { get; set; }
        /// <summary>
        /// 日志内容
        /// </summary>
        public string?  log_content { get; set; }
        /// <summary>
        /// 客户端IP
        /// </summary>
        public string?  client_ip { get; set; }
        /// <summary>
        /// 当前操作页面URL
        /// </summary>
        public string?  page_url { get; set; }
        /// <summary>
        /// 来源页面URL
        /// </summary>
        public string?  referrer_url { get; set; }
        /// <summary>
        /// 客户端信息
        /// </summary>
        public string?  user_agent { get; set; }
        /// <summary>
        /// Cookies
        /// </summary>
        public string?  user_cookies { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string?  log_remark { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        [DataType(DataType.DateTime)]
        public DateTime  created_at { get; set; }

    }
}
