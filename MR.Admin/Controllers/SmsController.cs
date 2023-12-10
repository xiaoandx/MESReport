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
    /// 内容摘要: 短信处理类
    /// </summary>
    public class SmsController : BaseController<SmsController>
    {

        /// <summary>
        /// 短信 实例化数据上下文
        /// </summary>
        /// <param name="context"></param>
        public SmsController(MRManageContext context) : base(context)
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
        /// 获取短信列表数据(通常用于管理页面或列表页面)
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="limit">每页显示数量</param>
        /// <param name="created_at">创建日期</param>
        /// <param name="mobile_phone">手机号</param>
        /// <param name="sms_content">短信内容</param>
        /// <returns>返回短信JSON数据集合</returns>
        public async Task<IActionResult> List(int page, int limit, string created_at, string mobile_phone, string sms_content)
        {
            try
            {
                var result = from item in _context.Set<Sms>()
                             select new
                             {
                                 item.sms_id,
                                 item.mobile_phone,
                                 item.result_content,
                                 send_status = item.result_content!.Contains("OK") || item.result_content.Contains("stat=100") ? "<span class=\"layui-badge layui-bg-green\">发送成功</span>" : "<span class=\"layui-badge layui-bg-red\">发送失败</span>",
                                 item.sms_content,
                                 item.created_at,
                                 format_time = item.created_at.ToString("yyyy-MM-dd HH:mm:ss")
                             };

                if (!string.IsNullOrEmpty(mobile_phone))
                    result = result.Where(t => t.mobile_phone.Contains(mobile_phone.Trim()));

                if (!string.IsNullOrEmpty(sms_content))
                    result = result.Where(t => t.sms_content.Contains(sms_content.Trim()));

                if (!string.IsNullOrEmpty(created_at))
                {
                    created_at = created_at.Replace(" - ", ",");
                    string[] splitDate = created_at.Split(new char[] { ',' });
                    string start_at = splitDate[0];
                    string end_at = splitDate[1];
                    result = result.Where(t => t.created_at >= Convert.ToDateTime(start_at) && t.created_at <= Convert.ToDateTime(end_at).AddDays(1));
                }

                var list = await result.OrderByDescending(t => t.sms_id).Skip((page - 1) * limit).Take(limit).ToListAsync();
                var lCount = result.LongCount();
                return Json(new { code = 0, msg = "success", data = list, count = lCount });
            }
            catch (Exception ex)
            {
                log.Error(" -> GetDataByPage", ex);
                return Json(new { code = 0, errcode = (int)ENUMHelper.ExceptionType.List, errmsg = ex.Message });
            }
        }

        /// <summary>
        /// 按天统计短信数据(通常用于统计/绘图页面)
        /// </summary>
        /// <returns>返回每日统计短信JSON数据集合</returns>
        public async Task<IActionResult> ChartByDay()
        {
            try
            {
                // money = g.Sum(t => t.total_pay)
                var list = await _context.mr_sms.GroupBy(t => new { t.created_at.Year, t.created_at.Month, t.created_at.Day }).Select(g => (new { year = g.Key.Year, month = g.Key.Month, day = g.Key.Day, count = g.Count() })).OrderBy(t => t.year).ThenBy(t => t.month).ThenBy(t => t.day).ToListAsync();
                AddLogs((int)ENUMHelper.LogType.Update, (int)ENUMHelper.Platform.Admin + "" + (int)ENUMHelper.OperateType.Edit + "" + (int)ENUMHelper.InfoType.Info, "统计短信=" + list.ToJson());

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
