using System;
using System.ComponentModel.DataAnnotations;

namespace MR.Models
{

    /// <summary>
    /// 内容摘要: 用户 mr_users 实体类
    /// </summary>
    public class Users
    {
        /// <summary>
        /// 用户
        /// </summary>
        [Key]
        public long  user_id { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string?  user_name { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string?  user_avatar { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string?  user_password { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string?  nick_name { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string?  real_name { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string?  mobile_phone { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        public string?  user_email { get; set; }
        /// <summary>
        /// 职位
        /// </summary>
        public string?  user_job { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public int  gender_id { get; set; }
        /// <summary>
        /// 角色
        /// </summary>
        public int  role_id { get; set; }
        /// <summary>
        /// 签名
        /// </summary>
        public string?  user_slogan { get; set; }
        /// <summary>
        /// 介绍
        /// </summary>
        public string?  user_intro { get; set; }
        /// <summary>
        /// 限制IP段登录
        /// </summary>
        public string?  limit_ip { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public bool  user_status { get; set; }
        /// <summary>
        /// TOKEN
        /// </summary>
        public string?  user_token { get; set; }
        /// <summary>
        /// 最后登录时间
        /// </summary>
        [DataType(DataType.DateTime)]
        public DateTime  last_login_at { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        [DataType(DataType.DateTime)]
        public DateTime  created_at { get; set; }

    }
}
