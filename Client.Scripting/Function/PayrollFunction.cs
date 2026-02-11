/* PayrollFunction */

using System;
using System.Linq;
using System.Text.Json;
using System.Reflection;
using System.Collections.Generic;

namespace PayrollEngine.Client.Scripting.Function;

/// <summary>Base class for any payroll function</summary>
// ReSharper disable once PartialTypeWithSinglePart
public abstract partial class PayrollFunction : Function
{
    /// <summary>String type name</summary>
    protected const string StringType = "String";

    /// <summary>Integer type name</summary>
    protected const string IntType = "Int";

    /// <summary>Numeric type name</summary>
    protected const string NumericType = "Num";

    /// <summary>Decimal type name</summary>
    protected const string DecimalType = "Dec";

    /// <summary>Date type name</summary>
    protected const string DateType = "Date";

    /// <summary>Timespan type name</summary>
    protected const string TimeSpanType = "TimeSpan";

    /// <summary>Boolean type name</summary>
    protected const string BooleanType = "Bool";

    /// <summary>New function instance</summary>
    /// <param name="runtime">The function runtime</param>
    protected PayrollFunction(object runtime) :
        base(runtime)
    {
        // payroll
        PayrollId = Runtime.PayrollId;

        // regulation
        Namespace = Runtime.Namespace;

        // division
        DivisionId = Runtime.DivisionId;

        // culture
        PayrollCulture = Runtime.PayrollCulture;

        // employee
        EmployeeId = Runtime.EmployeeId;
        EmployeeIdentifier = Runtime.EmployeeIdentifier;

        // evaluation
        EvaluationDate = Runtime.EvaluationDate;
        var (start, end) = (Tuple<DateTime, DateTime>)Runtime.GetEvaluationPeriod();
        EvaluationPeriod = new(start, end);

        // cycle
        Cycle = GetCycle();
        PreviousCycle = GetCycle(-1);
        NextCycle = GetCycle(1);

        // date/period
        Period = GetPeriod();
        PreviousPeriod = GetPeriod(-1);
        NextPeriod = GetPeriod(1);
        CycleStartOffset = GetPeriodOffset(CycleStart);
        CycleEndOffset = GetPeriodOffset(CycleEnd);

        Periods = new(GetPeriod);
        CaseValue = new(x => GetCaseValue(x));
        CaseValueTags = new(GetCaseValueTags);
    }

    #region Scripting Development

    /// <summary>New function instance without runtime (scripting development)</summary>
    /// <remarks>Use <see cref="Function.GetSourceFileName"/> in your constructor for the source file name</remarks>
    /// <param name="sourceFileName">The name of the source file</param>
    protected PayrollFunction(string sourceFileName) :
        base(sourceFileName)
    {
    }

    #endregion

    #region Payroll

    /// <summary>The payroll id</summary>
    public int PayrollId { get; }

    #endregion

    #region Namespace

    /// <summary>The function regulation namespace</summary>
    [ActionProperty("Regulation namespace")]
    public string Namespace { get; }

    /// <summary>Test for function regulation namespace</summary>
    public bool HasNamespace => !string.IsNullOrWhiteSpace(Namespace);

    #endregion

    #region Division

    /// <summary>The division id</summary>
    public int DivisionId { get; }

    #endregion

    #region Culture

    /// <summary>The payroll culture</summary>
    public string PayrollCulture { get; }

    #endregion

    #region Calendar

    /// <summary>Gets the calendar period</summary>
    /// <param name="moment">Moment within the period (default: today)</param>
    /// <param name="offset">The period offset (default: 0/current)</param>
    /// <returns>The period start and end date</returns>
    public DatePeriod GetCalendarPeriod(DateTime? moment = null, int offset = 0) =>
        GetCalendarPeriod(GetDerivedCalendar(DivisionId, EmployeeId ?? 0), moment,
            offset, GetDerivedCulture(DivisionId, EmployeeId ?? 0));

    /// <summary>
    /// Count the calendar days from a date period
    /// </summary>
    /// <param name="culture">The calendar culture</param>
    public int GetCalendarDayCount(string culture = null) =>
        GetCalendarDayCount(GetDerivedCalendar(DivisionId, EmployeeId ?? 0),
            PeriodStart, PeriodEnd, culture);

    #endregion

    #region Employee

    /// <summary>The employee id</summary>
    public int? EmployeeId { get; }

    /// <summary>The employee identifier</summary>
    [ActionProperty("Employee identifier")]
    public string EmployeeIdentifier { get; }

    /// <summary>Get employee attribute value</summary>
    public object GetEmployeeAttribute(string attributeName) =>
        Runtime.GetEmployeeAttribute(attributeName);

    /// <summary>Get employee attribute typed value</summary>
    public T GetEmployeeAttribute<T>(string attributeName, T defaultValue = default) =>
        ChangeValueType(GetEmployeeAttribute(attributeName), defaultValue);

    #endregion

    #region Cycle

    /// <summary>The current cycle start date</summary>
    [ActionProperty("Cycle start date")]
    public DateTime CycleStart => Cycle.Start;

    /// <summary>The current cycle start year</summary>
    [ActionProperty("Cycle start year")]
    public int CycleStartYear => CycleStart.Year;

    /// <summary>The current cycle start month</summary>
    [ActionProperty("Cycle start month")]
    public int CycleStartMonth => CycleStart.Month;

    /// <summary>The current cycle start day</summary>
    [ActionProperty("Cycle start day")]
    public int CycleStartDay => CycleStart.Day;

    /// <summary>The current cycle end date</summary>
    [ActionProperty("Cycle end date")]
    public DateTime CycleEnd => Cycle.End;

    /// <summary>The current cycle end year</summary>
    [ActionProperty("Cycle end year")]
    public int CycleEndYear => CycleEnd.Year;

