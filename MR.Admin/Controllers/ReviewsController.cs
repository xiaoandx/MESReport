using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MR.Manage.Data;
using MR.Models;
using Senparc.CO2NET.Extensions;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using MR.Utility.Helper;

namespace MR.Manage.Controllers
{
    /// <summary>。
    /// 内容摘要: 各类信息审核类
    /// </summary>
    public class ReviewsController : BaseController<ReviewsController>
    {

        /// <summary>
        /// 审核意见 实例化数据上下文
        /// </summary>
        /// <param name="context"></param>
        public ReviewsController(MRManageContext context) : base(context)
        {

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
        /// 新增审核意见数据(通常用于新增页面)
        /// </summary>
        /// <param name="reviews">Reviews对象</param>
        /// <returns>返回新增审核意见后的单条JSON数据</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Reviews reviews)
        {
            try
            {
                log.Information("reviews=" + reviews.ToJson());
                var user_id = User.FindFirst(ClaimTypes.Sid)!.Value;
                long.TryParse(user_id, out long get_user_id);

                reviews.user_id = get_user_id;
                reviews.created_at = DateTime.Now;
                _context.mr_reviews.Add(reviews);
                await _context.SaveChangesAsync();

                // 更新被审核对象主表状态
                if (reviews.type_id == 1)
                {
                    try
                    {
                        var item = await _context.mr_articles.FirstOrDefaultAsync(t => t.article_id == reviews.object_id);
                        if (item != null)
                        {
                            item.review_status_id = reviews.status_id;
                            _context.mr_articles.Update(item);
                            await _context.SaveChangesAsync();

                            return Json(new { code = 0, msg = "操作成功", data = item });
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Fatal(ex, " -> SetStatus");
                        return Json(new { code = 0, errcode = (int)ENUMHelper.ExceptionType.Create, errmsg = ex.Message });
                    }
                }
                AddLogs((int)ENUMHelper.LogType.Update, (int)ENUMHelper.Platform.Admin + "" + (int)ENUMHelper.OperateType.Edit + "" + (int)ENUMHelper.InfoType.Info, "新增审核意见数据,reviews=" + reviews.ToJson());

                return Json(new { code = 0, msg = "添加成功", data = reviews });
            }
            catch (Exception ex)
            {
                log.Fatal(ex, " -> Create");
                return Json(new { code = 0, errcode = (int)ENUMHelper.ExceptionType.Create, errmsg = ex.Message });
            }
        }
    }
}
