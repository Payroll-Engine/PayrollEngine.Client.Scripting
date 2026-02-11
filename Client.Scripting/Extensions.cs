/* Extensions */

using System;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Globalization;
using System.Collections.Generic;
//using PayrollEngine.Client.Scripting.Function;

namespace PayrollEngine.Client.Scripting;

/// <summary><see cref="Type">Type</see> extension methods</summary>
public static class TypeExtensions
{
    /// <summary>
    /// Determines whether the type is numeric.
    /// See https://stackoverflow.com/a/1750024
    /// </summary>
    /// <param name="type">The type</param>
    /// <returns>True for numeric types</returns>
    public static bool IsNumericType(this Type type)
    {
        switch (Type.GetTypeCode(type))
        {
            case TypeCode.Byte:
            case TypeCode.SByte:
            case TypeCode.UInt16:
            case TypeCode.UInt32:
            case TypeCode.UInt64:
            case TypeCode.Int16:
            case TypeCode.Int32:
            case TypeCode.Int64:
            case TypeCode.Decimal:
            case TypeCode.Double:
            case TypeCode.Single:
                return true;
            default:
                return false;
        }
    }
}

/// <summary><see cref="Nullable">Type</see> extension methods</summary>
public static class NullableExtensions
{
    /// <summary>Safe nullable boolean cast</summary>
    public static bool Safe(this bool? value, bool defaultValue = false) =>
        value ?? defaultValue;

    /// <summary>Safe nullable int cast</summary>
    public static int Safe(this int? value, int defaultValue = 0) =>
        value ?? defaultValue;

    /// <summary>Safe nullable decimal cast</summary>
    public static decimal Safe(this decimal? value, decimal defaultValue = 0) =>
        value ?? defaultValue;

    /// <summary>Safe nullable DateTime cast</summary>
    public static DateTime Safe(this DateTime? value, DateTime defaultValue = default) =>
        value ?? defaultValue;
}

/// <summary><see cref="string">String</see> extension methods</summary>
public static class StringExtensions
{

    #region Localization

    /// <summary>Gets the localized text</summary>
    /// <param name="culture">The culture name</param>
    /// <param name="localizations">The localizations</param>
    /// <param name="defaultValue">The default value</param>
    /// <returns>The localized text, the default value in case of absent language</returns>
    public static string GetLocalization(this string culture, Dictionary<string, string> localizations, string defaultValue = null)
    {
        if (localizations == null)
        {
            return defaultValue;
        }

        // culture
        culture ??= CultureInfo.CurrentCulture.Name;

        // direct localization
        if (localizations.TryGetValue(culture, out var localization))
        {
            return localization;
        }

        // base language localization
        var index = culture.IndexOf('-');
        if (index <= 0)
        {
            return defaultValue;
        }
        var baseCulture = culture.Substring(0, index);
        return localizations.GetValueOrDefault(baseCulture, defaultValue);
    }

    #endregion

    #region Modification

    /// <param name="source">The source value</param>
    extension(string source)
    {
        /// <summary>Ensures a start prefix</summary>
        /// <param name="prefix">The prefix to add</param>
        /// <returns>The string with prefix</returns>
        public string EnsureStart(string prefix)
        {
            if (!string.IsNullOrWhiteSpace(prefix))
            {
                if (string.IsNullOrWhiteSpace(source))
                {
                    source = prefix;
                }
                else if (!source.StartsWith(prefix))
                {
                    source = $"{prefix}{source}";
                }
            }
            return source;
        }

        /// <summary>Ensures a start prefix</summary>
        /// <param name="prefix">The prefix to add</param>
        /// <param name="comparison">The comparison culture</param>
        /// <returns>The string with prefix</returns>
        public string EnsureStart(string prefix, StringComparison comparison)
        {
            if (!string.IsNullOrWhiteSpace(prefix))
            {
                if (string.IsNullOrWhiteSpace(source))
                {
                    source = prefix;
                }
                else if (!source.StartsWith(prefix, comparison))
                {
                    source = $"{prefix}{source}";
                }
            }
            return source;
        }

        /// <summary>Ensures an ending suffix</summary>
        /// <param name="suffix">The suffix to add</param>
        /// <returns>The string with suffix</returns>
        public string EnsureEnd(string suffix)
        {
            if (!string.IsNullOrWhiteSpace(suffix))
            {
                if (string.IsNullOrWhiteSpace(source))
                {
                    source = suffix;
                }
                else if (!source.EndsWith(suffix))
                {
                    source = $"{source}{suffix}";
                }
            }
            return source;
        }

        /// <summary>Ensures an ending suffix</summary>
        /// <param name="suffix">The suffix to add</param>
        /// <param name="comparison">The comparison culture</param>
        /// <returns>The string with suffix</returns>
        public string EnsureEnd(string suffix, StringComparison comparison)
        {
            if (!string.IsNullOrWhiteSpace(suffix))
            {
                if (string.IsNullOrWhiteSpace(source))
                {
                    source = suffix;
                }
                else if (!source.EndsWith(suffix, comparison))
                {
                    source = $"{source}{suffix}";
                }
            }
            return source;
        }

        /// <summary>Remove prefix from string</summary>
        /// <param name="prefix">The prefix to remove</param>
        /// <returns>The string without suffix</returns>
        public string RemoveFromStart(string prefix)
        {
            if (!string.IsNullOrWhiteSpace(source) && !string.IsNullOrWhiteSpace(prefix) &&
                source.StartsWith(prefix))
            {
                source = source.Substring(prefix.Length);
            }
            return source;
        }

        /// <summary>Remove prefix from string</summary>
        /// <param name="prefix">The prefix to remove</param>
        /// <param name="comparison">The comparison culture</param>
        /// <returns>The string without the starting prefix</returns>
        public string RemoveFromStart(string prefix, StringComparison comparison)
        {
            if (!string.IsNullOrWhiteSpace(source) && !string.IsNullOrWhiteSpace(prefix) &&
                source.StartsWith(prefix, comparison))
            {
                source = source.Substring(prefix.Length);
            }
            return source;
        }

        /// <summary>Remove suffix from string</summary>
        /// <param name="suffix">The suffix to remove</param>
        /// <returns>The string without the ending suffix</returns>
        public string RemoveFromEnd(string suffix)
        {
            if (!string.IsNullOrWhiteSpace(source) && !string.IsNullOrWhiteSpace(suffix) &&
                source.EndsWith(suffix))
            {
                source = source.Substring(0, source.Length - suffix.Length);
            }
            return source;
        }

        /// <summary>Remove suffix from string</summary>
        /// <param name="suffix">The suffix to remove</param>
        /// <param name="comparison">The comparison culture</param>
        /// <returns>The string without the ending suffix</returns>
        public string RemoveFromEnd(string suffix, StringComparison comparison)
        {
            if (!string.IsNullOrWhiteSpace(source) && !string.IsNullOrWhiteSpace(suffix) &&
                source.EndsWith(suffix, comparison))
            {
                source = source.Substring(0, source.Length - suffix.Length);
            }
            return source;
        }

        /// <summary>Remove all special characters</summary>
        /// <returns>The source value without special characters</returns>
        public string RemoveSpecialCharacters()
        {
            if (string.IsNullOrWhiteSpace(source))
            {
                return source;
            }
            var builder = new StringBuilder();
            foreach (var c in source)
            {
                if (c is >= '0' and <= '9' or >= 'A' and <= 'Z' or >= 'a' and <= 'z')
                {
                    builder.Append(c);
                }
            }
            return builder.ToString();
        }
    }

    #endregion

    #region Csv

    /// <summary>Test for a CSV token</summary>
    /// <param name="source">The source value</param>
    /// <param name="token">The token to search</param>
    /// <param name="separator">The token separator</param>
    /// <returns>True if the token is available</returns>
    public static bool ContainsCsvToken(this string source, string token, char separator = ',')
    {
        if (string.IsNullOrWhiteSpace(source) || string.IsNullOrWhiteSpace(token))
        {
            return false;
        }
        return source.Split(separator, StringSplitOptions.RemoveEmptyEntries).Contains(token);
    }

    #endregion

    #region Attributes

    /// <summary>Prefix for text attribute fields</summary>
    public static readonly string TextAttributePrefix = "TA_";

    /// <summary>Prefix for date attribute fields</summary>
    public static readonly string DateAttributePrefix = "DA_";

    /// <summary>Prefix for numeric attribute fields</summary>
    public static readonly string NumericAttributePrefix = "NA_";

    /// <param name="attribute">The attribute</param>
    extension(string attribute)
    {
        /// <summary>To text attribute field name</summary>
        /// <returns>String starting uppercase</returns>
        public string ToTextAttributeField() => attribute.ToAttributeField(TextAttributePrefix);

        /// <summary>To date attribute field name</summary>
        /// <returns>String starting uppercase</returns>
        public string ToDateAttributeField() => attribute.ToAttributeField(DateAttributePrefix);

        /// <summary>To numeric attribute field name</summary>
        /// <returns>String starting uppercase</returns>
        public string ToNumericAttributeField() => attribute.ToAttributeField(NumericAttributePrefix);

        private string ToAttributeField(string prefix)
        {
            if (string.IsNullOrWhiteSpace(attribute))
            {
                throw new ArgumentException(nameof(attribute));
            }
            return attribute.EnsureStart(prefix);
        }
    }

    #endregion

    #region Case Relation

    /// <summary>The related case separator</summary>
    public static readonly char RelatedCaseSeparator = ':';

    /// <summary>The case field slot separator</summary>
    public static readonly char CaseFieldSlotSeparator = ':';

    /// <param name="caseRelation">The case relation</param>
    extension(string caseRelation)
    {
        /// <summary>Extract related cases from a case relation string, format is 'sourceCaseName:targetCaseName'</summary>
        /// <returns>The related cases a tuple: item1=source case, item2=target case</returns>
        public Tuple<string, string> ToRelatedCaseNames()
        {
            if (string.IsNullOrWhiteSpace(caseRelation))
            {
                return null;
            }

            var relatedCases = caseRelation.Split(RelatedCaseSeparator, StringSplitOptions.RemoveEmptyEntries);
            if (relatedCases.Length != 2)
            {
                throw new ArgumentException($"invalid case relation {caseRelation}, please use 'sourceCaseName:targetCaseName').");
            }
            return new(relatedCases[0], relatedCases[1]);
        }

        /// <summary>Extract related cases from a case relation string, format is 'sourceCaseName:targetCaseName'</summary>
        /// <param name="targetCaseName">The target case name</param>
        /// <returns>The related cases a tuple: item1=source case, item2=target case</returns>
        public string ToCaseRelationKey(string targetCaseName)
        {
            if (string.IsNullOrWhiteSpace(caseRelation))
            {
                return null;
            }
            if (string.IsNullOrWhiteSpace(targetCaseName))
            {
                return null;
            }

            return $"{caseRelation}{RelatedCaseSeparator}{targetCaseName}";
        }
    }

    #endregion

    #region Json

    /// <param name="json">The JSON string</param>
    extension(string json)
    {
        /// <summary>Convert string to JSON value</summary>
        /// <param name="defaultValue">The default value</param>
        /// <returns>The converted value</returns>
        public T ConvertJson<T>(T defaultValue = default) =>
            string.IsNullOrWhiteSpace(json) ? defaultValue : JsonSerializer.Deserialize<T>(json);

