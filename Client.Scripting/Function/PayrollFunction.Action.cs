/* PayrollFunction.Action */

using System;
using System.Linq;
using System.Collections.Generic;

namespace PayrollEngine.Client.Scripting.Function;

/// <summary>Payroll function</summary>
public partial class PayrollFunction
{
    /// <summary>None value</summary>
    protected dynamic None => null;

    /// <summary>Null value</summary>
    protected dynamic Null => null;

    #region Case Value

    /// <summary>Test for case field value</summary>
    /// <param name="field">Object field name</param>
    /// <returns>True if field exists</returns>
    [ActionParameter("field", "The case field name", [StringType])]
    [PayrollAction("HasFieldValue", "Test for case field value", "Case")]
    public bool HasFieldValue(string field) =>
        GetCaseValueType(field) != null;

    /// <summary>Get case filed value</summary>
    /// <param name="field">Object field name</param>
    [ActionParameter("field", "The case field name", [StringType])]
    [PayrollAction("GetFieldValue", "Get case field value", "Case")]
    public ActionValue GetFieldValue(string field) =>
        new(GetCaseValue<object>(field));

    #endregion

    #region Lookup

    /// <summary>Test for lookup value by key or range value</summary>
    [ActionParameter("lookup", "The lookup name", [StringType])]
    [ActionParameter("keyOrRangeValue", "The lookup key or range value")]
    [ActionParameter("field", "The JSON value field name (optional)")]
    [PayrollAction("HasLookupValue", "Test for lookup value by key", "Lookup")]
    public bool HasLookupValue(string lookup, ActionValue keyOrRangeValue, string field = null) =>
        GetLookupValue(lookup, keyOrRangeValue, field).HasValue;

    /// <summary>Test for lookup value by key and range value</summary>
    [ActionParameter("lookup", "The lookup name", [StringType])]
    [ActionParameter("key", "The lookup key", [StringType])]
    [ActionParameter("field", "The JSON value field name (optional)")]
    [PayrollAction("HasLookupValue", "Test for lookup value by key and range value", "Lookup")]
    public bool HasLookupValue(string lookup, ActionValue key, ActionValue rangeValue, string field = null) =>
        GetLookupValue(lookup, key, rangeValue, field).HasValue;

    /// <summary>Get lookup value by key or range value</summary>
    [ActionParameter("lookup", "The lookup name", [StringType])]
    [ActionParameter("keyOrRangeValue", "The lookup key or range value")]
    [ActionParameter("field", "The JSON value field name (optional)")]
    [PayrollAction("GetLookupValue", "Get lookup value by key or range value", "Lookup")]
    public ActionValue GetLookupValue(string lookup, ActionValue keyOrRangeValue, string field = null)
    {
        // range value
        if (keyOrRangeValue.IsNumeric)
        {
            // basic range lookup value
            if (string.IsNullOrWhiteSpace(field))
            {
                return new(GetRangeLookup<object>(
                    lookupName: lookup,
                    rangeValue: keyOrRangeValue));

            }
            // object field range lookup value
            return new(GetRangeObjectLookup<object>(
                lookupName: lookup,
                rangeValue: keyOrRangeValue,
                objectKey: field));
        }

        // key
        if (string.IsNullOrWhiteSpace(field))
        {
            // basic lookup value
            return new(GetLookup<object>(
                lookupName: lookup,
                lookupKey: keyOrRangeValue));
        }
        // object field lookup value
        return new(GetObjectLookup<object>(
            lookupName: lookup,
            lookupKey: keyOrRangeValue,
            objectKey: field));
    }

    /// <summary>Get lookup value by key and range value</summary>
    [ActionParameter("lookup", "The lookup name", [StringType])]
    [ActionParameter("key", "The lookup key")]
    [ActionParameter("rangeValue", "The lookup key or range value")]
    [ActionParameter("field", "The JSON value field name (optional)")]
    [PayrollAction("GetLookupValue", "Get lookup value by key and value field name", "Lookup")]
    public ActionValue GetLookupValue(string lookup, ActionValue key, ActionValue rangeValue, string field = null)
    {
        // basic range lookup value
        if (string.IsNullOrWhiteSpace(field))
        {
            return new(GetRangeLookup<object>(
                lookupName: lookup,
                lookupKey: key,
                rangeValue: rangeValue));

        }
        // object field range lookup value
        return new(GetRangeObjectLookup<object>(
            lookupName: lookup,
            lookupKey: key,
            rangeValue: rangeValue,
            objectKey: field));
    }

