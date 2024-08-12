using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Northwind.Reporting.Interfaces;
using Northwind.Reporting.Models;

namespace Northwind.Reporting.Rcl.Areas.Reporting.Controllers
{
    [Area("Reporting")]
    [Route("Reporting/[controller]")]
    [Authorize()]
    public class HomeController : Controller
    {
        public HomeController(ILogger<HomeController> logger, IReportRecordRepository recordRepository, IReportFactory reportFactory)
        {
            Logger = logger;
            RecordRepository = recordRepository;
            ReportFactory = reportFactory;
        }

        private ILogger<HomeController> Logger { get; set; }

        private IReportRecordRepository RecordRepository { get; set; }

        private IReportFactory ReportFactory { get; set; }

        /// <summary>
        /// List out all the reports which the user can run here.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Index")]
        public IActionResult Index()
        {
            try
            {
                return View(ReportFactory.Reports);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error getting home page");
                return View("Error");
            }
        }

        /// <summary>
        /// List out all the reports with status that the user can see.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("MyReports")]
        public async Task<IActionResult> MyReports()
        {
            try
            {
                string user = User.Identity?.Name ?? (User.Claims.Where(w => w.Type == "name").FirstOrDefault()?.Value ?? string.Empty);

                return View(await RecordRepository.Fetch(w => w.UserName == user));
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error getting home page");

                return View("Error");
            }
        }

        [HttpGet]
        [Route("Download/{reportId}")]
        public async Task<IActionResult> Download([FromRoute] long reportId)
        {
            try
            {
                ReportRecord? record = await RecordRepository.Fetch(reportId);

                if (record == default)
                {
                    return NotFound();
                }
                else if (System.IO.File.Exists(record.OutputPath ?? string.Empty))
                {
                    string mime = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    if (record.OutputPath?.EndsWith("csv") ?? false)
                    {
                        mime = "text/csv";
                    }
                    else if (record.OutputPath?.EndsWith("txt") ?? false)
                    {
                        mime = "text/text";
                    }

                    return new FileStreamResult(new FileStream(path: (record.OutputPath ?? string.Empty), mode: FileMode.Open, access: FileAccess.Read), mime)
                    {
                        LastModified = DateTime.UtcNow,
                        FileDownloadName = $"{record.ReportName}.{((record.OutputPath?.Contains(".") ?? false) ? record.OutputPath?.Substring(record.OutputPath.IndexOf(".") + 1) : "txt")}"
                    };
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error getting home page");

                return View("Error");
            }
        }

        [HttpPost]
        [Route("Delete/{reportId}")]
        public async Task<IActionResult> Delete([FromRoute] long reportId)
        {
            try
            {
                _ = await RecordRepository.Delete(reportId);

                return RedirectToAction("MyReports");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error deleting item");

                return View("Error");
            }
        }
    }
}
