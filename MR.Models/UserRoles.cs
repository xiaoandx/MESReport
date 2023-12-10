using System;
using System.ComponentModel.DataAnnotations;

namespace MR.Models
{

    /// <summary>
    /// 内容摘要: 角色 mr_user_roles 实体类
    /// </summary>
    public class UserRoles
    {
        /// <summary>
        /// 角色
        /// </summary>
        [Key]
        public int  role_id { get; set; }
        /// <summary>
        /// 角色名称
        /// </summary>
        public string?  role_name { get; set; }
        /// <summary>
        /// 角色权限
        /// </summary>
        public string?  role_authority { get; set; }
        /// <summary>
        /// 拥有权限
        /// </summary>
        public string?  role_desc { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string?  role_remark { get; set; }

    }
}
