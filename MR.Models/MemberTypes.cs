using System;
using System.ComponentModel.DataAnnotations;

namespace MR.Models
{

    /// <summary>
    /// 内容摘要: 会员类型 mr_member_types 实体类
    /// </summary>
    public class MemberTypes
    {
        /// <summary>
        /// 会员类型
        /// </summary>
        [Key]
        public int  type_id { get; set; }
        /// <summary>
        /// 会员类型名称
        /// </summary>
        public string?  type_name { get; set; }

    }
}
