using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind.Reporting.Rcl.Areas.Reporting.Controllers
{
    [Area("Reporting")]
    [Route("Reporting/[controller]")]
    [Authorize()]
    public class HomeController : Controller
    {
        public HomeController(ILogger<HomeController> logger)
        {
            Logger = logger;
        }

        private ILogger<HomeController> Logger { get; set; }

        public IActionResult Index()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                this.Logger.LogError(ex, "Error getting home page");
                return View("Error");
            }
        }
    }
}
