using Microsoft.Extensions.DependencyInjection;
using Northwind.Context.Interfaces;
using Northwind.Context.Models.Reporting;
using Northwind.Reporting.Interfaces;
using System.ComponentModel.DataAnnotations;

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
            parameters.StartDate = parameters.StartDate.Date;
            parameters.EndDate = parameters.EndDate.Date.AddDays(1).AddTicks(-1); // the last moment of the day

            INorthwindService service = this.ServiceProvider.GetRequiredService<INorthwindService>();

            return (await service.SalesTotalsByAmounts(parameters.StartDate, parameters.EndDate)).AsEnumerable();
        }
    }

    public class SalesTotalsByAmountsReportParameters
    {
        public SalesTotalsByAmountsReportParameters()
        {
            DateTime now = DateTime.UtcNow;

            this.StartDate = now.AddDays((now.Day * -1) + 1).AddMonths(-1);
            this.EndDate = now.AddDays(now.Day * -1);
        }

        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
    }
}
