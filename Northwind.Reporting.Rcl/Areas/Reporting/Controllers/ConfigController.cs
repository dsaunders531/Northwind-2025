using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Northwind.Reporting.Extensions;
using Northwind.Reporting.Interfaces;
using Northwind.Reporting.Models;
using Northwind.Reporting.Rcl.Data;
using Northwind.Reporting.Rcl.Reports;
using Patterns.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind.Reporting.Rcl.Areas.Reporting.Controllers
{
    /// <summary>
    /// The controller which handles the configuration pages for the reports.
    /// </summary>
    [Area("Reporting")]
    [Route("Reporting/[controller]")]
    [Authorize()]
    public class ConfigController : Controller
    {
        public ConfigController(ILogger<HomeController> logger, IReportRecordRepository recordRepository)
        {
            this.Logger = logger;
            this.RecordRepository = recordRepository;
        }

        private ILogger<HomeController> Logger { get; set; }

        private IReportRecordRepository RecordRepository { get; set; }

        [HttpGet]
        [Route("SalesTotalByAmount")]
        public IActionResult SalesTotalByAmount()
        {
            try
            {
                return View(new ReportConfig<SalesTotalsByAmountsReportParameters>());
            }
            catch (Exception ex)
            {
                this.Logger.LogError(ex, "Error getting sales total by amount");
                return View("Error");
            }
        }

        [HttpPost]
        [Route("SalesTotalByAmount")]        
        public async Task<IActionResult> SalesTotalByAmount([FromForm] ReportConfig<SalesTotalsByAmountsReportParameters> model)
        {
            try
            {
                // the report name must match the report.Name property
                return await this.Save(model, "Sales totals by quantity");                
            }
            catch (Exception ex)
            {
                this.Logger.LogError(ex, "Error saving sales total by amount");

                ModelState.AddModelError(string.Empty, $"An error happened saving the form. {ex.GetType()}: {ex.Message}");

                return View(model);
            }
        }

        [HttpGet]
        [Route("SalesByCategoryAndYear")]
        public IActionResult SalesByCategoryAndYear()
        {
            try
            {
                return View(new ReportConfig<SalesByCategoryAndYearReportParameters>());
            }
            catch (Exception ex)
            {
                this.Logger.LogError(ex, "Error getting sales by category");               
                return View("Error");
            }
        }

        [HttpPost]
        [Route("SalesByCategoryAndYear")]
        public async Task<IActionResult> SalesByCategoryAndYear([FromForm] ReportConfig<SalesByCategoryAndYearReportParameters> model)
        {
            try
            {
                // the report name must match the report.Name property
                return await this.Save(model, "Sales By Category");                
            }
            catch (Exception ex)
            {
                this.Logger.LogError(ex, "Error saving sales by category");
                
                ModelState.AddModelError(string.Empty, $"An error happened saving the form. {ex.GetType()}: {ex.Message}");

                return View(model);                
            }
        }

        private async Task<IActionResult> Save<TParameter>(ReportConfig<TParameter> model, string reportName)
            where TParameter : IReportParametersBase
        {
            if (ModelState.IsValid)
            {
                ReportRecord record = new ReportRecord()
                {
                    Frequency = model.Frequency,
                    FrequencyWeeklyMonthly = model.FrequencyWeeklyMonthly,
                    OutputFormat = model.ReportWriter,
                    ReportName = reportName,
                    ReportParametersJson = model.Parameters.ToJson(),
                    Status = Enums.ReportStatus.Created,
                    UserName = User.Identity?.Name ?? (User.Claims.Where(w => w.Type == "name").FirstOrDefault()?.Value ?? string.Empty)
                };

                record.RunTime = record.CalculateDue();

                _ = await this.RecordRepository.Create(record);

                return LocalRedirect("/Reporting/Home/MyReports");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "There is something wrong with the form!");

                return View(model);
            }            
        }
    }    
}
