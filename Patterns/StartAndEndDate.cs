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
        private DateTime startDate;

        private DateTime endDate;

        /// <summary>
        /// Gets or sets always return the start date as the start of the day.
        /// </summary>
        public DateTime StartDate { get => startDate.Date; set => startDate = value; }

        /// <summary>
        /// Gets or sets always return the end date as the last moment of the day.
        /// </summary>
        public DateTime EndDate { get => endDate.Date.AddDays(1).AddTicks(-1); set => endDate = value; }
    }
}
