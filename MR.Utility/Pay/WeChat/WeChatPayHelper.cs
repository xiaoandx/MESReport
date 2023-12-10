using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Xml.Linq;

namespace MR.Utility.Pay.WeChat
{
    /// <summary>
    /// 封装微信小程序支付类
    /// </summary>
    public class WeChatPayHelper
    {
        /// <summary>
        /// Unifiedorder
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="totalfee"></param>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        public PayRequesEntity Unifiedorder(string openid, string totalfee, string orderNo)
        {
            var PayUrl = WeChatConfig.UnifiedorderURL;
            var param = GetUnifiedOrderParam(openid, orderNo, totalfee);
            // 统一下单后拿到的xml结果
            var payResXML = HttpMethods.Post(PayUrl, param);
            var payRes = XDocument.Parse(payResXML);
            var root = payRes.Element("xml");
            var res = GetPayRequestParam(root!);
            return res;
        }

        /// <summary>
        /// 取统一下单的请求参数
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="orderNo"></param>
        /// <param name="totalfee"></param>
        /// <returns></returns>
        private string GetUnifiedOrderParam(string openid, string orderNo, string totalfee)
        {
            string appid = WeChatConfig.APPID;
            string secret = WeChatConfig.SERCRET;
            string mch_id = WeChatConfig.MCHID;
            string ip = WeChatConfig.IP;
            string PayResulturl = WeChatConfig.NOTIFY_URL;

            string strcode = "XX微信小程序购买";////商品描述交易字段格式根据不同的应用场景按照以下格式：APP——需传入应用市场上的APP名字-实际商品名称，天天爱消除-游戏充值。
            byte[] buffer = Encoding.UTF8.GetBytes(strcode);
            string body = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
            System.Random Random = new System.Random();
            var dic = new Dictionary<string, string>
                {
                    {"appid", appid},
                    {"mch_id", mch_id},
                    {"nonce_str", GetRandomString(20)/*Random.Next().ToString()*/},
                    {"body", body},
                    {"out_trade_no", orderNo},//商户自己的订单号码
                    {"total_fee", "1"},
                    {"spbill_create_ip", ip},//服务器的IP地址
                    {"notify_url", PayResulturl},//异步通知的地址，不能带参数
                    {"trade_type", "JSAPI" },
                    {"openid", openid}
                };

            // 加入签名
            dic.Add("sign", GetSignString(dic));

            var sb = new StringBuilder();
            sb.Append("<xml>");
            foreach (var d in dic)
            {
                sb.Append("<" + d.Key + ">" + d.Value + "</" + d.Key + ">");
            }
            sb.Append("</xml>");
            return sb.ToString();
        }

        /// <summary>
        /// 获取返回给小程序的支付参数
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        private PayRequesEntity GetPayRequestParam(XElement root)
        {
            string appid = WeChatConfig.APPID;
            //当return_code 和result_code都为SUCCESS时才有我们要的prepay_id
            if (root.Element("return_code")!.Value == "SUCCESS" && root.Element("result_code")!.Value == "SUCCESS")
            {
                var res = new Dictionary<string, string>
                {
                    {"appId", appid},
                    {"timeStamp", Convert.ToInt64((DateTime.Now - new DateTime(1970, 1, 1)).TotalSeconds).ToString()},
                    {"nonceStr", GetRandomString(20)},
                    {"package",  "prepay_id=" + root.Element("prepay_id")!.Value},
                    {"signType", "MD5"}
                };
                //在服务器上签名
                res.Add("paySign", GetSignString(res));

                var payEntity = new PayRequesEntity
                {
                    package = res["package"],
                    nonceStr = res["nonceStr"],
                    paySign = res["paySign"],
                    signType = res["signType"],
                    timeStamp = res["timeStamp"]
                };
                return payEntity;
            }

            return new PayRequesEntity();
        }

        /// <summary>
        /// 从字符串里随机得到，规定个数的字符串.
        /// </summary>
        /// <param name="CodeCount"></param>
        /// <returns></returns>
        private static string GetRandomString(int CodeCount)
        {
            string allChar = "1,2,3,4,5,6,7,8,9,A,B,C,D,E,F,G,H,i,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";
            string[] allCharArray = allChar.Split(',');
            string RandomCode = "";
            int temp = -1;
            Random rand = new Random();
            for (int i = 0; i < CodeCount; i++)
            {
                if (temp != -1)
                {
                    rand = new Random(temp * i * ((int)DateTime.Now.Ticks));
                }
                int t = rand.Next(allCharArray.Length - 1);
                while (temp == t)
                {
                    t = rand.Next(allCharArray.Length - 1);
                }
                temp = t;
                RandomCode += allCharArray[t];
            }

            return RandomCode;
        }

        /// <summary>
        /// GetSignString
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        private string GetSignString(Dictionary<string, string> dic)
        {
            string key = WeChatConfig.MCHKEY;//商户平台 API安全里面设置的KEY  32位长度                                                                                                            
            dic = dic.OrderBy(d => d.Key).ToDictionary(d => d.Key, d => d.Value);//排序
            //连接字段
            var sign = dic.Aggregate("", (current, d) => current + (d.Key + "=" + d.Value + "&"));
            sign += "key=" + key;
            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            sign = BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(sign))).Replace("-", null);
            return sign;
        }

        /// <summary>
        /// PostUnifiedOrder
        /// </summary>
        /// <param name="payUrl"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        private static Object PostUnifiedOrder(string payUrl, string para)
        {
            string result = string.Empty;
            try
            {
                HttpClient client = new HttpClient();
                HttpContent httpContent = new StringContent(para);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                httpContent.Headers.ContentType.CharSet = "utf-8";
                HttpResponseMessage hrm = client.PostAsync(payUrl, httpContent).Result;
                return hrm;
            }
            catch (Exception e)
            {
                result = e.Message;
            }
            return result;
        }
    }
}