        /// <summary>Convert JSON value</summary>
        /// <param name="objectKey">The object key</param>
        /// <param name="defaultValue">The default value</param>
        /// <returns>The converted value</returns>
        public T ObjectValueJson<T>(string objectKey, T defaultValue = default)
        {
            if (string.IsNullOrWhiteSpace(objectKey))
            {
                throw new ArgumentException(nameof(objectKey));
            }

            var dictionary = json.ConvertJson<Dictionary<string, object>>();
            if (!dictionary.TryGetValue(objectKey, out var value))
            {
                return defaultValue;
            }

            if (value == null)
            {
                return defaultValue;
            }
            return (T)value;
        }
    }

    #endregion

}

/// <summary><see cref="IEnumerable{T}">IEnumerable</see> extension methods</summary>
public static class EnumerableExtensions
{
    /// <param name="source">The source collection</param>
    extension<T>(IEnumerable<T> source)
    {
        /// <summary>Test if all items are included</summary>
        /// <param name="test">The collection with the test items</param>
        /// <returns>True if all items of the test items are available in the source</returns>
        public bool ContainsAll(IEnumerable<T> test) =>
            test.All(source.Contains);

        /// <summary>Test if any item is included</summary>
        /// <param name="test">The collection with the test items</param>
        /// <returns>True if any item of the test items is available in the source</returns>
        public bool ContainsAny(IEnumerable<T> test) =>
            test.Any(source.Contains);
    }
}

/// <summary><see cref="int">Integer</see> extension methods</summary>
public static class IntExtensions
{
    /// <summary>Determines whether a value is within a range</summary>
    /// <param name="value">The value to test</param>
    /// <param name="min">The minimum value</param>
    /// <param name="max">The maximum value</param>
    /// <returns>True if the specified value is within minimum and maximum</returns>
    public static bool IsWithin(this int value, int min, int max) =>
        value >= min && value <= max;

    /// <summary>Determines whether a value is within a range</summary>
    /// <param name="value">The value to test</param>
    /// <param name="min">The minimum value</param>
    /// <param name="max">The maximum value</param>
    /// <returns>True if the specified value is within minimum and maximum</returns>
    public static bool IsWithin(this int? value, int min, int max) =>
        value.HasValue && value.Value.IsWithin(min, max);
}

/// <summary><see cref="decimal">Decimal</see> extension methods</summary>
public static class DecimalExtensions
{
    /// <summary>Determines whether a value is within a range</summary>
    /// <param name="value">The value to test</param>
    /// <param name="min">The minimum value</param>
    /// <param name="max">The maximum value</param>
    /// <returns>True if the specified value is within minimum and maximum</returns>
    public static bool IsWithin(this decimal value, decimal min, decimal max) =>
        value >= min && value <= max;

    /// <summary>Determines whether a value is within a range</summary>
    /// <param name="value">The value to test</param>
    /// <param name="min">The minimum value</param>
    /// <param name="max">The maximum value</param>
    /// <returns>True if the specified value is within minimum and maximum</returns>
    public static bool IsWithin(this decimal? value, decimal min, decimal max) =>
        value.HasValue && value.Value.IsWithin(min, max);

    /// <param name="value">The decimal number to truncate</param>
    extension(decimal value)
    {
        /// <summary>Returns the integral digits of the specified decimal, using a step size</summary>
        /// <param name="stepSize">The step size used to truncate</param>
        /// <returns>The result of d rounded toward zero, to the nearest whole number within the step size</returns>
        public decimal Truncate(int stepSize) =>
            value == 0 ? 0 : value - (value % stepSize);

        /// <summary>Rounds a decimal value up</summary>
        /// <param name="stepSize">The round step size</param>
        /// <returns>The up-rounded value</returns>
        public decimal RoundUp(decimal stepSize) =>
            value == 0 || stepSize == 0 ? value : Math.Ceiling(value / stepSize) * stepSize;

        /// <summary>Rounds a decimal value down</summary>
        /// <param name="stepSize">The round step size</param>
        /// <returns>The rounded value</returns>
        public decimal RoundDown(decimal stepSize) =>
            value == 0 || stepSize == 0 ? value : Math.Floor(value / stepSize) * stepSize;

        /// <summary>Rounds a decimal value wit predefined rounding type</summary>
        /// <param name="rounding">The rounding type</param>
        /// <returns>The rounded value</returns>
        public decimal Round(DecimalRounding rounding) =>
            rounding switch
            {
                DecimalRounding.Integer => decimal.Round(value),
                DecimalRounding.Half => value.RoundHalf(),
                DecimalRounding.Fifth => value.RoundFifth(),
                DecimalRounding.Tenth => value.RoundTenth(),
                DecimalRounding.Twentieth => value.RoundTwentieth(),
                DecimalRounding.Fiftieth => value.RoundFiftieth(),
                DecimalRounding.Hundredth => value.RoundHundredth(),
                _ => value
            };

        /// <summary>Rounds a decimal value to a one-half (e.g. 50 cents)</summary>
        /// <returns>The rounded value to one-half</returns>
        public decimal RoundHalf() => value.RoundPartOfOne(2);

        /// <summary>Rounds a decimal value to a one-fifth (e.g. 20 cents)</summary>
        /// <returns>The rounded value to one-fifth</returns>
        public decimal RoundFifth() => value.RoundPartOfOne(5);

        /// <summary>Rounds a decimal value to a one-tenth (e.g. 10 cents)</summary>
        /// <returns>The rounded value to one-tenth</returns>
        public decimal RoundTenth() => value.RoundPartOfOne(10);

        /// <summary>Rounds a decimal value to a one-twentieth (e.g. 5 cents)</summary>
        /// <returns>The rounded value to one-twentieth</returns>
        public decimal RoundTwentieth() => value.RoundPartOfOne(20);

        /// <summary>Rounds a decimal value to a one-fiftieth (e.g. 2 cents)</summary>
        /// <returns>The rounded value to one-fiftieth</returns>
        public decimal RoundFiftieth() => value.RoundPartOfOne(50);

        /// <summary>Rounds a decimal value to a one-hundredth (e.g. 1 cents)</summary>
        /// <returns>The rounded value to one-hundredth</returns>
        public decimal RoundHundredth() => value.RoundPartOfOne(100);

        /// <summary>Rounds a decimal value to a one-tenth</summary>
        /// <param name="divisor">The divisor factor</param>
        /// <returns>The rounded value to one-tenth</returns>
        public decimal RoundPartOfOne(int divisor) =>
            value == 0 ? 0 : Math.Round(value * divisor, MidpointRounding.AwayFromZero) / divisor;
    }
}

/// <summary><see cref="DateTime">DateTime</see> extension methods</summary>
public static class DateTimeExtensions
{
    /// <param name="moment">The moment to subtract from</param>
    extension(DateTime moment)
    {
        /// <summary>Returns the DateTime resulting from subtracting
        /// a time span to this DateTime</summary>
        /// <param name="timeSpan">The time span to subtract</param>
        public DateTime Subtract(TimeSpan timeSpan) =>
            moment.Add(timeSpan.Negate());

        /// <summary>Returns the DateTime resulting from subtracting
        /// a number of ticks to this DateTime</summary>
        /// <param name="ticks">The ticks to subtract</param>
        public DateTime SubtractTicks(long ticks) =>
            moment == DateTime.MinValue ? moment : moment.AddTicks(ticks * -1);

        /// <summary>Returns the DateTime resulting from subtracting
        /// a fractional number of seconds to this DateTime</summary>
        /// <param name="seconds">The seconds to subtract</param>
        public DateTime SubtractSeconds(double seconds) =>
            moment.AddSeconds(seconds * -1);

        /// <summary>Returns the DateTime resulting from subtracting
        /// a fractional number of minutes to this DateTime</summary>
        /// <param name="minutes">The minutes to subtract</param>
        public DateTime SubtractMinutes(double minutes) =>
            moment.AddMinutes(minutes * -1);

        /// <summary>Returns the DateTime resulting from subtracting
        /// a fractional number of hours to this DateTime</summary>
        /// <param name="hours">The hours to subtract</param>
        public DateTime SubtractHours(double hours) =>
            moment.AddHours(hours * -1);

        /// <summary>Returns the DateTime resulting from subtracting
        /// a fractional number of days to this DateTime</summary>
        /// <param name="days">The days to subtract</param>
        public DateTime SubtractDays(double days) =>
            moment.AddDays(days * -1);

        /// <summary>Returns the DateTime resulting from subtracting
        /// a number of months to this DateTime</summary>
        /// <param name="months">The months to subtract</param>
        public DateTime SubtractMonths(int months) =>
            moment.AddMonths(months * -1);

        /// <summary>Returns the DateTime resulting from subtracting
        /// a number of years to this DateTime</summary>
        /// <param name="years">The years to subtract</param>
        public DateTime SubtractYears(int years) =>
            moment.AddYears(years * -1);

        /// <summary>Format a compact date (removes empty time parts)</summary>
        /// <returns>The formatted period start date</returns>
        public string ToCompactString() => moment.IsMidnight() ? 
            moment.ToShortDateString() : 
            $"{moment.ToShortDateString()} {moment.ToShortTimeString()}";

        /// <summary>Format a period start date (removes empty time parts), using the current culture</summary>
        /// <returns>The formatted period start date</returns>
        public string ToPeriodStartString() => moment.ToCompactString();

        /// <summary>Format a period end date (removed empty time parts, and round last moment values), using the current culture</summary>
        /// <returns>The formatted period end date</returns>
        public string ToPeriodEndString() => moment.IsMidnight() || moment.IsLastMomentOfDay() ? 
            moment.ToShortDateString() : 
            $"{moment.ToShortDateString()} {moment.ToShortTimeString()}";

        /// <summary>Test if the date is in UTC</summary>
        /// <returns>True, date time is UTC</returns>
        public bool IsUtc() =>
            moment.Kind == DateTimeKind.Utc;

        /// <summary>Convert a date into the UTC value. Dates (without time part) are used without time adaption</summary>
        /// <returns>The UTC date time</returns>
        public DateTime ToUtc()
        {
            // ReSharper disable once ConvertSwitchStatementToSwitchExpression
            switch (moment.Kind)
            {
                case DateTimeKind.Utc:
                    // already utc
                    return moment;
                case DateTimeKind.Unspecified:
                    // specify kind
                    return DateTime.SpecifyKind(moment, DateTimeKind.Utc);
                case DateTimeKind.Local:
                    // convert to utc
                    return moment.ToUniversalTime();
                default:
                    throw new ArgumentOutOfRangeException($"Unknown date time kind {moment.Kind}.");
            }
        }

        /// <summary>Convert a date into the UTC valueDates (without time part) are used without time adaption</summary>
        /// <returns>The UTC date time</returns>
        public DateTime ToUtcTime()
        {
            // ReSharper disable once ConvertSwitchStatementToSwitchExpression
            switch (moment.Kind)
            {
                case DateTimeKind.Utc:
                    // already utc
                    return moment;
                case DateTimeKind.Unspecified:
                    // specify kind
                    return DateTime.SpecifyKind(moment, DateTimeKind.Utc);
                case DateTimeKind.Local:
                    // convert to utc
                    return moment.ToUniversalTime();
                default:
                    throw new ArgumentOutOfRangeException($"Unknown date time kind {moment.Kind}.");
            }
        }

