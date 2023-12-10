using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MR.Manage.Data;
using MR.Models;
using Senparc.CO2NET.Extensions;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using MR.Utility.Helper;

namespace MR.Manage.Controllers
{

    /// <summary>
    /// 内容摘要: 文章处理类
    /// </summary>
    [Authorize]
    public class ArticlesController : BaseController<ArticlesController>
    {

        /// <summary>
        /// 文章 实例化数据上下文
        /// </summary>
        /// <param name="context"></param>
        public ArticlesController(MRManageContext context) : base(context)
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
        /// 审核页面
        /// </summary>
        /// <returns></returns>
        public IActionResult Review()
        {
            return View();
        }

        /// <summary>
        /// 草稿箱页面
        /// </summary>
        /// <returns></returns>
        public IActionResult Draft()
        {
            return View();
        }

        /// <summary>
        /// 选择页面
        /// </summary>
        /// <returns></returns>
        public IActionResult Selected()
        {
            return View();
        }

        /// <summary>
        /// 获取文章列表数据
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="limit">每页显示数量</param>
        /// <param name="created_at">创建日期</param>
        /// <param name="article_title">标题</param>
        /// <param name="type_id">类型</param>
        /// <param name="status_id">状态</param>
        /// <param name="article_author">作者</param>
        /// <param name="article_content">内容</param>
        /// <param name="review_status_id">审核状态</param>
        /// <returns></returns>
        public async Task<IActionResult> List(int page, int limit, string created_at, string article_title, int type_id, int status_id, string article_author, string article_content, int review_status_id)
        {
            try
            {
                var result = from item in _context.Set<Articles>()
                             select new
                             {
                                 item.article_id,
                                 item.lang_id,
                                 item.article_title,
                                 show_article_title = string.IsNullOrEmpty(item.cover_img) ? item.article_title : "<img src='"+ MR.Manage.Filters.AppSettingsFilter.GetSetting("Aliyun:OSS_URL") + item.cover_img + "' width='20' height='20'> " + item.article_title,
                                 item.cover_img,
                                 item.article_content,
                                 item.article_author,
                                 item.article_source,
                                 item.article_summary,
                                 item.type_id,
                                 item.type_subid,
                                 item.status_id,
                                 status_name  = _context.mr_article_status.FirstOrDefault(t => t.status_id == item.status_id)!.status_name,
                                 item.review_status_id,
                                 review_status_name = _context.mr_review_status.FirstOrDefault(t => t.status_id == item.review_status_id)!.status_name,
                                 item.is_top,
                                 item.is_show,
                                 item.user_id,
                                 item.visit_num,
                                 item.set_publish_time,
                                 item.article_remark,
                                 item.created_at,
                                 format_time = item.created_at.ToString("yyyy-MM-dd HH:mm"),
                                 type_name = item.type_id > 0 ? _context.mr_article_types.FirstOrDefault(t => t.type_id == item.type_id)!.type_name : "-",
                                 type_subname = item.type_subid > 0 ? _context.mr_article_types.FirstOrDefault(t => t.type_id == item.type_subid)!.type_name : "-",
                                 review_content = _context.mr_reviews.OrderByDescending(t => t.created_at).FirstOrDefault(t => t.type_id == 1 && t.object_id == item.article_id)!.review_content
                             };

                if (!string.IsNullOrEmpty(article_title))
                {
                    result = result.Where(t => t.article_title.Contains(article_title.Trim()));
                }
                if (type_id > 0)
                {
                    result = result.Where(t => t.type_id == type_id);
                }
                if (status_id > 0)
                {
                    result = result.Where(t => t.status_id == status_id);
                }
                if (!string.IsNullOrEmpty(article_author))
                {
                    result = result.Where(t => t.article_author.Contains(article_author.Trim()));
                }
                if (!string.IsNullOrEmpty(article_content))
                {
                    result = result.Where(t => t.article_content.Contains(article_content.Trim()));
                }
                if (review_status_id > 0)
                {
                    result = result.Where(t => t.review_status_id == review_status_id);
                }
                else
                {
                    result = result.Where(t => t.review_status_id != 2);
                }

                if (!string.IsNullOrEmpty(created_at))
                {
                    created_at = created_at.Replace(" - ", ",");
                    string[] splitDate = created_at.Split(new char[] { ',' });
                    string start_at = splitDate[0];
                    string end_at = splitDate[1];
                    result = result.Where(t => t.created_at >= Convert.ToDateTime(start_at) && t.created_at <= Convert.ToDateTime(end_at).AddDays(1));
                }

                var list = await result.OrderByDescending(t => t.article_id).Skip((page - 1) * limit).Take(limit).ToListAsync();
                var lCount = result.LongCount();
                AddLogs((int)ENUMHelper.LogType.Search, (int)ENUMHelper.Platform.Admin + "" + (int)ENUMHelper.OperateType.List + "" + (int)ENUMHelper.InfoType.Info, "查询文章LINQ=" + result.Expression.ToString() + ",DATA=" + list.ToJson());
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
        /// 获取文章数据
        /// </summary>
        /// <returns>返回文章JSON数据集合</returns>
        public async Task<IActionResult> Select()
        {
            try
            {
                var list = await _context.mr_articles.OrderBy(t => t.article_id).ToListAsync();
                AddLogs((int)ENUMHelper.LogType.Search, (int)ENUMHelper.Platform.Admin + "" + (int)ENUMHelper.OperateType.Select + "" + (int)ENUMHelper.InfoType.Info, "查询文章下拉填充,DATA=" + list.ToJson());

                return Json(new { code = 0, msg = "success", data = list });
            }
            catch (Exception ex)
            {
                log.Fatal(ex, " -> Select");
                return Json(new { code = 0, errcode = (int)ENUMHelper.ExceptionType.Select, errmsg = ex.Message });
            }
        }

        /// <summary>
        /// 根据ID获取文章详情数据
        /// </summary>
        /// <param name="id">文章ID</param>
        /// <returns>返回单条文章JSON数据</returns>
        [HttpGet("/articles/details")]
        public async Task<IActionResult> Details(long? id)
        {
            try
            {
                log.Information("id=" + id);
                var result = await _context.mr_articles.Where(t => t.article_id == id).Select(item => (new
                {
                    item.article_id,
                    item.lang_id,
                    item.article_title,
                    show_article_title = string.IsNullOrEmpty(item.cover_img) ? item.article_title : "<img src='" + MR.Manage.Filters.AppSettingsFilter.GetSetting("Aliyun:OSS_URL") + item.cover_img + "' width='20' height='20'> " + item.article_title,
                    item.cover_img,
                    show_cover_img = !string.IsNullOrEmpty(item.cover_img) ? MR.Manage.Filters.AppSettingsFilter.GetSetting("Aliyun:OSS_URL") + item.cover_img : "/images/no_photo.png",
                    item.article_content,
                    item.article_author,
                    item.article_source,
                    item.article_summary,
                    item.type_id,
                    item.type_subid,
                    item.status_id,
                    status_name = _context.mr_article_status.FirstOrDefault(t => t.status_id == item.status_id)!.status_name,
                    item.review_status_id,
                    review_status_name = _context.mr_review_status.FirstOrDefault(t => t.status_id == item.review_status_id)!.status_name,
                    item.is_top,
                    item.is_show,
                    item.user_id,
                    user_name = _context.mr_users.FirstOrDefault(t => t.user_id == item.user_id)!.user_name,
                    item.visit_num,
                    item.set_publish_time,
                    item.article_remark,
                    item.created_at,
                    format_time = item.created_at.ToString("yyyy-MM-dd HH:mm"),

                    type_name = item.type_id > 0 ? _context.mr_article_types.FirstOrDefault(t => t.type_id == item.type_id)!.type_name : "-",
                    type_subname = item.type_subid > 0 ? _context.mr_article_types.FirstOrDefault(t => t.type_id == item.type_subid)!.type_name : "-",
                    review_content = _context.mr_reviews.OrderByDescending(t => t.created_at).FirstOrDefault(t => t.type_id == 1 && t.object_id == item.article_id)!.review_content,
                    attachments = _context.mr_attachments.Where(t => t.object_id == item.article_id && t.type_id == 1).Select(c => new
                    {
                        c.attachment_name,
                        attachment_path = !string.IsNullOrEmpty(c.attachment_path) ? MR.Manage.Filters.AppSettingsFilter.GetSetting("Aliyun:OSS_URL") + c.attachment_path : "-",
                    }).ToList()
                })).FirstOrDefaultAsync();
                AddLogs((int)ENUMHelper.LogType.Search, (int)ENUMHelper.Platform.Admin + "" + (int)ENUMHelper.OperateType.Details + "" + (int)ENUMHelper.InfoType.Info, "获取文章详情ID=" + id + ",result=" + result.ToJson());
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
        /// 按天统计文章数据
        /// </summary>
        /// <returns>返回每日统计文章JSON数据集合</returns>
        public async Task<IActionResult> ChartByDay()
        {
            try
            {
                // money = g.Sum(t => t.total_pay)
                var list = await _context.mr_articles.GroupBy(t => new { t.created_at.Year, t.created_at.Month, t.created_at.Day }).Select(g => (new { year = g.Key.Year, month = g.Key.Month, day = g.Key.Day, count = g.Count() })).OrderBy(t => t.year).ThenBy(t => t.month).ThenBy(t => t.day).ToListAsync();
                AddLogs((int)ENUMHelper.LogType.Count, (int)ENUMHelper.Platform.Admin + "" + (int)ENUMHelper.OperateType.Chart + "" + (int)ENUMHelper.InfoType.Info, "按天统计文章数据,list=" + list.ToJson());

                return Json(new { code = 0, msg = "success", data = list });
            }
            catch (Exception ex)
            {
                log.Fatal(ex, " -> ChartByDay");
                return Json(new { code = 0, errcode = (int)ENUMHelper.ExceptionType.Chart, errmsg = ex.Message });
            }
        }

        /// <summary>
        /// 新增文章数据
        /// </summary>
        /// <param name="articles">Articles对象</param>
        /// <returns>返回新增文章后的单条JSON数据</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Articles articles)
        {
            try
            {
                var attachment = articles.article_remark;
                log.Information("articles=" + articles.ToJson());

                var user_id = User.FindFirst(ClaimTypes.Sid)!.Value;
                long.TryParse(user_id, out long get_user_id);

                articles.user_id = get_user_id;
                articles.article_remark = "";
                articles.review_status_id = 1; // 如果开启审核默认，默认=1，不开启=2
                articles.created_at = DateTime.Now;
                _context.mr_articles.Add(articles);
                await _context.SaveChangesAsync();

                // 处理附件:attachment=MiniX_logs_20220626131111683475.xlsx|/upload/2022/0626/20220626235913891868.xlsx,后端API接口的错误信息返回规范.pdf|/upload/2022/0626/20220626235925848134.pdf
                if (!string.IsNullOrEmpty(attachment) && attachment.Contains("|"))
                {
                    string[] splitAttachment = attachment.Split(new char[] { ',' });
                    for (int i = 0; i < splitAttachment.Length; i++)
                    {
                        string value = splitAttachment[i];
                        if (!string.IsNullOrEmpty(value) && value.Contains("|"))
                        {
                            try
                            {
                                string[] splitFile = value.Split(new char[] { '|' });
                                var attachment_name = splitFile[0];
                                var attachment_path = splitFile[1];
                                _context.mr_attachments.Add(new Attachments()
                                {
                                    object_id = articles.article_id,
                                    type_id = 1,
                                    attachment_name = attachment_name,
                                    attachment_path = attachment_path,
                                    attachment_remark = "",
                                    created_at = DateTime.Now
                                });
                                _context.SaveChanges();
                            }
                            catch (Exception ex)
                            {
                                return Json(new { code = 0, errcode = (int)ENUMHelper.ExceptionType.Create, errmsg = ex.Message });
                            }
                        }
                    }
                }

                AddLogs((int)ENUMHelper.LogType.Create, (int)ENUMHelper.Platform.Admin + "" + (int)ENUMHelper.OperateType.Create + "" + (int)ENUMHelper.InfoType.Info, "新增文章,articles=" + articles.ToJson());

                return Json(new { code = 0, msg = "发布成功", data = articles });
            }
            catch (Exception ex)
            {
                log.Fatal(ex, " -> Create");
                return Json(new { code = 0, errcode = (int)ENUMHelper.ExceptionType.Create, errmsg = ex.Message });
            }
        }

        /// <summary>
        /// 根据ID更改文章数据
        /// </summary>
        /// <param name="articles">Articles对象</param>
        /// <returns>返回修改文章后的单条JSON数据</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Articles articles)
        {
            try
            {
                var attachment = articles.article_remark;
                log.Information("articles=" + articles.ToJson());
                var item = _context.mr_articles.FirstOrDefault(t => t.article_id == articles.article_id);
                if (item != null)
                {
                    item.article_title = articles.article_title;
                    item.cover_img = articles.cover_img;
                    item.article_content = articles.article_content;
                    item.article_author = articles.article_author;
                    item.article_source = articles.article_source;
                    item.article_summary = articles.article_summary;
                    item.type_id = articles.type_id;
                    item.type_subid = articles.type_subid;
                    item.status_id = articles.status_id;
                    item.is_top = articles.is_top;
                    item.is_show = articles.is_show;
                    item.set_publish_time = articles.set_publish_time;
                    item.article_remark = "";
                    _context.mr_articles.Update(item);
                    await _context.SaveChangesAsync();
                }

                // 处理附件:attachment=MiniX_logs_20220626131111683475.xlsx|/upload/2022/0626/20220626235913891868.xlsx,后端API接口的错误信息返回规范.pdf|/upload/2022/0626/20220626235925848134.pdf
                if (!string.IsNullOrEmpty(attachment) && attachment.Contains("|"))
                {
                    string[] splitAttachment = attachment.Split(new char[] { ',' });
                    for (int i = 0; i < splitAttachment.Length; i++)
                    {
                        string value = splitAttachment[i];
                        if (!string.IsNullOrEmpty(value) && value.Contains("|"))
                        {
                            try
                            {
                                string[] splitFile = value.Split(new char[] { '|' });
                                var attachment_name = splitFile[0];
                                var attachment_path = splitFile[1];
                                _context.mr_attachments.Add(new Attachments()
                                {
                                    object_id = articles.article_id,
                                    type_id = 1,
                                    attachment_name = attachment_name,
                                    attachment_path = attachment_path,
                                    attachment_remark = "",
                                    created_at = DateTime.Now
                                });
                                _context.SaveChanges();
                            }
                            catch (Exception ex)
                            {
                                return Json(new { code = 0, errcode = (int)ENUMHelper.ExceptionType.Edit, errmsg = ex.Message });
                            }
                        }
                    }
                }
                AddLogs((int)ENUMHelper.LogType.Update, (int)ENUMHelper.Platform.Admin + "" + (int)ENUMHelper.OperateType.Edit + "" + (int)ENUMHelper.InfoType.Info, "编辑文章ID=" + articles.article_id + ",articles=" + articles.ToJson());

                return Json(new { code = 0, msg = "更新成功", data = item });
            }
            catch (Exception ex)
            {
                log.Fatal(ex, " -> Edit");
                return Json(new { code = 0, errcode = (int)ENUMHelper.ExceptionType.Edit, errmsg = ex.Message });
            }
        }

        /// <summary>
        /// 更新文章是否置顶
        /// </summary>
        /// <param name="id">文章ID</param>
        /// <param name="is_top">是否置顶:1=是|0=否</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetIsTop(long id, bool is_top)
        {
            try
            {
                log.Information("id=" + id + ",is_top=" + is_top);
                var item = await _context.mr_articles.FirstOrDefaultAsync(t=> t.article_id == id);
                if (item != null)
                {
                    item.is_top = is_top;
                    _context.mr_articles.Update(item);
                    await _context.SaveChangesAsync();
                    AddLogs((int)ENUMHelper.LogType.Update, (int)ENUMHelper.Platform.Admin + "" + (int)ENUMHelper.OperateType.Set + "" + (int)ENUMHelper.InfoType.Info, "更新文章是否置顶ID=" + id);

                    return Json(new { code = 0, msg = "操作成功", data = item });
                }
                else
                {
                    return Json(new { code = 0, msg = "操作失败，ID=" + id + "不存在！" });
                }
            }
            catch (Exception ex)
            {
                log.Fatal(ex, " -> SetStatus");
                return Json(new { code = 0, errcode = (int)ENUMHelper.ExceptionType.Set, errmsg = ex.Message });
            }
        }

        /// <summary>
        /// 更新文章是否显示
        /// </summary>
        /// <param name="id">文章ID</param>
        /// <param name="is_top">是否置顶:1=是|0=否</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetIsShow(long id, bool is_show)
        {
            try
            {
                log.Information("id=" + id + ",is_show=" + is_show);
                var item = await _context.mr_articles.FirstOrDefaultAsync(t => t.article_id == id);
                if (item != null)
                {
                    item.is_show = is_show;
                    _context.mr_articles.Update(item);
                    await _context.SaveChangesAsync();
                    AddLogs((int)ENUMHelper.LogType.Update, (int)ENUMHelper.Platform.Admin + "" + (int)ENUMHelper.OperateType.Set + "" + (int)ENUMHelper.InfoType.Info, "更新文章是否显示ID=" + id);

                    return Json(new { code = 0, msg = "操作成功", data = item });
                }
                else
                {
                    return Json(new { code = 0, msg = "操作失败，ID=" + id + "不存在！" });
                }
            }
            catch (Exception ex)
            {
                log.Fatal(ex, " -> SetStatus");
                return Json(new { code = 0, errcode = (int)ENUMHelper.ExceptionType.Set, errmsg = ex.Message });
            }
        }

        /// <summary>
        /// 根据ID删除文章数据
        /// </summary>
        /// <param name="id">文章ID</param>
        /// <returns>返回删除操作信息</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                log.Information("id=" + id);
                var item = _context.mr_articles.Find(id);
                if (item == null)
                {
                    return Json(new { code = 0, msg = "操作失败，ID=" + id + "不存在！" });
                }
                _context.mr_articles.Remove(item);
                await _context.SaveChangesAsync();
                AddLogs((int)ENUMHelper.LogType.Delete, (int)ENUMHelper.Platform.Admin + "" + (int)ENUMHelper.OperateType.Logout + "" + (int)ENUMHelper.InfoType.Info, "删除文章ID=" + id + ",item=" + item.ToJson());

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
