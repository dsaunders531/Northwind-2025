// <copyright file="StartAndEndDate.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

namespace Patterns
{
    /// <summary>
    /// Base class which need a start and end date.
    /// </summary>
    /// <remarks>Implements standard behavoir - start is always at 00:00 and end is always at 23:59 on the day.</remarks>
    public class StartAndEndDate
    {
        public StartAndEndDate() { }

        /// <summary>
        /// Calculate the start and end days for a year.
        /// </summary>
        /// <param name="year"></param>
        public StartAndEndDate(int year)
        {
            startDate = new DateTime(year, 1, 1).Date;
            endDate = startDate.AddYears(1).AddTicks(-1);
        }

        /// <summary>
        /// Calculate the start and end days for a year and quarter.
        /// </summary>
        /// <param name="year"></param>
        /// <param name="quarter"></param>
        public StartAndEndDate(int year, int quarter)
        {
            int startmonth = quarter;//  1 = 1, 2 = 4, 3 = 7, 4 = 10            
            
            switch (startmonth)
            {
                case 1:
                    startmonth = 1;

                    break;
                case 2:
                    startmonth = 4;

                    break;
                case 3:
                    startmonth = 7;
                    break;
                case 4:
                default:
                    startmonth = 10;
                    break;
            }

            startDate = new DateTime(year, startmonth, 1).Date;
            endDate = startDate.AddMonths(3).AddTicks(-1);
        }

        private DateTime startDate;

        private DateTime endDate;

        /// <summary>
        /// Gets or sets always return the start date as the start of the day.
        /// </summary>
        public DateTime StartDate { get => startDate.Date; set => startDate = value; }

        /// <summary>
        /// Gets or sets always return the end date as the last moment of the day.
        /// </summary>
        public DateTime EndDate
        {
            get
            {
                return endDate < DateTime.MaxValue ? endDate.Date.AddDays(1).AddTicks(-1) : DateTime.MaxValue;
            }
            set => endDate = value;
        }
    }    
}