        /// <summary>Convert a date into the UTC string value, using the current culture</summary>
        /// <returns>The UTC date time string</returns>
        public string ToUtcString() => moment.ToUtcString(CultureInfo.CurrentCulture);

        /// <summary>Convert a date into the UTC string value</summary>
        /// <param name="provider">The format provider</param>
        /// <returns>The UTC date time string</returns>
        public string ToUtcString(IFormatProvider provider) =>
            moment.ToUtcTime().ToString("o", provider);

        /// <summary>Round to hour, if it is the last tick from an hour</summary>
        /// <returns>Date of the next hour if the input is on the last tick on an hour, else the original value</returns>
        public DateTime RoundTickToHour()
        {
            if (moment >= Date.MaxValue)
            {
                return moment;
            }
            var nextTickDate = moment.AddTicks(1);
            // check for hour change
            return nextTickDate.Hour == moment.Hour ? moment : nextTickDate;
        }

        /// <summary>Get the year start date in UTC</summary>
        /// <returns>First moment of the year</returns>
        public DateTime YearStart() =>
            Date.YearStart(moment.Year);

        /// <summary>Get the year-end date in UTC</summary>
        /// <returns>Last moment of the year</returns>
        public DateTime YearEnd() =>
            Date.YearEnd(moment.Year);

        /// <summary>Get the month start date in UTC</summary>
        /// <returns>First moment of the month</returns>
        public DateTime MonthStart() =>
            Date.MonthStart(moment.Year, moment.Month);

        /// <summary>Get the month end date in UTC</summary>
        /// <returns>Last moment of the month</returns>
        public DateTime MonthEnd() =>
            Date.MonthEnd(moment.Year, moment.Month);

        /// <summary>Get the day start date and time in UTC</summary>
        /// <returns>First moment of the day</returns>
        public DateTime DayStart() =>
            Date.DayStart(moment.Year, moment.Month, moment.Day);

        /// <summary>Get the day end date and time in UTC</summary>
        /// <returns>Last moment of the day</returns>
        public DateTime DayEnd() =>
            Date.DayEnd(moment.Year, moment.Month, moment.Day);

        /// <summary>Test if a specific time moment is within a period</summary>
        /// <param name="start">The period start</param>
        /// <param name="end">The period end</param>
        /// <returns>True if the test is within start and end</returns>
        public bool IsWithin(DateTime start, DateTime end) =>
            start < end ?
                moment >= start && moment <= end :
                moment >= end && moment <= start;

        /// <summary>Test if a specific time moment is within a period</summary>
        /// <param name="period">The period </param>
        /// <returns>True if the test is within the period</returns>
        public bool IsWithin(DatePeriod period) =>
            period.IsWithin(moment);

        /// <summary>Test if a specific time moment is before a period</summary>
        /// <param name="period">The period to test</param>
        /// <returns>True, if the moment is before the period</returns>
        public bool IsBefore(DatePeriod period) =>
            period.IsBefore(moment);

        /// <summary>Test if a specific time moment is after a period</summary>
        /// <param name="period">The period to test</param>
        /// <returns>True, if the moment is after the period</returns>
        public bool IsAfter(DatePeriod period) =>
            period.IsAfter(moment);

        /// <summary>Test if a specific time moment is the first day of a period</summary>
        /// <param name="period">The period </param>
        /// <returns>True if the test day is the first period day</returns>
        public bool IsFirstDay(DatePeriod period) =>
            period.IsFirstDay(moment);

        /// <summary>Test if a specific time moment is the last day of a period</summary>
        /// <param name="period">The period </param>
        /// <returns>True if the test day is the last period day</returns>
        public bool IsLastDay(DatePeriod period) =>
            period.IsLastDay(moment);

        /// <summary>Test if a specific day is the first day of the year</summary>
        /// <returns>True if the test day is the first day in the year</returns>
        public bool IsFirstDayOfYear() =>
            moment.IsSameDay(new(moment.Year, 1, 1));

        /// <summary>Test if a specific time moment is the last day of the year</summary>
        /// <returns>True if the test day is the last day in the year</returns>
        public bool IsLastDayOfYear() =>
            moment.IsSameDay(new(moment.Year, Date.MonthsInYear, 1));

        /// <summary>Returns the number of days in the month of a specific date</summary>
        /// <returns>The number of days in for the specified moment /></returns>
        /// <returns>Last moment of the year</returns>
        public int DaysInMonth() =>
            DateTime.DaysInMonth(moment.Year, moment.Month);

        /// <summary>Returns the date month as enumeration</summary>
        /// <returns>The date month</returns>
        public Month Month() =>
            (Month)moment.Month;

        /// <summary>Test if a specific day is the first day of the month</summary>
        /// <returns>True if the test day is the first day of the month</returns>
        public bool IsFirstDayOfMonth() =>
            moment.IsFirstDayOfMonth((Month)moment.Month);

        /// <summary>Test if a specific day is the first day of the month</summary>
        /// <param name="month">The month to test</param>
        /// <returns>True if the test day is the first day of the month</returns>
        public bool IsFirstDayOfMonth(Month month) =>
            moment.IsSameDay(new(moment.Year, (int)month, 1));

        /// <summary>Test if a specific day is the last day of the month</summary>
        /// <param name="ignoreLeapYear">Ignore the leap year day</param>
        /// <returns>True if the test day is the last day of the month</returns>
        public bool IsLastDayOfMonth(bool ignoreLeapYear = false) => 
            moment.IsLastDayOfMonth(moment.Month(), ignoreLeapYear);

        /// <summary>Test if a specific day is the last day of the month</summary>
        /// <param name="month">The month to test</param>
        /// <param name="ignoreLeapYear">Ignore the leap year day</param>
        /// <returns>True if the test day is the last day of the month</returns>
        public bool IsLastDayOfMonth(Month month, bool ignoreLeapYear = false)
        {
            var lastMonthDay = new DateTime(moment.Year, (int)month, DateTime.DaysInMonth(moment.Year, (int)month));
            if (moment.IsSameDay(lastMonthDay))
            {
                return true;
            }
            // leap year
            if (ignoreLeapYear && DateTime.IsLeapYear(moment.Year))
            {
                // test february 28th
                return moment.IsSameDay(lastMonthDay.SubtractDays(1));
            }
            return false;
        }

        /// <summary>Get ISO 8601 week number of the year</summary>
        /// <returns>ISO 8601 week number</returns>
        public int GetIso8601WeekOfYear() => moment.GetIso8601WeekOfYear(CultureInfo.CurrentCulture);

        /// <summary>Get ISO 8601 week number of the year</summary>
        /// <param name="culture">The calendar culture</param>
        /// <returns>ISO 8601 week number</returns>
        public int GetIso8601WeekOfYear(CultureInfo culture)
        {
            System.DayOfWeek day = culture.Calendar.GetDayOfWeek(moment);
            if (day >= System.DayOfWeek.Monday && day <= System.DayOfWeek.Wednesday)
            {
                moment = moment.AddDays(3);
            }
            return culture.Calendar.GetWeekOfYear(
                moment, System.Globalization.CalendarWeekRule.FirstFourDayWeek, System.DayOfWeek.Monday);
        }
    }

    /// <summary>Get week start date by ISO 8601 week number of the year</summary>
    /// <param name="year">The year</param>
    /// <param name="weekOfYear">The ISO 8601 week number of the year</param>
    /// <returns>First day of the week</returns>
    public static DateTime FirstDayOfWeek(int year, int weekOfYear) =>
        FirstDayOfWeek(year, weekOfYear, CultureInfo.CurrentCulture);

    /// <summary>Get week start date by ISO 8601 week number of the year</summary>
    /// <param name="year">The year</param>
    /// <param name="weekOfYear">The ISO 8601 week number of the year</param>
    /// <param name="culture">The calendar culture</param>
    /// <returns>First day of the week</returns>
    public static DateTime FirstDayOfWeek(int year, int weekOfYear, CultureInfo culture)
    {
        DateTime jan1 = new(year, 1, 1);
        var daysOffset = (int)culture.DateTimeFormat.FirstDayOfWeek - (int)jan1.DayOfWeek;
        DateTime firstWeekDay = jan1.AddDays(daysOffset);
        var firstWeek = culture.Calendar.GetWeekOfYear(
            jan1, culture.DateTimeFormat.CalendarWeekRule, culture.DateTimeFormat.FirstDayOfWeek);
        if (firstWeek is <= 1 or >= 52 && daysOffset >= -3)
        {
            weekOfYear -= 1;
        }
        return firstWeekDay.AddDays(weekOfYear * 7);
    }

    /// <summary>Get weekend date by ISO 8601 week number of the year</summary>
    /// <param name="year">The year</param>
    /// <param name="weekOfYear">The ISO 8601 week number of the year</param>
    /// <returns>Last day of the week</returns>
    public static DateTime LastDayOfWeek(int year, int weekOfYear) =>
        LastDayOfWeek(year, weekOfYear, CultureInfo.CurrentCulture);

    /// <summary>Get weekend date by ISO 8601 week number of the year</summary>
    /// <param name="year">The year</param>
    /// <param name="weekOfYear">The ISO 8601 week number of the year</param>
    /// <param name="culture">The calendar culture</param>
    /// <returns>Last day of the week</returns>
    public static DateTime LastDayOfWeek(int year, int weekOfYear, CultureInfo culture) =>
        FirstDayOfWeek(year, weekOfYear, culture).AddDays(Date.DaysInWeek - 1);

    /// <param name="date">The date to start</param>
    extension(DateTime date)
    {
        /// <summary>Get the previous matching day</summary>
        /// <param name="dayOfWeek">Target day of week</param>
        /// <returns>The previous matching day</returns>
        public DateTime GetPreviousWeekDay(System.DayOfWeek dayOfWeek)
        {
            while (date.DayOfWeek != dayOfWeek)
            {
                date = date.AddDays(-1);
            }
            return date;
        }

        /// <summary>Get the next matching day</summary>
        /// <param name="dayOfWeek">Target day of week</param>
        /// <returns>The next matching day</returns>
        public DateTime GetNextWeekDay(System.DayOfWeek dayOfWeek)
        {
            while (date.DayOfWeek != dayOfWeek)
            {
                date = date.AddDays(1);
            }
            return date;
        }

        /// <summary>Test for working day</summary>
        /// <param name="days">Available days</param>
        /// <returns>True if date is a working day</returns>
        public bool IsDayOfWeek(IEnumerable<System.DayOfWeek> days) =>
            days.Contains(date.DayOfWeek);

        /// <summary>
        /// Test if the date is midnight
        /// </summary>
        /// <returns>True in case the moment is date</returns>
        public bool IsMidnight() =>
            // https://stackoverflow.com/questions/681435/what-is-the-best-way-to-determine-if-a-system-datetime-is-midnight
            date.TimeOfDay.Ticks == 0;

        /// <summary>Test if two dates are in the same year</summary>
        /// <param name="compare">The second date to test</param>
        /// <returns>Return true if year and mont of both dates is equal</returns>
        public bool IsSameYear(DateTime compare) =>
            date.Year == compare.Year;

