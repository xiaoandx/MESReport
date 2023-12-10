using System;
using System.ComponentModel.DataAnnotations;

namespace MR.Models
{

    /// <summary>
    /// 内容摘要: 附件类型 mr_attachment_types 实体类
    /// </summary>
    public class AttachmentTypes
    {
        /// <summary>
        /// 附件类型
        /// </summary>
        [Key]
        public int  type_id { get; set; }
        /// <summary>
        /// 附件类型名称
        /// </summary>
        public string?  type_name { get; set; }

    }
}
