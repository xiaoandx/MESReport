using System;
using System.ComponentModel.DataAnnotations;

namespace MR.Models
{

    /// <summary>
    /// 内容摘要: 附件 mr_attachments 实体类
    /// </summary>
    public class Attachments
    {
        /// <summary>
        /// 附件
        /// </summary>
        [Key]
        public long  attachment_id { get; set; }
        /// <summary>
        /// 对象
        /// </summary>
        public long  object_id { get; set; }
        /// <summary>
        /// 附件类型
        /// </summary>
        public int  type_id { get; set; }
        /// <summary>
        /// 附件名称
        /// </summary>
        public string?  attachment_name { get; set; }
        /// <summary>
        /// 附件路径
        /// </summary>
        public string?  attachment_path { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string?  attachment_remark { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        [DataType(DataType.DateTime)]
        public DateTime  created_at { get; set; }

    }
}
