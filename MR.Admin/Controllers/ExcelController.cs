using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Aliyun.OSS;
using MR.Manage.Data;
using MR.Models;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using MR.Utility.Helper;

namespace MR.Manage.Controllers
{
    /// <summary>
    /// 内容摘要: 导出各类报表数据类
    /// </summary>
    [Authorize]
    public class ExcelController : BaseController<ExcelController>
    {

        /// <summary>
        // 导出数据Excel 实例化数据上下文
        /// </summary>
        /// <param name="context"></param>
        public ExcelController(MRManageContext context) : base(context)
        {

        }

        
        // 以下新建项目都可以拷贝重复使用
        #region
        /// <summary>
        /// 导出会员
        /// </summary>
        /// <param name="select_date">筛选日期区间</param>
        /// <param name="mobile_phone">手机号</param>
        /// <param name="member_name">昵称</param>
        /// <param name="invite_code">邀请码</param>
        /// <param name="source_id">推广来源</param>
        /// <param name="type_id">会员角色</param>
        /// <returns></returns>
        public IActionResult ExportMembers(string select_date, string mobile_phone, string member_name, string invite_code, int source_id = -1, int type_id = -1)
        {
            try
            {
                var saveDir = MR.Manage.Filters.AppSettingsFilter.GetSetting("Upload:PATH");
                var tempPath = saveDir + "\\" + DateTime.Now.Year + "\\" + DateTime.Now.ToString("MMdd") + "\\";
                string sFileName = DateTime.Now.Year + "\\" + DateTime.Now.ToString("MMdd") + "\\" + MR.Utility.Helper.CommonHelper.CreateNo() + ".xlsx";
                if (!Directory.Exists(tempPath))
                {
                    Directory.CreateDirectory(tempPath);
                }
                FileInfo file = new FileInfo(Path.Combine(saveDir!, sFileName));
                using (ExcelPackage package = new ExcelPackage(file))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("导出数据");
                    worksheet.Cells[2, 1].Value = "ID";
                    worksheet.Cells[2, 2].Value = "微信OPEN";
                    worksheet.Cells[2, 3].Value = "用户名";
                    worksheet.Cells[2, 4].Value = "手机号";
                    worksheet.Cells[2, 5].Value = "密码";
                    worksheet.Cells[2, 6].Value = "昵称";
                    worksheet.Cells[2, 7].Value = "姓名";
                    worksheet.Cells[2, 8].Value = "性别";
                    worksheet.Cells[2, 9].Value = "头像";
                    worksheet.Cells[2, 10].Value = "生日";
                    worksheet.Cells[2, 11].Value = "职位";
                    worksheet.Cells[2, 12].Value = "邀请人";
                    worksheet.Cells[2, 13].Value = "邀请码";
                    worksheet.Cells[2, 14].Value = "分享二维码图片";
                    worksheet.Cells[2, 15].Value = "推广来源";
                    worksheet.Cells[2, 16].Value = "媒介";
                    worksheet.Cells[2, 17].Value = "简介";
                    worksheet.Cells[2, 18].Value = "状态";
                    worksheet.Cells[2, 19].Value = "会员角色";
                    worksheet.Cells[2, 20].Value = "国家";
                    worksheet.Cells[2, 21].Value = "省";
                    worksheet.Cells[2, 22].Value = "市";
                    worksheet.Cells[2, 23].Value = "区/县/旗";
                    worksheet.Cells[2, 24].Value = "TOKEN";
                    worksheet.Cells[2, 25].Value = "加密密钥";
                    worksheet.Cells[2, 26].Value = "备注";
                    worksheet.Cells[2, 27].Value = "创建日期";

                    // 设置表头
                    worksheet.Cells[1, 1, 1, 27].Merge = true;
                    worksheet.Cells[1, 1].Value = "会员";
                    // worksheet.Cells.Style.WrapText = true;
                    worksheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[1, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    worksheet.Row(1).Height = 30;
                    worksheet.Row(2).Height = 25;
                    // worksheet.Cells.Style.ShrinkToFit = true;// 单元格自动适应大小

                    var result = from item in _context.Set<Members>()
                                 select new
                                 {
                                     item.member_id,
                                     item.open_id,
                                     item.member_account,
                                     item.mobile_phone,
                                     item.member_password,
                                     item.member_name,
                                     item.real_name,
                                     item.gender_id,
                                     gender_name = item.gender_id > -1 ? _context.mr_genders.FirstOrDefault(t => t.gender_id == item.gender_id)!.gender_name : "-",
                                     item.member_avatar,
                                     item.member_birth,
                                     item.member_job,
                                     item.invite_member_id,
                                     item.invite_code,
                                     item.qrcode_img,
                                     item.source_id,
                                     source_name = item.source_id > -1 ? _context.mr_utm_sources.FirstOrDefault(t => t.source_id == item.source_id)!.source_name : "-",
                                     item.medium_id,
                                     medium_name = item.medium_id > -1 ? _context.mr_utm_mediums.FirstOrDefault(t => t.medium_id == item.medium_id)!.medium_name : "-",
                                     item.member_intro,
                                     item.member_status,
                                     item.type_id,
                                     type_name = item.type_id > -1 ? _context.mr_member_types.FirstOrDefault(t => t.type_id == item.type_id)!.type_name : "-",
                                     item.country_name,
                                     item.province_name,
                                     item.city_name,
                                     item.district_name,
                                     item.member_token,
                                     item.encrypt_token,
                                     item.member_remark,
                                     item.created_at,
                                     format_time = item.created_at.ToString("yyyy-MM-dd HH:mm")
                                 };

                    if (!string.IsNullOrEmpty(mobile_phone))
                    {
                        result = result.Where(t => t.mobile_phone.Contains(mobile_phone.Trim()));
                    }
                    if (!string.IsNullOrEmpty(member_name))
                    {
                        result = result.Where(t => t.member_name.Contains(member_name.Trim()));
                    }
                    if (!string.IsNullOrEmpty(invite_code))
                    {
                        result = result.Where(t => t.invite_code.Contains(invite_code.Trim()));
                    }
                    if (source_id > 0)
                    {
                        result = result.Where(t => t.source_id == source_id);
                    }
                    if (type_id > 0)
                    {
                        result = result.Where(t => t.type_id == type_id);
                    }

                    if (!string.IsNullOrEmpty(select_date))
                    {
                        select_date = select_date.Replace(" - ", ",");
                        string[] splitDate = select_date.Split(new char[] { ',' });
                        string start_at = splitDate[0];
                        string end_at = splitDate[1];
                        result = result.Where(t => t.created_at >= Convert.ToDateTime(start_at) && t.created_at <= Convert.ToDateTime(end_at).AddDays(1));
                    }

                    var list = result.OrderByDescending(t => t.created_at).ToList();

                    // 填充值
                    if (list.Count() == 0)
                    {
                        return Json(new { code = 0, msg = "暂无数据", count = 0 });
                    }
                    for (int i = 0; i < list.Count; i++)
                    {
                        worksheet.Cells[$"A{i + 3}"].Value = list[i].member_id;
                        worksheet.Cells[$"B{i + 3}"].Value = list[i].open_id;
                        worksheet.Cells[$"C{i + 3}"].Value = list[i].member_account;
                        worksheet.Cells[$"D{i + 3}"].Value = list[i].mobile_phone;
                        worksheet.Cells[$"E{i + 3}"].Value = list[i].member_password;
                        worksheet.Cells[$"F{i + 3}"].Value = list[i].member_name;
                        worksheet.Cells[$"G{i + 3}"].Value = list[i].real_name;
                        worksheet.Cells[$"H{i + 3}"].Value = list[i].gender_name;
                        worksheet.Cells[$"I{i + 3}"].Value = list[i].member_avatar;
                        worksheet.Cells[$"J{i + 3}"].Value = list[i].member_birth;
                        worksheet.Cells[$"K{i + 3}"].Value = list[i].member_job;
                        worksheet.Cells[$"L{i + 3}"].Value = list[i].invite_member_id;
                        worksheet.Cells[$"M{i + 3}"].Value = list[i].invite_code;
                        worksheet.Cells[$"N{i + 3}"].Value = list[i].qrcode_img;
                        worksheet.Cells[$"O{i + 3}"].Value = list[i].source_name;
                        worksheet.Cells[$"P{i + 3}"].Value = list[i].medium_name;
                        worksheet.Cells[$"Q{i + 3}"].Value = list[i].member_intro;
                        worksheet.Cells[$"R{i + 3}"].Value = list[i].member_status;
                        worksheet.Cells[$"S{i + 3}"].Value = list[i].type_name;
                        worksheet.Cells[$"T{i + 3}"].Value = list[i].country_name;
                        worksheet.Cells[$"U{i + 3}"].Value = list[i].province_name;
                        worksheet.Cells[$"V{i + 3}"].Value = list[i].city_name;
                        worksheet.Cells[$"W{i + 3}"].Value = list[i].district_name;
                        worksheet.Cells[$"X{i + 3}"].Value = list[i].member_token;
                        worksheet.Cells[$"Y{i + 3}"].Value = list[i].encrypt_token;
                        worksheet.Cells[$"Z{i + 3}"].Value = list[i].member_remark;
                        worksheet.Cells[$"AA{i + 3}"].Value = list[i].format_time;
                    }
                    package.Save();
                }

                // 上传至阿里云OSS
                var newFileName = "MR.members_" + DateTime.Now.ToString("yyyyMMddHHmmss") + MR.Utility.Helper.CommonHelper.GenerateRandomNum(6);//根据表名命名
                var returnPath = "/" + DateTime.Now.Year + "/" + DateTime.Now.ToString("MMdd") + "/" + newFileName + ".xlsx";
                using FileStream fi = new FileStream(Path.Combine(saveDir!, sFileName), FileMode.Open);
                OssClient client = new OssClient(MR.Utility.Config.AliyunOSS.Endpoint, MR.Utility.Config.AliyunOSS.AccessKeyId, MR.Utility.Config.AliyunOSS.AccessKeySecret);
                var key = "upload/" + DateTime.Now.Year + "/" + DateTime.Now.ToString("MMdd") + "/";
                using (MemoryStream memStream = new MemoryStream())
                {
                    // 创建目录
                    client.PutObject(MR.Utility.Config.AliyunOSS.BucketName, key, memStream);
                }
                ObjectMetadata metadata = new ObjectMetadata();
                metadata.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                client.PutObject(MR.Utility.Config.AliyunOSS.BucketName, key + newFileName + ".xlsx", fi, metadata);
                var oss_url = MR.Manage.Filters.AppSettingsFilter.GetSetting("Aliyun:OSS_URL");

                return Json(new { code = 0, msg = "导出成功", path = oss_url + "/upload" + returnPath });
            }
            catch (Exception ex)
            {
                log.Fatal(ex, " ->ExportMembers");
                return Json(new { code = 0, errcode =  (int)ENUMHelper.ExceptionType.Chart, errmsg = ex.Message });
            }
        }

