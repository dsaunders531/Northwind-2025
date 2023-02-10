using Northwind.Reporting.Enums;
using Northwind.Reporting.Models;

namespace Northwind.Reporting.Extensions
{
    public static class ReportRecordExtensions
    {
        private static bool Validate(this ReportRecord record)
        {
            if ((record.Frequency == ReportFrequency.Weekly || record.Frequency == ReportFrequency.Monthly) && !record.FrequencyWeeklyMonthly.HasValue)
            {
                throw new ArgumentException("FrequencyDailyMonthly must have a value when the report frequency is weekly or monthly.");
            }

            return true;
        }

        public static DateTime CalculateDue(this ReportRecord record)
        {
            _ = record.Validate();

            switch (record.Frequency)
            {
                case ReportFrequency.Daily:
                    return DateTime.UtcNow.Date.AddDays(1);

                case ReportFrequency.Weekly:
                    // assuming that 0 = Sunday.
                    DayOfWeek nowDoW = DateTime.UtcNow.DayOfWeek;

                    if ((record.FrequencyWeeklyMonthly ?? 0) < (int)nowDoW)
                    {
                        return DateTime.UtcNow.AddDays(7 - ((int)nowDoW - (record.FrequencyWeeklyMonthly ?? 0))).Date;
                    }
                    else if ((record.FrequencyWeeklyMonthly ?? 0) > (int)nowDoW)
                    {
                        return DateTime.UtcNow.AddDays((int)nowDoW * -1 + (record.FrequencyWeeklyMonthly ?? 0)).Date;
                    }
                    else
                    {
                        return DateTime.UtcNow.AddDays(7).Date;
                    }

                case ReportFrequency.Monthly:
                    return DateTime.UtcNow.AddDays(DateTime.UtcNow.Day * -1).AddMonths(1).AddDays(record.FrequencyWeeklyMonthly ?? 0).Date;

                case ReportFrequency.Immediate:
                default:
                    return DateTime.UtcNow;
            }
        }

        public static DateTime CalculateNextDue(this ReportRecord record)
        {
            _ = record.Validate();

            switch (record.Frequency)
            {
                case ReportFrequency.Daily:
                    return record.RunTime.AddDays(1);

                case ReportFrequency.Weekly:
                    return record.RunTime.AddDays(7);

                case ReportFrequency.Monthly:
                    return record.RunTime.AddMonths(1);

                case ReportFrequency.Immediate:
                default:
                    throw new ArgumentException("Reports which are to be run immediately do not have a calculate next");
            }
        }

        public static ReportRecord Clone(this ReportRecord record)
        {
            return new ReportRecord()
            {
                Frequency = record.Frequency,
                FrequencyWeeklyMonthly = record.FrequencyWeeklyMonthly,
                OutputFormat = record.OutputFormat,
                OutputPath = null,
                ReportName = record.ReportName,
                ReportParametersJson = record.ReportParametersJson,
                RunTime = record.CalculateNextDue(),
                Status = ReportStatus.Created,
                UserName = record.UserName
            };
        }
    }
}
