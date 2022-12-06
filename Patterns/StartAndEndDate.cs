namespace Patterns
{
    /// <summary>
    /// Base class which need a start and end date.
    /// </summary>
    /// <remarks>Implements standard behavoir - start is always at 00:00 and end is always at 23:59 on the day.</remarks>
    public class StartAndEndDate
    {
        private DateTime _startDate;

        private DateTime _endDate;

        /// <summary>
        /// Always return the start date as the start of the day.
        /// </summary>
        public DateTime StartDate { get => _startDate.Date; set => _startDate = value; }

        /// <summary>
        /// Always return the end date as the last moment of the day.
        /// </summary>
        public DateTime EndDate { get => _endDate.Date.AddDays(1).AddTicks(-1); set => _endDate = value; }
    }
}
