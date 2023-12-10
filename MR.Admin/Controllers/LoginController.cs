using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MR.Manage.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Microsoft.AspNetCore.Http.Extensions;
using Senparc.CO2NET.Extensions;
using System.Security.Claims;
using IdentityModel;
using System.IdentityModel.Tokens.Jwt;
using MR.Utility.Authorization;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using MR.Utility.Helper;
using MR.Models;
using Microsoft.EntityFrameworkCore;
using Lazy.Captcha.Core;
using Serilog.Context;
using Microsoft.AspNetCore.Authorization;

namespace MR.Manage.Controllers
{
    /// <summary>
    /// 内容摘要: 登陆页面
    /// </summary>
    public class LoginController : Controller
    {
        /// <summary>
        /// _cache
        /// </summary>
        private readonly IDistributedCache? _cache = null;
        /// <summary>
        /// _captcha
        /// </summary>
        private readonly ICaptcha _captcha;
        /// <summary>
        /// cookie
        /// </summary>
        CookieHelper cookie = new CookieHelper();

        /// <summary>
        /// 初始化数据库上下文
        /// </summary>
        private readonly MRManageContext _context;

        /// <summary>
        /// 构造及初始化类参数
        /// </summary>
        public LoginController(MRManageContext context, ICaptcha captcha)
        {
            _context = context;
            _captcha = captcha;
        }

        /// <summary>
        /// 登录页面
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 退出页面
        /// </summary>
        /// <returns></returns>
        public IActionResult Logout()
        {
            return View();
        }

