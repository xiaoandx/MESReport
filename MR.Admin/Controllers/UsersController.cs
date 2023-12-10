using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MR.Manage.Data;
using MR.Models;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http.Extensions;
using Senparc.CO2NET.Extensions;
using System.Net.Http;
using System.Security.Claims;
using IdentityModel;
using System.IdentityModel.Tokens.Jwt;
using MR.Utility.Authorization;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using MR.Utility.Helper;

namespace MR.Manage.Controllers
{
    /// <summary>
    /// 内容摘要: 后台管理员操作类
    /// </summary>
    public class UsersController : BaseController<UsersController>
    {
        // 用户实例化数据上下文
        public UsersController(MRManageContext context) : base(context)
        {

        }

        /// <summary>
        /// 管理页面
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
        /// 用户详情页面
        /// </summary>
        /// <returns></returns>
        public IActionResult Info()
        {
            return View();
        }

        /// <summary>
        /// 修改密码页面
        /// </summary>
        /// <returns></returns>
        public IActionResult Password()
        {
            return View();
        }

        /// <summary>
        /// 重置密码页面
        /// </summary>
        /// <returns></returns>
        public IActionResult ResetPassword()
        {
            return View();
        }

        /// <summary>
        /// 根据搜索条件获取用户列表
        /// </summary>
        /// <param name="page">当前页码</param>
        /// <param name="limit">每页显示数量</param>
        /// <param name="user_name">用户名</param>
        /// <param name="nick_name">昵称</param>
        /// <param name="mobile_phone">手机号码</param>
        /// <param name="user_job">职位</param>
        /// <param name="created_at">创建日期</param>
        /// <param name="role_id">角色ID</param>
        /// <param name="user_status">状态：1=启用，0=禁用</param>
        /// <returns></returns>
        public async Task<IActionResult> List(int page, int limit, string user_name, string nick_name, string mobile_phone, string user_job, string created_at, int role_id=-1, int user_status=-1)
        {
            try
            {
                var result = from item in _context.Set<Users>()
                             select new
                             {
                                 item.user_id,
                                 item.user_name,
                                 item.user_avatar,
                                 item.user_password,
                                 item.nick_name,
                                 item.real_name,
                                 mobile_phone = !string.IsNullOrEmpty(item.mobile_phone) ? item.mobile_phone : "-",
                                 item.user_email,
                                 user_job = !string.IsNullOrEmpty(item.user_job) ? item.user_job : "-",
                                 item.gender_id,
                                 item.role_id,
                                 role_name = item.role_id > -1 ? _context.mr_user_roles.FirstOrDefault(t => t.role_id == item.role_id)!.role_name : "-",
                                 item.user_slogan,
                                 item.user_intro,
                                 item.limit_ip,
                                 item.user_status,
                                 item.user_token,
                                 item.last_login_at,
                                 item.created_at,
                                 format_time = item.created_at.ToString("yyyy-MM-dd HH:mm")
                             };

                if (!string.IsNullOrEmpty(user_name))
                {
                    result = result.Where(t => t.user_name.Contains(user_name.Trim()));
                }

                if (!string.IsNullOrEmpty(nick_name))
                {
                    result = result.Where(t => t.nick_name.Contains(nick_name.Trim()));
                }

                if (!string.IsNullOrEmpty(mobile_phone))
                {
                    result = result.Where(t => t.mobile_phone.Contains(mobile_phone.Trim()));
                }

                if (!string.IsNullOrEmpty(user_job))
                {
                    result = result.Where(t => t.user_job.Contains(user_job.Trim()));
                }

                if (role_id > 0)
                {
                    result = result.Where(t => t.role_id == role_id);
                }

                if (user_status > -1)
                {
                    if (user_status == 1)
                    {
                        result = result.Where(t => t.user_status == true);
                    }
                    if (user_status == 0)
                    {
                        result = result.Where(t => t.user_status == false);
                    }
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
                AddLogs((int)ENUMHelper.LogType.Search, (int)ENUMHelper.Platform.Admin + "" + (int)ENUMHelper.OperateType.List + "" + (int)ENUMHelper.InfoType.Info, "查询LINQ=" + result.Expression.ToString() + ",DATA=" + list.ToJson());
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
                log.Fatal(ex, " -> GetDataByPage");
                return Json(new { code = 0, errcode = (int)ENUMHelper.ExceptionType.List, errmsg = ex.Message });
            }
        }

        /// <summary>
        /// 获取最近5条活跃管理员列表
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Active()
        {
            try
            {
                var result = from item in _context.mr_users
                             select new
                             {
                                 item.user_id,
                                 item.user_name,
                                 item.nick_name,
                                 item.mobile_phone,
                                 item.user_status,
                                 item.last_login_at,
                                 item.created_at,
                                 item.role_id,
                                 role_name = _context.mr_user_roles.FirstOrDefault(t => t.role_id == item.role_id)!.role_name,
                                 last_date = Convert.ToDateTime(item.last_login_at).ToString("MM/dd"),
                             };

                var list = await result.OrderByDescending(t => t.last_login_at).Take(5).ToListAsync();
                var lCount = result.LongCount();
                AddLogs((int)ENUMHelper.LogType.Search, (int)ENUMHelper.Platform.Admin + "" + (int)ENUMHelper.OperateType.List + "" + (int)ENUMHelper.InfoType.Info, "查询LINQ=" + result.Expression.ToString() + ",DATA=" + list.ToJson());
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
                log.Fatal(ex, " -> Active");
                return Json(new { code = 0, errcode = (int)ENUMHelper.ExceptionType.List, errmsg = ex.Message });
            }
        }

        /// <summary>
        /// 获取用户列表(用于select下拉填充数据)
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Select()
        {
            try
            {
                var list = await _context.mr_users.OrderByDescending(t => t.user_id).ToListAsync();
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
        /// 根据ID获取用户信息
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        public async Task<IActionResult> Details(long? id)
        {
            try
            {
                log.Information("id=" + id);
                var item = await _context.mr_users.FirstOrDefaultAsync(t => t.user_id == id);
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
                log.Fatal(ex, " -> Details");
                return Json(new { code = 0, errcode = (int)ENUMHelper.ExceptionType.Details, errmsg = ex.Message });
            }
        }

        /// <summary>
        /// 根据TOKEN获取用户信息
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> DetailsByToken()
        {
            try
            {
                var user_id = User.FindFirst(ClaimTypes.Sid)!.Value;
                long.TryParse(user_id, out long get_user_id);
                log.Information("user_id=" + user_id);
                var users = await _context.mr_users.FirstOrDefaultAsync(t => t.user_id == get_user_id);
                if (users == null)
                {
                    return Json(new { code = 0, msg = "获取数据失败" });
                }

                AddLogs((int)ENUMHelper.LogType.Search, (int)ENUMHelper.Platform.Admin + "" + (int)ENUMHelper.OperateType.Details + "" + (int)ENUMHelper.InfoType.Info, "查询资料ID=" + users.user_id + ",users=" + users.ToJson());
                return Json(new { code = 0, msg = "获取数据成功", data = users });
            }
            catch (Exception ex)
            {
                log.Fatal(ex, " -> Details");
                return Json(new { code = 0, errcode = (int)ENUMHelper.ExceptionType.Details, errmsg = ex.Message });
            }
        }

        /// <summary>
        /// 检测TOKEN
        /// </summary>
        /// <param name="token">TOKEN</param>
        /// <returns></returns>
        public async Task<IActionResult> CheckToken(string token)
        {
            try
            {
                log.Information("token=" + token);
                var users = await _context.mr_users.FirstOrDefaultAsync(t => t.user_token!.Contains(token));
                if (users == null)
                {
                    AddLogs((int)ENUMHelper.LogType.Search, (int)ENUMHelper.Platform.Admin + "" + (int)ENUMHelper.OperateType.Check + "" + (int)ENUMHelper.InfoType.Info, "下线=检测用户TOKEN是否过期,TOKEN=" + token);
                    return Json(new { code = 0, msg = "token为空" });
                }
                else
                {
                    AddLogs((int)ENUMHelper.LogType.Search, (int)ENUMHelper.Platform.Admin + "" + (int)ENUMHelper.OperateType.Check + "" + (int)ENUMHelper.InfoType.Info, "在线=检测用户TOKEN是否过期,TOKEN=" + token);
                    return Json(new { code = 0, msg = "获取数据成功", data = users });
                }
            }
            catch (Exception ex)
            {
                log.Fatal(ex, " -> Details");
                return Json(new { code = 0, errcode = (int)ENUMHelper.ExceptionType.Check, errmsg = ex.Message });
            }
        }

        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="users">Users对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Users users)
        {
            try
            {
                log.Information("users=" + users.ToJson());
                string password = string.IsNullOrEmpty(users.user_password) ? "" : users.user_password;
                password = MR.Utility.Helper.MD5Helper.MD5Encrypt32(password);
                users.user_password = password;
                users.created_at = DateTime.Now;
                users.last_login_at = DateTime.Now;
                if (!_context.mr_users.Any(t => t.user_name!.Equals(users.user_name!.Trim())))
                {
                    _context.mr_users.Add(users);
                    await _context.SaveChangesAsync();
                    AddLogs((int)ENUMHelper.LogType.Create, (int)ENUMHelper.Platform.Admin + "" + (int)ENUMHelper.OperateType.Create + "" + (int)ENUMHelper.InfoType.Info, "新增,users=" + users.ToJson());

                    return Json(new { code = 0, msg = "添加成功", status = 0, data = users });
                }
                else
                {
                    return Json(new { code = 0, msg = "用户名已存在，请更换！", status = 1 });
                }
            }
            catch (Exception ex)
            {
                log.Fatal(ex, " -> Create");
                return Json(new { code = 0, errcode = (int)ENUMHelper.ExceptionType.Create, errmsg = ex.Message });
            }
        }

        /// <summary>
        /// 编辑用户资料
        /// </summary>
        /// <param name="id"></param>
        /// <param name="users"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Users users)
        {
            try
            {
                log.Information("users=" + users.ToJson());
                var user = _context.mr_users.FirstOrDefault(c => c.user_id == id);
                if (user != null)
                {
                    user.role_id = users.role_id;
                    user.nick_name = users.nick_name;
                    user.mobile_phone = users.mobile_phone;
                    user.user_job = users.user_job;
                    user.user_email = users.user_email;
                    user.limit_ip = users.limit_ip;
                    user.user_status = users.user_status;
                    _context.mr_users.Update(user);
                    await _context.SaveChangesAsync();
                    AddLogs((int)ENUMHelper.LogType.Update, (int)ENUMHelper.Platform.Admin + "" + (int)ENUMHelper.OperateType.Edit + "" + (int)ENUMHelper.InfoType.Info, "更新资料ID=" + id + ",users=" + users.ToJson());

                    return Json(new { code = 0, msg = "更新成功" });
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
        /// 编辑当前登录用户资料
        /// </summary>
        /// <param name="users">Users对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditByToken(Users users)
        {
            try
            {
                log.Information("users=" + users.ToJson());
                var user_id = User.FindFirst(ClaimTypes.Sid)!.Value;
                long.TryParse(user_id, out long get_user_id);
                
                var user = _context.mr_users.FirstOrDefault(t => t.user_id == get_user_id);
                if (user != null)
                {
                    user.nick_name = users.nick_name;
                    user.mobile_phone = users.mobile_phone;
                    user.user_job = users.user_job;
                    user.user_email = users.user_email;
                    user.user_slogan = users.user_slogan;
                    user.user_intro = users.user_intro;
                    _context.mr_users.Update(user);
                    await _context.SaveChangesAsync();

                    AddLogs((int)ENUMHelper.LogType.Update, (int)ENUMHelper.Platform.Admin + "" + (int)ENUMHelper.OperateType.Edit + "" + (int)ENUMHelper.InfoType.Info, "更新资料TOKEN=" + users.user_token + ",users=" + users.ToJson());
                    return Json(new { code = 0, msg = "更新成功" });
                }
                else
                {
                    return Json(new { code = 0, msg = "操作失败，ID=" + get_user_id + "不存在！" });
                }
            }
            catch (Exception ex)
            {
                log.Fatal(ex, " -> EditByToken");
                return Json(new { code = 0, errcode = (int)ENUMHelper.ExceptionType.Edit, errmsg = ex.Message });
            }
        }

        /// <summary>
        /// 更新密码-当前登录用户
        /// </summary>
        /// <param name="old_password">旧密码</param>
        /// <param name="new_password">新密码</param>
        /// <param name="token">TOKEN</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPassword(string old_password, string new_password, string token)
        {
            var message = "";
            try
            {
                var user_id = User.FindFirst(ClaimTypes.Sid)!.Value;
                long.TryParse(user_id, out long get_user_id);

                AddLogs((int)ENUMHelper.LogType.Update, (int)ENUMHelper.Platform.Admin + "" + (int)ENUMHelper.OperateType.Edit + "" + (int)ENUMHelper.InfoType.Info, "修改密码,old_password=" + old_password + ",new_password=" + new_password + ",TOKEN=" + token);
                new_password = MR.Utility.Helper.MD5Helper.MD5Encrypt32(new_password);

                // 校验原始秘密
                old_password = MR.Utility.Helper.MD5Helper.MD5Encrypt32(old_password);
                var users = await _context.mr_users.FirstOrDefaultAsync(t => t.user_id == get_user_id && t.user_password!.Contains(old_password));
                if (users == null)
                {
                    message = "原始密码不正确";
                }
                else
                {
                    users.user_password = new_password;//更新密码
                    _context.mr_users.Update(users);
                    await _context.SaveChangesAsync();

                    message = "密码修改成功，请重新登录！";
                }

                return Json(new { code = 0, msg = message });
            }
            catch (Exception ex)
            {
                log.Fatal(ex, " -> EditPassword");
                return Json(new { code = 0, errcode = (int)ENUMHelper.ExceptionType.Edit, errmsg = ex.Message });
            }
        }

        /// <summary>
        /// 重置密码-管理员列表重置用户密码
        /// </summary>
        /// <param name="new_password">新密码</param>
        /// <param name="id">用户ID</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(string new_password, long id)
        {
            var message = "";
            try
            {
                log.Information("id=" + id + ",new_password=" + new_password);
                AddLogs((int)ENUMHelper.LogType.Update, (int)ENUMHelper.Platform.Admin + "" + (int)ENUMHelper.OperateType.Set + "" + (int)ENUMHelper.InfoType.Info, "重置密码,new_password=" + new_password + ",ID=" + id);
                new_password = MR.Utility.Helper.MD5Helper.MD5Encrypt32(new_password);

                var users = await _context.mr_users.FirstOrDefaultAsync(t => t.user_id == id);
                if (users == null)
                {
                    message = "用户不存在";
                }
                else
                {
                    users.user_password = new_password;//更新密码
                    _context.mr_users.Update(users);
                    await _context.SaveChangesAsync();

                    message = "密码修改成功！";
                }
                return Json(new { code = 0, msg = message });
            }
            catch (Exception ex)
            {
                log.Fatal(ex, " -> ResetPassword");
                return Json(new { code = 0, errcode = (int)ENUMHelper.ExceptionType.Update, errmsg = ex.Message });
            }
        }

        /// <summary>
        /// 更新用户状态
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <param name="status">用户状态:启用=1|禁用=0</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetStatus(long id, bool status)
        {
            try
            {
                log.Information("id=" + id + ",status=" + status);
                var item = await _context.mr_users.FirstOrDefaultAsync(t => t.user_id == id);
                if (item != null)
                {
                    item.user_status = status;
                    _context.mr_users.Update(item);
                    await _context.SaveChangesAsync();
                    AddLogs((int)ENUMHelper.LogType.Update, (int)ENUMHelper.Platform.Admin + "" + (int)ENUMHelper.OperateType.Set + "" + (int)ENUMHelper.InfoType.Info, "更新用户状态:id=" + id + ",status=" + status);

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
                return Json(new { code = 0, errcode = (int)ENUMHelper.ExceptionType.Update, errmsg = ex.Message });
            }
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Logout()
        {
            // 清理Redis缓存
            HttpContext.Response.Cookies.Delete("CLOUDIN_MR_VERIFY_CODE_" + User.FindFirst(ClaimTypes.Email)!.Value);
            HttpContext.Response.Cookies.Delete("CLOUDIN_MR_ROLE_ID_" + User.FindFirst(ClaimTypes.Email)!.Value);

            // 退出Claims
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // 清理用户表中的TOKEN
            try
            {
                var user_id = User.FindFirst(ClaimTypes.Sid)!.Value;
                long.TryParse(user_id, out long get_user_id);
                var users = _context.mr_users.FirstOrDefault(t => t.user_id == get_user_id);
                if (users != null)
                {
                    users.user_token = "";
                    _context.Update(users);
                    _context.SaveChanges();
                    AddLogs((int)ENUMHelper.LogType.UserLogout, (int)ENUMHelper.Platform.Admin + "" + (int)ENUMHelper.OperateType.Logout + "" + (int)ENUMHelper.InfoType.Info, "用户退出后台,清理user_id=" + user_id);

                    return Json(new { code = 0, msg = "success" });
                }
                else
                {
                    return Json(new { code = 0, msg = "操作失败，ID=" + get_user_id + "不存在！" });
                }
            }
            catch (Exception ex)
            {
                log.Fatal(ex, " -> 清理用户表中的TOKEN");
                return Json(new { code = 0, errcode = (int)ENUMHelper.ExceptionType.Delete, errmsg = ex.Message });
            }
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                var item = _context.mr_users.Find(id);
                if (item == null)
                {
                    return Json(new { code = 0, msg = "ID为空，删除失败！" });
                }
                 _context.mr_users.Remove(item); // 演示系统暂时关闭删除功能，正式发布需要开启
                await _context.SaveChangesAsync();
                AddLogs((int)ENUMHelper.LogType.Delete, (int)ENUMHelper.Platform.Admin + "" + (int)ENUMHelper.OperateType.Logout + "" + (int)ENUMHelper.InfoType.Info, "删除ID=" + id + ",item=" + item.ToJson());

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