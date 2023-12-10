using System;
using System.ComponentModel.DataAnnotations;

namespace MR.Models
{

    /// <summary>
    /// 内容摘要: 短信 mr_sms 实体类
    /// </summary>
    public class Sms
    {
        /// <summary>
        /// 短信
        /// </summary>
        [Key]
        public long  sms_id { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string?  mobile_phone { get; set; }
        /// <summary>
        /// 短信内容
        /// </summary>
        public string?  sms_content { get; set; }
        /// <summary>
        /// 接口返回信息
        /// </summary>
        public string?  result_content { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        [DataType(DataType.DateTime)]
        public DateTime  created_at { get; set; }

    }
}
