using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MR.Utility.CustomExceptions;

namespace MR.Manage.Filters
{
    /// <summary>
    /// 全局异常错误日志
    /// </summary>
    public class GlobalExceptionFilter : IExceptionFilter {
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<GlobalExceptionFilter> _logger;

        /// <summary>
        /// GlobalExceptionFilter
        /// </summary>
        /// <param name="env"></param>
        /// <param name="logger"></param>
        public GlobalExceptionFilter(IWebHostEnvironment env, ILogger<GlobalExceptionFilter> logger) {
            _env = env;
            _logger = logger;
        }

        /// <summary>
        /// OnException
        /// </summary>
        /// <param name="context"></param>
        public void OnException(ExceptionContext context)
        {
            var result = new ContentResult { ContentType = "text/json;charset=utf-8;" };

            if (context.Exception.GetType() == typeof(BusinessException)) {
                context.Result = new JsonResult(new { context.Exception.Message, status = false });
            }
            else
            {
                result.StatusCode = StatusCodes.Status500InternalServerError;
                result.Content = _env.IsDevelopment() ? context.Exception.Message : "服务器发生了意外的内部错误";
                context.Result = result;
            }
            // 记录到日志
            var detailMessage = WriteLog(context.Exception);
            _logger.LogError(detailMessage);
            // 异常已处理
            context.ExceptionHandled = true;
        }

        /// <summary>
        /// 自定义返回格式
        /// </summary>
        /// <param name="ex">异常信息</param>
        /// <returns></returns>
        private string WriteLog(Exception ex)
        {
            var content = "【异常信息】：" + ex.Message + " \r\n【异常类型】：" + ex.GetType().Name + "\r\n【堆栈调用】：" + ex.StackTrace;
            return content;
        }
    }
}