    /// <summary>Apply a range value to the lookup ranges considering the lookup range mode</summary>
    /// <param name="lookup">Lookup name</param>
    /// <param name="range">Range value</param>
    /// <param name="field">Object field name</param>
    [ActionParameter("lookup", "The lookup name", [StringType])]
    [ActionParameter("range", "The range value", [DecimalType])]
    [ActionParameter("field", "The JSON value field name (optional)")]
    [PayrollAction("ApplyRangeLookupValue", "Apply a range value to the lookup ranges considering the lookup range mode", "Lookup")]
    public ActionValue ApplyRangeLookupValue(string lookup, decimal range, string field = null) =>
        ApplyRangeValue(
            lookupName: lookup,
            rangeValue: range,
            valueFieldName: field);

    #endregion

    #region Math

    /// <summary>Get the smallest collection value</summary>
    /// <param name="values">Value collection</param>
    [ActionParameter("values", "Value collection", [NumericType, DateType, TimeSpanType])]
    [PayrollAction("Min", "Get the smallest collection value", "Math")]
    public ActionValue Min(params ActionValue[] values)
    {
        // empty
        if (values.Length == 0)
        {
            return ActionValue.Null;
        }
        // single value
        if (values.Length == 1)
        {
            return values[0];
        }
        // multiple values
        var min = values[0];
        for (var i = 1; i < values.Length; i++)
        {
            min = Min(min, values[i]);
        }
        return min;
    }

    /// <summary>Get the minimum value</summary>
    /// <param name="left">Left value</param>
    /// <param name="right">Right value</param>
    [ActionParameter("left", "The left compare value", [NumericType, DateType, TimeSpanType])]
    [ActionParameter("right", "The right compare value", [NumericType, DateType, TimeSpanType])]
    [PayrollAction("Min", "Get the minimum value", "Math")]
    public ActionValue Min(ActionValue left, ActionValue right)
    {
        // null
        if (left?.Value == null && right?.Value == null)
        {
            return ActionValue.Null;
        }
        if (left?.Value == null)
        {
            return right;
        }
        if (right?.Value == null)
        {
            return left;
        }

        // numeric compare
        if (left.TryToDecimal(out var leftValue) &&
            right.TryToDecimal(out var rightValue))
        {
            return Math.Min(leftValue, rightValue);
        }

        // date compare
        if (left.TryToDateTime(out var leftDate) &&
            right.TryToDateTime(out var rightDate))
        {
            return leftDate <= rightDate ? leftDate : rightDate;
        }

        // timespan compare
        if (left.TryToTimeSpan(out var leftTimeSpan) &&
            right.TryToTimeSpan(out var rightTimeSpan))
        {
            return leftTimeSpan <= rightTimeSpan ? leftTimeSpan : rightTimeSpan;
        }

        return ActionValue.Null;
    }

    /// <summary>Get largest value of collection</summary>
    /// <param name="values">Value collection</param>
    [ActionParameter("values", "Value collection", [NumericType, DateType, TimeSpanType])]
    [PayrollAction("Max", "Get the largest collection value", "Math")]
    public ActionValue Max(params ActionValue[] values)
    {
        // empty
        if (values.Length == 0)
        {
            return ActionValue.Null;
        }
        // single value
        if (values.Length == 1)
        {
            return values[0];
        }
        // multiple values
        var max = values[0];
        for (var i = 1; i < values.Length; i++)
        {
            max = Max(max, values[i]);
        }
        return max;
    }

    /// <summary>Get the maximum value</summary>
    /// <param name="left">Left value</param>
    /// <param name="right">Right value</param>
    [ActionParameter("left", "The left compare value", [NumericType, DateType, TimeSpanType])]
    [ActionParameter("right", "The right compare value", [NumericType, DateType, TimeSpanType])]
    [PayrollAction("Max", "Get the maximum value", "Math")]
    public ActionValue Max(ActionValue left, ActionValue right)
    {
        // null
        if (left?.Value == null && right?.Value == null)
        {
            return ActionValue.Null;
        }
        if (left?.Value == null)
        {
            return right;
        }
        if (right?.Value == null)
        {
            return left;
        }

        // numeric compare
        if (left.TryToDecimal(out var leftValue) &&
            right.TryToDecimal(out var rightValue))
        {
            return Math.Max(leftValue, rightValue);
        }

        // date compare
        if (left.TryToDateTime(out var leftDate) &&
            right.TryToDateTime(out var rightDate))
        {
            return leftDate >= rightDate ? leftDate : rightDate;
        }

        // timespan compare
        if (left.TryToTimeSpan(out var leftTimeSpan) &&
            right.TryToTimeSpan(out var rightTimeSpan))
        {
            return leftTimeSpan >= rightTimeSpan ? leftTimeSpan : rightTimeSpan;
        }

        return ActionValue.Null;
    }

