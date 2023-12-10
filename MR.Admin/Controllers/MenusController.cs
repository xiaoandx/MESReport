using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MR.Manage.Data;
using MR.Models;
using Senparc.CO2NET.Extensions;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using MR.Utility.Helper;

namespace MR.Manage.Controllers
{
    /// <summary>
    /// 内容摘要: 后台菜单管理类
    /// </summary>
    public class MenusController : BaseController<MenusController>
    {

        /// <summary>
        /// 菜单 实例化数据上下文
        /// </summary>
        /// <param name="context"></param>
        public MenusController(MRManageContext context) : base(context)
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
        /// 权限设置页面
        /// </summary>
        /// <returns></returns>
        public IActionResult Authority()
        {
            return View();
        }

        /// <summary>
        /// 获取菜单数据
        /// </summary>
        /// <param name="parent_id">父目录ID</param>
        /// <returns></returns>
        public async Task<IActionResult> Lists(int parent_id)
        {
            try
            {
                if (parent_id == -1)
                {
                    var result = from item in _context.Set<Menus>().Where(t => t.parent_id == -1 && t.menu_status == true)
                                 select new
                                 {
                                     item.menu_id,
                                     item.menu_name,
                                     item.menu_icon,
                                     item.menu_url,
                                     item.menu_rank,
                                     item.parent_id,
                                     item.menu_code,
                                     item.menu_status,
                                     haveChild = _context.mr_menus.Any(t => t.parent_id == item.menu_id),
                                     //haveChild = true,
                                 };

                    var lCount = result.LongCount();
                    var list = await result.OrderBy(t => t.menu_id).ToListAsync();
                    AddLogs((int)ENUMHelper.LogType.Search, (int)ENUMHelper.Platform.Admin + "" + (int)ENUMHelper.OperateType.List + "" + (int)ENUMHelper.InfoType.Info, "查询LINQ=" + result.Expression.ToString() + ",DATA=" + list.ToJson());
                    return Json(new { code = 0, msg = "success", data = list, count = lCount });
                }
                else
                {
                    var result = from item in _context.Set<Menus>().Where(t => t.parent_id == parent_id && t.menu_status == true)
                                 select new
                                 {
                                     item.menu_id,
                                     item.menu_name,
                                     item.menu_icon,
                                     item.menu_url,
                                     item.menu_rank,
                                     item.parent_id,
                                     item.menu_code,
                                     item.menu_status,

                                     haveChild = _context.mr_menus.Any(t => t.parent_id == item.menu_id),
                                 };

                    var lCount = result.LongCount();
                    var list = await result.OrderBy(t => t.menu_id).ToListAsync();
                    AddLogs((int)ENUMHelper.LogType.Search, (int)ENUMHelper.Platform.Admin + "" + (int)ENUMHelper.OperateType.List + "" + (int)ENUMHelper.InfoType.Info, "查询LINQ=" + result.Expression.ToString() + ",DATA=" + list.ToJson());
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
        /// 获取菜单列表
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public async Task<IActionResult> List(long id)
        {
            var list = new List<Menus>();
            long lCount = 0;
            try
            {
                var result = _context.mr_menus.Where(t => t.parent_id == id && t.menu_status == true);

                list = await result.OrderBy(t => t.menu_rank).ToListAsync();
                lCount = result.LongCount();
            }
            catch (Exception ex)
            {
                log.Fatal(ex, " -> List");
            }

            return Json(new { code = 0, msg = "success", data = list, count = lCount });
        }

        /// <summary>
        /// 获取菜单列表(二级)
        /// </summary>
        /// <param name="parent_id"></param>
        /// <returns></returns>
        public async Task<IActionResult> ItemList(int parent_id)
        {
            log.Information("parent_id=" + parent_id);
            var result = _context.mr_menus.Where(t => t.parent_id == parent_id && t.menu_status == true);

            var list = await result.OrderBy(t => t.menu_rank).ToListAsync();
            long lCount = result.LongCount();

            return Json(new { code = 0, msg = "", data = list, count = lCount });
        }

        /// <summary>
        /// 获取菜单数据用于树形目录(二级)
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> ListTree()
        {
            try
            {
                var list = await _context.mr_menus.Where(c => c.parent_id == -1).ToListAsync();
                var node = new List<dynamic>();
                foreach (var item in list)
                {
                    var obj = new
                    {
                        name = item.menu_name,
                        id = item.menu_id,
                        icon = item.menu_icon,
                        url = item.menu_url,
                        rank = item.menu_rank,

                        children = await TreeChild(item.menu_id)
                    };
                    node.Add(obj);
                }
                return Json(new
                {
                    code = 0,
                    msg = "success",
                    data = node
                });
            }
            catch (Exception ex)
            {
                log.Fatal(ex, " -> ListTree");
                return Json(new { code = 0, errcode = (int)ENUMHelper.ExceptionType.List, errmsg = ex.Message });
            }
        }

        /// <summary>
        /// 获取菜单数据用于树形目录(三级)
        /// </summary>
        /// <param name="parent_id">父目录ID</param>
        /// <returns></returns>
        public async Task<List<dynamic>> TreeChild(long parent_id)
        {
            log.Information("parent_id=" + parent_id);
            var list = await _context.mr_menus.Where(c => c.parent_id == parent_id).OrderBy(t => t.menu_rank).ToListAsync();
            var returnList = new List<dynamic>();
            foreach (var item in list)
            {
                var obj = new
                {
                    name = item.menu_name,
                    id = item.menu_id,
                    icon = item.menu_icon,
                    url = item.menu_url,
                    rank = item.menu_rank,

                    children = await TreeChild(item.menu_id)
                };
                returnList.Add(obj);
            }
            return returnList;
        }

        /// <summary>
        /// 获取菜单数据(通常用于select组件下拉填充数据)
        /// </summary>
        /// <returns>返回菜单JSON数据集合</returns>
        public async Task<IActionResult> Select()
        {
            try
            {
                var list = await _context.mr_menus.OrderBy(t => t.menu_id).ToListAsync();
                AddLogs((int)ENUMHelper.LogType.Search, (int)ENUMHelper.Platform.Admin + "" + (int)ENUMHelper.OperateType.Select + "" + (int)ENUMHelper.InfoType.Info, "查询下拉填充,DATA=" + list.ToJson());
                return Json(new { code = 0, msg = "success", data = list });
            }
            catch (Exception ex)
            {
                log.Fatal(ex, " -> Select");
                return Json(new { code = 0, errcode = (int)ENUMHelper.ExceptionType.Select, errmsg = ex.Message });
            }
        }

        /// <summary>
        /// 根据ID获取菜单详情(通常用于编辑页面或详情页面)
        /// </summary>
        /// <param name="id">菜单ID</param>
        /// <returns>返回单条菜单JSON数据</returns>
        [HttpGet("/menus/details/")]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                log.Information("id=" + id);
                var item = await _context.mr_menus.FirstOrDefaultAsync(t => t.menu_id == id);
                AddLogs((int)ENUMHelper.LogType.Search, (int)ENUMHelper.Platform.Admin + "" + (int)ENUMHelper.OperateType.Details + "" + (int)ENUMHelper.InfoType.Info, "获取详情ID=" + id + ",item=" + item.ToJson());
                if (item == null)
                {
                    return Json(new { code = 0, msg = "数据为空" });
                }
                else
                {
                    return Json(new { code = 0, msg = "success", data = item });
                }
            }
            catch (Exception ex)
            {
                log.Fatal(ex, " -> GetById");
                return Json(new { code = 0, errcode = (int)ENUMHelper.ExceptionType.Details, errmsg = ex.Message });
            }
        }

        /// <summary>
        /// 新增菜单数据(通常用于新增页面)
        /// </summary>
        /// <param name="menus">Menus对象</param>
        /// <returns>返回新增菜单后的单条JSON数据</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Menus menus)
        {
            try
            {
                log.Information("menus=" + menus.ToJson());
                _context.mr_menus.Add(menus);
                await _context.SaveChangesAsync();
                AddLogs((int)ENUMHelper.LogType.Create, (int)ENUMHelper.Platform.Admin + "" + (int)ENUMHelper.OperateType.Create + "" + (int)ENUMHelper.InfoType.Info, "新增,menus=" + menus.ToJson());

                return Json(new { code = 0, msg = "添加成功", data = menus });
            }
            catch (Exception ex)
            {
                log.Fatal(ex, " -> Create");
                return Json(new { code = 0, errcode = (int)ENUMHelper.ExceptionType.Create, errmsg = ex.Message });
            }
        }

        /// <summary>
        /// 根据ID更改菜单数据(通常用于编辑页面)
        /// </summary>
        /// <param name="id">菜单ID</param>
        /// <param name="menus">Menus对象</param>
        /// <returns>返回修改菜单后的单条JSON数据</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Menus menus)
        {
            try
            {
                log.Information("menus=" + menus.ToJson());
                var item = _context.mr_menus.Find(id);
                if (item != null)
                {
                    item.menu_id = menus.menu_id;
                    item.menu_name = menus.menu_name;
                    item.menu_icon = menus.menu_icon;
                    item.menu_url = menus.menu_url;
                    item.menu_rank = menus.menu_rank;
                    item.parent_id = menus.parent_id;
                    item.menu_code = menus.menu_code;
                    item.menu_status = menus.menu_status;
                    item.menu_remark = menus.menu_remark;

                    _context.mr_menus.Update(item);
                    await _context.SaveChangesAsync();
                    AddLogs((int)ENUMHelper.LogType.Update, (int)ENUMHelper.Platform.Admin + "" + (int)ENUMHelper.OperateType.Edit + "" + (int)ENUMHelper.InfoType.Info, "编辑ID=" + id + ",menus=" + menus.ToJson());

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
        /// 根据ID删除菜单数据
        /// </summary>
        /// <param name="id">菜单ID</param>
        /// <returns>返回删除操作信息</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                log.Information("id=" + id);
                var item = _context.mr_menus.Find(id);
                if (item == null)
                {
                    return Json(new { code = 0, msg = "操作失败，ID=" + id + "不存在！" });
                }
                _context.mr_menus.Remove(item); // 演示系统暂时关闭删除功能，正式发布需要开启
                await _context.SaveChangesAsync();
                AddLogs((int)ENUMHelper.LogType.Delete, (int)ENUMHelper.Platform.Admin + "" + (int)ENUMHelper.OperateType.Delete + "" + (int)ENUMHelper.InfoType.Info, "删除ID=" + id + ",item=" + item.ToJson());

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
