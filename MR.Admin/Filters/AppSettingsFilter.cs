using System;
namespace MR.Manage.Filters
{
	public class AppSettingsFilter
	{
		public AppSettingsFilter()
		{
		}

        /// <summary>
        /// 读取配置节点数据
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static string GetSetting(string node)
		{
#pragma warning disable CS8602 // 解引用可能出现空引用。
            var value = MR.Utility.Config.CfgManager.Configuration[node];
#pragma warning restore CS8602 // 解引用可能出现空引用。
            return string.IsNullOrEmpty(value) ? "" : value;
        }
	}
}

