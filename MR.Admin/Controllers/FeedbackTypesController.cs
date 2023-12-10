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
    /// 内容摘要: 意见反馈类型
    /// </summary>
    [Authorize]
    public class FeedbackTypesController : BaseController<FeedbackTypesController>
    {

        /// <summary>
        /// 意见类别表 实例化数据上下文
        /// </summary>
        /// <param name="context"></param>
        public FeedbackTypesController(MRManageContext context) : base(context)
        {

        }

        /// <summary>
        /// 获取意见类别表数据
        /// </summary>
        /// <returns>返回意见类别表JSON数据集合</returns>
        public async Task<IActionResult> Select()
        {
            try
            {
                var list = await _context.mr_feedback_types.OrderBy(t => t.type_id).ToListAsync();
                AddLogs((int)ENUMHelper.LogType.Search, (int)ENUMHelper.Platform.Admin + "" + (int)ENUMHelper.OperateType.Select + "" + (int)ENUMHelper.InfoType.Info, "查询意见类别表下拉填充,DATA=" + list.ToJson());

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