    /// <summary>The current cycle end month</summary>
    [ActionProperty("Cycle end month")]
    public int CycleEndMonth => CycleEnd.Month;

    /// <summary>The current cycle end day</summary>
    [ActionProperty("Cycle edn day")]
    public int CycleEndDay => CycleEnd.Day;

    /// <summary>The current cycle duration</summary>
    [ActionProperty("Cycle duration")]
    public TimeSpan CycleDuration => Cycle.Duration;

    /// <summary>The current cycle</summary>
    public DatePeriod Cycle { get; }

    /// <summary>The day count of the current cycle</summary>
    [ActionProperty("Cycle day count")]
    public int CycleDays => (int)Math.Round(Cycle.TotalDays);

    /// <summary>The previous cycle</summary>
    public DatePeriod PreviousCycle { get; }

    /// <summary>The next cycle</summary>
    public DatePeriod NextCycle { get; }

    /// <summary>Get cycle by offset to the current cycle</summary>
    /// <param name="offset">The cycle offset: 0=current, -1=previous, 1=next</param>
    /// <returns>The offset cycle</returns>
    public DatePeriod GetCycle(int offset = 0) =>
        GetCycle(EvaluationPeriod.Start, offset);

    /// <summary>Get cycle by moment</summary>
    /// <param name="moment">The cycle moment</param>
    /// <param name="offset">The cycle offset: 0=current, -1=previous, 1=next</param>
    /// <returns>The cycle including the moment</returns>
    public DatePeriod GetCycle(DateTime moment, int offset = 0)
    {
        var (start, end) = (Tuple<DateTime, DateTime>)Runtime.GetCycle(moment, offset);
        return new(start, end);
    }

    /// <summary>Get periods by offset range</summary>
    /// <param name="startOffset">The offset to the first period</param>
    /// <param name="endOffset">The offset to the last period</param>
    /// <returns>The periods ordered by date</returns>
    public List<DatePeriod> GetPeriods(int startOffset, int endOffset)
    {
        if (startOffset > endOffset)
        {
            throw new ArgumentOutOfRangeException(nameof(startOffset));
        }
        var periods = new List<DatePeriod>();
        for (var offset = startOffset; offset <= endOffset; offset++)
        {
            periods.Add(GetPeriod(offset));
        }
        return periods;
    }

    /// <summary>Get cycle periods</summary>
    /// <returns>The cycle periods ordered by date</returns>
    public List<DatePeriod> GetCyclePeriods() =>
        GetPeriods(CycleStartOffset, CycleEndOffset);

    /// <summary>Get past cycle periods from the first cycle period to the current period</summary>
    /// <param name="includeCurrent">Include the current period</param>
    /// <returns>The past cycle periods ordered by date</returns>
    public List<DatePeriod> GetPastCyclePeriods(bool includeCurrent = true) =>
        GetPeriods(CycleStartOffset, includeCurrent ? 0 : -1);

    /// <summary>Get future cycle periods from the current period to the last cycle period</summary>
    /// <param name="includeCurrent">Include the current period</param>
    /// <returns>The future cycle periods ordered by date</returns>
    public List<DatePeriod> GetFutureCyclePeriods(bool includeCurrent = true) =>
        GetPeriods(includeCurrent ? 0 : 1, CycleEndOffset);

    /// <summary>Test if a date is the first cycle day</summary>
    /// <param name="moment">The cycle moment</param>
    /// <returns>True for the first cycle day</returns>
    public bool IsFirstCycleDay(DateTime moment) =>
        moment.IsSameDay(CycleStart);

    /// <summary>Test if a date is the last cycle day</summary>
    /// <param name="moment">The cycle moment</param>
    /// <returns>Tru for the last cycle day</returns>
    public bool IsLastCycleDay(DateTime moment) =>
        moment.IsSameDay(CycleEnd);

    #endregion

    #region Date/Period

    /// <summary>The evaluation date</summary>
    [ActionProperty("Evaluation date")]
    public DateTime EvaluationDate { get; }

    /// <summary>The evaluation period</summary>
    public DatePeriod EvaluationPeriod { get; }

    /// <summary>Periods by offset</summary>
    public ScriptDictionary<int, DatePeriod> Periods { get; }

    /// <summary>The current period start date</summary>
    [ActionProperty("Period start date")]
    public DateTime PeriodStart => Period.Start;

    /// <summary>The current period start year</summary>
    [ActionProperty("Period start year")]
    public int PeriodStartYear => PeriodStart.Year;

    /// <summary>The current period start month</summary>
    [ActionProperty("Period start month")]
    public int PeriodStartMonth => PeriodStart.Month;
    
    /// <summary>The current period start day</summary>
    [ActionProperty("Period start day")]
    public int PeriodStartDay => PeriodStart.Day;

    /// <summary>The current period end date</summary>
    [ActionProperty("Period end date")]
    public DateTime PeriodEnd => Period.End;

    /// <summary>The current period end year</summary>
    [ActionProperty("Period end year")]
    public int PeriodEndYear => PeriodEnd.Year;

    /// <summary>The current period end month</summary>
    [ActionProperty("Period end month")]
    public int PeriodEndMonth => PeriodEnd.Month;
    
    /// <summary>The current period end day</summary>
    [ActionProperty("Period end day")]
    public int PeriodEndDay => PeriodEnd.Day;

    /// <summary>The current period duration</summary>
    [ActionProperty("Period duration")]
    public TimeSpan PeriodDuration => Period.Duration;

    /// <summary>The current period</summary>
    public DatePeriod Period { get; }

    /// <summary>The day count of the current period</summary>
    [ActionProperty("Period day count")]
    public int PeriodDays => (int)Math.Round(Period.TotalDays);

    /// <summary>The previous period</summary>
    public DatePeriod PreviousPeriod { get; }

