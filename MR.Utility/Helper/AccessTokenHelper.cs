using System;
using System.Collections.Generic;

namespace MR.Utility.Helper
{
    /// <summary>
    /// TOKEN类
    /// </summary>
    public class AccessTokenHelper
    {
        /// <summary>
        /// 生成TOKEN
        /// </summary>
        /// <param name="account">账号</param>
        /// <returns></returns>
        [Obsolete]
        public static string GetToken(string account)
        {
            var iat = DateTimeOffset.UtcNow.ToUnixTimeSeconds();//当前时间的unix时间戳（秒），整型；
            var exp = DateTimeOffset.UtcNow.AddSeconds(3600 * 24 * 7).ToUnixTimeSeconds();//通常设置10分钟有效，即exp=iat+600，注意不少于当前时间且不超过当前时间60分钟；

            //载荷（payload）
            var payload = new Dictionary<string, object>
                    {
                        { "iat", iat },//发布时间 
                        { "exp", exp },//到期时间
                        { "account",account}
                    };
            //生成JWT
            string JWTString = JwtHelper.CreateJWT(payload);
            //log.Debug("GetAccessToken="+ JWTString);

            //校验JWT
            string result_message;//需要解析的消息
            string init_payload;//获取负载
            if (JwtHelper.ValidateJWT(JWTString, out init_payload, out result_message))
            {
                //log.Debug("init_payload=" + init_payload);
            }

            return JWTString;
        }
    }
}
