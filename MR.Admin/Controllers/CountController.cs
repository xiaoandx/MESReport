using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using MR.Manage.Data;
using MR.Models;
using Microsoft.AspNetCore.Http;
using Senparc.CO2NET.Extensions;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using MR.Utility.Helper;

namespace MR.Manage.Controllers
{
    /// <summary>
    /// 内容摘要: 控制台统计数据
    /// </summary>
    [Authorize]
    public class CountController : BaseController<CountController>
    {

        /// <summary>
        /// 访问来源 实例化数据上下文
        /// </summary>
        /// <param name="context"></param>
        public CountController(MRManageContext context) : base(context)
        {

        }

        /// <summary>
        /// 数据统计-用于控制台页面
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Console()
        {
            //获取TOKEN
            int total_data_month1 = 0;
            int total_data_day1 = 0;
            int total_data_month2 = 0;
            int total_data_day2 = 0;
            int total_data_month3 = 0;
            int total_data_day3 = 0;
            int total_data_month4 = 0;
            int total_data_day4 = 0;
            int total_data_month5 = 0;
            int total_data_day5 = 0;
            int total_data_month6 = 0;
            int total_data_day6 = 0;
            int total_data_month7 = 0;
            int total_data_day7 = 0;
            int total_data_month8 = 0;
            int total_data_day8 = 0;

            try
            {
                // 会员
                total_data_month1 = await _context.mr_members.CountAsync();
                total_data_day1 = await _context.mr_members.Where(t => t.created_at.Date == DateTime.Now.Date).CountAsync();
                // 文章
                total_data_month2 = await _context.mr_articles.CountAsync();
                total_data_day2 = await _context.mr_articles.Where(t => t.created_at.Date == DateTime.Now.Date).CountAsync();
                // 消息
                total_data_month3 = await _context.mr_messages.CountAsync();
                total_data_day3 = await _context.mr_messages.Where(t => t.created_at.Date == DateTime.Now.Date).CountAsync();
                // 短信
                total_data_month4 = await _context.mr_sms.CountAsync();
                total_data_day4 = await _context.mr_sms.Where(t => t.created_at.Date == DateTime.Now.Date).CountAsync();
                // 积分
                total_data_month5 = await _context.mr_points.CountAsync();
                total_data_day5 = await _context.mr_points.Where(t => t.created_at.Date == DateTime.Now.Date).CountAsync();
                // 验证码
                total_data_month6 = await _context.mr_codes.CountAsync();
                total_data_day6 = await _context.mr_codes.Where(t => t.created_at.Date == DateTime.Now.Date).CountAsync();
                // 意见反馈
                total_data_month7 = await _context.mr_feedback.CountAsync();
                total_data_day7 = await _context.mr_feedback.Where(t => t.created_at.Date == DateTime.Now.Date).CountAsync();
                // 日志
                total_data_month8 = await _context.mr_logs.CountAsync();
                total_data_day8 = await _context.mr_logs.Where(t => t.created_at.Date == DateTime.Now.Date).CountAsync();

                var data = new
                {
                    total_data_month1 = total_data_month1,
                    total_data_day1 = total_data_day1,
                    total_data_month2 = total_data_month2,
                    total_data_day2 = total_data_day2,
                    total_data_month3 = total_data_month3,
                    total_data_day3 = total_data_day3,
                    total_data_month4 = total_data_month4,
                    total_data_day4 = total_data_day4,
                    total_data_month5 = total_data_month5,
                    total_data_day5 = total_data_day5,
                    total_data_month6 = total_data_month6,
                    total_data_day6 = total_data_day6,
                    total_data_month7 = total_data_month7,
                    total_data_day7 = total_data_day7,
                    total_data_month8 = total_data_month8,
                    total_data_day8 = total_data_day8,
                    role_type = User.FindFirst(ClaimTypes.NameIdentifier)!.Value
                };

                return Json(new { code = 0, msg = "success", data = data });
            }
            catch (Exception ex)
            {
                log.Fatal(ex, " -> Count");
                return Json(new { code = 0, errcode = (int)ENUMHelper.ExceptionType.Chart, errmsg = ex.Message });
            }
        }

        /// <summary>
        /// 统计消息情况
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Message()
        {
            try
            {
                var data = new
                {
                    total_num = await _context.mr_messages.CountAsync(),
                    read_num = await _context.mr_messages.Where(t => t.is_read).CountAsync(),
                    unread_num = await _context.mr_messages.Where(t => !t.is_read).CountAsync(),
                    all_unready_num = await _context.mr_messages.Where(t => !t.is_read).CountAsync(),
                    system_unready_num = await _context.mr_messages.Where(t => t.type_id == 1 && !t.is_read).CountAsync(),
                    member_unready_num = await _context.mr_messages.Where(t => t.type_id == 2 && !t.is_read).CountAsync(),
                    push_unready_num = await _context.mr_messages.Where(t => t.type_id == 3 && !t.is_read).CountAsync()
                };

                return Json(new { code = 0, msg = "success", data = data });
            }
            catch (Exception ex)
            {
                log.Fatal(ex, " -> CountUnMessage");
                return Json(new { code = 0, errcode = (int)ENUMHelper.ExceptionType.Chart, errmsg = ex.Message });
            }
        }
    }
}
