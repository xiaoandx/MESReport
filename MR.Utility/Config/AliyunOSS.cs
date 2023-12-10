using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MR.Utility.Config
{
    /// <summary>
    /// 配置文件
    /// </summary>
    public class AliyunOSS
    {
        /// <summary>
        /// BucketName（此处需要修改成你自己的内容）
        /// </summary>
        public static string BucketName = "xxxxxx";
        /// <summary>
        /// AccessKeyId（此处需要修改成你自己的内容）
        /// </summary>
        public static string AccessKeyId = "xxxxxx";
        /// <summary>
        /// AccessKeySecret（此处需要修改成你自己的内容）
        /// </summary>
        public static string AccessKeySecret = "xxxxxx";
        /// <summary>
        /// Endpoint（此处需要修改成你自己的内容）
        /// </summary>
        public static string Endpoint = "oss-cn-shanghai.aliyuncs.com";
        /// <summary>
        /// DirToDownload
        /// </summary>
        public static string DirToDownload = "";
        /// <summary>
        /// FileToUpload
        /// </summary>
        public static string FileToUpload = "";
        /// <summary>
        /// BigFileToUpload
        /// </summary>
        public static string BigFileToUpload = "d:\\test100m.zip";
        /// <summary>
        /// ImageFileToUpload
        /// </summary>
        public static string ImageFileToUpload = "<your local image file to upload>";
        /// <summary>
        /// CallbackServer（此处需要修改成你自己的内容）
        /// </summary>
        public static string CallbackServer = "https://xxxxxx.oss-cn-shanghai.aliyuncs.com";
    }
}
