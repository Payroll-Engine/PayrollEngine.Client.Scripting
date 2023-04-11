/* CaseChangeFunction */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PayrollEngine.Client.Scripting.Function;

#region Action

/// <summary>Case change action context</summary>
public class CaseChangeActionContext : PayrollActionContext<CaseChangeFunction>
{
    /// <summary>The case field name</summary>
    public string CaseFieldName { get; }

    /// <summary>Constructor</summary>
    /// <param name="function">The function</param>
    public CaseChangeActionContext(CaseChangeFunction function) :
        base(function)
    {
    }

    /// <summary>Constructor</summary>
    /// <param name="function">The function</param>
    /// <param name="caseFieldName">The case field name</param>
    public CaseChangeActionContext(CaseChangeFunction function, string caseFieldName) :
        base(function)
    {
        if (string.IsNullOrWhiteSpace(caseFieldName))
        {
            throw new ArgumentException(nameof(caseFieldName));
        }
        CaseFieldName = caseFieldName;
    }
}

/// <summary>Case change action case value</summary>
public class CaseChangeActionValueContext : PayrollActionValueContext<CaseChangeFunction>
{
    /// <summary>The case field name</summary>
    public string CaseFieldName { get; }

    /// <summary>Constructor</summary>
    /// <param name="function">The function</param>
    /// <param name="caseFieldName">The case field name</param>
    public CaseChangeActionValueContext(CaseChangeFunction function, string caseFieldName) :
        this(function)
    {
        if (string.IsNullOrWhiteSpace(caseFieldName))
        {
            throw new ArgumentException(nameof(caseFieldName));
        }
        CaseFieldName = caseFieldName;
    }

    /// <summary>Constructor</summary>
    /// <param name="function">The function</param>
    public CaseChangeActionValueContext(CaseChangeFunction function) :
        base(function)
    {
    }

    /// <inheritdoc />
    public override CaseValue GetCaseChangeValue(string caseFieldName)
    {
        if (!Function.GetFieldNames().Any(x => string.Equals(x, caseFieldName)))
        {
            throw new ScriptException($"Unknown case change field {caseFieldName}");
        }

        return new CaseValue(caseFieldName, Date.Now,
            Function.GetStart(caseFieldName),
            Function.GetEnd(caseFieldName),
            new PayrollValue(Function.GetValue(caseFieldName)));
    }
}

/// <summary>Action case change value</summary>
public class ActionCaseChangeValue<TValue> : ActionCaseValue<CaseChangeActionValueContext, CaseChangeFunction, TValue>
{
    /// <summary>Default constructor</summary>
    public ActionCaseChangeValue(CaseChangeActionValueContext context, object sourceValue, DateTime? valueDate = null) :
        base(context, sourceValue, valueDate)
    {
    }
}

/// <summary>Base class for case change actions</summary>
public abstract class CaseChangeActionsBase : CaseActionsBase
{
    /// <summary>Compare culture</summary>
    protected static StringComparison GetCompareCulture(bool ignoreCase) =>
        ignoreCase ? StringComparison.InvariantCultureIgnoreCase : StringComparison.InvariantCulture;

    /// <summary>New source action</summary>
    protected static ActionCaseChangeValue<TValue> NewCaseFieldActionValue<TValue>(CaseChangeActionContext context)
    {
        try
        {
            return new ActionCaseChangeValue<TValue>(new(context.Function, context.CaseFieldName),
                ActionCaseValue.ToCaseChangeReference(context.CaseFieldName));
        }
        catch (Exception exception)
        {
            context.Function.LogError($"Invalid case field name {context.CaseFieldName}: {exception.GetBaseException().Message}");
            return default;
        }
    }

    /// <summary>New action</summary>
    protected static ActionCaseChangeValue<TValue> NewActionValue<TValue>(CaseChangeActionContext context,
        object value, DateTime? valueDate = null)
    {
        try
        {
            return value == null ? null : new ActionCaseChangeValue<TValue>(new(context.Function), value, valueDate);
        }
        catch (Exception exception)
        {
            context.Function.LogError($"Invalid case action value {value}: {exception.GetBaseException().Message}");
            return default;
        }
    }

