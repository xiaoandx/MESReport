using System;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using MR.Utility.Config;

namespace MR.Utility.Authorization
{
    /// <summary>
    /// TokenAuthConfiguration
    /// </summary>
    public static class TokenAuthConfiguration {

        /// <summary>
        /// 发行人
        /// </summary>
        public static string? Issuer { get; }

        /// <summary>
        /// 订阅人
        /// </summary>
        public static string? Audience { get; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public static TimeSpan Expiration { get; }

        /// <summary>
        /// 安全密钥
        /// </summary>
        public static SecurityKey? IssuerSigningKey { get; }

        /// <summary>
        /// 签名验证
        /// </summary>
        public static SigningCredentials? SigningCredentials { get; }

        /// <summary>
        /// TokenAuthConfiguration
        /// </summary>
        static TokenAuthConfiguration() {
            var section = CfgManager.Configuration!.GetSection("Authentication:JwtBearer");
            if (section != null)
            {
                Issuer = section["Issuer"];
                Audience = section["Audience"];
                Expiration = TimeSpan.FromHours(Double.Parse(section["Expiration"]!));
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(section["SecurityKey"]!));
                SigningCredentials = new SigningCredentials(IssuerSigningKey, SecurityAlgorithms.HmacSha256);
            }
            
        }
    }
}
