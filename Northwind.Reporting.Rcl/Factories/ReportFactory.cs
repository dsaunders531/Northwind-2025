using Northwind.Reporting.Interfaces;
using Northwind.Reporting.Rcl.Reports;

namespace Northwind.Reporting.Rcl.Factories
{
    internal class ReportFactory : IReportFactory
    {
        public ReportFactory(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
            this._reports = new Lazy<IEnumerable<Report>>(this.BuildReportList, false);
        }

        private IServiceProvider ServiceProvider { get; set; }

        private IEnumerable<Report> BuildReportList()
        {
            return new Report[] {
                new SalesByCategoryAndYearReport(this.ServiceProvider),
                new SalesTotalsByAmountsReport(this.ServiceProvider)
            };
        }

        // Use a Lazy so the list is build the first time it is needed.
        private Lazy<IEnumerable<Report>> _reports;

        public IEnumerable<Report> Reports => _reports.Value;

        public bool Exists(string name)
        {
            return this.Reports.Any(a => a.Name.ToLowerInvariant() == name.ToLowerInvariant());
        }

        public Report GetReport(string name)
        {
            return this.Reports.Where(w => w.Name.ToLowerInvariant() == name.ToLowerInvariant()).FirstOrDefault() 
                ?? throw new KeyNotFoundException($"Could not find report {name}.");
        }
    }
}