    /// <summary>Resolve action value</summary>
    protected static TValue ResolveActionValue<TValue>(CaseChangeActionContext context, object value) =>
        NewActionValue<TValue>(context, value).ResolvedValue;

    /// <summary>Add issue</summary>
    protected void AddIssue(CaseChangeActionContext context, string message) =>
        context.AddIssue(message);

    /// <summary>Add issue</summary>
    protected void AddIssue(CaseChangeActionContext context, string issueName, params object[] parameters)
    {
        // issue attribute
        var attribute = FindIssueAttribute(issueName);
        if (attribute == null)
        {
            throw new ScriptException($"Missing action issue attribute {issueName} on type {GetType()}");
        }
        if (attribute.ParameterCount != parameters.Length)
        {
            throw new ScriptException($"Mismatching action parameter count on issue attribute {issueName} (expected={attribute.ParameterCount}, actual={parameters.Length}");
        }

        // localized message from lookup
        var format = GetLocalIssueMessage(context.Function, attribute.Name, attribute.Message);
        //context.Function.LogWarning($"AddIssue: format={format}, parameters={string.Join(",", parameters)}");
        var message = string.Format(format, parameters);
        AddIssue(context, message);
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
    /// <param name="function">The function</param>
    /// <param name="key">The issue key</param>
    /// <param name="defaultMessage">The default message</param>
    /// <remarks>Lookup name=Namespace.Actions, lookup key=issue name
    /// Format example: Invalid Email (1)</remarks>
    private string GetLocalIssueMessage(CaseChangeFunction function, string key, string defaultMessage)
    {
        var lookupName = Namespace.EnsureEnd(".Actions");
        //function.LogWarning($"GetIssueMessage: lookupName={lookupName}, localizationKey={key}");
        var message = function.GetLookup<string>(lookupName, key, function.UserLanguage) ?? defaultMessage;

        // prepare parameter placeholders for string format
        for (var i = 0; i < 10; i++)
        {
            message = message.Replace($"({i})", $"{{{i}}}");
        }
        return message;
    }
}

#endregion

/// <summary>Base class for case change functions</summary>
// ReSharper disable once PartialTypeWithSinglePart
public abstract partial class CaseChangeFunction : CaseFunction
{
    /// <summary>Initializes a new instance with the function runtime</summary>
    /// <param name="runtime">The runtime</param>
    protected CaseChangeFunction(object runtime) :
        base(runtime)
    {
        Start = new(GetStart, SetStart);
        End = new(GetEnd, SetEnd);
        Value = new(GetValue, SetValue);
        PayrollValue = new(caseFieldName => new(GetValue(caseFieldName)),
            (caseFieldName, value) => SetValue(caseFieldName, value.Value));
    }

    /// <summary>New function instance without runtime (scripting development)</summary>
    /// <remarks>Use <see cref="Function.GetSourceFileName"/> in your constructor for the source file name</remarks>
    /// <param name="sourceFileName">The name of the source file</param>
    protected CaseChangeFunction(string sourceFileName) :
        base(sourceFileName)
    {
    }

    #region Case

    /// <summary>Get the cancellation date</summary>
    public DateTime? CancellationDate => Runtime.CancellationDate;

    /// <summary>Get the cancellation state</summary>
    public bool Cancellation => CancellationDate.HasValue;

    /// <summary>Test if a case is available</summary>
    /// <param name="caseName">The name of the case</param>
    /// <returns>True if the case is available</returns>
    public bool CaseAvailable(string caseName) =>
        Runtime.CaseAvailable(caseName);

