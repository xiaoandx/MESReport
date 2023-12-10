using FluentScheduler;
using Microsoft.Extensions.DependencyInjection;

namespace MR.Manage.Extensions
{
    /// <summary>
    /// 定义任务调度
    /// </summary>
    public static class FluentSchedulerSetup
    {
        /// <summary>
        /// 任务调度服务注册
        /// </summary>
        /// <param name="services"></param>
        public static void AddFluentSchedulerSetup(this IServiceCollection services)
        {
            // 注册同步服务
            JobManager.Initialize(new TaskHelper());
        }
    }
}
