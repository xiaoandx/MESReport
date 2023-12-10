using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MR.Manage.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MR.Manage.Controllers
{
    /// <summary>
    /// 内容摘要: 控制台主页面
    /// </summary>
    public class MainController : BaseController<MainController>
    {
        public MainController(MRManageContext context) : base(context)
        {

        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        // GET: /<controller>/
        public IActionResult Console()
        {
            return View();
        }
    }
}
