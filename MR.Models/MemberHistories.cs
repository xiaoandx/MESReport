using System;
using System.ComponentModel.DataAnnotations;

namespace MR.Models
{

    /// <summary>
    /// 内容摘要: 浏览商品记录 mr_member_histories 实体类
    /// </summary>
    public class MemberHistories
    {
        /// <summary>
        /// 浏览商品记录
        /// </summary>
        [Key]
        public long  history_id { get; set; }
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
        /// 浏览页面URL
        /// </summary>
        public string?  page_url { get; set; }
        /// <summary>
        /// 用户IP
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
        public string?  history_remark { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        [DataType(DataType.DateTime)]
        public DateTime  created_at { get; set; }

    }
}
