/* CaseRelationFunction */

using System;
using System.Linq;

namespace PayrollEngine.Client.Scripting.Function;

#region Action

/// <summary>Case relation action context </summary>
public class CaseRelationActionContext : PayrollActionContext<CaseRelationFunction>
{
    /// <summary>Constructor</summary>
    /// <param name="function">The function</param>
    public CaseRelationActionContext(CaseRelationFunction function) :
        base(function)
    {
    }
}

/// <summary>Case change action case value</summary>
public class CaseRelationActionValueContext : PayrollActionValueContext<CaseRelationFunction>
{
    /// <summary>Constructor</summary>
    /// <param name="function">The function</param>
    public CaseRelationActionValueContext(CaseRelationFunction function) :
        base(function)
    {
    }

    /// <inheritdoc />
    public override CaseValue GetCaseChangeValue(string caseFieldName)
    {
        // source case field
        if (Function.GetSourceFieldNames().Any(x => string.Equals(x, caseFieldName)))
        {
            return new CaseValue(caseFieldName, Date.Now,
                Function.GetSourceStart(caseFieldName),
                Function.GetSourceEnd(caseFieldName),
                new PayrollValue(Function.GetSourceValue(caseFieldName)));
        }

        // target case field
        if (Function.GetTargetFieldNames().Any(x => string.Equals(x, caseFieldName)))
        {
            return new CaseValue(caseFieldName, Date.Now,
                Function.GetTargetStart(caseFieldName),
                Function.GetTargetEnd(caseFieldName),
                new PayrollValue(Function.GetTargetValue(caseFieldName)));
        }

        throw new ScriptException($"Unknown case change field {caseFieldName}");
    }
}

/// <summary>Case relation action case change value</summary>
public class CaseRelationActionCaseValue<TValue> : ActionCaseValue<CaseRelationActionValueContext, CaseRelationFunction, TValue>
{
    /// <summary>Default constructor</summary>
    public CaseRelationActionCaseValue(CaseRelationActionValueContext context, object sourceValue, DateTime? valueDate = null) :
        base(context, sourceValue, valueDate)
    {
    }
}

/// <summary>Base class for case change actions</summary>
public abstract class CaseRelationActionsBase : CaseActionsBase
{
    /// <summary>Compare culture</summary>
    protected static StringComparison GetCompareCulture(bool ignoreCase) =>
        ignoreCase ? StringComparison.InvariantCultureIgnoreCase : StringComparison.InvariantCulture;

    
    /// <summary>New source action</summary>
    protected static CaseRelationActionCaseValue<TValue> GetSourceActionValue<TValue>(CaseRelationActionContext context,
        string source)
    {
        if (string.IsNullOrWhiteSpace(source))
        {
            throw new ArgumentException("Invalid case relation source", nameof(source));
        }

        try
        {
            return new CaseRelationActionCaseValue<TValue>(new(context.Function), ActionCaseValueBase.ToCaseValueReference(source));
        }
        catch (Exception exception)
        {
            context.Function.LogError($"Invalid case field name {source}: {exception.GetBaseException().Message}");
            return default;
        }
    }

    /// <summary>New action</summary>
    protected static CaseRelationActionCaseValue<TValue> GetActionValue<TValue>(CaseRelationActionContext context,
        object value, DateTime? valueDate = null)
    {
        try
        {
            return new CaseRelationActionCaseValue<TValue>(new(context.Function), value, valueDate);
        }
        catch (Exception exception)
        {
            context.Function.LogError($"Invalid case action value {value}: {exception.GetBaseException().Message}");
            return default;
        }
    }

    /// <summary>Resolve action value</summary>
    protected static TValue ResolveActionValue<TValue>(CaseRelationActionContext context, object value) =>
        GetActionValue<TValue>(context, value).ResolvedValue;
}

#endregion

/// <summary>Base class for case relation functions</summary>
// ReSharper disable once PartialTypeWithSinglePart
public abstract partial class CaseRelationFunction : PayrollFunction
{
    /// <summary>Initializes a new instance with the function runtime</summary>
    /// <param name="runtime">The runtime</param>
    protected CaseRelationFunction(object runtime) :
        base(runtime)
    {
        // source (read-only)
        SourceStart = new(GetSourceStart);
        SourceEnd = new(GetSourceEnd);
        SourceValue = new(GetSourceValue);
        SourcePayrollValue = new(GetSourcePayrollValue);

        // target (read-write)
        TargetStart = new(GetTargetStart, SetTargetStart);
        TargetEnd = new(GetTargetEnd, SetTargetEnd);
        TargetValue = new(GetTargetValue, SetTargetValue);
        TargetPayrollValue = new(GetTargetPayrollValue, SetTargetPayrollValue);
    }

