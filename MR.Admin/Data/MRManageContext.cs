using System;
using Microsoft.EntityFrameworkCore;

namespace MR.Manage.Data
{

    /// <summary>
    /// 内容摘要: MRManageContext 数据库上下文类,EF Core n
    /// </summary>
    public class MRManageContext : DbContext
    {
        /// <summary>
        /// 数据库上下文
        /// </summary>
        public MRManageContext(DbContextOptions<MRManageContext> options): base(options)
        {
           // 有新增数据表时，必须覆盖此类
        }
        
        
        /// <summary>
        /// 广告
        /// </summary>
        public DbSet<MR.Models.Ads> mr_ads { get; set; }
        
        /// <summary>
        /// 链接类型
        /// </summary>
        public DbSet<MR.Models.AdsLinks> mr_ads_links { get; set; }
        
        /// <summary>
        /// 广告位置
        /// </summary>
        public DbSet<MR.Models.AdsPositions> mr_ads_positions { get; set; }
        
        /// <summary>
        /// 区域
        /// </summary>
        public DbSet<MR.Models.Areas> mr_areas { get; set; }
        
        /// <summary>
        /// 文章状态
        /// </summary>
        public DbSet<MR.Models.ArticleStatus> mr_article_status { get; set; }
        
        /// <summary>
        /// 文章分类
        /// </summary>
        public DbSet<MR.Models.ArticleTypes> mr_article_types { get; set; }
        
        /// <summary>
        /// 文章
        /// </summary>
        public DbSet<MR.Models.Articles> mr_articles { get; set; }
        
        /// <summary>
        /// 附件类型
        /// </summary>
        public DbSet<MR.Models.AttachmentTypes> mr_attachment_types { get; set; }
        
        /// <summary>
        /// 附件
        /// </summary>
        public DbSet<MR.Models.Attachments> mr_attachments { get; set; }
        
        /// <summary>
        /// 验证码
        /// </summary>
        public DbSet<MR.Models.Codes> mr_codes { get; set; }
        
        /// <summary>
        /// 国家
        /// </summary>
        public DbSet<MR.Models.Country> mr_country { get; set; }
        
        /// <summary>
        /// 活动报名
        /// </summary>
        public DbSet<MR.Models.EventEnroll> mr_event_enroll { get; set; }
        
        /// <summary>
        /// 签到
        /// </summary>
        public DbSet<MR.Models.EventSigns> mr_event_signs { get; set; }
        
        /// <summary>
        /// 活动类型
        /// </summary>
        public DbSet<MR.Models.EventTypes> mr_event_types { get; set; }
        
        /// <summary>
        /// 活动
        /// </summary>
        public DbSet<MR.Models.Events> mr_events { get; set; }
        
        /// <summary>
        /// 活动图片
        /// </summary>
        public DbSet<MR.Models.EventsPhotos> mr_events_photos { get; set; }
        
        /// <summary>
        /// 意见反馈
        /// </summary>
        public DbSet<MR.Models.Feedback> mr_feedback { get; set; }
        
        /// <summary>
        /// 意见类别表
        /// </summary>
        public DbSet<MR.Models.FeedbackTypes> mr_feedback_types { get; set; }
        
        /// <summary>
        /// 性别
        /// </summary>
        public DbSet<MR.Models.Genders> mr_genders { get; set; }
        
        /// <summary>
        /// 多语言
        /// </summary>
        public DbSet<MR.Models.Languages> mr_languages { get; set; }
        
        /// <summary>
        /// 日志类型表
        /// </summary>
        public DbSet<MR.Models.LogTypes> mr_log_types { get; set; }
        
        /// <summary>
        /// 日志
        /// </summary>
        public DbSet<MR.Models.Logs> mr_logs { get; set; }
        
        /// <summary>
        /// 收货地址表
        /// </summary>
        public DbSet<MR.Models.MemberAddresses> mr_member_addresses { get; set; }
        
        /// <summary>
        /// 评论
        /// </summary>
        public DbSet<MR.Models.MemberComments> mr_member_comments { get; set; }
        
        /// <summary>
        /// 收藏
        /// </summary>
        public DbSet<MR.Models.MemberFavorites> mr_member_favorites { get; set; }
        
        /// <summary>
        /// 浏览商品记录
        /// </summary>
        public DbSet<MR.Models.MemberHistories> mr_member_histories { get; set; }
        
