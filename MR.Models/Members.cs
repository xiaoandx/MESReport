using System;
using System.ComponentModel.DataAnnotations;

namespace MR.Models
{

    /// <summary>
    /// 内容摘要: 会员 mr_members 实体类
    /// </summary>
    public class Members
    {
        /// <summary>
        /// 会员
        /// </summary>
        [Key]
        public long  member_id { get; set; }
        /// <summary>
        /// 微信OPEN
        /// </summary>
        public string?  open_id { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string?  member_account { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string?  mobile_phone { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string?  member_password { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string?  member_name { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string?  real_name { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public int  gender_id { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string?  member_avatar { get; set; }
        /// <summary>
        /// 生日
        /// </summary>
        public string?  member_birth { get; set; }
        /// <summary>
        /// 职位
        /// </summary>
        public string?  member_job { get; set; }
        /// <summary>
        /// 邀请人
        /// </summary>
        public long  invite_member_id { get; set; }
        /// <summary>
        /// 邀请码
        /// </summary>
        public string?  invite_code { get; set; }
        /// <summary>
        /// 分享二维码图片
        /// </summary>
        public string?  qrcode_img { get; set; }
        /// <summary>
        /// 推广来源
        /// </summary>
        public int  source_id { get; set; }
        /// <summary>
        /// 媒介
        /// </summary>
        public int  medium_id { get; set; }
        /// <summary>
        /// 简介
        /// </summary>
        public string?  member_intro { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public bool  member_status { get; set; }
        /// <summary>
        /// 会员角色
        /// </summary>
        public int  type_id { get; set; }
        /// <summary>
        /// 国家
        /// </summary>
        public string?  country_name { get; set; }
        /// <summary>
        /// 省
        /// </summary>
        public string?  province_name { get; set; }
        /// <summary>
        /// 市
        /// </summary>
        public string?  city_name { get; set; }
        /// <summary>
        /// 区/县/旗
        /// </summary>
        public string?  district_name { get; set; }
        /// <summary>
        /// TOKEN
        /// </summary>
        public string?  member_token { get; set; }
        /// <summary>
        /// 加密密钥
        /// </summary>
        public string?  encrypt_token { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string?  member_remark { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        [DataType(DataType.DateTime)]
        public DateTime  created_at { get; set; }

    }
}
