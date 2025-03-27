/* HourPeriod */

using System;
using System.Text.Json.Serialization;

namespace PayrollEngine.Client.Scripting;

/// <summary>An immutable hour period between the start and end time</summary>
public sealed class HourPeriod
{
    /// <summary>Create a period from start to end</summary>
    public HourPeriod(decimal start, decimal end)
    {
        if (end < start)
        {
            Start = end;
            End = start;
        }
        else
        {
            Start = start;
            End = end;
        }
    }

    /// <summary>Create a period from conditional start to conditional end</summary>
    public HourPeriod(decimal? start, decimal? end) :
        this(start ?? 0m, end ?? 0m)
    {
    }

    /// <summary>
    /// Create time from start time and duration
    /// </summary>
    /// <param name="start">Period start</param>
    /// <param name="hours">Duration time</param>
    public static HourPeriod FromStart(decimal start, decimal hours) =>
        new(start, start + hours);

    /// <summary>
    /// Create time from start time and duration
    /// </summary>
    /// <param name="start">Period start</param>
    /// <param name="hours">Duration time</param>
    public static HourPeriod FromStart(decimal? start, decimal? hours) =>
        FromStart(start ?? 0m, hours ?? 0m);

    /// <summary>
    /// Create time from end time and duration
    /// </summary>
    /// <param name="hours">Duration time</param>
    /// <param name="end">Period end</param>
    public static HourPeriod FromEnd(decimal hours, decimal end) =>
        new(end - hours, end);

    /// <summary>
    /// Create time from end time and duration
    /// </summary>
    /// <param name="hours">Duration time</param>
    /// <param name="end">Period end</param>
    public static HourPeriod FromEnd(decimal? hours, decimal? end) =>
        FromEnd(hours ?? 0m, end ?? 0m);

    /// <summary>The period start time</summary>
    public decimal Start { get; }

    /// <summary>The period end time</summary>
    public decimal End { get; }

    /// <summary>Test if start and end are equal</summary>
    [JsonIgnore]
    public bool IsMoment => Start.Equals(End);

    /// <summary>Test if period is empty</summary>
    [JsonIgnore]
    public bool IsEmpty => IsMoment;

    /// <summary>The period hours</summary>
    [JsonIgnore]
    public decimal Hours => End - Start;

    /// <summary>The period duration</summary>
    [JsonIgnore]
    public TimeSpan Duration =>
        IsEmpty ?
            TimeSpan.Zero :
            TimeSpan.FromHours((double)Hours);

    /// <summary>Get start date</summary>
    public DateTime GetStartDate(DateTime day) =>
        day.Date.AddHours((double)Start);

    /// <summary>Get end date</summary>
    public DateTime GetEndDate(DateTime day) =>
        day.Date.AddHours((double)End);

    #region Object

    /// <summary>Determines whether the specified <see cref="object" /> is equal to this instance</summary>
    /// <param name="source">The object to compare with the current object</param>
    /// <returns>True if the specified <see cref="object" /> is equal to this instance</returns>
    public override bool Equals(object source)
    {
        var compare = source as HourPeriod;
        if (compare == null)
        {
            return false;
        }
        if (ReferenceEquals(this, compare))
        {
            return true;
        }
        return Start.Equals(compare.Start) &&
               End.Equals(compare.End);
    }

    /// <summary>Returns a hash code for this instance</summary>
    /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table</returns>
    public override int GetHashCode() =>
        (Start, End).GetHashCode();

    /// <summary>Compare two time periods for equal values</summary>
    /// <param name="left">The left period to compare</param>
    /// <param name="right">The right period to compare</param>
    /// <returns>True if the periods are equal</returns>
    public static bool operator ==(HourPeriod left, HourPeriod right)
    {
        if (ReferenceEquals(left, right))
        {
            return true;
        }
        if (left is null || right is null)
        {
            return false;
        }
        return left.Equals(right);
    }

    /// <summary>Compare two time periods for different values</summary>
    /// <param name="left">The left period to compare</param>
    /// <param name="right">The right period to compare</param>
    /// <returns>True if the periods are different</returns>
    public static bool operator !=(HourPeriod left, HourPeriod right) =>
        !(left == right);

    /// <summary>Returns a <see cref="string" /> that represents this instance</summary>
    /// <returns>A <see cref="string" /> that represents this instance</returns>
    public override string ToString() =>
        $"{Start:0.##} - {End:0.##}";

    #endregion

}