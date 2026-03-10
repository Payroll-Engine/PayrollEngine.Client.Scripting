/* CaseChangeFunction */

// ReSharper disable RedundantUsingDirective
using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using PayrollEngine.Client.Scripting;
// ReSharper restore RedundantUsingDirective

namespace PayrollEngine.Client.Scripting.Function;

/// <summary>
/// Base class for case change functions that operate on a case being entered or modified.
/// </summary>
/// <remarks>
/// Extends <see cref="CaseFunction"/> with full read/write access to all case field data:
/// start dates (<see cref="Start"/>), end dates (<see cref="End"/>), field values (<see cref="Value"/>),
/// payroll values (<see cref="PayrollValue"/>), field availability, initialization, and case attributes.
/// <para>Also provides access to case change metadata: reason (<see cref="GetReason"/>),
/// forecast (<see cref="GetForecast"/>), and cancellation date (<see cref="CancellationDate"/>).</para>
/// <para>Two concrete functions inherit from this class:</para>
/// <list type="bullet">
///   <item><see cref="CaseBuildFunction"/> — populates case fields before the input form is displayed.</item>
///   <item><see cref="CaseValidateFunction"/> — validates field values when the user submits the form.</item>
/// </list>
/// </remarks>
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
        FieldNames = new(Runtime.GetFieldNames());
    }

    /// <summary>New function instance without runtime (scripting development)</summary>
    /// <remarks>Use <see cref="Function.GetSourceFileName"/> in your constructor for the source file name</remarks>
    /// <param name="sourceFileName">The name of the source file</param>
    protected CaseChangeFunction(string sourceFileName) :
        base(sourceFileName)
    {
    }

    #region Case

    /// <summary>Gets the cancellation date of this case change, or <c>null</c> if not cancelled</summary>
    [ActionProperty("Case cancellation date")]
    public DateTime? CancellationDate => Runtime.CancellationDate;

    /// <summary>Tests whether this case change is a cancellation</summary>
    [ActionProperty("Test for canceled case")]
    public bool Cancellation => CancellationDate.HasValue;

    /// <summary>Tests whether the current case is available for input</summary>
    /// <returns><c>true</c> if the case is available</returns>
    public bool CaseAvailable() =>
        CaseAvailable(CaseName);

    /// <summary>Tests whether a named case is available for input</summary>
    /// <param name="caseName">The PascalCase case name</param>
    /// <returns><c>true</c> if the case is available</returns>
    public bool CaseAvailable(string caseName) =>
        Runtime.CaseAvailable(caseName);

    /// <summary>Set case attribute value</summary>
    /// <param name="attributeName">The name of the case attribute</param>
    /// <param name="value">The value of the case attribute</param>
    public void SetCaseAttribute(string attributeName, object value) =>
        SetCaseAttribute(CaseName, attributeName, value);

    /// <summary>Set case attribute value</summary>
    /// <param name="caseName">The name of the case</param>
    /// <param name="attributeName">The name of the case attribute</param>
    /// <param name="value">The value of the case attribute</param>
    public void SetCaseAttribute(string caseName, string attributeName, object value) =>
        Runtime.SetCaseAttribute(caseName, attributeName, value);

    /// <summary>Removes an attribute from the current case</summary>
    /// <param name="attributeName">The name of the case attribute</param>
    /// <returns><c>true</c> if the attribute existed and was removed</returns>
    public bool RemoveCaseAttribute(string attributeName) =>
        RemoveCaseAttribute(CaseName, attributeName);

    /// <summary>Removes an attribute from a named case</summary>
    /// <param name="caseName">The PascalCase case name</param>
    /// <param name="attributeName">The name of the case attribute</param>
    /// <returns><c>true</c> if the attribute existed and was removed</returns>
    public bool RemoveCaseAttribute(string caseName, string attributeName) =>
        Runtime.RemoveCaseAttribute(caseName, attributeName);

    /// <summary>Get the case change reason</summary>
    public string GetReason() =>
        Runtime.GetReason();

    /// <summary>Set the case change reason</summary>
    public void SetReason(string reason) =>
        Runtime.SetReason(reason);

    /// <summary>Get the case change forecast</summary>
    public string GetForecast() =>
        Runtime.GetForecast();

    /// <summary>Set the case change forecast</summary>
    public void SetForecast(string forecast) =>
        Runtime.SetForecast(forecast);

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

    /// <summary>Gets the names of all case fields in the current case change</summary>
    public List<string> FieldNames { get; }

    /// <summary>Tests whether the case change contains at least one field</summary>
    public bool HasFields() => Runtime.HasFields();

    /// <summary>Tests whether a case field has all three components set: start date, end date, and value</summary>
    /// <param name="caseFieldName">The case field name</param>
    /// <returns><c>true</c> if start, end, and value are all present</returns>
    public bool IsFieldComplete(string caseFieldName) => Runtime.IsFieldComplete(caseFieldName);

    /// <summary>Tests whether a case field has none of its components set (no start, no end, no value)</summary>
    /// <param name="caseFieldName">The case field name</param>
    /// <returns><c>true</c> if start, end, and value are all absent</returns>
    public bool IsFieldEmpty(string caseFieldName) => Runtime.IsFieldEmpty(caseFieldName);

    /// <summary>Tests whether a named case field exists in the current case change</summary>
    /// <param name="caseFieldName">The case field name</param>
    /// <returns><c>true</c> if the field is present</returns>
    public bool HasField(string caseFieldName) => Runtime.HasField(caseFieldName);

    /// <summary>Tests whether a case field is currently available (visible) in the form</summary>
    /// <param name="caseFieldName">The case field name</param>
    /// <returns><c>true</c> if the field is visible</returns>
    public bool FieldAvailable(string caseFieldName) =>
        Runtime.FieldAvailable(caseFieldName);

    /// <summary>Controls whether a case field is available (visible) in the form</summary>
    /// <param name="caseFieldName">The case field name</param>
    /// <param name="available"><c>true</c> to show the field; <c>false</c> to hide it</param>
    public void FieldAvailable(string caseFieldName, bool available) =>
        Runtime.FieldAvailable(caseFieldName, available);

    /// <summary>Sets value, start date, and end date of a case field only if not already set</summary>
    /// <param name="caseFieldName">Name of the case field</param>
    /// <param name="value">The case field value</param>
    /// <param name="start">The case field start date</param>
    /// <param name="end">The case field end date</param>
    /// <remarks>Each component is applied individually — a component already present is not overwritten.
    /// Use <see cref="SetField"/> to overwrite all components unconditionally.</remarks>
    public void InitField(string caseFieldName, object value, DateTime? start = null, DateTime? end = null)
    {
        InitValue(caseFieldName, value);
        if (start != null)
        {
            InitStart(caseFieldName, start);
        }
        if (end != null)
        {
            InitEnd(caseFieldName, end);
        }
    }

    /// <summary>Set the case field value, start and end</summary>
    /// <param name="caseFieldName">Name of the case field</param>
    /// <param name="value">The case field value</param>
    /// <param name="start">The case field start date</param>
    /// <param name="end">The case field end date</param>
    public void SetField(string caseFieldName, object value, DateTime? start = null, DateTime? end = null)
    {
        SetValue(caseFieldName, value);
        if (start != null)
        {
            SetStart(caseFieldName, start);
        }
        if (end != null)
        {
            SetEnd(caseFieldName, end);
        }
    }

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

    /// <summary>Sets the start date of a case field only if no start date is currently set</summary>
    /// <param name="caseFieldName">Name of the case field</param>
    /// <param name="start">The case field start date</param>
    /// <remarks>Use <see cref="SetStart"/> to overwrite unconditionally.</remarks>
    public void InitStart(string caseFieldName, DateTime? start) =>
        Runtime.InitStart(caseFieldName, start);

    /// <summary>Sets the start date on all case fields that have a value or a mandatory end date</summary>
    /// <param name="start">The start date to apply</param>
    public void UpdateStart(DateTime? start)
    {
        foreach (var fieldName in FieldNames)
        {
            if (MandatoryEnd(fieldName) || MandatoryValue(fieldName) || HasValue(fieldName))
            {
                SetStart(fieldName, start);
            }
        }
    }

    /// <summary>Tests whether an end date is mandatory for the given case field</summary>
    /// <param name="caseFieldName">The case field name</param>
    /// <returns><c>true</c> if an end date must be provided</returns>
    public bool MandatoryEnd(string caseFieldName) =>
        Runtime.MandatoryEnd(caseFieldName);

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

    /// <summary>Sets the end date of a case field only if no end date is currently set</summary>
    /// <param name="caseFieldName">Name of the case field</param>
    /// <param name="end">The case field end date</param>
    /// <remarks>Use <see cref="SetEnd"/> to overwrite unconditionally.</remarks>
    public void InitEnd(string caseFieldName, DateTime? end) =>
        Runtime.InitEnd(caseFieldName, end);

    /// <summary>Sets the end date on all case fields that have a value or a mandatory end date</summary>
    /// <param name="end">The end date to apply</param>
    public void UpdateEnd(DateTime? end)
    {
        foreach (var fieldName in FieldNames)
        {
            if (MandatoryEnd(fieldName) || MandatoryValue(fieldName) || HasValue(fieldName))
            {
                SetEnd(fieldName, end);
            }
        }
    }

    /// <summary>Tests whether a value is mandatory for the given case field</summary>
    /// <param name="caseFieldName">The case field name</param>
    /// <returns><c>true</c> if a value must be provided</returns>
    public bool MandatoryValue(string caseFieldName) =>
        Runtime.MandatoryValue(caseFieldName);

    /// <summary>Returns the value period of a case field as a <see cref="DatePeriod"/></summary>
    /// <param name="caseFieldName">The case field name</param>
    public DatePeriod GetPeriod(string caseFieldName) =>
        new(GetStart(caseFieldName), GetEnd(caseFieldName));

    /// <summary>Returns the <see cref="ValueType"/> of a case field's value</summary>
    /// <param name="caseFieldName">The case field name</param>
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
    public T GetValue<T>(string caseFieldName) =>
        GetValue(caseFieldName, default(T));

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

    /// <summary>Sets the value of a case field only if no value is currently set</summary>
    /// <param name="caseFieldName">Name of the case field</param>
    /// <param name="value">The case field value</param>
    /// <remarks>Use <see cref="SetValue(string, object)"/> to overwrite unconditionally.</remarks>
    public void InitValue(string caseFieldName, object value) =>
        Runtime.InitValue(caseFieldName, value);

    /// <summary>Sets the payroll value of a case field only if no value is currently set</summary>
    /// <param name="caseFieldName">Name of the case field</param>
    /// <param name="value">The case field payroll value</param>
    /// <remarks>Use <see cref="SetValue(string, object)"/> to overwrite unconditionally.</remarks>
    public void InitValue(string caseFieldName, PayrollValue value) => InitValue(caseFieldName, value.Value);

    /// <summary>Adds a tag to a case field value</summary>
    /// <param name="caseFieldName">The name of the case field</param>
    /// <param name="tag">The tag to add</param>
    /// <returns><c>true</c> if the tag was added; <c>false</c> if it was already present</returns>
    public bool AddCaseValueTag(string caseFieldName, string tag) => Runtime.AddCaseValueTag(caseFieldName, tag);

    /// <summary>Removes a tag from a case field value</summary>
    /// <param name="caseFieldName">The name of the case field</param>
    /// <param name="tag">The tag to remove</param>
    /// <returns><c>true</c> if the tag was removed; <c>false</c> if it was not present</returns>
    public bool RemoveCaseValueTag(string caseFieldName, string tag) => Runtime.RemoveCaseValueTag(caseFieldName, tag);

    /// <summary>Set case field attribute value</summary>
    /// <param name="caseFieldName">The name of the case field</param>
    /// <param name="attributeName">The name of the case field attribute</param>
    /// <param name="value">The value of the case field attribute (null = remove attribute)</param>
    public void SetCaseFieldAttribute(string caseFieldName, string attributeName, object value) =>
        Runtime.SetCaseFieldAttribute(caseFieldName, attributeName, value);

    /// <summary>Removes an attribute from a case field</summary>
    /// <param name="caseFieldName">The case field name</param>
    /// <param name="attributeName">The attribute name</param>
    /// <returns><c>true</c> if the attribute existed and was removed</returns>
    public bool RemoveCaseFieldAttribute(string caseFieldName, string attributeName) =>
        Runtime.RemoveCaseFieldAttribute(caseFieldName, attributeName);

    /// <summary>Set case value attribute value</summary>
    /// <param name="caseFieldName">The name of the case field</param>
    /// <param name="attributeName">The name of the case field attribute</param>
    /// <param name="value">The value of the case value attribute (null = remove attribute)</param>
    public void SetCaseValueAttribute(string caseFieldName, string attributeName, object value) =>
        Runtime.SetCaseValueAttribute(caseFieldName, attributeName, value);

    /// <summary>Removes an attribute from a case value</summary>
    /// <param name="caseFieldName">The case field name</param>
    /// <param name="attributeName">The attribute name</param>
    /// <returns><c>true</c> if the attribute existed and was removed</returns>
    public bool RemoveCaseValueAttribute(string caseFieldName, string attributeName) =>
        Runtime.RemoveCaseValueAttribute(caseFieldName, attributeName);

    /// <summary>Show the case field</summary>
    /// <param name="caseFieldName">The name of the case field</param>
    public void ShowCaseField(string caseFieldName) =>
        SetCaseFieldAttribute(caseFieldName, InputAttributes.Hidden, false);

    /// <summary>Hide the case field</summary>
    /// <param name="caseFieldName">The name of the case field</param>
    public void HideCaseField(string caseFieldName) =>
        SetCaseFieldAttribute(caseFieldName, InputAttributes.Hidden, true);

    /// <summary>Reads the current case change into a typed object</summary>
    /// <typeparam name="T">A class implementing <see cref="ICaseObject"/> with a parameterless constructor</typeparam>
    /// <returns>A new instance of <typeparamref name="T"/> populated from the current case field values</returns>
    public T GetChangeCaseObject<T>() where T : class, ICaseObject, new()
    {
        var caseObject = new T();
        foreach (var propertyInfo in CaseObject.GetProperties<T>())
        {
            // copy case value to property
            var value = GetValue(propertyInfo.CaseFieldName);
            caseObject.SetValue(propertyInfo.CaseFieldName, value);
        }
        return caseObject;
    }

    /// <summary>Writes the properties of a typed object back into the current case field values</summary>
    /// <typeparam name="T">A class implementing <see cref="ICaseObject"/> with a parameterless constructor</typeparam>
    /// <param name="data">The object whose property values are written to the case fields</param>
    public void SetChangeCaseObject<T>(T data) where T : class, ICaseObject, new()
    {
        ArgumentNullException.ThrowIfNull(data);
        foreach (var propertyInfo in CaseObject.GetProperties<T>())
        {
            // copy property value to case value
            var propertyValue = data.GetValue(propertyInfo.CaseFieldName);
            SetValue(propertyInfo.CaseFieldName, propertyValue);
        }
    }

    /// <summary>Returns all slot values for a case field, keyed by their payroll value</summary>
    /// <param name="caseFieldName">The name of the case field</param>
    /// <returns>Dictionary mapping each slot's <see cref="CasePayrollValue"/> to its slot name</returns>
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

    /// <summary>Returns all slot values for a case field as a typed dictionary</summary>
    /// <typeparam name="T">The value type to cast each slot value to</typeparam>
    /// <param name="caseFieldName">The name of the case field</param>
    /// <returns>Dictionary mapping the typed slot value to its slot name</returns>
    public Dictionary<T, string> GetValueSlots<T>(string caseFieldName)
    {
        var slotValues = new Dictionary<T, string>();
        var caseValueSlots = GetCaseValueSlots(caseFieldName);
        if (caseValueSlots != null && caseValueSlots.Any())
        {
            foreach (var caseValueSlot in caseValueSlots)
            {
                var caseValue = GetCaseValue(CaseFieldSlot(caseFieldName, caseValueSlot));
                var value = ChangeValueType<T>(caseValue.Value);
                if (value != null)
                {
                    slotValues.Add(value, caseValueSlot);
                }
            }
        }
        return slotValues;
    }

    #endregion

}