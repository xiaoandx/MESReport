using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MR.Manage.Data;
using MR.Models;
using MR.Utility.Helper;
using Senparc.CO2NET.Extensions;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace MR.Manage.Controllers
{

    /// <summary>
    /// 内容摘要: 会员处理类
    /// </summary>
    [Authorize]
    public class MembersController : BaseController<MembersController>
    {

        /// <summary>
        /// 会员 实例化数据上下文
        /// </summary>
        /// <param name="context"></param>
        public MembersController(MRManageContext context) : base(context)
        {

        }

        /// <summary>
        /// 管理/列表页面
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 新增页面
        /// </summary>
        /// <returns></returns>
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// 编辑页面
        /// </summary>
        /// <returns></returns>
        public IActionResult Edit()
        {
            return View();
        }

        /// <summary>
        /// 详情页面
        /// </summary>
        /// <returns></returns>
        public IActionResult Views()
        {
            return View();
        }

        /// <summary>
        /// 统计页面
        /// </summary>
        /// <returns></returns>
        public IActionResult Chart()
        {
            return View();
        }

        /// <summary>
        /// 获取会员列表数据
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="limit">每页显示数量</param>
        /// <param name="created_at">创建日期</param>
        /// <param name="mobile_phone">手机号</param>
        /// <param name="member_name">昵称</param>
        /// <param name="invite_code">邀请码</param>
        /// <param name="source_id">推广来源</param>
        /// <param name="type_id">会员角色</param>
        /// <returns>返回会员JSON数据集合</returns>
        public async Task<IActionResult> List(int page, int limit, string created_at, string mobile_phone, string member_name, string invite_code, int source_id = -1, int type_id = -1)
        {
            try
            {
                var result = from item in _context.Set<Members>()
                             select new
                             {
                                 item.member_id,
                                 item.open_id,
                                 item.member_account,
                                 item.mobile_phone,
                                 item.member_password,
                                 item.member_name,
                                 item.real_name,
                                 item.gender_id,
                                 gender_name = item.gender_id > -1 ? _context.mr_genders.FirstOrDefault(t => t.gender_id == item.gender_id)!.gender_name : "-",
                                 item.member_avatar,
                                 show_member_avatar = string.IsNullOrEmpty(item.member_avatar) ? "/images/logo.png" : MR.Manage.Filters.AppSettingsFilter.GetSetting("Aliyun:OSS_URL") + item.member_avatar,
                                 item.member_birth,
                                 item.member_job,
                                 item.invite_member_id,
                                 item.invite_code,
                                 item.qrcode_img,
                                 item.source_id,
                                 source_name = item.source_id > -1 ? _context.mr_utm_sources.FirstOrDefault(t => t.source_id == item.source_id)!.source_name : "-",
                                 item.medium_id,
                                 medium_name = item.medium_id > -1 ? _context.mr_utm_mediums.FirstOrDefault(t => t.medium_id == item.medium_id)!.medium_name : "-",
                                 item.member_intro,
                                 item.member_status,
                                 item.type_id,
                                 type_name = item.type_id > -1 ? _context.mr_member_types.FirstOrDefault(t => t.type_id == item.type_id)!.type_name : "-",
                                 item.country_name,
                                 item.province_name,
                                 item.city_name,
                                 item.district_name,
                                 item.member_token,
                                 item.encrypt_token,
                                 item.member_remark,
                                 item.created_at,
                                 format_time = item.created_at.ToString("yyyy-MM-dd HH:mm"),
                                 register_time = item.created_at.ToString("MM/dd")
                             };

                if (!string.IsNullOrEmpty(mobile_phone))
                {
                    result = result.Where(t => t.mobile_phone.Contains(mobile_phone.Trim()));
                }
                if (!string.IsNullOrEmpty(member_name))
                {
                    result = result.Where(t => t.member_name.Contains(member_name.Trim()));
                }
                if (!string.IsNullOrEmpty(invite_code))
                {
                    result = result.Where(t => t.invite_code.Contains(invite_code.Trim()));
                }
                if (source_id > 0)
                {
                    result = result.Where(t => t.source_id == source_id);
                }
                if (type_id > 0)
                {
                    result = result.Where(t => t.type_id == type_id);
                }

                if (!string.IsNullOrEmpty(created_at))
                {
                    created_at = created_at.Replace(" - ", ",");
                    string[] splitDate = created_at.Split(new char[] { ',' });
                    string start_at = splitDate[0];
                    string end_at = splitDate[1];
                    result = result.Where(t => t.created_at >= Convert.ToDateTime(start_at) && t.created_at <= Convert.ToDateTime(end_at).AddDays(1));
                }

                var list = await result.OrderByDescending(t => t.created_at).Skip((page - 1) * limit).Take(limit).ToListAsync();
                var lCount = result.LongCount();
                AddLogs((int)ENUMHelper.LogType.Search, (int)ENUMHelper.Platform.Admin + "" + (int)ENUMHelper.OperateType.List + "" + (int)ENUMHelper.InfoType.Info, "查询会员LINQ=" + result.Expression.ToString() + ",DATA=" + list.ToJson());
                if (lCount <= 0)
                {
                    return Json(new { code = 0, msg = "暂无数据", count = 0 });
                }
                else
                {
                    return Json(new { code = 0, msg = "success", data = list, count = lCount });
                }
            }
            catch (Exception ex)
            {
                log.Fatal(ex, " -> List");
                return Json(new { code = 0, errcode = (int)ENUMHelper.ExceptionType.List, errmsg = ex.Message });
            }
        }

        /// <summary>
        /// 获取会员数据
        /// </summary>
        /// <returns>返回会员JSON数据集合</returns>
        public async Task<IActionResult> Select()
        {
            try
            {
                var list = await _context.mr_members.OrderBy(t => t.member_id).ToListAsync();
                AddLogs((int)ENUMHelper.LogType.Search, (int)ENUMHelper.Platform.Admin + "" + (int)ENUMHelper.OperateType.Select + "" + (int)ENUMHelper.InfoType.Info, "查询会员下拉填充,DATA=" + list.ToJson());
                return Json(new { code = 0, msg = "success", data = list });
            }
            catch (Exception ex)
            {
                log.Fatal(ex, " -> Select");
                return Json(new { code = 0, errcode = (int)ENUMHelper.ExceptionType.Select, errmsg = ex.Message });
            }
        }

        /// <summary>
        /// 根据ID获取会员详情
        /// </summary>
        /// <param name="id">会员ID</param>
        /// <returns>返回单条会员JSON数据</returns>
        [HttpGet("/members/details")]
        public async Task<IActionResult> Details(long id)
        {
            try
            {
                log.Information("id=" + id);
                var result = await _context.mr_members.Where(t => t.member_id == id).Select(item => (new
                {
                    item.member_id,
                    item.open_id,
                    item.member_account,
                    item.mobile_phone,
                    item.member_password,
                    item.member_name,
                    item.real_name,
                    item.gender_id,
                    gender_name = item.gender_id > -1 ? _context.mr_genders.FirstOrDefault(t => t.gender_id == item.gender_id)!.gender_name : "-",
                    item.member_avatar,
                    item.member_birth,
                    item.member_job,
                    item.invite_member_id,
                    item.invite_code,
                    item.qrcode_img,
                    item.source_id,
                    source_name = item.source_id > -1 ? _context.mr_utm_sources.FirstOrDefault(t => t.source_id == item.source_id)!.source_name : "-",
                    item.medium_id,
                    medium_name = item.medium_id > -1 ? _context.mr_utm_mediums.FirstOrDefault(t => t.medium_id == item.medium_id)!.medium_name : "-",
                    item.member_intro,
                    item.member_status,
                    item.type_id,
                    type_name = item.type_id > -1 ? _context.mr_member_types.FirstOrDefault(t => t.type_id == item.type_id)!.type_name : "-",
                    item.country_name,
                    item.province_name,
                    item.city_name,
                    item.district_name,
                    item.member_token,
                    item.encrypt_token,
                    item.member_remark,
                    item.created_at,
                    format_time = item.created_at.ToString("yyyy-MM-dd HH:mm:ss")
                })).FirstOrDefaultAsync();
                AddLogs((int)ENUMHelper.LogType.Search, (int)ENUMHelper.Platform.Admin + "" + (int)ENUMHelper.OperateType.Details + "" + (int)ENUMHelper.InfoType.Info, "获取会员详情ID=" + id + ",result=" + result.ToJson());
                if (result == null)
                {
                    return Json(new { code = 0, msg = "数据为空", count = 0 });
                }
                else
                {
                    return Json(new { code = 0, msg = "success", data = result });
                }
            }
            catch (Exception ex)
            {
                log.Fatal(ex, " -> Details");
                return Json(new { code = 0, errcode = (int)ENUMHelper.ExceptionType.Details, errmsg = ex.Message });
            }
        }

        /// <summary>
        /// 按天统计会员数据
        /// </summary>
        /// <returns>返回每日统计会员JSON数据集合</returns>
        public async Task<IActionResult> ChartByDay()
        {
            try
            {
                // money = g.Sum(t => t.total_pay)
                var list = await _context.mr_members.GroupBy(t => new { t.created_at.Year, t.created_at.Month, t.created_at.Day }).Select(g => (new { year = g.Key.Year, month = g.Key.Month, day = g.Key.Day, count = g.Count() })).OrderBy(t => t.year).ThenBy(t => t.month).ThenBy(t => t.day).ToListAsync();
                AddLogs((int)ENUMHelper.LogType.Count, (int)ENUMHelper.Platform.Admin + "" + (int)ENUMHelper.OperateType.Chart + "" + (int)ENUMHelper.InfoType.Info, "查询会员统计数据,list=" + list.ToJson());
                return Json(new { code = 0, msg = "success", data = list });
            }
            catch (Exception ex)
            {
                log.Fatal(ex, " -> ChartByDay");
                return Json(new { code = 0, errcode = (int)ENUMHelper.ExceptionType.Chart, errmsg = ex.Message });
            }
        }
    }
}
