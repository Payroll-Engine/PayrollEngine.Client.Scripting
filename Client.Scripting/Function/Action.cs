/* Action */

using System;
using System.Text.Json;
using System.Globalization;

namespace PayrollEngine.Client.Scripting.Function;

#region Enum

/// <summary>Action value type</summary>
public enum ActionValueType
{
    /// <summary>No type</summary>
    None,
    /// <summary>String type</summary>
    String,
    /// <summary>Boolean type</summary>
    Boolean,
    /// <summary>Integer type</summary>
    Integer,
    /// <summary>Decimal type</summary>
    Decimal,
    /// <summary>DateTime type</summary>
    DateTime,
    /// <summary>TimeSpan type</summary>
    TimeSpan
}

#endregion

#region Action Value

/// <summary>Action value</summary>
public sealed class ActionValue
{

    #region Lifecycle

    /// <summary>Value constructor</summary>
    /// <param name="value">Action value</param>
    /// <param name="culture">Value culture (default: invariant)</param>
    public ActionValue(object value, CultureInfo culture = null)
    {
        Value = value switch
        {
            JsonElement jsonElement => jsonElement.GetValue(),
            ActionValue actionValue => actionValue.Value,
            _ => value
        };
        Culture = culture ?? CultureInfo.InvariantCulture;
    }

    /// <summary>Create action value from value</summary>
    /// <param name="value">Source value</param>
    public static ActionValue From(object value) =>
        value switch
        {
            ActionValue actionValue => actionValue,
            null => Null,
            _ => new(value)
        };

    #endregion

    #region Value & Type

    /// <summary>Empty instance</summary>
    public static readonly ActionValue Null = new(null);

    /// <summary>Value culture</summary>
    public CultureInfo Culture { get; }

    /// <summary>Action primitive value</summary>
    public object Value { get; }

    /// <summary>Test for action value</summary>
    public bool HasValue => !IsNull;

    /// <summary>Test for action null value</summary>
    public bool IsNull => Value == null;

    /// <summary>Test for action string value</summary>
    public bool IsString => Value is string;

    /// <summary>Test for integer value</summary>
    public bool IsInt => TryToInt(out _);

    /// <summary>Test for decimal value</summary>
    public bool IsDecimal => TryToDecimal(out _);

    /// <summary>Test for numeric value (int or decimal)</summary>
    public bool IsNumeric => IsInt || IsDecimal;

    /// <summary>Test for action date value</summary>
    public bool IsDateTime => TryToDateTime(out _);

    /// <summary>Test for action timespan value</summary>
    public bool IsTimeSpan => TryToTimeSpan(out _);

    /// <summary>Test for action bool value</summary>
    public bool IsBool => TryToBool(out _);

    /// <summary>Get the action value type</summary>
    public ActionValueType ValueType
    {
        get
        {
            if (IsNull)
            {
                return ActionValueType.None;
            }
            if (IsInt)
            {
                return ActionValueType.Integer;
            }
            // decimal after int
            if (IsDecimal)
            {
                return ActionValueType.Decimal;
            }
            if (IsBool)
            {
                return ActionValueType.Boolean;
            }
            if (IsDateTime)
            {
                return ActionValueType.DateTime;
            }
            if (IsTimeSpan)
            {
                return ActionValueType.TimeSpan;
            }
            if (IsString)
            {
                return ActionValueType.String;
            }

            return ActionValueType.None;
        }
    }

    #endregion

    #region Convert

    /// <summary>Action value as string</summary>
    public string AsString => Value?.ToString();

    /// <summary>Action value as boolean</summary>
    public bool AsBool => TryToBool(out var value) ?
        value :
        throw new ScriptException($"Invalid boolean action value {this}");

    /// <summary>Action value as integer</summary>
    public int AsInt => TryToInt(out var value) ?
        value :
        throw new ScriptException($"Invalid integer action value {this}");

    /// <summary>Action value as decimal</summary>
    public decimal AsDecimal => TryToDecimal(out var value) ?
        value :
        throw new ScriptException($"Invalid decimal action value {this}");

    /// <summary>Action value as date</summary>
    public DateTime AsDateTime => TryToDateTime(out var value) ?
        value :
        throw new ScriptException($"Invalid date action value {this}");