        /// <summary>Test if two dates are in the same year and month</summary>
        /// <param name="compare">The second date to test</param>
        /// <returns>Return true if year and mont of both dates is equal</returns>
        public bool IsSameMonth(DateTime compare) =>
            date.IsSameYear(compare) && date.Month == compare.Month;

        /// <summary>Test if two dates are in the same day</summary>
        /// <param name="compare">The second date to test</param>
        /// <returns>Return true if both dates are in the same day</returns>
        public bool IsSameDay(DateTime compare) =>
            date.IsSameMonth(compare) && date.Day == compare.Day;

        /// <summary>Test if two dates are in the same hour</summary>
        /// <param name="compare">The second date to test</param>
        /// <returns>Return true if both dates are in the same hour</returns>
        public bool IsSameHour(DateTime compare) =>
            date.IsSameDay(compare) && date.Hour == compare.Hour;

        /// <summary>Calculates the current age, counting the completed years</summary>
        /// <returns>The current age</returns>
        public int Age() => date.Age(DateTime.UtcNow);

        /// <summary>Calculates the age at a specific moment, counting the completed years</summary>
        /// <param name="testMoment">The test moment</param>
        /// <returns>The age at the test moment</returns>
        public int Age(DateTime testMoment)
        {
            if (testMoment <= date)
            {
                throw new ArgumentOutOfRangeException(nameof(testMoment), "calculate age: birth-date must be older than test-date.");
            }
            var age = testMoment.Year - date.Year;
            // leap years
            if (date > testMoment.AddYears(-age))
            {
                age--;
            }
            return age;
        }

        /// <summary>Get the previous tick</summary>
        /// <returns>The previous tick</returns>
        public DateTime PreviousTick() =>
            date == DateTime.MinValue ? date : date.AddTicks(-1);

        /// <summary>Get the next tick</summary>
        /// <returns>The next tick</returns>
        public DateTime NextTick() =>
            date == DateTime.MaxValue ? date : date.AddTicks(1);

        /// <summary>Return the last moment of the day</summary>
        /// <returns><seealso cref="DateTime"/> from the latest moment in a day</returns>
        public DateTime LastMomentOfDay() =>
            date == DateTime.MaxValue ? date : date.Date.AddTicks(TimeSpan.TicksPerDay).PreviousTick();

        /// <summary>Test if the date is the last moment of the day</summary>
        /// <returns>True on the last moment of the day</returns>
        public bool IsLastMomentOfDay() =>
            Equals(date.LastMomentOfDay(), date);

        /// <summary>Rounds a date time up</summary>
        /// <param name="stepSize">Size of the rounding step</param>
        /// <returns>The rounded date time</returns>
        public DateTime RoundUp(TimeSpan stepSize)
        {
            var modTicks = date.Ticks % stepSize.Ticks;
            var delta = modTicks != 0 ? stepSize.Ticks - modTicks : 0;
            return delta != 0 ? new(date.Ticks + delta, date.Kind) : date;
        }

        /// <summary>Rounds a date time down</summary>
        /// <param name="stepSize">Size of the rounding step</param>
        /// <returns>The rounded date time</returns>
        public DateTime RoundDown(TimeSpan stepSize)
        {
            var delta = date.Ticks % stepSize.Ticks;
            return delta != 0 ? new(date.Ticks - delta, date.Kind) : date;
        }

        /// <summary>Rounds a date time to the nearest value</summary>
        /// <param name="stepSize">Size of the rounding step</param>
        /// <returns>The rounded date time</returns>
        public DateTime Round(TimeSpan stepSize)
        {
            var delta = date.Ticks % stepSize.Ticks;
            if (delta == 0)
            {
                return date;
            }
            var roundUp = delta > stepSize.Ticks / 2;
            var offset = roundUp ? stepSize.Ticks : 0;
            return new(date.Ticks + offset - delta, date.Kind);
        }
    }
}

/// <summary>Json extensions</summary>
public static class JsonExtensions
{
    /// <param name="valueKind">Kind of the value</param>
    extension(JsonValueKind valueKind)
    {
        /// <summary>Gets the type of the system</summary>
        /// <returns>The system type</returns>
        public Type GetSystemType()
        {
            switch (valueKind)
            {
                case JsonValueKind.Object:
                    return typeof(object);
                case JsonValueKind.Array:
                    return typeof(Array);
                case JsonValueKind.String:
                    return typeof(string);
                case JsonValueKind.Number:
                    return typeof(int);
                case JsonValueKind.True:
                case JsonValueKind.False:
                    return typeof(bool);
                default:
                    return null;
            }
        }

        /// <summary>Gets the type of the system</summary>
        /// <param name="value">The value, used to determine the numeric type</param>
        /// <returns>The system type</returns>
        public Type GetSystemType(object value)
        {
            if (valueKind != JsonValueKind.Number || value == null)
            {
                return valueKind.GetSystemType();
            }

            // integral types
            // https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/integral-numeric-types
            if (value is sbyte)
            {
                return typeof(sbyte);
            }
            if (value is byte)
            {
                return typeof(byte);
            }
            if (value is short)
            {
                return typeof(short);
            }
            if (value is ushort)
            {
                return typeof(ushort);
            }
            if (value is int)
            {
                return typeof(int);
            }
            if (value is uint)
            {
                return typeof(uint);
            }
            if (value is long)
            {
                return typeof(long);
            }
            if (value is ulong)
            {
                return typeof(ulong);
            }

            // floating point types
            // https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/floating-point-numeric-types
            if (value is float)
            {
                return typeof(float);
            }
            if (value is double)
            {
                return typeof(double);
            }
            if (value is decimal)
            {
                return typeof(decimal);
            }

            return typeof(int);
        }
    }

    /// <summary>Get the json element value</summary>
    /// <param name="jsonElement">The json element</param>
    /// <returns>The json element value</returns>
    public static object GetValue(this JsonElement jsonElement)
    {
        switch (jsonElement.ValueKind)
        {
            case JsonValueKind.String:
                return jsonElement.GetString();
            case JsonValueKind.Number:
                return jsonElement.GetDecimal();
            case JsonValueKind.True:
            case JsonValueKind.False:
                return jsonElement.GetBoolean();
            case JsonValueKind.Array:
                // recursive values
                return jsonElement.EnumerateArray().
                    Select(GetValue).ToList();
            case JsonValueKind.Object:
                // recursive values
                return jsonElement.EnumerateObject().
                    ToDictionary(item => item.Name, item => item.Value.GetValue());
            case JsonValueKind.Undefined:
            case JsonValueKind.Null:
                return null;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}

/// <summary><see cref="Dictionary{TKey,TValue}">Dictionary</see> extension methods</summary>
public static class DictionaryExtensions
{
    /// <param name="dictionary">The dictionary</param>
    extension(Dictionary<string, object> dictionary)
    {
        /// <summary>Get string/object dictionary value</summary>
        /// <param name="key">The value key</param>
        /// <param name="defaultValue">The default value</param>
        /// <returns>The dictionary value</returns>
        public object GetValue(string key, object defaultValue = null)
        {
            if (!dictionary.TryGetValue(key, out var value))
            {
                return defaultValue;
            }

            if (value is JsonElement jsonElement)
            {
                value = jsonElement.GetValue();
            }
            return value ?? defaultValue;
        }

        /// <summary>Get string/T dictionary value</summary>
        /// <param name="key">The value key</param>
        /// <param name="defaultValue">The default value</param>
        /// <returns>The dictionary value</returns>
        public T GetValue<T>(string key, T defaultValue = default) =>
            (T)Convert.ChangeType(dictionary.GetValue(key, (object)defaultValue), typeof(T));
    }

    /// <summary>Get value from a string/JSON-string dictionary</summary>
    /// <param name="dictionary">The dictionary</param>
    /// <param name="dictionaryKey">The dictionary key</param>
    /// <param name="objectKey">The object key</param>
    /// <param name="defaultValue">The default value</param>
    /// <returns>The dictionary value</returns>
    public static T GetValue<T>(this Dictionary<string, string> dictionary, string dictionaryKey,
        string objectKey, T defaultValue = default) =>
        dictionary.ContainsKey(dictionaryKey) ?
            dictionary[dictionaryKey].ObjectValueJson(objectKey, defaultValue) ?? defaultValue :
            defaultValue;
}

/// <summary><see cref="TimeSpan">TimeSpan</see> extension methods</summary>
public static class TimeSpanExtensions
{
    /// <param name="timeSpan">The time span</param>
    extension(TimeSpan timeSpan)
    {
        /// <summary>Rounds a time interval up</summary>
        /// <param name="stepSize">Size of the rounding step</param>
        /// <returns>The rounded time interval</returns>
        public TimeSpan RoundUp(TimeSpan stepSize)
        {
            var modTicks = timeSpan.Ticks % stepSize.Ticks;
            var delta = modTicks != 0 ? stepSize.Ticks - modTicks : 0;
            return delta != 0 ? new(timeSpan.Ticks + delta) : timeSpan;
        }

        /// <summary>Rounds a time interval down</summary>
        /// <param name="stepSize">Size of the rounding step</param>
        /// <returns>The rounded time interval</returns>
        public TimeSpan RoundDown(TimeSpan stepSize)
        {
            var delta = timeSpan.Ticks % stepSize.Ticks;
            return delta != 0 ? new(timeSpan.Ticks - delta) : timeSpan;
        }

        /// <summary>Rounds a time interval to the nearest value</summary>
        /// <param name="stepSize">Size of the rounding step</param>
        /// <returns>The rounded time interval</returns>
        public TimeSpan Round(TimeSpan stepSize)
        {
            var delta = timeSpan.Ticks % stepSize.Ticks;
            if (delta == 0)
            {
                return timeSpan;
            }
            var roundUp = delta > stepSize.Ticks / 2;
            var offset = roundUp ? stepSize.Ticks : 0;
            return new(timeSpan.Ticks + offset - delta);
        }
    }
}

/// <summary><see cref="DatePeriod">DatePeriod</see> extension methods</summary>
public static class DatePeriodExtensions
{
    /// <param name="period">The period</param>
    extension(DatePeriod period)
    {
        /// <summary>Test if a specific time moment is before this period</summary>
        /// <param name="testMoment">The moment to test</param>
        /// <returns>True, if the moment is before this period</returns>
        public bool IsBefore(DateTime testMoment) =>
            testMoment < period.Start;

        /// <summary>Test if a specific time period is before this period</summary>
        /// <param name="testPeriod">The period to test</param>
        /// <returns>True, if the period is before this period</returns>
        public bool IsBefore(DatePeriod testPeriod) =>
            testPeriod.End < period.Start;

        /// <summary>Test if a specific time moment is after this period</summary>
        /// <param name="testMoment">The moment to test</param>
        /// <returns>True, if the moment is after this period</returns>
        public bool IsAfter(DateTime testMoment) =>
            testMoment > period.End;

        /// <summary>Test if a specific time period is after this period</summary>
        /// <param name="testPeriod">The period to test</param>
        /// <returns>True, if the period is after this period</returns>
        public bool IsAfter(DatePeriod testPeriod) =>
            testPeriod.Start > period.End;

        /// <summary>Test if a specific time moment is within the period, including open periods</summary>
        /// <param name="testMoment">The moment to test</param>
        /// <returns>True, if the moment is within this period</returns>
        public bool IsWithin(DateTime testMoment) =>
            testMoment.IsWithin(period.Start, period.End);

