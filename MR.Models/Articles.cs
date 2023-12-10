using System;
using System.ComponentModel.DataAnnotations;

namespace MR.Models
{

    /// <summary>
    /// 内容摘要: 文章 mr_articles 实体类
    /// </summary>
    public class Articles
    {
        /// <summary>
        /// 文章
        /// </summary>
        [Key]
        public long  article_id { get; set; }
        /// <summary>
        /// 多语言
        /// </summary>
        public int  lang_id { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string?  article_title { get; set; }
        /// <summary>
        /// 封面图
        /// </summary>
        public string?  cover_img { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string?  article_content { get; set; }
        /// <summary>
        /// 作者
        /// </summary>
        public string?  article_author { get; set; }
        /// <summary>
        /// 文章来源
        /// </summary>
        public string?  article_source { get; set; }
        /// <summary>
        /// 摘要
        /// </summary>
        public string?  article_summary { get; set; }
        /// <summary>
        /// 分类
        /// </summary>
        public int  type_id { get; set; }
        /// <summary>
        /// 子分类
        /// </summary>
        public int  type_subid { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int  status_id { get; set; }
        /// <summary>
        /// 审核状态
        /// </summary>
        public int  review_status_id { get; set; }
        /// <summary>
        /// 是否置顶
        /// </summary>
        public bool  is_top { get; set; }
        /// <summary>
        /// 是否显示
        /// </summary>
        public bool  is_show { get; set; }
        /// <summary>
        /// 发布人
        /// </summary>
        public long  user_id { get; set; }
        /// <summary>
        /// 访问量
        /// </summary>
        public int  visit_num { get; set; }
        /// <summary>
        /// 定时发布时间
        /// </summary>
        public string?  set_publish_time { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string?  article_remark { get; set; }
        /// <summary>
        /// 发布日期
        /// </summary>
        [DataType(DataType.DateTime)]
        public DateTime  created_at { get; set; }

    }
}