    /// <summary>Action value as timespan</summary>
    public TimeSpan AsTimeSpan => TryToTimeSpan(out var value) ?
        value :
        throw new ScriptException($"Invalid timespan action value {this}");

    /// <summary>Convert acton value to bool</summary>
    /// <param name="value">Source value</param>
    public bool TryToBool(out bool value)
    {
        value = false;
        switch (Value)
        {
            // decimal
            case bool boolValue:
                value = boolValue;
                return true;
            // boolean string
            case string s when bool.TryParse(s,
                result: out var parsed):
                value = parsed;
                return true;
            default:
                return false;
        }
    }

    /// <summary>Convert acton value to int</summary>
    /// <param name="value">Source value</param>
    public bool TryToInt(out int value)
    {
        value = 0;
        switch (Value)
        {
            // int
            case int i:
                value = i;
                return true;
            // numeric primitives
            case sbyte or byte or
                short or ushort or
                int or uint or
                long or ulong or
                decimal or float or double:
                value = Convert.ToInt32(Value);
                return true;
            // numeric string
            case string s when int.TryParse(s,
                style: NumberStyles.Any,
                provider: Culture,
                result: out var parsed):
                value = parsed;
                return true;
            default:
                return false;
        }
    }

    /// <summary>Convert acton value to decimal</summary>
    /// <param name="value">Source value</param>
    public bool TryToDecimal(out decimal value)
    {
        value = 0m;
        switch (Value)
        {
            // decimal
            case decimal dec:
                value = dec;
                return true;
            // numeric primitives
            case sbyte or byte or
                short or ushort or
                int or uint or
                long or ulong or
                float or double:
                value = Convert.ToDecimal(Value);
                return true;
            // numeric string
            case string s when decimal.TryParse(s,
                style: NumberStyles.Any,
                provider: Culture,
                result: out var parsed):
                value = parsed;
                return true;
            default:
                return false;
        }
    }

    /// <summary>Convert acton value to datetime</summary>
    /// <param name="value">Source value</param>
    public bool TryToDateTime(out DateTime value)
    {
        value = Date.MinValue;
        switch (Value)
        {
            // date time
            case DateTime dateTime:
                value = dateTime;
                return true;
            // date string
            case string s when DateTime.TryParse(s,
                provider: Culture,
                result: out var parsed):
                value = parsed;
                return true;
            default:
                return false;
        }
    }

    /// <summary>Convert acton value to timespan</summary>
    /// <param name="value">Source value</param>
    public bool TryToTimeSpan(out TimeSpan value)
    {
        value = TimeSpan.Zero;
        switch (Value)
        {
            // time span
            case TimeSpan timeSpan:
                value = timeSpan;
                return true;
            // date string
            case string s when TimeSpan.TryParse(s,
                formatProvider: Culture,
                result: out var parsed):
                value = parsed;
                return true;
            default:
                return false;
        }
    }

    #endregion

    #region Casting operators

    /// <summary>Convert action value to string/></summary>
    public static implicit operator string(ActionValue value) =>
        value?.AsString;

    /// <summary>Convert action value to int/></summary>
    public static implicit operator int(ActionValue value) =>
        value != null && value.TryToInt(out var numericValue) ?
            numericValue : 0;

    /// <summary>Convert action value to nullable int</summary>
    public static implicit operator int?(ActionValue value) =>
        value != null && value.TryToInt(out var numericValue) ?
            numericValue : 0;

    /// <summary>Convert action value to decimal</summary>
    public static implicit operator decimal(ActionValue value) =>
        value != null && value.TryToDecimal(out var numericValue) ?
            numericValue : 0;

    /// <summary>Convert action value to nullable decimal</summary>
    public static implicit operator decimal?(ActionValue value) =>
        value != null && value.TryToDecimal(out var numericValue) ?
            numericValue : 0;

    /// <summary>Convert action value to DateTime</summary>
    public static implicit operator DateTime(ActionValue value) =>
        value != null && value.TryToDateTime(out var dateTime) ?
            dateTime : DateTime.MinValue;

