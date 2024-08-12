using Northwind.Reporting.Enums;
using Northwind.Reporting.Extensions;
using Northwind.Reporting.Interfaces;
using Northwind.Reporting.Models;
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
            MockReport report = new MockReport(new MockServiceProvider());

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

        /// <summary>
        /// See that the calculated dates are as expected.
        /// </summary>
        [Test]
        public void GetRunDateTime()
        {
            // Immediate
            ReportRecord record = new ReportRecord()
            {
                Frequency = ReportFrequency.Immediate,
                FrequencyWeeklyMonthly = null,
                ReportParametersJson = new MockReportParameters() { ReportPeriod = ReportPeriod.Yesterday }.ToJson()
            };

            DateTime calculated = record.CalculateDue();

            Assert.That(calculated.Date, Is.EqualTo(DateTime.UtcNow.Date));

            // Daily
            record.Frequency = ReportFrequency.Daily;
            calculated = record.CalculateDue();

            Assert.That(calculated.Date, Is.EqualTo(DateTime.UtcNow.AddDays(1).Date));

            // Weekly
            record.Frequency = ReportFrequency.Weekly;
            record.FrequencyWeeklyMonthly = 1; // Monday

            calculated = record.CalculateDue();

            Assert.That(calculated.Date, Is.GreaterThan(DateTime.UtcNow.Date));
            Assert.That(calculated.DayOfWeek, Is.EqualTo(DayOfWeek.Monday));

            // Monthly
            record.Frequency = ReportFrequency.Monthly;
            record.FrequencyWeeklyMonthly = 14; // 14th day of the month

            calculated = record.CalculateDue();

            Assert.That(calculated.Date, Is.GreaterThan(DateTime.UtcNow.Date));
            Assert.That(calculated.Day, Is.EqualTo(14));
        }

        [Test]
        public void GetNextRunDateTime()
        {
            MockReportParameters reportParameters = new MockReportParameters();

            // set a start and end - there should be no difference after calculating.
            reportParameters.ReportPeriod = null;
            reportParameters.StartDate = DateTime.UtcNow.AddYears(-1);
            reportParameters.EndDate = DateTime.UtcNow.AddDays(-1);

            reportParameters.CalculateStartAndEndDates();

            Assert.That(reportParameters.StartDate, Is.Not.Null);
            Assert.That(reportParameters.EndDate, Is.Not.Null);

            // see that the start is the first moment of the day
            Assert.That(reportParameters.StartDate, Is.EqualTo(DateTime.UtcNow.AddYears(-1).Date));
            Assert.That(reportParameters.EndDate, Is.EqualTo(DateTime.UtcNow.Date.AddTicks(-1)));

            // check that setting the report period does not override any dates which have been set
            reportParameters.ReportPeriod = ReportPeriod.Yesterday;

            reportParameters.CalculateStartAndEndDates();

            Assert.That(reportParameters.StartDate, Is.EqualTo(DateTime.UtcNow.AddYears(-1).Date));
            Assert.That(reportParameters.EndDate, Is.EqualTo(DateTime.UtcNow.Date.AddTicks(-1)));

            // set yesterday
            reportParameters.ReportPeriod = ReportPeriod.Yesterday;
            reportParameters.StartDate = null;
            reportParameters.EndDate = null;

            reportParameters.CalculateStartAndEndDates();
            Assert.That(reportParameters.StartDate, Is.EqualTo(DateTime.UtcNow.Date.AddDays(-1)));
            Assert.That(reportParameters.EndDate, Is.EqualTo(DateTime.UtcNow.Date.AddTicks(-1)));

            // set last 7 days
            reportParameters.ReportPeriod = ReportPeriod.Last7Days;
            reportParameters.StartDate = null;
            reportParameters.EndDate = null;

            reportParameters.CalculateStartAndEndDates();
            Assert.That(reportParameters.StartDate, Is.EqualTo(DateTime.UtcNow.Date.AddDays(-8)));
            Assert.That(reportParameters.EndDate, Is.EqualTo(DateTime.UtcNow.Date.AddTicks(-1)));

            // set last month            
            reportParameters.ReportPeriod = ReportPeriod.LastMonth;
            reportParameters.StartDate = null;
            reportParameters.EndDate = null;

            reportParameters.CalculateStartAndEndDates();
            Assert.That(reportParameters.StartDate, Is.EqualTo(DateTime.UtcNow.AddMonths(-1).AddDays((DateTime.UtcNow.Day - 1) * -1).Date));
            Assert.That(reportParameters.EndDate, Is.EqualTo(DateTime.UtcNow.AddDays((DateTime.UtcNow.Day - 1) * -1).Date.AddTicks(-1)));

            // month to date
            reportParameters.ReportPeriod = ReportPeriod.MonthToDate;
            reportParameters.StartDate = null;
            reportParameters.EndDate = null;

            reportParameters.CalculateStartAndEndDates();
            Assert.That(reportParameters.StartDate, Is.EqualTo(DateTime.UtcNow.AddDays((DateTime.UtcNow.Day - 1) * -1).Date));
            Assert.That(reportParameters.EndDate, Is.EqualTo(DateTime.UtcNow.Date.AddTicks(-1)));

            // last year
            reportParameters.ReportPeriod = ReportPeriod.LastYear;
            reportParameters.StartDate = null;
            reportParameters.EndDate = null;

            reportParameters.CalculateStartAndEndDates();

            Assert.That(reportParameters.StartDate, Is.EqualTo(new DateTime(DateTime.UtcNow.AddYears(-1).Year, 1, 1).Date));
            Assert.That(reportParameters.EndDate, Is.EqualTo(new DateTime(DateTime.UtcNow.Year, 1, 1).Date.AddTicks(-1)));

            // year to date
            reportParameters.ReportPeriod = ReportPeriod.YearToDate;
            reportParameters.StartDate = null;
            reportParameters.EndDate = null;

            reportParameters.CalculateStartAndEndDates();

            Assert.That(reportParameters.StartDate, Is.EqualTo(new DateTime(DateTime.UtcNow.Year, 1, 1).Date));
            Assert.That(reportParameters.EndDate, Is.EqualTo(DateTime.UtcNow.Date.AddTicks(-1)));
        }

        internal class MockReportParameters : ReportParametersBase
        { }

        internal class MockReportOptions : IReportOptions
        {
            public string? ReportParametersJson { get; set; }

            public ReportWriter OutputFormat { get; set; }
        }
    }
}