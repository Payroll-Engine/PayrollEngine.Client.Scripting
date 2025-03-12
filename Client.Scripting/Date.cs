/* Date */

using System;
using System.Globalization;

namespace PayrollEngine.Client.Scripting;

/// <summary>Date specifications</summary>
public static class Date
{
    /// <summary>First month in year</summary>
    public static readonly int FirstMonthOfCalendarYear = 1;

    /// <summary>First day in month</summary>
    public static readonly int FirstDayOfMonth = 1;

    /// <summary>Number of months in a year</summary>
    public static readonly int MonthsInYear = 12;

    /// <summary>Last month in year</summary>
    public static readonly int LastMonthOfCalendarYear = MonthsInYear;

    /// <summary>Number of days in a week</summary>
    public static readonly int DaysInWeek = 7;

    /// <summary>Represents the smallest possible value of a time instant</summary>
    public static DateTime MinValue => DateTime.MinValue.ToUtc();

    /// <summary>Represents the largest possible value of a time instant</summary>
    public static DateTime MaxValue => DateTime.MaxValue.ToUtc();

    /// <summary>Gets a time instant that is set to the current date and time</summary>
    public static DateTime Now => DateTime.UtcNow;

    /// <summary>Gets a time instant that is set to the current day</summary>
    public static DateTime Today => Now.Date;

    /// <summary>Gets a time instant that is set to the next day</summary>
    public static readonly DateTime Tomorrow = Today.AddDays(1);

    /// <summary>Gets a time instant that is set to the previous day</summary>
    public static readonly DateTime Yesterday = Today.AddDays(-1);

    /// <summary>Get the year start date in UTC</summary>
    public static DateTime YearStart(int year) =>
        new(year, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    /// <summary>Get the year-end date in UTC</summary>
    public static DateTime YearEnd(int year) =>
        YearStart(year).AddYears(1).AddTicks(-1);

    /// <summary>Get the month start date in UTC</summary>
    public static DateTime MonthStart(int year, int month) =>
        new(year, month, 1, 0, 0, 0, DateTimeKind.Utc);

    /// <summary>Get the month end date in UTC</summary>
    public static DateTime MonthEnd(int year, int month) =>
        MonthStart(year, month).AddMonths(1).AddTicks(-1);

    /// <summary>Get the day start date and time in UTC</summary>
    public static DateTime DayStart(int year, int month, int day) =>
        new(year, month, day, 0, 0, 0, DateTimeKind.Utc);

    /// <summary>Get the day end date and time in UTC</summary>
    public static DateTime DayEnd(int year, int month, int day) =>
        DayStart(year, month, day).AddDays(1).AddTicks(-1);

    /// <summary>Get the minimum date</summary>
    public static DateTime Min(DateTime left, DateTime right) =>
        left < right ? left : right;

    /// <summary>Get the maximum date</summary>
    public static DateTime Max(DateTime left, DateTime right) =>
        left > right ? left : right;

    /// <summary>Get the minimum timespan</summary>
    public static TimeSpan Min(TimeSpan left, TimeSpan right) =>
        left < right ? left : right;

    /// <summary>Get the maximum timespan</summary>
    public static TimeSpan Max(TimeSpan left, TimeSpan right) =>
        left > right ? left : right;

    #region Convert

    /// <summary>Parse date time string</summary>
    /// <param name="dateValue">The date value</param>
    /// <returns>Valid date or null</returns>
    public static DateTime? Parse(string dateValue)
    {
        if (string.IsNullOrWhiteSpace(dateValue))
        {
            return null;
        }
        dateValue = dateValue.Trim('"');

        // predefined constants
        switch (dateValue.ToLower())
        {
            case "yesterday":
                return Yesterday;
            case "today":
                return Today;
            case "tomorrow":
                return Tomorrow;
            case "previousmonth":
                var previousMonth = Today.AddMonths(-1);
                return new(previousMonth.Year, previousMonth.Month, 1);
            case "month":
                var month = Today;
                return new(month.Year, month.Month, 1);
            case "nextmonth":
                var nextMonth = Today.AddMonths(1);
                return new(nextMonth.Year, nextMonth.Month, 1);
            case "previousyear":
                return new(Today.AddYears(-1).Year, 1, 1);
            case "year":
                return new(Today.Year, 1, 1);
            case "nextyear":
                return new(Today.AddYears(1).Year, 1, 1);
        }

        // offset
        if (dateValue.StartsWith("offset:", StringComparison.InvariantCultureIgnoreCase))
        {
            var offset = dateValue.Substring("offset:".Length);
            if (!string.IsNullOrWhiteSpace(offset))
            {
                var valueText = offset.Substring(0, offset.Length - 1).TrimStart('+');
                if (int.TryParse(valueText, out var value))
                {

                    switch (offset[^1])
                    {
                        // days
                        case 'd':
                            return Today.AddDays(value);
                        // weeks
                        case 'w':
                            return Today.AddDays(DaysInWeek * value);
                        // months
                        case 'm':
                            return Today.AddMonths(value);
                        // years
                        case 'y':
                            return Today.AddYears(value);
                    }
                }
            }
        }

        // date time parsing
        if (DateTime.TryParse(dateValue, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out var parameter))
        {
            return parameter;
        }

        return null;
    }

    #endregion

}