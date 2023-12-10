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
    /// 内容摘要: 消息处理类
    /// </summary>
    public class MessagesController : BaseController<MessagesController>
    {

        /// <summary>
        /// 消息 实例化数据上下文
        /// </summary>
        /// <param name="context"></param>
        public MessagesController(MRManageContext context) : base(context)
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
        /// 统计页面
        /// </summary>
        /// <returns></returns>
        public IActionResult Chart()
        {
            return View();
        }

        /// <summary>
        /// 消息提醒页面
        /// </summary>
        /// <returns></returns>
        public IActionResult Tips()
        {
            return View();
        }

        /// <summary>
        /// 获取消息列表数据(通常用于管理页面或列表页面)
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="limit">每页显示数量</param>
        /// <param name="created_at">创建日期</param>
        /// <param name="member_name">昵称</param>
        /// <param name="message_title">消息标题</param>
        /// <param name="message_content">消息内容</param>
        /// <param name="is_read">是否已读</param>
        /// <param name="type_id">消息类型</param>
        /// <returns>返回消息JSON数据集合</returns>
        public async Task<IActionResult> List(int page, int limit, string created_at, string member_name, string message_title, string message_content, int is_read = -1,int type_id = -1)
        {
            try
            {
                var result = from item in _context.Set<Messages>()
                             select new
                             {
                                 item.message_id,
                                 item.object_id,
                                 item.object_type_id,
                                 object_name = item.type_id > -1 ? _context.mr_objects.FirstOrDefault(t => t.object_id == item.object_type_id)!.object_name : "-",
                                 item.type_id,
                                 type_name = item.type_id > -1 ? _context.mr_message_types.FirstOrDefault(t => t.type_id == item.type_id)!.type_name : "-",
                                 item.member_id,
                                 member_name = item.member_id > -1 ? _context.mr_members.FirstOrDefault(t => t.member_id == item.member_id)!.member_name : "-",
                                 item.to_member_id,
                                 item.message_title,
                                 item.message_content,
                                 item.message_img,
                                 item.is_read,
                                 item.message_remark,
                                 item.created_at,
                                 format_time = item.created_at.ToString("yyyy-MM-dd HH:mm:ss")
                             };

                if (!string.IsNullOrEmpty(member_name))
                {
                    result = result.Where(t => t.member_name.Contains(member_name.Trim()));
                }
                if (!string.IsNullOrEmpty(message_title))
                {
                    result = result.Where(t => t.message_title.Contains(message_title.Trim()));
                }
                if (!string.IsNullOrEmpty(message_content))
                {
                    result = result.Where(t => t.message_content.Contains(message_content.Trim()));
                }

                if (is_read > -1)
                {
                    if (is_read == 1)
                    {
                        result = result.Where(t => t.is_read == true);
                    }
                    if (is_read == 0)
                    {
                        result = result.Where(t => t.is_read == false);
                    }
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
                var list = await result.OrderByDescending(t => t.message_id).Skip((page - 1) * limit).Take(limit).ToListAsync();
                var lCount = result.LongCount();
                AddLogs((int)ENUMHelper.LogType.Search, (int)ENUMHelper.Platform.Admin + "" + (int)ENUMHelper.OperateType.List + "" + (int)ENUMHelper.InfoType.Info, "查询消息LINQ=" + result.Expression.ToString() + ",DATA=" + list.ToJson());
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
        /// 根据ID获取消息详情
        /// </summary>
        /// <param name="id">消息ID</param>
        /// <returns>返回单条消息JSON数据</returns>
        [HttpGet("/messages/details")]
        public async Task<IActionResult> Details(long id)
        {
            try
            {
                log.Information("id=" + id);
                var result = await _context.mr_messages.Where(t => t.message_id == id).Select(item => (new
                {
                    item.message_id,
                    item.object_id,
                    item.object_type_id,
                    object_name = item.type_id > -1 ? _context.mr_objects.FirstOrDefault(t => t.object_id == item.object_type_id)!.object_name : "-",
                    item.type_id,
                    type_name = item.type_id > -1 ? _context.mr_message_types.FirstOrDefault(t => t.type_id == item.type_id)!.type_name : "-",
                    item.member_id,
                    member_name = item.member_id > -1 ? _context.mr_members.FirstOrDefault(t => t.member_id == item.member_id)!.member_name : "-",
                    item.to_member_id,
                    item.message_title,
                    item.message_content,
                    item.message_img,
                    item.is_read,
                    item.message_remark,
                    item.created_at,
                    format_time = item.created_at.ToString("yyyy-MM-dd HH:mm:ss")
                })).FirstOrDefaultAsync();
                AddLogs((int)ENUMHelper.LogType.Search, (int)ENUMHelper.Platform.Admin + "" + (int)ENUMHelper.OperateType.Details + "" + (int)ENUMHelper.InfoType.Info, "获取消息详情ID=" + id + ",result=" + result.ToJson());
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
        /// 按天统计消息数据(通常用于统计/绘图页面)
        /// </summary>
        /// <returns>返回每日统计消息JSON数据集合</returns>
        public async Task<IActionResult> ChartByDay()
        {
            try
            {
                var list = await _context.mr_messages.GroupBy(t => new { t.created_at.Year, t.created_at.Month, t.created_at.Day }).Select(g => (new { year = g.Key.Year, month = g.Key.Month, day = g.Key.Day, count = g.Count() })).OrderBy(t => t.year).ThenBy(t => t.month).ThenBy(t => t.day).ToListAsync();
                AddLogs((int)ENUMHelper.LogType.Count, (int)ENUMHelper.Platform.Admin + "" + (int)ENUMHelper.OperateType.Chart + "" + (int)ENUMHelper.InfoType.Info, "查询消息统计数据,list=" + list.ToJson());

                return Json(new { code = 0, msg = "success", data = list });
            }
            catch (Exception ex)
            {
                log.Fatal(ex, " -> ChartByDay");
                return Json(new { code = 0, errcode = (int)ENUMHelper.ExceptionType.Delete, errmsg = ex.Message });
            }
        }

        /// <summary>
        /// 更新消息是否已读
        /// </summary>
        /// <param name="id">消息ID</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetReady(long id)
        {
            try
            {
                log.Information("id=" + id);
                var item = await _context.mr_messages.FirstOrDefaultAsync(t => t.message_id == id);
                if (item != null)
                {
                    item.is_read = true;
                    _context.mr_messages.Update(item);
                    await _context.SaveChangesAsync();
                    AddLogs((int)ENUMHelper.LogType.Update, (int)ENUMHelper.Platform.Admin + "" + (int)ENUMHelper.OperateType.Set + "" + (int)ENUMHelper.InfoType.Info, "更新消息是否已读ID=" + id);

                    return Json(new { code = 0, msg = "操作成功", data = item });
                }
                else
                {
                    return Json(new { code = 0, msg = "操作失败，ID=" + id + "不存在！" });
                }
            }
            catch (Exception ex)
            {
                log.Fatal(ex, " -> SetRead");
                return Json(new { code = 0, errcode = (int)ENUMHelper.ExceptionType.Set, errmsg = ex.Message });
            }
        }

        /// <summary>
        /// 根据消息类型更新该类型消息全部已读
        /// </summary>
        /// <param name="type_id">消息类型</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetReadyAll(int type_id)
        {
            try
            {
                log.Information("type_id=" + type_id);
                if (type_id == -1)
                {
                    var item = await _context.mr_messages.ToListAsync();
                    item.ForEach(c =>
                    {
                        c.is_read = true;
                    });
                    _context.mr_messages.UpdateRange(item);
                    await _context.SaveChangesAsync();
                    AddLogs((int)ENUMHelper.LogType.Update, (int)ENUMHelper.Platform.Admin + "" + (int)ENUMHelper.OperateType.Set + "" + (int)ENUMHelper.InfoType.Info, "根据消息类型更新该类型消息全部已读type_id=" + type_id);

                    return Json(new { code = 0, msg = "操作成功", data = item });
                }
                else
                {
                    var item = await _context.mr_messages.Where(t => t.type_id == type_id).ToListAsync();
                    item.ForEach(c =>
                    {
                        c.is_read = true;
                    });
                    _context.mr_messages.UpdateRange(item);
                    await _context.SaveChangesAsync();
                    AddLogs((int)ENUMHelper.LogType.Update, (int)ENUMHelper.Platform.Admin + "" + (int)ENUMHelper.OperateType.Set + "" + (int)ENUMHelper.InfoType.Info, "根据消息类型更新该类型消息全部已读type_id=" + type_id);

                    return Json(new { code = 0, msg = "操作成功", data = item });
                }
            }
            catch (Exception ex)
            {
                log.Fatal(ex, " -> SetRead");
                return Json(new { code = 0, errcode = (int)ENUMHelper.ExceptionType.Set, errmsg = ex.Message });
            }
        }
    }
}
