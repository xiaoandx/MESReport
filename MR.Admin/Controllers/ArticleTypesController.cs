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
    /// 内容摘要: 文章类别
    /// </summary>
    [Authorize]
    public class ArticleTypesController : BaseController<ArticleTypesController>
    {

        /// <summary>
        /// 分类 实例化数据上下文
        /// </summary>
        /// <param name="context"></param>
        public ArticleTypesController(MRManageContext context) : base(context)
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
        /// 获取文章分类列表
        /// </summary>
        /// <param name="parent_id">父目录ID</param>
        /// <returns></returns>
        public async Task<IActionResult> List(int parent_id)
        {
            try
            {
                if (parent_id == -1)
                {
                    var result = from item in _context.Set<ArticleTypes>().Where(t => t.parent_id == -1)
                                 select new
                                 {
                                     item.type_id,
                                     item.type_name,
                                     item.parent_id,
                                     item.is_show,
                                     haveChild = _context.mr_article_types.Any(t => t.parent_id == item.type_id),
                                     article_num = _context.mr_articles.Count(t => t.type_id == item.type_id)
                                 };

                    long lCount = result.LongCount();
                    var list = await result.OrderBy(n => n.type_id).ToListAsync();
                    AddLogs((int)ENUMHelper.LogType.Search, (int)ENUMHelper.Platform.Admin + "" + (int)ENUMHelper.OperateType.List + "" + (int)ENUMHelper.InfoType.Info, "查询文章分类LINQ=" + result.Expression.ToString() + ",DATA=" + list.ToJson());

                    return Json(new { code = 0, msg = "success", data = list, count = lCount });
                }
                else
                {
                    var result = from item in _context.Set<ArticleTypes>().Where(t => t.parent_id == parent_id)
                                 select new
                                 {
                                     item.type_id,
                                     item.type_name,
                                     item.parent_id,
                                     item.is_show,
                                     haveChild = _context.mr_article_types.Any(t => t.parent_id == item.type_id),
                                     article_num = _context.mr_articles.Count(t => t.type_subid == item.type_id)
                                 };

                    long lCount = result.LongCount();
                    var list = await result.OrderBy(n => n.type_id).ToListAsync();
                    AddLogs((int)ENUMHelper.LogType.Search, (int)ENUMHelper.Platform.Admin + "" + (int)ENUMHelper.OperateType.List + "" + (int)ENUMHelper.InfoType.Info, "查询文章分类LINQ=" + result.Expression.ToString() + ",DATA=" + list.ToJson());

                    return Json(new { code = 0, msg = "success", data = result, count = lCount });
                }
            }
            catch (Exception ex)
            {
                log.Fatal(ex, " -> List");
                return Json(new { code = 0, errcode = (int)ENUMHelper.ExceptionType.List, errmsg = ex.Message });
            }
        }

        /// <summary>
        /// 获取分类数据
        /// </summary>
        /// <param name="parent_id">父目录ID</param>
        /// <returns>返回分类JSON数据集合</returns>
        public async Task<IActionResult> Select(int parent_id)
        {
            try
            {
                var list = await _context.mr_article_types.OrderBy(t => t.type_id).Where(t => t.parent_id == parent_id && t.is_show).ToListAsync();
                AddLogs((int)ENUMHelper.LogType.Search, (int)ENUMHelper.Platform.Admin + "" + (int)ENUMHelper.OperateType.Select + "" + (int)ENUMHelper.InfoType.Info, "查询文章分类下拉填充,DATA=" + list.ToJson());

                return Json(new { code = 0, msg = "success", data = list });
            }
            catch (Exception ex)
            {
                log.Fatal(ex, " -> Select");
                return Json(new { code = 0, errcode = (int)ENUMHelper.ExceptionType.Select, errmsg = ex.Message });
            }
        }

        /// <summary>
        /// 根据ID获取分类详情数据
        /// </summary>
        /// <param name="id">分类ID</param>
        /// <returns>返回单条分类JSON数据</returns>
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                log.Information("id=" + id);
                var result = await _context.mr_article_types.Where(t => t.type_id == id).Select(item => (new
                {
                    item.type_id,
                    item.type_name,
                    item.parent_id,
                    item.is_show
                })).FirstOrDefaultAsync();
                AddLogs((int)ENUMHelper.LogType.Search, (int)ENUMHelper.Platform.Admin + "" + (int)ENUMHelper.OperateType.Details + "" + (int)ENUMHelper.InfoType.Info, "获取文章分类详情ID=" + id + ",result=" + result.ToJson());
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
        /// 新增分类数据
        /// </summary>
        /// <param name="articleTypes">ArticleTypes对象</param>
        /// <returns>返回新增分类后的单条JSON数据</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ArticleTypes articleTypes)
        {
            try
            {
                log.Information("articleTypes=" + articleTypes.ToJson());
                _context.mr_article_types.Add(articleTypes);
                await _context.SaveChangesAsync();
                AddLogs((int)ENUMHelper.LogType.Create, (int)ENUMHelper.Platform.Admin + "" + (int)ENUMHelper.OperateType.Create + "" + (int)ENUMHelper.InfoType.Info, "新增文章分类,articleTypes=" + articleTypes.ToJson());

                return Json(new { code = 0, msg = "添加成功", data = articleTypes });
            }
            catch (Exception ex)
            {
                log.Fatal(ex, " -> Create");
                return Json(new { code = 0, errcode = (int)ENUMHelper.ExceptionType.Create, errmsg = ex.Message });
            }
        }

        /// <summary>
        /// 根据ID更改分类数据
        /// </summary>
        /// <param name="id">分类ID</param>
        /// <param name="articleTypes">ArticleTypes对象</param>
        /// <returns>返回修改分类后的单条JSON数据</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ArticleTypes articleTypes)
        {
            try
            {
                log.Information("articleTypes=" + articleTypes.ToJson());
                var item = _context.mr_article_types.Find(id);
                if (item != null)
                {
                    item.type_name = articleTypes.type_name;
                    item.is_show = articleTypes.is_show;
                    _context.mr_article_types.Update(item);
                    await _context.SaveChangesAsync();
                    AddLogs((int)ENUMHelper.LogType.Update, (int)ENUMHelper.Platform.Admin + "" + (int)ENUMHelper.OperateType.Edit + "" + (int)ENUMHelper.InfoType.Info, "编辑文章分类ID=" + id + ",articleTypes=" + articleTypes.ToJson());

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
        /// 根据ID更改分类状态
        /// </summary>
        /// <param name="id">分类ID</param>
        /// <param name="status">是否显示</param>
        /// <returns>返回修改分类后的单条JSON数据</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetShow(int id, bool status)
        {
            try
            {
                log.Information("status=" + status);
                var item = _context.mr_article_types.Find(id);
                if (item != null)
                {
                    item.is_show = status;
                    _context.mr_article_types.Update(item);
                    await _context.SaveChangesAsync();
                    AddLogs((int)ENUMHelper.LogType.Delete, (int)ENUMHelper.Platform.Admin + "" + (int)ENUMHelper.OperateType.Logout + "" + (int)ENUMHelper.InfoType.Info, "删除文章分类ID=" + id + ",status=" + status);

                    return Json(new { code = 0, msg = "更新成功", data = item });
                }
                else
                {
                    return Json(new { code = 0, msg = "操作失败，ID=" + id + "不存在！" });
                }
            }
            catch (Exception ex)
            {
                log.Fatal(ex, " -> SetShow");
                return Json(new { code = 0, errcode = (int)ENUMHelper.ExceptionType.Set, errmsg = ex.Message });
            }
        }

        /// <summary>
        /// 根据ID删除分类数据
        /// </summary>
        /// <param name="id">分类ID</param>
        /// <returns>返回删除操作信息</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                log.Information("id=" + id);
                var item = _context.mr_article_types.Find(id);
                if (item == null)
                {
                    return Json(new { code = 0, msg = "操作失败，ID=" + id + "不存在！" });
                }
                // _context.mr_article_types.Remove(item); // 演示系统暂时关闭删除功能，正式发布需要开启
                await _context.SaveChangesAsync();
                AddLogs((int)ENUMHelper.LogType.Delete, (int)ENUMHelper.Platform.Admin + "" + (int)ENUMHelper.OperateType.Logout + "" + (int)ENUMHelper.InfoType.Info, "删除文章分类ID=" + id + ",item=" + item.ToJson());

                return Json(new { code = 0, msg = "为保证体验功能完整性，暂时关闭删除功能！" });
            }
            catch (Exception ex)
            {
                log.Fatal(ex, " -> Delete");
                return Json(new { code = 0, errcode = (int)ENUMHelper.ExceptionType.Delete, errmsg = ex.Message });
            }
        }
    }
}
