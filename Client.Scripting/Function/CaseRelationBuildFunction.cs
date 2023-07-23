/* CaseRelationBuildFunction */
// ReSharper disable RedundantUsingDirective
using System;
using System.Collections.Generic;
using System.Linq;
// ReSharper restore RedundantUsingDirective

namespace PayrollEngine.Client.Scripting.Function;

/// <summary>Build related case, considering related source case values, see <see cref="CaseRelationFunction.SourceValue"/> and <see cref="CaseRelationFunction.HasSourceValue"/></summary>
/// <example>
/// <code language="c#">
/// // Example with case value
/// (int)Employee["Level"] >= 2
/// </code>
/// <code language="c#">
/// // Example with related case value
/// TargetValue["Wage"] = (decimal)SourceValue[\"Wage\"] * 0.125M; TargetStart[\"Wage\"] = SourceStart[\"Wage\"]?.AddMonths(3)
/// </code>
/// </example>
// ReSharper disable once PartialTypeWithSinglePart
public partial class CaseRelationBuildFunction : CaseRelationFunction
{
    /// <summary>Initializes a new instance with the function runtime</summary>
    /// <param name="runtime">The runtime</param>
    public CaseRelationBuildFunction(object runtime) :
        base(runtime)
    {
    }

    /// <summary>New function instance without runtime (scripting development)</summary>
    /// <remarks>Use <see cref="Function.GetSourceFileName"/> in your constructor for the source file name</remarks>
    /// <param name="sourceFileName">The name of the source file</param>
    protected CaseRelationBuildFunction(string sourceFileName) :
        base(sourceFileName)
    {
    }

    /// <summary>Get case relation build actions</summary>
    public string[] GetBuildActions() =>
        Runtime.GetBuildActions();

    /// <summary>Entry point for the runtime</summary>
    /// <remarks>Internal usage only, do not call this method</remarks>
    public bool? Build()
    {
        InvokeBuildActions();

        // ReSharper disable EmptyRegion
        #region Function

        #endregion
        // ReSharper restore EmptyRegion

        // compiler will optimize this out if the code provides a return
        return default;
    }

    private void InvokeBuildActions()
    {
        var context = new CaseRelationActionContext(this);
        foreach (var action in GetBuildActions())
        {
            InvokeConditionAction<CaseRelationActionContext, CaseRelationBuildActionAttribute>(context, action);
        }
    }
}