using System;
using System.ComponentModel.DataAnnotations;

namespace MR.Models
{

    /// <summary>
    /// 内容摘要: 通用实体类
    /// </summary>
    public class CommonParas
    {
        /// <summary>
        /// 分类ID
        /// </summary>
        public int category_id { get; set; }

        /// <summary>
        /// 子分类ID
        /// </summary>
        public int category_sub_id { get; set; }

        /// <summary>
        /// 关键字
        /// </summary>
        public string? keywords { get; set; }

        /// <summary>
        /// 页码
        /// </summary>
        public int page { get; set; }

        /// <summary>
        /// 每页读取条数
        /// </summary>
        public int limit { get; set; }

        /// <summary>
        /// 排序条件
        /// </summary>
        public string? order { get; set; }

        /// <summary>
        /// 会员ID
        /// </summary>
        public int member_id { get; set; }

        /// <summary>
        /// 店铺ID
        /// </summary>
        public int store_id { get; set; }

        /// <summary>
        /// IDS
        /// </summary>
        public string? ids { get; set; }

    }
}
