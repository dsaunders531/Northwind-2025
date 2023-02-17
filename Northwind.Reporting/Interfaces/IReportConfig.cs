using Northwind.Reporting.Enums;

namespace Northwind.Reporting.Interfaces
{
    public interface IReportConfig
    {
        ReportFrequency Frequency { get; set; }

        int? FrequencyWeeklyMonthly { get; set; }

        ReportWriter ReportWriter { get; set; }
    }
}
