using System;
using System.Collections.Generic;
using System.Text;

namespace MR.Utility.Pay.WeChat
{
    /// <summary>
    /// 微信支付配置
    /// </summary>
    public class WeChatConfig
    {
        /// <summary>
        /// APPID（此处需要修改成你自己的内容）
        /// </summary>
        public static string APPID = "xxxxxx";
        /// <summary>
        /// SERCRET（此处需要修改成你自己的内容）
        /// </summary>
        public static string SERCRET = "xxxxxx";
        /// <summary>
        /// MCHID（此处需要修改成你自己的内容）
        /// </summary>
        public static string MCHID = "xxxxxx";
        /// <summary>
        /// MCHKEY（此处需要修改成你自己的内容）
        /// </summary>
        public static string MCHKEY = "xxxxxx";
        /// <summary>
        /// UnifiedorderURL
        /// </summary>
        public static string UnifiedorderURL = "https://api.mch.weixin.qq.com/pay/unifiedorder";
        /// <summary>
        /// 支付推送消息
        /// </summary>
        public static string NOTIFY_URL = "https://api.xxxxx.com/api/orders/miniwechat/notify";
        /// <summary>
        /// IP
        /// </summary>
        public static string IP = "8.8.8.8";
    }
}