    /// <summary>Convert action value to nullable DateTime</summary>
    public static implicit operator DateTime?(ActionValue value) =>
        value != null && value.TryToDateTime(out var dateTime) ?
            dateTime : DateTime.MinValue;

    /// <summary>Convert action value to TimeSpan</summary>
    public static implicit operator TimeSpan(ActionValue value) =>
        value != null && value.TryToTimeSpan(out var dateTime) ?
            dateTime : TimeSpan.MinValue;

    /// <summary>Convert action value to nullable TimeSpan</summary>
    public static implicit operator TimeSpan?(ActionValue value) =>
        value != null && value.TryToTimeSpan(out var dateTime) ?
            dateTime : TimeSpan.MinValue;

    /// <summary>Convert action value to bool</summary>
    public static implicit operator bool(ActionValue value) =>
        value != null && value.TryToBool(out var @bool) && @bool;

    /// <summary>Convert action value to nullable bool</summary>
    public static implicit operator bool?(ActionValue value) =>
        value != null && value.TryToBool(out var @bool) && @bool;

    /// <summary>Implicit int conversion</summary>
    public static implicit operator ActionValue(int value) => new(value);

    /// <summary>Implicit long conversion</summary>
    public static implicit operator ActionValue(long value) => new(value);

    /// <summary>Implicit float conversion</summary>
    public static implicit operator ActionValue(float value) => new(value);

    /// <summary>Implicit double conversion</summary>
    public static implicit operator ActionValue(double value) => new(value);

    /// <summary>Implicit decimal conversion</summary>
    public static implicit operator ActionValue(decimal value) => new(value);

    /// <summary>Implicit string conversion</summary>
    public static implicit operator ActionValue(string value) => new(value);

    /// <summary>Implicit bool conversion</summary>
    public static implicit operator ActionValue(bool value) => new(value);

    /// <summary>Implicit datetime conversion</summary>
    public static implicit operator ActionValue(DateTime value) => new(value);

    /// <summary>Implicit timespan conversion</summary>
    public static implicit operator ActionValue(TimeSpan value) => new(value);

    #endregion

    #region Unary operators

    /// <summary>Unary plus of an action value</summary>
    public static ActionValue operator +(ActionValue value) =>
        value != null && value.TryToDecimal(out var numericValue) ? +numericValue : value;

    /// <summary>Unary minus of an action value</summary>
    public static ActionValue operator -(ActionValue value) =>
        value != null && value.TryToDecimal(out var numericValue) ? -numericValue : value;

    /// <summary>Logical negation of an action value (bool)</summary>
    public static ActionValue operator !(ActionValue value) =>
        value != null && value.TryToBool(out var @bool) ? !@bool : value;

    /// <summary>Test if action value is true</summary>
    public static bool operator true(ActionValue value) =>
        value != null && value.TryToBool(out var @bool) && @bool;

    /// <summary>Test if action value is false</summary>
    public static bool operator false(ActionValue value) =>
        value != null && value.TryToBool(out var @bool) && !@bool;

    #endregion

    #region Binary operators

    /// <summary>Addition of two action values (decimal, int, string)</summary>
    /// <remarks>Treats undefined values as zero</remarks>
    public static ActionValue operator +(ActionValue left, ActionValue right)
    {
        // string
        if (left.IsString && right.IsString)
        {
            return left.AsString + right.AsString;
        }

        // numeric
        var leftNumeric = left.TryToDecimal(out var leftValue);
        var rightNumeric = right.TryToDecimal(out var rightValue);
        if (!leftNumeric && !rightNumeric)
        {
            return Null;
        }
        if (!leftNumeric)
        {
            return rightValue;
        }
        if (!rightNumeric)
        {
            return leftValue;
        }

        // time span
        var leftTimeSpan = left.TryToTimeSpan(out var leftTimeSpanValue);
        var rightTimeSpan = right.TryToTimeSpan(out var rightTimeSpanValue);
        if (leftTimeSpan && rightTimeSpan)
        {
            return leftTimeSpanValue.Add(rightTimeSpanValue);
        }

        // date with time span
        var leftDate = left.TryToDateTime(out var leftDateValue);
        if (leftDate && rightTimeSpan)
        {
            return leftDateValue.Add(rightTimeSpanValue);
        }

        return leftValue + rightValue;
    }

