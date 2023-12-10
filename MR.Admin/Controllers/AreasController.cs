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
    /// 内容摘要: 区域处理类
    /// </summary>
    [Authorize]
    public class AreasController : BaseController<AreasController>
    {

        /// <summary>
        /// 区域 实例化数据上下文
        /// </summary>
        /// <param name="context"></param>
        public AreasController(MRManageContext context) : base(context)
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
        /// 获取区域列表数据(通常用于管理页面或列表页面)
        /// </summary>
        /// <param name="area_name">区域名称</param>
        /// <param name="parent_id">父目录ID</param>
        /// <returns>返回区域JSON数据集合</returns>
        public async Task<IActionResult> List(string area_name, int parent_id)
        {
            try
            {
                if (parent_id == 1)
                {
                    var result = from item in _context.Set<Areas>().Where(t => t.level_type!.Contains("1"))
                                 select new
                                 {
                                     item.area_id,
                                     item.area_name,
                                     item.parent_id,
                                     item.short_name,
                                     item.level_type,
                                     item.city_code,
                                     item.zip_code,
                                     item.merger_name,
                                     item.area_lng,
                                     item.area_lat,
                                     item.area_pinyin,
                                     haveChild = _context.mr_areas.Any(t => t.parent_id == item.area_id.ToString())
                                 };

                    var lCount = result.LongCount();

                    if (!string.IsNullOrEmpty(area_name))
                    {
                        result = result.Where(t => t.area_name.Contains(area_name.Trim()));
                    }

                    var list = await result.OrderBy(t => t.area_id).ToListAsync();
                    return Json(new { code = 0, msg = "success", data = list, count = lCount });
                }
                else
                {
                    var result = from item in _context.Set<Areas>().Where(t => t.parent_id == parent_id.ToString())
                                 select new
                                 {
                                     item.area_id,
                                     item.area_name,
                                     item.parent_id,
                                     item.short_name,
                                     item.level_type,
                                     item.city_code,
                                     item.zip_code,
                                     item.merger_name,
                                     item.area_lng,
                                     item.area_lat,
                                     item.area_pinyin,
                                     haveChild = _context.mr_areas.Any(t => t.parent_id == item.area_id.ToString())
                                 };

                    var lCount = result.LongCount();
                    if (!string.IsNullOrEmpty(area_name))
                    {
                        result = result.Where(t => t.area_name.Contains(area_name.Trim()));
                    }
                    var list = await result.OrderBy(t => t.area_id).ToListAsync();
                    AddLogs((int)ENUMHelper.LogType.Search, (int)ENUMHelper.Platform.Admin + "" + (int)ENUMHelper.OperateType.List + "" + (int)ENUMHelper.InfoType.Info, "查询LINQ=" + result.Expression.ToString() + ",DATA=" + list.ToJson());
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
        /// 获取区域数据(通常用于select组件下拉填充数据)
        /// </summary>
        /// <returns>返回区域JSON数据集合</returns>
        public async Task<IActionResult> Select()
        {
            try
            {
                var list = await _context.mr_areas.OrderBy(t => t.area_id).ToListAsync();
                AddLogs((int)ENUMHelper.LogType.Search, (int)ENUMHelper.Platform.Admin + "" + (int)ENUMHelper.OperateType.Select + "" + (int)ENUMHelper.InfoType.Info, "查询区域下拉填充,DATA=" + list.ToJson());

                return Json(new { code = 0, msg = "success", data = list });
            }
            catch (Exception ex)
            {
                log.Fatal(ex, " -> Select");
                return Json(new { code = 0, errcode = (int)ENUMHelper.ExceptionType.Select, errmsg = ex.Message });
            }
        }

        /// <summary>
        /// 根据ID获取区域详情数据(通常用于详情页面)
        /// </summary>
        /// <param name="id">区域ID</param>
        /// <returns>返回单条区域JSON数据</returns>
        [HttpGet("/areas/details")]
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                log.Information("id=" + id);
                var result = await _context.mr_areas.Where(t => t.area_id == id).Select(item => (new
                {
                    item.area_id,
                    item.area_name,
                    item.parent_id,
                    item.short_name,
                    item.level_type,
                    item.city_code,
                    item.zip_code,
                    item.merger_name,
                    item.area_lng,
                    item.area_lat,
                    item.area_pinyin
                })).FirstOrDefaultAsync();
                AddLogs((int)ENUMHelper.LogType.Search, (int)ENUMHelper.Platform.Admin + "" + (int)ENUMHelper.OperateType.Details + "" + (int)ENUMHelper.InfoType.Info, "获取区域详情ID=" + id + ",result=" + result.ToJson());
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
        /// 新增区域数据(通常用于新增页面)
        /// </summary>
        /// <param name="areas">Areas对象</param>
        /// <returns>返回新增区域后的单条JSON数据</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Areas areas)
        {
            try
            {
                log.Information("areas=" + areas.ToJson());
                _context.mr_areas.Add(areas);
                await _context.SaveChangesAsync();
                AddLogs((int)ENUMHelper.LogType.Create, (int)ENUMHelper.Platform.Admin + "" + (int)ENUMHelper.OperateType.Create + "" + (int)ENUMHelper.InfoType.Info, "新增区域,areas=" + areas.ToJson());

                return Json(new { code = 0, msg = "添加成功", data = areas });
            }
            catch (Exception ex)
            {
                log.Fatal(ex, " -> Create");
                return Json(new { code = 0, errcode = (int)ENUMHelper.ExceptionType.Create, errmsg = ex.Message });
            }
        }

        /// <summary>
        /// 根据ID更改区域数据(通常用于编辑页面)
        /// </summary>
        /// <param name="id">区域ID</param>
        /// <param name="areas">Areas对象</param>
        /// <returns>返回修改区域后的单条JSON数据</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Areas areas)
        {
            try
            {
                log.Information("areas=" + areas.ToJson());
                var item = _context.mr_areas.Find(id);
                if (item != null)
                {
                    item.area_name = areas.area_name;
                    item.short_name = areas.short_name;
                    item.level_type = areas.level_type;
                    item.city_code = areas.city_code;
                    item.zip_code = areas.zip_code;
                    item.merger_name = areas.merger_name;
                    item.area_lng = areas.area_lng;
                    item.area_lat = areas.area_lat;
                    item.area_pinyin = areas.area_pinyin;
                    _context.mr_areas.Update(item);
                    await _context.SaveChangesAsync();
                    AddLogs((int)ENUMHelper.LogType.Update, (int)ENUMHelper.Platform.Admin + "" + (int)ENUMHelper.OperateType.Edit + "" + (int)ENUMHelper.InfoType.Info, "编辑区域ID=" + id + ",areas=" + areas.ToJson());

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
        /// 根据ID删除区域数据
        /// </summary>
        /// <param name="id">区域ID</param>
        /// <returns>返回删除操作信息</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                log.Information("id=" + id);
                var item = _context.mr_areas.Find(id);
                if (item == null)
                {
                    return Json(new { code = 0, msg = "操作失败，ID=" + id + "不存在！" });
                }
                _context.mr_areas.Remove(item);
                await _context.SaveChangesAsync();
                AddLogs((int)ENUMHelper.LogType.Delete, (int)ENUMHelper.Platform.Admin + "" + (int)ENUMHelper.OperateType.Logout + "" + (int)ENUMHelper.InfoType.Info, "删除区域ID=" + id + ",item=" + item.ToJson());

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