        /// <summary>
        /// 导出会员积分
        /// </summary>
        /// <param name="select_date">筛选日期区间</param>
        /// <param name="member_name">会员昵称</param>
        /// <param name="mobile_phone">手机号</param>
        /// <param name="total_point">会员总积分</param>
        /// <returns></returns>
        public IActionResult ExportPointMembers(string select_date, string member_name, string mobile_phone, int total_point = -1)
        {
            try
            {
                var saveDir = MR.Manage.Filters.AppSettingsFilter.GetSetting("Upload:PATH");
                var tempPath = saveDir + "\\" + DateTime.Now.Year + "\\" + DateTime.Now.ToString("MMdd") + "\\";
                string sFileName = DateTime.Now.Year + "\\" + DateTime.Now.ToString("MMdd") + "\\" + MR.Utility.Helper.CommonHelper.CreateNo() + ".xlsx";
                if (!Directory.Exists(tempPath))
                {
                    Directory.CreateDirectory(tempPath);
                }
                FileInfo file = new FileInfo(Path.Combine(saveDir!, sFileName));
                using (ExcelPackage package = new ExcelPackage(file))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("导出数据");
                    worksheet.Cells[2, 1].Value = "ID";
                    worksheet.Cells[2, 2].Value = "会员昵称";
                    worksheet.Cells[2, 3].Value = "手机号";
                    worksheet.Cells[2, 4].Value = "会员总积分";
                    worksheet.Cells[2, 5].Value = "可用积分";
                    worksheet.Cells[2, 6].Value = "冻结积分";
                    worksheet.Cells[2, 7].Value = "创建日期";

                    // 设置表头
                    worksheet.Cells[1, 1, 1, 7].Merge = true;
                    worksheet.Cells[1, 1].Value = "会员积分";
                    // worksheet.Cells.Style.WrapText = true;
                    worksheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[1, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    worksheet.Row(1).Height = 30;
                    worksheet.Row(2).Height = 25;
                    // worksheet.Cells.Style.ShrinkToFit = true;// 单元格自动适应大小

                    var result = from item in _context.Set<PointMembers>()
                                 select new
                                 {
                                     item.point_member_id,
                                     item.member_id,
                                     member_name = _context.mr_members.FirstOrDefault(t => t.member_id == item.member_id)!.member_name,
                                     mobile_phone = _context.mr_members.FirstOrDefault(t => t.member_id == item.member_id)!.mobile_phone,
                                     item.total_point,
                                     item.available_point,
                                     item.froze_point,
                                     item.created_at,
                                     format_time = item.created_at.ToString("yyyy-MM-dd HH:mm")
                                 };

                    if (!string.IsNullOrEmpty(member_name))
                    {
                        result = result.Where(t => t.member_name.Contains(member_name.Trim()));
                    }
                    if (!string.IsNullOrEmpty(mobile_phone))
                    {
                        result = result.Where(t => t.mobile_phone.Contains(mobile_phone.Trim()));
                    }
                    if (total_point > 0)
                    {
                        result = result.Where(t => t.total_point == total_point);
                    }

                    if (!string.IsNullOrEmpty(select_date))
                    {
                        select_date = select_date.Replace(" - ", ",");
                        string[] splitDate = select_date.Split(new char[] { ',' });
                        string start_at = splitDate[0];
                        string end_at = splitDate[1];
                        result = result.Where(t => t.created_at >= Convert.ToDateTime(start_at) && t.created_at <= Convert.ToDateTime(end_at).AddDays(1));
                    }

                    var list = result.OrderByDescending(t => t.created_at).ToList();

                    // 填充值
                    if (list.Count() == 0)
                    {
                        return Json(new { code = 0, msg = "暂无数据", count = 0 });
                    }
                    for (int i = 0; i < list.Count; i++)
                    {
                        worksheet.Cells[$"A{i + 3}"].Value = list[i].point_member_id;
                        worksheet.Cells[$"B{i + 3}"].Value = list[i].member_name;
                        worksheet.Cells[$"C{i + 3}"].Value = list[i].mobile_phone;
                        worksheet.Cells[$"D{i + 3}"].Value = list[i].total_point;
                        worksheet.Cells[$"E{i + 3}"].Value = list[i].available_point;
                        worksheet.Cells[$"F{i + 3}"].Value = list[i].froze_point;
                        worksheet.Cells[$"G{i + 3}"].Value = list[i].format_time;
                    }
                    package.Save();
                }

                // 上传至阿里云OSS
                var newFileName = "MR.pointmembers_" + DateTime.Now.ToString("yyyyMMddHHmmss") + MR.Utility.Helper.CommonHelper.GenerateRandomNum(6);//根据表名命名
                var returnPath = "/" + DateTime.Now.Year + "/" + DateTime.Now.ToString("MMdd") + "/" + newFileName + ".xlsx";
                using FileStream fi = new FileStream(Path.Combine(saveDir!, sFileName), FileMode.Open);
                OssClient client = new OssClient(MR.Utility.Config.AliyunOSS.Endpoint, MR.Utility.Config.AliyunOSS.AccessKeyId, MR.Utility.Config.AliyunOSS.AccessKeySecret);
                var key = "upload/" + DateTime.Now.Year + "/" + DateTime.Now.ToString("MMdd") + "/";
                using (MemoryStream memStream = new MemoryStream())
                {
                    // 创建目录
                    client.PutObject(MR.Utility.Config.AliyunOSS.BucketName, key, memStream);
                }
                ObjectMetadata metadata = new ObjectMetadata();
                metadata.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                client.PutObject(MR.Utility.Config.AliyunOSS.BucketName, key + newFileName + ".xlsx", fi, metadata);
                var oss_url = MR.Manage.Filters.AppSettingsFilter.GetSetting("Aliyun:OSS_URL");

                return Json(new { code = 0, msg = "导出成功", path = oss_url + "/upload" + returnPath });
            }
            catch (Exception ex)
            {
                log.Fatal(ex, " ->ExportPointMembers");
                return Json(new { code = 0, errcode = (int)ENUMHelper.ExceptionType.Chart, errmsg = ex.Message });
            }
        }

        /// <summary>
        /// 导出积分记录
        /// </summary>
        /// <param name="select_date">筛选日期区间</param>
        /// <param name="point_action">积分说明</param>
        /// <param name="point_id">积分类型</param>
        /// <param name="point_num">积分数量</param>
        /// <param name="member_id">会员ID</param>
        /// <returns></returns>
        public IActionResult ExportPointRecords(string select_date, string point_action, int point_id = -1, int point_num = -1, long member_id = -1)
        {
            try
            {
                var saveDir = MR.Manage.Filters.AppSettingsFilter.GetSetting("Upload:PATH");
                var tempPath = saveDir + "\\" + DateTime.Now.Year + "\\" + DateTime.Now.ToString("MMdd") + "\\";
                string sFileName = DateTime.Now.Year + "\\" + DateTime.Now.ToString("MMdd") + "\\" + MR.Utility.Helper.CommonHelper.CreateNo() + ".xlsx";
                if (!Directory.Exists(tempPath))
                {
                    Directory.CreateDirectory(tempPath);
                }
                FileInfo file = new FileInfo(Path.Combine(saveDir!, sFileName));
                using (ExcelPackage package = new ExcelPackage(file))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("导出数据");
                    worksheet.Cells[2, 1].Value = "ID";
                    worksheet.Cells[2, 2].Value = "会员昵称";
                    worksheet.Cells[2, 3].Value = "积分类型";
                    worksheet.Cells[2, 4].Value = "积分数量";
                    worksheet.Cells[2, 5].Value = "积分说明";
                    worksheet.Cells[2, 6].Value = "TOKEN校验";
                    worksheet.Cells[2, 7].Value = "备注";
                    worksheet.Cells[2, 8].Value = "创建日期";

                    // 设置表头
                    worksheet.Cells[1, 1, 1, 8].Merge = true;
                    worksheet.Cells[1, 1].Value = "积分记录";
                    // worksheet.Cells.Style.WrapText = true;
                    worksheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[1, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    worksheet.Row(1).Height = 30;
                    worksheet.Row(2).Height = 25;
                    // worksheet.Cells.Style.ShrinkToFit = true;// 单元格自动适应大小

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

                    if (!string.IsNullOrEmpty(select_date))
                    {
                        select_date = select_date.Replace(" - ", ",");
                        string[] splitDate = select_date.Split(new char[] { ',' });
                        string start_at = splitDate[0];
                        string end_at = splitDate[1];
                        result = result.Where(t => t.created_at >= Convert.ToDateTime(start_at) && t.created_at <= Convert.ToDateTime(end_at).AddDays(1));
                    }

                    var list = result.OrderByDescending(t => t.created_at).ToList();

                    // 填充值
                    if (list.Count() == 0)
                    {
                        return Json(new { code = 0, msg = "暂无数据", count = 0 });
                    }
                    for (int i = 0; i < list.Count; i++)
                    {
                        worksheet.Cells[$"A{i + 3}"].Value = list[i].record_id;
                        worksheet.Cells[$"B{i + 3}"].Value = list[i].member_name;
                        worksheet.Cells[$"C{i + 3}"].Value = list[i].point_name;
                        worksheet.Cells[$"D{i + 3}"].Value = list[i].point_num;
                        worksheet.Cells[$"E{i + 3}"].Value = list[i].point_action;
                        worksheet.Cells[$"F{i + 3}"].Value = list[i].point_token;
                        worksheet.Cells[$"G{i + 3}"].Value = list[i].point_remark;
                        worksheet.Cells[$"H{i + 3}"].Value = list[i].format_time;
                    }
                    package.Save();
                }

                // 上传至阿里云OSS
                var newFileName = "MR.pointrecords_" + DateTime.Now.ToString("yyyyMMddHHmmss") + MR.Utility.Helper.CommonHelper.GenerateRandomNum(6);//根据表名命名
                var returnPath = "/" + DateTime.Now.Year + "/" + DateTime.Now.ToString("MMdd") + "/" + newFileName + ".xlsx";
                using FileStream fi = new FileStream(Path.Combine(saveDir!, sFileName), FileMode.Open);
                OssClient client = new OssClient(MR.Utility.Config.AliyunOSS.Endpoint, MR.Utility.Config.AliyunOSS.AccessKeyId, MR.Utility.Config.AliyunOSS.AccessKeySecret);
                var key = "upload/" + DateTime.Now.Year + "/" + DateTime.Now.ToString("MMdd") + "/";
                using (MemoryStream memStream = new MemoryStream())
                {
                    // 创建目录
                    client.PutObject(MR.Utility.Config.AliyunOSS.BucketName, key, memStream);
                }
                ObjectMetadata metadata = new ObjectMetadata();
                metadata.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                client.PutObject(MR.Utility.Config.AliyunOSS.BucketName, key + newFileName + ".xlsx", fi, metadata);
                var oss_url = MR.Manage.Filters.AppSettingsFilter.GetSetting("Aliyun:OSS_URL");

                return Json(new { code = 0, msg = "导出成功", path = oss_url + "/upload" + returnPath });
            }
            catch (Exception ex)
            {
                log.Fatal(ex, " ->ExportPointRecords");
                return Json(new { code = 0, errcode = (int)ENUMHelper.ExceptionType.Chart, errmsg = ex.Message });
            }
        }

