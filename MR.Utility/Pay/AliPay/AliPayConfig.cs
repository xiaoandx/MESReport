using System;
using System.Collections.Generic;
using System.Text;

namespace MR.Utility.Pay.AliPay
{
    /// <summary>
    /// 支付宝支付参数配置
    /// </summary>
    public class AliPayConfig
    {
        /// <summary>
        /// AppId（此处需要修改成你自己的内容）
        /// </summary>
        public static string AppId = "xxxxxx";
        /// <summary>
        /// 应用私钥-PrivateKey（此处需要修改成你自己的内容）
        /// </summary>
        public static string PrivateKey = "xxxxxx";
        /// <summary>
        /// RSA2
        /// </summary>
        public static string SignType = "RSA2";
        /// <summary>
        /// 支付宝公钥-AlipayPublicKey（此处需要修改成你自己的内容）
        /// </summary>
        public static string AlipayPublicKey = "xxxxxx";
        /// <summary>
        /// CharSet
        /// </summary>
        public static string CharSet = "UTF-8";
        /// <summary>
        /// MchId（此处需要修改成你自己的内容）
        /// </summary>
        public static string MchId = "xxxxxx";


        /* 以下为PC端支付配置信息 */
        /// <summary>
        /// 应用ID,您的APPID（此处需要修改成你自己的内容）
        /// </summary>
        public static string pc_app_id = "xxxxxx";

        /// <summary>
        /// 支付宝网关
        /// </summary>
        public static string pc_gatewayUrl = "https://openapi.alipay.com/gateway.do";

        /// <summary>
        /// 商户私钥，您的原始格式RSA私钥（此处需要修改成你自己的内容）
        /// </summary>
        public static string pc_private_key = "xxxxxx";

        /// <summary>
        /// 支付宝公钥（此处需要修改成你自己的内容）
        /// 查看地址：https://openhome.alipay.com/platform/keyManage.htm 对应APPID下的支付宝公钥。
        /// </summary>
        public static string pc_alipay_public_key = "xxxxxx";

        /// <summary>
        /// 签名方式
        /// </summary>
        public static string pc_sign_type = "RSA2";

        /// <summary>
        /// 编码格式
        /// </summary>
        public static string pc_charset = "UTF-8";

    }
}
