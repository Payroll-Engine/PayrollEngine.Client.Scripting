/* PayrollFunction */

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.Json;

namespace PayrollEngine.Client.Scripting.Function;

#region Action

/// <summary>Action context</summary>
public interface IActionContext
{
    /// <summary>The function</summary>
    PayrollFunction Function { get; }
}

/// <summary>Condition action context</summary>
public interface IConditionActionContext : IActionContext
{
    /// <summary>The issues</summary>
    public List<ActionIssue> Issues { get; }
}

/// <summary>Action issue</summary>
public class ActionIssue
{
    /// <summary>The issue message</summary>
    public string Message { get; }

    /// <summary>The localization key</summary>
    public string LocalizationKey { get; }

    /// <summary>The validation issue parameters</summary>
    public object[] Parameters { get; }

    /// <summary>Constructor</summary>
    public ActionIssue(string message, string localizationKey = null,
        params object[] parameters)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            throw new ArgumentException(nameof(message));
        }

        Message = message;
        LocalizationKey = localizationKey;
        Parameters = parameters;
    }
}

/// <summary>Condition action context</summary>
public abstract class ConditionActionContextBase<TFunc> : IConditionActionContext
    where TFunc : PayrollFunction
{
    /// <summary>The function</summary>
    public TFunc Function { get; }
    PayrollFunction IActionContext.Function => Function;

    /// <summary>The action issues</summary>
    public List<ActionIssue> Issues { get; } = [];

    /// <summary>Constructor</summary>
    protected ConditionActionContextBase(TFunc function)
    {
        Function = function ?? throw new ArgumentNullException(nameof(function));
    }

    /// <summary>Test for any issue</summary>
    public bool HasIssues => Issues.Any();

    /// <summary>Add an action issue</summary>
    public void AddIssue(string message) =>
        Issues.Add(new(message));

    /// <summary>Clear all issues</summary>
    public void ClearIssues() =>
        Issues.Clear();
}

/// <summary>Payroll action context</summary>
public abstract class PayrollActionContext<TFunc> : ConditionActionContextBase<TFunc>
    where TFunc : PayrollFunction
{
    /// <summary>Constructor</summary>
    /// <param name="function">The function</param>
    protected PayrollActionContext(TFunc function) :
        base(function)
    {
    }
}

#endregion

