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

    /// <summary>
    /// 内容摘要: 会员积分记录明细类
    /// </summary>
    [Authorize]
    public class PointRecordsController : BaseController<PointRecordsController>
    {

        /// <summary>
        /// 积分记录 实例化数据上下文
        /// </summary>
        /// <param name="context"></param>
        public PointRecordsController(MRManageContext context) : base(context)
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
        /// 获取积分记录列表数据
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="limit">每页显示数量</param>
        /// <param name="created_at">创建日期</param>
        /// <param name="point_action">积分说明</param>
        /// <param name="point_id">积分类型</param>
        /// <param name="point_num">积分数量</param>
        /// <param name="member_id">会员ID</param>
        /// <returns>返回积分记录JSON数据集合</returns>
        public async Task<IActionResult> List(int page, int limit, string created_at, string point_action, int point_id = -1, int point_num = -1, long member_id = -1)
        {
            try
            {
                var result = from item in _context.Set<PointRecords>().Where(t => t.member_id == member_id)
                             select new
                             {
                                 item.record_id,
                                 item.member_id,
                                 member_name = item.point_id > -1 ? _context.mr_members.FirstOrDefault(t => t.member_id == item.member_id)!.member_name : "-",
                                 item.point_id,
                                 point_name = item.point_id > -1 ? _context.mr_points.FirstOrDefault(t => t.point_id == item.point_id)!.point_name : "-",
                                 item.point_num,
                                 item.point_action,
                                 item.point_token,
                                 item.point_remark,
                                 item.created_at,
                                 format_time = item.created_at.ToString("yyyy-MM-dd HH:mm")
                             };

                if (point_id > 0)
                {
                    result = result.Where(t => t.point_id == point_id);
                }
                if (point_num > 0)
                {
                    result = result.Where(t => t.point_num == point_num);
                }
                if (!string.IsNullOrEmpty(point_action))
                {
                    result = result.Where(t => t.point_action.Contains(point_action.Trim()));
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
                AddLogs((int)ENUMHelper.LogType.Search, (int)ENUMHelper.Platform.Admin + "" + (int)ENUMHelper.OperateType.List + "" + (int)ENUMHelper.InfoType.Info, "查询积分记录LINQ=" + result.Expression.ToString() + ",DATA=" + list.ToJson());
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
