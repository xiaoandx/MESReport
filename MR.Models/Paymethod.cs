using System;
using System.ComponentModel.DataAnnotations;

namespace MR.Models
{

    /// <summary>
    /// 内容摘要: 支付方式表 mr_paymethod 实体类
    /// </summary>
    public class Paymethod
    {
        /// <summary>
        /// 支付方式表
        /// </summary>
        [Key]
        public int  paymethod_id { get; set; }
        /// <summary>
        /// 支付平台名称
        /// </summary>
        public string?  paymethod_name { get; set; }

    }
}
