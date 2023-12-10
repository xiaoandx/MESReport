using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MR.Manage.Data;
using MR.Models;
using Senparc.CO2NET.Extensions;
using Microsoft.AspNetCore.Authorization;
using MR.Utility.Helper;
using System.Collections.Generic;

namespace MR.Manage.Controllers
{
    /// <summary>
    /// 内容摘要: 日志处理类
    /// </summary>
    [Authorize]
    public class LogsController : BaseController<LogsController>
    {

        /// <summary>
        /// 日志 实例化数据上下文
        /// </summary>
        /// <param name="context"></param>
        public LogsController(MRManageContext context) : base(context)
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
        /// 统计页面
        /// </summary>
        /// <returns></returns>
        public IActionResult Chart()
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
        /// 获取日志列表
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="limit">每页显示数量</param>
        /// <param name="created_at">创建日期</param>
        /// <param name="type_id">日志类型</param>
        /// <param name="platform_id">客户端类型</param>
        /// <param name="log_content">日志内容</param>
        /// <param name="client_ip">客户端IP</param>
        /// <param name="member_name">用户名</param>
        /// <returns>返回日志JSON数据集合</returns>
        public async Task<IActionResult> List(int page, int limit, string created_at, int type_id, int platform_id, string log_content, string client_ip, string member_name)
        {
            try
            {
                var result = from item in _context.Set<Logs>()
                             select new
                             {
                                 item.log_id,
                                 item.type_id,
                                 type_name = item.type_id > -1 ? _context.mr_log_types.FirstOrDefault(t => t.type_id == item.type_id)!.type_name : "-",
                                 item.user_id,
                                 item.member_id,
                                 member_name = item.user_id > -1 ? _context.mr_users.FirstOrDefault(t => t.user_id == item.user_id)!.user_name : _context.mr_members.FirstOrDefault(t => t.member_id == item.user_id)!.member_name,
                                 item.log_code,
                                 item.platform_id,
                                 platform_name = item.platform_id > -1 ? _context.mr_platforms.FirstOrDefault(t => t.platform_id == item.platform_id)!.platform_name : "-",
                                 log_content = item.log_content!.Length > 100 ? item.log_content.Substring(0, 100) : item.log_content,
                                 item.client_ip,
                                 item.page_url,
                                 item.referrer_url,
                                 item.user_agent,
                                 item.user_cookies,
                                 item.log_remark,
                                 item.created_at,
                                 format_time = item.created_at.ToString("yyyy-MM-dd HH:mm")
                             };

                if (type_id > 0)
                {
                    result = result.Where(t => t.type_id == type_id);
                }
                if (!string.IsNullOrEmpty(member_name))
                {
                    result = result.Where(t => t.member_name.Contains(member_name.Trim()));
                }
                if (!string.IsNullOrEmpty(log_content))
                {
                    result = result.Where(t => t.log_content.Contains(log_content.Trim()));
                }
                if (platform_id > 0)
                {
                    result = result.Where(t => t.platform_id == platform_id);
                }
                if (!string.IsNullOrEmpty(client_ip))
                {
                    result = result.Where(t => t.client_ip.Contains(client_ip.Trim()));
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
                AddLogs((int)ENUMHelper.LogType.Search, (int)ENUMHelper.Platform.Admin + "" + (int)ENUMHelper.OperateType.List + "" + (int)ENUMHelper.InfoType.Info, "查询LINQ=" + result.Expression.ToString() + ",DATA=" + list.ToJson());
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
        /// 获取最新5条日志信息
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Active()
        {
            try
            {
                var result = from item in _context.Set<Logs>().OrderByDescending(c => c.log_id).Take(5)
                             select new
                             {
                                 item.log_id,
                                 item.type_id,
                                 item.user_id,
                                 item.client_ip,
                                 item.page_url,
                                 item.referrer_url,
                                 item.user_agent,
                                 item.user_cookies,
                                 item.platform_id,
                                 item.created_at,
                                 format_time = item.created_at.ToString("MM/dd"),
                                 platform_name = item.platform_id > -1 ? _context.mr_platforms.FirstOrDefault(t => t.platform_id == item.platform_id)!.platform_name : "-",
                                 type_name = _context.mr_log_types.FirstOrDefault(t => t.type_id == item.type_id)!.type_name,
                                 member_name = item.user_id > -1 ? _context.mr_users.FirstOrDefault(t => t.user_id == item.user_id)!.user_name : _context.mr_members.FirstOrDefault(t => t.member_id == item.user_id)!.member_name,
                             };

                var list = await result.OrderByDescending(t => t.log_id).ToListAsync();
                var lCount = result.LongCount();
                AddLogs((int)ENUMHelper.LogType.Search, (int)ENUMHelper.Platform.Admin + "" + (int)ENUMHelper.OperateType.List + "" + (int)ENUMHelper.InfoType.Info, "查询LINQ=" + result.Expression.ToString() + ",DATA=" + list.ToJson());
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
                log.Fatal(ex, " -> Active");
                return Json(new { code = 0, errcode = (int)ENUMHelper.ExceptionType.List, errmsg = ex.Message });
            }
        }

        /// <summary>
        /// 获取会员日志记录
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="limit">每页显示数量</param>
        /// <param name="member_id">会员ID</param>
        /// <returns></returns>
        public async Task<IActionResult> MemberList(int page, int limit, int member_id)
        {
            try
            {
                var result = from item in _context.Set<Logs>().Where(t => t.type_id == 2 && t.member_id == member_id)
                             select new
                             {
                                 item.log_id,
                                 item.type_id,
                                 item.user_id,
                                 log_content = item.log_content!.Length > 100 ? item.log_content.Substring(0, 100) : item.log_content,
                                 item.client_ip,
                                 item.page_url,
                                 item.referrer_url,
                                 item.user_agent,
                                 item.user_cookies,
                                 item.platform_id,
                                 item.created_at,
                                 format_time = item.created_at.ToString("yyyy-MM-dd HH:mm:ss"),
                                 platform_name = item.platform_id > -1 ? _context.mr_platforms.FirstOrDefault(t => t.platform_id == item.platform_id)!.platform_name : "-",
                                 type_name = _context.mr_log_types.FirstOrDefault(t => t.type_id == item.type_id),
                                 member_name = item.user_id > -1 ? _context.mr_users.FirstOrDefault(t => t.user_id == item.user_id)!.user_name : _context.mr_members.FirstOrDefault(t => t.member_id == item.user_id)!.member_name,
                             };

                var list = await result.OrderByDescending(t => t.log_id).Skip((page - 1) * limit).Take(limit).ToListAsync();
                var lCount = result.LongCount();
                AddLogs((int)ENUMHelper.LogType.Search, (int)ENUMHelper.Platform.Admin + "" + (int)ENUMHelper.OperateType.List + "" + (int)ENUMHelper.InfoType.Info, "查询LINQ=" + result.Expression.ToString() + ",DATA=" + list.ToJson());
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
                log.Error(" -> MemberList", ex);
                return Json(new { code = 0, errcode = (int)ENUMHelper.ExceptionType.List, errmsg = ex.Message });
            }
        }

        /// <summary>
        /// 按天统计日志数据
        /// </summary>
        /// <returns>返回每日统计日志JSON数据集合</returns>
        public async Task<IActionResult> ChartByDay()
        {
            try
            {
                var list = await _context.mr_logs.GroupBy(t => new { t.created_at.Year, t.created_at.Month, t.created_at.Day }).Select(g => (new { year = g.Key.Year, month = g.Key.Month, day = g.Key.Day, count = g.Count() })).OrderBy(t => t.year).ThenBy(t => t.month).ThenBy(t => t.day).ToListAsync();
                AddLogs(9, "查询统计数据,list=" + list.ToJson(), "BP91000");
                return Json(new { code = 0, msg = "success", data = list });
            }
            catch (Exception ex)
            {
                log.Fatal(ex, " -> ChartByDay");
                return Json(new { code = 0, errcode = (int)ENUMHelper.ExceptionType.Chart, errmsg = ex.Message });
            }
        }

        /// <summary>
        /// 根据ID获取日志详情
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        public async Task<IActionResult> Details(long? id)
        {
            try
            {
                log.Information("id=" + id);
                var result = await _context.mr_logs.Where(t => t.log_id == id).Select(item => (new
                {
                    item.log_id,
                    item.type_id,
                    type_name = item.type_id > -1 ? _context.mr_log_types.FirstOrDefault(t => t.type_id == item.type_id)!.type_name : "-",
                    item.user_id,
                    item.member_id,
                    member_name = item.user_id > -1 ? _context.mr_users.FirstOrDefault(t => t.user_id == item.user_id)!.user_name : _context.mr_members.FirstOrDefault(t => t.member_id == item.user_id)!.member_name,
                    item.log_code,
                    item.platform_id,
                    platform_name = item.platform_id > -1 ? _context.mr_platforms.FirstOrDefault(t => t.platform_id == item.platform_id)!.platform_name : "-",
                    log_content = item.log_content!.Length > 100 ? item.log_content.Substring(0, 100) : item.log_content,
                    item.client_ip,
                    item.page_url,
                    item.referrer_url,
                    item.user_agent,
                    item.user_cookies,
                    item.log_remark,
                    item.created_at,
                    format_time = item.created_at.ToString("yyyy-MM-dd HH:mm:ss")
                })).FirstOrDefaultAsync();
                AddLogs((int)ENUMHelper.LogType.Search, (int)ENUMHelper.Platform.Admin + "" + (int)ENUMHelper.OperateType.Details + "" + (int)ENUMHelper.InfoType.Info, "id=" + id);

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
    }
}
