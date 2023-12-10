using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MR.Manage.Data;
using Aliyun.OSS;

namespace MR.Manage.Controllers
{
    /// <summary>
    /// 内容摘要: 上传处理类
    /// </summary>
    public class UploadController : BaseController<UploadController>
    {
        public UploadController(MRManageContext context) : base(context)
        {

        }

        /// <summary>
        /// 上传图片
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadImg(string catalogue, bool open_breviary)
        {
            var files = Request.Form.Files;
            var formFile = files[0];

            var fileName = formFile.FileName;
            var saveDir = MR.Manage.Filters.AppSettingsFilter.GetSetting("Upload:PATH");
            var tempPath = saveDir + "\\" + DateTime.Now.Year + "\\" + DateTime.Now.ToString("MMdd") + "\\";
            if (!Directory.Exists(tempPath))
            {
                Directory.CreateDirectory(tempPath);
            }

            var extension = Path.GetExtension(fileName);
            var newFileName = DateTime.Now.ToString("yyyyMMddHHmmss") + MR.Utility.Helper.CommonHelper.GenerateRandomNum(6);//重命名

            var savePath = tempPath + newFileName + extension;
            var returnPath = "/" + DateTime.Now.Year + "/" + DateTime.Now.ToString("MMdd") + "/" + newFileName + extension;

            try
            {
                if (!Directory.Exists(tempPath))
                {
                    Directory.CreateDirectory(tempPath);
                }
                if (System.IO.File.Exists(savePath))
                {
                    System.IO.File.Delete(savePath);
                }
                using (FileStream fs = System.IO.File.Create(savePath))
                {
                    await formFile.CopyToAsync(fs);
                    fs.Flush();
                    fs.Close();
                }

                using FileStream fi = new FileStream(savePath, FileMode.Open);

                // 上传至OSS
                try
                {
                    OssClient client = new OssClient(MR.Utility.Config.AliyunOSS.Endpoint, MR.Utility.Config.AliyunOSS.AccessKeyId, MR.Utility.Config.AliyunOSS.AccessKeySecret);
                    var key = "upload/" + DateTime.Now.Year + "/" + DateTime.Now.ToString("MMdd") + "/";
                    using (MemoryStream memStream = new MemoryStream())
                    {
                        // 创建目录
                        client.PutObject(MR.Utility.Config.AliyunOSS.BucketName, key, memStream);
                    }

                    ObjectMetadata metadata = new ObjectMetadata();
                    metadata.ContentType = Request.Form.Files[0].ContentType;
                    // 上传文件
                    client.PutObject(MR.Utility.Config.AliyunOSS.BucketName, key + newFileName + extension, fi, metadata);
                }
                catch (Exception ex)
                {
                    return Json(new { code = 0, errcode = 40014, errmsg = ex.Message });
                }

                var oss_url = MR.Manage.Filters.AppSettingsFilter.GetSetting("Aliyun:OSS_URL");
                return Json(new { code = 0, msg = "上传成功", path = "/upload" + returnPath, data = new { src = oss_url + "upload" + returnPath, title = "" } });
            }
            catch (Exception ex)
            {
                return Json(new { code = 0, errcode = 40014, errmsg = ex.Message });
            }
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadFile()
        {
            var files = Request.Form.Files;
            var formFile = files[0];

            var fileName = formFile.FileName;
            var saveDir = MR.Manage.Filters.AppSettingsFilter.GetSetting("Upload:PATH");
            var tempPath = saveDir + "\\" + DateTime.Now.Year + "\\" + DateTime.Now.ToString("MMdd") + "\\";
            if (!Directory.Exists(tempPath))
            {
                Directory.CreateDirectory(tempPath);
            }

            var extension = Path.GetExtension(fileName);
            var newFileName = DateTime.Now.ToString("yyyyMMddHHmmss") + MR.Utility.Helper.CommonHelper.GenerateRandomNum(6);//重命名

            var savePath = tempPath + newFileName + extension;
            var returnPath = "/" + DateTime.Now.Year + "/" + DateTime.Now.ToString("MMdd") + "/" + newFileName + extension;

            try
            {
                if (!Directory.Exists(tempPath))
                {
                    Directory.CreateDirectory(tempPath);
                }
                if (System.IO.File.Exists(savePath))
                {
                    System.IO.File.Delete(savePath);
                }
                using (FileStream fs = System.IO.File.Create(savePath))
                {
                    await formFile.CopyToAsync(fs);
                    fs.Flush();
                    fs.Close();
                }

                using FileStream fi = new FileStream(savePath, FileMode.Open);

                // 上传至OSS
                try
                {
                    OssClient client = new OssClient(MR.Utility.Config.AliyunOSS.Endpoint, MR.Utility.Config.AliyunOSS.AccessKeyId, MR.Utility.Config.AliyunOSS.AccessKeySecret);
                    var key = "upload/" + DateTime.Now.Year + "/" + DateTime.Now.ToString("MMdd") + "/";
                    using (MemoryStream memStream = new MemoryStream())
                    {
                        // 创建目录
                        client.PutObject(MR.Utility.Config.AliyunOSS.BucketName, key, memStream);
                    }

                    ObjectMetadata metadata = new ObjectMetadata();
                    metadata.ContentType = Request.Form.Files[0].ContentType;
                    // 上传文件
                    client.PutObject(MR.Utility.Config.AliyunOSS.BucketName, key + newFileName + extension, fi, metadata);
                }
                catch (Exception ex)
                {
                    return Json(new { code = 0, errcode = 40014, errmsg = ex.Message });
                }

                var oss_url = MR.Manage.Filters.AppSettingsFilter.GetSetting("Aliyun:OSS_URL");
                return Json(new { code = 0, msg = "上传成功", path = "/upload" + returnPath, returnPath, name = newFileName, old_name = fileName, local_path = savePath, data = new { src = oss_url + "/upload" + returnPath, title = "" } });
            }
            catch (Exception ex)
            {
                return Json(new { code = 0, errcode = 40014, errmsg = ex.Message });
            }
        }

        /// <summary>
        /// 后台富文本编辑器上传图片
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Editor()
        {
            var files = Request.Form.Files;
            var formFile = files[0];

            var fileName = formFile.FileName;
            var saveDir = MR.Manage.Filters.AppSettingsFilter.GetSetting("Upload:PATH");
            var tempPath = saveDir + "\\" + DateTime.Now.Year + "\\" + DateTime.Now.ToString("MMdd") + "\\";
            if (!Directory.Exists(tempPath))
            {
                Directory.CreateDirectory(tempPath);
            }

            var extension = Path.GetExtension(fileName);
            var newFileName = DateTime.Now.ToString("yyyyMMddHHmmss") + MR.Utility.Helper.CommonHelper.GenerateRandomNum(6) + extension;//重命名

            var savePath = tempPath + newFileName;
            var returnPath = "/" + DateTime.Now.Year + "/" + DateTime.Now.ToString("MMdd") + "/" + newFileName;

            try
            {
                if (!Directory.Exists(tempPath))
                {
                    Directory.CreateDirectory(tempPath);
                }
                if (System.IO.File.Exists(savePath))
                {
                    System.IO.File.Delete(savePath);
                }
                using (FileStream fs = System.IO.File.Create(savePath))
                {
                    await formFile.CopyToAsync(fs);
                    fs.Flush();
                }

                // 上传至OSS
                try
                {
                    using FileStream fi = new FileStream(savePath, FileMode.Open);
                    OssClient client = new OssClient(MR.Utility.Config.AliyunOSS.Endpoint, MR.Utility.Config.AliyunOSS.AccessKeyId, MR.Utility.Config.AliyunOSS.AccessKeySecret);
                    var key = "upload/" + DateTime.Now.Year + "/" + DateTime.Now.ToString("MMdd") + "/";
                    using (MemoryStream memStream = new MemoryStream())
                    {
                        // 创建目录
                        client.PutObject(MR.Utility.Config.AliyunOSS.BucketName, key, memStream);
                    }

                    ObjectMetadata metadata = new ObjectMetadata();
                    metadata.ContentType = Request.Form.Files[0].ContentType;
                    // 上传文件
                    client.PutObject(MR.Utility.Config.AliyunOSS.BucketName, key + newFileName + extension, fi, metadata);
                }
                catch (Exception ex)
                {
                    return Json(new { code = 0, errcode = 40014, errmsg = ex.Message });
                }

                var oss_url = MR.Manage.Filters.AppSettingsFilter.GetSetting("Aliyun:OSS_URL");

                return Json(new { code = 0, msg = "上传成功", data = new { src = oss_url + returnPath, title = "" } });
            }
            catch (Exception ex)
            {
                return Json(new { code = 0, errcode = 40014, errmsg = ex.Message });
            }
        }
    }
}
