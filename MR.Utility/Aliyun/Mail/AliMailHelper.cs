using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading.Tasks;

using Tea;
using Tea.Utils;

namespace MR.Utility.Aliyun.Mail
{
    /// <summary>
    /// 阿里云邮件推送
    /// </summary>
    public class AliMailHelper
    {
        /**
         * 使用AK-SK初始化账号Client
         * @param accessKeyId
         * @param accessKeySecret
         * @return Client
         * @throws Exception
         */
        public static AlibabaCloud.SDK.Dm20151123.Client CreateClient(string accessKeyId, string accessKeySecret)
        {
            AlibabaCloud.OpenApiClient.Models.Config config = new AlibabaCloud.OpenApiClient.Models.Config
            {
                // 您的AccessKey ID
                AccessKeyId = accessKeyId,
                // 您的AccessKey Secret
                AccessKeySecret = accessKeySecret,
            };
            // 访问的域名
            config.Endpoint = "dm.aliyuncs.com";
            return new AlibabaCloud.SDK.Dm20151123.Client(config);
        }

        /// <summary>
        /// 发送单个邮件
        /// </summary>
        /// <param name="send_address"></param>
        /// <param name="to_address"></param>
        /// <param name="subject"></param>
        /// <param name="html_body"></param>
        /// <param name="from_alias"></param>
        public static void SingleSendMail(string send_address,string to_address,string subject,string html_body,string from_alias)
        {
            // （此处需要修改成你自己的内容）
            var accessKeyId = "xxxxxx";
            var accessKeySecret = "xxxxxx";
            AlibabaCloud.SDK.Dm20151123.Client client = CreateClient(accessKeyId, accessKeySecret);
            AlibabaCloud.SDK.Dm20151123.Models.SingleSendMailRequest singleSendMailRequest = new AlibabaCloud.SDK.Dm20151123.Models.SingleSendMailRequest
            {
                AccountName = send_address,
                AddressType = 1,
                ReplyToAddress = false,
                ToAddress = to_address,
                Subject = subject,
                HtmlBody = html_body,
                FromAlias = from_alias,
            };
            // 复制代码运行请自行打印 API 的返回值
            client.SingleSendMail(singleSendMailRequest);
        }

    }
}
