using Northwind.Reporting.Enums;

namespace Northwind.Reporting.Interfaces
{
    public interface IReportParametersBase
    {
        DateTime? EndDate { get; set; }
        ReportPeriod? ReportPeriod { get; set; }
        DateTime? StartDate { get; set; }
    }
}