    /// <summary>Test value is within a value range</summary>
    /// <param name="value">The value to test</param>
    /// <param name="min">Left value</param>
    /// <param name="max">Right value</param>
    [ActionParameter("value", "The value to test", [NumericType, DateType, TimeSpanType])]
    [ActionParameter("min", "The minimum value", [NumericType, DateType, TimeSpanType])]
    [ActionParameter("max", "The maximum value", [NumericType, DateType, TimeSpanType])]
    [PayrollAction("Within", "Test value is within a value range", "Math")]
    public ActionValue Within(ActionValue value, ActionValue min, ActionValue max)
    {
        // null
        if (min?.Value == null || max?.Value == null)
        {
            return false;
        }

        // numeric range
        if (value.TryToDecimal(out var decimalValue) &&
            min.TryToDecimal(out var minDecimalValue) &&
            max.TryToDecimal(out var maxDecimalValue))
        {
            return decimalValue >= minDecimalValue && decimalValue <= maxDecimalValue;
        }

        // date range
        if (value.TryToDateTime(out var dateValue) &&
             min.TryToDateTime(out var minDateValue) &&
             max.TryToDateTime(out var maxDateValue))
        {
            return dateValue >= minDateValue && dateValue <= maxDateValue;
        }

        // time span
        if (value.TryToTimeSpan(out var timeSpanValue) &&
             min.TryToTimeSpan(out var minTimeSpanValue) &&
             max.TryToTimeSpan(out var maxTimeSpanValue))
        {
            return timeSpanValue >= minTimeSpanValue && timeSpanValue <= maxTimeSpanValue;
        }

        return false;
    }

    /// <summary>Ensure value is within a value range</summary>
    /// <param name="value">The value to limit</param>
    /// <param name="min">Left value</param>
    /// <param name="max">Right value</param>
    [ActionParameter("value", "The value to limit", [NumericType, DateType, TimeSpanType])]
    [ActionParameter("min", "The minimum value", [NumericType, DateType, TimeSpanType])]
    [ActionParameter("max", "The maximum value", [NumericType, DateType, TimeSpanType])]
    [PayrollAction("Range", "Ensure value is within a value range", "Math")]
    public ActionValue Range(ActionValue value, ActionValue min, ActionValue max)
    {
        // null
        if (min?.Value == null && max?.Value == null)
        {
            return value;
        }
        if (min?.Value == null)
        {
            return Min(value, max);
        }
        if (max?.Value == null)
        {
            return Max(value, min);
        }

        // numeric range
        if (value.TryToDecimal(out var decimalValue) &&
            min.TryToDecimal(out var minDecimalValue) &&
            max.TryToDecimal(out var maxDecimalValue))
        {
            var minDecimal = Math.Min(minDecimalValue, maxDecimalValue);
            var maxDecimal = Math.Max(minDecimalValue, maxDecimalValue);

            // no limits
            if (minDecimal == decimal.MinValue || maxDecimal == decimal.MaxValue)
            {
                return decimalValue;
            }
            // no range
            if (minDecimal == maxDecimal)
            {
                return minDecimal;
            }
            // less than min
            if (decimalValue < minDecimal)
            {
                return minDecimal;
            }
            // more than max
            if (decimalValue > maxDecimal)
            {
                return maxDecimal;
            }
            // value in range
            return decimalValue;
        }

        // date range
        if (value.TryToDateTime(out var dateValue) &&
             min.TryToDateTime(out var minDateValue) &&
             max.TryToDateTime(out var maxDateValue))
        {
            var minDate = Date.Min(minDateValue, maxDateValue);
            var maxDate = Date.Max(minDateValue, maxDateValue);

            // no limits
            if (minDate == DateTime.MinValue || maxDate == DateTime.MaxValue)
            {
                return dateValue;
            }
            // no range
            if (minDate == maxDate)
            {
                return minDate;
            }
            // less than min
            if (dateValue < minDate)
            {
                return minDate;
            }
            // more than max
            if (dateValue > maxDate)
            {
                return maxDate;
            }
            // value in range
            return dateValue;
        }

        // time span
        if (value.TryToTimeSpan(out var timeSpanValue) &&
             min.TryToTimeSpan(out var minTimeSpanValue) &&
             max.TryToTimeSpan(out var maxTimeSpanValue))
        {
            var minTimeSpan = Date.Min(minTimeSpanValue, maxTimeSpanValue);
            var maxTimeSpan = Date.Max(minTimeSpanValue, maxTimeSpanValue);

            // no limits
            if (minTimeSpan == TimeSpan.MinValue || maxTimeSpan == TimeSpan.MaxValue)
            {
                return timeSpanValue;
            }
            // no range
            if (minTimeSpan == maxTimeSpan)
            {
                return minTimeSpan;
            }
            // less than min
            if (timeSpanValue < minTimeSpan)
            {
                return minTimeSpan;
            }
            // more than max
            if (timeSpanValue > maxTimeSpan)
            {
                return maxTimeSpan;
            }
            // value in range
            return timeSpanValue;
        }

        return ActionValue.Null;
    }

