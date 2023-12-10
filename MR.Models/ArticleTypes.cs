using System;
using System.ComponentModel.DataAnnotations;

namespace MR.Models
{

    /// <summary>
    /// 内容摘要: 文章分类 mr_article_types 实体类
    /// </summary>
    public class ArticleTypes
    {
        /// <summary>
        /// 文章分类
        /// </summary>
        [Key]
        public int  type_id { get; set; }
        /// <summary>
        /// 分类名称
        /// </summary>
        public string?  type_name { get; set; }
        /// <summary>
        /// 父目录
        /// </summary>
        public int  parent_id { get; set; }
        /// <summary>
        /// 是否显示
        /// </summary>
        public bool  is_show { get; set; }

    }
}