    /// <summary>New function instance without runtime (scripting development)</summary>
    /// <remarks>Use <see cref="Function.GetSourceFileName"/> in your constructor for the source file name</remarks>
    /// <param name="sourceFileName">The name of the source file</param>
    protected CaseRelationFunction(string sourceFileName) :
        base(sourceFileName)
    {
    }

    #region Source Case

    /// <summary>Gets the name of the source case</summary>
    public string SourceCaseName => Runtime.SourceCaseName;

    /// <summary>Gets the source case slot</summary>
    public string SourceCaseSlot => Runtime.SourceCaseSlot;

    /// <summary>Get the source case cancellation date</summary>
    public DateTime? SourceCaseCancellationDate => Runtime.SourceCaseCancellationDate;

    /// <summary>Get the source cancellation state</summary>
    public bool SourceCaseCancellation => SourceCaseCancellationDate.HasValue;

    /// <summary>Get start date of the source case value by the case field name</summary>
    public ScriptDictionary<string, DateTime?> SourceStart { get; }

    /// <summary>Get the start date of a case field</summary>
    /// <param name="caseFieldName">The case field name</param>
    public DateTime? GetSourceStart(string caseFieldName) => Runtime.GetSourceStart(caseFieldName);

    /// <summary>Get end date of the source case value by the case field name</summary>
    public ScriptDictionary<string, DateTime?> SourceEnd { get; }

    /// <summary>Get end date of the source case value by the case field name</summary>
    /// <param name="caseFieldName">The case field name</param>
    public DateTime? GetSourceEnd(string caseFieldName) => Runtime.GetSourceEnd(caseFieldName);

    /// <summary>Get the source case value type</summary>
    /// <param name="caseFieldName">The name of the case field</param>
    public ValueType GetSourceValueType(string caseFieldName) =>
        (ValueType)Runtime.GetSourceValueType(caseFieldName);

    /// <summary>Get source case value by the case field name</summary>
    public ScriptDictionary<string, object> SourceValue { get; }

    /// <summary>Get source case value by the case field name</summary>
    /// <param name="caseFieldName">The case field name</param>
    public object GetSourceValue(string caseFieldName) => Runtime.GetSourceValue(caseFieldName);

    /// <summary>Get source case value by the case field name with a default value</summary>
    /// <param name="caseFieldName">The case field name</param>
    /// <param name="defaultValue">The default value</param>
    public object GetSourceValue(string caseFieldName, object defaultValue) =>
        HasSourceValue(caseFieldName) ? GetSourceValue(caseFieldName) : defaultValue;

    /// <summary>Get source case field typed value by the case field name</summary>
    /// <param name="caseFieldName">The case field name</param>
    public T GetSourceValue<T>(string caseFieldName) => (T)GetSourceValue(caseFieldName);

    /// <summary>Get source case field typed value by the case field name with a default value</summary>
    /// <param name="caseFieldName">The case field name</param>
    /// <param name="defaultValue">The default value</param>
    public T GetSourceValue<T>(string caseFieldName, T defaultValue) =>
        HasSourceValue(caseFieldName) ? GetSourceValue<T>(caseFieldName) : defaultValue;

    /// <summary>Get source case field <see cref="PayrollValue"/> by the case field name</summary>
    public ScriptDictionary<string, PayrollValue> SourcePayrollValue { get; }

    /// <summary>Get source case field <see cref="PayrollValue"/> by the case field name</summary>
    /// <param name="caseFieldName">The case field name</param>
    public PayrollValue GetSourcePayrollValue(string caseFieldName) =>
        new(Runtime.GetSourceValue(caseFieldName));

    /// <summary>Get period of a source case field</summary>
    /// <param name="caseFieldName">The case field name</param>
    public DatePeriod GetSourcePeriod(string caseFieldName) =>
        new(SourceStart[caseFieldName], SourceEnd[caseFieldName]);

    /// <summary>Get source field names</summary>
    public string[] GetSourceFieldNames() => Runtime.GetSourceFieldNames();

    /// <summary>Test if the source case contains fields</summary>
    public bool HasSourceFields() => Runtime.HasSourceFields();

