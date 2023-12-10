using System;
using System.ComponentModel.DataAnnotations;

namespace MR.Models
{

    /// <summary>
    /// 内容摘要: 搜索类型 mr_search_types 实体类
    /// </summary>
    public class SearchTypes
    {
        /// <summary>
        /// 搜索类型
        /// </summary>
        [Key]
        public int  type_id { get; set; }
        /// <summary>
        /// 搜索类型名称
        /// </summary>
        public string?  type_name { get; set; }

    }
}
