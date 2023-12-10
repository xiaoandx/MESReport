using System;
using System.ComponentModel.DataAnnotations;

namespace MR.Models
{

    /// <summary>
    /// 内容摘要: 收货地址表 mr_member_addresses 实体类
    /// </summary>
    public class MemberAddresses
    {
        /// <summary>
        /// 收货地址表
        /// </summary>
        [Key]
        public int  address_id { get; set; }
        /// <summary>
        /// 会员
        /// </summary>
        public long  member_id { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string?  member_name { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string?  mobile_phone { get; set; }
        /// <summary>
        /// 省
        /// </summary>
        public int  province_id { get; set; }
        /// <summary>
        /// 市
        /// </summary>
        public int  city_id { get; set; }
        /// <summary>
        /// 区
        /// </summary>
        public int  district_id { get; set; }
        /// <summary>
        /// 详细地址
        /// </summary>
        public string?  address_detail { get; set; }
        /// <summary>
        /// 邮政编码
        /// </summary>
        public string?  zip_code { get; set; }
        /// <summary>
        /// 标签
        /// </summary>
        public int  address_tag { get; set; }
        /// <summary>
        /// 默认地址
        /// </summary>
        public bool  is_default { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string?  address_remark { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        [DataType(DataType.DateTime)]
        public DateTime  created_at { get; set; }

    }
}
