using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using Tea;
using Tea.Utils;
using AlibabaCloud.SDK;
using Microsoft.AspNetCore.Mvc;

namespace MR.Utility.Sms
{
    /// <summary>
    /// </summary>
    public class AliyunSmsHelper
    {
        /**
         * 使用AK&SK初始化账号Client
         * @param accessKeyId
         * @param accessKeySecret
         * @return Client
         * @throws Exception
         */
        public static AlibabaCloud.SDK.Dysmsapi20170525.Client CreateClient(string accessKeyId, string accessKeySecret)
        {
            AlibabaCloud.OpenApiClient.Models.Config config = new AlibabaCloud.OpenApiClient.Models.Config
            {
                // 必填，您的 AccessKey ID
                AccessKeyId = accessKeyId,
                // 必填，您的 AccessKey Secret
                AccessKeySecret = accessKeySecret,
            };
            // 访问的域名
            config.Endpoint = "dysmsapi.aliyuncs.com";
            return new AlibabaCloud.SDK.Dysmsapi20170525.Client(config);
        }

        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <param name="sign_name">签名</param>
        /// <param name="template_code">短信模版编码</param>
        /// <param name="mobile_phone">手机号</param>
        /// <param name="code">验证码</param>
        public static string SendVerifyCode(string sign_name, string template_code, string mobile_phone, string code)
        {
            string result = "";

            // 以下代码示例仅供参考，建议使用更安全的 STS 方式，更多鉴权访问方式请参见：https://help.aliyun.com/document_detail/378671.html
            AlibabaCloud.SDK.Dysmsapi20170525.Client client = CreateClient(Config.AliyunOSS.AccessKeyId, Config.AliyunOSS.AccessKeySecret);
            AlibabaCloud.SDK.Dysmsapi20170525.Models.SendSmsRequest sendSmsRequest = new AlibabaCloud.SDK.Dysmsapi20170525.Models.SendSmsRequest
            {
                SignName = sign_name,
                PhoneNumbers = mobile_phone,
                TemplateCode = template_code,
                TemplateParam = "{\"code\":\"" + code + "\"}",
            };
            AlibabaCloud.TeaUtil.Models.RuntimeOptions runtime = new AlibabaCloud.TeaUtil.Models.RuntimeOptions();
            try
            {
                var sendSmsResponse = client.SendSmsWithOptions(sendSmsRequest, runtime);
                result = sendSmsResponse.Body.Message;
            }
            catch (TeaException error)
            {
                result = error.Message;
            }
            catch (Exception _error)
            {
                TeaException error = new TeaException(new Dictionary<string, object>
                {
                    { "message", _error.Message }
                });
                result = error.Message;
            }

            return result;
        }
    }
}

