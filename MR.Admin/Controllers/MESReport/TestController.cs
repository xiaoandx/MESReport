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

namespace MR.Manage.Controllers.MESReport
{

    /// <summary>
    /// 内容摘要: 广告类
    /// </summary>
    [Authorize]
    public class TestController : BaseController<TestController>
    {

        /// <summary>
        /// 广告 实例化数据上下文
        /// </summary>
        /// <param name="context"></param>
        public TestController(MRManageContext context) : base(context)
        {

        }

        /// <summary>
        /// 管理/列表页面
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View("~/Views/MESReport/Test/Index.cshtml");
        }

        /// <summary>
        /// 管理/列表页面
        /// </summary>
        /// <returns></returns>
        public IActionResult Show()
        {
            return View("~/Views/MESReport/Test/Show.cshtml");
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
        /// 获取广告列表数据
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="limit">每页显示数量</param>
        /// <param name="created_at">创建日期</param>
        /// <param name="ads_name">广告名称</param>
        /// <returns>返回广告JSON数据集合</returns>
        public async Task<IActionResult> List(int page, int limit, string created_at, string ads_name, int link_id, int is_show = -1)
        {
            try
            {
                var result = from item in _context.Set<Ads>()
                             select new
                             {
                                 item.ads_id,
                                 item.lang_id,
                                 item.position_id,
                                 item.link_id,
                                 link_name = item.link_id > -1 ? _context.mr_ads_links.FirstOrDefault(t => t.link_id == item.link_id)!.link_name : "-",
                                 item.ads_name,
                                 item.ads_image,
                                 show_ads_image = !string.IsNullOrEmpty(item.ads_image) ? MR.Manage.Filters.AppSettingsFilter.GetSetting("Aliyun:OSS_URL") + item.ads_image : "/images/no_photo.png",
                                 item.oss_url,
                                 item.user_id,
                                 item.page_url,
                                 item.sort_num,
                                 item.is_show,
                                 item.ads_remark,
                                 item.created_at,
                                 format_time = item.created_at.ToString("yyyy-MM-dd HH:mm")
                             };

                if (!string.IsNullOrEmpty(ads_name))
                {
                    result = result.Where(t => t.ads_name.Contains(ads_name.Trim()));
                }

                if (is_show > -1)
                {
                    if (is_show == 1)
                    {
                        result = result.Where(t => t.is_show == true);
                    }
                    if (is_show == 0)
                    {
                        result = result.Where(t => t.is_show == false);
                    }
                }

                if (link_id > 0)
                {
                    result = result.Where(t => t.link_id == link_id);
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
                AddLogs((int)ENUMHelper.LogType.Search, (int)ENUMHelper.Platform.Admin + "" + (int)ENUMHelper.OperateType.List + "" + (int)ENUMHelper.InfoType.Info, "查询广告LINQ=" + result.Expression.ToString() + ",DATA=" + list.ToJson());
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
        /// 获取广告数据
        /// </summary>
        /// <returns>返回广告JSON数据集合</returns>
        public async Task<IActionResult> Select()
        {
            try
            {
                var list = await _context.mr_ads.OrderBy(t => t.ads_id).ToListAsync();
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
        /// 根据ID获取广告详情
        /// </summary>
        /// <param name="id">广告ID</param>
        /// <returns>返回单条广告JSON数据</returns>
        [HttpGet("/ads/details/")]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                log.Information("id=" + id);
                var result = await _context.mr_ads.Where(t => t.ads_id == id).Select(item => (new
                {
                    item.ads_id,
                    item.lang_id,
                    item.position_id,
                    item.link_id,
                    item.ads_name,
                    item.ads_image,
                    item.oss_url,
                    item.user_id,
                    item.page_url,
                    item.sort_num,
                    item.is_show,
                    item.ads_remark,
                    item.created_at,
                    format_time = item.created_at.ToString("yyyy-MM-dd HH:mm")
                })).FirstOrDefaultAsync();
                AddLogs((int)ENUMHelper.LogType.Search, (int)ENUMHelper.Platform.Admin + "" + (int)ENUMHelper.OperateType.Details + "" + (int)ENUMHelper.InfoType.Info, "获取广告详情ID=" + id + ",result=" + result.ToJson());
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
        /// 新增广告数据
        /// </summary>
        /// <param name="ads">Ads对象</param>
        /// <returns>返回新增广告后的单条JSON数据</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Ads ads)
        {
            try
            {
                log.Information("ads=" + ads.ToJson());
                var user_id = User.FindFirst(ClaimTypes.Sid)!.Value;
                long.TryParse(user_id, out long get_user_id);
                if (string.IsNullOrEmpty(user_id))
                {
                    user_id = "-1";
                }

                ads.user_id = get_user_id;
                ads.position_id = 1;
                ads.lang_id = 1;
                ads.oss_url = "";
                ads.created_at = DateTime.Now;
                _context.mr_ads.Add(ads);
                await _context.SaveChangesAsync();
                AddLogs((int)ENUMHelper.LogType.Create, (int)ENUMHelper.Platform.Admin + "" + (int)ENUMHelper.OperateType.Create + "" + (int)ENUMHelper.InfoType.Info, "新增广告,ads=" + ads.ToJson());

                return Json(new { code = 0, msg = "添加成功", data = ads });
            }
            catch (Exception ex)
            {
                log.Fatal(ex, " -> Create");
                return Json(new { code = 0, errcode = (int)ENUMHelper.ExceptionType.Create, errmsg = ex.Message });
            }
        }

        /// <summary>
        /// 根据ID更改广告数据
        /// </summary>
        /// <param name="id">广告ID</param>
        /// <param name="ads">Ads对象</param>
        /// <returns>返回修改广告后的单条JSON数据</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Ads ads)
        {
            try
            {
                log.Information("ads=" + ads.ToJson());
                var item = _context.mr_ads.Find(id);
                if (item != null)
                {
                    item.link_id = ads.link_id;
                    item.ads_name = ads.ads_name;
                    item.ads_image = ads.ads_image;
                    item.page_url = ads.page_url;
                    item.sort_num = ads.sort_num;
                    item.is_show = ads.is_show;
                    item.ads_remark = ads.ads_remark;
                    _context.mr_ads.Update(item);
                    await _context.SaveChangesAsync();
                    AddLogs((int)ENUMHelper.LogType.Update, (int)ENUMHelper.Platform.Admin + "" + (int)ENUMHelper.OperateType.Edit + "" + (int)ENUMHelper.InfoType.Info, "编辑广告ID=" + id + ",ads=" + ads.ToJson());

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
        /// 更新是否显示状态
        /// </summary>
        /// <param name="id">广告ID</param>
        /// <param name="status">是否显示</param>
        /// <returns>返回更新是否显示后的单条JSON数据</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetShow(long id, bool status)
        {
            try
            {
                log.Information("id=" + id + ",status=" + status);
                var item = await _context.mr_ads.FirstOrDefaultAsync(t => t.ads_id == id);
                if (item != null)
                {
                    item.is_show = status;
                    _context.mr_ads.Update(item);
                    await _context.SaveChangesAsync();
                    AddLogs((int)ENUMHelper.LogType.Update, (int)ENUMHelper.Platform.Admin + "" + (int)ENUMHelper.OperateType.Edit + "" + (int)ENUMHelper.InfoType.Info, "更新广告ID=" + id);

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
                return Json(new { code = 0, errcode = (int)ENUMHelper.ExceptionType.Edit, errmsg = ex.Message });
            }
        }

        /// <summary>
        /// 更新排序
        /// </summary>
        /// <param name="id">广告ID</param>
        /// <param name="sort_num">排序</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetSort(long id, int sort_num)
        {
            try
            {
                log.Information("id=" + id + ",sort_num=" + sort_num);
                var item = await _context.mr_ads.FirstOrDefaultAsync(t => t.ads_id == id);
                if (item != null)
                {
                    item.sort_num = sort_num;
                    _context.mr_ads.Update(item);
                    await _context.SaveChangesAsync();
                    AddLogs((int)ENUMHelper.LogType.Update, (int)ENUMHelper.Platform.Admin + "" + (int)ENUMHelper.OperateType.Edit + "" + (int)ENUMHelper.InfoType.Info, "更新广告ID=" + id);

                    return Json(new { code = 0, msg = "更新成功", data = item });
                }
                else
                {
                    return Json(new { code = 0, msg = "操作失败，ID=" + id + "不存在！" });
                }
            }
            catch (Exception ex)
            {
                log.Fatal(ex, " -> SetSort");
                return Json(new { code = 0, errcode = (int)ENUMHelper.ExceptionType.Edit, errmsg = ex.Message });
            }
        }

        /// <summary>
        /// 根据ID删除广告数据
        /// </summary>
        /// <param name="id">广告ID</param>
        /// <returns>返回删除操作信息</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                log.Information("id=" + id);
                var item = _context.mr_ads.Find(id);
                if (item == null)
                {
                    return Json(new { code = 0, msg = "操作失败，ID=" + id + "不存在！" });
                }
                _context.mr_ads.Remove(item);
                await _context.SaveChangesAsync();
                AddLogs((int)ENUMHelper.LogType.Delete, (int)ENUMHelper.Platform.Admin + "" + (int)ENUMHelper.OperateType.Logout + "" + (int)ENUMHelper.InfoType.Info, "删除广告ID=" + id + ",item=" + item.ToJson());

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