        /// <summary>
        /// 导出用户
        /// </summary>
        /// <param name="select_date">筛选日期区间</param>
        /// <param name="user_name">用户名</param>
        /// <param name="nick_name">昵称</param>
        /// <param name="mobile_phone">手机号码</param>
        /// <param name="user_job">职位</param>
        /// <param name="role_id">用户角色</param>
        /// <param name="user_status">状态</para>
        /// <returns></returns>
        public IActionResult ExportUsers(string select_date, string user_name, string nick_name, string mobile_phone, string user_job, int role_id = -1, int user_status = -1)
        {
            try
            {
                var saveDir = MR.Manage.Filters.AppSettingsFilter.GetSetting("Upload:PATH");
                var tempPath = saveDir + "\\" + DateTime.Now.Year + "\\" + DateTime.Now.ToString("MMdd") + "\\";
                string sFileName = DateTime.Now.Year + "\\" + DateTime.Now.ToString("MMdd") + "\\" + MR.Utility.Helper.CommonHelper.CreateNo() + ".xlsx";
                if (!Directory.Exists(tempPath))
                {
                    Directory.CreateDirectory(tempPath);
                }
                FileInfo file = new FileInfo(Path.Combine(saveDir!, sFileName));
                using (ExcelPackage package = new ExcelPackage(file))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("导出数据");
                    worksheet.Cells[2, 1].Value = "ID";
                    worksheet.Cells[2, 2].Value = "用户名";
                    worksheet.Cells[2, 3].Value = "头像";
                    worksheet.Cells[2, 4].Value = "密码";
                    worksheet.Cells[2, 5].Value = "昵称";
                    worksheet.Cells[2, 6].Value = "姓名";
                    worksheet.Cells[2, 7].Value = "手机号";
                    worksheet.Cells[2, 8].Value = "Email";
                    worksheet.Cells[2, 9].Value = "职位";
                    worksheet.Cells[2, 10].Value = "性别";
                    worksheet.Cells[2, 11].Value = "角色";
                    worksheet.Cells[2, 12].Value = "签名";
                    worksheet.Cells[2, 13].Value = "介绍";
                    worksheet.Cells[2, 14].Value = "限制IP段登录";
                    worksheet.Cells[2, 15].Value = "状态";
                    worksheet.Cells[2, 16].Value = "TOKEN";
                    worksheet.Cells[2, 17].Value = "最后登录时间";
                    worksheet.Cells[2, 18].Value = "创建日期";

                    // 设置表头
                    worksheet.Cells[1, 1, 1, 18].Merge = true;
                    worksheet.Cells[1, 1].Value = "用户";
                    // worksheet.Cells.Style.WrapText = true;
                    worksheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[1, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    worksheet.Row(1).Height = 30;
                    worksheet.Row(2).Height = 25;
                    // worksheet.Cells.Style.ShrinkToFit = true;// 单元格自动适应大小

                    var result = from item in _context.Set<Users>()
                                 select new
                                 {
                                     item.user_id,
                                     item.user_name,
                                     item.user_avatar,
                                     item.user_password,
                                     item.nick_name,
                                     item.real_name,
                                     item.mobile_phone,
                                     item.user_email,
                                     item.user_job,
                                     item.gender_id,
                                     gender_name = item.gender_id > -1 ? _context.mr_genders.FirstOrDefault(t => t.gender_id == item.gender_id)!.gender_name : "-",
                                     item.role_id,
                                     role_name = item.role_id > -1 ? _context.mr_user_roles.FirstOrDefault(t => t.role_id == item.role_id)!.role_name : "-",
                                     item.user_slogan,
                                     item.user_intro,
                                     item.limit_ip,
                                     item.user_status,
                                     item.user_token,
                                     item.last_login_at,
                                     format_last_login_at = item.last_login_at.ToString("yyyy-MM-dd HH:mm"),
                                     item.created_at,
                                     format_time = item.created_at.ToString("yyyy-MM-dd HH:mm")
                                 };

                    if (!string.IsNullOrEmpty(user_name))
                    {
                        result = result.Where(t => t.user_name.Contains(user_name.Trim()));
                    }
                    if (!string.IsNullOrEmpty(nick_name))
                    {
                        result = result.Where(t => t.nick_name.Contains(nick_name.Trim()));
                    }
                    if (!string.IsNullOrEmpty(mobile_phone))
                    {
                        result = result.Where(t => t.mobile_phone.Contains(mobile_phone.Trim()));
                    }
                    if (!string.IsNullOrEmpty(user_job))
                    {
                        result = result.Where(t => t.user_job.Contains(user_job.Trim()));
                    }
                    if (role_id > 0)
                    {
                        result = result.Where(t => t.role_id == role_id);
                    }
                    if (user_status > -1)
                    {
                        if (user_status == 1)
                        {
                            result = result.Where(t => t.user_status == true);
                        }
                        if (user_status == 0)
                        {
                            result = result.Where(t => t.user_status == false);
                        }
                    }

                    if (!string.IsNullOrEmpty(select_date))
                    {
                        select_date = select_date.Replace(" - ", ",");
                        string[] splitDate = select_date.Split(new char[] { ',' });
                        string start_at = splitDate[0];
                        string end_at = splitDate[1];
                        result = result.Where(t => t.created_at >= Convert.ToDateTime(start_at) && t.created_at <= Convert.ToDateTime(end_at).AddDays(1));
                    }

                    var list = result.OrderByDescending(t => t.created_at).ToList();

                    // 填充值
                    if (list.Count() == 0)
                    {
                        return Json(new { code = 0, msg = "暂无数据", count = 0 });
                    }
                    for (int i = 0; i < list.Count; i++)
                    {
                        worksheet.Cells[$"A{i + 3}"].Value = list[i].user_id;
                        worksheet.Cells[$"B{i + 3}"].Value = list[i].user_name;
                        worksheet.Cells[$"C{i + 3}"].Value = list[i].user_avatar;
                        worksheet.Cells[$"D{i + 3}"].Value = list[i].user_password;
                        worksheet.Cells[$"E{i + 3}"].Value = list[i].nick_name;
                        worksheet.Cells[$"F{i + 3}"].Value = list[i].real_name;
                        worksheet.Cells[$"G{i + 3}"].Value = list[i].mobile_phone;
                        worksheet.Cells[$"H{i + 3}"].Value = list[i].user_email;
                        worksheet.Cells[$"I{i + 3}"].Value = list[i].user_job;
                        worksheet.Cells[$"J{i + 3}"].Value = list[i].gender_name;
                        worksheet.Cells[$"K{i + 3}"].Value = list[i].role_name;
                        worksheet.Cells[$"L{i + 3}"].Value = list[i].user_slogan;
                        worksheet.Cells[$"M{i + 3}"].Value = list[i].user_intro;
                        worksheet.Cells[$"N{i + 3}"].Value = list[i].limit_ip;
                        worksheet.Cells[$"O{i + 3}"].Value = list[i].user_status ? "启用" : "禁用";
                        worksheet.Cells[$"P{i + 3}"].Value = list[i].user_token;
                        worksheet.Cells[$"Q{i + 3}"].Value = list[i].format_last_login_at;
                        worksheet.Cells[$"R{i + 3}"].Value = list[i].format_time;
                    }
                    package.Save();
                }

                // 上传至阿里云OSS
                var newFileName = "MR.users_" + DateTime.Now.ToString("yyyyMMddHHmmss") + MR.Utility.Helper.CommonHelper.GenerateRandomNum(6);//根据表名命名
                var returnPath = "/" + DateTime.Now.Year + "/" + DateTime.Now.ToString("MMdd") + "/" + newFileName + ".xlsx";
                using FileStream fi = new FileStream(Path.Combine(saveDir!, sFileName), FileMode.Open);
                OssClient client = new OssClient(MR.Utility.Config.AliyunOSS.Endpoint, MR.Utility.Config.AliyunOSS.AccessKeyId, MR.Utility.Config.AliyunOSS.AccessKeySecret);
                var key = "upload/" + DateTime.Now.Year + "/" + DateTime.Now.ToString("MMdd") + "/";
                using (MemoryStream memStream = new MemoryStream())
                {
                    // 创建目录
                    client.PutObject(MR.Utility.Config.AliyunOSS.BucketName, key, memStream);
                }
                ObjectMetadata metadata = new ObjectMetadata();
                metadata.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                client.PutObject(MR.Utility.Config.AliyunOSS.BucketName, key + newFileName + ".xlsx", fi, metadata);
                var oss_url = MR.Manage.Filters.AppSettingsFilter.GetSetting("Aliyun:OSS_URL");

                return Json(new { code = 0, msg = "导出成功", path = oss_url + "/upload" + returnPath });
            }
            catch (Exception ex)
            {
                log.Fatal(ex, " ->ExportUsers");
                return Json(new { code = 0, errcode =  (int)ENUMHelper.ExceptionType.Chart, errmsg = ex.Message });
            }
        }

        /// <summary>
        /// 导出验证码
        /// </summary>
        /// <param name="select_date">筛选日期区间</param>
        /// <param name="mobile_phone">手机号码</param>
        /// <param name="code_from">客户端</param>
        /// <param name="created_at">创建日期</para>
        /// <returns></returns>
        public IActionResult ExportCodes(string select_date, string mobile_phone, string code_from, string created_at)
        {
            try
            {
                var saveDir = MR.Manage.Filters.AppSettingsFilter.GetSetting("Upload:PATH");
                var tempPath = saveDir + "\\" + DateTime.Now.Year + "\\" + DateTime.Now.ToString("MMdd") + "\\";
                string sFileName = DateTime.Now.Year + "\\" + DateTime.Now.ToString("MMdd") + "\\" + MR.Utility.Helper.CommonHelper.CreateNo() + ".xlsx";
                if (!Directory.Exists(tempPath))
                {
                    Directory.CreateDirectory(tempPath);
                }
                FileInfo file = new FileInfo(Path.Combine(saveDir!, sFileName));
                using (ExcelPackage package = new ExcelPackage(file))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("导出数据");
                    worksheet.Cells[2, 1].Value = "ID";
                    worksheet.Cells[2, 2].Value = "国家代码";
                    worksheet.Cells[2, 3].Value = "手机号码";
                    worksheet.Cells[2, 4].Value = "验证码";
                    worksheet.Cells[2, 5].Value = "客户端";
                    worksheet.Cells[2, 6].Value = "有效期";
                    worksheet.Cells[2, 7].Value = "创建日期";

                    // 设置表头
                    worksheet.Cells[1, 1, 1, 7].Merge = true;
                    worksheet.Cells[1, 1].Value = "验证码";
                    // worksheet.Cells.Style.WrapText = true;
                    worksheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[1, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    worksheet.Row(1).Height = 30;
                    worksheet.Row(2).Height = 25;
                    // worksheet.Cells.Style.ShrinkToFit = true;// 单元格自动适应大小

                    var result = from item in _context.Set<Codes>()
                                 select new
                                 {
                                     item.code_id,
                                     item.country_code,
                                     item.mobile_phone,
                                     item.code_num,
                                     item.code_from,
                                     item.due_minute,
                                     item.created_at,
                                     format_time = item.created_at.ToString("yyyy-MM-dd HH:mm")
                                 };

                    if (!string.IsNullOrEmpty(mobile_phone))
                    {
                        result = result.Where(t => t.mobile_phone.Contains(mobile_phone.Trim()));
                    }
                    if (!string.IsNullOrEmpty(code_from))
                    {
                        result = result.Where(t => t.code_from.Contains(code_from.Trim()));
                    }

                    if (!string.IsNullOrEmpty(select_date))
                    {
                        select_date = select_date.Replace(" - ", ",");
                        string[] splitDate = select_date.Split(new char[] { ',' });
                        string start_at = splitDate[0];
                        string end_at = splitDate[1];
                        result = result.Where(t => t.created_at >= Convert.ToDateTime(start_at) && t.created_at <= Convert.ToDateTime(end_at).AddDays(1));
                    }

                    var list = result.OrderByDescending(t => t.created_at).ToList();

                    // 填充值
                    if (list.Count() == 0)
                    {
                        return Json(new { code = 0, msg = "暂无数据", count = 0 });
                    }
                    for (int i = 0; i < list.Count; i++)
                    {
                        worksheet.Cells[$"A{i + 3}"].Value = list[i].code_id;
                        worksheet.Cells[$"B{i + 3}"].Value = list[i].country_code;
                        worksheet.Cells[$"C{i + 3}"].Value = list[i].mobile_phone;
                        worksheet.Cells[$"D{i + 3}"].Value = list[i].code_num;
                        worksheet.Cells[$"E{i + 3}"].Value = list[i].code_from;
                        worksheet.Cells[$"F{i + 3}"].Value = list[i].due_minute;
                        worksheet.Cells[$"G{i + 3}"].Value = list[i].format_time;
                    }
                    package.Save();
                }

                // 上传至阿里云OSS
                var newFileName = "MR.codes_" + DateTime.Now.ToString("yyyyMMddHHmmss") + MR.Utility.Helper.CommonHelper.GenerateRandomNum(6);//根据表名命名
                var returnPath = "/" + DateTime.Now.Year + "/" + DateTime.Now.ToString("MMdd") + "/" + newFileName + ".xlsx";
                using FileStream fi = new FileStream(Path.Combine(saveDir!, sFileName), FileMode.Open);
                OssClient client = new OssClient(MR.Utility.Config.AliyunOSS.Endpoint, MR.Utility.Config.AliyunOSS.AccessKeyId, MR.Utility.Config.AliyunOSS.AccessKeySecret);
                var key = "upload/" + DateTime.Now.Year + "/" + DateTime.Now.ToString("MMdd") + "/";
                using (MemoryStream memStream = new MemoryStream())
                {
                    // 创建目录
                    client.PutObject(MR.Utility.Config.AliyunOSS.BucketName, key, memStream);
                }
                ObjectMetadata metadata = new ObjectMetadata();
                metadata.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                client.PutObject(MR.Utility.Config.AliyunOSS.BucketName, key + newFileName + ".xlsx", fi, metadata);
                var oss_url = MR.Manage.Filters.AppSettingsFilter.GetSetting("Aliyun:OSS_URL");

                return Json(new { code = 0, msg = "导出成功", path = oss_url + "/upload" + returnPath });
            }
            catch (Exception ex)
            {
                log.Fatal(ex, " ->ExportCodes");
                return Json(new { code = 0, errcode =  (int)ENUMHelper.ExceptionType.Chart, errmsg = ex.Message });
            }
        }

