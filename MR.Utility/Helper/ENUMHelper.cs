using System;
namespace MR.Utility.Helper
{
    /// <summary>
    /// 枚举类型类
    /// </summary>
	public class ENUMHelper
	{
		public ENUMHelper()
		{

        }

        /// <summary>
        /// 平台类型
        /// </summary>
        public enum Platform
        {
            /// <summary>
            /// PC官网
            /// </summary>
            PC = 1,
            /// <summary>
            /// 苹果版
            /// </summary>
            iPhone =2,
            /// <summary>
            /// 安卓版
            /// </summary>
            Android = 3,
            /// <summary>
            /// 移动端
            /// </summary>
            H5 = 4,
            /// <summary>
            /// 微信小程序
            /// </summary>
            WeChat = 5,
            /// <summary>
            /// 后台管理系统
            /// </summary>
            Admin = 6,
            /// <summary>
            /// 其它
            /// </summary>
            Others = 7
        }

        /// <summary>
        /// 操作类型
        /// </summary>
        public enum OperateType
        {
            /// <summary>
            /// 登入
            /// </summary>
            Login = 10,
            /// <summary>
            /// 登出
            /// </summary>
            Logout = 11,
            /// <summary>
            /// 读取列表
            /// </summary>
            List = 20,
            /// <summary>
            /// 下拉数据
            /// </summary>
            Select = 21,
            /// <summary>
            /// 获取详情
            /// </summary>
            Details = 22,
            /// <summary>
            /// 检测数据
            /// </summary>
            Check = 23,
            /// <summary>
            /// 统计数据
            /// </summary>
            Chart = 30,
            /// <summary>
            /// 创建操作
            /// </summary>
            Create = 40,
            /// <summary>
            /// 编辑操作
            /// </summary>
            Edit = 50,
            /// <summary>
            /// 更新操作
            /// </summary>
            Update = 51,
            /// <summary>
            /// 设置操作
            /// </summary>
            Set = 52,
            /// <summary>
            /// 删除操作
            /// </summary>
            Delete = 60
        }

        /// <summary>
        /// 错误类型
        /// </summary>
        public enum ErrorType
        {
            PasswordError = 4001,
            VerifyCodeError = 4002,
            ExceptionError = 4003
        }

        /// <summary>
        /// 异常类型
        /// </summary>
        public enum ExceptionType
        {
            /// <summary>
            /// 列表
            /// </summary>
            List = 40001,
            /// <summary>
            /// 下拉
            /// </summary>
            Select = 40002,
            /// <summary>
            /// 详情
            /// </summary>
            Details = 40003,
            /// <summary>
            /// 检测数据
            /// </summary>
            Check = 40004,
            /// <summary>
            /// 统计数据
            /// </summary>
            Chart = 40005,
            /// <summary>
            /// 创建操作
            /// </summary>
            Create = 40006,
            /// <summary>
            /// 编辑操作
            /// </summary>
            Edit = 40007,
            /// <summary>
            /// 更新操作
            /// </summary>
            Update = 40008,
            /// <summary>
            /// 设置操作
            /// </summary>
            Set = 40009,
            /// <summary>
            /// 删除操作
            /// </summary>
            Delete = 40010,
            /// <summary>
            /// 登陆操作
            /// </summary>
            Login = 40011
        }

        /// <summary>
        /// 信息类型
        /// </summary>
        public enum InfoType
        {
            Info = 1000,
            LoginSuccess = 1001
        }

        /// <summary>
        /// 日志类型
        /// </summary>
        public enum LogType
        {
            /// <summary>
            /// 后台管理员登录
            /// </summary>
            Login = 1,
            /// <summary>
            /// 会员登录
            /// </summary>
            MemberLogin = 2,
            /// <summary>
            /// 会员注册
            /// </summary>
            MemberRegister = 3,
            /// <summary>
            /// 新增
            /// </summary>
            Create = 4,
            /// <summary>
            /// 删除
            /// </summary>
            Delete = 5,
            /// <summary>
            /// 编辑
            /// </summary>
            Update = 6,
            /// <summary>
            /// 查询
            /// </summary>
            Search = 7,
            /// <summary>
            /// 授权
            /// </summary>
            Authorize = 8,
            /// <summary>
            /// 统计
            /// </summary>
            Count = 9,
            /// <summary>
            /// 导入
            /// </summary>
            Import = 10,
            /// <summary>
            /// 导出
            /// </summary>
            Export = 11,
            /// <summary>
            /// 推送消息
            /// </summary>
            Push = 12,
            /// <summary>
            /// 回调
            /// </summary>
            Callback = 13,
            /// <summary>
            /// 上传
            /// </summary>
            Upload = 14,
            /// <summary>
            /// 下载
            /// </summary>
            Download = 15,
            /// <summary>
            /// 管理员登出
            /// </summary>
            UserLogout = 16,
            /// <summary>
            /// 会员登出
            /// </summary>
            MemberLogout = 17
        }
    }
}