    /// <summary>The next period</summary>
    public DatePeriod NextPeriod { get; }

    /// <summary>Test for the first cycle period</summary>
    public bool FirstCyclePeriod => CycleStart == PeriodStart;

    /// <summary>Test for the last cycle period</summary>
    public bool LastCyclePeriod => PeriodEnd == CycleEnd;

    /// <summary>Offset of the current period to the start of the current cycle,<br />
    /// 0 for the first cycle period, -1 for the previous period, and so on</summary>
    public int CycleStartOffset { get; }

    /// <summary>Offset of the current period to the end of the current cycle,<br />
    /// 0 for the ultimate cycle period, 1 for the penultimate cycle period, and so on</summary>
    public int CycleEndOffset { get; }

    /// <summary>The number of periods from the cycle start of the current period</summary>
    public int PastCyclePeriods => Math.Abs(CycleStartOffset);

    /// <summary>The number of periods from the current period to the cycle end</summary>
    public int FutureCyclePeriods => CycleEndOffset;

    /// <summary>The number of periods within a cycle</summary>
    public int PeriodsInCycle => CycleStartOffset + CycleEndOffset + 1;

    /// <summary>Get period before the evaluation date</summary>
    /// <returns>Period from the minimal date until the evaluation date</returns>
    public DatePeriod PastPeriod() => new(Date.MinValue, EvaluationDate.PreviousTick());

    /// <summary>Get period after the evaluation date</summary>
    /// <returns>Period from the evaluation date until the maximal date</returns>
    public DatePeriod FuturePeriod() => new(EvaluationDate.NextTick(), Date.MaxValue);

    /// <summary>Get period by offset to the current period</summary>
    /// <param name="offset">The period offset: 0=current, -1=previous, 1=next</param>
    /// <returns>The offset period</returns>
    public DatePeriod GetPeriod(int offset = 0) =>
        GetPeriod(EvaluationPeriod.Start, offset);

    /// <summary>Get period by moment</summary>
    /// <param name="moment">The period moment</param>
    /// <param name="offset">The period offset: 0=current, -1=previous, 1=next</param>
    /// <returns>The period including the moment</returns>
    public DatePeriod GetPeriod(DateTime moment, int offset = 0)
    {
        var (start, end) = (Tuple<DateTime, DateTime>)Runtime.GetPeriod(moment, offset);
        return new(start, end);
    }

    /// <summary>Get offset to period</summary>
    /// <param name="moment">The period moment</param>
    /// <returns>The period offset</returns>
    public int GetPeriodOffset(DateTime moment)
    {
        // current period
        if (Period.IsWithin(moment))
        {
            return 0;
        }

        var offset = 0;
        if (Period.IsBefore(moment))
        {
            // past period
            var periodStart = PeriodStart;
            while (periodStart > moment)
            {
                offset--;
                // previous period
                periodStart = GetPeriod(offset).Start;
            }
        }
        else
        {
            // future period
            var periodEnd = PeriodEnd;
            while (periodEnd < moment)
            {
                offset++;
                // next period
                periodEnd = GetPeriod(offset).End;
            }
        }
        return offset;
    }

    /// <summary>Test if a date is the first period day</summary>
    /// <param name="moment">The period moment</param>
    /// <returns>True for the first period day</returns>
    public bool IsFirstPeriodDay(DateTime moment) =>
        moment.IsSameDay(PeriodStart);

    /// <summary>Test if a date is the last period day</summary>
    /// <param name="moment">The period moment</param>
    /// <returns>Tru for the last period day</returns>
    public bool IsLastPeriodDay(DateTime moment) =>
        moment.IsSameDay(PeriodEnd);

    #endregion

    #region Case Value

    /// <summary>Get case value type</summary>
    /// <param name="caseFieldName">The case field name</param>
    /// <returns>The case value type</returns>
    public ValueType? GetCaseValueType(string caseFieldName)
    {
        int? valueType = Runtime.GetCaseValueType(caseFieldName);
        if (valueType == null || !Enum.IsDefined(typeof(ValueType), valueType.Value))
        {
            return null;
        }
        return (ValueType)valueType.Value;
    }

    /// <summary>Test for existing case field attribute</summary>
    /// <param name="caseFieldName">Name of the case field</param>
    /// <param name="attributeName">The attribute name</param>
    public bool HasCaseFieldAttribute(string caseFieldName, string attributeName) =>
        GetCaseFieldAttribute(caseFieldName, attributeName) != null;

    /// <summary>Get case field attribute value</summary>
    /// <param name="caseFieldName">Name of the case field</param>
    /// <param name="attributeName">The attribute name</param>
    public object GetCaseFieldAttribute(string caseFieldName, string attributeName) =>
        Runtime.GetCaseFieldAttribute(caseFieldName, attributeName);

    /// <summary>Get case attribute typed value</summary>
    /// <param name="caseFieldName">Name of the case field</param>
    /// <param name="attributeName">The attribute name</param>
    /// <param name="defaultValue">The default value</param>
    public T GetCaseFieldAttribute<T>(string caseFieldName, string attributeName, T defaultValue = default) =>
        ChangeValueType(GetCaseFieldAttribute(caseFieldName, attributeName), defaultValue);

    /// <summary>Test for existing case field value attribute</summary>
    /// <param name="caseFieldName">Name of the case field</param>
    /// <param name="attributeName">The attribute name</param>
    public bool HasCaseValueAttribute(string caseFieldName, string attributeName) =>
        GetCaseValueAttribute(caseFieldName, attributeName) != null;

    /// <summary>Get case field value attribute value</summary>
    /// <param name="caseFieldName">Name of the case field</param>
    /// <param name="attributeName">The attribute name</param>
    public object GetCaseValueAttribute(string caseFieldName, string attributeName) =>
        Runtime.GetCaseValueAttribute(caseFieldName, attributeName);