        /// <summary>
        /// 导出短信
        /// </summary>
        /// <param name="select_date">筛选日期区间</param>
        /// <param name="mobile_phone">手机号</param>
        /// <param name="sms_content">短信内容</para>
        /// <returns></returns>
        public IActionResult ExportSms(string select_date, string mobile_phone, string sms_content)
        {
            try
            {
                var saveDir = MR.Manage.Filters.AppSettingsFilter.GetSetting("Upload:PATH");
                var tempPath = saveDir + "\\" + DateTime.Now.Year + "\\" + DateTime.Now.ToString("MMdd") + "\\";
                string sFileName = DateTime.Now.Year + "\\" + DateTime.Now.ToString("MMdd") + "\\" + MR.Utility.Helper.CommonHelper.CreateNo() + ".xlsx";
                if (!Directory.Exists(tempPath))
                {
                    Directory.CreateDirectory(tempPath);
                }
                FileInfo file = new FileInfo(Path.Combine(saveDir!, sFileName));
                using (ExcelPackage package = new ExcelPackage(file))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("导出数据");
                    worksheet.Cells[2, 1].Value = "ID";
                    worksheet.Cells[2, 2].Value = "手机号";
                    worksheet.Cells[2, 3].Value = "短信内容";
                    worksheet.Cells[2, 4].Value = "接口返回信息";
                    worksheet.Cells[2, 5].Value = "创建日期";

                    // 设置表头
                    worksheet.Cells[1, 1, 1, 5].Merge = true;
                    worksheet.Cells[1, 1].Value = "短信";
                    // worksheet.Cells.Style.WrapText = true;
                    worksheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[1, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    worksheet.Row(1).Height = 30;
                    worksheet.Row(2).Height = 25;
                    // worksheet.Cells.Style.ShrinkToFit = true;// 单元格自动适应大小

                    var result = from item in _context.Set<Sms>()
                                 select new
                                 {
                                     item.sms_id,
                                     item.mobile_phone,
                                     item.sms_content,
                                     item.result_content,
                                     send_status = item.result_content!.Contains("OK") || item.result_content.Contains("stat=100") ? "发送成功" : "发送失败",
                                     item.created_at,
                                     format_time = item.created_at.ToString("yyyy-MM-dd HH:mm")
                                 };

                    if (!string.IsNullOrEmpty(mobile_phone))
                    {
                        result = result.Where(t => t.mobile_phone.Contains(mobile_phone.Trim()));
                    }
                    if (!string.IsNullOrEmpty(sms_content))
                    {
                        result = result.Where(t => t.sms_content.Contains(sms_content.Trim()));
                    }

                    if (!string.IsNullOrEmpty(select_date))
                    {
                        select_date = select_date.Replace(" - ", ",");
                        string[] splitDate = select_date.Split(new char[] { ',' });
                        string start_at = splitDate[0];
                        string end_at = splitDate[1];
                        result = result.Where(t => t.created_at >= Convert.ToDateTime(start_at) && t.created_at <= Convert.ToDateTime(end_at).AddDays(1));
                    }

                    var list = result.OrderByDescending(t => t.created_at).ToList();

                    // 填充值
                    if (list.Count() == 0)
                    {
                        return Json(new { code = 0, msg = "暂无数据", count = 0 });
                    }
                    for (int i = 0; i < list.Count; i++)
                    {
                        worksheet.Cells[$"A{i + 3}"].Value = list[i].sms_id;
                        worksheet.Cells[$"B{i + 3}"].Value = list[i].mobile_phone;
                        worksheet.Cells[$"C{i + 3}"].Value = list[i].sms_content;
                        worksheet.Cells[$"D{i + 3}"].Value = list[i].send_status;
                        worksheet.Cells[$"E{i + 3}"].Value = list[i].format_time;
                    }
                    package.Save();
                }

                // 上传至阿里云OSS
                var newFileName = "MR.sms_" + DateTime.Now.ToString("yyyyMMddHHmmss") + MR.Utility.Helper.CommonHelper.GenerateRandomNum(6);//根据表名命名
                var returnPath = "/" + DateTime.Now.Year + "/" + DateTime.Now.ToString("MMdd") + "/" + newFileName + ".xlsx";
                using FileStream fi = new FileStream(Path.Combine(saveDir!, sFileName), FileMode.Open);
                OssClient client = new OssClient(MR.Utility.Config.AliyunOSS.Endpoint, MR.Utility.Config.AliyunOSS.AccessKeyId, MR.Utility.Config.AliyunOSS.AccessKeySecret);
                var key = "upload/" + DateTime.Now.Year + "/" + DateTime.Now.ToString("MMdd") + "/";
                using (MemoryStream memStream = new MemoryStream())
                {
                    // 创建目录
                    client.PutObject(MR.Utility.Config.AliyunOSS.BucketName, key, memStream);
                }
                ObjectMetadata metadata = new ObjectMetadata();
                metadata.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                client.PutObject(MR.Utility.Config.AliyunOSS.BucketName, key + newFileName + ".xlsx", fi, metadata);
                var oss_url = MR.Manage.Filters.AppSettingsFilter.GetSetting("Aliyun:OSS_URL");

                return Json(new { code = 0, msg = "导出成功", path = oss_url + "/upload" + returnPath });
            }
            catch (Exception ex)
            {
                log.Fatal(ex, " ->ExportSms");
                return Json(new { code = 0, errcode =  (int)ENUMHelper.ExceptionType.Chart, errmsg = ex.Message });
            }
        }

