using Alipay.AopSdk.Core;
using Alipay.AopSdk.Core.Domain;
using Alipay.AopSdk.Core.Request;
using Alipay.AopSdk.Core.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace MR.Utility.Pay.AliPay
{
    /// <summary>
    /// 支付宝电脑端支付类
    /// </summary>
    public class PCPayHelper
    {
        /// <summary>
        /// 支付订单
        /// </summary>
        /// <param name="out_trade_no">外部订单号，商户网站订单系统中唯一的订单号</param>
        /// <param name="total_amount">付款金额</param>
        /// <param name="subject">订单名称</param>
        /// <returns></returns>
        public static string Pay(string out_trade_no, string total_amount, string subject)
        {
            DefaultAopClient client = new DefaultAopClient(AliPayConfig.pc_gatewayUrl, AliPayConfig.pc_app_id, AliPayConfig.pc_private_key, "json", "1.0", AliPayConfig.pc_sign_type, AliPayConfig.pc_alipay_public_key, AliPayConfig.pc_charset, false);
            // 商品描述
            string body = "商品";

            // 组装业务参数model
            AlipayTradePagePayModel model = new AlipayTradePagePayModel();
            model.Body = body;
            model.Subject = subject;
            model.TotalAmount = total_amount;
            model.OutTradeNo = out_trade_no;
            model.ProductCode = "FAST_INSTANT_TRADE_PAY";

            AlipayTradePagePayRequest request = new AlipayTradePagePayRequest();
            // 设置同步回调地址
            request.SetReturnUrl("https://www.xxxxxx.com/orders/");
            // 设置异步通知接收地址
            request.SetNotifyUrl("https://api.xxxxxx.com/api/orders/pc/notify");
            // 将业务model载入到request
            request.SetBizModel(model);

            AlipayTradePagePayResponse response = client.PageExecute(request, null, "post");

            return response.Body;
        }
    }
}
