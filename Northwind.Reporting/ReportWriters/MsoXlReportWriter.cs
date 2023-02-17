using ClosedXML.Attributes;
using ClosedXML.Excel;
using Northwind.Reporting.Interfaces;
using System.Reflection;

namespace Northwind.Reporting.ReportWriters
{
    public class MsoXlReportWriter<TDataRow> : ReportWriter<TDataRow>
         where TDataRow : class
    {
        public MsoXlReportWriter()
        {
            WriterType = Enums.ReportWriter.msoXl;
        }

        public override Task<Uri> Write(IEnumerable<TDataRow> data)
        {
            string outputPath = Path.Combine(ReportOutputBase, $"{Guid.NewGuid()}.xlsx");

            if (data.Any())
            {
                using (XLWorkbook workbook = new XLWorkbook())
                {
                    IXLWorksheet sheet = workbook.AddWorksheet("data");

                    // Add the header row:
                    PropertyInfo[] props = typeof(TDataRow).GetProperties();

                    int column = 1;
                    HashSet<KeyValuePair<int, string>> columns = new HashSet<KeyValuePair<int, string>>();

                    foreach (PropertyInfo item in props)
                    {                        
                        XLColumnAttribute? attr = item.GetCustomAttribute<XLColumnAttribute>();

                        columns.Add(new KeyValuePair<int, string>(attr?.Order ?? column, attr?.Header ?? item.Name));

                        column++;
                    }

                    column = 1;
                    foreach (KeyValuePair<int, string> item in columns.OrderBy(o => o.Key))
                    {
                        sheet.Cell(1, column).Value = item.Value;
                        column++;
                    }

                    sheet.Cell("A2").InsertData(data);

                    workbook.SaveAs(outputPath);
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