        /// <summary>
        /// 导出意见反馈
        /// </summary>
        /// <param name="select_date">筛选日期区间</param>
        /// <param name="feedback_name">姓名</param>
        /// <param name="feedback_email">Email</param>
        /// <param name="feedback_phone">联系电话</param>
        /// <param name="feedback_content">反馈内容</para>
        /// <param name="type_id">意见类型</param>
        /// <returns></returns>
        public IActionResult ExportFeedback(string select_date, string feedback_name, string feedback_email, string feedback_phone, string feedback_content, int type_id = -1)
        {
            try
            {
                var saveDir = MR.Manage.Filters.AppSettingsFilter.GetSetting("Upload:PATH");
                var tempPath = saveDir + "\\" + DateTime.Now.Year + "\\" + DateTime.Now.ToString("MMdd") + "\\";
                string sFileName = DateTime.Now.Year + "\\" + DateTime.Now.ToString("MMdd") + "\\" + MR.Utility.Helper.CommonHelper.CreateNo() + ".xlsx";
                if (!Directory.Exists(tempPath))
                {
                    Directory.CreateDirectory(tempPath);
                }
                FileInfo file = new FileInfo(Path.Combine(saveDir!, sFileName));
                using (ExcelPackage package = new ExcelPackage(file))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("导出数据");
                    worksheet.Cells[2, 1].Value = "ID";
                    worksheet.Cells[2, 2].Value = "意见类型";
                    worksheet.Cells[2, 3].Value = "会员昵称";
                    worksheet.Cells[2, 4].Value = "姓名";
                    worksheet.Cells[2, 5].Value = "Email";
                    worksheet.Cells[2, 6].Value = "联系电话";
                    worksheet.Cells[2, 7].Value = "反馈内容";
                    worksheet.Cells[2, 8].Value = "客户端IP";
                    worksheet.Cells[2, 9].Value = "备注";
                    worksheet.Cells[2, 10].Value = "创建日期";

                    // 设置表头
                    worksheet.Cells[1, 1, 1, 10].Merge = true;
                    worksheet.Cells[1, 1].Value = "意见反馈";
                    // worksheet.Cells.Style.WrapText = true;
                    worksheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[1, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    worksheet.Row(1).Height = 30;
                    worksheet.Row(2).Height = 25;
                    // worksheet.Cells.Style.ShrinkToFit = true;// 单元格自动适应大小

                    var result = from item in _context.Set<Feedback>()
                                 select new
                                 {
                                     item.feedback_id,
                                     item.type_id,
                                     type_name = item.type_id > -1 ? _context.mr_feedback_types.FirstOrDefault(t => t.type_id == item.type_id)!.type_name : "-",
                                     item.member_id,
                                     member_name = item.member_id > -1 ? _context.mr_members.FirstOrDefault(t => t.member_id == item.member_id)!.member_name : "-",
                                     item.feedback_name,
                                     item.feedback_email,
                                     item.feedback_phone,
                                     item.feedback_content,
                                     item.client_ip,
                                     item.feedback_remark,
                                     item.created_at,
                                     format_time = item.created_at.ToString("yyyy-MM-dd HH:mm")
                                 };

                    if (type_id > 0)
                    {
                        result = result.Where(t => t.type_id == type_id);
                    }
                    if (!string.IsNullOrEmpty(feedback_name))
                    {
                        result = result.Where(t => t.feedback_name.Contains(feedback_name.Trim()));
                    }
                    if (!string.IsNullOrEmpty(feedback_email))
                    {
                        result = result.Where(t => t.feedback_email.Contains(feedback_email.Trim()));
                    }
                    if (!string.IsNullOrEmpty(feedback_phone))
                    {
                        result = result.Where(t => t.feedback_phone.Contains(feedback_phone.Trim()));
                    }
                    if (!string.IsNullOrEmpty(feedback_content))
                    {
                        result = result.Where(t => t.feedback_content.Contains(feedback_content.Trim()));
                    }

                    if (!string.IsNullOrEmpty(select_date))
                    {
                        select_date = select_date.Replace(" - ", ",");
                        string[] splitDate = select_date.Split(new char[] { ',' });
                        string start_at = splitDate[0];
                        string end_at = splitDate[1];
                        result = result.Where(t => t.created_at >= Convert.ToDateTime(start_at) && t.created_at <= Convert.ToDateTime(end_at).AddDays(1));
                    }

                    var list = result.OrderByDescending(t => t.created_at).ToList();

                    // 填充值
                    if (list.Count() == 0)
                    {
                        return Json(new { code = 0, msg = "暂无数据", count = 0 });
                    }
                    for (int i = 0; i < list.Count; i++)
                    {
                        worksheet.Cells[$"A{i + 3}"].Value = list[i].feedback_id;
                        worksheet.Cells[$"B{i + 3}"].Value = list[i].type_name;
                        worksheet.Cells[$"C{i + 3}"].Value = list[i].member_name;
                        worksheet.Cells[$"D{i + 3}"].Value = list[i].feedback_name;
                        worksheet.Cells[$"E{i + 3}"].Value = list[i].feedback_email;
                        worksheet.Cells[$"F{i + 3}"].Value = list[i].feedback_phone;
                        worksheet.Cells[$"G{i + 3}"].Value = list[i].feedback_content;
                        worksheet.Cells[$"H{i + 3}"].Value = list[i].client_ip;
                        worksheet.Cells[$"I{i + 3}"].Value = list[i].feedback_remark;
                        worksheet.Cells[$"J{i + 3}"].Value = list[i].format_time;
                    }
                    package.Save();
                }

                // 上传至阿里云OSS
                var newFileName = "MR.feedback_" + DateTime.Now.ToString("yyyyMMddHHmmss") + MR.Utility.Helper.CommonHelper.GenerateRandomNum(6);//根据表名命名
                var returnPath = "/" + DateTime.Now.Year + "/" + DateTime.Now.ToString("MMdd") + "/" + newFileName + ".xlsx";
                using FileStream fi = new FileStream(Path.Combine(saveDir!, sFileName), FileMode.Open);
                OssClient client = new OssClient(MR.Utility.Config.AliyunOSS.Endpoint, MR.Utility.Config.AliyunOSS.AccessKeyId, MR.Utility.Config.AliyunOSS.AccessKeySecret);
                var key = "upload/" + DateTime.Now.Year + "/" + DateTime.Now.ToString("MMdd") + "/";
                using (MemoryStream memStream = new MemoryStream())
                {
                    // 创建目录
                    client.PutObject(MR.Utility.Config.AliyunOSS.BucketName, key, memStream);
                }
                ObjectMetadata metadata = new ObjectMetadata();
                metadata.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                client.PutObject(MR.Utility.Config.AliyunOSS.BucketName, key + newFileName + ".xlsx", fi, metadata);
                var oss_url = MR.Manage.Filters.AppSettingsFilter.GetSetting("Aliyun:OSS_URL");

                return Json(new { code = 0, msg = "导出成功", path = oss_url + "/upload" + returnPath });
            }
            catch (Exception ex)
            {
                log.Fatal(ex, " ->ExportFeedback");
                return Json(new { code = 0, errcode =  (int)ENUMHelper.ExceptionType.Chart, errmsg = ex.Message });
            }
        }

        /// <summary>
        /// 导出消息
        /// </summary>
        /// <param name="select_date">筛选日期区间</param>
        /// <param name="member_name">会员昵称</param>
        /// <param name="message_title">消息标题</param>
        /// <param name="message_content">消息内容</param>
        /// <param name="is_read">是否已读</param>
        /// <param name="type_id">消息类别</param>
        /// <returns></returns>
        public IActionResult ExportMessages(string select_date, string member_name, string message_title, string message_content, int is_read = -1, int type_id = -1)
        {
            try
            {
                var saveDir = MR.Manage.Filters.AppSettingsFilter.GetSetting("Upload:PATH");
                var tempPath = saveDir + "\\" + DateTime.Now.Year + "\\" + DateTime.Now.ToString("MMdd") + "\\";
                string sFileName = DateTime.Now.Year + "\\" + DateTime.Now.ToString("MMdd") + "\\" + MR.Utility.Helper.CommonHelper.CreateNo() + ".xlsx";
                if (!Directory.Exists(tempPath))
                {
                    Directory.CreateDirectory(tempPath);
                }
                FileInfo file = new FileInfo(Path.Combine(saveDir!, sFileName));
                using (ExcelPackage package = new ExcelPackage(file))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("导出数据");
                    worksheet.Cells[2, 1].Value = "ID";
                    worksheet.Cells[2, 2].Value = "对象";
                    worksheet.Cells[2, 3].Value = "对象类型";
                    worksheet.Cells[2, 4].Value = "消息类别";
                    worksheet.Cells[2, 5].Value = "会员";
                    worksheet.Cells[2, 6].Value = "接收消息会员";
                    worksheet.Cells[2, 7].Value = "消息标题";
                    worksheet.Cells[2, 8].Value = "消息内容";
                    worksheet.Cells[2, 9].Value = "图片路径";
                    worksheet.Cells[2, 10].Value = "是否已读";
                    worksheet.Cells[2, 11].Value = "备注";
                    worksheet.Cells[2, 12].Value = "创建日期";

                    // 设置表头
                    worksheet.Cells[1, 1, 1, 12].Merge = true;
                    worksheet.Cells[1, 1].Value = "消息";
                    // worksheet.Cells.Style.WrapText = true;
                    worksheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[1, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    worksheet.Row(1).Height = 30;
                    worksheet.Row(2).Height = 25;
                    // worksheet.Cells.Style.ShrinkToFit = true;// 单元格自动适应大小

                    var result = from item in _context.Set<Messages>()
                                 select new
                                 {
                                     item.message_id,
                                     item.object_id,
                                     item.object_type_id,
                                     object_name = item.type_id > -1 ? _context.mr_objects.FirstOrDefault(t => t.object_id == item.object_type_id)!.object_name : "-",
                                     item.type_id,
                                     type_name = item.type_id > -1 ? _context.mr_message_types.FirstOrDefault(t => t.type_id == item.type_id)!.type_name : "-",
                                     item.member_id,
                                     member_name = item.member_id > -1 ? _context.mr_members.FirstOrDefault(t => t.member_id == item.member_id)!.member_name : "-",
                                     item.to_member_id,
                                     item.message_title,
                                     item.message_content,
                                     item.message_img,
                                     item.is_read,
                                     item.message_remark,
                                     item.created_at,
                                     format_time = item.created_at.ToString("yyyy-MM-dd HH:mm:ss")
                                 };

                    if (type_id > 0)
                    {
                        result = result.Where(t => t.type_id == type_id);
                    }
                    if (!string.IsNullOrEmpty(member_name))
                    {
                        result = result.Where(t => t.member_name.Contains(member_name.Trim()));
                    }
                    if (!string.IsNullOrEmpty(message_title))
                    {
                        result = result.Where(t => t.message_title.Contains(message_title.Trim()));
                    }
                    if (!string.IsNullOrEmpty(message_content))
                    {
                        result = result.Where(t => t.message_content.Contains(message_content.Trim()));
                    }
                    if (is_read > -1)
                    {
                        if (is_read == 1)
                        {
                            result = result.Where(t => t.is_read == true);
                        }
                        if (is_read == 0)
                        {
                            result = result.Where(t => t.is_read == false);
                        }
                    }

                    if (!string.IsNullOrEmpty(select_date))
                    {
                        select_date = select_date.Replace(" - ", ",");
                        string[] splitDate = select_date.Split(new char[] { ',' });
                        string start_at = splitDate[0];
                        string end_at = splitDate[1];
                        result = result.Where(t => t.created_at >= Convert.ToDateTime(start_at) && t.created_at <= Convert.ToDateTime(end_at).AddDays(1));
                    }

                    var list = result.OrderByDescending(t => t.created_at).ToList();

                    // 填充值
                    if (list.Count() == 0)
                    {
                        return Json(new { code = 0, msg = "暂无数据", count = 0 });
                    }
                    for (int i = 0; i < list.Count; i++)
                    {
                        worksheet.Cells[$"A{i + 3}"].Value = list[i].message_id;
                        worksheet.Cells[$"B{i + 3}"].Value = list[i].object_id;
                        worksheet.Cells[$"C{i + 3}"].Value = list[i].object_name;
                        worksheet.Cells[$"D{i + 3}"].Value = list[i].type_name;
                        worksheet.Cells[$"E{i + 3}"].Value = list[i].member_name;
                        worksheet.Cells[$"F{i + 3}"].Value = list[i].to_member_id;
                        worksheet.Cells[$"G{i + 3}"].Value = list[i].message_title;
                        worksheet.Cells[$"H{i + 3}"].Value = list[i].message_content;
                        worksheet.Cells[$"I{i + 3}"].Value = list[i].message_img;
                        worksheet.Cells[$"J{i + 3}"].Value = list[i].is_read ? "已读" : "未读";
                        worksheet.Cells[$"K{i + 3}"].Value = list[i].message_remark;
                        worksheet.Cells[$"L{i + 3}"].Value = list[i].format_time;
                    }
                    package.Save();
                }

                // 上传至阿里云OSS
                var newFileName = "MR.messages_" + DateTime.Now.ToString("yyyyMMddHHmmss") + MR.Utility.Helper.CommonHelper.GenerateRandomNum(6);//根据表名命名
                var returnPath = "/" + DateTime.Now.Year + "/" + DateTime.Now.ToString("MMdd") + "/" + newFileName + ".xlsx";
                using FileStream fi = new FileStream(Path.Combine(saveDir!, sFileName), FileMode.Open);
                OssClient client = new OssClient(MR.Utility.Config.AliyunOSS.Endpoint, MR.Utility.Config.AliyunOSS.AccessKeyId, MR.Utility.Config.AliyunOSS.AccessKeySecret);
                var key = "upload/" + DateTime.Now.Year + "/" + DateTime.Now.ToString("MMdd") + "/";
                using (MemoryStream memStream = new MemoryStream())
                {
                    // 创建目录
                    client.PutObject(MR.Utility.Config.AliyunOSS.BucketName, key, memStream);
                }
                ObjectMetadata metadata = new ObjectMetadata();
                metadata.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                client.PutObject(MR.Utility.Config.AliyunOSS.BucketName, key + newFileName + ".xlsx", fi, metadata);
                var oss_url = MR.Manage.Filters.AppSettingsFilter.GetSetting("Aliyun:OSS_URL");

                return Json(new { code = 0, msg = "导出成功", path = oss_url + "/upload" + returnPath });
            }
            catch (Exception ex)
            {
                log.Fatal(ex, " ->ExportMessages");
                return Json(new { code = 0, errcode =  (int)ENUMHelper.ExceptionType.Chart, errmsg = ex.Message });
            }
        }

