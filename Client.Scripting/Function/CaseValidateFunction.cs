/* CaseValidateFunction */

// ReSharper disable RedundantUsingDirective
using System;
using System.Linq;
using System.Text.Json;
using System.Linq.Expressions;
using System.Collections.Generic;
using PayrollEngine.Client.Scripting;
// ReSharper restore RedundantUsingDirective
// ReSharper disable EmptyRegion

namespace PayrollEngine.Client.Scripting.Function;

/// <summary>
/// Validates case field values when the user submits the input form.
/// </summary>
/// <remarks>
/// This function runs after the user confirms the case form. It checks field values for
/// correctness and adds issues for any violations. Validation issues are displayed inline
/// next to the affected field or at the case level.
/// <para>Typical uses:</para>
/// <list type="bullet">
///   <item>Check that a field value is within an allowed range.</item>
///   <item>Ensure date constraints: start must be before end, dates within the payroll period.</item>
///   <item>Cross-field validation: two fields that must be consistent with each other.</item>
///   <item>Business rules: reject submissions that conflict with existing case values.</item>
/// </list>
/// <para><strong>Adding issues:</strong> Use <see cref="AddCaseIssue"/> for case-level messages
/// and <see cref="AddCaseFieldIssue"/> to attach an issue to a specific field.
/// Issues with localised messages can be added via <see cref="AddCaseAttributeIssue"/> and
/// <see cref="AddFieldAttributeIssue"/> by referencing a lookup-backed <c>ActionIssueAttribute</c>.</para>
/// <para><strong>Return value:</strong> Return <c>null</c> (or omit the return) to pass validation.
/// Returning <c>false</c> marks the case as invalid without adding an explicit message.
/// The runtime rejects the submission if any issues are present regardless of the return value.</para>
/// <para><strong>Low-Code / No-Code:</strong> Simple validations can be expressed through
/// action expressions using <c>CaseValidateAction</c> attributes — no C# scripting required.
/// The <see cref="Validate"/> entry point invokes all registered actions before executing
/// any inline script body.</para>
/// </remarks>
/// <example>
/// <code language="c#">
/// // Ensure a salary value is positive
/// if (GetValue&lt;decimal&gt;("Salary") &lt;= 0)
///     AddCaseFieldIssue("Salary", "Salary must be greater than zero.");
/// </code>
/// <code language="c#">
/// // Cross-field: end date must be after start date
/// if (GetEnd("Contract") &lt;= GetStart("Contract"))
///     AddCaseFieldIssue("Contract", "End date must be after start date.");
/// </code>
/// </example>
/// <seealso cref="CaseAvailableFunction"/>
/// <seealso cref="CaseBuildFunction"/>
// ReSharper disable once PartialTypeWithSinglePart
public partial class CaseValidateFunction : CaseChangeFunction
{
    /// <summary>Initializes a new instance with the function runtime</summary>
    /// <param name="runtime">The runtime</param>
    public CaseValidateFunction(object runtime) :
        base(runtime)
    {
    }
    /// <summary>New function instance without runtime (scripting development)</summary>
    /// <remarks>Use <see cref="Function.GetSourceFileName"/> in your constructor for the source file name</remarks>
    /// <param name="sourceFileName">The name of the source file</param>
    protected CaseValidateFunction(string sourceFileName) :
        base(sourceFileName)
    {
    }

    #region Info

    /// <summary>Adds or updates a named entry in the form's edit-info attribute</summary>
    /// <param name="name">The info entry name</param>
    /// <param name="value">The info entry value</param>
    public void AddInfo(string name, object value)
    {
        // info values
        var values = new Dictionary<string, object>();
        var attribute = GetCaseAttribute(InputAttributes.EditInfo) as string;
        if (!string.IsNullOrWhiteSpace(attribute))
        {
            values = JsonSerializer.Deserialize<Dictionary<string, object>>(attribute);
        }

        // set/replace value
        values[name] = value;
        SetCaseAttribute(InputAttributes.EditInfo, JsonSerializer.Serialize(values));
    }

    /// <summary>Removes a named entry from the form's edit-info attribute</summary>
    /// <param name="name">The info entry name to remove</param>
    public void RemoveInfo(string name)
    {
        // info values
        var attribute = GetCaseAttribute(InputAttributes.EditInfo) as string;
        if (string.IsNullOrWhiteSpace(attribute))
        {
            return;
        }
        var values = JsonSerializer.Deserialize<Dictionary<string, object>>(attribute);
        if (!values.Remove(name))
        {
            return;
        }

        // remove value
        SetCaseAttribute(InputAttributes.EditInfo, values.Count > 0 ? JsonSerializer.Serialize(values) : null);
    }

    #endregion

    #region Issue

    /// <summary>Test for issues</summary>
    public bool HasIssues() => Runtime.HasIssues();

    /// <summary>Add a new case validation issue</summary>
    /// <param name="message">The issue message</param>
    public void AddCaseIssue(string message) =>
        Runtime.AddCaseIssue(message);

    /// <summary>Add a new case field validation issue</summary>
    /// <param name="caseFieldName">Name of the case field</param>
    /// <param name="message">The issue message</param>
    public void AddCaseFieldIssue(string caseFieldName, string message) =>
        Runtime.AddCaseFieldIssue(caseFieldName, message);

    /// <summary>Add case issue from attribute</summary>
    /// <param name="attributeName">Attribute name</param>
    /// <param name="parameters">Message parameters</param>
    public void AddCaseAttributeIssue(string attributeName, params object[] parameters) =>
        AddCaseIssue(GetAttributeIssue(attributeName, parameters));

    /// <summary>Add case field issue from attribute</summary>
    /// <param name="caseFieldName">Case field name</param>
    /// <param name="attributeName">Attribute name</param>
    /// <param name="parameters">Message parameters</param>
    public void AddFieldAttributeIssue(string caseFieldName, string attributeName, params object[] parameters) =>
        AddCaseFieldIssue(caseFieldName, GetAttributeIssue(attributeName, parameters));

    /// <summary>Adds a case field issue using a localised message from an attribute; alias for <see cref="AddFieldAttributeIssue"/></summary>
    /// <param name="caseFieldName">Case field name</param>
    /// <param name="attributeName">Attribute name</param>
    /// <param name="parameters">Optional message format parameters</param>
    public void AddAttributeIssue(string caseFieldName, string attributeName, params object[] parameters) =>
        AddCaseFieldIssue(caseFieldName, GetAttributeIssue(attributeName, parameters));

    #endregion

    #region Action
    #endregion

    /// <summary>Entry point for the runtime</summary>
    /// <remarks>Internal usage only, do not call this method</remarks>
    public bool? Validate()
    {
        #region ActionInvoke
        #endregion

        #region Function
        #endregion

        // compiler will optimize this out if the code provides a return
        return null;
    }
}
