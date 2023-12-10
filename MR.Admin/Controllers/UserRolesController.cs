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
    /// 内容摘要: 用户角色类
    /// </summary>
    public class UserRolesController : BaseController<UserRolesController>
    {

        /// <summary>
        /// 角色 实例化数据上下文
        /// </summary>
        /// <param name="context"></param>
        public UserRolesController(MRManageContext context) : base(context)
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
        /// 角色管理页面
        /// </summary>
        /// <returns></returns>
        public IActionResult Roles()
        {
            return View();
        }

        /// <summary>
        /// 获取角色列表数据(通常用于管理页面或列表页面)
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="limit">每页显示数量</param>
        /// <returns>返回角色JSON数据集合</returns>
        public async Task<IActionResult> List(int page, int limit)
        {
            try
            {
                var result = _context.mr_user_roles.Where(t => t.role_id > 0);
                var list =  await result.OrderByDescending(t => t.role_id).Skip((page - 1) * limit).Take(limit).ToListAsync();
                var lCount = result.LongCount();
                AddLogs((int)ENUMHelper.LogType.Search, (int)ENUMHelper.Platform.Admin + "" + (int)ENUMHelper.OperateType.List + "" + (int)ENUMHelper.InfoType.Info, "查询广告下拉填充,DATA=" + list.ToJson());

                return Json(new { code = 0, msg = "success", data = list, count = lCount });
            }
            catch (Exception ex)
            {
                log.Fatal(ex, " -> List");
                return Json(new { code = 0, errcode = (int)ENUMHelper.ExceptionType.List, errmsg = ex.Message });
            }
        }

        /// <summary>
        /// 获取角色数据(通常用于select组件下拉填充数据)
        /// </summary>
        /// <returns>返回角色JSON数据集合</returns>
        public async Task<IActionResult> Select()
        {
            try
            {
                var list = await _context.mr_user_roles.OrderBy(t => t.role_id).ToListAsync();
                AddLogs((int)ENUMHelper.LogType.Search, (int)ENUMHelper.Platform.Admin + "" + (int)ENUMHelper.OperateType.Select + "" + (int)ENUMHelper.InfoType.Info, "查询广告下拉填充,DATA=" + list.ToJson());

                return Json(new { code = 0, msg = "success", data = list });
            }
            catch (Exception ex)
            {
                log.Fatal(ex, " -> Select");
                return Json(new { code = 0, errcode = (int)ENUMHelper.ExceptionType.Select, errmsg = ex.Message });
            }
        }

        /// <summary>
        /// 根据ID获取角色详情(通常用于编辑页面或详情页面)
        /// </summary>
        /// <param name="id">角色ID</param>
        /// <returns>返回单条角色JSON数据</returns>
        [HttpGet("/userroles/details/")]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                log.Information("id=" + id);
                var result = await _context.mr_user_roles.Where(t => t.role_id == id).Select(item => (new
                {
                    item.role_id,
                    item.role_name,
                    item.role_authority,
                    item.role_desc,
                    item.role_remark,
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
                log.Fatal(ex, " -> GetById");
                return Json(new { code = 0, errcode = (int)ENUMHelper.ExceptionType.Details, errmsg = ex.Message });
            }
        }

        /// <summary>
        /// 新增角色数据(通常用于新增页面)
        /// </summary>
        /// <param name="userRoles">UserRoles对象</param>
        /// <returns>返回新增角色后的单条JSON数据</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserRoles userRoles)
        {
            try
            {
                log.Information("userRoles=" + userRoles.ToJson());
                _context.mr_user_roles.Add(userRoles);
                await _context.SaveChangesAsync();
                AddLogs((int)ENUMHelper.LogType.Create, (int)ENUMHelper.Platform.Admin + "" + (int)ENUMHelper.OperateType.Create + "" + (int)ENUMHelper.InfoType.Info, "新增角色,userRoles=" + userRoles.ToJson());

                return Json(new { code = 0, msg = "添加成功", data = userRoles });
            }
            catch (Exception ex)
            {
                log.Fatal(ex, " -> Create");
                return Json(new { code = 0, errcode = (int)ENUMHelper.ExceptionType.Create, errmsg = ex.Message });
            }
        }

        /// <summary>
        /// 根据ID更改角色数据(通常用于编辑页面)
        /// </summary>
        /// <param name="id">角色ID</param>
        /// <param name="userRoles">UserRoles对象</param>
        /// <returns>返回修改角色后的单条JSON数据</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UserRoles userRoles)
        {
            try
            {
                log.Information("userRoles=" + userRoles.ToJson());
                var item = _context.mr_user_roles.Find(id);
                if (item != null)
                {
                    item.role_name = userRoles.role_name;
                    item.role_authority = userRoles.role_authority;
                    item.role_desc = userRoles.role_desc;
                    item.role_remark = userRoles.role_remark;
                    _context.mr_user_roles.Update(item);
                    await _context.SaveChangesAsync();
                    AddLogs((int)ENUMHelper.LogType.Update, (int)ENUMHelper.Platform.Admin + "" + (int)ENUMHelper.OperateType.Edit + "" + (int)ENUMHelper.InfoType.Info, "编辑角色ID=" + id + ",userRoles=" + userRoles.ToJson());

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
        /// 更新角色权限
        /// </summary>
        /// <param name="userRoles">用户角色对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(UserRoles userRoles)
        {
            try
            {
                log.Information("userRoles=" + userRoles.ToJson());
                var item = _context.mr_user_roles.Find(userRoles.role_id);
                if (item != null)
                {
                    item.role_authority = userRoles.role_authority;
                    _context.mr_user_roles.Update(item);
                    await _context.SaveChangesAsync();
                    AddLogs((int)ENUMHelper.LogType.Update, (int)ENUMHelper.Platform.Admin + "" + (int)ENUMHelper.OperateType.Set + "" + (int)ENUMHelper.InfoType.Info, "编辑,userRoles=" + userRoles.ToJson());

                    return Json(new { code = 0, msg = "更新成功" });
                }
                else
                {
                    return Json(new { code = 0, msg = "操作失败，ID=" + userRoles.role_id + "不存在！" });
                }
            }
            catch (Exception ex)
            {
                log.Fatal(ex, " -> Update");
                return Json(new { code = 0, errcode = (int)ENUMHelper.ExceptionType.Update, errmsg = ex.Message });
            }
        }

        /// <summary>
        /// 根据ID删除角色数据
        /// </summary>
        /// <param name="id">角色ID</param>
        /// <returns>返回删除操作信息</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                log.Information("id=" + id);
                var item = _context.mr_user_roles.Find(id);
                if (item == null)
                {
                    return Json(new { code = 0, msg = "操作失败，ID=" + id + "不存在！" });
                }
                _context.mr_user_roles.Remove(item);
                await _context.SaveChangesAsync();
                AddLogs((int)ENUMHelper.LogType.Delete, (int)ENUMHelper.Platform.Admin + "" + (int)ENUMHelper.OperateType.Logout + "" + (int)ENUMHelper.InfoType.Info, "删除角色ID=" + id + ",item=" + item.ToJson());

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
