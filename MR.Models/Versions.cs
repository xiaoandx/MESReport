using System;
using System.ComponentModel.DataAnnotations;

namespace MR.Models
{

    /// <summary>
    /// 内容摘要: 版本 mr_versions 实体类
    /// </summary>
    public class Versions
    {
        /// <summary>
        /// 版本
        /// </summary>
        [Key]
        public int  version_id { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public int  version_code { get; set; }
        /// <summary>
        /// 版本名称
        /// </summary>
        public string?  version_name { get; set; }
        /// <summary>
        /// 版本信息
        /// </summary>
        public string?  version_info { get; set; }
        /// <summary>
        /// 是否强制更新
        /// </summary>
        public bool  force_update { get; set; }
        /// <summary>
        /// 下载链接
        /// </summary>
        public string?  download_url { get; set; }
        /// <summary>
        /// 客户端类型
        /// </summary>
        public int  version_type { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string?  version_remark { get; set; }
        /// <summary>
        /// 是否显示
        /// </summary>
        public bool  is_show { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        [DataType(DataType.DateTime)]
        public DateTime  created_at { get; set; }

    }
}
