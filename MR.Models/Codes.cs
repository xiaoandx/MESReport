using System;
using System.ComponentModel.DataAnnotations;

namespace MR.Models
{

    /// <summary>
    /// 内容摘要: 验证码 mr_codes 实体类
    /// </summary>
    public class Codes
    {
        /// <summary>
        /// 验证码
        /// </summary>
        [Key]
        public long  code_id { get; set; }
        /// <summary>
        /// 国家代码
        /// </summary>
        public string?  country_code { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string?  mobile_phone { get; set; }
        /// <summary>
        /// 验证码
        /// </summary>
        public string?  code_num { get; set; }
        /// <summary>
        /// 客户端
        /// </summary>
        public string?  code_from { get; set; }
        /// <summary>
        /// 验证码有效期多少分钟
        /// </summary>
        public int  due_minute { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        [DataType(DataType.DateTime)]
        public DateTime  created_at { get; set; }

    }
}