    /// <summary>Get case field value attribute typed value</summary>
    /// <param name="caseFieldName">Name of the case field</param>
    /// <param name="attributeName">The attribute name</param>
    /// <param name="defaultValue">The default value</param>
    public T GetCaseValueAttribute<T>(string caseFieldName, string attributeName, T defaultValue = default) =>
        ChangeValueType(GetCaseValueAttribute(caseFieldName, attributeName), defaultValue);

    /// <summary>Test for available cases from the current period</summary>
    /// <param name="caseFieldNames">The name of the case fields to test</param>
    /// <returns>True if all case values are available</returns>
    public bool TestAvailableCaseValues(IEnumerable<string> caseFieldNames) =>
        TestAvailableCaseValues(Period, caseFieldNames);

    /// <summary>Test for available cases</summary>
    /// <param name="period">The value period</param>
    /// <param name="caseFieldNames">The name of the case fields to test</param>
    /// <returns>True if all case values are available</returns>
    public bool TestAvailableCaseValues(DatePeriod period, IEnumerable<string> caseFieldNames) =>
        GetFirstUnavailableCaseValue(period, caseFieldNames) == null;

    /// <summary>Get the first available case value from the current period</summary>
    /// <param name="caseFieldNames">The name of the case fields to test</param>
    /// <returns>The first available case value, otherwise null</returns>
    public CasePayrollValue GetFirstAvailableCaseValue(IEnumerable<string> caseFieldNames) =>
        GetFirstAvailableCaseValue(Period, caseFieldNames);

    /// <summary>Get the first available case value within a time period</summary>
    /// <param name="period">The value period</param>
    /// <param name="caseFieldNames">The name of the case fields to test</param>
    /// <returns>The first available case value, otherwise null</returns>
    public CasePayrollValue GetFirstAvailableCaseValue(DatePeriod period, IEnumerable<string> caseFieldNames)
    {
        foreach (var caseFieldName in caseFieldNames)
        {
            var value = GetPeriodCaseValue(period, caseFieldName);
            if (value != null && value.Any())
            {
                return value;
            }
        }
        return null;
    }

    /// <summary>Get the first unavailable case value from the current period</summary>
    /// <param name="caseFieldNames">The name of the case fields to test</param>
    /// <returns>The case field name of the first unavailable case value, otherwise null</returns>
    public string GetFirstUnavailableCaseValue(IEnumerable<string> caseFieldNames) =>
        GetFirstUnavailableCaseValue(Period, caseFieldNames);

    /// <summary>Get the first unavailable case value within a time period</summary>
    /// <param name="period">The value period</param>
    /// <param name="caseFieldNames">The name of the case fields to test</param>
    /// <returns>The case field name of the first unavailable case value, otherwise null</returns>
    public string GetFirstUnavailableCaseValue(DatePeriod period, IEnumerable<string> caseFieldNames)
    {
        foreach (var caseFieldName in caseFieldNames)
        {
            var value = GetPeriodCaseValue(period, caseFieldName);
            if (value == null || !value.Any())
            {
                return caseFieldName;
            }
        }
        return null;
    }

    /// <summary>Get the case field name including the case slot</summary>
    /// <param name="caseFieldName">The case field name</param>
    /// <param name="caseSlot">The case slot</param>
    /// <returns>The case field slot name</returns>
    public string CaseFieldSlot(string caseFieldName, string caseSlot)
    {
        if (string.IsNullOrWhiteSpace(caseFieldName) || caseFieldName.Contains(StringExtensions.CaseFieldSlotSeparator))
        {
            throw new ArgumentException(nameof(caseFieldName));
        }
        if (string.IsNullOrWhiteSpace(caseSlot) || caseSlot.Contains(StringExtensions.CaseFieldSlotSeparator))
        {
            throw new ArgumentException(nameof(caseSlot));
        }
        return $"{caseFieldName}{StringExtensions.CaseFieldSlotSeparator}{caseSlot}";
    }

    /// <summary>Get the case payroll value by case field name</summary>
    public ScriptDictionary<string, CasePayrollValue> CaseValue { get; }

    /// <summary>Get the case value tags by case field name</summary>
    public ScriptDictionary<string, List<string>> CaseValueTags { get; }

    /// <summary>Get the case payroll typed value by case field name</summary>
    /// <param name="period">The value period</param>
    /// <param name="caseFieldName">The case field name</param>
    /// <param name="caseSlot">The case slot</param>
    /// <returns>A case value from the period</returns>
    public T GetPeriodCaseValue<T>(DatePeriod period, string caseFieldName, string caseSlot = null) =>
        GetPeriodCaseValue(period, caseFieldName, caseSlot).ValueAs<T>();

    /// <summary>Get the case payroll value by case field name</summary>
    /// <param name="period">The value period</param>
    /// <param name="caseFieldName">The case field name</param>
    /// <param name="caseSlot">The case slot</param>
    /// <returns>A case value from the period</returns>
    public CasePayrollValue GetPeriodCaseValue(DatePeriod period, string caseFieldName, string caseSlot = null)
    {
        if (!string.IsNullOrWhiteSpace(caseSlot))
        {
            caseFieldName = CaseFieldSlot(caseFieldName, caseSlot);
        }
        var periodValues = GetPeriodCaseValues(period, caseFieldName);
        return periodValues.Count == 1 ? periodValues[caseFieldName] : new(caseFieldName);
    }

    /// <summary>Get multiple case values of a date period</summary>
    /// <param name="period">The date period</param>
    /// <param name="caseFieldNames">The case field names</param>
    /// <returns>Dictionary of case values grouped by case field name</returns>
    public CasePayrollValueDictionary GetPeriodCaseValues(DatePeriod period, params string[] caseFieldNames) =>
        TupleExtensions.TupleToCaseValuesDictionary(Runtime.GetCasePeriodValues(period.Start, period.End, caseFieldNames));

