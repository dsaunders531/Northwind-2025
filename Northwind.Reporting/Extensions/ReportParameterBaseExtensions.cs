using Northwind.Reporting.Enums;
using Northwind.Reporting.Models;

namespace Northwind.Reporting.Extensions
{
    public static class ReportParameterBaseExtensions
    {
        /// <summary>
        /// Calculate the start and end dates for repeating reports
        /// </summary>
        /// <param name="model"></param>
        public static void CalculateStartAndEndDates(this ReportParametersBase model)
        {
            // only calculate when the report period has a value
            if (model.ReportPeriod.HasValue && !model.StartDate.HasValue && !model.EndDate.HasValue)
            {
                DateTime now = DateTime.UtcNow;
                int year = now.Year;
                Dictionary<int, int[]> quarters = new Dictionary<int, int[]>
                {
                    { 1, new int[] { 1, 2, 3 } },
                    { 2, new int[] { 4, 5, 6 } },
                    { 3, new int[] { 7, 8, 9 } },
                    { 4, new int[] { 10, 11, 12 } }
                };

                int currentQuarter = quarters.Where(w => w.Value.Contains(now.Month)).Select(s => s.Key).First();

                switch (model.ReportPeriod ?? ReportPeriod.Yesterday)
                {
                    case ReportPeriod.Yesterday:
                        model.StartDate = now.Date.AddDays(-1);
                        model.EndDate = now.Date.AddTicks(-1);
                        break;

                    case ReportPeriod.Last7Days:
                        model.StartDate = now.Date.AddDays(-8);
                        model.EndDate = now.Date.AddTicks(-1);
                        break;

                    case ReportPeriod.MonthToDate:
                        model.StartDate = now.AddDays((now.Day - 1) * -1).Date;
                        model.EndDate = now.Date.AddTicks(-1);
                        break;

                    case ReportPeriod.LastMonth:
                        model.StartDate = now.AddDays((now.Day - 1) * -1).AddMonths(-1).Date;
                        model.EndDate = now.AddDays((now.Day - 1) * -1).Date.AddTicks(-1);
                        break;

                    case ReportPeriod.LastQuarter:
                        int lastQuarter = currentQuarter == 1 ? 4 : currentQuarter - 1;
                        year = currentQuarter == 1 ? year - 1 : year;

                        model.StartDate = new DateTime(year, quarters[lastQuarter][0], 1);
                        model.EndDate = new DateTime(year, quarters[currentQuarter][0], 1).Date.AddTicks(-1);

                        break;

                    case ReportPeriod.QuarterToDate:
                        model.StartDate = new DateTime(now.Year, quarters[currentQuarter][0], 1);
                        model.EndDate = now.Date.AddTicks(-1);
                        break;

                    case ReportPeriod.LastYear:
                        model.StartDate = new DateTime(year - 1, 1, 1);
                        model.EndDate = new DateTime(year, 1, 1).Date.AddTicks(-1);
                        break;

                    case ReportPeriod.YearToDate:
                    default:
                        model.StartDate = new DateTime(year, 1, 1);
                        model.EndDate = now.Date.AddTicks(-1);
                        break;
                }
            }
            else
            {
                // Make sure the dates cover all the time on selected days
                model.StartDate = (model.StartDate ?? DateTime.UtcNow).Date;
                model.EndDate = (model.EndDate ?? DateTime.UtcNow).Date.AddDays(1).AddTicks(-1); // the last moment of the day
            }
        }
    }
}