        /// <summary>Test if a specific time period is within the period, including open periods</summary>
        /// <param name="testPeriod">The period to test</param>
        /// <returns>True, if the test period is within this period</returns>
        public bool IsWithin(DatePeriod testPeriod) =>
            period.IsWithin(testPeriod.Start) && period.IsWithin(testPeriod.End);

        /// <summary>Test if a specific time moment is within or before the period, including open periods</summary>
        /// <param name="testMoment">The moment to test</param>
        /// <returns>True, if the moment is within or before this period</returns>
        public bool IsWithinOrBefore(DateTime testMoment) =>
            testMoment <= period.End;

        /// <summary>Test if a specific time moment is within or after the period, including open periods</summary>
        /// <param name="testMoment">The moment to test</param>
        /// <returns>True, if the moment is within or after this period</returns>
        public bool IsWithinOrAfter(DateTime testMoment) =>
            testMoment >= period.Start;

        /// <summary>Test if period is overlapping this period</summary>
        /// <param name="testPeriod">The period to test</param>
        /// <returns>True, if the period is overlapping this period</returns>
        public bool IsOverlapping(DatePeriod testPeriod) =>
            testPeriod.Start < period.End && period.Start < testPeriod.End;

        /// <summary>Get the intersection of a date period with this period</summary>
        /// <param name="intersectPeriod">The period to intersect</param>
        /// <returns>The intersecting date period, null if no intersection is present</returns>
        public DatePeriod Intersect(DatePeriod intersectPeriod)
        {
            if (!period.IsOverlapping(intersectPeriod))
            {
                return null;
            }
            return new(
                new(Math.Max(period.Start.Ticks, intersectPeriod.Start.Ticks)),
                new(Math.Min(period.End.Ticks, intersectPeriod.End.Ticks)));
        }

        /// <summary>Split date period by splitting dates</summary>
        /// <param name="splitMoments">The moments used to split</param>
        /// <returns>The splitting periods</returns>
        public List<DatePeriod> Split(List<DateTime> splitMoments)
        {
            // ensure unique moments, ordered from oldest to newest
            var moments = new HashSet<DateTime>(splitMoments).OrderBy(x => x);

            var splitPeriods = new List<DatePeriod>();
            foreach (var moment in moments)
            {
                // no intersection
                if (!period.IsWithin(moment))
                {
                    continue;
                }

                var periodStart = splitPeriods.Any() ?
                    // in-between period
                    splitPeriods.Last().End.NextTick() :
                    // first period
                    period.Start;
                splitPeriods.Add(new(periodStart, moment.PreviousTick()));
            }

            // last period
            if (splitPeriods.Any())
            {
                var last = splitPeriods.Last();
                if (last.End != period.End)
                {
                    splitPeriods.Add(new(last.End.NextTick(), period.End));
                }
            }
            return splitPeriods;
        }

        /// <summary>Get period days</summary>
        public List<DateTime> Days()
        {
            var days = new List<DateTime>();
            var current = period.Start.Date;
            while (current <= period.End.Date)
            {
                days.Add(current);
                current = current.AddDays(1);
            }
            return days;
        }

        /// <summary>Test if a specific time moment is the first day of a period</summary>
        /// <param name="test">The moment to test</param>
        /// <returns>True if the test day is the first period day</returns>
        public bool IsFirstDay(DateTime test) =>
            test.IsSameDay(period.Start);

        /// <summary>Test if a specific time moment is the last day of a period</summary>
        /// <param name="test">The moment to test</param>
        /// <returns>True if the test day is the last period day</returns>
        public bool IsLastDay(DateTime test) =>
            test.IsSameDay(period.End);

        /// <summary>Test if a date is the period start day</summary>
        /// <param name="test">The date to test</param>
        /// <returns>True, if the test date is the period start day</returns>
        public bool IsStartDay(DateTime test) =>
            period.HasStart && period.Start.IsSameDay(test);

        /// <summary>Test if the period start is on a specific weekday</summary>
        /// <param name="testDay">The weekday to test</param>
        /// <returns>True, if the period start day matches the weekday</returns>
        public bool IsStartDayOfWeek(System.DayOfWeek testDay) =>
            period.HasStart && period.Start.DayOfWeek == testDay;

        /// <summary>Test if the period start is a monday</summary>
        /// <returns>True, if the period start day is a monday</returns>
        public bool IsStartMonday() => period.IsStartDayOfWeek(System.DayOfWeek.Monday);

        /// <summary>Test if the period start is a tuesday</summary>
        /// <returns>True, if the period start day is a tuesday</returns>
        public bool IsStartTuesday() => period.IsStartDayOfWeek(System.DayOfWeek.Tuesday);

        /// <summary>Test if the period start is a wednesday</summary>
        /// <returns>True, if the period start day is a Wednesday</returns>
        public bool IsStartWednesday() => period.IsStartDayOfWeek(System.DayOfWeek.Wednesday);

        /// <summary>Test if the period start is a thursday</summary>
        /// <returns>True, if the period start day is a thursday</returns>
        public bool IsStartThursday() => period.IsStartDayOfWeek(System.DayOfWeek.Thursday);

        /// <summary>Test if the period start is a friday</summary>
        /// <returns>True, if the period start day is a friday</returns>
        public bool IsStartFriday() => period.IsStartDayOfWeek(System.DayOfWeek.Friday);

        /// <summary>Test if the period start is a saturday</summary>
        /// <returns>True, if the period start day is a saturday</returns>
        public bool IsStartSaturday() => period.IsStartDayOfWeek(System.DayOfWeek.Saturday);

        /// <summary>Test if the period start is a sunday</summary>
        /// <returns>True, if the period start day is a sunday</returns>
        public bool IsStartSunday() => period.IsStartDayOfWeek(System.DayOfWeek.Sunday);

        /// <summary>Test if a date is the period end day</summary>
        /// <param name="test">The date to test</param>
        /// <returns>True, if the test date is the period end day</returns>
        public bool IsEndDay(DateTime test) =>
            period.HasEnd && period.End.IsSameDay(test);

        /// <summary>Test if the period end is on a specific weekday</summary>
        /// <param name="testDay">The weekday to test</param>
        /// <returns>True, if the period end day matches the weekday</returns>
        public bool IsEndDayOfWeek(System.DayOfWeek testDay) =>
            period.HasEnd && period.End.DayOfWeek == testDay;

        /// <summary>Test if the period end is a monday</summary>
        /// <returns>True, if the period end day is a monday</returns>
        public bool IsEndMonday() => period.IsEndDayOfWeek(System.DayOfWeek.Monday);

        /// <summary>Test if the period end is a tuesday</summary>
        /// <returns>True, if the period end day is a tuesday</returns>
        public bool IsEndTuesday() => period.IsEndDayOfWeek(System.DayOfWeek.Tuesday);

        /// <summary>Test if the period end is a wednesday</summary>
        /// <returns>True, if the period end day is a Wednesday</returns>
        public bool IsEndWednesday() => period.IsEndDayOfWeek(System.DayOfWeek.Wednesday);

        /// <summary>Test if the period end is a thursday</summary>
        /// <returns>True, if the period end day is a thursday</returns>
        public bool IsEndThursday() => period.IsEndDayOfWeek(System.DayOfWeek.Thursday);

        /// <summary>Test if the period end is a friday</summary>
        /// <returns>True, if the period end day is a friday</returns>
        public bool IsEndFriday() => period.IsEndDayOfWeek(System.DayOfWeek.Friday);

        /// <summary>Test if the period end is a saturday</summary>
        /// <returns>True, if the period end day is a saturday</returns>
        public bool IsEndSaturday() => period.IsEndDayOfWeek(System.DayOfWeek.Saturday);

        /// <summary>Test if the period end is a sunday</summary>
        /// <returns>True, if the period end day is a sunday</returns>
        public bool IsEndSunday() => period.IsEndDayOfWeek(System.DayOfWeek.Sunday);

        /// <summary>Calculate the count of working days</summary>
        /// <param name="days">Available days</param>
        /// <returns>The number of working days</returns>
        public int GetWorkingDaysCount(IEnumerable<System.DayOfWeek> days)
        {
            var dayCount = 0;
            var date = period.Start.Date;
            var dayOfWeeks = days as System.DayOfWeek[] ?? days.ToArray();
            while (date <= period.End.Date)
            {
                if (date.IsDayOfWeek(dayOfWeeks))
                {
                    dayCount++;
                }
                date = date.AddDays(1);
            }
            return dayCount;
        }
    }

    /// <param name="datePeriods">The date periods</param>
    extension(IEnumerable<DatePeriod> datePeriods)
    {
        /// <summary>Total duration of all date periods</summary>
        /// <returns>Accumulated total duration</returns>
        public TimeSpan TotalDuration()
        {
            var duration = TimeSpan.Zero;
            return datePeriods.Aggregate(duration, (current, period) => current.Add(period.Duration));
        }