        /// <summary>
        /// 会员等级
        /// </summary>
        public DbSet<MR.Models.MemberLevels> mr_member_levels { get; set; }
        
        /// <summary>
        /// 点赞
        /// </summary>
        public DbSet<MR.Models.MemberLikes> mr_member_likes { get; set; }
        
        /// <summary>
        /// 签到
        /// </summary>
        public DbSet<MR.Models.MemberSigns> mr_member_signs { get; set; }
        
        /// <summary>
        /// 会员类型
        /// </summary>
        public DbSet<MR.Models.MemberTypes> mr_member_types { get; set; }
        
        /// <summary>
        /// 会员
        /// </summary>
        public DbSet<MR.Models.Members> mr_members { get; set; }
        
        /// <summary>
        /// 菜单
        /// </summary>
        public DbSet<MR.Models.Menus> mr_menus { get; set; }
        
        /// <summary>
        /// 阅读记录表
        /// </summary>
        public DbSet<MR.Models.MessageRead> mr_message_read { get; set; }
        
        /// <summary>
        /// 消息类型
        /// </summary>
        public DbSet<MR.Models.MessageTypes> mr_message_types { get; set; }
        
        /// <summary>
        /// 消息
        /// </summary>
        public DbSet<MR.Models.Messages> mr_messages { get; set; }
        
        /// <summary>
        /// 对象
        /// </summary>
        public DbSet<MR.Models.Objects> mr_objects { get; set; }
        
        /// <summary>
        /// 支付方式表
        /// </summary>
        public DbSet<MR.Models.Paymethod> mr_paymethod { get; set; }
        
        /// <summary>
        /// 平台
        /// </summary>
        public DbSet<MR.Models.Platforms> mr_platforms { get; set; }
        
        /// <summary>
        /// 会员积分
        /// </summary>
        public DbSet<MR.Models.PointMembers> mr_point_members { get; set; }
        
        /// <summary>
        /// 积分记录
        /// </summary>
        public DbSet<MR.Models.PointRecords> mr_point_records { get; set; }
        
        /// <summary>
        /// 积分
        /// </summary>
        public DbSet<MR.Models.Points> mr_points { get; set; }
        
        /// <summary>
        /// 审核状态
        /// </summary>
        public DbSet<MR.Models.ReviewStatus> mr_review_status { get; set; }
        
        /// <summary>
        /// 审核类型
        /// </summary>
        public DbSet<MR.Models.ReviewTypes> mr_review_types { get; set; }
        
        /// <summary>
        /// 审核意见
        /// </summary>
        public DbSet<MR.Models.Reviews> mr_reviews { get; set; }
        
        /// <summary>
        /// 搜索关键字记录
        /// </summary>
        public DbSet<MR.Models.SearchRecords> mr_search_records { get; set; }
        
        /// <summary>
        /// 搜索类型
        /// </summary>
        public DbSet<MR.Models.SearchTypes> mr_search_types { get; set; }
        
        /// <summary>
        /// 基本信息
        /// </summary>
        public DbSet<MR.Models.SettingBases> mr_setting_bases { get; set; }
        
        /// <summary>
        /// 短信
        /// </summary>
        public DbSet<MR.Models.Sms> mr_sms { get; set; }
        
        /// <summary>
        /// 角色
        /// </summary>
        public DbSet<MR.Models.UserRoles> mr_user_roles { get; set; }
        
        /// <summary>
        /// 用户
        /// </summary>
        public DbSet<MR.Models.Users> mr_users { get; set; }
        
        /// <summary>
        /// 推广媒介
        /// </summary>
        public DbSet<MR.Models.UtmMediums> mr_utm_mediums { get; set; }
        
        /// <summary>
        /// 推广来源
        /// </summary>
        public DbSet<MR.Models.UtmSources> mr_utm_sources { get; set; }
        
        /// <summary>
        /// 版本
        /// </summary>
        public DbSet<MR.Models.Versions> mr_versions { get; set; }
        
        /// <summary>
        /// 访问频道
        /// </summary>
        public DbSet<MR.Models.VisitChannels> mr_visit_channels { get; set; }
        
        /// <summary>
        /// 访问量
        /// </summary>
        public DbSet<MR.Models.Visits> mr_visits { get; set; }

    }
}
