using System;
using System.Collections.Generic;
using System.Text;

namespace MR.Utility.Pay.WeChat
{
    /// <summary>
    /// PayRequesEntity
    /// </summary>
    public class PayRequesEntity
    {
        /// <summary>
        /// 时间戳从1970年1月1日00:00:00至今的秒数,即当前的时间
        /// </summary>
        public string? timeStamp { get; set; }

        /// <summary>
        /// 随机字符串，长度为32个字符以下。
        /// </summary>
        public string? nonceStr { get; set; }

        /// <summary>
        /// 统一下单接口返回的 prepay_id 参数值
        /// </summary>
        public string? package { get; set; }

        /// <summary>
        /// 签名算法
        /// </summary>
        public string? signType { get; set; }

        /// <summary>
        /// 签名
        /// </summary>
        public string? paySign { get; set; }
    }
}