    /// <summary>Test if a source case field is defined</summary>
    /// <param name="caseFieldName">The case field name</param>
    public bool HasSourceField(string caseFieldName) => Runtime.HasSourceField(caseFieldName);

    /// <summary>Test if a source case field is complete</summary>
    /// <param name="caseFieldName">The case field name</param>
    public bool IsSourceFieldComplete(string caseFieldName) => Runtime.IsSourceFieldComplete(caseFieldName);

    /// <summary>Test if a source case field is empty (no start, end and value)</summary>
    /// <param name="caseFieldName">The case field name</param>
    public bool IsSourceFieldEmpty(string caseFieldName) => Runtime.IsSourceFieldEmpty(caseFieldName);

    /// <summary>Test if source case field start is defined</summary>
    /// <param name="caseFieldName">The case field name</param>
    public bool HasSourceStart(string caseFieldName) => Runtime.HasSourceStart(caseFieldName);

    /// <summary>Test if source case field end is defined</summary>
    /// <param name="caseFieldName">The case field name</param>
    public bool HasSourceEnd(string caseFieldName) => Runtime.HasSourceEnd(caseFieldName);

    /// <summary>Test if a source case value is defined</summary>
    /// <param name="caseFieldName">The case field name</param>
    public bool HasSourceValue(string caseFieldName) => Runtime.HasSourceValue(caseFieldName);

    /// <summary>Test a source case value</summary>
    /// <param name="caseFieldName">The case field name</param>
    /// <param name="testValue">The value to test</param>
    public bool SourceValueEquals<T>(string caseFieldName, T testValue) =>
        Equals(GetSourceValue(caseFieldName, default(T)), testValue);

    /// <summary>Test a string source case value containing a csv token</summary>
    /// <param name="caseFieldName">The case field name</param>
    /// <param name="token">The token to search</param>
    public bool SourceValueContainsCsvToken(string caseFieldName, string token)
    {
        var value = GetSourceValue<string>(caseFieldName);
        return value != null && value.ContainsCsvToken(token);
    }

    /// <summary>Test if an integer source case value is within a range</summary>
    /// <param name="caseFieldName">The case field name</param>
    /// <param name="min">The minimum value</param>
    /// <param name="max">The maximum value</param>
    public bool SourceValueIsWithin(string caseFieldName, int min, int max) =>
        GetSourceValue<int?>(caseFieldName, 0).IsWithin(min, max);

    /// <summary>Test if a decimal source case value is within a range</summary>
    /// <param name="caseFieldName">The case field name</param>
    /// <param name="min">The minimum value</param>
    /// <param name="max">The maximum value</param>
    public bool SourceValueIsWithin(string caseFieldName, decimal min, decimal max) =>
        GetSourceValue<decimal?>(caseFieldName, 0).IsWithin(min, max);

    /// <summary>Get source case attribute value</summary>
    /// <param name="attributeName">The attribute name</param>
    public object GetSourceCaseAttribute(string attributeName) =>
        Runtime.GetSourceCaseAttribute(attributeName);

    /// <summary>Get attribute value</summary>
    /// <param name="attributeName">The attribute name</param>
    /// <param name="defaultValue">The default value</param>
    public T GetSourceCaseAttribute<T>(string attributeName, T defaultValue = default)
    {
        var value = GetSourceCaseAttribute(attributeName);
        return value == null ? defaultValue : (T)Convert.ChangeType(value, typeof(T));
    }

    /// <summary>Get source case field attribute value</summary>
    /// <param name="caseFieldName">The case field name</param>
    /// <param name="attributeName">The attribute name</param>
    public object GetSourceCaseFieldAttribute(string caseFieldName, string attributeName) =>
        Runtime.GetSourceCaseFieldAttribute(caseFieldName, attributeName);

    /// <summary>Get source case field attribute typed value</summary>
    /// <param name="caseFieldName">The case field name</param>
    /// <param name="attributeName">The attribute name</param>
    /// <param name="defaultValue">The default value</param>
    public T GetSourceCaseFieldAttribute<T>(string caseFieldName, string attributeName, T defaultValue = default)
    {
        var value = GetSourceCaseFieldAttribute(caseFieldName, attributeName);
        return value == null ? defaultValue : (T)Convert.ChangeType(value, typeof(T));
    }

    /// <summary>Get source case value attribute value</summary>
    /// <param name="caseFieldName">The case field name</param>
    /// <param name="attributeName">The attribute name</param>
    public object GetSourceCaseValueAttribute(string caseFieldName, string attributeName) =>
        Runtime.GetSourceCaseValueAttribute(caseFieldName, attributeName);