    #endregion

    #region String

    /// <summary>Concat multiple strings</summary>
    /// <param name="values">Values to concat</param>
    /// <returns>True, if source is listed in tests</returns>
    [ActionParameter("values", "Value collection", [StringType])]
    [PayrollAction("Concat", "Concat multiple strings", "String")]
    public ActionValue Concat(params ActionValue[] values)
    {
        // string concat
        var stringValues = new List<string>();
        foreach (var value in values)
        {
            if (value.IsString)
            {
                stringValues.Add(value.AsString);
            }
        }
        return stringValues.Any() ? string.Concat(stringValues) : ActionValue.Null;
    }

    /// <summary>Test if value is from a specific value domain</summary>
    /// <param name="source">Object to test</param>
    /// <param name="values">Available values</param>
    /// <returns>True, if source is listed in tests</returns>
    [ActionParameter("values", "Value collection", [NumericType, DateType, StringType])]
    [PayrollAction("Contains", "Test if value is from a specific value domain", "String")]
    public bool Contains(ActionValue source, params ActionValue[] values)
    {
        // test values
        if (!values.Any())
        {
            return false;
        }

        // numeric
        if (source.TryToDecimal(out var sourceNumeric))
        {
            return values.Select(
                x => x.TryToDecimal(out var testNumeric) ? testNumeric : 0).Contains(sourceNumeric);
        }

        // date
        if (source.TryToDateTime(out var sourceDate))
        {
            return values.Select(
                x => x.TryToDateTime(out var testDate) ? testDate : DateTime.MinValue).Contains(sourceDate);
        }

        // time span
        if (source.TryToTimeSpan(out var sourceTimeSpan))
        {
            return values.Select(
                x => x.TryToTimeSpan(out var testDate) ? testDate : TimeSpan.MinValue).Contains(sourceTimeSpan);
        }

        // string
        if (source.IsString)
        {
            if (string.IsNullOrWhiteSpace(source.AsString))
            {
                return false;
            }
            return values.Select(x => x.AsString).Contains(source.AsString);
        }
        return false;
    }

    #endregion

    #region Date

    /// <summary>Get the timespan between two dates</summary>
    /// <returns>Timespan between the dates or none</returns>
    [ActionParameter("start", "The start date", [DateType])]
    [ActionParameter("end", "The end date", [DateType])]
    [PayrollAction("GetTimeSpan", "Get the timespan between two dates", "Date")]
    public ActionValue GetTimeSpan(ActionValue start, ActionValue end)
    {
        if (start.TryToDateTime(out var leftDate) &&
            end.TryToDateTime(out var rightDate))
        {
            return rightDate - leftDate;
        }
        return None;
    }

    /// <summary>Test for same date year</summary>
    /// <returns>True, if year are equals</returns>
    [ActionParameter("left", "The left date to compare", [DateType])]
    [ActionParameter("right", "The right date to compare", [DateType])]
    [PayrollAction("SameYear", "Test for same date year", "Date")]
    public ActionValue SameYear(ActionValue left, ActionValue right)
    {
        if (left.TryToDateTime(out var leftDate) &&
            right.TryToDateTime(out var rightDate))
        {
            return leftDate.Year == rightDate.Year;
        }
        return None;
    }

