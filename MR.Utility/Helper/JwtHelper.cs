using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JWT;
using JWT.Algorithms;
using JWT.Exceptions;
using JWT.Serializers;

namespace MR.Utility.Helper
{
    /// <summary>
    /// JSON Web Tokens 封装类
    /// </summary>
    public class JwtHelper
    {
        [Obsolete]
        static IJwtAlgorithm algorithm = new HMACSHA256Algorithm();//HMACSHA256加密
        static IJsonSerializer serializer = new JsonNetSerializer();//序列化和反序列
        static IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();//Base64编解码
        static IDateTimeProvider provider = new UtcDateTimeProvider();//UTC时间获取

        const string secret = "11a80451c48c8c05d7d35e8303af39d9";//秘钥-cloudin-dhls-zgsw-api

        /// <summary>
        /// 创建JWT
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        [Obsolete]
        public static string CreateJWT(Dictionary<string, object> payload)
        {
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);
            return encoder.Encode(payload, secret);
        }

        /// <summary>
        /// 校验JWT
        /// </summary>
        /// <param name="token"></param>
        /// <param name="payload"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        [Obsolete]
        public static bool ValidateJWT(string token, out string payload, out string message)
        {
            bool isValidted = false;
            payload = "";
            try
            {
                IJwtValidator validator = new JwtValidator(serializer, provider);//用于验证JWT的类
                IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder, algorithm);//用于解析JWT的类
                payload = decoder.Decode(token, secret, verify: true);

                isValidted = true;

                message = "验证成功";
            }
            catch (TokenExpiredException)//当前时间大于负载过期时间（负荷中的exp），会引发Token过期异常
            {
                message = "过期了！";
            }
            catch (SignatureVerificationException)//如果签名不匹配，引发签名验证异常
            {
                message = "签名错误！";
            }
            return isValidted;
        }
    }
}

