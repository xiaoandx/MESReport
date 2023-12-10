namespace MR.Manage.Extensions
{
    using FluentScheduler;
    using MR.Manage.Data;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Serilog;
    using Serilog.Core;
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using MR.Utility.Config;
    using MR.Manage.Filters;

    /// <summary>
    /// TaskHelper
    /// </summary>
    public class TaskHelper: Registry
    {
        /// <summary>
        /// Log变量
        /// </summary>
        public Logger log;

        public TaskHelper()
        {
            //初始化日志
            log = new LoggerConfiguration()
             .ReadFrom.Configuration(new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
             .Build())
             .CreateLogger();

            DoTask();
        }

        /// <summary>
        /// 执行任务
        /// </summary>
        private void DoTask()
        {
            NonReentrantAsDefault();
            Schedule(() =>
            {
                var task = Task.Run(async () =>
                {
                    await ClearLogs();
                });

            }).WithName("ClearLogs").ToRunNow().AndEvery(1).Days(); // 每个天执行一次
        }

        /// <summary>
        /// 只保留三个月内日志数据
        /// </summary>
        /// <returns></returns>
        private async Task ClearLogs()
        {
            log.Information("--------------------------任务开始1--------------------------");

            // 数据库连接对象
            var connection = AppSettingsFilter.GetSetting("ConnectionStrings:PRO_LOCAL");
            var server_version = ServerVersion.AutoDetect(connection);
            var dbContextOption = new DbContextOptions<MRManageContext>();
            var dbContextOptionBuilder = new DbContextOptionsBuilder<MRManageContext>(dbContextOption);
            var _context = new MRManageContext(dbContextOptionBuilder.UseMySql(server_version).Options);

            try
            {
                var logs = await _context.mr_logs.Where(t => DateTime.Now.Year == t.created_at.Year && DateTime.Now.Month - t.created_at.Month > 3).ToListAsync();
                _context.mr_logs.RemoveRange(logs);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                log.Fatal(ex, " -> 执行具体任务");
            }
        }
    }
}