        /// <summary>
        /// 导出访问量
        /// </summary>
        /// <param name="select_date">筛选日期区间</param>
        /// <param name="visit_path">被访问页面</param>
        /// <param name="member_name">会员昵称</param>
        /// <param name="client_ip">客户端IP</param>
        /// <param name="source_id">客户端来源</param>
        /// <param name="channel_id">频道</param>
        /// <returns></returns>
        public FileResult ExportVisit(string select_date, string visit_path, string member_name, string client_ip, int source_id = -1, int channel_id = -1)
        {
            try
            {
                var saveDir = MR.Manage.Filters.AppSettingsFilter.GetSetting("Upload:PATH");
                var tempPath = saveDir + "\\" + DateTime.Now.Year + "\\" + DateTime.Now.ToString("MMdd") + "\\";
                string sFileName = DateTime.Now.Year + "\\" + DateTime.Now.ToString("MMdd") + "\\" + MR.Utility.Helper.CommonHelper.CreateNo() + ".xlsx";
                if (!Directory.Exists(tempPath))
                {
                    Directory.CreateDirectory(tempPath);
                }
                FileInfo file = new FileInfo(Path.Combine(saveDir!, sFileName));
                using (ExcelPackage package = new ExcelPackage(file))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("导出数据");
                    worksheet.Cells[2, 1].Value = "ID";
                    worksheet.Cells[2, 2].Value = "来源网址";
                    worksheet.Cells[2, 3].Value = "搜索引擎";
                    worksheet.Cells[2, 4].Value = "客户端访问浏览器";
                    worksheet.Cells[2, 5].Value = "被访问页面";
                    worksheet.Cells[2, 6].Value = "客户端信息";
                    worksheet.Cells[2, 7].Value = "客户端Cookies";
                    worksheet.Cells[2, 8].Value = "设备操作系统";
                    worksheet.Cells[2, 9].Value = "搜客户端平台";
                    worksheet.Cells[2, 10].Value = "设备品牌";
                    worksheet.Cells[2, 11].Value = "设备型号";
                    worksheet.Cells[2, 12].Value = "设备语言";
                    worksheet.Cells[2, 13].Value = "系统版本号";
                    worksheet.Cells[2, 14].Value = "设备信息";
                    worksheet.Cells[2, 15].Value = "临时信息";
                    worksheet.Cells[2, 16].Value = "会员昵称";
                    worksheet.Cells[2, 17].Value = "所属公司";
                    worksheet.Cells[2, 18].Value = "用户唯一标识";
                    worksheet.Cells[2, 19].Value = "客户端IP";
                    worksheet.Cells[2, 20].Value = "客户端来源";
                    worksheet.Cells[2, 21].Value = "频道名称";
                    worksheet.Cells[2, 22].Value = "备注";
                    worksheet.Cells[2, 23].Value = "日期";

                    // 设置表头
                    worksheet.Cells[1, 1, 1, 23].Merge = true;
                    worksheet.Cells[1, 1].Value = "访问量";
                    // worksheet.Cells.Style.WrapText = true;
                    worksheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[1, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    worksheet.Row(1).Height = 30;
                    worksheet.Row(2).Height = 25;
                    // worksheet.Cells.Style.ShrinkToFit = true;// 单元格自动适应大小

                    var result = from item in _context.Set<Visits>()
                                 select new
                                 {
                                     item.visit_id,
                                     item.referrer_url,
                                     item.search_engine,
                                     item.client_browser,
                                     item.visit_path,
                                     item.client_info,
                                     item.client_cookies,
                                     item.device_system,
                                     device_platform = !string.IsNullOrEmpty(item.device_platform) ? item.device_platform : "-",
                                     item.device_brand,
                                     device_model = !string.IsNullOrEmpty(item.device_model) ? item.device_model : "-",
                                     item.device_language,
                                     item.device_version,
                                     item.device_info,
                                     item.temp_info,
                                     item.member_id,
                                     member_name = item.member_id > -1 ? _context.mr_members.FirstOrDefault(t => t.member_id == item.member_id)!.member_name : "-",
                                     item.company_id,
                                     item.client_id,
                                     item.client_ip,
                                     item.source_id,
                                     source_name = item.source_id > -1 ? _context.mr_utm_sources.FirstOrDefault(t => t.source_id == item.source_id)!.source_name : "-",
                                     item.channel_id,
                                     channel_name = item.channel_id > -1 ? _context.mr_visit_channels.FirstOrDefault(t => t.channel_id == item.channel_id)!.channel_name : "-",
                                     item.visit_remark,
                                     item.created_at,
                                     format_time = item.created_at.ToString("yyyy-MM-dd HH:mm:ss")
                                 };

                    if (!string.IsNullOrEmpty(visit_path))
                    {
                        result = result.Where(t => t.visit_path.Contains(visit_path.Trim()));
                    }
                    if (!string.IsNullOrEmpty(member_name))
                    {
                        result = result.Where(t => t.member_name.Contains(member_name.Trim()));
                    }
                    if (!string.IsNullOrEmpty(client_ip))
                    {
                        result = result.Where(t => t.client_ip.Contains(client_ip.Trim()));
                    }
                    if (source_id > 0)
                    {
                        result = result.Where(t => t.source_id == source_id);
                    }
                    if (channel_id > 0)
                    {
                        result = result.Where(t => t.channel_id == channel_id);
                    }

                    if (!string.IsNullOrEmpty(select_date))
                    {
                        select_date = select_date.Replace(" - ", ",");
                        string[] splitDate = select_date.Split(new char[] { ',' });
                        string start_at = splitDate[0];
                        string end_at = splitDate[1];
                        result = result.Where(t => t.created_at >= Convert.ToDateTime(start_at) && t.created_at <= Convert.ToDateTime(end_at).AddDays(1));
                    }

                    var list = result.OrderByDescending(t => t.created_at).ToList();

                    // 填充值
                    if (list.Count() == 0)
                    {
                        //return Json(new { code = 0, msg = "暂无数据", count = 0 });
                    }
                    for (int i = 0; i < list.Count; i++)
                    {
                        worksheet.Cells[$"A{i + 3}"].Value = list[i].visit_id;
                        worksheet.Cells[$"B{i + 3}"].Value = list[i].referrer_url;
                        worksheet.Cells[$"C{i + 3}"].Value = list[i].search_engine;
                        worksheet.Cells[$"D{i + 3}"].Value = list[i].client_browser;
                        worksheet.Cells[$"E{i + 3}"].Value = list[i].visit_path;
                        worksheet.Cells[$"F{i + 3}"].Value = list[i].client_info;
                        worksheet.Cells[$"G{i + 3}"].Value = list[i].client_cookies;
                        worksheet.Cells[$"H{i + 3}"].Value = list[i].device_system;
                        worksheet.Cells[$"I{i + 3}"].Value = list[i].device_platform;
                        worksheet.Cells[$"J{i + 3}"].Value = list[i].device_brand;
                        worksheet.Cells[$"K{i + 3}"].Value = list[i].device_model;
                        worksheet.Cells[$"L{i + 3}"].Value = list[i].device_language;
                        worksheet.Cells[$"M{i + 3}"].Value = list[i].device_version;
                        worksheet.Cells[$"N{i + 3}"].Value = list[i].device_info;
                        worksheet.Cells[$"O{i + 3}"].Value = list[i].temp_info;
                        worksheet.Cells[$"P{i + 3}"].Value = list[i].member_name;
                        worksheet.Cells[$"Q{i + 3}"].Value = list[i].company_id;
                        worksheet.Cells[$"R{i + 3}"].Value = list[i].client_id;
                        worksheet.Cells[$"S{i + 3}"].Value = list[i].client_ip;
                        worksheet.Cells[$"T{i + 3}"].Value = list[i].source_name;
                        worksheet.Cells[$"U{i + 3}"].Value = list[i].channel_name;
                        worksheet.Cells[$"V{i + 3}"].Value = list[i].visit_remark;
                        worksheet.Cells[$"W{i + 3}"].Value = list[i].format_time;
                    }
                    package.Save();
                }

                //// 上传至阿里云OSS
                //var newFileName = "MR.visit_" + DateTime.Now.ToString("yyyyMMddHHmmss") + MR.Utility.Helper.CommonHelper.GenerateRandomNum(6);//根据表名命名
                //var returnPath = "/" + DateTime.Now.Year + "/" + DateTime.Now.ToString("MMdd") + "/" + newFileName + ".xlsx";
                using FileStream fi = new FileStream(Path.Combine(saveDir!, sFileName), FileMode.Open);
                //OssClient client = new OssClient(MR.Utility.Config.AliyunOSS.Endpoint, MR.Utility.Config.AliyunOSS.AccessKeyId, MR.Utility.Config.AliyunOSS.AccessKeySecret);
                //var key = "upload/" + DateTime.Now.Year + "/" + DateTime.Now.ToString("MMdd") + "/";
                //using (MemoryStream memStream = new MemoryStream())
                //{
                //    // 创建目录
                //    client.PutObject(MR.Utility.Config.AliyunOSS.BucketName, key, memStream);
                //}
                //ObjectMetadata metadata = new ObjectMetadata();
                //metadata.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                //client.PutObject(MR.Utility.Config.AliyunOSS.BucketName, key + newFileName + ".xlsx", fi, metadata);
                //var oss_url = MR.Manage.Filters.AppSettingsFilter.GetSetting("Aliyun:OSS_URL");

                //return Json(new { code = 0, msg = "导出成功", path = oss_url + "/upload" + returnPath });

                return File(fi, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "UploadTemplate.xlsx");
            }
            catch (Exception ex)
            {
                log.Fatal(ex, " ->ExportVisit");
                return null;
                //return Json(new { code = 0, errcode =  (int)ENUMHelper.ExceptionType.Chart, errmsg = ex.Message });
            }
        }