    /// <summary>Set case attribute value</summary>
    /// <param name="caseName">The name of the case</param>
    /// <param name="attributeName">The name of the case attribute</param>
    /// <param name="value">The value of the case attribute</param>
    public void SetCaseAttribute(string caseName, string attributeName, object value) =>
        Runtime.SetCaseAttribute(caseName, attributeName, value);

    /// <summary>Remove case attribute</summary>
    /// <param name="caseName">The name of the case</param>
    /// <param name="attributeName">The name of the case attribute</param>
    /// <returns>True if the case attribute has been removed</returns>
    public bool RemoveCaseAttribute(string caseName, string attributeName) =>
        Runtime.RemoveCaseAttribute(caseName, attributeName);

    #endregion

    #region Case Fields

    /// <summary>Get or set start date of the case field</summary>
    public ScriptDictionary<string, DateTime?> Start { get; }

    /// <summary>Get or set end date of the case value by the case field name</summary>
    public ScriptDictionary<string, DateTime?> End { get; }

    /// <summary>Get or set case field value by the case field name</summary>
    public ScriptDictionary<string, object> Value { get; }

    /// <summary>Get or set case value by the case field name</summary>
    public ScriptDictionary<string, PayrollValue> PayrollValue { get; }

    /// <summary>Get field names</summary>
    public string[] GetFieldNames() => Runtime.GetFieldNames();

    /// <summary>Test if the case contains fields</summary>
    public bool HasFields() => Runtime.HasFields();

    /// <summary>Test if a case field is complete (no start, end and value)</summary>
    /// <param name="caseFieldName">Name of the case field</param>
    public bool IsFieldComplete(string caseFieldName) => Runtime.IsFieldComplete(caseFieldName);

    /// <summary>Test if a case field is empty</summary>
    /// <param name="caseFieldName">Name of the case field</param>
    public bool IsFieldEmpty(string caseFieldName) => Runtime.IsFieldEmpty(caseFieldName);

    /// <summary>Test if a case field is defined</summary>
    /// <param name="caseFieldName">Name of the case field</param>
    public bool HasField(string caseFieldName) => Runtime.HasField(caseFieldName);

    /// <summary>Test if case field is available</summary>
    /// <param name="caseFieldName">The name of the case field</param>
    /// <returns>True if the case field is available</returns>
    public bool FieldAvailable(string caseFieldName) =>
        Runtime.FieldAvailable(caseFieldName);

    /// <summary>Set if the case field is available</summary>
    /// <param name="caseFieldName">The name of the case field</param>
    /// <param name="available">The case field available value</param>
    public void FieldAvailable(string caseFieldName, bool available) =>
        Runtime.FieldAvailable(caseFieldName, available);

    /// <summary>Initialize the case field</summary>
    /// <param name="caseFieldName">Name of the case field</param>
    /// <param name="start">The case field start date</param>
    /// <param name="end">The case field end date</param>
    /// <param name="value">The case field value</param>
    public void InitField(string caseFieldName, DateTime? start = null, DateTime? end = null, object value = null)
    {
        InitStart(caseFieldName, start);
        InitEnd(caseFieldName, end);
        InitValue(caseFieldName, value);
    }

    /// <summary>Initialize the case field</summary>
    /// <param name="caseFieldName">Name of the case field</param>
    /// <param name="start">The case field start date</param>
    /// <param name="value">The case field value</param>
    public void InitField(string caseFieldName, DateTime? start = null, object value = null) =>
        InitField(caseFieldName, start, null, value);

    /// <summary>Test if a case field start is defined</summary>
    /// <param name="caseFieldName">Name of the case field</param>
    public bool HasStart(string caseFieldName) => Runtime.HasStart(caseFieldName);

    /// <summary>Get the start date of a case field</summary>
    /// <param name="caseFieldName">Name of the case field</param>
    public DateTime? GetStart(string caseFieldName) => Runtime.GetStart(caseFieldName);

    /// <summary>Set the start date of a case field</summary>
    /// <param name="caseFieldName">Name of the case field</param>
    /// <param name="start">The case field start date</param>
    public void SetStart(string caseFieldName, DateTime? start) =>
        Runtime.SetStart(caseFieldName, start);

