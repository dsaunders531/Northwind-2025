using Microsoft.Extensions.DependencyInjection;
using Northwind.Context.Interfaces;
using Northwind.Context.Models.Reporting;
using Northwind.Reporting.Extensions;
using Northwind.Reporting.Interfaces;
using Northwind.Reporting.Models;

namespace Northwind.Reporting.Rcl.Reports
{
    public class SalesTotalsByAmountsReport : Report<SalesTotalsByAmount, SalesTotalsByAmountsReportParameters>
    {
        public SalesTotalsByAmountsReport(IServiceProvider serviceProvider) : base(serviceProvider) {}

        public override string Name => "Sales totals by quantity";

        public override string Description => "Show sales totals between dates.";

        public override string ConfigPageRoute => "/Reporting/Config/SalesTotalByAmount";

        public override async Task<IEnumerable<SalesTotalsByAmount>> Run(SalesTotalsByAmountsReportParameters parameters)
        {
            // Make sure the dates cover all the time on selected days
            parameters.CalculateStartAndEndDates();
            
            INorthwindService service = ServiceProvider.GetRequiredService<INorthwindService>();

            return (await service.SalesTotalsByAmounts(parameters.StartDate.Value, parameters.EndDate.Value)).AsEnumerable();
        }
    }

    public class SalesTotalsByAmountsReportParameters : ReportParametersBase
    {
        public SalesTotalsByAmountsReportParameters()
        {
            DateTime now = DateTime.UtcNow;

            StartDate = now.AddDays((now.Day * -1) + 1).AddMonths(-1);
            EndDate = now.AddDays(now.Day * -1);
        }        
    }
}
