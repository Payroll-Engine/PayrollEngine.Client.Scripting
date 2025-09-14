/* CaseBuildFunction */

// ReSharper disable RedundantUsingDirective
using System;
using System.Linq;
using System.Text.Json;
using System.Collections.Generic;
using PayrollEngine.Client.Scripting;
// ReSharper restore RedundantUsingDirective

namespace PayrollEngine.Client.Scripting.Function;

/// <summary>Build a case (default: true), optionally considering related source cases</summary>
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
public partial class CaseBuildFunction : CaseChangeFunction
{
    /// <summary>Initializes a new instance with the function runtime</summary>
    /// <param name="runtime">The runtime</param>
    public CaseBuildFunction(object runtime) :
        base(runtime)
    {
    }

    /// <summary>New function instance without runtime (scripting development)</summary>
    /// <remarks>Use <see cref="Function.GetSourceFileName"/> in your constructor for the source file name</remarks>
    /// <param name="sourceFileName">The name of the source file</param>
    protected CaseBuildFunction(string sourceFileName) :
        base(sourceFileName)
    {
    }

    /// <summary>Get case build actions</summary>
    public string[] GetBuildActions() =>
        Runtime.GetBuildActions();

    /// <summary>Get case field build actions</summary>
    public string[] GetFieldBuildActions(string caseFieldName) =>
        Runtime.GetFieldBuildActions(caseFieldName);

    #region Validation

    /// <summary>
    /// Set case build to valid
    /// </summary>
    public void BuildValid() => BuildValidity(true);

    /// <summary>
    /// Set case build to invalid
    /// </summary>
    public void BuildInvalid() => BuildValidity(false);

    /// <summary>
    /// Set case build validity
    /// </summary>
    /// <param name="valid">Build validity</param>
    public void BuildValidity(bool valid) =>
        SetCaseAttribute(InputAttributes.Validity, valid);

    #endregion

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

    /// <summary>Entry point for the runtime</summary>
    /// <remarks>Internal usage only, do not call this method</remarks>
    public bool? Build()
    {
        InvokeCaseFieldActions();
        InvokeCaseBuildActions();

        // ReSharper disable EmptyRegion
        #region Function
        #endregion
        // ReSharper restore EmptyRegion

        // compiler will optimize this out if the code provides a return
        return null;
    }

    private void InvokeCaseBuildActions()
    {
        var context = new CaseChangeActionContext(this);
        foreach (var action in GetBuildActions())
        {
            InvokeConditionAction<CaseChangeActionContext, CaseChangeActionAttribute>(context, action);
        }
    }

    private void InvokeCaseFieldActions()
    {
        foreach (var fieldName in FieldNames)
        {
            var context = new CaseChangeActionContext(this, fieldName);
            foreach (var action in GetFieldBuildActions(fieldName))
            {
                InvokeConditionAction<CaseChangeActionContext, CaseChangeActionAttribute>(context, action);
            }
        }
    }
}