    /// <summary>Initialize the start date of a case field</summary>
    /// <param name="caseFieldName">Name of the case field</param>
    /// <param name="start">The case field start date</param>
    public void InitStart(string caseFieldName, DateTime? start) =>
        Runtime.InitStart(caseFieldName, start);

    /// <summary>Test if a case field end is defined</summary>
    /// <param name="caseFieldName">Name of the case field</param>
    public bool HasEnd(string caseFieldName) => Runtime.HasEnd(caseFieldName);

    /// <summary>Get the end date of a case field</summary>
    /// <param name="caseFieldName">Name of the case field</param>
    public DateTime? GetEnd(string caseFieldName) => Runtime.GetEnd(caseFieldName);

    /// <summary>Set the end date of a case field</summary>
    /// <param name="caseFieldName">Name of the case field</param>
    /// <param name="end">The case field end date</param>
    public void SetEnd(string caseFieldName, DateTime? end) =>
        Runtime.SetEnd(caseFieldName, end);

    /// <summary>Initialize the end date of a case field</summary>
    /// <param name="caseFieldName">Name of the case field</param>
    /// <param name="end">The case field end date</param>
    public void InitEnd(string caseFieldName, DateTime? end) =>
        Runtime.InitEnd(caseFieldName, end);

    /// <summary>Get the value period of a case field</summary>
    /// <param name="caseFieldName">Name of the case field</param>
    public DatePeriod GetPeriod(string caseFieldName) =>
        new(GetStart(caseFieldName), GetEnd(caseFieldName));

    /// <summary>Get a case value type</summary>
    /// <param name="caseFieldName">The name of the case field</param>
    public ValueType GetValueType(string caseFieldName) =>
        (ValueType)Runtime.GetValueType(caseFieldName);

    /// <summary>Test if a case field value is defined</summary>
    /// <param name="caseFieldName">Name of the case field</param>
    public bool HasValue(string caseFieldName) => Runtime.HasValue(caseFieldName);

    /// <summary>Get the value of a case field</summary>
    /// <param name="caseFieldName">Name of the case field</param>
    public object GetValue(string caseFieldName) => Runtime.GetValue(caseFieldName);

    /// <summary>Get the value of a case field with a default value</summary>
    /// <param name="caseFieldName">Name of the case field</param>
    /// <param name="defaultValue">The case field default value</param>
    public object GetValue(string caseFieldName, object defaultValue) =>
        HasValue(caseFieldName) ? GetValue(caseFieldName) : defaultValue;

    /// <summary>Get the typed value of a case field</summary>
    /// <param name="caseFieldName">Name of the case field</param>
    public T GetValue<T>(string caseFieldName) => (T)GetValue(caseFieldName);

    /// <summary>Get the typed value of a case field with a default value</summary>
    /// <param name="caseFieldName">Name of the case field</param>
    /// <param name="defaultValue">The case field default value</param>
    public T GetValue<T>(string caseFieldName, T defaultValue) =>
        HasValue(caseFieldName) ? (T)GetValue(caseFieldName) : defaultValue;

    /// <summary>Set the value of a case field</summary>
    /// <param name="caseFieldName">Name of the case field</param>
    /// <param name="value">The case field value</param>
    public void SetValue(string caseFieldName, object value) => Runtime.SetValue(caseFieldName, value);

    /// <summary>Set the value of a case field</summary>
    /// <param name="caseFieldName">Name of the case field</param>
    /// <param name="value">The case field value</param>
    public void SetValue(string caseFieldName, PayrollValue value) => SetValue(caseFieldName, value.Value);

    /// <summary>Initialize the value of a case field</summary>
    /// <param name="caseFieldName">Name of the case field</param>
    /// <param name="value">The case field value</param>
    public void InitValue(string caseFieldName, object value) =>
        Runtime.InitValue(caseFieldName, value);

