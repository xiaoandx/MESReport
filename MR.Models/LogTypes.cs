using System;
using System.ComponentModel.DataAnnotations;

namespace MR.Models
{

    /// <summary>
    /// 内容摘要: 日志类型表 mr_log_types 实体类
    /// </summary>
    public class LogTypes
    {
        /// <summary>
        /// 日志类型表
        /// </summary>
        [Key]
        public int  type_id { get; set; }
        /// <summary>
        /// 类型名称
        /// </summary>
        public string?  type_name { get; set; }

    }
}