    /// <summary>Get source case value attribute typed value</summary>
    /// <param name="caseFieldName">The case field name</param>
    /// <param name="attributeName">The attribute name</param>
    /// <param name="defaultValue">The default value</param>
    public T GetSourceCaseValueAttribute<T>(string caseFieldName, string attributeName, T defaultValue = default)
    {
        var value = GetSourceCaseValueAttribute(caseFieldName, attributeName);
        return value == null ? defaultValue : (T)Convert.ChangeType(value, typeof(T));
    }

    #endregion

    #region Target Case

    /// <summary>Gets the name of the target case</summary>
    public string TargetCaseName => Runtime.TargetCaseName;

    /// <summary>Gets the target case slot</summary>
    public string TargetCaseSlot => Runtime.TargetCaseSlot;

    /// <summary>Get the target case cancellation date</summary>
    public DateTime? TargetCaseCancellationDate => Runtime.TargetCaseCancellationDate;

    /// <summary>Get the target cancellation state</summary>
    public bool TargetCaseCancellation => TargetCaseCancellationDate.HasValue;

    /// <summary>Get or set the start date of the target case value by the case field name</summary>
    public ScriptDictionary<string, DateTime?> TargetStart { get; }

    /// <summary>Get the start date of the target case value by the case field name</summary>
    /// <param name="caseFieldName">The case field name</param>
    public DateTime? GetTargetStart(string caseFieldName) => Runtime.GetTargetStart(caseFieldName);

    /// <summary>Set the start date of the target case value by the case field name</summary>
    /// <param name="caseFieldName">The case field name</param>
    /// <param name="start">The start date</param>
    public void SetTargetStart(string caseFieldName, DateTime? start) => Runtime.SetTargetStart(caseFieldName, start);

    /// <summary>Get or set end date of the target case value by the case field name</summary>
    public ScriptDictionary<string, DateTime?> TargetEnd { get; }

    /// <summary>Get end date of the target case value by the case field name</summary>
    /// <param name="caseFieldName">The case field name</param>
    public DateTime? GetTargetEnd(string caseFieldName) => Runtime.GetTargetEnd(caseFieldName);

    /// <summary>Set the end date of the target case value by the case field name</summary>
    /// <param name="caseFieldName">The case field name</param>
    /// <param name="end">The end date</param>
    public void SetTargetEnd(string caseFieldName, DateTime? end) => Runtime.SetTargetEnd(caseFieldName, end);

    /// <summary>Get or set target case value by the case field name</summary>
    public ScriptDictionary<string, object> TargetValue { get; }

    /// <summary>Get target case value by the case field name</summary>
    /// <param name="caseFieldName">The case field name</param>
    public object GetTargetValue(string caseFieldName) => Runtime.GetTargetValue(caseFieldName);

    /// <summary>Get target case value by the case field name with a default value</summary>
    /// <param name="caseFieldName">The case field name</param>
    /// <param name="defaultValue">The default value</param>
    public object GetTargetValue(string caseFieldName, object defaultValue) =>
        HasTargetValue(caseFieldName) ? GetTargetValue(caseFieldName) : defaultValue;

    /// <summary>Get target case typed value by the case field name</summary>
    /// <param name="caseFieldName">The case field name</param>
    public T GetTargetValue<T>(string caseFieldName) => (T)GetTargetValue(caseFieldName);

    /// <summary>Get target case typed value by the case field name</summary>
    /// <param name="caseFieldName">The case field name</param>
    /// <param name="defaultValue">The default value</param>
    public T GetTargetValue<T>(string caseFieldName, T defaultValue) =>
        HasTargetValue(caseFieldName) ? GetTargetValue<T>(caseFieldName) : defaultValue;

    /// <summary>Set target case value by the case field name</summary>
    /// <param name="caseFieldName">The case field name</param>
    /// <param name="value">The target value</param>
    public void SetTargetValue(string caseFieldName, object value) => Runtime.SetTargetValue(caseFieldName, value);

    /// <summary>Get or set target case <see cref="PayrollValue"/> by the case field name</summary>
    public ScriptDictionary<string, PayrollValue> TargetPayrollValue { get; }

    /// <summary>Get target case value by the case field name</summary>
    /// <param name="caseFieldName">The case field name</param>
    public PayrollValue GetTargetPayrollValue(string caseFieldName) => new(GetTargetValue(caseFieldName));

