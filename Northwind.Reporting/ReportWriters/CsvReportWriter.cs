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
            WriterType = Enums.ReportWriter.Csv;
        }

        public override Task<Uri> Write(IEnumerable<TDataRow> data)
        {
            FileHelperAsyncEngine<TDataRow> fileEngine = new FileHelperAsyncEngine<TDataRow>();

            string outputPath = Path.Combine(ReportOutputBase, $"{Guid.NewGuid()}.csv");

            if (data.Any())
            {
                fileEngine.HeaderText = fileEngine.GetFileHeader();

                using (fileEngine.BeginWriteFile(outputPath))
                {
                    foreach (TDataRow item in data)
                    {
                        fileEngine.WriteNext(item);
                    }

                    fileEngine.Close();
                }
            }
            else
            {
                outputPath = Path.Combine(ReportOutputBase, "NoDataFound.txt");
                File.WriteAllText(outputPath, "No Data Found!");
            }

            return Task.FromResult(new Uri(outputPath));
        }
    }
}
