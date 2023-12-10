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
    /// 内容摘要: 验证码处理类
    /// </summary>
    [Authorize]
    public class CodesController : BaseController<CodesController>
    {

        /// <summary>
        /// 验证码 实例化数据上下文
        /// </summary>
        /// <param name="context"></param>
        public CodesController(MRManageContext context) : base(context)
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
        /// 获取验证码列表数据
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="limit">每页显示数量</param>
        /// <param name="created_at">创建日期</param>
        /// <param name="mobile_phone">手机号</param>
        /// <param name="code_from">终端</param>
        /// <returns>返回验证码JSON数据集合</returns>
        public async Task<IActionResult> List(int page, int limit, string created_at, string mobile_phone, string code_from)
        {
            try
            {
                var result = from item in _context.Set<Codes>()
                             select new
                             {
                                 item.code_id,
                                 item.country_code,
                                 item.mobile_phone,
                                 item.code_num,
                                 item.code_from,
                                 item.due_minute,
                                 item.created_at,
                                 format_time = item.created_at.ToString("yyyy-MM-dd HH:mm:ss")
                             };

                if (!string.IsNullOrEmpty(mobile_phone))
                {
                    result = result.Where(t => t.mobile_phone.Contains(mobile_phone.Trim()));
                }
                if (!string.IsNullOrEmpty(code_from))
                {
                    result = result.Where(t => t.code_from.Contains(code_from.Trim()));
                }

                if (!string.IsNullOrEmpty(created_at))
                {
                    created_at = created_at.Replace(" - ", ",");
                    string[] splitDate = created_at.Split(new char[] { ',' });
                    string start_at = splitDate[0];
                    string end_at = splitDate[1];
                    result = result.Where(t => t.created_at >= Convert.ToDateTime(start_at) && t.created_at <= Convert.ToDateTime(end_at).AddDays(1));
                }

                var list = await result.OrderByDescending(t => t.code_id).Skip((page - 1) * limit).Take(limit).ToListAsync();
                var lCount = result.LongCount();
                AddLogs((int)ENUMHelper.LogType.Search, (int)ENUMHelper.Platform.Admin + "" + (int)ENUMHelper.OperateType.List + "" + (int)ENUMHelper.InfoType.Info, "查询LINQ=" + result.Expression.ToString() + ",DATA=" + list.ToJson());

                return Json(new { code = 0, msg = "success", data = list, count = lCount });
            }
            catch (Exception ex)
            {
                log.Fatal(ex, " -> List");
                return Json(new { code = 0, errcode = (int)ENUMHelper.ExceptionType.List, errmsg = ex.Message });
            }
        }

        /// <summary>
        /// 按天统计验证码数据-通常用于统计/绘图页面
        /// </summary>
        /// <returns>返回每日统计验证码JSON数据集合</returns>
        public async Task<IActionResult> ChartByDay()
        {
            try
            {
                var list = await _context.mr_codes.GroupBy(t => new { t.created_at.Year, t.created_at.Month, t.created_at.Day }).Select(g => (new { year = g.Key.Year, month = g.Key.Month, day = g.Key.Day, count = g.Count() })).OrderBy(t => t.year).ThenBy(t => t.month).ThenBy(t => t.day).ToListAsync();
                AddLogs((int)ENUMHelper.LogType.Count, (int)ENUMHelper.Platform.Admin + "" + (int)ENUMHelper.OperateType.Chart + "" + (int)ENUMHelper.InfoType.Info, "查询统计数据,list=" + list.ToJson());
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
