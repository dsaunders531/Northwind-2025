﻿using FileHelpers;
using Northwind.Reporting.Interfaces;

namespace Northwind.Reporting.Tests
{
    [DelimitedRecord(",")]
    public class MockReportDataRow
    {
        public long Id { get; set; }

        public string Value { get; set; }

        [FieldConverter(ConverterKind.Date, "dd-MM-yyyy")]
        public DateTime Date { get; set; }
    }

    public class MockReportParameters
    {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }

    public class MockReport : Report<MockReportDataRow, MockReportParameters>
    {
        public MockReport(IServiceProvider provider) : base(provider)
        {
        }

        public override string Name => "Mock Report";

        public override string Description => "A report to test the functionality of the reporting system";

        public override string ConfigPageRoute => string.Empty;

        public override Task<IEnumerable<MockReportDataRow>> Run(MockReportParameters parameters)
        {
            List<MockReportDataRow> result = new List<MockReportDataRow>();

            for (DateTime i = parameters.StartDate; i <= parameters.EndDate; i = i.AddDays(1))
            {
                result.Add(new MockReportDataRow() { Id = i.Ticks, Value = i.ToLongDateString(), Date = i });
            }

            return Task.FromResult(result.AsEnumerable());
        }
    }
}