    /// <summary>Get multiple case values of an offset period</summary>
    /// <param name="periodOffset">The offset period</param>
    /// <param name="caseFieldNames">The case field names</param>
    /// <returns>Dictionary of case values grouped by case field name</returns>
    public CasePayrollValueDictionary GetPeriodCaseValues(int periodOffset, params string[] caseFieldNames) =>
        GetPeriodCaseValues(GetPeriod(periodOffset), caseFieldNames);

    /// <summary>Get case payroll typed value of an offset period</summary>
    /// <param name="periodOffset">The offset period</param>
    /// <param name="caseFieldName">The case field name</param>
    /// <param name="caseSlot">The case slot</param>
    /// <returns>A case value from the offset period</returns>
    public T GetPeriodCaseValue<T>(int periodOffset, string caseFieldName, string caseSlot = null) =>
        GetPeriodCaseValue(periodOffset, caseFieldName, caseSlot).ValueAs<T>();

    /// <summary>Get case payroll value of an offset period</summary>
    /// <param name="periodOffset">The offset period</param>
    /// <param name="caseFieldName">The case field name</param>
    /// <param name="caseSlot">The case slot</param>
    /// <returns>A case value from the offset period</returns>
    public CasePayrollValue GetPeriodCaseValue(int periodOffset, string caseFieldName, string caseSlot = null) =>
        GetPeriodCaseValue(GetPeriod(periodOffset), caseFieldName, caseSlot);

    /// <summary>Get case payroll typed value of the current period</summary>
    /// <param name="caseFieldName">The case field name</param>
    /// <param name="caseSlot">The case slot</param>
    /// <returns>A case value from the current period</returns>
    public T GetCaseValue<T>(string caseFieldName, string caseSlot = null) =>
        GetCaseValue(caseFieldName, caseSlot).ValueAs<T>();

    /// <summary>Get case payroll value of the current period</summary>
    /// <param name="caseFieldName">The case field name</param>
    /// <param name="caseSlot">The case slot</param>
    /// <returns>A case value from the current period</returns>
    public CasePayrollValue GetCaseValue(string caseFieldName, string caseSlot = null) =>
        GetPeriodCaseValue(Period, caseFieldName, caseSlot);

    /// <summary>Get case payroll value of multiple periods</summary>
    /// <param name="periodStartOffset">Offset of start period</param>
    /// <param name="periodEndOffset">Offset of end period</param>
    /// <param name="caseFieldName">The case field name</param>
    /// <param name="caseSlot">The case slot</param>
    /// <returns>Dictionary of case values grouped by periods</returns>
    public PeriodCasePayrollValueDictionary GetPeriodCaseValue(int periodStartOffset,
        int periodEndOffset, string caseFieldName, string caseSlot = null)
    {
        if (periodEndOffset < periodStartOffset)
        {
            throw new ArgumentOutOfRangeException(nameof(periodEndOffset));
        }
        var values = new Dictionary<DatePeriod, CasePayrollValue>();
        for (var offset = periodStartOffset; offset <= periodEndOffset; offset++)
        {
            var period = GetPeriod(offset);
            values.Add(period, GetPeriodCaseValue(period, caseFieldName, caseSlot));
        }
        return new(values);
    }

    /// <summary>Get multiple case values of multiple periods</summary>
    /// <param name="periodStartOffset">Offset of start period</param>
    /// <param name="periodEndOffset">Offset of end period</param>
    /// <param name="caseFieldNames">The case field names</param>
    /// <returns>Dictionary of multiple case values grouped by period</returns>
    public MultiPeriodCasePayrollValueDictionary GetMultiPeriodCaseValues(int periodStartOffset,
        int periodEndOffset, params string[] caseFieldNames)
    {
        if (periodEndOffset < periodStartOffset)
        {
            throw new ArgumentOutOfRangeException(nameof(periodEndOffset));
        }
        var values = new Dictionary<DatePeriod, CasePayrollValueDictionary>();
        for (var offset = periodStartOffset; offset <= periodEndOffset; offset++)
        {
            var period = GetPeriod(offset);
            values.Add(period, GetPeriodCaseValues(period, caseFieldNames));
        }
        return new(values);
    }

    /// <summary>Get raw case value from a specific date</summary>
    /// <param name="caseFieldName">The case field name</param>
    /// <param name="valueDate">The value date</param>
    /// <returns>Raw case value from a specific date</returns>
    public CaseValue GetRawCaseValue(string caseFieldName, DateTime valueDate) =>
        TupleExtensions.TupleToCaseValue(Runtime.GetCaseValue(caseFieldName, valueDate.ToUtc()));

    /// <summary>Get multiple raw case values from a specific date</summary>
    /// <param name="caseFieldNames">The case field names</param>
    /// <param name="valueDate">The value date</param>
    /// <returns>Raw case value from a specific date</returns>
    public List<CaseValue> GetRawCaseValues(IList<string> caseFieldNames, DateTime valueDate) =>
        TupleExtensions.TupleToCaseValues(Runtime.GetCaseValues(caseFieldNames, valueDate.ToUtc()));

    /// <summary>Get raw payroll value from a specific date</summary>
    /// <param name="caseFieldName">The case field name</param>
    /// <param name="valueDate">The value date</param>
    /// <returns>Raw case value from a specific date</returns>
    public T GetRawCaseValue<T>(string caseFieldName, DateTime valueDate)
    {
        var value = GetRawCaseValue(caseFieldName, valueDate)?.Value;
        return value != null ? value.ValueAs<T>() : default;
    }

