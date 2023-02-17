namespace Northwind.Reporting.Interfaces
{
    public interface IReportFactory
    {
        public IEnumerable<Report> Reports { get; }


        public Report GetReport(string name);

        public bool Exists(string name);
    }
}
