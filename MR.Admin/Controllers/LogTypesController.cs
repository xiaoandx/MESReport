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
    /// 内容摘要: 日志类型
    /// </summary>
    public class LogTypesController : BaseController<LogTypesController>
    {

        /// <summary>
        /// 日志类别表 实例化数据上下文
        /// </summary>
        /// <param name="context"></param>
        public LogTypesController(MRManageContext context) : base(context)
        {

        }

        /// <summary>
        /// 获取日志类型数据
        /// </summary>
        /// <returns>返回日志类别表JSON数据集合</returns>
        public async Task<IActionResult> Select()
        {
            try
            {
                var list = await _context.mr_log_types.OrderBy(t => t.type_id).ToListAsync();
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