    /// <summary>Subtraction of two action values</summary>
    /// <remarks>Treats undefined values as zero</remarks>
    public static ActionValue operator -(ActionValue left, ActionValue right)
    {
        // numeric
        var leftNumeric = left.TryToDecimal(out var leftValue);
        var rightNumeric = right.TryToDecimal(out var rightValue);
        if (!leftNumeric && !rightNumeric)
        {
            return Null;
        }
        if (!leftNumeric)
        {
            // negate right operand on undefined left operand
            return -rightValue;
        }
        if (!rightNumeric)
        {
            return leftValue;
        }

        // time span
        var leftTimeSpan = left.TryToTimeSpan(out var leftTimeSpanValue);
        var rightTimeSpan = right.TryToTimeSpan(out var rightTimeSpanValue);
        if (leftTimeSpan && rightTimeSpan)
        {
            return leftTimeSpanValue.Subtract(rightTimeSpanValue);
        }

        // date with time span
        var leftDate = left.TryToDateTime(out var leftDateValue);
        if (leftDate && rightTimeSpan)
        {
            return leftDateValue.Subtract(rightTimeSpanValue);
        }

        return leftValue - rightValue;
    }

    /// <summary>Multiplication of two action values</summary>
    /// <remarks>Treats undefined values as one</remarks>
    public static ActionValue operator *(ActionValue left, ActionValue right)
    {
        // numeric
        var leftNumeric = left.TryToDecimal(out var leftValue);
        var rightNumeric = right.TryToDecimal(out var rightValue);
        if (!leftNumeric && !rightNumeric)
        {
            return Null;
        }
        if (!leftNumeric)
        {
            return rightValue;
        }
        if (!rightNumeric)
        {
            return leftValue;
        }
        return leftValue * rightValue;
    }

    /// <summary>Division of two action values</summary>
    /// <remarks><see cref="Null">Null</see>/>on undefined left, treat undefined right as one</remarks>
    public static ActionValue operator /(ActionValue left, ActionValue right)
    {
        // numeric
        if (!left.TryToDecimal(out var leftValue))
        {
            return Null;
        }
        if (!right.TryToDecimal(out var rightValue))
        {
            return left;
        }
        return leftValue / rightValue;
    }

    /// <summary>Remainder of two action values</summary>
    /// <remarks><see cref="Null">Null</see>/>on undefined left, treat undefined right as one</remarks>
    public static ActionValue operator %(ActionValue left, ActionValue right)
    {
        // numeric
        if (!left.TryToDecimal(out var leftValue))
        {
            // zero on undefined left operand
            return Null;
        }
        if (!right.TryToDecimal(out var rightValue))
        {
            return left;
        }
        return leftValue % rightValue;
    }

    /// <summary>Test if two action values are true (bool)</summary>
    public static ActionValue operator &(ActionValue left, ActionValue right) =>
        left.TryToBool(out var leftBoolean) && leftBoolean &&
        right.TryToBool(out var rightBoolean) && rightBoolean;

    /// <summary>Test if any action value is true (bool)</summary>
    public static ActionValue operator |(ActionValue left, ActionValue right) =>
        (left.TryToBool(out var leftBoolean) && leftBoolean) |
        (right.TryToBool(out var rightBoolean) && rightBoolean);

    /// <summary>Compare two action values for equality</summary>
    public static bool operator ==(ActionValue left, ActionValue right)
    {
        if (ReferenceEquals(left, right))
        {
            return true;
        }
        // undefined
        if (left is null || right is null)
        {
            return false;
        }

        // numeric compare
        if (left.TryToDecimal(out var leftValue) && right.TryToDecimal(out var rightValue))
        {
            return Equals(leftValue, rightValue);
        }

        // date compare
        if (left.TryToDateTime(out var leftDate) && right.TryToDateTime(out var rightDate))
        {
            return Equals(leftDate, rightDate);
        }

        // timespan compare
        if (left.TryToTimeSpan(out var leftTimeSpan) && right.TryToTimeSpan(out var rightTimeSpan))
        {
            return Equals(leftTimeSpan, rightTimeSpan);
        }

        // string compare (case-insensitive)
        if (left.Value is string leftString && right.Value is string rightString)
        {
            return string.Equals(leftString, rightString, StringComparison.OrdinalIgnoreCase);
        }

        return Equals(left.Value, right.Value);
    }

