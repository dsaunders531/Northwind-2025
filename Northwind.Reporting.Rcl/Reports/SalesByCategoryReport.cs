using Microsoft.Extensions.DependencyInjection;
using Northwind.Context.Interfaces;
using Northwind.Context.Models.Reporting;
using Northwind.Reporting.Extensions;
using Northwind.Reporting.Interfaces;
using Northwind.Reporting.Models;
using System.ComponentModel.DataAnnotations;

namespace Northwind.Reporting.Rcl.Reports
{
    public class SalesByCategoryAndYearReport : Report<SaleByCategoryReport, SalesByCategoryAndYearReportParameters>
    {
        public SalesByCategoryAndYearReport(IServiceProvider serviceProvider) : base(serviceProvider) { }

        public override string Name => "Sales By Category";

        public override string Description => "Show the sales by category for a year.";

        public override string ConfigPageRoute => "/Reporting/Config/SalesByCategoryAndYear";

        public override async Task<IEnumerable<SaleByCategoryReport>> Run(SalesByCategoryAndYearReportParameters parameters)
        {
            INorthwindService northwindService = this.ServiceProvider.GetRequiredService<INorthwindService>();

            // not really relevant here - since year is the only parameter we are interested in.
            parameters.CalculateStartAndEndDates();

            return (await northwindService.SalesByCategory(parameters.Category, parameters.Year)).AsEnumerable();
        }
    }

    public class SalesByCategoryAndYearReportParameters : ReportParametersBase
    {
        public SalesByCategoryAndYearReportParameters()
        {
            this.Year = DateTime.UtcNow.Year;
        }
        
        [Required]
        public string Category { get; set; } = string.Empty;

        [Required]
        public int Year { get; set; }
    }
}
