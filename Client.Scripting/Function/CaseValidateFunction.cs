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

/// <summary>Validate a case (default: true), optionally considering related source cases</summary>
/// <example>
/// <code language="c#">
/// // Example with case value
/// (int)Employee["Level"] >= 2
/// </code>
/// <code language="c#">
/// // Example with related case value
/// HasCaseValue("Wage")
/// </code>
/// <code language="c#">
/// // Example with optional related case value
/// HasCaseValue("Wage") ? (int)Employee["Level"] >= 2 : false
/// </code>
/// </example>
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

    /// <summary>
    /// Add build info
    /// </summary>
    /// <param name="name">Info name</param>
    /// <param name="value">Info value</param>
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

    /// <summary>
    /// Remove build info
    /// </summary>
    /// <param name="name">Info name</param>
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
    public void AddCaseIssue(string message) =>
        Runtime.AddCaseIssue(message);

    /// <summary>Add a new case field validation issue</summary>
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
