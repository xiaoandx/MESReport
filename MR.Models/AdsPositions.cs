using System;
using System.ComponentModel.DataAnnotations;

namespace MR.Models
{

    /// <summary>
    /// 内容摘要: 广告位置 mr_ads_positions 实体类
    /// </summary>
    public class AdsPositions
    {
        /// <summary>
        /// 广告位置
        /// </summary>
        [Key]
        public int  position_id { get; set; }
        /// <summary>
        /// 广告位置名称
        /// </summary>
        public string?  position_name { get; set; }

    }
}
