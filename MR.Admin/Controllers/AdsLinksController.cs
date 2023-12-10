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
    /// 内容摘要: 广告链接处理类
    /// </summary>
    [Authorize]
    public class AdsLinksController : BaseController<AdsLinksController>
    {

        /// <summary>
        /// 链接类型 实例化数据上下文
        /// </summary>
        /// <param name="context"></param>
        public AdsLinksController(MRManageContext context) : base(context)
        {

        }

        /// <summary>
        /// 获取链接类型数据
        /// </summary>
        /// <returns>返回链接类型JSON数据集合</returns>
        public async Task<IActionResult> Select()
        {
            try
            {
                var list = await _context.mr_ads_links.OrderBy(t => t.link_id).ToListAsync();
                AddLogs((int)ENUMHelper.LogType.Search, (int)ENUMHelper.Platform.Admin + "" + (int)ENUMHelper.OperateType.Select + "" + (int)ENUMHelper.InfoType.Info, "查询链接类型下拉填充,DATA=" + list.ToJson());

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