    /// <summary>Get raw case values created within a date period</summary>
    /// <param name="caseFieldName">The case field name</param>
    /// <param name="period">The case value creation period</param>
    /// <returns>Raw case values from a date period</returns>
    public List<CaseValue> GetPeriodRawCaseValues(string caseFieldName, DatePeriod period) =>
        GetRawCaseValues(caseFieldName, period.Start, period.End);

    /// <summary>Get raw case values created within a date period</summary>
    /// <param name="caseFieldName">The case field name</param>
    /// <remarks>Case value tags and attributes are not supported</remarks>
    /// <returns>Raw case values from a date period</returns>
    public List<CaseValue> GetPeriodRawCaseValues(string caseFieldName) =>
        GetPeriodRawCaseValues(caseFieldName, Period);

    /// <summary>Get raw case values created within an offset period</summary>
    /// <param name="caseFieldName">The case field name</param>
    /// <param name="periodOffset">The offset period</param>
    /// <returns>Raw case values from an offset period</returns>
    public List<CaseValue> GetPeriodRawCaseValues(string caseFieldName, int periodOffset) =>
        GetPeriodRawCaseValues(caseFieldName, GetPeriod(periodOffset));

    /// <summary>Get raw case values created within the current period</summary>
    /// <param name="caseFieldName">The case field name</param>
    /// <param name="startDate">The date after the case value was created</param>
    /// <param name="endDate">The date before the case value was created</param>
    /// <returns>Raw case values from the current period</returns>
    public List<CaseValue> GetRawCaseValues(string caseFieldName, DateTime? startDate = null, DateTime? endDate = null) =>
        TupleExtensions.TupleToCaseValues(Runtime.GetCaseValues(caseFieldName, startDate, endDate));

    /// <summary>Get case data object with the current period value</summary>
    /// <param name="recursive">Recursive objects (default: true)</param>
    /// <param name="writeable">Writeable values only</param>
    /// <returns>Data object with change data</returns>
    public List<CasePayrollValue> GetCaseObjectValues<T>(bool recursive = true, bool writeable = false) where T : class, ICaseObject, new()
    {
        var values = new List<CasePayrollValue>();
        foreach (var propertyInfo in CaseObject.GetProperties<T>(recursive, writeable))
        {
            var caseValue = GetCaseValue(propertyInfo.CaseFieldName);
            if (caseValue != null)
            {
                values.Add(caseValue);
            }
        }
        return values;
    }

    /// <summary>Get case data object by y moment within the payroll values</summary>
    /// <param name="caseValues">Case payroll values</param>
    /// <param name="moment">Value moment</param>
    /// <returns>Data object with change data</returns>
    public T GetCaseObject<T>(List<CasePayrollValue> caseValues, DateTime moment) where T : class, ICaseObject, new()
    {
        var caseObject = new T();
        foreach (var propertyInfo in CaseObject.GetProperties<T>(writeable: true))
        {
            var caseFieldName = propertyInfo.CaseFieldName;
            var casePayrollValue = caseValues.FirstOrDefault(x => string.Equals(x.CaseFieldName, caseFieldName));
            var value = casePayrollValue?.PeriodValues.FirstOrDefault(x => x.Period.IsWithin(moment))?.Value;
            if (value != null)
            {
                caseObject.SetValue(caseFieldName, value);
            }
        }
        return caseObject;
    }

    /// <summary>Get case object from a specific date</summary>
    /// <param name="valueDate">Value date</param>
    /// <param name="recursive">Recursive objects (default: true)</param>
    /// <returns>Data object with change data</returns>
    public T GetRawCaseObject<T>(DateTime valueDate, bool recursive = true) where T : class, ICaseObject, new()
    {
        var caseObject = new T();

        // retrieve case values
        var caseFieldNames = CaseObject.GetProperties<T>(recursive, writeable: true).Select(x => x.CaseFieldName).ToList();
        var caseValues = GetRawCaseValues(caseFieldNames, valueDate);

        foreach (var propertyInfo in CaseObject.GetProperties<T>(writeable: true))
        {
            var caseValue = caseValues.FirstOrDefault(x => string.Equals(x.CaseFieldName, propertyInfo.CaseFieldName));
            if (caseValue != null)
            {
                caseObject.SetValue(propertyInfo.CaseFieldName, caseValue.Value.Convert(propertyInfo.Property.PropertyType));
            }
        }

        return caseObject;
    }

    /// <summary>Get case object from a date period</summary>
    /// <param name="moments">Value moments</param>
    /// <param name="recursive">Recursive objects (default: true)</param>
    /// <returns>Data object with change data</returns>
    public List<T> GetPeriodRawCaseObjects<T>(List<DateTime> moments, bool recursive = true)
        where T : class, ICaseObject, new()
    {
        var properties = CaseObject.GetProperties<T>(recursive, writeable: true);

        // period values by case field
        var periodValues = new Dictionary<string, List<CaseValue>>();
        foreach (var property in properties)
        {
            var caseFieldName = property.Property.GetCaseFieldName();
            var values = GetPeriodRawCaseValues(caseFieldName);
            periodValues.Add(caseFieldName, values);
        }

        // period objects
        var objects = new List<T>();
        foreach (var moment in moments)
        {
            // moment object
            var obj = new T();
            foreach (var property in properties)
            {
                var caseFieldName = property.Property.GetCaseFieldName();
                var periodValue = periodValues[caseFieldName];
                var value = periodValue.FirstOrDefault(x => x.Created == moment);
                if (value?.Value != null)
                {
                    obj.SetValue(caseFieldName, value.Value.Convert(property.Property.PropertyType));
                }
            }
            objects.Add(obj);
        }
        return objects;
    }

