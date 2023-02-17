using Northwind.Reporting.Enums;

namespace Northwind.Reporting.Interfaces
{
    public abstract class ReportWriter<TDataRow>
        where TDataRow : class
    {
        /// <summary>
        /// The type of writer (only one is needed per report writer value.
        /// </summary>
        public ReportWriter WriterType { get; protected set; }

        public string ReportOutputBase => Environment.GetEnvironmentVariable("REPORTWRITER_BASE_DIR") ?? @"c:\tmp";

        /// <summary>
        /// Write the data to a file.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public abstract Task<Uri> Write(IEnumerable<TDataRow> data);
    }
}
