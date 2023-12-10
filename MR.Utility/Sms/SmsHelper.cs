using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Reflection;

namespace MR.Utility.Sms
{

    /// <summary>
    /// </summary>
    public class SMSHelper
    {
        /// <summary>
        /// sms.cn注册账号（此处需要修改成你自己的内容）
        /// </summary>
        public static string sms_uid = "xxx";
        /// <summary>
        /// sms.cn注册账号密码（此处需要修改成你自己的内容）
        /// </summary>
        public static string sms_password = "******";

        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <param name="code">验证码</param>
        /// <returns>返回短信发送结果</returns>
        public static string SendVerifyCode(string mobile, string code)
        {
            string result = "";

            try
            {
#pragma warning disable CS8602 // 解引用可能出现空引用。
                string content = "您的验证码是" + code + "。如非本人操作，请忽略本短信。【" + Config.CfgManager.Configuration["Setting:SignName"] + "】";
#pragma warning restore CS8602 // 解引用可能出现空引用。

                StringBuilder sb = new StringBuilder();
                var sms_pwd = Utility.Helper.MD5Helper.MD5Encrypt32(sms_password + sms_uid).ToLower();
                sb.Append("ac=send&uid=" + sms_uid + "&pwd=" + sms_pwd + "&mobile=" + mobile + "&content=" + content);
                byte[] bData = System.Text.Encoding.UTF8.GetBytes(sb.ToString());
                result = Helper.HttpClientHelper.Post("http://api.sms.cn/sms/", bData);
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 发送通用短信模板
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <param name="content">发送内容</param>
        /// <returns>返回短信发送结果</returns>
        public static string SendContent(string mobile, string content)
        {
            string result = "";

            try
            {
                StringBuilder sb = new StringBuilder();
                var sms_pwd = Utility.Helper.MD5Helper.MD5Encrypt32(sms_password + sms_uid).ToLower();
                sb.Append("ac=send&uid=" + sms_uid + "&pwd=" + sms_pwd + "&mobile=" + mobile + "&content=" + content);
                byte[] bData = System.Text.Encoding.UTF8.GetBytes(sb.ToString());
                result = Helper.HttpClientHelper.Post("http://api.sms.cn/sms/", bData);
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            return result;
        }
    }
}
