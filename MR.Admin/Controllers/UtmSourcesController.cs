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

    /// <summary>利。
    /// 内容摘要: UtmSources 处理请求的类
    /// </summary>
    [Authorize]
    public class UtmSourcesController : BaseController<UtmSourcesController>
    {

        /// <summary>
        /// 推广来源 实例化数据上下文
        /// </summary>
        /// <param name="context"></param>
        public UtmSourcesController(MRManageContext context) : base(context)
        {

        }

        /// <summary>
        /// 获取推广来源数据
        /// </summary>
        /// <returns>返回推广来源JSON数据集合</returns>
        public async Task<IActionResult> Select()
        {
            try
            {
                var list = await _context.mr_utm_sources.OrderBy(t => t.source_id).ToListAsync();
                AddLogs((int)ENUMHelper.LogType.Search, (int)ENUMHelper.Platform.Admin + "" + (int)ENUMHelper.OperateType.Select + "" + (int)ENUMHelper.InfoType.Info, "查询推广来源下拉填充,DATA=" + list.ToJson());

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
