using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MR.Manage.Data;
using MR.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Senparc.CO2NET.Extensions;
using Serilog.Core;
using Serilog;
using System.IO;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;

namespace MR.Manage.Controllers
{
    /// <summary>
    /// 内容摘要: Base基类
    /// </summary>
    [Authorize]
    public class BaseController<T> : Controller
    {
        /// <summary>
        /// _context
        /// </summary>
        protected readonly MRManageContext _context;

        /// <summary>
        /// Log变量
        /// </summary>
        public Logger log;

        public BaseController(MRManageContext context)
        {
            _context = context;

            // 初始化日志
            log = new LoggerConfiguration()
             .ReadFrom.Configuration(new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
             .Build())
             .CreateLogger();
        }

        /// <summary>
        /// 添加操作日志
        /// </summary>
        /// <param name="type">类型：4=新增，5=删除，6=修改，7=查询</param>
        /// <param name="log_code">日志标识</param>
        /// <param name="log_content">日志内容</param>
        public void AddLogs(int type, string log_code, string log_content)
        {
            try
            {
                var user_id = User.FindFirst(ClaimTypes.Sid)!.Value;
                long.TryParse(user_id, out long get_user_id);
                if (string.IsNullOrEmpty(user_id))
                {
                    user_id = "-1";
                }

                string page_url = HttpContext.Request.GetDisplayUrl();
                string referrer_url = HttpContext.Request.UrlReferrer();
                string user_agent = HttpContext.Request.UserAgent();
                string user_cookies = HttpContext.Request.Cookies.ToJson();

                _context.mr_logs.Add(new Logs()
                {
                    type_id = 1,
                    user_id = get_user_id,
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
            catch (Exception ex)
            {
                log.Fatal(ex, " -> 添加操作日志");
            }
        }
    }
}
