using System;
using System.ComponentModel.DataAnnotations;

namespace MR.Models
{

    /// <summary>
    /// 内容摘要: 文章状态 mr_article_status 实体类
    /// </summary>
    public class ArticleStatus
    {
        /// <summary>
        /// 文章状态
        /// </summary>
        [Key]
        public int  status_id { get; set; }
        /// <summary>
        /// 状态名称
        /// </summary>
        public string?  status_name { get; set; }

    }
}
