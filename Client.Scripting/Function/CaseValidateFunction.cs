﻿/* CaseValidateFunction */

// ReSharper disable RedundantUsingDirective
using System;
using System.Linq;
using System.Text.Json;
using System.Linq.Expressions;
using System.Collections.Generic;
using PayrollEngine.Client.Scripting;
// ReSharper restore RedundantUsingDirective

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

    /// <summary>Get case validate actions</summary>
    public string[] GetValidateActions() =>
        Runtime.GetValidateActions();

    /// <summary>Get case field validate actions</summary>
    public string[] GetFieldValidateActions(string caseFieldName) =>
        Runtime.GetFieldValidateActions(caseFieldName);

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
        if (!values.ContainsKey(name))
        {
            return;
        }

        // remove value
        values.Remove(name);
        SetCaseAttribute(InputAttributes.EditInfo, values.Count > 0 ? JsonSerializer.Serialize(values) : null);
    }

    #endregion

    /// <summary>Test for issues</summary>
    public bool HasIssues() => Runtime.HasIssues();

    /// <summary>Add a new case issue</summary>
    public void AddIssue(string message) =>
        Runtime.AddIssue(message);

    /// <summary>Add a new case field issue</summary>
    public void AddIssue(string caseFieldName, string message) =>
        Runtime.AddIssue(caseFieldName, message);

    /// <summary>Entry point for the runtime</summary>
    /// <remarks>Internal usage only, do not call this method</remarks>
    public bool? Validate()
    {
        // case field validation
        if (!InvokeCaseFieldActions())
        {
            return false;
        }

        // case validation
        if (!InvokeCaseValidateActions())
        {
            return false;
        }

        // ReSharper disable EmptyRegion
        #region Function
        #endregion
        // ReSharper restore EmptyRegion

        // compiler will optimize this out if the code provides a return
        return null;
    }

    private bool InvokeCaseValidateActions()
    {
        var context = new CaseChangeActionContext(this);
        foreach (var action in GetValidateActions())
        {
            InvokeConditionAction<CaseChangeActionContext, CaseChangeActionAttribute>(context, action);
            if (!context.HasIssues)
            {
                continue;
            }
            context.Issues.ForEach(x => AddIssue(x.Message));
            return false;
        }
        return true;
    }

    private bool InvokeCaseFieldActions()
    {
        foreach (var fieldName in FieldNames)
        {
            var context = new CaseChangeActionContext(this, fieldName);
            foreach (var action in GetFieldValidateActions(fieldName))
            {
                InvokeConditionAction<CaseChangeActionContext, CaseChangeActionAttribute>(context, action);
                if (!context.HasIssues)
                {
                    continue;
                }
                context.Issues.ForEach(x => AddIssue(fieldName, x.Message));
                return false;
            }
        }
        return true;
    }
}