    /// <summary>Initialize the value of a case field</summary>
    /// <param name="caseFieldName">Name of the case field</param>
    /// <param name="value">The case field payroll value</param>
    public void InitValue(string caseFieldName, PayrollValue value) => InitValue(caseFieldName, value.Value);

    /// <summary>Ass case value tag</summary>
    /// <param name="caseFieldName">The name of the case field</param>
    /// <param name="tag">The tag to add</param>
    /// <returns>True if the case value tag has ben added</returns>
    public bool AddCaseValueTag(string caseFieldName, string tag) => Runtime.AddCaseValueTag(caseFieldName, tag);

    /// <summary>Ass case value tag</summary>
    /// <param name="caseFieldName">The name of the case field</param>
    /// <param name="tag">The tag to remove</param>
    /// <returns>True if the case value tag has ben removed</returns>
    public bool RemoveCaseValueTag(string caseFieldName, string tag) => Runtime.RemoveCaseValueTag(caseFieldName, tag);

    /// <summary>Set case field attribute value</summary>
    /// <param name="caseFieldName">The name of the case field</param>
    /// <param name="attributeName">The name of the case field attribute</param>
    /// <param name="value">The value of the case field attribute (null = remove attribute)</param>
    public void SetCaseFieldAttribute(string caseFieldName, string attributeName, object value) =>
        Runtime.SetCaseFieldAttribute(caseFieldName, attributeName, value);

    /// <summary>Remove case field attribute</summary>
    /// <param name="caseFieldName">The name of the case field</param>
    /// <param name="attributeName">The name of the case field attribute</param>
    /// <returns>True if the case field attribute has been removed</returns>
    public bool RemoveCaseFieldAttribute(string caseFieldName, string attributeName) =>
        Runtime.RemoveCaseFieldAttribute(caseFieldName, attributeName);

    /// <summary>Set case value attribute value</summary>
    /// <param name="caseFieldName">The name of the case field</param>
    /// <param name="attributeName">The name of the case field attribute</param>
    /// <param name="value">The value of the case value attribute (null = remove attribute)</param>
    public void SetCaseValueAttribute(string caseFieldName, string attributeName, object value) =>
        Runtime.SetCaseValueAttribute(caseFieldName, attributeName, value);

    /// <summary>Remove case value attribute</summary>
    /// <param name="caseFieldName">The name of the case field</param>
    /// <param name="attributeName">The name of the case field attribute</param>
    /// <returns>True if the case value attribute has been removed</returns>
    public bool RemoveCaseValueAttribute(string caseFieldName, string attributeName) =>
        Runtime.RemoveCaseValueAttribute(caseFieldName, attributeName);

    /// <summary>Get the case slot values, grouped by case value</summary>
    /// <param name="caseFieldName">The name of the case field</param>
    /// <returns>The case values in a dictionary grouped by case slot value</returns>
    public Dictionary<CasePayrollValue, string> GetValueSlots(string caseFieldName)
    {
        var slotValues = new Dictionary<CasePayrollValue, string>();
        var caseValueSlots = GetCaseValueSlots(caseFieldName);
        if (caseValueSlots != null && caseValueSlots.Any())
        {
            foreach (var caseValueSlot in caseValueSlots)
            {
                var caseValue = GetCaseValue(CaseFieldSlot(caseFieldName, caseValueSlot));
                if (caseValue.HasValue)
                {
                    slotValues.Add(caseValue, caseValueSlot);
                }
            }
        }
        return slotValues;
    }

    /// <summary>Get the typed case slot values, grouped by case value</summary>
    /// <param name="caseFieldName">The name of the case field</param>
    /// <returns>The case values in a dictionary grouped by case slot value</returns>
    public Dictionary<T, string> GetValueSlots<T>(string caseFieldName)
    {
        var slotValues = new Dictionary<T, string>();
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
                        slotValues.Add(value, caseValueSlot);
                    }
                }
            }
        }
        return slotValues;
    }

    #endregion

}