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
    /// 内容摘要: VisitChannels 处理请求的类
    /// </summary>
    [Authorize]
    public class VisitChannelsController : BaseController<VisitChannelsController>
    {

        /// <summary>
        /// 访问频道 实例化数据上下文
        /// </summary>
        /// <param name="context"></param>
        public VisitChannelsController(MRManageContext context) : base(context)
        {

        }

        /// <summary>
        /// 获取访问频道数据
        /// </summary>
        /// <returns>返回访问频道JSON数据集合</returns>
        public async Task<IActionResult> Select()
        {
            try
            {
                var list = await _context.mr_visit_channels.OrderBy(t => t.channel_id).ToListAsync();
                AddLogs((int)ENUMHelper.LogType.Search, (int)ENUMHelper.Platform.Admin + "" + (int)ENUMHelper.OperateType.Select + "" + (int)ENUMHelper.InfoType.Info, "查询访问频道下拉填充,DATA=" + list.ToJson());

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
