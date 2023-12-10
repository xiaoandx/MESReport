using System;
using System.ComponentModel.DataAnnotations;

namespace MR.Models
{

    /// <summary>
    /// 内容摘要: 点赞 mr_member_likes 实体类
    /// </summary>
    public class MemberLikes
    {
        /// <summary>
        /// 点赞
        /// </summary>
        [Key]
        public long  like_id { get; set; }
        /// <summary>
        /// 多语言
        /// </summary>
        public int  lang_id { get; set; }
        /// <summary>
        /// 会员
        /// </summary>
        public long  member_id { get; set; }
        /// <summary>
        /// 点赞对象类型
        /// </summary>
        public int  object_type_id { get; set; }
        /// <summary>
        /// 点赞对象
        /// </summary>
        public long  object_id { get; set; }
        /// <summary>
        /// 签到平台
        /// </summary>
        public int  platform_id { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string?  like_remark { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        [DataType(DataType.DateTime)]
        public DateTime  created_at { get; set; }

    }
}
