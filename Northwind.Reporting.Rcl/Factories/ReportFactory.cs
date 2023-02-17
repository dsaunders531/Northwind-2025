using Northwind.Reporting.Interfaces;
using Northwind.Reporting.Rcl.Reports;

namespace Northwind.Reporting.Rcl.Factories
{
    internal class ReportFactory : IReportFactory
    {
        public ReportFactory(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
            _reports = new Lazy<IEnumerable<Report>>(BuildReportList, false);
        }

        private IServiceProvider ServiceProvider { get; set; }

        private IEnumerable<Report> BuildReportList()
        {
            return new Report[] {
                new SalesByCategoryAndYearReport(ServiceProvider),
                new SalesTotalsByAmountsReport(ServiceProvider)
            };
        }

        // Use a Lazy so the list is build the first time it is needed.
        private Lazy<IEnumerable<Report>> _reports;

        public IEnumerable<Report> Reports => _reports.Value;

        public bool Exists(string name)
        {
            return Reports.Any(a => a.Name.ToLowerInvariant() == name.ToLowerInvariant());
        }

        public Report GetReport(string name)
        {
            return Reports.Where(w => w.Name.ToLowerInvariant() == name.ToLowerInvariant()).FirstOrDefault() 
                ?? throw new KeyNotFoundException($"Could not find report {name}.");
        }
    }
}
