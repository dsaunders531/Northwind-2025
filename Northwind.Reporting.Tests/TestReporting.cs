using Northwind.Reporting.Enums;
using Northwind.Reporting.Interfaces;
using Patterns.Extensions;

namespace Northwind.Reporting.Tests
{
    public class ReportingTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task RunMockReport()
        {
            

            Report<MockReportDataRow, MockReportParameters> report = new MockReport(new MockServiceProvider());

            Uri result = await report.Run(new MockReportOptions()
            {
                OutputFormat = ReportWriter.Csv,
                ReportParametersJson = new MockReportParameters()
                {
                    StartDate = new DateTime(2020, 10, 1),
                    EndDate = new DateTime(2021, 2, 28)
                }.ToJson()
            });

            // Note that we are not testing the output - each report will be different.             
            Assert.That(File.Exists(result.AbsolutePath));

            File.Delete(result.AbsolutePath);
        }

        [Test]
        public void GetRunDateTime()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void GetNextRunDateTime()
        {
            throw new NotImplementedException();
        }

        internal class MockReportOptions : IReportOptions
        {
            public string ReportParametersJson { get; set; }

            public ReportWriter OutputFormat { get; set; }
        }
    }
}