using System;
using System.ComponentModel.DataAnnotations;

namespace MR.Models
{

    /// <summary>
    /// 内容摘要: 收藏 mr_member_favorites 实体类
    /// </summary>
    public class MemberFavorites
    {
        /// <summary>
        /// 收藏
        /// </summary>
        [Key]
        public long  favorite_id { get; set; }
        /// <summary>
        /// 多语言
        /// </summary>
        public int  lang_id { get; set; }
        /// <summary>
        /// 会员
        /// </summary>
        public long  member_id { get; set; }
        /// <summary>
        /// 收藏对象类型
        /// </summary>
        public int  object_type_id { get; set; }
        /// <summary>
        /// 收藏对象
        /// </summary>
        public long  object_id { get; set; }
        /// <summary>
        /// 客户端IP
        /// </summary>
        public string?  client_ip { get; set; }
        /// <summary>
        /// 客户端信息
        /// </summary>
        public string?  client_info { get; set; }
        /// <summary>
        /// 签到平台
        /// </summary>
        public int  platform_id { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string?  favorite_remark { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [DataType(DataType.DateTime)]
        public DateTime  created_at { get; set; }

    }
}
