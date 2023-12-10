using System;
using System.ComponentModel.DataAnnotations;

namespace MR.Models
{

    /// <summary>
    /// 内容摘要: 对象 mr_objects 实体类
    /// </summary>
    public class Objects
    {
        /// <summary>
        /// 对象
        /// </summary>
        [Key]
        public int  object_id { get; set; }
        /// <summary>
        /// 对象名称
        /// </summary>
        public string?  object_name { get; set; }

    }
}