/// <summary>Base class for any script function</summary>
// ReSharper disable once PartialTypeWithSinglePart
public abstract partial class PayrollFunction : Function
{
    /// <summary>New function instance</summary>
    /// <param name="runtime">The function runtime</param>
    protected PayrollFunction(object runtime) :
        base(runtime)
    {
        // payroll
        PayrollId = Runtime.PayrollId;

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

    #region Payrol

    /// <summary>The payroll id</summary>
    public int PayrollId { get; }

    #endregion

    
    #region Culture

    /// <summary>The payroll culture</summary>
    public string PayrollCulture { get; }

    #endregion


    #region Employee

    /// <summary>The employee id</summary>
    public int? EmployeeId { get; }

    /// <summary>The employee identifier</summary>
    public string EmployeeIdentifier { get; }

    /// <summary>Get employee attribute value</summary>
    public object GetEmployeeAttribute(string attributeName) =>
        Runtime.GetEmployeeAttribute(attributeName);

    /// <summary>Get employee attribute typed value</summary>
    public T GetEmployeeAttribute<T>(string attributeName, T defaultValue = default)
    {
        var value = GetEmployeeAttribute(attributeName);
        return value == null ? defaultValue : (T)Convert.ChangeType(value, typeof(T));
    }

    #endregion

    #region Cycle

    /// <summary>The current cycle start date</summary>
    public DateTime CycleStart => Cycle.Start;

    /// <summary>The current cycle end date</summary>
    public DateTime CycleEnd => Cycle.End;

    /// <summary>The current cycle</summary>
    public DatePeriod Cycle { get; }

    /// <summary>The day count of the current cycle</summary>
    public double CycleDays => Cycle.TotalDays;

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
    public DateTime EvaluationDate { get; }

    /// <summary>The evaluation period</summary>
    public DatePeriod EvaluationPeriod { get; }

    /// <summary>Periods by offset</summary>
    public ScriptDictionary<int, DatePeriod> Periods { get; }

    /// <summary>The current period start date</summary>
    public DateTime PeriodStart => Period.Start;

    /// <summary>The current period end date</summary>
    public DateTime PeriodEnd => Period.End;

    /// <summary>The current period</summary>
    public DatePeriod Period { get; }

    /// <summary>The day count of the current period</summary>
    public double PeriodDays => Period.TotalDays;

    /// <summary>The previous period</summary>
    public DatePeriod PreviousPeriod { get; }

    /// <summary>The next period</summary>
    public DatePeriod NextPeriod { get; }

    /// <summary>True for the first cycle period</summary>
    public bool FirstCyclePeriod => CycleStart == PeriodStart;

    /// <summary>True for the last cycle period</summary>
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
    public T GetCaseFieldAttribute<T>(string caseFieldName, string attributeName, T defaultValue = default)
    {
        var value = GetCaseFieldAttribute(caseFieldName, attributeName);
        return value == null ? defaultValue : (T)Convert.ChangeType(value, typeof(T));
    }

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
    public T GetCaseValueAttribute<T>(string caseFieldName, string attributeName, T defaultValue = default)
    {
        var value = GetCaseValueAttribute(caseFieldName, attributeName);
        return value == null ? defaultValue : (T)Convert.ChangeType(value, typeof(T));
    }

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

    /// <summary>Get raw case values created within a date period</summary>
    /// <param name="caseFieldName">The case field name</param>
    /// <param name="period">The case value creation period</param>
    /// <returns>Raw case values from a date period</returns>
    public List<CaseValue> GetRawCaseValues(string caseFieldName, DatePeriod period) =>
        GetRawCaseValues(caseFieldName, period.Start, period.End);

    /// <summary>Get raw case values created within a date period</summary>
    /// <param name="caseFieldName">The case field name</param>
    /// <remarks>Case value tags and attributes are not supported</remarks>
    /// <returns>Raw case values from a date period</returns>
    public List<CaseValue> GetPeriodRawCaseValues(string caseFieldName) =>
        GetRawCaseValues(caseFieldName, Period);

    /// <summary>Get raw case values created within an offset period</summary>
    /// <param name="caseFieldName">The case field name</param>
    /// <param name="periodOffset">The offset period</param>
    /// <returns>Raw case values from an offset period</returns>
    public List<CaseValue> GetRawCaseValues(string caseFieldName, int periodOffset) =>
        GetRawCaseValues(caseFieldName, GetPeriod(periodOffset));

    /// <summary>Get raw case values created within the current period</summary>
    /// <param name="caseFieldName">The case field name</param>
    /// <param name="startDate">The date after the case value was created</param>
    /// <param name="endDate">The date before the case value was created</param>
    /// <returns>Raw case values from the current period</returns>
    public List<CaseValue> GetRawCaseValues(string caseFieldName, DateTime? startDate = null, DateTime? endDate = null) =>
        TupleExtensions.TupleToCaseValues(Runtime.GetCaseValues(caseFieldName, startDate, endDate));

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
                if (caseValue.HasValue)
                {
                    var value = (T)Convert.ChangeType(caseValue.Value, typeof(T));
                    if (value != null)
                    {
                        slotValues.Add(caseValueSlot, value);
                    }
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

    #endregion

    #region Actions

    /// <summary>Condition consequent marker</summary>
    private const char ConditionConsequentMarker = '?';

    /// <summary>Condition alternative marker</summary>
    private const char ConditionAlternativeMarker = ':';

    /// <summary>Condition action separator</summary>
    private const char ConditionActionSeparator = ';';

    /// <summary>Function action, using extension methods</summary>
    private sealed class Action
    {
        internal string Namespace { get; init; }
        internal string MethodName { get; init; }
        internal string MethodParameters { get; init; }

        public override string ToString() => $"{Namespace}.{MethodName}({MethodParameters})";
    }

    private sealed class ConditionNode
    {
        private string Expression { get; }
        internal string ConditionAction { get; }
        internal List<ConditionNode> ConsequentNodes { get; }
        internal List<ConditionNode> AlternativeNodes { get; }

        internal bool IsCondition => ConsequentNodes != null || AlternativeNodes != null;
        internal bool IsAction => !IsCondition;

        internal ConditionNode(string conditionAction)
        {
            if (string.IsNullOrWhiteSpace(conditionAction))
            {
                throw new ArgumentException(nameof(conditionAction));
            }
            Expression = conditionAction;
            ConditionAction = conditionAction;
        }

        internal ConditionNode(string expression, string conditionAction,
            List<ConditionNode> consequentNodes, List<ConditionNode> alternativeNodes)
        {
            if (string.IsNullOrWhiteSpace(expression))
            {
                throw new ArgumentException(nameof(expression));
            }
            if (string.IsNullOrWhiteSpace(conditionAction))
            {
                throw new ArgumentException(nameof(conditionAction));
            }
            Expression = expression;
            ConditionAction = conditionAction;
            if ((consequentNodes == null || !consequentNodes.Any()) &&
                (alternativeNodes == null || !alternativeNodes.Any()))
            {
                throw new ArgumentException(nameof(consequentNodes));
            }
            ConsequentNodes = consequentNodes;
            AlternativeNodes = alternativeNodes;
        }

        public override string ToString() => Expression;
    }

    /// <summary>Function action cache</summary>
    private static class ActionCache
    {
        // action cache
        // key: namespace + method name
        // value: tuple
        //   - provider attribute (target type and namespace)
        //   - action attribute (attribute type and method name)
        private static readonly Dictionary<string, Tuple<ActionProviderAttribute, ActionAttribute, MethodInfo>> Methods = new();

        internal static MethodInfo GetActionMethod<TAttribute>(Type functionType, Action action)
            where TAttribute : ActionAttribute
        {
            if (functionType == null)
            {
                throw new ArgumentNullException(nameof(functionType));
            }
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            var key = $"{action.Namespace}.{action.MethodName}";
            if (!Methods.Any())
            {
                // setup action cache
                foreach (var type in functionType.Assembly.GetTypes().Where(x => !x.IsGenericType && !x.IsNested))
                {
                    // action provider attribute
                    foreach (var providerAttribute in type.GetCustomAttributes<ActionProviderAttribute>())
                    {
                        foreach (var typeMethod in type.GetMethods(BindingFlags.Public | BindingFlags.Instance))
                        {
                            // action attribute
                            var actionAttribute = typeMethod.GetCustomAttribute<ActionAttribute>();
                            if (actionAttribute == null)
                            {
                                continue;
                            }
                            var methodKey = $"{providerAttribute.Namespace}.{typeMethod.Name}";
                            Methods.Add(methodKey, new(providerAttribute, actionAttribute, typeMethod));
                        }
                    }
                }
            }

            // missing method
            if (!Methods.ContainsKey(key))
            {
                return null;
            }

            var attributeType = typeof(TAttribute);
            var method = Methods[key];

            // test function type
            if (method.Item1.Type.IsAssignableFrom(functionType) &&
                // action namespace
                string.Equals(method.Item1.Namespace, action.Namespace) &&
                // action method type
                attributeType.IsInstanceOfType(method.Item2) &&
                // method name
                string.Equals(method.Item3.Name, action.MethodName))
            {
                return method.Item3;
            }

            return null;
        }
    }

    /// <summary>Test for disabled action</summary>
    /// <param name="expression">The action expression</param>
    /// <returns>List of extension methods</returns>
    protected bool IsDisabledAction(string expression) =>
        !string.IsNullOrWhiteSpace(expression) && expression.StartsWith("'");

    /// <summary>Get type extension methods from the current assembly</summary>
    /// <param name="context">The action context</param>
    /// <param name="expression">The action expression</param>
    /// <returns>List of extension methods</returns>
    protected bool InvokeAction<TContext, TAction>(TContext context, string expression)
        where TContext : IActionContext
        where TAction : ActionAttribute
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }
        if (string.IsNullOrWhiteSpace(expression))
        {
            throw new ArgumentException(nameof(expression));
        }

        // ignore disabled actions
        if (IsDisabledAction(expression))
        {
            return false;
        }

        var functionType = typeof(TContext);
        var action = ParseAction(expression);
        var method = GetActionMethod<TAction>(action);
        if (method == null)
        {
            LogError($"Unknown function action {action} of type {typeof(TAction)}");
            return false;
        }

        // parameters
        var parameters = method.GetParameters();
        if (parameters.Length < 1)
        {
            // missing context parameter
            LogError($"Invalid function action {action} parameter count {parameters.Length}");
            return false;
        }

        // context parameter
        if (functionType != parameters[0].ParameterType &&
            !functionType.IsSubclassOf(parameters[0].ParameterType))
        {
            LogError($"Invalid action function argument {parameters[0].ParameterType}, expected {functionType}");
            return false;
        }

        // parameter values
        // parameter 1: action context
        var parameterValues = new List<object> { context };

        // further parameters: dynamic user parameters
        var parameterOffset = parameterValues.Count;
        if (action.MethodParameters != null && parameters.Length > parameterOffset)
        {
            var tokens = ParseMethodParameters(action.MethodParameters);
            //LogWarning($"InvokeAction: action.MethodParameters={action.MethodParameters}, tokens.Count={tokens.Count}");
            if (tokens.Count > parameters.Length - parameterValues.Count)
            {
                LogError($"Too many action parameter for method {method.Name} ({tokens.Count}): {action.MethodParameters}");
                return false;
            }

            for (var index = parameterOffset; index < parameters.Length; index++)
            {
                var parameter = parameters[index];
                var tokenIndex = index - parameterOffset;
                if (tokenIndex >= tokens.Count)
                {
                    // optional parameters
                    if (!parameter.IsOptional)
                    {
                        LogError($"Missing action parameter {parameter.Name} for method {method.Name}");
                        return false;
                    }
                    parameterValues.Add(parameter.DefaultValue);
                    break;
                }

                // parameter value
                try
                {
                    var parameterValue = GetActionParameterValue(tokens[tokenIndex], parameter.ParameterType);
                    if (parameterValue != null)
                    {
                        parameterValues.Add(parameterValue);
                    }
                }
                catch (Exception exception)
                {
                    throw new ScriptException($"Action argument {parameter.Name} error: {exception.GetBaseException().Message}", exception);
                }
            }
        }

        if (parameterValues.Count != parameters.Length)
        {
            LogError($"Mismatching action parameter for method {method.Name}: expected={parameters.Length}, actual={parameterValues.Count}");
            return false;
        }

        // extended method invocation
        try
        {
            // create instance of declaring type: requires a default constructor
            if (method.DeclaringType == null)
            {
                throw new ScriptException($"Missing declaring type for method {method.Name}");
            }
            var actionsInstance = Activator.CreateInstance(method.DeclaringType);

            //LogWarning($"Invoking action {action} on method {method.Name}");
            method.Invoke(actionsInstance, parameterValues.ToArray());
            if (actionsInstance is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
        catch (Exception exception)
        {
            throw new ScriptException($"Action {method.Name} failed: {exception.GetBaseException().Message}", exception);
        }
        return true;
    }

    private static List<string> ParseMethodParameters(string parameters)
    {
        var result = new List<string>();

        var subParameterCount = 0;
        var curIndex = 0;
        var startIndex = 0;
        foreach (var parameter in parameters)
        {
            switch (parameter)
            {
                case '(':
                    subParameterCount++;
                    break;
                case ')':
                    subParameterCount--;
                    break;
                case ',':
                    // ignore parameter separator from sub methods
                    if (subParameterCount == 0)
                    {
                        var curParameter = parameters.Substring(startIndex, curIndex - startIndex).Trim();
                        result.Add(curParameter);
                        startIndex = curIndex + 1;
                    }
                    break;
            }

            curIndex++;
        }

        if (startIndex == 0 && curIndex > 0)
        {
            // simple parameter
            var curParameter = parameters.Substring(0, curIndex).Trim();
            result.Add(curParameter);
        }
        else if (startIndex > 0 && startIndex < curIndex - 1)
        {
            // remaining parameter
            var curParameter = parameters.Substring(startIndex + 1).Trim();
            result.Add(curParameter);
        }
        return result;
    }

    /// <summary>Convert action parameter type</summary>
    private static object GetActionParameterValue(string value, Type type)
    {
        var underlyingType = Nullable.GetUnderlyingType(type);
        // an underlying nullable type, so the type is nullable
        // apply logic for null or empty test
        if (underlyingType != null && string.IsNullOrWhiteSpace(value))
        {
            return null;
        }
        return Convert.ChangeType(value, underlyingType ?? type, CultureInfo.InvariantCulture);
    }

    /// <summary>Get action methods by namespace</summary>
    private MethodInfo GetActionMethod<TAttribute>(Action action)
        where TAttribute : ActionAttribute =>
        ActionCache.GetActionMethod<TAttribute>(GetType(), action);

    /// <summary>Parse action from reference</summary>
    private static Action ParseAction(string expression)
    {
        // name
        var name = expression;

        // parameter
        string parameters = null;
        var paramStartIndex = expression.IndexOf("(", StringComparison.InvariantCulture);
        if (paramStartIndex > 0)
        {
            var paramEndIndex = expression.LastIndexOf(")", StringComparison.InvariantCulture);
            if (paramEndIndex > paramStartIndex)
            {
                name = expression.Substring(0, paramStartIndex);
                parameters = expression.Substring(paramStartIndex + 1, paramEndIndex - paramStartIndex - 1);
            }
        }

        // namespace
        var @namespace = DefaultActionNamespace;
        var namespaceIndex = name.LastIndexOf(".", StringComparison.InvariantCulture);
        if (namespaceIndex > 0 && (paramStartIndex < 0 || namespaceIndex < paramStartIndex))
        {
            @namespace = name.Substring(0, namespaceIndex);
            name = name.Substring(namespaceIndex + 1);
        }

        return new() { Namespace = @namespace, MethodName = name, MethodParameters = parameters };
    }

    /// <summary>Invoke condition action</summary>
    /// <param name="context">The action context</param>
    /// <param name="action">The action</param>
    /// <returns>True if action was invoked</returns>
    protected bool InvokeConditionAction<TContext, TAction>(TContext context, string action)
        where TContext : IConditionActionContext
        where TAction : ActionAttribute
    {
        // ignore disabled actions
        if (IsDisabledAction(action))
        {
            return false;
        }

        // parse conditional action
        var node = ParseConditionNode(action);
        if (node == null)
        {
            return InvokeAction<TContext, TAction>(context, action);
        }

        // invoke conditional action
        return InvokeConditionAction<TContext, TAction>(context, node);
    }

    private bool InvokeConditionAction<TContext, TAction>(TContext context, ConditionNode node)
        where TContext : IConditionActionContext
        where TAction : ActionAttribute
    {
        // loop until the action invocation or null consequent/alternative
        while (node != null)
        {
            // inverted condition action
            var invert = false;
            var action = node.ConditionAction;
            if (node.IsCondition && node.ConditionAction.StartsWith('!'))
            {
                invert = true;
                action = action.RemoveFromStart("!");
            }

            // invoke condition action
            if (!InvokeAction<TContext, TAction>(context, action))
            {
                return false;
            }

            // action node
            if (node.IsAction)
            {
                //LogWarning($"Invoked action: {node.Action}, expression={node.Expression}");
                return true;
            }

            // condition: use issues counter as condition trigger
            var consequent = !context.Issues.Any();
            if (invert)
            {
                consequent = !consequent;
            }
            // clear temporary condition issues
            context.Issues.Clear();

            //LogWarning($"Consequent: {consequent},  node-Consequent{node.Consequent}, node-Alternative{node.Alternative}");

            // next node
            node = InvokeConditionNodes<TContext, TAction>(context,
                consequent ? node.ConsequentNodes : node.AlternativeNodes);
        }
        return false;
    }

    private ConditionNode InvokeConditionNodes<TContext, TAction>(TContext context, List<ConditionNode> nodes)
        where TContext : IConditionActionContext
        where TAction : ActionAttribute
    {
        if (nodes == null || !nodes.Any())
        {
            return null;
        }

        // single-action condition
        if (nodes.Count == 1)
        {
            return nodes[0];
        }

        // multi-action condition
        foreach (var conditionNode in nodes)
        {
            // ignore condition nodes on multi-action expression
            if (!conditionNode.IsAction)
            {
                continue;
            }

            // invoke condition multi-action
            if (!InvokeAction<TContext, TAction>(context, conditionNode.ConditionAction))
            {
                return null;
            }
        }
        // no sub node support
        return null;
    }

    private ConditionNode ParseConditionNode(string expression)
    {
        //LogWarning($"$$$ Parsing expression [{expression}] $$$");
        if (string.IsNullOrWhiteSpace(expression))
        {
            return null;
        }

        var startExpression = expression;

        var consequentIndex = expression.IndexOf($" {ConditionConsequentMarker} ", StringComparison.InvariantCultureIgnoreCase);
        var alternativeIndex = expression.IndexOf($" {ConditionAlternativeMarker} ", StringComparison.InvariantCultureIgnoreCase);
        // alternative marker at the end of line
        if (alternativeIndex < 0 && expression.EndsWith(ConditionAlternativeMarker))
        {
            alternativeIndex = expression.Length - 1;
        }
        // only one marker present
        if (consequentIndex < 0 && alternativeIndex >= 0 || alternativeIndex < 0 && consequentIndex >= 0)
        {
            //LogWarning($"!!! consequentIndex < 0 && alternativeIndex >= 0 || alternativeIndex < 0 && consequentIndex >= 0 [{expression}]");
            return null;
        }
        // no condition: action
        if (consequentIndex < 0 && alternativeIndex < 0)
        {
            return new ConditionNode(expression);
        }
        // invalid condition syntax: missing ':' or ':' before ?
        if (alternativeIndex < consequentIndex)
        {
            //LogWarning($"!!! alternativeIndex < consequentIndex [{expression}]");
            return null;
        }

        var conditionAction = expression.Substring(0, consequentIndex);

        // remove condition action from expression
        expression = expression.Substring(consequentIndex + 1).Trim().
            RemoveFromStart($"{ConditionConsequentMarker}").Trim();

        // find alternative location
        alternativeIndex = FindAlternateIndex(expression);
        if (alternativeIndex < 0)
        {
            LogError($"Missing condition alternate marker ':' in expression [{startExpression}]");
            return null;
        }

        // consequent
        var consequentNodes = new List<ConditionNode>();
        var consequentExpressions = expression.Substring(0, alternativeIndex);
        foreach (var consequentExpression in GetConditionExpressions(consequentExpressions))
        {
            var node = ParseConditionNode(consequentExpression);
            if (node != null)
            {
                consequentNodes.Add(node);
            }
        }

        // alternative
        var alternativeNodes = new List<ConditionNode>();
        var alternativeExpressions = expression.Substring(alternativeIndex + 1);
        foreach (var alternativeExpression in GetConditionExpressions(alternativeExpressions))
        {
            var node = ParseConditionNode(alternativeExpression);
            if (node != null)
            {
                alternativeNodes.Add(node);
            }
        }

        // one part is required
        if (!consequentNodes.Any()! && alternativeNodes.Any())
        {
            LogError($"Missing condition consequent or alternative: {startExpression}");
            return null;
        }

        //LogWarning($"!!! dynamic expression=[{conditionAction} ? {consequent?.Expression} : {alternative?.Expression}]");
        return new ConditionNode(startExpression, conditionAction, consequentNodes, alternativeNodes);
    }

    private static List<string> GetConditionExpressions(string expression)
    {
        // empty expression
        if (string.IsNullOrWhiteSpace(expression))
        {
            return [];
        }

        expression = expression.Trim();

        // single action expression
        if (!expression.Contains(ConditionActionSeparator))
        {
            return [expression];
        }

        // single or multi-action expression
        var nodes = new List<string>();
        var nodeIndex = 0;
        for (var index = 0; index < expression.Length; index++)
        {
            var c = expression[index];

            // action separator
            if (c == ConditionActionSeparator)
            {
                if (index > 0 && index < expression.Length - 1)
                {
                    var left = expression[index - 1];
                    var right = expression[index + 1];
                    // ReSharper disable once GrammarMistakeInComment
                    // previous character is letter (action name) or parameter closing character )
                    // next character must be a space
                    if ((char.IsLetter(left) || left == ')') && right == ' ')
                    {
                        var node = expression.Substring(nodeIndex, index).Trim();
                        nodes.Add(node);
                        nodeIndex = index + 1;
                    }
                }
            }
            index++;
        }

        // ending expression
        if (nodeIndex < expression.Length)
        {
            var node = expression.Substring(nodeIndex).Trim();
            nodes.Add(node);
        }

        return nodes;
    }

    private static int FindAlternateIndex(string expression)
    {
        if (string.IsNullOrWhiteSpace(expression) || expression.Length < 2)
        {
            return -1;
        }

        var consequentCount = 0;
        for (var index = 1; index < expression.Length; index++)
        {
            switch (expression[index])
            {
                case ConditionConsequentMarker:
                    // consequent mask: ' ? '
                    if (index < expression.Length - 1 &&
                        expression[index - 1] == ' ' && expression[index + 1] == ' ')
                    {
                        consequentCount++;
                    }
                    break;
                case ConditionAlternativeMarker:
                    // alternative mask: ' : ' or at the end of line ' :'
                    if (expression[index - 1] == ' ' &&
                        (index == expression.Length - 1 || expression[index + 1] == ' '))
                    {
                        if (consequentCount == 0)
                        {
                            return index;
                        }
                        consequentCount--;
                    }
                    break;
            }
        }
        return -1;
    }

    #endregion

}