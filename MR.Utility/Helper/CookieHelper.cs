using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace MR.Utility.Helper
{
    /// <summary>
    /// Cookie操作类
    /// 引用：Microsoft.AspNetCore.Mvc
    /// </summary>
    public class CookieHelper: Controller
    {
        /// <summary>
        /// 设置本地cookie
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>  
        /// <param name="days">过期时长，单位：分钟</param>      
        public void SetCookies(string key, string value, int days = 1)
        {
            HttpContext.Response.Cookies.Append(key, value, new CookieOptions
            {
                Expires = DateTime.Now.AddDays(days)
            });
        }

        /// <summary>
        /// 获取cookies
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>返回对应的值</returns>
        public string GetCookies(string key)
        {
            HttpContext.Request.Cookies.TryGetValue(key, out string value);
            if (string.IsNullOrEmpty(value))
            {
                value = string.Empty;
            }
            return value;
        }

        /// <summary>
        /// 删除指定的cookie
        /// </summary>
        /// <param name="key">键</param>
        public void DeleteCookies(string key)
        {
            HttpContext.Response.Cookies.Delete(key);
        }
    }
}
