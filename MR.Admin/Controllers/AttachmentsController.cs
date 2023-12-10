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
using System.Net.Mail;

namespace MR.Manage.Controllers
{
    /// <summary>
    /// 内容摘要: 附件处理类
    /// </summary>
    [Authorize]
    public class AttachmentsController : BaseController<AttachmentsController>
    {

        /// <summary>
        /// 附件 实例化数据上下文
        /// </summary>
        /// <param name="context"></param>
        public AttachmentsController(MRManageContext context) : base(context)
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
        /// 获取附件列表数据
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="limit">每页显示数量</param>
        /// <param name="created_at">创建日期</param>
        /// <returns>返回附件JSON数据集合</returns>
        public async Task<IActionResult> List(int page, int limit, string created_at)
        {
            try
            {
                var result = _context.mr_attachments.Where(t => t.attachment_id > 0);

                if (!string.IsNullOrEmpty(created_at))
                {
                    created_at = created_at.Replace(" - ", ",");
                    string[] splitDate = created_at.Split(new char[] { ',' });
                    string start_at = splitDate[0];
                    string end_at = splitDate[1];
                    result = result.Where(t => t.created_at >= Convert.ToDateTime(start_at) && t.created_at <= Convert.ToDateTime(end_at).AddDays(1));
                }
                var list =  await result.OrderByDescending(t => t.attachment_id).Skip((page - 1) * limit).Take(limit).ToListAsync();
                var lCount = result.LongCount();
                AddLogs((int)ENUMHelper.LogType.Search, (int)ENUMHelper.Platform.Admin + "" + (int)ENUMHelper.OperateType.List + "" + (int)ENUMHelper.InfoType.Info, "查询附件LINQ=" + result.Expression.ToString() + ",DATA=" + list.ToJson());

                return Json(new { code = 0, msg = "success", data = list, count = lCount });
            }
            catch (Exception ex)
            {
                log.Fatal(ex, " -> List");
                return Json(new { code = 0, errcode = (int)ENUMHelper.ExceptionType.List, errmsg = ex.Message });
            }
        }

        /// <summary>
        /// 获取附件数据
        /// </summary>
        /// <returns>返回附件JSON数据集合</returns>
        public async Task<IActionResult> Select()
        {
            try
            {
                var list = await _context.mr_attachments.OrderBy(t => t.attachment_id).ToListAsync();
                AddLogs((int)ENUMHelper.LogType.Search, (int)ENUMHelper.Platform.Admin + "" + (int)ENUMHelper.OperateType.Select + "" + (int)ENUMHelper.InfoType.Info, "查询附件下拉填充,DATA=" + list.ToJson());

                return Json(new { code = 0, msg = "success", data = list });
            }
            catch (Exception ex)
            {
                log.Fatal(ex, " -> Select");
                return Json(new { code = 0, errcode = (int)ENUMHelper.ExceptionType.Select, errmsg = ex.Message });
            }
        }

        /// <summary>
        /// 根据ID获取附件详情
        /// </summary>
        /// <param name="id">附件ID</param>
        /// <returns>返回单条附件JSON数据</returns>
        [HttpGet("/attachments/details")]
        public async Task<IActionResult> Details(long id)
        {
            try
            {
                log.Information("id=" + id);
                var result = await _context.mr_attachments.Where(t => t.attachment_id == id).Select(item => (new
                {
                    item.attachment_id,
                    item.object_id,
                    item.type_id,
                    item.attachment_name,
                    item.attachment_path,
                    item.attachment_remark,
                    item.created_at,
                    format_time = item.created_at.ToString("yyyy-MM-dd HH:mm")
                })).FirstOrDefaultAsync();
                AddLogs((int)ENUMHelper.LogType.Search, (int)ENUMHelper.Platform.Admin + "" + (int)ENUMHelper.OperateType.Details + "" + (int)ENUMHelper.InfoType.Info, "获取附件详情ID=" + id + ",result=" + result.ToJson());
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
        /// 新增附件数据
        /// </summary>
        /// <param name="attachments">Attachments对象</param>
        /// <returns>返回新增附件后的单条JSON数据</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Attachments attachments)
        {
            try
            {
                log.Information("attachments=" + attachments.ToJson());
                attachments.created_at = DateTime.Now;
                _context.mr_attachments.Add(attachments);
                await _context.SaveChangesAsync();
                AddLogs((int)ENUMHelper.LogType.Create, (int)ENUMHelper.Platform.Admin + "" + (int)ENUMHelper.OperateType.Create + "" + (int)ENUMHelper.InfoType.Info, "新增附件,attachments=" + attachments.ToJson());

                return Json(new { code = 0, msg = "上传成功", data = attachments });
            }
            catch (Exception ex)
            {
                log.Fatal(ex, " -> Create");
                return Json(new { code = 0, errcode = (int)ENUMHelper.ExceptionType.Create, errmsg = ex.Message });
            }
        }
    }
}