    /// <summary>Set target case value by the case field name</summary>
    /// <param name="caseFieldName">The case field name</param>
    /// <param name="value">The target value</param>
    public void SetTargetPayrollValue(string caseFieldName, PayrollValue value) => SetTargetValue(caseFieldName, value.Value);

    /// <summary>Get period of a target case field</summary>
    /// <param name="caseFieldName">The case field name</param>
    public DatePeriod GetTargetPeriod(string caseFieldName) =>
        new(TargetStart[caseFieldName], TargetEnd[caseFieldName]);

    /// <summary>Get target field names</summary>
    public string[] GetTargetFieldNames() => Runtime.GetTargetFieldNames();

    /// <summary>Test if the target case contains fields</summary>
    public bool HasTargetFields() => Runtime.HasTargetFields();

    /// <summary>Test if a target case field is defined</summary>
    /// <param name="caseFieldName">The case field name</param>
    public bool HasTargetField(string caseFieldName) => Runtime.HasTargetField(caseFieldName);

    /// <summary>Test if a target case field is complete</summary>
    /// <param name="caseFieldName">The case field name</param>
    public bool IsTargetFieldComplete(string caseFieldName) => Runtime.IsTargetFieldComplete(caseFieldName);

    /// <summary>Test if a target case field is empty (no start, end and value)</summary>
    /// <param name="caseFieldName">The case field name</param>
    public bool IsTargetFieldEmpty(string caseFieldName) => Runtime.IsTargetFieldEmpty(caseFieldName);

    /// <summary>Test if a target case field is available</summary>
    /// <param name="caseFieldName">The case field name</param>
    public bool TargetFieldAvailable(string caseFieldName) =>
        Runtime.TargetFieldAvailable(caseFieldName);

    /// <summary>Set if the target case field is available</summary>
    /// <param name="caseFieldName">The case field name</param>
    /// <param name="available">Target field available state</param>
    public void TargetFieldAvailable(string caseFieldName, bool available) =>
        Runtime.TargetFieldAvailable(caseFieldName, available);

    /// <summary>Test if target case field start is defined</summary>
    /// <param name="caseFieldName">The case field name</param>
    public bool HasTargetStart(string caseFieldName) => Runtime.HasTargetStart(caseFieldName);

    /// <summary>Initialize the target start</summary>
    /// <param name="caseFieldName">The case field name</param>
    /// <param name="start">The start date</param>
    public void InitTargetStart(string caseFieldName, DateTime? start) => Runtime.InitTargetStart(caseFieldName, start);

    /// <summary>Test if target case field end is defined</summary>
    /// <param name="caseFieldName">The case field name</param>
    public bool HasTargetEnd(string caseFieldName) => Runtime.HasTargetEnd(caseFieldName);

    /// <summary>Initialize the target end</summary>
    /// <param name="caseFieldName">The case field name</param>
    /// <param name="end">The end date</param>
    public void InitTargetEnd(string caseFieldName, DateTime? end) => Runtime.InitTargetEnd(caseFieldName, end);

    /// <summary>Get the target case value type</summary>
    /// <param name="caseFieldName">The name of the case field</param>
    public ValueType GetTargetValueType(string caseFieldName) =>
        (ValueType)Runtime.GetTargetValueType(caseFieldName);

    /// <summary>Test if a target case value is defined</summary>
    /// <param name="caseFieldName">The case field name</param>
    public bool HasTargetValue(string caseFieldName) => Runtime.HasTargetValue(caseFieldName);

    /// <summary>Initialize the target value</summary>
    /// <param name="caseFieldName">The case field name</param>
    /// <param name="value">The target init value</param>
    public void InitTargetValue(string caseFieldName, object value) => Runtime.InitTargetValue(caseFieldName, value);

    /// <summary>Test a target case value</summary>
    /// <param name="caseFieldName">The case field name</param>
    /// <param name="testValue">The value to test</param>
    public bool TargetValueEquals<T>(string caseFieldName, T testValue) =>
        Equals(GetTargetValue(caseFieldName, default(T)), testValue);

    /// <summary>Test if a string target case value for a csv separated token</summary>
    /// <param name="caseFieldName">The case field name</param>
    /// <param name="token">The token to search</param>
    public bool TargetValueContainsCsvToken(string caseFieldName, string token)
    {
        var value = GetTargetValue<string>(caseFieldName);
        return value != null && value.ContainsCsvToken(token);
    }

