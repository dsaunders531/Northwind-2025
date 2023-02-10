using Northwind.Reporting.Models;
using Patterns;

namespace Northwind.Reporting.Interfaces
{
    public interface IReportRecordRepository : IRepository<ReportRecord, long>
    {
    }
}
