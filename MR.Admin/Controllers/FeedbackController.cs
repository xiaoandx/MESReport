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
    /// 内容摘要: 意见反馈处理类
    /// </summary>
    [Authorize]
    public class FeedbackController : BaseController<FeedbackController>
    {

        /// <summary>
        /// 意见反馈 实例化数据上下文
        /// </summary>
        /// <param name="context"></param>
        public FeedbackController(MRManageContext context) : base(context)
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
        /// 获取意见反馈列表数据(通常用于管理页面或列表页面)
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="limit">每页显示数量</param>
        /// <param name="created_at">创建日期</param>
        /// <param name="feedback_name">姓名</param>
        /// <param name="feedback_phone">手机号</param>
        /// <param name="feedback_content">反馈内容</param>
        /// <param name="feedback_email">Email</param>
        /// <param name="type_id">反馈类型</param>
        /// <returns>返回意见反馈JSON数据集合</returns>
        public async Task<IActionResult> List(int page, int limit, string created_at, string feedback_name, string feedback_phone, string feedback_content, string feedback_email, int type_id =-1)
        {
            try
            {
                var result = from item in _context.Set<Feedback>()
                             select new
                             {
                                 item.feedback_id,
                                 item.type_id,
                                 type_name = item.type_id > -1 ? _context.mr_feedback_types.FirstOrDefault(t => t.type_id == item.type_id)!.type_name : "",
                                 item.member_id,
                                 item.feedback_name,
                                 item.feedback_email,
                                 item.feedback_phone,
                                 item.feedback_content,
                                 item.client_ip,
                                 item.feedback_remark,
                                 item.created_at,
                                 format_time = item.created_at.ToString("yyyy-MM-dd HH:mm")
                             };

                if (type_id > -1)
                {
                    result = result.Where(t => t.type_id == type_id);
                }

                if (!string.IsNullOrEmpty(feedback_name))
                {
                    result = result.Where(t => t.feedback_name.Contains(feedback_name.Trim()));
                }

                if (!string.IsNullOrEmpty(feedback_phone))
                {
                    result = result.Where(t => t.feedback_phone.Contains(feedback_phone.Trim()));
                }

                if (!string.IsNullOrEmpty(feedback_email))
                {
                    result = result.Where(t => t.feedback_email.Contains(feedback_email.Trim()));
                }

                if (!string.IsNullOrEmpty(feedback_content))
                {
                    result = result.Where(t => t.feedback_content.Contains(feedback_content.Trim()));
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
                AddLogs((int)ENUMHelper.LogType.Search, (int)ENUMHelper.Platform.Admin + "" + (int)ENUMHelper.OperateType.List + "" + (int)ENUMHelper.InfoType.Info, "查询意见反馈LINQ=" + result.Expression.ToString() + ",DATA=" + list.ToJson());
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
        /// 根据ID获取意见反馈详情
        /// </summary>
        /// <param name="id">意见反馈ID</param>
        /// <returns>返回单条意见反馈JSON数据</returns>
        [HttpGet("/feedback/details")]
        public async Task<IActionResult> Details(long id)
        {
            try
            {
                log.Information("id=" + id);
                var result = await _context.mr_feedback.Where(t => t.feedback_id == id).Select(item => (new
                {
                    item.feedback_id,
                    item.type_id,
                    item.member_id,
                    item.feedback_name,
                    item.feedback_email,
                    item.feedback_phone,
                    item.feedback_content,
                    item.client_ip,
                    item.feedback_remark,
                    item.created_at,
                    format_time = item.created_at.ToString("yyyy-MM-dd HH:mm")
                })).FirstOrDefaultAsync();
                AddLogs((int)ENUMHelper.LogType.Search, (int)ENUMHelper.Platform.Admin + "" + (int)ENUMHelper.OperateType.Details + "" + (int)ENUMHelper.InfoType.Info, "获取意见反馈详情ID=" + id + ",result=" + result.ToJson());
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
        /// 按天统计意见反馈数据(通常用于统计/绘图页面)
        /// </summary>
        /// <returns>返回每日统计意见反馈JSON数据集合</returns>
        public async Task<IActionResult> ChartByDay()
        {
            try
            {
                var list = await _context.mr_feedback.GroupBy(t => new { t.created_at.Year, t.created_at.Month, t.created_at.Day }).Select(g => (new { year = g.Key.Year, month = g.Key.Month, day = g.Key.Day, count = g.Count() })).OrderBy(t => t.year).ThenBy(t => t.month).ThenBy(t => t.day).ToListAsync();
                AddLogs((int)ENUMHelper.LogType.Count, (int)ENUMHelper.Platform.Admin + "" + (int)ENUMHelper.OperateType.Chart + "" + (int)ENUMHelper.InfoType.Info, "查询统计数据,list=" + list.ToJson());

                return Json(new { code = 0, msg = "success", data = list });
            }
            catch (Exception ex)
            {
                log.Fatal(ex, " -> ChartByDay");
                return Json(new { code = 0, errcode = (int)ENUMHelper.ExceptionType.Chart, errmsg = ex.Message });
            }
        }

        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="feedback">Feedback对象</param>
        /// <returns>返回新增意见反馈后的单条JSON数据</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Feedback feedback)
        {
            try
            {
                log.Information("feedback=" + feedback.ToJson());

                feedback.type_id = 1;
                feedback.member_id = -1;
                feedback.client_ip = HttpContext.Connection.RemoteIpAddress!.ToString();
                feedback.feedback_remark = "";
                feedback.created_at = DateTime.Now;
                _context.mr_feedback.Add(feedback);
                await _context.SaveChangesAsync();
                AddLogs((int)ENUMHelper.LogType.Create, (int)ENUMHelper.Platform.Admin + "" + (int)ENUMHelper.OperateType.Create + "" + (int)ENUMHelper.InfoType.Info, "新增意见反馈,feedback=" + feedback.ToJson());

                return Json(new { code = 0, msg = "添加成功", data = feedback });
            }
            catch (Exception ex)
            {
                log.Fatal(ex, " -> Create");
                return Json(new { code = 0, errcode = (int)ENUMHelper.ExceptionType.Create, errmsg = ex.Message });
            }
        }

        /// <summary>
        /// 根据ID更改数据
        /// </summary>
        /// <param name="id">意见反馈ID</param>
        /// <param name="feedback">Feedback对象</param>
        /// <returns>返回修改意见反馈后的单条JSON数据</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, Feedback feedback)
        {
            try
            {
                log.Information("feedback=" + feedback.ToJson());
                var item = _context.mr_feedback.Find(id);
                if (item != null)
                {
                    item.feedback_name = feedback.feedback_name;
                    item.feedback_email = feedback.feedback_email;
                    item.feedback_phone = feedback.feedback_phone;
                    item.feedback_content = feedback.feedback_content;
                    _context.mr_feedback.Update(item);
                    await _context.SaveChangesAsync();
                    AddLogs((int)ENUMHelper.LogType.Update, (int)ENUMHelper.Platform.Admin + "" + (int)ENUMHelper.OperateType.Edit + "" + (int)ENUMHelper.InfoType.Info, "编辑意见反馈ID=" + id + ",feedback=" + feedback.ToJson());

                    return Json(new { code = 0, msg = "更新成功", data = item });
                }
                else
                {
                    return Json(new { code = 0, msg = "操作失败，ID=" + id + "不存在！" });
                }
            }
            catch (Exception ex)
            {
                log.Fatal(ex, " -> Edit");
                return Json(new { code = 0, errcode = (int)ENUMHelper.ExceptionType.Edit, errmsg = ex.Message });
            }
        }

        /// <summary>
        /// 根据ID删除数据
        /// </summary>
        /// <param name="id">意见反馈ID</param>
        /// <returns>返回删除操作信息</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                log.Information("id=" + id);
                var item = _context.mr_feedback.Find(id);
                if (item == null)
                {
                    return Json(new { code = 0, msg = "操作失败，ID=" + id + "不存在！" });
                }
                _context.mr_feedback.Remove(item);
                await _context.SaveChangesAsync();
                AddLogs((int)ENUMHelper.LogType.Delete, (int)ENUMHelper.Platform.Admin + "" + (int)ENUMHelper.OperateType.Logout + "" + (int)ENUMHelper.InfoType.Info, "删除意见反馈ID=" + id + ",item=" + item.ToJson());

                return Json(new { code = 0, msg = "删除成功" });
            }
            catch (Exception ex)
            {
                log.Fatal(ex, " -> Delete");
                return Json(new { code = 0, errcode = (int)ENUMHelper.ExceptionType.Delete, errmsg = ex.Message });
            }
        }

    }
}
