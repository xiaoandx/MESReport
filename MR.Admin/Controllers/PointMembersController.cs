using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MR.Manage.Data;
using MR.Models;
using MR.Utility.Helper;
using Senparc.CO2NET.Extensions;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace MR.Manage.Controllers
{

    /// <summary>。
    /// 内容摘要: 会员积分类
    /// </summary>
    [Authorize]
    public class PointMembersController : BaseController<PointMembersController>
    {

        /// <summary>
        /// 会员积分 实例化数据上下文
        /// </summary>
        /// <param name="context"></param>
        public PointMembersController(MRManageContext context) : base(context)
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
        /// 获取会员积分列表数据
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="limit">每页显示数量</param>
        /// <param name="created_at">创建日期</param>
        /// <param name="member_name">会员昵称</param>
        /// <param name="mobile_phone">手机号</param>
        /// <param name="total_point">会员总积分</param>
        /// <returns>返回会员积分JSON数据集合</returns>
        public async Task<IActionResult> List(int page, int limit, string created_at, string member_name, string mobile_phone, int total_point = -1)
        {
            try
            {
                var result = from item in _context.Set<PointMembers>()
                             select new
                             {
                                 item.point_member_id,
                                 item.member_id,
                                 member_name = _context.mr_members.FirstOrDefault(t => t.member_id == item.member_id)!.member_name,
                                 mobile_phone = _context.mr_members.FirstOrDefault(t => t.member_id == item.member_id)!.mobile_phone,
                                 item.total_point,
                                 item.available_point,
                                 item.froze_point,
                                 item.created_at,
                                 format_time = item.created_at.ToString("yyyy-MM-dd HH:mm")
                             };

                if (!string.IsNullOrEmpty(member_name))
                {
                    result = result.Where(t => t.member_name.Contains(member_name.Trim()));
                }
                if (!string.IsNullOrEmpty(mobile_phone))
                {
                    result = result.Where(t => t.mobile_phone.Contains(mobile_phone.Trim()));
                }
                if (total_point > 0)
                {
                    result = result.Where(t => t.total_point == total_point);
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
                AddLogs((int)ENUMHelper.LogType.Search, (int)ENUMHelper.Platform.Admin + "" + (int)ENUMHelper.OperateType.List + "" + (int)ENUMHelper.InfoType.Info, "查询会员积分LINQ=" + result.Expression.ToString() + ",DATA=" + list.ToJson());
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
    }
}
