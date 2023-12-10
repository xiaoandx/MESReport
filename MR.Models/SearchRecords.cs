using System;
using System.ComponentModel.DataAnnotations;

namespace MR.Models
{

    /// <summary>
    /// 内容摘要: 搜索关键字记录 mr_search_records 实体类
    /// </summary>
    public class SearchRecords
    {
        /// <summary>
        /// 搜索关键字记录
        /// </summary>
        [Key]
        public long  record_id { get; set; }
        /// <summary>
        /// 多语言
        /// </summary>
        public int  lang_id { get; set; }
        /// <summary>
        /// 搜索关键字
        /// </summary>
        public string?  search_keyword { get; set; }
        /// <summary>
        /// 搜索结果数量
        /// </summary>
        public int  result_num { get; set; }
        /// <summary>
        /// 会员
        /// </summary>
        public long  member_id { get; set; }
        /// <summary>
        /// 搜索类型
        /// </summary>
        public int  type_id { get; set; }
        /// <summary>
        /// 客户端IP
        /// </summary>
        public string?  client_ip { get; set; }
        /// <summary>
        /// 客户端信息
        /// </summary>
        public string?  client_info { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string?  record_remark { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        [DataType(DataType.DateTime)]
        public DateTime  created_at { get; set; }

    }
}
