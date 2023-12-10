using System;
using System.ComponentModel.DataAnnotations;

namespace MR.Models
{

    /// <summary>
    /// 内容摘要: 通用用户登录实体类
    /// </summary>
    public class LoginParas
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string? user_name { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string? user_password { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string? mobile { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        public string? captcha { get; set; }

        /// <summary>
        /// TOKEN
        /// </summary>
        public string? token { get; set; }

        /// <summary>
        /// 多语言
        /// </summary>
        public int lang_id { get; set; }

        /// <summary>
        /// 邀请人ID
        /// </summary>
        public int invite_member_id { get; set; }

        /// <summary>
        /// 来源终端
        /// </summary>
        public string? platform { get; set; }

    }
}