    /// <summary>Test for same date month</summary>
    /// <returns>True, if year and month are equals</returns>
    [ActionParameter("left", "The left date to compare", [DateType])]
    [ActionParameter("right", "The right date to compare", [DateType])]
    [PayrollAction("SameMonth", "Test for same date month", "Date")]
    public ActionValue SameMonth(ActionValue left, ActionValue right)
    {
        if (left.TryToDateTime(out var leftDate) &&
            right.TryToDateTime(out var rightDate))
        {
            return leftDate.Year == rightDate.Year &&
                   leftDate.Month == rightDate.Month;
        }
        return None;
    }

    /// <summary>Test for same date day</summary>
    /// <returns>True, if year, month and day are equals</returns>
    [ActionParameter("left", "The left date to compare", [DateType])]
    [ActionParameter("right", "The right date to compare", [DateType])]
    [PayrollAction("SameDay", "Test for same date day", "Date")]
    public ActionValue SameDay(ActionValue left, ActionValue right)
    {
        if (left.TryToDateTime(out var leftDate) &&
            right.TryToDateTime(out var rightDate))
        {
            return leftDate.Year == rightDate.Year &&
                   leftDate.Month == rightDate.Month &&
                   leftDate.Day == rightDate.Day;
        }
        return None;
    }

    /// <summary>Get years between two dates</summary>
    /// <returns>The year (int) od dates, otherwise none</returns>
    [ActionParameter("start", "The start date", [DateType])]
    [ActionParameter("end", "The end date", [DateType])]
    [PayrollAction("YearDiff", "Test for same date year", "Date")]
    public ActionValue YearDiff(ActionValue start, ActionValue end)
    {
        if (start.TryToDateTime(out var startDate) &&
            end.TryToDateTime(out var endDate))
        {
            return (DateTime.MinValue + endDate.Subtract(startDate)).Year;
        }
        return None;
    }

    /// <summary>Get persons age</summary>
    /// <returns>True, if source is listed in tests</returns>
    [ActionParameter("birthDate", "The persons birth date", [DateType])]
    [ActionParameter("testDate", "Reference date (default: utc-now)", [DateType])]
    [PayrollAction("Age", "Get persons age", "Date")]
    public ActionValue Age(ActionValue birthDate, ActionValue testDate = null)
    {
        var years = YearDiff(birthDate, testDate ?? DateTime.UtcNow);
        return years != None ? years - 1 : None;
    }

    #endregion

    #region System

    /// <summary>Returns true whether the value is Null</summary>
    [ActionParameter("value", "Value to test")]
    [PayrollAction("IsNull", "Returns true whether the value is Null", "System")]
    public ActionValue IsNull(ActionValue value) =>
        value == null || value.IsNull;

    /// <summary>Returns true whether the value is not Null</summary>
    [ActionParameter("value", "Value to test")]
    [PayrollAction("IsNotNull", "Returns true whether the value is not Null", "System")]
    public ActionValue IsNotNull(ActionValue value) =>
        value != null && !value.IsNull;

    /// <summary>Returns the second value in case the first value is null</summary>
    [ActionParameter("first", "Value to return if defined")]
    [ActionParameter("second", "Value to return if first value is undefined")]
    [PayrollAction("IfNull", "Returns the second value in case the first value is null", "System")]
    public ActionValue IfNull(ActionValue first, ActionValue second) =>
        IsNull(first) ? second : first;

    /// <summary>Returns one of two values, depending on the evaluation of an expression</summary>
    [ActionParameter("expression", "Expression to evaluate")]
    [ActionParameter("onTrue", "Value or expression returned if expr is True")]
    [ActionParameter("onFalse", "Value or expression returned if expr is False")]
    [PayrollAction("IIf", "Returns one of two parts, depending on the evaluation of an expression", "System")]
    public ActionValue IIf(bool expression, ActionValue onTrue, ActionValue onFalse) =>
        expression ? onTrue : onFalse;

    /// <summary>Set namespace to a name</summary>
    [ActionParameter("name", "Name to be extended")]
    [ActionParameter("namespace", "Namespace to apply (default: regulation namespace)")]
    [PayrollAction("SetNamespace", "Set namespace to a name", "System")]
    public string SetNamespace(string name, string @namespace = null)
    {
        if (string.IsNullOrWhiteSpace(@namespace) && string.IsNullOrWhiteSpace(Namespace))
        {
            return name;
        }
        return name.EnsureEnd(@namespace ?? Namespace);
    }

    /// <summary>Log a message</summary>
    [ActionParameter("message", "Message to log")]
    [ActionParameter("level", "Log level (default: Information)")]
    [PayrollAction("Log", "Log a message", "System")]
    public void Log(string message, LogLevel level = LogLevel.Information) =>
        Log(level, message);

    #endregion

}
