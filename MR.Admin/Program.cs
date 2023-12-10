using System.Reflection;
using System.Text;
using Lazy.Captcha.Core;
using Lazy.Captcha.Core.Generator;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using MR.Manage.Extensions;
using MR.Manage.Filters;
using MR.Utility.Config;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// 默认使用内存存储（AddDistributedMemoryCache）
// 全部配置
builder.Services.AddCaptcha(builder.Configuration, option =>
{
    option.CaptchaType = CaptchaType.ARITHMETIC; // 验证码类型
    option.CodeLength = 2; // 验证码长度, 要放在CaptchaType设置后.  当类型为算术表达式时，长度代表操作的个数
    option.ExpirySeconds = 30; // 验证码过期时间
    option.IgnoreCase = true; // 比较时是否忽略大小写
    option.StoreageKeyPrefix = "MR"; // 存储键前缀

    option.ImageOption.Animation = false; // 是否启用动画
    option.ImageOption.FrameDelay = 30; // 每帧延迟,Animation=true时有效, 默认30

    option.ImageOption.Width = 150; // 验证码宽度
    option.ImageOption.Height = 50; // 验证码高度
    option.ImageOption.BackgroundColor = SkiaSharp.SKColor.FromHsl(255, 255, 255, 10); // 验证码背景色

    option.ImageOption.BubbleCount = 2; // 气泡数量
    option.ImageOption.BubbleMinRadius = 5; // 气泡最小半径
    option.ImageOption.BubbleMaxRadius = 15; // 气泡最大半径
    option.ImageOption.BubbleThickness = 1; // 气泡边沿厚度

    option.ImageOption.InterferenceLineCount = 5; // 干扰线数量

    option.ImageOption.FontSize = 36; // 字体大小
    option.ImageOption.FontFamily = DefaultFontFamilys.Instance.Actionj; // 字体

    /* 
     * 中文使用kaiti，其他字符可根据喜好设置（可能部分转字符会出现绘制不出的情况）。
     * 当验证码类型为“ARITHMETIC”时，不要使用“Ransom”字体。（运算符和等号绘制不出来）
     */

    option.ImageOption.TextBold = true;// 粗体，该配置2.0.3新增
});

// 如果使用redis分布式缓存
//builder.Services.AddStackExchangeRedisCache(options =>
//{
//    options.Configuration = builder.Configuration.GetConnectionString("RedisCache");
//    options.InstanceName = "captcha:";
//});

// 注册任务调度
builder.Services.AddFluentSchedulerSetup();

// 注入Claim认证
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
      .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, auth =>
      {
          auth.Cookie.HttpOnly = true;
          auth.Cookie.SecurePolicy = CookieSecurePolicy.Always;
          auth.LoginPath = new PathString("/"); // 登录页面的url
          auth.LoginPath = new PathString("/login"); // 登录页面的url
          auth.AccessDeniedPath = new PathString("/login"); // 没有授权跳转的页面
          auth.ExpireTimeSpan = TimeSpan.FromHours(24); // cookies的过期时间
      });


// 注入全局异常日志服务
builder.Services.AddControllers(option =>
{
    option.Filters.Add(typeof(GlobalExceptionFilter));
});

// 增加Http组件
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// 根据环境设定数据库连接
var env = AppSettingsFilter.GetSetting("Setting:Env");
if (builder.Environment.IsProduction())
{
    if (env == "PRO")
    {
        // 开发模式-生产库
        builder.Services.AddDbContext<MR.Manage.Data.MRManageContext>(opt => {
            if (AppSettingsFilter.GetSetting("ConnectionStrings:PRO_LOCAL") != null)
            {
                var connection = AppSettingsFilter.GetSetting("ConnectionStrings:PRO_LOCAL");
                var server_version = ServerVersion.AutoDetect(connection);
                //opt.UseMySql(connection, server_version);
                opt.UseSqlite("Filename=mesreport.db");
            }
        });
    }
    else
    {
        // 开发模式-测试库
        builder.Services.AddDbContext<MR.Manage.Data.MRManageContext>(opt =>
        {
            if (AppSettingsFilter.GetSetting("ConnectionStrings:TEST_LOCAL") != null)
            {
                var connection = AppSettingsFilter.GetSetting("ConnectionStrings:TEST_LOCAL");
                var server_version = ServerVersion.AutoDetect(connection);
                //opt.UseMySql(connection, server_version);
                opt.UseSqlite("Filename=mesreport.db");
            }
        });
    }
}
else
{
    if (env == "PRO")
    {
        // 生产模式-生产库
        builder.Services.AddDbContext<MR.Manage.Data.MRManageContext>(opt => {
            if (AppSettingsFilter.GetSetting("ConnectionStrings:PRO_PUBLIC") != null)
            {
                var connection = AppSettingsFilter.GetSetting("ConnectionStrings:PRO_PUBLIC");
                var server_version = ServerVersion.AutoDetect(connection);
                //opt.UseMySql(connection, server_version);
                opt.UseSqlite("Filename=mesreport.db");
            }
        });
    }
    else
    {
        // 生产模式-测试库
        builder.Services.AddDbContext<MR.Manage.Data.MRManageContext>(opt =>
        {
            if (AppSettingsFilter.GetSetting("ConnectionStrings:TEST_PUBLIC") != null)
            {
                var connection = AppSettingsFilter.GetSetting("ConnectionStrings:TEST_PUBLIC");
                var server_version = ServerVersion.AutoDetect(connection);
                //opt.UseMySql(connection, server_version);
                opt.UseSqlite("Filename=mesreport.db");
            }
        });
    }
}

var app = builder.Build();
// 注意：注册的各类服务必须放在builder.Build()之前

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/home/error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//// 处理404页面 - 有问题不能使用
//app.Use(async (context, next) =>
//{
//    await next();
//    if (context.Response.StatusCode == 404)
//    {
//        context.Request.Path = "/home/error";
//        await next();
//    }
//});

//https跳转
//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseForwardedHeaders(new ForwardedHeadersOptions { ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();

