using Northwind.Reporting.Enums;

namespace Northwind.Reporting.Interfaces
{
    /// <summary>
    /// The report is only interested in the parameters so it is dependant on this properly only.
    /// </summary>
    /// <remarks>This should make testing easier</remarks>
    public interface IReportOptions
    {
        string ReportParametersJson { get; }

        ReportWriter OutputFormat { get; }
    }
}