        /// <summary>Test if any period is overlapping another period</summary>
        /// <returns>True, if the period is overlapping this period</returns>
        public bool HasOverlapping()
        {
            var periodList = datePeriods.ToList();
            for (var current = 1; current < periodList.Count; current++)
            {
                for (var remain = current + 1; remain < periodList.Count; remain++)
                {
                    if (periodList[remain].IsOverlapping(periodList[current]))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>Test if a specific time moment is within any date period</summary>
        /// <param name="testMoment">The moment to test</param>
        /// <returns>True, if the moment is within this period</returns>
        public bool IsWithinAny(DateTime testMoment) =>
            datePeriods.Any(periodValue => periodValue.IsWithin(testMoment));

        /// <summary>Test if a specific time period is within any date period</summary>
        /// <param name="testPeriod">The period to test</param>
        /// <returns>True, if the test period is within this period</returns>
        public bool IsWithinAny(DatePeriod testPeriod) =>
            datePeriods.Any(periodValue => periodValue.IsWithin(testPeriod));

        /// <summary>Get limits period, from the earliest start to the latest end</summary>
        /// <returns>Date period including all date periods, an anytime period for empty collections</returns>
        public DatePeriod Limits()
        {
            DateTime? start = null;
            DateTime? end = null;
            foreach (var datePeriod in datePeriods)
            {
                // start
                if (!start.HasValue || datePeriod.Start < start.Value)
                {
                    start = datePeriod.Start;
                }
                // end
                if (!end.HasValue || datePeriod.End > end.Value)
                {
                    end = datePeriod.End;
                }
            }
            return new(start, end);
        }

        /// <summary>Get all intersections of a date period with any date period</summary>
        /// <param name="intersectPeriod">The period to intersect</param>
        /// <returns>List of intersecting date periods</returns>
        public List<DatePeriod> Intersections(DatePeriod intersectPeriod)
        {
            var intersections = new List<DatePeriod>();
            foreach (var datePeriod in datePeriods)
            {
                var intersection = datePeriod.Intersect(intersectPeriod);
                if (intersection != null)
                {
                    intersections.Add(intersection);
                }
            }
            return intersections;
        }
    }
}

/// <summary><see cref="HourPeriod">TimePeriod</see> extension methods</summary>
public static class HourPeriodExtensions
{
    /// <param name="period">The period</param>
    extension(HourPeriod period)
    {
        /// <summary>Test if a specific time moment is before this period</summary>
        /// <param name="test">The moment to test</param>
        /// <returns>True, if the moment is before this period</returns>
        public bool IsBefore(decimal test) =>
            test < period.Start;

        /// <summary>Test if a specific time period is before this period</summary>
        /// <param name="testPeriod">The period to test</param>
        /// <returns>True, if the period is before this period</returns>
        public bool IsBefore(HourPeriod testPeriod) =>
            testPeriod.End < period.Start;

        /// <summary>Test if a specific time moment is after this period</summary>
        /// <param name="test">The moment to test</param>
        /// <returns>True, if the moment is after this period</returns>
        public bool IsAfter(decimal test) =>
            test > period.End;

        /// <summary>Test if a specific time period is after this period</summary>
        /// <param name="testPeriod">The period to test</param>
        /// <returns>True, if the period is after this period</returns>
        public bool IsAfter(HourPeriod testPeriod) =>
            testPeriod.Start > period.End;

        /// <summary>Test if a specific time moment is within the period, including open periods</summary>
        /// <param name="test">The moment to test</param>
        /// <returns>True, if the moment is within this period</returns>
        public bool IsWithin(decimal test) =>
            test.IsWithin(period.Start, period.End);

        /// <summary>Test if a specific time period is within the period, including open periods</summary>
        /// <param name="testPeriod">The period to test</param>
        /// <returns>True, if the test period is within this period</returns>
        public bool IsWithin(HourPeriod testPeriod) => 
            period.IsWithin(testPeriod.Start) && period.IsWithin(testPeriod.End);

        /// <summary>Test if a specific time moment is within or before the period, including open periods</summary>
        /// <param name="test">The moment to test</param>
        /// <returns>True, if the moment is within or before this period</returns>
        public bool IsWithinOrBefore(decimal test) =>
            test <= period.End;

        /// <summary>Test if a specific time moment is within or after the period, including open periods</summary>
        /// <param name="test">The moment to test</param>
        /// <returns>True, if the moment is within or after this period</returns>
        public bool IsWithinOrAfter(decimal test) =>
            test >= period.Start;

        /// <summary>Test if period is overlapping this period</summary>
        /// <param name="testPeriod">The period to test</param>
        /// <returns>True, if the period is overlapping this period</returns>
        public bool IsOverlapping(HourPeriod testPeriod) =>
            testPeriod.Start < period.End && period.Start < testPeriod.End;

        /// <summary>Get the intersection of a time period with this period</summary>
        /// <param name="intersectPeriod">The period to intersect</param>
        /// <returns>The intersecting time period, null if no intersection is present</returns>
        public HourPeriod Intersect(HourPeriod intersectPeriod)
        {
            if (!period.IsOverlapping(intersectPeriod))
            {
                return null;
            }
            return new(
                Math.Max(period.Start, intersectPeriod.Start),
                Math.Min(period.End, intersectPeriod.End));
        }

        /// <summary>Get the hours of intersection</summary>
        /// <param name="intersectPeriod">The period to intersect</param>
        /// <returns>The intersecting duration in hours, 0 if no intersection is present</returns>
        public decimal IntersectHours(HourPeriod intersectPeriod)
        {
            var intersect = period.Intersect(intersectPeriod);
            return intersect == null ? 0 : (decimal)intersect.Duration.TotalHours;
        }
    }

    /// <param name="timePeriods">The time periods</param>
    extension(IEnumerable<HourPeriod> timePeriods)
    {
        /// <summary>Total duration of all time periods</summary>
        /// <returns>Accumulated total duration</returns>
        public TimeSpan TotalDuration()
        {
            var duration = TimeSpan.Zero;
            return timePeriods.Aggregate(duration, (current, period) => current.Add(period.Duration));
        }

        /// <summary>Test if any period is overlapping another period</summary>
        /// <returns>True, if the period is overlapping this period</returns>
        public bool HasOverlapping()
        {
            var periodList = timePeriods.ToList();
            for (var current = 1; current < periodList.Count; current++)
            {
                for (var remain = current + 1; remain < periodList.Count; remain++)
                {
                    if (periodList[remain].IsOverlapping(periodList[current]))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>Test if a specific time moment is within any time period</summary>
        /// <param name="test">The moment to test</param>
        /// <returns>True, if the moment is within this period</returns>
        public bool IsWithinAny(decimal test) =>
            timePeriods.Any(periodValue => periodValue.IsWithin(test));

        /// <summary>Test if a specific time period is within any time period</summary>
        /// <param name="testPeriod">The period to test</param>
        /// <returns>True, if the test period is within this period</returns>
        public bool IsWithinAny(HourPeriod testPeriod) =>
            timePeriods.Any(periodValue => periodValue.IsWithin(testPeriod));

        /// <summary>Get limits period, from the earliest start to the latest end</summary>
        /// <returns>Time period including all time periods, an anytime period for empty collections</returns>
        public HourPeriod Limits()
        {
            decimal? start = null;
            decimal? end = null;
            foreach (var timePeriod in timePeriods)
            {
                // start
                if (!start.HasValue || timePeriod.Start < start.Value)
                {
                    start = timePeriod.Start;
                }
                // end
                if (!end.HasValue || timePeriod.End > end.Value)
                {
                    end = timePeriod.End;
                }
            }
            return new(start, end);
        }

        /// <summary>Get all intersections of a time period with any time period</summary>
        /// <param name="intersectPeriod">The period to intersect</param>
        /// <returns>List of intersecting time periods</returns>
        public List<HourPeriod> Intersections(HourPeriod intersectPeriod)
        {
            var intersections = new List<HourPeriod>();
            foreach (var timePeriod in timePeriods)
            {
                var intersection = timePeriod.Intersect(intersectPeriod);
                if (intersection != null)
                {
                    intersections.Add(intersection);
                }
            }
            return intersections;
        }
    }
}

/// <summary><see cref="CaseValue">CaseValue</see> extension methods</summary>
public static class CaseValueExtensions
{
    /// <param name="caseValue">The case value</param>
    extension(CaseValue caseValue)
    {
        /// <summary>Extract date periods</summary>
        /// <returns>Accumulated total duration</returns>
        public DatePeriod Period() =>
            new(caseValue.Start, caseValue.End);

        /// <summary>Convert the case value to custom type</summary>
        /// <param name="defaultValue">The default value</param>
        /// <returns>Accumulated total duration</returns>
        public T ValueAs<T>(T defaultValue = default)
        {
            if (caseValue == null || caseValue.Value == null)
            {
                return defaultValue;
            }
            return (T)caseValue.ValueAs(typeof(T));
        }

        /// <summary>Convert the case value to custom type</summary>
        /// <param name="type">Target type</param>
        /// <returns>Accumulated total duration</returns>
        public object ValueAs(Type type) =>
            Convert.ChangeType(caseValue.Value, type);

        /// <summary>Convert case value to string/></summary>
        public string ToString() => caseValue.ValueAs<string>();

        /// <summary>Convert case value to int/></summary>
        public int ToInt() => caseValue.ValueAs<int>();

        /// <summary>Convert case value to nullable int/></summary>
        public int? ToNullableInt() => caseValue.ValueAs<int?>();

        /// <summary>Convert case value to decimal/></summary>
        public decimal ToDecimal() => caseValue.ValueAs<decimal>();

        /// <summary>Convert case value to decimal</summary>
        /// <param name="rounding">The rounding type</param>
        /// <summary>Decimal case value</summary>
        public decimal ToDecimal(DecimalRounding rounding) => caseValue.ToDecimal().Round(rounding);

        /// <summary>Convert case value to nullable decimal/></summary>
        public decimal? ToNullableDecimal() => caseValue.ValueAs<decimal?>();

        /// <summary>Convert case value to nullable decimal/></summary>
        /// <param name="rounding">The rounding type</param>
        /// <summary>Nullable decimal case value</summary>
        public decimal? ToNullableDecimal(DecimalRounding rounding) => caseValue.ToNullableDecimal()?.Round(rounding);

        /// <summary>Convert case value to date time/></summary>
        public DateTime ToDateTime() => caseValue.ValueAs<DateTime>();

        /// <summary>Convert case value to nullable date time/></summary>
        public DateTime? ToNullableDateTime() => caseValue.ValueAs<DateTime?>();
    }

    /// <param name="periodValues">The case period values</param>
    extension(IEnumerable<CaseValue> periodValues)
    {
        /// <summary>Extract date periods</summary>
        /// <returns>Accumulated total duration</returns>
        public IEnumerable<DatePeriod> Periods()
        {
            foreach (var periodValue in periodValues)
            {
                yield return periodValue.Period();
            }
        }

        /// <summary>Get first matching period containing the test date</summary>
        /// <param name="date">The date of the case value</param>
        /// <returns>Accumulated total duration</returns>
        public CaseValue CaseValueWithin(DateTime date)
        {
            foreach (var caseValue in periodValues)
            {
                var period = caseValue.Period();
                if (period.IsWithin(date))
                {
                    return caseValue;
                }
            }
            return null;
        }

        /// <summary>Get case period values grouped by value</summary>
        /// <returns>Case period values grouped by value</returns>
        public IEnumerable<IGrouping<PayrollValue, CaseValue>> GroupByValue() =>
            periodValues.GroupBy(x => x.Value);

        /// <summary>Total duration of all time periods</summary>
        /// <returns>Accumulated total duration</returns>
        public TimeSpan TotalDuration() =>
            periodValues.Periods().TotalDuration();

        /// <summary>Get all intersections of a date period with any date period</summary>
        /// <param name="intersectPeriod">The period to intersect</param>
        /// <returns>List of intersecting date periods</returns>
        public List<CaseValue> Intersections(DatePeriod intersectPeriod) =>
            [.. periodValues.Where(periodValue => periodValue.Period().IsOverlapping(intersectPeriod))];

        /// <summary>Get case period values matching a period predicate</summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>List of case period value matching the predicate</returns>
        public IEnumerable<CaseValue> WherePeriod(Func<DatePeriod, bool> predicate)
        {
            if (periodValues == null)
            {
                throw new ArgumentNullException(nameof(periodValues));
            }

            foreach (var periodValue in periodValues)
            {
                if (predicate(periodValue.Period()))
                {
                    yield return periodValue;
                }
            }
        }

        /// <summary>Get date periods with start on specific weekdays</summary>
        /// <param name="weekdays">The week days</param>
        /// <returns>List of intersecting date periods</returns>
        public IEnumerable<CaseValue> WhereStart(params System.DayOfWeek[] weekdays) =>
            periodValues.WherePeriod(x => weekdays.Contains(x.Start.DayOfWeek));

        /// <summary>Get date periods with start is not on specific weekdays</summary>
        /// <param name="weekdays">The week days</param>
        /// <returns>List of intersecting date periods</returns>
        public IEnumerable<CaseValue> WhereStartNot(params System.DayOfWeek[] weekdays) =>
            periodValues.WherePeriod(x => !weekdays.Contains(x.Start.DayOfWeek));

        /// <summary>Get date periods with end on specific weekdays</summary>
        /// <param name="weekdays">The week days</param>
        /// <returns>List of intersecting date periods</returns>
        public IEnumerable<CaseValue> WhereEnd(params System.DayOfWeek[] weekdays) =>
            periodValues.WherePeriod(x => weekdays.Contains(x.End.DayOfWeek));

        /// <summary>Get date periods with end is not on specific weekdays</summary>
        /// <param name="weekdays">The week days</param>
        /// <returns>List of intersecting date periods</returns>
        public IEnumerable<CaseValue> WhereEndNot(params System.DayOfWeek[] weekdays) =>
            periodValues.WherePeriod(x => !weekdays.Contains(x.End.DayOfWeek));
    }
}

/// <summary><see cref="PayrollValue">PayrollValue</see> extension methods</summary>
public static class PayrollValueExtensions
{
    /// <param name="payrollValue">The payroll value</param>
    extension(PayrollValue payrollValue)
    {
        /// <summary>Convert payroll case value to custom type</summary>
        /// <param name="defaultValue">The default value</param>
        public T ValueAs<T>(T defaultValue = default) =>
            (T)payrollValue.Convert(typeof(T), defaultValue);

        /// <summary>Convert payroll case value to custom type</summary>
        /// <param name="type">Target type</param>
        /// <param name="defaultValue">The default value</param>
        public object Convert(Type type, object defaultValue = null)
        {
            if (payrollValue?.Value == null)
            {
                return defaultValue;
            }

            if (type == typeof(string))
            {
                return (string)payrollValue.Value;
            }
            if (type == typeof(bool))
            {
                return (bool)payrollValue.Value;
            }
            if (type == typeof(bool?))
            {
                return (bool?)payrollValue.Value;
            }
            if (type == typeof(int))
            {
                return (int)payrollValue.Value;
            }
            if (type == typeof(int?))
            {
                return (int?)payrollValue.Value;
            }
            if (type == typeof(decimal))
            {
                return (decimal)payrollValue.Value;
            }
            if (type == typeof(decimal?))
            {
                return (decimal?)payrollValue.Value;
            }
            if (type == typeof(DateTime))
            {
                return (DateTime)payrollValue.Value;
            }
            if (type == typeof(DateTime?))
            {
                return (DateTime?)payrollValue.Value;
            }
            return payrollValue.Value;
        }

        /// <summary>Convert payroll case value to string</summary>
        public string ToString() => payrollValue.ValueAs<string>();

        /// <summary>Convert payroll case value to bool</summary>
        public bool ToBoolean() => payrollValue.ValueAs<bool>();

        /// <summary>Convert payroll case value to nullable bool</summary>
        public bool? ToNullableBoolean() => payrollValue.ValueAs<bool?>();

        /// <summary>Convert payroll case value to int</summary>
        public int ToInt() => payrollValue.ValueAs<int>();

        /// <summary>Convert payroll case value to nullable int</summary>
        public int? ToNullableInt() => payrollValue.ValueAs<int?>();

        /// <summary>Convert payroll case value to decimal</summary>
        public decimal ToDecimal() => payrollValue.ValueAs<decimal>();

        /// <summary>Convert payroll case value to decimal</summary>
        /// <param name="rounding">The rounding type</param>
        /// <summary>Decimal payroll case value</summary>
        public decimal ToDecimal(DecimalRounding rounding) => payrollValue.ToDecimal().Round(rounding);

        /// <summary>Convert payroll case value to nullable decimal/></summary>
        public decimal? ToNullableDecimal() => payrollValue.ValueAs<decimal?>();

        /// <summary>Convert payroll case value to nullable decimal/></summary>
        /// <param name="rounding">The rounding type</param>
        /// <summary>Nullable decimal payroll case value</summary>
        public decimal? ToNullableDecimal(DecimalRounding rounding) => payrollValue.ToNullableDecimal()?.Round(rounding);

        /// <summary>Convert payroll case value to date time/></summary>
        public DateTime ToDateTime() => payrollValue.ValueAs<DateTime>();

        /// <summary>Convert payroll case value to nullable date time/></summary>
        public DateTime? ToNullableDateTime() => payrollValue.ValueAs<DateTime?>();
    }

    /// <summary>Extract all decimal values</summary>
    /// <param name="values">The payroll values</param>
    /// <returns>A list containing all decimal values</returns>
    public static IEnumerable<decimal> GetDecimalValues(this IEnumerable<PayrollValue> values) =>
        values.Where(x => x.Value is decimal).Select(x => (decimal)x.Value);
}

/// <summary><see cref="PeriodValue">PeriodValue</see> extension methods</summary>
public static class PeriodValueExtensions
{
    /// <param name="values">The period payroll values</param>
    extension(IEnumerable<PeriodValue> values)
    {
        /// <summary>Get the earliest start date</summary>
        /// <returns>The earliest start date</returns>
        public DateTime? GetPeriodStart()
        {
            DateTime? start = null;
            foreach (var value in values)
            {
                if (!start.HasValue || value.Start < start)
                {
                    start = value.Start;
                }
            }
            return start;
        }

        /// <summary>Get the latest end date</summary>
        /// <returns>The latest end date</returns>
        public DateTime? GetPeriodEnd()
        {
            DateTime? end = null;
            foreach (var value in values)
            {
                if (!end.HasValue || value.End > end)
                {
                    end = value.End;
                }
            }
            return end;
        }

        /// <summary>Get the date period over all values, from the earliest start to the latest end</summary>
        /// <returns>The overall period, anytime in case of an empty source</returns>
        public DatePeriod GetPeriod()
        {
            var periodValues = values as PeriodValue[] ?? values.ToArray();
            return new(periodValues.GetPeriodStart(), periodValues.GetPeriodEnd());
        }

        /// <summary>Extract all date start dates</summary>
        /// <returns>A date period start dates</returns>
        public IEnumerable<DateTime> GetPeriodStarts() =>
            values?.Where(x => x.Start.HasValue).Select(x => x.Start.Value);

        /// <summary>Extract all date end dates</summary>
        /// <returns>A date period end dates</returns>
        public IEnumerable<DateTime> GetPeriodEnds() =>
            values?.Where(x => x.End.HasValue).Select(x => x.End.Value);

        /// <summary>Extract all date periods</summary>
        /// <returns>A list containing all date periods</returns>
        public IEnumerable<DatePeriod> GetPeriods() =>
            values?.Where(x => x.Period != null).Select(x => x.Period);

        /// <summary>Extract all date period durations</summary>
        /// <returns>A list containing all date period durations</returns>
        public IEnumerable<TimeSpan> GetDurations() =>
            values?.Where(x => x.Period != null).Select(x => x.Period.Duration);

        /// <summary>Summarize the total duration from all date period durations</summary>
        /// <returns>Total duration from all periods, an empty time span on empty collection</returns>
        public TimeSpan TotalDuration() =>
            // summarize from all durations the time span ticks
            values != null ? new(values.GetDurations().Sum(ts => ts.Ticks)) : TimeSpan.Zero;

        /// <summary>Total days considering the value as factor</summary>
        /// <returns>Total days by value as factor</returns>
        public decimal TotalDaysByValue() => values.TotalDaysByValue(true);

        /// <summary>Total days considering the value as factor</summary>
        /// <param name="includeEndDay">Include the end day</param>
        /// <returns>Total days by value as factor</returns>
        public decimal TotalDaysByValue(bool includeEndDay)
        {
            decimal totalDays = 0;
            if (values != null)
            {
                foreach (var value in values)
                {
                    if (value.Value is decimal decimalValue)
                    {
                        var days = value.Period.Duration.Days;
                        if (days > 0)
                        {
                            // end day handling
                            if (includeEndDay && value.End.HasValue && value.End.Value.IsMidnight())
                            {
                                days++;
                            }
                            totalDays += days * decimalValue;
                        }
                    }
                }
            }
            return totalDays;
        }
    }

    /// <param name="periodValue">The case value</param>
    extension(PeriodValue periodValue)
    {
        /// <summary>Convert case period value to custom type</summary>
        /// <param name="defaultValue">The default value</param>
        /// <returns>Accumulated total duration</returns>
        public T ValueAs<T>(T defaultValue = default)
        {
            if (periodValue == null || periodValue.Value == null)
            {
                return defaultValue;
            }
            return (T)periodValue.ValueAs(typeof(T));
        }

        /// <summary>Convert case period value to custom type</summary>
        /// <param name="type">Target type</param>
        /// <returns>Accumulated total duration</returns>
        public object ValueAs(Type type) =>
            Convert.ChangeType(periodValue.Value, type);

        /// <summary>Convert case period value to string/></summary>
        public string ToString() => periodValue.ValueAs<string>();

        /// <summary>Convert case period value to int/></summary>
        public int ToInt() => periodValue.ValueAs<int>();

        /// <summary>Convert case period value to nullable int/></summary>
        public int? ToNullableInt() => periodValue.ValueAs<int?>();

        /// <summary>Convert case period value to decimal/></summary>
        public decimal ToDecimal() => periodValue.ValueAs<decimal>();

        /// <summary>Convert case period value to decimal</summary>
        /// <param name="rounding">The rounding type</param>
        /// <summary>Decimal case period value</summary>
        public decimal ToDecimal(DecimalRounding rounding) => periodValue.ToDecimal().Round(rounding);

        /// <summary>Convert case period value to nullable decimal/></summary>
        public decimal? ToNullableDecimal() => periodValue.ValueAs<decimal?>();

        /// <summary>Convert case period value to nullable decimal/></summary>
        /// <param name="rounding">The rounding type</param>
        /// <summary>Nullable decimal case period value</summary>
        public decimal? ToNullableDecimal(DecimalRounding rounding) => periodValue.ToNullableDecimal()?.Round(rounding);

        /// <summary>Convert case period value to date time/></summary>
        public DateTime ToDateTime() => periodValue.ValueAs<DateTime>();

        /// <summary>Convert case period value to nullable date time/></summary>
        public DateTime? ToNullableDateTime() => periodValue.ValueAs<DateTime?>();
    }
}

/// <summary>Payroll results extension methods</summary>
public static class PayrollResultsExtensions
{
    /// <param name="results">The case value results</param>
    extension(IEnumerable<CaseValueResult> results)
    {
        /// <summary>Get case value result values</summary>
        /// <returns>Case value result values</returns>
        public List<PayrollValue> Values() =>
            results.Select(x => x.Value).ToList();

        /// <summary>Get summary of decimal case value results</summary>
        /// <returns>Case value decimal result values summary</returns>
        public decimal Sum() =>
            results.Where(x => x.Value.Value is decimal).Select(x => (decimal)x.Value.Value).Sum();
    }

    /// <param name="results">The collector results</param>
    extension(IEnumerable<CollectorResult> results)
    {
        /// <summary>Get collector result values</summary>
        /// <returns>Collector result values</returns>
        public List<decimal> Values() =>
            results.Select(x => x.Value).ToList();

        /// <summary>Get summary of collector results</summary>
        /// <returns>Collector result values summary</returns>
        public decimal Sum() => results.Values().Sum();
    }

    /// <param name="results">The collector custom results</param>
    extension(IEnumerable<CollectorCustomResult> results)
    {
        /// <summary>Get collector custom result values</summary>
        /// <returns>Collector custom result values</returns>
        public List<decimal> Values() =>
            results.Select(x => x.Value).ToList();

        /// <summary>Get summary of collector custom results</summary>
        /// <returns>Collector custom results result values summary</returns>
        public decimal Sum() => results.Values().Sum();
    }

    /// <param name="results">The wage type results</param>
    extension(IEnumerable<WageTypeResult> results)
    {
        /// <summary>Get wage type result values</summary>
        /// <returns>Wage type result values</returns>
        public List<decimal> Values() =>
            results.Select(x => x.Value).ToList();

        /// <summary>Get summary of wage type results</summary>
        /// <returns>Wage type result values summary</returns>
        public decimal Sum() => results.Values().Sum();
    }

    /// <param name="results">The wage type custom results</param>
    extension(IEnumerable<WageTypeCustomResult> results)
    {
        /// <summary>Get wage type custom results values</summary>
        /// <returns>Wage type custom result values</returns>
        public List<decimal> Values() =>
            results.Select(x => x.Value).ToList();

        /// <summary>Get summary of wage type custom results</summary>
        /// <returns>Wage type result values summary</returns>
        public decimal Sum() => results.Values().Sum();
    }
}

/// <summary>Value type extension methods</summary>
public static class ValueTypeExtensions
{
    /// <param name="valueType">The value type</param>
    extension(ValueType valueType)
    {
        /// <summary>Test if value type is a bool</summary>
        /// <returns>True for boolean value types</returns>
        public bool IsBoolean() =>
            valueType == ValueType.Boolean;

        /// <summary>Test if value type is a string</summary>
        /// <returns>True for string value types</returns>
        public bool IsString() =>
            valueType is ValueType.String or
                ValueType.WebResource;

        /// <summary>Test if value type is a date time</summary>
        /// <returns>True for date time value types</returns>
        public bool IsDateTime() =>
            valueType is ValueType.Date or
                ValueType.DateTime;

        /// <summary>Test if value type is a number</summary>
        /// <returns>True for number value types</returns>
        public bool IsNumber() => 
            valueType.IsInteger() || valueType.IsDecimal();

        /// <summary>Test if value type is an integer</summary>
        /// <returns>True for integer value types</returns>
        public bool IsInteger() =>
            valueType is ValueType.Integer or
                ValueType.Weekday or
                ValueType.Month;

        /// <summary>Test if value type is a decimal number</summary>
        /// <returns>True for decimal number value types</returns>
        public bool IsDecimal() =>
            valueType is ValueType.Decimal or
                ValueType.Money or
                ValueType.Percent or
                ValueType.NumericBoolean;

        /// <summary>Get the data type</summary>
        /// <returns>The data type</returns>
        public Type GetDataType()
        {
            if (valueType.IsString())
            {
                return typeof(string);
            }
            if (valueType.IsDateTime())
            {
                return typeof(DateTime);
            }
            if (valueType.IsInteger())
            {
                return typeof(int);
            }
            if (valueType.IsDecimal())
            {
                return typeof(decimal);
            }
            if (valueType.IsBoolean())
            {
                return typeof(bool);
            }
            if (valueType == ValueType.None)
            {
                return typeof(DBNull);
            }
            throw new ScriptException($"Unknown value type {valueType}.");
        }
    }

    /// <summary>Get the value type</summary>
    /// <param name="value">The value</param>
    /// <returns>The value type</returns>
    public static ValueType GetValueType(this object value)
    {
        if (value is string)
        {
            return ValueType.String;
        }
        if (value is int)
        {
            return ValueType.Integer;
        }
        if (value is decimal or float)
        {
            return ValueType.Decimal;
        }
        if (value is bool)
        {
            return ValueType.Boolean;
        }
        if (value is DateTime)
        {
            return ValueType.DateTime;
        }
        return ValueType.None;
    }

    /// <summary>Convert json by value type</summary>
    /// <param name="valueType">The value type</param>
    /// <param name="json">The json to convert</param>
    /// <returns>Object value</returns>
    public static object JsonToValue(this ValueType valueType, string json)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            throw new ArgumentException(nameof(json));
        }

        if (valueType.IsInteger())
        {
            return string.IsNullOrWhiteSpace(json) ? 0 : JsonSerializer.Deserialize<int>(json);
        }
        if (valueType.IsDecimal())
        {
            return string.IsNullOrWhiteSpace(json) ? 0 : JsonSerializer.Deserialize<decimal>(json);
        }
        if (valueType.IsString())
        {
            return string.IsNullOrWhiteSpace(json) ? null :
                json.StartsWith('"') ? JsonSerializer.Deserialize<string>(json) : json;
        }
        if (valueType.IsDateTime())
        {
            return string.IsNullOrWhiteSpace(json) ? default :
                json.StartsWith('"') ?
                    JsonSerializer.Deserialize<DateTime>(json) :
                DateTime.Parse(json, null, DateTimeStyles.AdjustToUniversal);
        }
        if (valueType.IsBoolean())
        {
            return !string.IsNullOrWhiteSpace(json) && JsonSerializer.Deserialize<bool>(json);
        }
        if (valueType == ValueType.None)
        {
            return null;
        }
        throw new ScriptException($"unknown value type {valueType}.");
    }
}

/// <summary><see cref="Tuple">Tuple</see> extension methods (internal usage)</summary>
public static class TupleExtensions
{
    /// <param name="values">The tuple values</param>
    extension(List<Tuple<DateTime, DateTime?, object>> values)
    {
        /// <summary>Convert tuple values to case period values</summary>
        /// <returns>The period values</returns>
        public List<PeriodValue> TupleToPeriodValues()
        {
            var periodValues = new List<PeriodValue>();
            if (values != null)
            {
                foreach (var value in values)
                {
                    if (value != null)
                    {
                        periodValues.Add(new(value.Item1, value.Item2, value.Item3));
                    }
                }
            }
            return periodValues;
        }

        /// <summary>Convert tuple values to a case value</summary>
        /// <param name="caseFieldName">The case field name</param>
        /// <returns>The case values</returns>
        public CasePayrollValue TupleToCasePeriodValue(string caseFieldName)
        {
            if (string.IsNullOrWhiteSpace(caseFieldName))
            {
                throw new ArgumentException(nameof(caseFieldName));
            }
            return new(caseFieldName, values.Select(x => new PeriodValue(x.Item1, x.Item2, x.Item3)));
        }
    }

    /// <summary>Convert tuple values to case value</summary>
    /// <param name="value">The tuple value</param>
    /// <returns>The case period values</returns>
    public static CaseValue TupleToCaseValue(this Tuple<string, DateTime, Tuple<DateTime?, DateTime?>, object, DateTime?, List<string>, Dictionary<string, object>> value)
    {
        if (value == null)
        {
            return null;
        }
        return new(value.Item1, value.Item2, value.Item3.Item1, value.Item3.Item2, new(value.Item4), value.Item5, value.Item6, value.Item7);
    }

    /// <summary>Convert tuple values to case values</summary>
    /// <param name="values">The tuple values</param>
    /// <returns>The case period values</returns>
    public static List<CaseValue> TupleToCaseValues(this List<Tuple<string, DateTime, 
        Tuple<DateTime?, DateTime?>, object, DateTime?, List<string>, Dictionary<string, object>>> values)
    {
        var caseValues = new List<CaseValue>();
        if (values != null)
        {
            foreach (var value in values)
            {
                if (value != null)
                {
                    caseValues.Add(new(value.Item1, value.Item2, value.Item3.Item1, 
                        value.Item3.Item2, new(value.Item4), value.Item5, value.Item6, value.Item7));
                }
            }
        }
        return caseValues;
    }

    /// <summary>Convert tuple values to a case value dictionary</summary>
    /// <param name="values">The values</param>
    /// <returns>The case values grouped by case field name</returns>
    public static CasePayrollValueDictionary TupleToCaseValuesDictionary(this Dictionary<string,
        List<Tuple<DateTime, DateTime?, DateTime?, object>>> values)
    {
        var caseValues = new Dictionary<string, CasePayrollValue>();
        foreach (var value in values)
        {
            var periodValues = value.Value.Select(x => new PeriodValue(x.Item2, x.Item3, x.Item4));
            caseValues.Add(value.Key, new(value.Key, periodValues));
        }
        return new(caseValues);
    }

    /// <summary>Convert tuple values to a collector result</summary>
    /// <param name="values">The tuple values</param>
    /// <returns>The collector results</returns>
    public static List<CollectorResult> TupleToCollectorResults(this List<Tuple<string, 
        Tuple<DateTime, DateTime>, decimal, List<string>, Dictionary<string, object>>> values) =>
    [
        ..values.Select(x => new CollectorResult
        {
            CollectorName = x.Item1,
            Start = x.Item2.Item1,
            End = x.Item2.Item2,
            Value = x.Item3,
            Tags = x.Item4,
            Attributes = x.Item5
        })
    ];

    /// <summary>Convert tuple values to a collector custom result</summary>
    /// <param name="values">The tuple values</param>
    /// <returns>The collector custom results</returns>
    public static List<CollectorCustomResult> TupleToCollectorCustomResults(
        this List<Tuple<string, string, Tuple<DateTime, DateTime>, decimal, List<string>, Dictionary<string, object>>> values) =>
    [
        ..values.Select(x => new CollectorCustomResult
        {
            CollectorName = x.Item1,
            Source = x.Item2,
            Start = x.Item3.Item1,
            End = x.Item3.Item2,
            Value = x.Item4,
            Tags = x.Item5,
            Attributes = x.Item6
        })
    ];

    /// <summary>Convert tuple values to a wage type result</summary>
    /// <param name="values">The tuple values</param>
    /// <returns>The wage type results</returns>
    public static List<WageTypeResult> TupleToWageTypeResults
        (this List<Tuple<decimal, string, Tuple<DateTime, DateTime>, decimal, List<string>, Dictionary<string, object>>> values) =>
    [
        ..values.Select(x => new WageTypeResult
        {
            WageTypeNumber = x.Item1,
            WageTypeName = x.Item2,
            Start = x.Item3.Item1,
            End = x.Item3.Item2,
            Value = x.Item4,
            Tags = x.Item5,
            Attributes = x.Item6
        })
    ];

    /// <summary>Convert tuple values to a wage type custom result</summary>
    /// <param name="values">The tuple values</param>
    /// <returns>The wage type custom results</returns>
    public static List<WageTypeCustomResult> TupleToWageTypeCustomResults(
        this List<Tuple<decimal, string, string, Tuple<DateTime, DateTime>, decimal, List<string>, Dictionary<string, object>>> values) =>
    [
        ..values.Select(x => new WageTypeCustomResult
        {
            WageTypeNumber = x.Item1,
            Name = x.Item2,
            Source = x.Item3,
            Start = x.Item4.Item1,
            End = x.Item4.Item2,
            Value = x.Item5,
            Tags = x.Item6,
            Attributes = x.Item7
        })
    ];

    /// <summary>Convert tuple values to a wage type custom result</summary>
    /// <param name="brackets">The lookup brackets</param>
    /// <returns>The lookup brackets</returns>
    public static List<LookupRangeBracket> TupleToLookupRangeBracketList(
        List<Tuple<string, string, decimal, decimal, decimal?>> brackets) =>
    [
        ..brackets.Select(x => new LookupRangeBracket
        {
            Key = x.Item1,
            Value = x.Item2,
            RangeStart = x.Item3,
            RangeEnd = x.Item4,
            RangeValue = x.Item5
        })
    ];
}