        /// <summary>
        /// 获取TOKEN
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Token()
        {
            var random_words = "CLOUDIN_MR_" + CommonHelper.GenerateRandomWordsNumber(8) + DateTime.Now.ToString("yyyyMMddHHmmss");
            var token = MD5Helper.MD5Encrypt32(random_words);
            return Json(new { code = 0, msg = "success", data = token });
        }

        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <param name="t">TOKEN</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult VerifyCode(string t)
        {
            var info = _captcha.Generate(t);
            // 有多处验证码且过期时间不一样，可传第二个参数覆盖默认配置。
            // var info = _captcha.Generate(id,120);
            var stream = new MemoryStream(info.Bytes);
            return File(stream, "image/gif");
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="code">验证码</param>
        /// <param name="users">用户对象</param>
        /// <param name="token">TOKEN</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginParas paras)
        {
            var menus = new List<dynamic>();
            var type = 1;
            var user_token = "";
            try
            {
                var user_name = paras.user_name;
                var user_password = paras.user_password;
                var captcha = paras.captcha;
                var token = paras.token;
                // log.Information("captcha=" + captcha + ",token=" + token);

                if (!_captcha.Validate(token, captcha))
                {
                    return Json(new { code = 0, msg = "验证码错误！" });
                }
                else
                {
                    // 加密后比对
                    user_password = MR.Utility.Helper.MD5Helper.MD5Encrypt32(user_password);

                    var users = await _context.mr_users.FirstOrDefaultAsync(t => t.user_name!.Equals(user_name!.Trim()) && t.user_password!.Contains(user_password));
                    if (users == null)
                    {
                        var log_content = "账号或密码错误！";
                        
                        var item_users = await _context.mr_users.FirstOrDefaultAsync(t => t.user_name!.Equals(user_name!.Trim()));
                        if (item_users == null)
                        {
                            AddLogs((int)ENUMHelper.Platform.Admin + "" + (int)ENUMHelper.OperateType.Login + "" + (int)ENUMHelper.ErrorType.PasswordError, log_content, -1);

                            return Json(new { code = 0, msg = log_content });
                        }
                        else
                        {
                            var password_error_num = _context.mr_logs.Where(t => t.user_id == item_users.user_id && t.log_code!.Equals("6104001") && DateTime.Now.Date == t.created_at.Date && DateTime.Now.Hour - t.created_at.Hour <1).Count();
                            var num = 5 - password_error_num;
                            if (num == 1)
                            {
                                log_content = "账号或密码错误，还剩最后一次机会！";
                            }
                            else if (num <= 0)
                            {
                                log_content = "你的账号已被封禁1小时，请稍后再试！";
                            }
                            else
                            {
                                log_content = "账号或密码错误，还剩" + num + "次机会！";
                            }

                            AddLogs((int)ENUMHelper.Platform.Admin + "" + (int)ENUMHelper.OperateType.Login + "" + (int)ENUMHelper.ErrorType.PasswordError, log_content, item_users.user_id);

                            return Json(new { code = 0, msg = log_content });
                        }
                    }
                    else if (!users.user_status)
                    {
                        return Json(new { code = 0, msg = "账号已被禁用，请联系管理员！" });
                    }
                    else
                    {
                        var password_error_num = _context.mr_logs.Where(t => t.user_id == users.user_id && t.log_code!.Equals("6104001") && DateTime.Now.Date == t.created_at.Date && DateTime.Now.Hour - t.created_at.Hour < 1).Count();
                        var num = 5 - password_error_num;
                        if (num <= 0)
                        {
                            return Json(new { code = 0, msg = "你的账号已被封禁1小时，请稍后再试！" });
                        }

                        HttpContext.Response.Cookies.Append("CLOUDIN_MR_ROLE_ID_" + token, users.role_id.ToString(), new CookieOptions
                        {
                            Expires = DateTime.Now.AddDays(1)
                        });
                        type = users.role_id;

                        // 根据权限控制台菜单
                        var role = _context.mr_user_roles.FirstOrDefault(c => c.role_id == users.role_id);
                        var menucode_list = role!.role_authority!.Split(",").Select(c => { int.TryParse(c, out int b); return b; }).ToList();
                        var menu_list = _context.mr_menus.Where(c => menucode_list.Contains(c.menu_code) && c.menu_status == true).ToList();
                        var parent = menu_list.Where(c => c.parent_id == -1).ToList();
                        var children = menu_list.Where(c => c.parent_id != -1).OrderBy(c => c.parent_id).ToList();

                        var children_menus = new List<Menus>();
                        if (children.Count == 1)
                        {
                            for (int i = 0; i < children.Count; i++)
                            {
                                children_menus.Add(children[i]);
                                menus.Add(new { menu = _context.mr_menus.FirstOrDefault(c => c.menu_id == children_menus[0].parent_id), children = children_menus.OrderBy(c => c.menu_rank).ToList() });
                                children_menus.Clear();
                            }
                        }
                        else
                        {
                            for (int i = 0; i < children.Count; i++)
                            {
                                if (i != 0 && children[i].parent_id != children[i - 1].parent_id || i == children.Count - 1)
                                {
                                    if (i == children.Count - 1 && children[i].parent_id == children[i - 1].parent_id)
                                        children_menus.Add(children[i]);

                                    menus.Add(new { menu = _context.mr_menus.FirstOrDefault(c => c.menu_id == children_menus[0].parent_id), children = children_menus.OrderBy(c => c.menu_rank).ToList() });
                                    if (i == children.Count - 1 && children[i].parent_id != children[i - 1].parent_id)
                                    {
                                        children_menus.Clear();
                                        children_menus.Add(children[i]);
                                        menus.Add(new { menu = _context.mr_menus.FirstOrDefault(c => c.menu_id == children[i].parent_id), children = children_menus.OrderBy(c => c.menu_rank).ToList() });
                                    }
                                    children_menus.Clear();

                                    if (i != children.Count - 1)
                                        children_menus.Add(children[i]);
                                }
                                else
                                    children_menus.Add(children[i]);
                            }
                        }

                        // 新增日志
                        AddLogs((int)ENUMHelper.Platform.Admin + "" + (int)ENUMHelper.OperateType.Login + "" + (int)ENUMHelper.InfoType.LoginSuccess, "用户登录后台管理系统", users.user_id);

                        // Claim授权验证
                        var subjectId = users.user_id.ToString();
                        var claims = new List<Claim> {
                                new Claim(ClaimTypes.NameIdentifier, users.role_id.ToString()),
                                new Claim(ClaimTypes.Sid, users.user_id.ToString()),
                                new Claim(ClaimTypes.Name, string.IsNullOrEmpty(users.nick_name) ? "-" : users.nick_name),
                                new Claim(ClaimTypes.Email, string.IsNullOrEmpty(token) ? "-" : token),
                                new Claim(JwtClaimTypes.Name, string.IsNullOrEmpty(user_name) ? "-" : user_name),
                                new Claim(ClaimTypes.Expiration, DateTime.UtcNow.AddSeconds(TokenAuthConfiguration.Expiration.TotalSeconds).ToString()),
                                new Claim(JwtRegisteredClaimNames.AuthTime, DateTime.UtcNow.ToString("o")),
                            };

                        // 获取用户唯一TOKEN
                        user_token = JwtToken.IssueToken(claims);
                        if (_cache != null)
                        {
                            _cache.SetString(subjectId, user_token, new DistributedCacheEntryOptions { SlidingExpiration = TimeSpan.FromDays(30) });
                        }
                        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

                        // 更新TOKEN
                        users.last_login_at = DateTime.Now;
                        users.user_token = user_token;
                        _context.mr_users.Update(users);
                        await _context.SaveChangesAsync();

                        return Json(new { code = 0, msg = "登录成功", menus, data = type, token = user_token });
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { code = 0, errcode = (int)ENUMHelper.ExceptionType.Check, errmsg = ex.Message });
            }
        }

        /// <summary>
        /// 新增日志
        /// </summary>
        /// <param name="log_code">日志标识</param>
        /// <param name="log_content">日志内容</param>
        /// <param name="user_id">后台管理员ID</param>
        public void AddLogs(string log_code, string log_content, long user_id)
        {
            string page_url = HttpContext.Request.GetDisplayUrl();
            string referrer_url = HttpContext.Request.UrlReferrer();
            string user_agent = HttpContext.Request.UserAgent();
            string user_cookies = HttpContext.Request.Cookies.ToJson();

            _context.mr_logs.Add(new Logs()
            {
                type_id = 1,
                user_id = user_id,
                member_id = -1,
                log_code = log_code,
                platform_id = 1,
                log_content = log_content,
                client_ip = HttpContext.Connection.RemoteIpAddress!.ToString(),
                page_url = page_url,
                referrer_url = referrer_url,
                user_agent = user_agent,
                user_cookies = user_cookies,
                log_remark = "",
                created_at = DateTime.Now
            });
            _context.SaveChanges();
        }
    }
}
