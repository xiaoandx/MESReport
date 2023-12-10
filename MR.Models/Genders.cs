using System;
using System.ComponentModel.DataAnnotations;

namespace MR.Models
{

    /// <summary>
    /// 内容摘要: 性别 mr_genders 实体类
    /// </summary>
    public class Genders
    {
        /// <summary>
        /// 性别
        /// </summary>
        [Key]
        public int  gender_id { get; set; }
        /// <summary>
        /// 性别名称
        /// </summary>
        public string?  gender_name { get; set; }

    }
}
