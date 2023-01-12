namespace Patterns.Extensions
{
    /// <summary>
    /// Extension methods for DateTime structure.
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Convert a datetime to a json format datetime string.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToJsonDateTimeString(this DateTime value)
        {
            if (value.Kind != DateTimeKind.Utc)
            {
                value = new DateTime(value.Ticks, DateTimeKind.Utc);
            }

            return value.ToString("yyyy-MM-ddTHH:mm:ssZ");
        }

        /// <summary>
        /// Convert a datetime to a json format datetime string.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToJsonDateString(this DateTime value)
        {
            if (value.Kind != DateTimeKind.Utc)
            {
                value = new DateTime(value.Ticks, DateTimeKind.Utc);
            }

            return value.Date.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// Convert a datetime to a date web forms can understand.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToXFormDateTimeString(this DateTime value)
        {
            if (value.Kind != DateTimeKind.Utc)
            {
                value = new DateTime(value.Ticks, DateTimeKind.Utc);
            }

            return value.ToString("dd/MM/yyyy HH:mm:ss");
        }

        /// <summary>
        /// Convert a datetime to a date web forms can understand.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToXFormDateString(this DateTime value)
        {
            if (value.Kind != DateTimeKind.Utc)
            {
                value = new DateTime(value.Ticks, DateTimeKind.Utc);
            }

            return value.ToString("dd/MM/yyyy");
        }
        
        /// <summary>
        /// Convert a DateTime to a value MSSql Server can understand.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToSqlDateTime(this DateTime value)
        {
            return value.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// Convert a windows datetime to a unix epoch number.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double ToEpoch(this DateTime value)
        {
            // unix uses 1-Jan-1970 as base
            DateTime baseDate = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            return (value - baseDate).Ticks / TimeSpan.TicksPerSecond;
        }

        /// <summary>
        /// Convert a unix epoch to a datetime.
        /// </summary>
        /// <param name="epochTime"></param>
        /// <returns></returns>
        public static DateTime FromEpoch(this double epochTime)
        {
            return FromEpoch(Convert.ToInt64(Math.Floor(epochTime)));
        }

        /// <summary>
        /// Convert a unix epoch to a datetime.
        /// </summary>
        /// <param name="epochTime"></param>
        /// <returns></returns>
        public static DateTime FromEpoch(this long epochTime)
        {
            // unix uses 1-Jan-1970 as base
            DateTime baseDate = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            // Get the windows timespan as total seconds since 1-Jan-1970.
            TimeSpan totalSecondsSinceBase = new TimeSpan(epochTime * TimeSpan.TicksPerSecond);

            // Add the base and the seconds since together.
            return new DateTime(baseDate.Ticks + totalSecondsSinceBase.Ticks);
        }
    }
}
