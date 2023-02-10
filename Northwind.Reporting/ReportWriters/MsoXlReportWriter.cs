using ClosedXML.Excel;
using Northwind.Reporting.Interfaces;

namespace Northwind.Reporting.ReportWriters
{
    public class MsoXlReportWriter<TDataRow> : ReportWriter<TDataRow>
         where TDataRow : class
    {
        public MsoXlReportWriter()
        {
            this.WriterType = Enums.ReportWriter.msoXl;
        }

        public override Task<Uri> Write(IEnumerable<TDataRow> data)
        {
            string outputPath = Path.Combine(this.ReportOutputBase, $"{Guid.NewGuid()}.xlsx");            

            using (XLWorkbook workbook = new XLWorkbook())
            {
                IXLWorksheet sheet = workbook.AddWorksheet("data");

                sheet.Cell("A1").InsertData(data);

                workbook.SaveAs(outputPath);
            }

            return Task.FromResult(new Uri(outputPath));
        }
    }
}
