using System;
using System.Collections.Generic;
using System.Text;
using Alipay.AopSdk.Core;
using Alipay.AopSdk.Core.Domain;
using Alipay.AopSdk.Core.Request;
using Newtonsoft.Json;
using Alipay.AopSdk.Core.Response;

namespace MR.Utility.Pay.AliPay
{
    /// <summary>
    /// 支付宝APP支付接口
    /// </summary>
    public class AppPayHelper
    {
        /// <summary>
        /// 获取唤起支付所需的支付订单信息
        /// </summary>
        /// <param name="out_trade_no">订单号</param>
        /// <param name="total_amount">订单金额</param>
        /// <param name="subject">商品标题</param>
        /// <returns></returns>
        public static AlipayTradeAppPayResponse Pay(string out_trade_no,decimal total_amount,string subject)
        {
            IAopClient client = new DefaultAopClient("https://openapi.alipay.com/gateway.do", AliPayConfig.AppId, AliPayConfig.PrivateKey, "json", "1.0", "RSA2", AliPayConfig.AlipayPublicKey, AliPayConfig.CharSet, false);
            AlipayTradeAppPayRequest request = new AlipayTradeAppPayRequest();
            //request.SetNotifyUrl("https://api.mr-shop.com/api/orders/notify");
            Dictionary<string, object> bizContent = new Dictionary<string, object>();
            bizContent.Add("out_trade_no", out_trade_no);
            bizContent.Add("total_amount", total_amount);
            bizContent.Add("subject", subject);
            bizContent.Add("product_code", "QUICK_MSECURITY_PAY");
            //bizContent.Add("time_expire", "2022-08-01 22:00:00");

            ////商品明细信息，按需传入
            //List<object> goodsDetails = new List<object>();
            //Dictionary<string, object> goods1 = new Dictionary<string, object>();
            //goods1.Add("goods_id", "goodsNo1");
            //goods1.Add("goods_name", "子商品1");
            //goods1.Add("quantity", 1);
            //goods1.Add("price", 0.01);
            //goodsDetails.Add(goods1);
            //bizContent.Add("goods_detail", goodsDetails);

            ////扩展信息，按需传入
            //Dictionary<string, object> extendParams = new Dictionary<string, object>();
            //extendParams.Add("sys_service_provider_id", "2088501624560335");
            //bizContent.Add("extend_params", extendParams);

            string Contentjson = JsonConvert.SerializeObject(bizContent);
            request.BizContent = Contentjson;
            AlipayTradeAppPayResponse response = client.SdkExecute(request);

            return response;
        }
    }
}