    /// <summary>Compare two action values for difference</summary>
    public static bool operator !=(ActionValue left, ActionValue right) =>
        !(left == right);

    /// <summary>Compare if an action value is less than another action value</summary>
    public static ActionValue operator <(ActionValue left, ActionValue right)
    {
        // undefined
        if (!left.HasValue || !right.HasValue)
        {
            return false;
        }

        // numeric compare
        if (left.TryToDecimal(out var leftValue) && right.TryToDecimal(out var rightValue))
        {
            return leftValue < rightValue;
        }

        // date compare
        if (left.TryToDateTime(out var leftDate) && right.TryToDateTime(out var rightDate))
        {
            return leftDate < rightDate;
        }

        // timespan compare
        if (left.TryToTimeSpan(out var leftTimeSpan) && right.TryToTimeSpan(out var rightTimeSpan))
        {
            return leftTimeSpan < rightTimeSpan;
        }

        throw new ScriptException($"operator < error in action values {left} and {right}.");
    }

    /// <summary>Compare if an action value is greater than another action value</summary>
    public static ActionValue operator >(ActionValue left, ActionValue right)
    {
        // undefined
        if (!left.HasValue || !right.HasValue)
        {
            return false;
        }

        // numeric compare
        if (left.TryToDecimal(out var leftValue) && right.TryToDecimal(out var rightValue))
        {
            return leftValue > rightValue;
        }

        // date compare
        if (left.TryToDateTime(out var leftDate) && right.TryToDateTime(out var rightDate))
        {
            return leftDate > rightDate;
        }

        // timespan compare
        if (left.TryToTimeSpan(out var leftTimeSpan) && right.TryToTimeSpan(out var rightTimeSpan))
        {
            return leftTimeSpan > rightTimeSpan;
        }

        throw new ScriptException($"operator > error in action values {left} and {right}.");
    }

    /// <summary>Compare if an action value is less or equals than another action value</summary>
    public static ActionValue operator <=(ActionValue left, ActionValue right)
    {
        // undefined
        if (!left.HasValue || !right.HasValue)
        {
            return false;
        }

        // numeric compare
        if (left.TryToDecimal(out var leftValue) && right.TryToDecimal(out var rightValue))
        {
            return leftValue <= rightValue;
        }

        // date compare
        if (left.TryToDateTime(out var leftDate) && right.TryToDateTime(out var rightDate))
        {
            return leftDate <= rightDate;
        }

        // timespan compare
        if (left.TryToTimeSpan(out var leftTimeSpan) && right.TryToTimeSpan(out var rightTimeSpan))
        {
            return leftTimeSpan <= rightTimeSpan;
        }

        throw new ScriptException($"operator <= error in action values {left} and {right}.");
    }

    /// <summary>Compare if an action value is greater or equals than another action value</summary>
    public static ActionValue operator >=(ActionValue left, ActionValue right)
    {
        // undefined
        if (!left.HasValue || !right.HasValue)
        {
            return false;
        }

        // numeric compare
        if (left.TryToDecimal(out var leftValue) && right.TryToDecimal(out var rightValue))
        {
            return leftValue >= rightValue;
        }

        // date compare
        if (left.TryToDateTime(out var leftDate) && right.TryToDateTime(out var rightDate))
        {
            return leftDate >= rightDate;
        }

        // timespan compare
        if (left.TryToTimeSpan(out var leftTimeSpan) && right.TryToTimeSpan(out var rightTimeSpan))
        {
            return leftTimeSpan >= rightTimeSpan;
        }

        throw new ScriptException($"operator >= error in action values {left} and {right}.");
    }

    #endregion

    #region Decimal

    /// <summary>Returns the integral digits of the specified decimal, using a step size</summary>
    /// <param name="decimals">The number of significant decimal places (precision) in the return value</param>
    /// <param name="mode">A value that specifies how to round d if it is midway between two other numbers</param>
    public ActionValue Round(int decimals = 0, MidpointRounding mode = MidpointRounding.ToEven) =>
        IsDecimal ? decimal.Round(AsDecimal, decimals, mode) : this;

