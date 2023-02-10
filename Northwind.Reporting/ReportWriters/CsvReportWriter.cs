using FileHelpers;
using Northwind.Reporting.Interfaces;

namespace Northwind.Reporting.ReportWriters
{
    /// <summary>
    /// Writer to create csv files.
    /// </summary>
    /// <typeparam name="TDataRow"></typeparam>
    /// <remarks>The data row type must be annotated with the file helper attributes.</remarks>
    public class CsvReportWriter<TDataRow> : ReportWriter<TDataRow>
        where TDataRow : class
    {
        public CsvReportWriter()
        {
            this.WriterType = Enums.ReportWriter.Csv;
        }

        public override Task<Uri> Write(IEnumerable<TDataRow> data)
        {
            FileHelperAsyncEngine<TDataRow> fileEngine = new FileHelperAsyncEngine<TDataRow>();

            string outputPath = Path.Combine(this.ReportOutputBase, $"{Guid.NewGuid()}.csv");

            using (fileEngine.BeginWriteFile(outputPath))
            {
                foreach (TDataRow item in data)
                {
                    fileEngine.WriteNext(item);
                }

                fileEngine.Close();
            }

            return Task.FromResult(new Uri(outputPath));
        }
    }
}
