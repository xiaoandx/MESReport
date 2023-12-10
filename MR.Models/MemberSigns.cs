using System;
using System.ComponentModel.DataAnnotations;

namespace MR.Models
{

    /// <summary>
    /// 内容摘要: 签到 mr_member_signs 实体类
    /// </summary>
    public class MemberSigns
    {
        /// <summary>
        /// 签到
        /// </summary>
        [Key]
        public long  sign_id { get; set; }
        /// <summary>
        /// 多语言
        /// </summary>
        public int  lang_id { get; set; }
        /// <summary>
        /// 会员
        /// </summary>
        public long  member_id { get; set; }
        /// <summary>
        /// 签到平台
        /// </summary>
        public int  platform_id { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string?  sign_remark { get; set; }
        /// <summary>
        /// 签到日期
        /// </summary>
        [DataType(DataType.DateTime)]
        public DateTime  created_at { get; set; }

    }
}