        /// <summary>
        /// 导出广告
        /// </summary>
        /// <param name="ads_name">广告名称</param>
        /// <param name="is_show">是否显示</param>
        /// <param name="link_id">跳转类型</param>
        /// <param name="select_date">日期区间</param>
        /// <returns></returns>
        public IActionResult ExportAds(string ads_name, int link_id, string select_date, int is_show = -1)
        {
            try
            {
                var saveDir = MR.Manage.Filters.AppSettingsFilter.GetSetting("Upload:PATH");
                var tempPath = saveDir + "\\" + DateTime.Now.Year + "\\" + DateTime.Now.ToString("MMdd") + "\\";
                string sFileName = DateTime.Now.Year + "\\" + DateTime.Now.ToString("MMdd") + "\\" + MR.Utility.Helper.CommonHelper.CreateNo() + ".xlsx";
                if (!Directory.Exists(tempPath))
                {
                    Directory.CreateDirectory(tempPath);
                }
                FileInfo file = new FileInfo(Path.Combine(saveDir!, sFileName));
                using (ExcelPackage package = new ExcelPackage(file))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("导出数据");
                    worksheet.Cells[2, 1].Value = "ID";
                    worksheet.Cells[2, 2].Value = "多语言";
                    worksheet.Cells[2, 3].Value = "广告位置";
                    worksheet.Cells[2, 4].Value = "链接类型";
                    worksheet.Cells[2, 5].Value = "广告名称";
                    worksheet.Cells[2, 6].Value = "图片";
                    worksheet.Cells[2, 7].Value = "OSS链接";
                    worksheet.Cells[2, 8].Value = "发布人";
                    worksheet.Cells[2, 9].Value = "链接地址";
                    worksheet.Cells[2, 10].Value = "排序";
                    worksheet.Cells[2, 11].Value = "是否显示";
                    worksheet.Cells[2, 12].Value = "备注";
                    worksheet.Cells[2, 13].Value = "创建日期";

                    // 设置表头
                    worksheet.Cells[1, 1, 1, 13].Merge = true;
                    worksheet.Cells[1, 1].Value = "广告";
                    // worksheet.Cells.Style.WrapText = true;
                    worksheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[1, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    worksheet.Row(1).Height = 30;
                    worksheet.Row(2).Height = 25;
                    // worksheet.Cells.Style.ShrinkToFit = true;// 单元格自动适应大小

                    var result = from item in _context.Set<Ads>()
                                 select new
                                 {
                                     item.ads_id,
                                     item.lang_id,
                                     item.position_id,
                                     position_name = item.position_id > -1 ? _context.mr_ads_positions.FirstOrDefault(t => t.position_id == item.position_id)!.position_name : "-",
                                     item.link_id,
                                     link_name = item.link_id > -1 ? _context.mr_ads_links.FirstOrDefault(t => t.link_id == item.link_id)!.link_name : "-",
                                     item.ads_name,
                                     item.ads_image,
                                     show_ads_image = !string.IsNullOrEmpty(item.ads_image) ? MR.Manage.Filters.AppSettingsFilter.GetSetting("Aliyun:OSS_URL") + item.ads_image : "/images/no_photo.png",
                                     item.oss_url,
                                     item.user_id,
                                     user_name = item.user_id > -1 ? _context.mr_users.FirstOrDefault(t => t.user_id == item.user_id)!.user_name : "-",
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

                    if (!string.IsNullOrEmpty(select_date))
                    {
                        select_date = select_date.Replace(" - ", ",");
                        string[] splitDate = select_date.Split(new char[] { ',' });
                        string start_at = splitDate[0];
                        string end_at = splitDate[1];
                        result = result.Where(t => t.created_at >= Convert.ToDateTime(start_at) && t.created_at <= Convert.ToDateTime(end_at).AddDays(1));
                    }

                    var list = result.OrderByDescending(t => t.created_at).ToList();

                    // 填充值
                    if (list.Count() == 0)
                    {
                        return Json(new { code = 0, msg = "暂无数据", count = 0 });
                    }

                    for (int i = 0; i < list.Count; i++)
                    {
                        worksheet.Cells[$"A{i + 3}"].Value = list[i].ads_id;
                        worksheet.Cells[$"B{i + 3}"].Value = "中文";
                        worksheet.Cells[$"C{i + 3}"].Value = list[i].position_name;
                        worksheet.Cells[$"D{i + 3}"].Value = list[i].link_name;
                        worksheet.Cells[$"E{i + 3}"].Value = list[i].ads_name;
                        worksheet.Cells[$"F{i + 3}"].Value = list[i].show_ads_image;
                        worksheet.Cells[$"G{i + 3}"].Value = list[i].oss_url;
                        worksheet.Cells[$"H{i + 3}"].Value = list[i].user_name;
                        worksheet.Cells[$"I{i + 3}"].Value = list[i].page_url;
                        worksheet.Cells[$"J{i + 3}"].Value = list[i].sort_num;
                        worksheet.Cells[$"K{i + 3}"].Value = list[i].is_show ? "显示" : "隐藏";
                        worksheet.Cells[$"L{i + 3}"].Value = list[i].ads_remark;
                        worksheet.Cells[$"M{i + 3}"].Value = list[i].format_time;
                    }
                    package.Save();
                }

                // 上传至阿里云OSS
                var newFileName = "MR.ads_" + DateTime.Now.ToString("yyyyMMddHHmmss") + MR.Utility.Helper.CommonHelper.GenerateRandomNum(6);//根据表名命名
                var returnPath = "/" + DateTime.Now.Year + "/" + DateTime.Now.ToString("MMdd") + "/" + newFileName + ".xlsx";
                using FileStream fi = new FileStream(Path.Combine(saveDir!, sFileName), FileMode.Open);
                OssClient client = new OssClient(MR.Utility.Config.AliyunOSS.Endpoint, MR.Utility.Config.AliyunOSS.AccessKeyId, MR.Utility.Config.AliyunOSS.AccessKeySecret);
                var key = "upload/" + DateTime.Now.Year + "/" + DateTime.Now.ToString("MMdd") + "/";
                using (MemoryStream memStream = new MemoryStream())
                {
                    client.PutObject(MR.Utility.Config.AliyunOSS.BucketName, key, memStream);
                }
                ObjectMetadata metadata = new ObjectMetadata();
                metadata.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                client.PutObject(MR.Utility.Config.AliyunOSS.BucketName, key + newFileName + ".xlsx", fi, metadata);
                var oss_url = MR.Manage.Filters.AppSettingsFilter.GetSetting("Aliyun:OSS_URL");

                return Json(new { code = 0, msg = "导出成功", path = oss_url + "/upload" + returnPath });
            }
            catch (Exception ex)
            {
                log.Fatal(ex, " ->ExportAds");
                return Json(new { code = 0, errcode =  (int)ENUMHelper.ExceptionType.Chart, errmsg = ex.Message });
            }
        }

        /// <summary>
        /// 导出日志
        /// </summary>
        /// <param name="select_date">筛选日期区间</param>
        /// <param name="member_name">会员昵称</param>
        /// <param name="log_content">日志内容</param>
        /// <param name="client_ip">客户端IP</param>
        /// <param name="type_id">日志类型</param>
        /// <param name="platform_id">平台类型</param>
        /// <returns></returns>
        public IActionResult ExportLogs(string select_date, string member_name, string log_content, string client_ip, int type_id = -1, int platform_id = -1)
        {
            try
            {
                var saveDir = MR.Manage.Filters.AppSettingsFilter.GetSetting("Upload:PATH");
                var tempPath = saveDir + "\\" + DateTime.Now.Year + "\\" + DateTime.Now.ToString("MMdd") + "\\";
                string sFileName = DateTime.Now.Year + "\\" + DateTime.Now.ToString("MMdd") + "\\" + MR.Utility.Helper.CommonHelper.CreateNo() + ".xlsx";
                if (!Directory.Exists(tempPath))
                {
                    Directory.CreateDirectory(tempPath);
                }
                FileInfo file = new FileInfo(Path.Combine(saveDir!, sFileName));
                using (ExcelPackage package = new ExcelPackage(file))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("导出数据");
                    worksheet.Cells[2, 1].Value = "ID";
                    worksheet.Cells[2, 2].Value = "日志类型";
                    worksheet.Cells[2, 3].Value = "后台用户";
                    worksheet.Cells[2, 4].Value = "会员";
                    worksheet.Cells[2, 5].Value = "日志标识码";
                    worksheet.Cells[2, 6].Value = "平台类型";
                    worksheet.Cells[2, 7].Value = "日志内容";
                    worksheet.Cells[2, 8].Value = "客户端IP";
                    worksheet.Cells[2, 9].Value = "当前操作页面URL";
                    worksheet.Cells[2, 10].Value = "来源页面URL";
                    worksheet.Cells[2, 11].Value = "客户端信息";
                    worksheet.Cells[2, 12].Value = "Cookies";
                    worksheet.Cells[2, 13].Value = "备注";
                    worksheet.Cells[2, 14].Value = "创建日期";

                    // 设置表头
                    worksheet.Cells[1, 1, 1, 14].Merge = true;
                    worksheet.Cells[1, 1].Value = "日志";
                    // worksheet.Cells.Style.WrapText = true;
                    worksheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[1, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    worksheet.Row(1).Height = 30;
                    worksheet.Row(2).Height = 25;
                    // worksheet.Cells.Style.ShrinkToFit = true;// 单元格自动适应大小

                    var result = from item in _context.Set<Logs>()
                                 select new
                                 {
                                     item.log_id,
                                     item.type_id,
                                     type_name = item.type_id > -1 ? _context.mr_log_types.FirstOrDefault(t => t.type_id == item.type_id)!.type_name : "-",
                                     item.user_id,
                                     item.member_id,
                                     member_name = item.user_id > -1 ? _context.mr_users.FirstOrDefault(t => t.user_id == item.user_id)!.user_name : _context.mr_members.FirstOrDefault(t => t.member_id == item.user_id)!.member_name,
                                     item.log_code,
                                     item.platform_id,
                                     platform_name = item.platform_id > -1 ? _context.mr_platforms.FirstOrDefault(t => t.platform_id == item.platform_id)!.platform_name : "-",
                                     log_content = item.log_content!.Length > 100 ? item.log_content.Substring(0, 100) : item.log_content,
                                     item.client_ip,
                                     item.page_url,
                                     item.referrer_url,
                                     item.user_agent,
                                     item.user_cookies,
                                     item.log_remark,
                                     item.created_at,
                                     format_time = item.created_at.ToString("yyyy-MM-dd HH:mm")
                                 };

                    if (type_id > 0)
                    {
                        result = result.Where(t => t.type_id == type_id);
                    }
                    if (!string.IsNullOrEmpty(member_name))
                    {
                        result = result.Where(t => t.member_name.Contains(member_name.Trim()));
                    }
                    if (!string.IsNullOrEmpty(log_content))
                    {
                        result = result.Where(t => t.log_content.Contains(log_content.Trim()));
                    }
                    if (platform_id > 0)
                    {
                        result = result.Where(t => t.platform_id == platform_id);
                    }
                    if (!string.IsNullOrEmpty(client_ip))
                    {
                        result = result.Where(t => t.client_ip.Contains(client_ip.Trim()));
                    }

                    if (!string.IsNullOrEmpty(select_date))
                    {
                        select_date = select_date.Replace(" - ", ",");
                        string[] splitDate = select_date.Split(new char[] { ',' });
                        string start_at = splitDate[0];
                        string end_at = splitDate[1];
                        result = result.Where(t => t.created_at >= Convert.ToDateTime(start_at) && t.created_at <= Convert.ToDateTime(end_at).AddDays(1));
                    }

                    var list = result.OrderByDescending(t => t.created_at).ToList();

                    // 填充值
                    if (list.Count() == 0)
                    {
                        return Json(new { code = 0, msg = "暂无数据", count = 0 });
                    }
                    for (int i = 0; i < list.Count; i++)
                    {
                        worksheet.Cells[$"A{i + 3}"].Value = list[i].log_id;
                        worksheet.Cells[$"B{i + 3}"].Value = list[i].type_name;
                        worksheet.Cells[$"C{i + 3}"].Value = list[i].member_name;
                        worksheet.Cells[$"D{i + 3}"].Value = list[i].member_name;
                        worksheet.Cells[$"E{i + 3}"].Value = list[i].log_code;
                        worksheet.Cells[$"F{i + 3}"].Value = list[i].platform_name;
                        worksheet.Cells[$"G{i + 3}"].Value = list[i].log_content;
                        worksheet.Cells[$"H{i + 3}"].Value = list[i].client_ip;
                        worksheet.Cells[$"I{i + 3}"].Value = list[i].page_url;
                        worksheet.Cells[$"J{i + 3}"].Value = list[i].referrer_url;
                        worksheet.Cells[$"K{i + 3}"].Value = list[i].user_agent;
                        worksheet.Cells[$"L{i + 3}"].Value = list[i].user_cookies;
                        worksheet.Cells[$"M{i + 3}"].Value = list[i].log_remark;
                        worksheet.Cells[$"N{i + 3}"].Value = list[i].format_time;
                    }
                    package.Save();
                }

                // 上传至阿里云OSS
                var newFileName = "MR.logs_" + DateTime.Now.ToString("yyyyMMddHHmmss") + MR.Utility.Helper.CommonHelper.GenerateRandomNum(6);//根据表名命名
                var returnPath = "/" + DateTime.Now.Year + "/" + DateTime.Now.ToString("MMdd") + "/" + newFileName + ".xlsx";
                using FileStream fi = new FileStream(Path.Combine(saveDir!, sFileName), FileMode.Open);
                OssClient client = new OssClient(MR.Utility.Config.AliyunOSS.Endpoint, MR.Utility.Config.AliyunOSS.AccessKeyId, MR.Utility.Config.AliyunOSS.AccessKeySecret);
                var key = "upload/" + DateTime.Now.Year + "/" + DateTime.Now.ToString("MMdd") + "/";
                using (MemoryStream memStream = new MemoryStream())
                {
                    // 创建目录
                    client.PutObject(MR.Utility.Config.AliyunOSS.BucketName, key, memStream);
                }
                ObjectMetadata metadata = new ObjectMetadata();
                metadata.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                client.PutObject(MR.Utility.Config.AliyunOSS.BucketName, key + newFileName + ".xlsx", fi, metadata);
                var oss_url = MR.Manage.Filters.AppSettingsFilter.GetSetting("Aliyun:OSS_URL");

                return Json(new { code = 0, msg = "导出成功", path = oss_url + "/upload" + returnPath });
            }
            catch (Exception ex)
            {
                log.Fatal(ex, " ->ExportLogs");
                return Json(new { code = 0, errcode =  (int)ENUMHelper.ExceptionType.Chart, errmsg = ex.Message });
            }
        }

        /// <summary>
        /// 导出文章
        /// </summary>
        /// <param name="select_date">筛选日期区间</param>
        /// <param name="article_title">标题</param>
        /// <param name="article_content">内容</param>
        /// <param name="article_author">作者</param>
        /// <param name="type_id">分类</param>
        /// <param name="status_id">状态</param>
        /// <param name="review_status_id">审核状态</param>
        /// <returns></returns>
        public IActionResult ExportArticles(string select_date, string article_title, string article_content, string article_author, int type_id = -1, int status_id = -1, int review_status_id = -1)
        {
            try
            {
                var saveDir = MR.Manage.Filters.AppSettingsFilter.GetSetting("Upload:PATH");
                var tempPath = saveDir + "\\" + DateTime.Now.Year + "\\" + DateTime.Now.ToString("MMdd") + "\\";
                string sFileName = DateTime.Now.Year + "\\" + DateTime.Now.ToString("MMdd") + "\\" + MR.Utility.Helper.CommonHelper.CreateNo() + ".xlsx";
                if (!Directory.Exists(tempPath))
                {
                    Directory.CreateDirectory(tempPath);
                }
                FileInfo file = new FileInfo(Path.Combine(saveDir!, sFileName));
                using (ExcelPackage package = new ExcelPackage(file))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("导出数据");
                    worksheet.Cells[2, 1].Value = "ID";
                    worksheet.Cells[2, 2].Value = "多语言";
                    worksheet.Cells[2, 3].Value = "标题";
                    worksheet.Cells[2, 4].Value = "封面图";
                    worksheet.Cells[2, 5].Value = "内容";
                    worksheet.Cells[2, 6].Value = "作者";
                    worksheet.Cells[2, 7].Value = "文章来源";
                    worksheet.Cells[2, 8].Value = "摘要";
                    worksheet.Cells[2, 9].Value = "分类";
                    worksheet.Cells[2, 10].Value = "子分类";
                    worksheet.Cells[2, 11].Value = "状态";
                    worksheet.Cells[2, 12].Value = "审核状态";
                    worksheet.Cells[2, 13].Value = "是否置顶";
                    worksheet.Cells[2, 14].Value = "是否显示";
                    worksheet.Cells[2, 15].Value = "发布人";
                    worksheet.Cells[2, 16].Value = "访问量";
                    worksheet.Cells[2, 17].Value = "定时发布时间";
                    worksheet.Cells[2, 18].Value = "备注";
                    worksheet.Cells[2, 19].Value = "发布日期";

                    // 设置表头
                    worksheet.Cells[1, 1, 1, 19].Merge = true;
                    worksheet.Cells[1, 1].Value = "文章";
                    // worksheet.Cells.Style.WrapText = true;
                    worksheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[1, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    worksheet.Row(1).Height = 30;
                    worksheet.Row(2).Height = 25;
                    // worksheet.Cells.Style.ShrinkToFit = true;// 单元格自动适应大小

                    var result = from item in _context.Set<Articles>()
                                 select new
                                 {
                                     item.article_id,
                                     item.lang_id,
                                     lang_name = _context.mr_languages.FirstOrDefault(t => t.lang_id == item.lang_id)!.lang_name,
                                     item.article_title,
                                     item.cover_img,
                                     show_cover_img = string.IsNullOrEmpty(item.cover_img) ? "" : MR.Manage.Filters.AppSettingsFilter.GetSetting("Aliyun:OSS_URL") + item.cover_img,
                                     item.article_content,
                                     item.article_author,
                                     item.article_source,
                                     item.article_summary,
                                     item.type_id,
                                     item.type_subid,
                                     type_name = item.type_id > 0 ? _context.mr_article_types.FirstOrDefault(t => t.type_id == item.type_id)!.type_name : "-",
                                     type_subname = item.type_subid > 0 ? _context.mr_article_types.FirstOrDefault(t => t.type_id == item.type_subid)!.type_name : "-",
                                     item.status_id,
                                     status_name = _context.mr_article_status.FirstOrDefault(t => t.status_id == item.status_id)!.status_name,
                                     item.review_status_id,
                                     review_status_name = _context.mr_review_status.FirstOrDefault(t => t.status_id == item.review_status_id)!.status_name,
                                     item.is_top,
                                     item.is_show,
                                     item.user_id,
                                     user_name = _context.mr_users.FirstOrDefault(t => t.user_id == item.user_id)!.user_name,
                                     item.visit_num,
                                     item.article_remark,
                                     item.set_publish_time,
                                     item.created_at,
                                     format_time = item.created_at.ToString("yyyy-MM-dd HH:mm"),
                                 };

                    if (!string.IsNullOrEmpty(article_title))
                    {
                        result = result.Where(t => t.article_title.Contains(article_title.Trim()));
                    }
                    if (!string.IsNullOrEmpty(article_content))
                    {
                        result = result.Where(t => t.article_content.Contains(article_content.Trim()));
                    }
                    if (!string.IsNullOrEmpty(article_author))
                    {
                        result = result.Where(t => t.article_author.Contains(article_author.Trim()));
                    }
                    if (type_id > 0)
                    {
                        result = result.Where(t => t.type_id == type_id);
                    }
                    if (status_id > 0)
                    {
                        result = result.Where(t => t.status_id == status_id);
                    }
                    if (review_status_id > 0)
                    {
                        result = result.Where(t => t.review_status_id == review_status_id);
                    }
                    else
                    {
                        result = result.Where(t => t.review_status_id != 2);
                    }

                    if (!string.IsNullOrEmpty(select_date))
                    {
                        select_date = select_date.Replace(" - ", ",");
                        string[] splitDate = select_date.Split(new char[] { ',' });
                        string start_at = splitDate[0];
                        string end_at = splitDate[1];
                        result = result.Where(t => t.created_at >= Convert.ToDateTime(start_at) && t.created_at <= Convert.ToDateTime(end_at).AddDays(1));
                    }

                    var list = result.OrderByDescending(t => t.created_at).ToList();

                    // 填充值
                    if (list.Count() == 0)
                    {
                        return Json(new { code = 0, msg = "暂无数据", count = 0 });
                    }
                    for (int i = 0; i < list.Count; i++)
                    {
                        worksheet.Cells[$"A{i + 3}"].Value = list[i].article_id;
                        worksheet.Cells[$"B{i + 3}"].Value = list[i].lang_id;
                        worksheet.Cells[$"C{i + 3}"].Value = list[i].article_title;
                        worksheet.Cells[$"D{i + 3}"].Value = list[i].cover_img;
                        worksheet.Cells[$"E{i + 3}"].Value = list[i].article_content;
                        worksheet.Cells[$"F{i + 3}"].Value = list[i].article_author;
                        worksheet.Cells[$"G{i + 3}"].Value = list[i].article_source;
                        worksheet.Cells[$"H{i + 3}"].Value = list[i].article_summary;
                        worksheet.Cells[$"I{i + 3}"].Value = list[i].type_name;
                        worksheet.Cells[$"J{i + 3}"].Value = list[i].type_subname;
                        worksheet.Cells[$"K{i + 3}"].Value = list[i].status_name;
                        worksheet.Cells[$"L{i + 3}"].Value = list[i].review_status_name;
                        worksheet.Cells[$"M{i + 3}"].Value = list[i].is_top ? "是" : "否";
                        worksheet.Cells[$"N{i + 3}"].Value = list[i].is_show ? "显示" : "隐藏";
                        worksheet.Cells[$"O{i + 3}"].Value = list[i].user_name;
                        worksheet.Cells[$"P{i + 3}"].Value = list[i].visit_num;
                        worksheet.Cells[$"Q{i + 3}"].Value = list[i].set_publish_time;
                        worksheet.Cells[$"R{i + 3}"].Value = list[i].article_remark;
                        worksheet.Cells[$"S{i + 3}"].Value = list[i].format_time;
                    }
                    package.Save();
                }

                // 上传至阿里云OSS
                var newFileName = "MR.articles_" + DateTime.Now.ToString("yyyyMMddHHmmss") + MR.Utility.Helper.CommonHelper.GenerateRandomNum(6);//根据表名命名
                var returnPath = "/" + DateTime.Now.Year + "/" + DateTime.Now.ToString("MMdd") + "/" + newFileName + ".xlsx";
                using FileStream fi = new FileStream(Path.Combine(saveDir!, sFileName), FileMode.Open);
                OssClient client = new OssClient(MR.Utility.Config.AliyunOSS.Endpoint, MR.Utility.Config.AliyunOSS.AccessKeyId, MR.Utility.Config.AliyunOSS.AccessKeySecret);
                var key = "upload/" + DateTime.Now.Year + "/" + DateTime.Now.ToString("MMdd") + "/";
                using (MemoryStream memStream = new MemoryStream())
                {
                    // 创建目录
                    client.PutObject(MR.Utility.Config.AliyunOSS.BucketName, key, memStream);
                }
                ObjectMetadata metadata = new ObjectMetadata();
                metadata.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                client.PutObject(MR.Utility.Config.AliyunOSS.BucketName, key + newFileName + ".xlsx", fi, metadata);
                var oss_url = MR.Manage.Filters.AppSettingsFilter.GetSetting("Aliyun:OSS_URL");

                return Json(new { code = 0, msg = "导出成功", path = oss_url + "/upload" + returnPath });
            }
            catch (Exception ex)
            {
                log.Fatal(ex, " ->ExportArticles");
                return Json(new { code = 0, errcode =  (int)ENUMHelper.ExceptionType.Chart, errmsg = ex.Message });
            }
        }

        #endregion
    }
}