    /// <summary>Get multiple case values of the current period</summary>
    /// <param name="caseFieldNames">The case field names</param>
    /// <returns>Dictionary of case values grouped by case field name</returns>
    public CasePayrollValueDictionary GetCaseValues(params string[] caseFieldNames) =>
        GetPeriodCaseValues(Period, caseFieldNames);

    /// <summary>Get the case value tags from the current period end</summary>
    /// <param name="caseFieldName">Name of the case field</param>
    /// <returns>The case value tags</returns>
    public List<string> GetCaseValueTags(string caseFieldName) =>
        GetCaseValueTags(caseFieldName, PeriodEnd);

    /// <summary>Get the case value tags from specific date</summary>
    /// <param name="caseFieldName">Name of the case field</param>
    /// <param name="valueDate">The value date</param>
    /// <returns>The case value tags</returns>
    public List<string> GetCaseValueTags(string caseFieldName, DateTime valueDate) =>
        Runtime.GetCaseValueTags(caseFieldName, valueDate);

    /// <summary>Get the case value slots</summary>
    /// <param name="caseFieldName">Name of the case field</param>
    /// <returns>The case value slot names</returns>
    public List<string> GetCaseValueSlots(string caseFieldName) =>
        Runtime.GetCaseValueSlots(caseFieldName);

    /// <summary>Get the case slot values, grouped by slot name</summary>
    /// <param name="caseFieldName">The name of the case field</param>
    /// <returns>The case values in a dictionary grouped by slot name</returns>
    public Dictionary<string, CasePayrollValue> GetSlotValues(string caseFieldName)
    {
        var slotValues = new Dictionary<string, CasePayrollValue>();
        var caseValueSlots = GetCaseValueSlots(caseFieldName);
        if (caseValueSlots != null && caseValueSlots.Any())
        {
            foreach (var caseValueSlot in caseValueSlots)
            {
                var caseValue = GetCaseValue(CaseFieldSlot(caseFieldName, caseValueSlot));
                if (caseValue.HasValue)
                {
                    slotValues.Add(caseValueSlot, caseValue);
                }
            }
        }
        return slotValues;
    }

    /// <summary>Get the typed case slot values, grouped by slot name</summary>
    /// <param name="caseFieldName">The name of the case field</param>
    /// <returns>The case values in a dictionary grouped by slot name</returns>
    public Dictionary<string, T> GetSlotValues<T>(string caseFieldName)
    {
        var slotValues = new Dictionary<string, T>();
        var caseValueSlots = GetCaseValueSlots(caseFieldName);
        if (caseValueSlots != null && caseValueSlots.Any())
        {
            foreach (var caseValueSlot in caseValueSlots)
            {
                var caseValue = GetCaseValue(CaseFieldSlot(caseFieldName, caseValueSlot));
                var value = ChangeValueType<T>(caseValue.Value);
                if (value != null)
                {
                    slotValues.Add(caseValueSlot, value);
                }
            }
        }
        return slotValues;
    }

    /// <summary>Get slot name by value</summary>
    /// <param name="caseFieldName">The name of the case field</param>
    /// <param name="value">The slot value</param>
    /// <param name="prefix">The slot prefix</param>
    /// <returns>The slot name matching the value and prefix, otherwise null</returns>
    public string GetSlotByValue(string caseFieldName, string value, string prefix = null)
    {
        var slotValues = GetSlotValues(caseFieldName);
        foreach (var slotValue in slotValues)
        {
            if (prefix != null && !slotValue.Key.StartsWith(prefix))
            {
                continue;
            }
            if (string.Equals(slotValue.Value, value))
            {
                return slotValue.Key;
            }
        }
        return null;
    }

    #endregion

    #region Lookups

    /// <summary>Test for existing lookup</summary>
    /// <param name="lookupName">The lookup name</param>
    /// <returns>True on existing lookup</returns>
    public bool HasLookup(string lookupName) =>
        Runtime.HasLookup(lookupName);

    /// <summary>Get lookup value</summary>
    /// <param name="lookupName">The lookup name</param>
    /// <param name="lookupKey">The lookup key</param>
    /// <param name="culture">The culture, null for the system culture (optional)</param>
    public T GetLookup<T>(string lookupName, string lookupKey, string culture = null)
    {
        var value = Runtime.GetLookup(lookupName, lookupKey, culture) as string;
        if (string.IsNullOrWhiteSpace(value))
        {
            return default;
        }
        // object lookup
        if (typeof(T) != typeof(string) && value.StartsWith("{") && value.EndsWith("}"))
        {
            return value.ConvertJson<T>();
        }
        return (T)Convert.ChangeType(value, typeof(T));
    }

    /// <summary>Get lookup value with multiple keys</summary>
    /// <param name="lookupName">The lookup name</param>
    /// <param name="lookupKeyValues">The lookup key values (serialized to JSON string)</param>
    /// <param name="culture">The culture, null for the system culture (optional)</param>
    public T GetLookup<T>(string lookupName, object[] lookupKeyValues, string culture = null)
    {
        if (lookupKeyValues == null || lookupKeyValues.Length == 0)
        {
            throw new ArgumentException(nameof(lookupKeyValues));
        }
        return GetLookup<T>(lookupName, JsonSerializer.Serialize(lookupKeyValues), culture);
    }

    /// <summary>Get object lookup by range value</summary>
    /// <param name="lookupName">The lookup name</param>
    /// <param name="objectKey">The object key</param>
    /// <param name="lookupKey">The lookup key (optional)</param>
    /// <param name="culture">The culture, null for the system culture (optional)</param>
    public T GetObjectLookup<T>(string lookupName, string objectKey,
        string lookupKey = null, string culture = null)
    {
        var value = GetLookup<string>(lookupName, lookupKey, culture);
        return string.IsNullOrWhiteSpace(value) ? default : value.ObjectValueJson<T>(objectKey);
    }
    