    /// <summary>Rounds a decimal value up</summary>
    /// <param name="stepSize">The round step size</param>
    /// <returns>The up-rounded value</returns>
    public ActionValue RoundUp(decimal stepSize = 1) =>
        IsDecimal ? AsDecimal.RoundUp(stepSize) : this;

    /// <summary>Rounds a decimal value down</summary>
    /// <param name="stepSize">The round step size</param>
    /// <returns>The down-rounded value</returns>
    public ActionValue RoundDown(decimal stepSize) =>
        IsDecimal ? AsDecimal.RoundDown(stepSize) : this;

    /// <summary>Returns the integral digits of the specified decimal, using a step size</summary>
    /// <param name="stepSize">The step size used to truncate</param>
    public ActionValue Truncate(int stepSize = 1) =>
        IsDecimal ? AsDecimal.Truncate(stepSize) : this;

    /// <summary>Returns a specified number raised to the specified power</summary>
    /// <param name="power">Number that specifies a power</param>
    public ActionValue Power(decimal power) =>
        IsNumeric ? Math.Pow((double)AsDecimal, (double)power) : this;

    /// <summary>Returns the absolute value of a specified number</summary>
    public ActionValue Abs() =>
        IsNumeric ? Math.Abs((double)AsDecimal) : this;

    /// <summary>Returns the square root of a specified number</summary>
    public ActionValue Sqrt() =>
        IsNumeric ? Math.Sqrt((double)AsDecimal) : this;

    #endregion

    #region Date

    /// <summary>Get year from date value</summary>
    public int Year => 
        TryToDateTime(out var dateTime) ? dateTime.Year : 0;

    /// <summary>Get month from date value</summary>
    public int Month => 
        TryToDateTime(out var dateTime) ? dateTime.Month : 0;

    /// <summary>Get day from date value</summary>
    public int Day => 
        TryToDateTime(out var dateTime) ? dateTime.Day : 0;

    /// <summary>Add years to date value</summary>
    public DateTime AddYears(int years) => 
        TryToDateTime(out var dateTime) ? dateTime.AddYears(years) : DateTime.MinValue;

    /// <summary>Add months to date value</summary>
    public DateTime AddMonths(int months) => 
        TryToDateTime(out var dateTime) ? dateTime.AddMonths(months) : DateTime.MinValue;

    /// <summary>Add days to date value</summary>
    public DateTime AddDays(int days) => 
        TryToDateTime(out var dateTime) ? dateTime.AddDays(days) : DateTime.MinValue;

    /// <summary>Add timespan to date value</summary>
    public DateTime Add(TimeSpan timeSpan) => 
        TryToDateTime(out var dateTime) ? dateTime.Add(timeSpan) : DateTime.MinValue;

    /// <summary>Subtract timespan to date value</summary>
    public DateTime Subtract(TimeSpan timeSpan) => 
        TryToDateTime(out var dateTime) ? dateTime.Subtract(timeSpan) : DateTime.MinValue;

    /// <summary>Get day from timespan value</summary>
    public int Days => 
        TryToTimeSpan(out var dateTime) ? dateTime.Days : 0;

    #endregion

    #region Object

    /// <summary>Determines whether the specified <see cref="object" /> is equal to this instance</summary>
    /// <param name="obj">The object to compare with the current object</param>
    /// <returns>True if the specified <see cref="object" /> is equal to this instance</returns>
    public override bool Equals(object obj)
    {
        var compare = obj as ActionValue;
        if (compare is null)
        {
            return false;
        }
        if (ReferenceEquals(this, compare))
        {
            return true;
        }

        if (Value == null)
        {
            return compare.Value == null;
        }
        return Value.Equals(compare.Value);
    }

    /// <summary>Returns a hash code for this instance</summary>
    /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table</returns>
    public override int GetHashCode() =>
        Value.GetHashCode();

    /// <summary>Returns a <see cref="string" /> that represents this instance</summary>
    /// <returns>A <see cref="string" /> that represents this instance</returns>
    public override string ToString() =>
        AsString ?? base.ToString();

    #endregion

}

#endregion