    /// <summary>Test a string target case value containing a csv token, matching the target slot</summary>
    /// <param name="caseFieldName">The case field name</param>
    public bool TargetValueHasSourceSlotCsvToken(string caseFieldName) =>
        TargetValueContainsCsvToken(caseFieldName, SourceCaseSlot);

    /// <summary>Test if an integer target case value is within a range</summary>
    /// <param name="caseFieldName">The case field name</param>
    /// <param name="min">The minimum value</param>
    /// <param name="max">The maximum value</param>
    public bool TargetValueIsWithin(string caseFieldName, int min, int max) =>
        GetTargetValue<int?>(caseFieldName, 0).IsWithin(min, max);

    /// <summary>Test if a decimal target case value is within a range</summary>
    /// <param name="caseFieldName">The case field name</param>
    /// <param name="min">The minimum value</param>
    /// <param name="max">The maximum value</param>
    public bool TargetValueIsWithin(string caseFieldName, decimal min, decimal max) =>
        GetTargetValue<decimal?>(caseFieldName, 0).IsWithin(min, max);

    /// <summary>Get target case attribute value</summary>
    public object GetTargetCaseAttribute(string attributeName) => Runtime.GetTargetCaseAttribute(attributeName);

    /// <summary>Get attribute value</summary>
    public T GetTargetCaseAttribute<T>(string attributeName, T defaultValue = default)
    {
        var value = Runtime.GetTargetCaseAttribute(attributeName);
        return value == null ? defaultValue : (T)Convert.ChangeType(value, typeof(T));
    }

    /// <summary>Get target case field attribute value</summary>
    /// <param name="caseFieldName">The case field name</param>
    /// <param name="attributeName">The attribute name</param>
    public object GetTargetCaseFieldAttribute(string caseFieldName, string attributeName) =>
        Runtime.GetTargetCaseFieldAttribute(caseFieldName, attributeName);

    /// <summary>Get target case field attribute typed value</summary>
    /// <param name="caseFieldName">The case field name</param>
    /// <param name="attributeName">The attribute name</param>
    /// <param name="defaultValue">The default value</param>
    public T GetTargetCaseFieldAttribute<T>(string caseFieldName, string attributeName, T defaultValue = default)
    {
        var value = GetTargetCaseFieldAttribute(caseFieldName, attributeName);
        return value == null ? defaultValue : (T)Convert.ChangeType(value, typeof(T));
    }

    /// <summary>Get target case value attribute value</summary>
    /// <param name="caseFieldName">The case field name</param>
    /// <param name="attributeName">The attribute name</param>
    public object GetTargetCaseValueAttribute(string caseFieldName, string attributeName) =>
        Runtime.GetTargetCaseValueAttribute(caseFieldName, attributeName);

    /// <summary>Get target case value attribute typed value</summary>
    /// <param name="caseFieldName">The case field name</param>
    /// <param name="attributeName">The attribute name</param>
    /// <param name="defaultValue">The default value</param>
    public T GetTargetCaseValueAttribute<T>(string caseFieldName, string attributeName, T defaultValue = default)
    {
        var value = GetTargetCaseValueAttribute(caseFieldName, attributeName);
        return value == null ? defaultValue : (T)Convert.ChangeType(value, typeof(T));
    }

    #endregion

    #region Init and Copy

    /// <summary>Initializes the target case field start date from the source case field</summary>
    public void InitStart(string sourceFieldName, string targetFieldName) =>
        Runtime.InitStart(sourceFieldName, targetFieldName);

    /// <summary>Copy the case field start date from source to target/// </summary>
    public void CopyStart(string sourceFieldName, string targetFieldName) =>
        Runtime.CopyStart(sourceFieldName, targetFieldName);

    /// <summary>Initializes the target case field end date from the source case field</summary>
    public void InitEnd(string sourceFieldName, string targetFieldName) =>
        Runtime.InitEnd(sourceFieldName, targetFieldName);

    /// <summary>Copy the case field end date from source to target</summary>
    public void CopyEnd(string sourceFieldName, string targetFieldName) =>
        Runtime.CopyEnd(sourceFieldName, targetFieldName);

    /// <summary>Initializes the target case value from the source case field</summary>
    public void InitValue(string sourceFieldName, string targetFieldName) =>
        Runtime.InitValue(sourceFieldName, targetFieldName);

    /// <summary>Copy the case value from source to target</summary>
    public void CopyValue(string sourceFieldName, string targetFieldName) =>
        Runtime.CopyValue(sourceFieldName, targetFieldName);

    #endregion

}