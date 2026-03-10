/* CaseBuildFunction */

// ReSharper disable RedundantUsingDirective
using System;
using System.Linq;
using System.Text.Json;
using System.Collections.Generic;
using PayrollEngine.Client.Scripting;
// ReSharper restore RedundantUsingDirective
// ReSharper disable EmptyRegion

namespace PayrollEngine.Client.Scripting.Function;

/// <summary>
/// Populates case fields before the input form is displayed to the user.
/// </summary>
/// <remarks>
/// This function executes every time the case form is opened or refreshed. It is used to
/// pre-fill default values, derive field content from existing case data, apply lookups,
/// compute dates, and control which fields are shown or hidden.
/// <para>Typical uses:</para>
/// <list type="bullet">
///   <item>Set <c>Start["Field"]</c> and <c>End["Field"]</c> to default the value period.</item>
///   <item>Pre-fill <c>Value["Field"]</c> from existing case values or lookups.</item>
///   <item>Show or hide fields with <see cref="CaseChangeFunction.ShowCaseField"/> / <see cref="CaseChangeFunction.HideCaseField"/>.</item>
///   <item>Mark the form as invalid with <see cref="BuildInvalid"/> to block submission.</item>
/// </list>
/// <para><strong>Return value:</strong> Return <c>null</c> to indicate a successful build.
/// Return <c>false</c> to mark the case as not buildable (equivalent to <see cref="BuildInvalid"/>).</para>
/// <para><strong>Low-Code / No-Code:</strong> Field population can also be expressed through
/// action expressions using <c>CaseBuildAction</c> attributes — no C# scripting required.
/// The <see cref="Build"/> entry point invokes all registered actions before executing
/// any inline script body.</para>
/// </remarks>
/// <example>
/// <code language="c#">
/// // Set the start date to today and derive a salary value
/// Start["Salary"] = PeriodStart;
/// Value["Salary"] = GetCaseValue&lt;decimal&gt;("BaseSalary") * 1.05m;
/// </code>
/// <code language="c#">
/// // Hide a field that is not applicable
/// if ((string)Employee["ContractType"] != "Management")
///     HideCaseField("ManagementBonus");
/// </code>
/// </example>
/// <seealso cref="CaseAvailableFunction"/>
/// <seealso cref="CaseValidateFunction"/>
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

    #region Validation

    /// <summary>Marks the case form as valid; submission is permitted</summary>
    public void BuildValid() => BuildValidity(true);

    /// <summary>Marks the case form as invalid; submission is blocked</summary>
    public void BuildInvalid() => BuildValidity(false);

    /// <summary>Sets the validity state of the case form</summary>
    /// <param name="valid"><c>true</c> to permit submission; <c>false</c> to block it</param>
    public void BuildValidity(bool valid) =>
        SetCaseAttribute(InputAttributes.Validity, valid);

    #endregion

    #region Info

    /// <summary>Adds or updates a named entry in the case form's edit-info attribute</summary>
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

    /// <summary>Removes a named entry from the case form's edit-info attribute</summary>
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

    #region Action
    #endregion

    /// <summary>Entry point for the runtime</summary>
    /// <remarks>Internal usage only, do not call this method</remarks>
    public bool? Build()
    {
        #region ActionInvoke
        #endregion

        #region Function
        #endregion

        // compiler will optimize this out if the code provides a return
        return null;
    }
}