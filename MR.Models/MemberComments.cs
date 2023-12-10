using System;
using System.ComponentModel.DataAnnotations;

namespace MR.Models
{

    /// <summary>
    /// 内容摘要: 评论 mr_member_comments 实体类
    /// </summary>
    public class MemberComments
    {
        /// <summary>
        /// 评论
        /// </summary>
        [Key]
        public long  comment_id { get; set; }
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
        /// 对象
        /// </summary>
        public long  object_id { get; set; }
        /// <summary>
        /// 评星
        /// </summary>
        public int  star_num { get; set; }
        /// <summary>
        /// 评论内容
        /// </summary>
        public string?  comment_content { get; set; }
        /// <summary>
        /// 客户端IP
        /// </summary>
        public string?  client_ip { get; set; }
        /// <summary>
        /// 客户端信息
        /// </summary>
        public string?  client_info { get; set; }
        /// <summary>
        /// 是否显示
        /// </summary>
        public bool  is_show { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        public bool  is_del { get; set; }
        /// <summary>
        /// 签到平台
        /// </summary>
        public int  platform_id { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string?  comment_remark { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        [DataType(DataType.DateTime)]
        public DateTime  created_at { get; set; }

    }
}
