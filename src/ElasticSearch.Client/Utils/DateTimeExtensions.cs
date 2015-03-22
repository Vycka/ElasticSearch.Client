using System;
using System.Globalization;

namespace ElasticSearch.Playground.Utils
{
    public static class DateTimeExtensions
    {
        #region Tomorrow & Yesterday

        /// <summary>
        /// Get start time of tomorrows day
        /// </summary>
        public static DateTime Tomorrow(this DateTime dateTime)
        {
            return dateTime.AddDays(1).StartOfDay();
        }

        /// <summary>
        /// Gets the start time of yesterday
        /// </summary>
        public static DateTime Yesterday(this DateTime dateTime)
        {
            return dateTime.AddDays(-1).StartOfDay();
        }

        #endregion

        #region End of Day/Week/Month/Year

        /// <summary>
        /// Returns the LAST possible time unit for provided DAY in dateTime
        /// </summary>
        public static DateTime EndOfDay(this DateTime dateTime)
        {
            return dateTime.Tomorrow().AddTicks(-1);
        }

        /// <summary>
        /// Returns the LAST possible time unit for provided WEEK in dateTime Based on CURRENT THREAD CULTURE
        /// </summary>
        public static DateTime EndOfWeek(this DateTime dateTime)
        {
            return dateTime.StartOfWeek().AddDays(7).AddTicks(-1);
        }

        /// <summary>
        /// Returns the LAST possible time unit for provided MONTH in dateTime
        /// </summary>
        public static DateTime EndOfMonth(this DateTime dateTime)
        {
            return dateTime.StartOfMonth().AddMonths(1).AddTicks(-1);
        }

        /// <summary>
        /// Returns the LAST possible time unit for provided YEAR in dateTime
        /// </summary>
        public static DateTime EndOfYear(this DateTime dateTime)
        {
            return dateTime.StartOfYear().AddYears(1).AddTicks(-1);
        }

        #endregion

        #region Start of Hour/Day/Week/Month/Year

        /// <summary>
        /// Returns the FIRST possible time unit for provided Hour in dateTime
        /// </summary>
        public static DateTime StartOfHour(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, 0, 0);
        }

        /// <summary>
        /// Returns the FIRST possible time unit for provided DAY in dateTime
        /// (Same as DateTime.Date)
        /// </summary>
        public static DateTime StartOfDay(this DateTime dateTime)
        {
            return dateTime.Date;
        }

        /// <summary>
        /// Returns the FIRST possible time unit for provided WEEK in dateTime Based on CURRENT THREAD CULTURE
        /// </summary>
        public static DateTime StartOfWeek(this DateTime dateTime)
        {
            var firstDayOfWeek = CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;

            return StartOfWeek(dateTime, firstDayOfWeek);
        }

        private static DateTime StartOfWeek(this DateTime dateTime, DayOfWeek startOfWeek)
        {
            int diff = dateTime.DayOfWeek - startOfWeek;
            if (diff < 0)
            {
                diff += 7;
            }

            DateTime mondayStart = dateTime.AddDays(-1 * diff).StartOfDay();

            return mondayStart;
        }


        /// <summary>
        /// Returns the FIRST possible time unit for provided MONTH in dateTime
        /// </summary>
        public static DateTime StartOfMonth(this DateTime dateTime)
        {
            DateTime startOfMonth = dateTime.AddDays(-dateTime.Day + 1).Date;

            return startOfMonth;
        }


        /// <summary>
        /// Returns the FIRST possible time unit for provided YEAR in dateTime
        /// </summary>
        public static DateTime StartOfYear(this DateTime dateTime)
        {
            var startOfYear = new DateTime(dateTime.Year, 1, 1);

            return startOfYear;
        }

        #endregion
    }
}
