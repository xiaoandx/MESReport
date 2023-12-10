using System;
using System.ComponentModel.DataAnnotations;

namespace MR.Models
{

    /// <summary>
    /// 内容摘要: 菜单 mr_menus 实体类
    /// </summary>
    public class Menus
    {
        /// <summary>
        /// 菜单
        /// </summary>
        [Key]
        public int  menu_id { get; set; }
        /// <summary>
        /// 菜单名称
        /// </summary>
        public string?  menu_name { get; set; }
        /// <summary>
        /// 图片
        /// </summary>
        public string?  menu_icon { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string?  menu_url { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int  menu_rank { get; set; }
        /// <summary>
        /// 父目录
        /// </summary>
        public int  parent_id { get; set; }
        /// <summary>
        /// 菜单代码
        /// </summary>
        public int  menu_code { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public bool  menu_status { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string?  menu_remark { get; set; }

    }
}
