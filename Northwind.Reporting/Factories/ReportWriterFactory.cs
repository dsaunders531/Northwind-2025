using Northwind.Reporting.Enums;
using Northwind.Reporting.Interfaces;
using Northwind.Reporting.ReportWriters;

namespace Northwind.Reporting.Factories
{
    public static class ReportWriterFactory
    {
        public static ReportWriter<TDataRow> GetWriter<TDataRow>(ReportWriter writer)
            where TDataRow : class
        {
            switch (writer)
            {
                case ReportWriter.msoXl:
                    return new MsoXlReportWriter<TDataRow>();
                case ReportWriter.Csv:
                default:
                    return new CsvReportWriter<TDataRow>();
            }
        }
    }
}
