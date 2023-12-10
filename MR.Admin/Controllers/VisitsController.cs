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

namespace MR.Manage.Controllers
{
    /// <summary>
    /// 内容摘要: 访问量处理类
    /// </summary>
    public class VisitsController : BaseController<VisitsController>
    {

        /// <summary>
        /// 访问量 实例化数据上下文
        /// </summary>
        /// <param name="context"></param>
        public VisitsController(MRManageContext context) : base(context)
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
        /// 详情页面
        /// </summary>
        /// <returns></returns>
        public IActionResult Views()
        {
            return View();
        }

        /// <summary>
        /// 获取列表数据(通常用于管理页面或列表页面)
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="limit">每页显示数量</param>
        /// <param name="created_at">创建日期</param>
        /// <param name="member_name">会员昵称</param>
        /// <param name="client_ip">客户端IP</param>
        /// <param name="visit_path">被访问页面</param>
        /// <param name="source_id">访问客户端</param>
        /// <param name="search_engine">搜索引擎</param>
        /// <returns>返回访问量JSON数据集合</returns>
        public async Task<IActionResult> List(int page, int limit, string created_at, string member_name, string client_ip, string visit_path, int source_id = -1, int channel_id = -1)
        {
            try
            {
                var result = from item in _context.Set<Visits>()
                             select new
                             {
                                 item.visit_id,
                                 item.referrer_url,
                                 item.search_engine,
                                 item.client_browser,
                                 item.visit_path,
                                 item.client_info,
                                 item.client_cookies,
                                 item.device_system,
                                 device_platform = !string.IsNullOrEmpty(item.device_platform) ? item.device_platform : "-",
                                 item.device_brand,
                                 device_model = !string.IsNullOrEmpty(item.device_model) ? item.device_model : "-",
                                 item.device_language,
                                 item.device_version,
                                 item.device_info,
                                 item.temp_info,
                                 item.member_id,
                                 member_name = item.member_id > -1 ? _context.mr_members.FirstOrDefault(t => t.member_id == item.member_id)!.member_name : "-",
                                 item.company_id,
                                 item.client_id,
                                 item.client_ip,
                                 item.source_id,
                                 source_name = item.source_id > -1 ? _context.mr_utm_sources.FirstOrDefault(t => t.source_id == item.source_id)!.source_name : "-",
                                 item.channel_id,
                                 channel_name = item.channel_id > -1 ? _context.mr_visit_channels.FirstOrDefault(t => t.channel_id == item.channel_id)!.channel_name : "-",
                                 item.client_version,
                                 item.visit_remark,
                                 item.created_at,
                                 format_time = item.created_at.ToString("yyyy-MM-dd HH:mm:ss")
                             };

                if (!string.IsNullOrEmpty(visit_path))
                {
                    result = result.Where(t => t.visit_path.Contains(visit_path.Trim()));
                }
                if (!string.IsNullOrEmpty(member_name))
                {
                    result = result.Where(t => t.member_name.Contains(member_name.Trim()));
                }
                if (!string.IsNullOrEmpty(client_ip))
                {
                    result = result.Where(t => t.client_ip.Contains(client_ip.Trim()));
                }
                if (source_id > 0)
                {
                    result = result.Where(t => t.source_id == source_id);
                }
                if (channel_id > 0)
                {
                    result = result.Where(t => t.channel_id == channel_id);
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
                AddLogs((int)ENUMHelper.LogType.Search, (int)ENUMHelper.Platform.Admin + "" + (int)ENUMHelper.OperateType.List + "" + (int)ENUMHelper.InfoType.Info, "查询访问量LINQ=" + result.Expression.ToString() + ",DATA=" + list.ToJson());
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
        /// 按天统计数据(通常用于统计/绘图页面)
        /// </summary>
        /// <returns>返回每日统计访问量JSON数据集合</returns>
        public async Task<IActionResult> ChartByDay()
        {
            try
            {
                var list = await _context.mr_visits.GroupBy(t => new { t.created_at.Year, t.created_at.Month, t.created_at.Day }).Select(g => (new { year = g.Key.Year, month = g.Key.Month, day = g.Key.Day, count = g.Count() })).OrderBy(t => t.year).ThenBy(t => t.month).ThenBy(t => t.day).ToListAsync();
                AddLogs((int)ENUMHelper.LogType.Count, (int)ENUMHelper.Platform.Admin + "" + (int)ENUMHelper.OperateType.Chart + "" + (int)ENUMHelper.InfoType.Info, "查询访问量统计数据,list=" + list.ToJson());

                return Json(new { code = 0, msg = "success", data = list });
            }
            catch (Exception ex)
            {
                log.Fatal(ex, " -> ChartByDay");
                return Json(new { code = 0, errcode = (int)ENUMHelper.ExceptionType.Chart, errmsg = ex.Message });
            }
        }

        /// <summary>
        /// 根据ID获取访问量详情数据(通常用于详情页面)
        /// </summary>
        /// <param name="id">访问量ID</param>
        /// <returns>返回单条访问量JSON数据</returns>
        [HttpGet("/visits/details")]
        public async Task<IActionResult> Details(long? id)
        {
            try
            {
                log.Information("id=" + id);
                var result = await _context.mr_visits.Where(t => t.visit_id == id).Select(item => (new
                {
                    item.visit_id,
                    referrer_url = string.IsNullOrEmpty(item.referrer_url) ? "-" : item.referrer_url,
                    item.search_engine,
                    client_browser = string.IsNullOrEmpty(item.client_browser) ? "-" : item.client_browser,
                    item.visit_path,
                    client_info = string.IsNullOrEmpty(item.client_info) ? "-" : item.client_info,
                    client_cookies = string.IsNullOrEmpty(item.client_cookies) ? "-" : item.client_cookies,
                    device_system = string.IsNullOrEmpty(item.device_system) ? "-" : item.device_system,
                    device_platform = string.IsNullOrEmpty(item.device_platform) ? "-" : item.device_platform,
                    device_brand = string.IsNullOrEmpty(item.device_brand) ? "-" : item.device_brand,
                    device_model = string.IsNullOrEmpty(item.device_info) ? "-" : item.device_model,
                    device_language = string.IsNullOrEmpty(item.device_language) ? "-" : item.device_language,
                    device_version = string.IsNullOrEmpty(item.device_version) ? "-" : item.device_version,
                    device_info = string.IsNullOrEmpty(item.device_info) ? "-" : item.device_info,
                    item.temp_info,
                    item.member_id,
                    item.company_id,
                    item.client_id,
                    item.client_ip,
                    item.source_id,
                    item.channel_id,
                    item.client_version,
                    item.visit_remark,
                    item.created_at,
                    format_time = item.created_at.ToString("yyyy-MM-dd HH:mm")
                })).FirstOrDefaultAsync();
                AddLogs((int)ENUMHelper.LogType.Search, (int)ENUMHelper.Platform.Admin + "" + (int)ENUMHelper.OperateType.Details + "" + (int)ENUMHelper.InfoType.Info, "获取访问量详情ID=" + id + ",result=" + result.ToJson());
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
