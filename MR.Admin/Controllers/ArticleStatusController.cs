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

namespace MR.Manage.Controllers
{

    /// <summary>
    /// 内容摘要: 文章状态
    /// </summary>
    [Authorize]
    public class ArticleStatusController : BaseController<ArticleStatusController>
    {

        /// <summary>
        /// 新闻状态 实例化数据上下文
        /// </summary>
        /// <param name="context"></param>
        public ArticleStatusController(MRManageContext context) : base(context)
        {

        }

        /// <summary>
        /// 获取文章状态数据
        /// </summary>
        /// <returns>返回新闻状态JSON数据集合</returns>
        public async Task<IActionResult> Select()
        {
            try
            {
                var list = await _context.mr_article_status.OrderBy(t => t.status_id).ToListAsync();
                AddLogs((int)ENUMHelper.LogType.Search, (int)ENUMHelper.Platform.Admin + "" + (int)ENUMHelper.OperateType.Select + "" + (int)ENUMHelper.InfoType.Info, "查询广告下拉填充,DATA=" + list.ToJson());

                return Json(new { code = 0, msg = "success", data = list });
            }
            catch (Exception ex)
            {
                log.Fatal(ex, " -> Select");
                return Json(new { code = 0, errcode = (int)ENUMHelper.ExceptionType.Select, errmsg = ex.Message });
            }
        }

    }
}