    /// <summary>Get payroll lookup range brackets</summary>
    /// <param name="lookupName">The name of the lookup</param>
    /// <param name="rangeValue">The range value (supported by threshold and progressive lookups)</param>
    /// <returns>List of lookup range brackets</returns>
    public List<LookupRangeBracket> GetLookupRanges(string lookupName, decimal? rangeValue = null) =>
        TupleExtensions.TupleToLookupRangeBracketList(Runtime.GetLookupRanges(lookupName, rangeValue));
    
    /// <summary>Get threshold lookup range bracket</summary>
    /// <param name="lookupName">The name of the lookup</param>
    /// <param name="rangeValue">The range value (supported by threshold and progressive lookups)</param>
    /// <returns>Matching threshold bracket, null on missing range</returns>
    public LookupRangeBracket GetLookupThresholdRange(string lookupName, decimal rangeValue) =>
        GetLookupRanges(lookupName, rangeValue).FirstOrDefault(x => x.RangeValue.HasValue);
    
    /// <summary>Get progressive lookup range brackets</summary>
    /// <param name="lookupName">The name of the lookup</param>
    /// <param name="rangeValue">The range value (supported by threshold and progressive lookups)</param>
    /// <returns>Matching progressive brackets</returns>
    public List<LookupRangeBracket> GetLookupProgressiveRanges(string lookupName, decimal rangeValue) =>
        GetLookupRanges(lookupName, rangeValue).Where(x => x.RangeValue.HasValue).ToList();

    /// <summary>Get lookup by range value</summary>
    /// <param name="lookupName">The lookup name</param>
    /// <param name="rangeValue">The range value</param>
    /// <param name="lookupKey">The lookup key (optional)</param>
    /// <param name="culture">The culture, null for the system culture (optional)</param>
    public T GetRangeLookup<T>(string lookupName, decimal rangeValue, string lookupKey = null, string culture = null)
    {
        var value = Runtime.GetRangeLookup(lookupName, rangeValue, lookupKey, culture) as string;
        if (string.IsNullOrWhiteSpace(value))
        {
            return default;
        }
        // object lookup
        if (typeof(T) != typeof(string) && value.StartsWith("{") && value.EndsWith("}"))
        {
            return value.ConvertJson<T>();
        }
        return (T)Convert.ChangeType(value, typeof(T));
    }

    /// <summary>Get object lookup by range value</summary>
    /// <param name="lookupName">The lookup name</param>
    /// <param name="rangeValue">The range value</param>
    /// <param name="objectKey">The object key</param>
    /// <param name="lookupKey">The lookup key (optional)</param>
    /// <param name="culture">The culture, null for the system culture (optional)</param>
    public T GetRangeObjectLookup<T>(string lookupName, decimal rangeValue, string objectKey,
        string lookupKey = null, string culture = null)
    {
        var value = GetRangeLookup<string>(lookupName, rangeValue, lookupKey, culture);
        return string.IsNullOrWhiteSpace(value) ? default : value.ObjectValueJson<T>(objectKey);
    }

    /// <summary>Apply a range value to the lookup ranges, and multiplying the lookup value with the range amount</summary>
    /// <param name="lookupName">The lookup name</param>
    /// <param name="rangeValue">The range value</param>
    /// <param name="valueFieldName">Value field name</param>
    /// <remarks>Only numeric JSON lookup values are supported.
    /// The first lookup range value must be zero.</remarks>
    /// <returns>Summary of all lookup ranges</returns>
    public decimal ApplyRangeValue(string lookupName, decimal rangeValue, string valueFieldName = null) =>
        Runtime.ApplyRangeValue(lookupName, rangeValue, valueFieldName);

    #endregion

    #region Issue

    /// <summary>
    /// Get issue from attribute
    /// </summary>
    /// <param name="attributeName">Attribute name</param>
    /// <param name="parameters">Message parameters</param>
    public string GetAttributeIssue(string attributeName, params object[] parameters)
    {
        // issue attribute
        var attribute = FindIssueAttribute(attributeName);
        if (attribute == null)
        {
            throw new ScriptException($"Missing action issue attribute {attributeName} on type {GetType()}.");
        }
        if (attribute.ParameterCount != parameters.Length)
        {
            throw new ScriptException($"Mismatching action parameter count on issue attribute {attributeName} (expected={attribute.ParameterCount}, actual={parameters.Length}.");
        }

        // localized message from lookup
        var format = GetLocalIssueMessage(attribute.Name, attribute.Message);
        //context.Function.LogWarning($"AddIssue: format={format}, parameters={string.Join(",", parameters)}");
        var message = string.Format(format, parameters);
        return message;
    }

    private ActionIssueAttribute FindIssueAttribute(string issueName)
    {
        foreach (var method in GetType().GetMethods())
        {
            // issue attribute
            var attribute = method.GetCustomAttributes<ActionIssueAttribute>()
                .FirstOrDefault(x => string.Equals(x.Name, issueName));
            if (attribute != null)
            {
                return attribute;
            }
        }
        return null;
    }

    /// <summary>Get localized issue value</summary>
    /// <param name="key">The issue key</param>
    /// <param name="defaultMessage">The default message</param>
    /// <remarks>Lookup name=Namespace.Actions, lookup key=issue name
    /// Format example: Invalid Email (1)</remarks>
    private string GetLocalIssueMessage(string key, string defaultMessage)
    {
        var lookupName = Namespace.EnsureEnd(".Action");
        //LogInfo($"GetIssueMessage: lookupName={lookupName}, localizationKey={key}");
        var message = GetLookup<string>(lookupName, key, UserCulture) ?? defaultMessage;

        // prepare parameter placeholders for string format
        for (var i = 0; i < 10; i++)
        {
            message = message.Replace($"({i})", $"{{{i}}}");
        }
        return message;
    }

    #endregion
}