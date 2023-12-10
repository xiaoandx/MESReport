using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;
using System.Text;

namespace MR.Utility.Helper
{
    /// <summary>
    /// 处理微信小程序用户数据的签名验证和解密
    /// </summary>
    public class WeChatAppDecrypt
    {
        private string appId;
        private string appSecret;

        /// <summary>
        /// 获取微信AccessToken
        /// </summary>
        /// <param name="strAppid"></param>
        /// <param name="strAppsecret"></param>
        /// <returns></returns>
        public static string GetAccessToken(string strAppid, string strAppsecret)
        {
            string result = "";

            WeChatAppDecrypt wechat = new WeChatAppDecrypt(strAppid, strAppsecret);
            string strResult = wechat.GetAccessTokenString();

            JObject obj = (JObject)JsonConvert.DeserializeObject(strResult)!;
            if (obj != null)
            {
                result = obj["access_token"]!.ToString();
            }

            return result;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="appId">应用程序的AppId</param>
        /// <param name="appSecret">应用程序的AppSecret</param>
        public WeChatAppDecrypt(string appId, string appSecret)
        {
            this.appId = appId;
            this.appSecret = appSecret;
            return;
        }

        /// <summary>
        /// 获取OpenId和SessionKey的Json数据包
        /// </summary>
        /// <param name="code">客户端发来的code</param>
        /// <returns>Json数据包</returns>
        public string GetOpenIdAndSessionKeyString(string code)
        {
            string temp = "https://api.weixin.qq.com/sns/jscode2session?" +
                "appid=" + appId
                + "&secret=" + appSecret
                + "&js_code=" + code
                + "&grant_type=authorization_code";

            return GetResponse(temp);

        }

        /// <summary>
        /// 获取AccessTokenn
        /// </summary>
        /// <returns></returns>
        public string GetAccessTokenString()
        {
            //&appid=APPID&secret=APPSECRET
            string temp = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential" +
                "&appid=" + appId
                + "&secret=" + appSecret;

            return GetResponse(temp);

        }

        /// <summary>
        /// 反序列化包含OpenId和SessionKey的Json数据包
        /// </summary>
        /// <param name="loginInfo"></param>
        /// <returns></returns>
        public OpenIdAndSessionKey DecodeOpenIdAndSessionKey(WechatLoginInfo loginInfo)
        {
            OpenIdAndSessionKey oiask = JsonConvert.DeserializeObject<OpenIdAndSessionKey>(GetOpenIdAndSessionKeyString(loginInfo.code!))!;
            //if (!String.IsNullOrEmpty(oiask.errcode))
            //    return null;
            return oiask;
        }

        /// <summary>
        /// 根据微信小程序平台提供的签名验证算法验证用户发来的数据是否有效
        /// </summary>
        /// <param name="rawData">公开的用户资料</param>
        /// <param name="signature">公开资料携带的签名信息</param>
        /// <param name="sessionKey">从服务端获取的SessionKey</param>
        /// <returns>True：资料有效，False：资料无效</returns>
        [Obsolete]
        public bool VaildateUserInfo(string rawData, string signature, string sessionKey)
        {
            //创建SHA1签名类
            SHA1 sha1 = new SHA1CryptoServiceProvider();
            //编码用于SHA1验证的源数据
            byte[] source = Encoding.UTF8.GetBytes(rawData + sessionKey);
            //生成签名
            byte[] target = sha1.ComputeHash(source);
            //转化为string类型，注意此处转化后是中间带短横杠的大写字母，需要剔除横杠转小写字母
            string result = BitConverter.ToString(target).Replace("-", "").ToLower();
            //比对，输出验证结果
            return signature == result;
        }

        /// <summary>
        /// 根据微信小程序平台提供的签名验证算法验证用户发来的数据是否有效
        /// </summary>
        /// <param name="loginInfo">登陆信息</param>
        /// <param name="sessionKey">从服务端获取的SessionKey</param>
        /// <returns>True：资料有效，False：资料无效</returns>
        [Obsolete]
        public bool VaildateUserInfo(WechatLoginInfo loginInfo, string sessionKey)
        {
            return VaildateUserInfo(loginInfo.rawData!, loginInfo.signature!, sessionKey);
        }

        /// <summary>
        /// 根据微信小程序平台提供的签名验证算法验证用户发来的数据是否有效
        /// </summary>
        /// <param name="loginInfo">登陆信息</param>
        /// <param name="idAndKey">包含OpenId和SessionKey的类</param>
        /// <returns>True：资料有效，False：资料无效</returns>
        [Obsolete]
        public bool VaildateUserInfo(WechatLoginInfo loginInfo, OpenIdAndSessionKey idAndKey)
        {
            return VaildateUserInfo(loginInfo, idAndKey.session_key!);
        }

        /// <summary>
        /// 根据微信小程序平台提供的解密算法解密数据
        /// </summary>
        /// <param name="encryptedData">加密数据</param>
        /// <param name="iv">初始向量</param>
        /// <param name="sessionKey">从服务端获取的SessionKey</param>
        /// <returns></returns>
        [Obsolete]
        public WechatUserInfo Decrypt(string encryptedData, string iv, string sessionKey)
        {
            WechatUserInfo userInfo;
            //创建解密器生成工具实例
            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            //设置解密器参数
            aes.Mode = CipherMode.CBC;
            aes.BlockSize = 128;
            aes.Padding = PaddingMode.PKCS7;
            //格式化待处理字符串
            byte[] byte_encryptedData = Convert.FromBase64String(encryptedData);
            byte[] byte_iv = Convert.FromBase64String(iv);
            byte[] byte_sessionKey = Convert.FromBase64String(sessionKey);

            aes.IV = byte_iv;
            aes.Key = byte_sessionKey;
            //根据设置好的数据生成解密器实例
            ICryptoTransform transform = aes.CreateDecryptor();

            //解密
            byte[] final = transform.TransformFinalBlock(byte_encryptedData, 0, byte_encryptedData.Length);

            //生成结果
            string result = Encoding.UTF8.GetString(final);

            //反序列化结果，生成用户信息实例
            userInfo = JsonConvert.DeserializeObject<WechatUserInfo>(result)!;

            return userInfo;

        }


        /// <summary>
        /// 根据微信小程序平台提供的解密算法解密数据 返回Json字符串
        /// </summary>
        /// <param name="encryptedData">加密数据</param>
        /// <param name="iv">初始向量</param>
        /// <param name="sessionKey">从服务端获取的SessionKey</param>
        /// <returns></returns>
        [Obsolete]
        public string DecryptToJson(string encryptedData, string iv, string sessionKey)
        {
            //创建解密器生成工具实例
            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            //设置解密器参数
            aes.Mode = CipherMode.CBC;
            aes.BlockSize = 128;
            aes.Padding = PaddingMode.PKCS7;
            //格式化待处理字符串
            byte[] byte_encryptedData = Convert.FromBase64String(encryptedData);
            byte[] byte_iv = Convert.FromBase64String(iv);
            byte[] byte_sessionKey = Convert.FromBase64String(sessionKey);

            aes.IV = byte_iv;
            aes.Key = byte_sessionKey;
            //根据设置好的数据生成解密器实例
            ICryptoTransform transform = aes.CreateDecryptor();

            //解密
            byte[] final = transform.TransformFinalBlock(byte_encryptedData, 0, byte_encryptedData.Length);

            //生成结果
            string result = Encoding.UTF8.GetString(final);

            return result;

        }

        /// <summary>
        /// 根据微信小程序平台提供的解密算法解密数据，推荐直接使用此方法
        /// </summary>
        /// <param name="loginInfo">登陆信息</param>
        /// <returns>用户信息</returns>
        [Obsolete]
        public WechatUserInfo? Decrypt(WechatLoginInfo loginInfo)
        {
            if (loginInfo == null)
                return null;

            if (String.IsNullOrEmpty(loginInfo.code))
                return null;

            OpenIdAndSessionKey oiask = DecodeOpenIdAndSessionKey(loginInfo);

            if (oiask == null)
                return null;

            if (!VaildateUserInfo(loginInfo, oiask))
                return null;

            WechatUserInfo userInfo = Decrypt(loginInfo.encryptedData!, loginInfo.iv!, oiask.session_key!);

            return userInfo;
        }

        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private string GetResponse(string url)
        {
            if (url.StartsWith("https"))
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;

            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = httpClient.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                return result;
            }
            return "";
        }


    }
    /// <summary>
    /// 微信小程序登录信息结构
    /// </summary>
    public class WechatLoginInfo
    {
        /// <summary>
        /// code
        /// </summary>
        public string? code { get; set; }
        /// <summary>
        /// encryptedData
        /// </summary>
        public string? encryptedData { get; set; }
        /// <summary>
        /// iv
        /// </summary>
        public string? iv { get; set; }
        /// <summary>
        /// rawData
        /// </summary>
        public string? rawData { get; set; }
        /// <summary>
        /// signature
        /// </summary>
        public string? signature { get; set; }
        /// <summary>
        /// member_id
        /// </summary>
        public long member_id { get; set; }
    }
    /// <summary>
    /// 微信小程序用户信息结构
    /// </summary>
    public class WechatUserInfo
    {
        /// <summary>
        /// openId
        /// </summary>
        public string? openId { get; set; }
        /// <summary>
        /// nickName
        /// </summary>
        public string? nickName { get; set; }
        /// <summary>
        /// gender
        /// </summary>
        public string? gender { get; set; }
        /// <summary>
        /// city
        /// </summary>
        public string? city { get; set; }
        /// <summary>
        /// province
        /// </summary>
        public string? province { get; set; }
        /// <summary>
        /// country
        /// </summary>
        public string? country { get; set; }
        /// <summary>
        /// avatarUrl
        /// </summary>
        public string? avatarUrl { get; set; }
        /// <summary>
        /// unionId
        /// </summary>
        public string? unionId { get; set; }
        /// <summary>
        /// watermark
        /// </summary>
        public Watermark? watermark { get; set; }

        /// <summary>
        /// Watermark
        /// </summary>
        public class Watermark
        {
            /// <summary>
            /// appid
            /// </summary>
            public string? appid { get; set; }
            /// <summary>
            /// timestamp
            /// </summary>
            public string? timestamp { get; set; }
        }
    }
    /// <summary>
    /// 微信小程序从服务端获取的OpenId和SessionKey信息结构
    /// </summary>
    public class OpenIdAndSessionKey
    {
        /// <summary>
        /// openid
        /// </summary>
        public string? openid { get; set; }
        /// <summary>
        /// session_key
        /// </summary>
        public string? session_key { get; set; }
        /// <summary>
        /// errcode
        /// </summary>
        public string? errcode { get; set; }
        /// <summary>
        /// errmsg
        /// </summary>
        public string? errmsg { get; set; }
    }
}