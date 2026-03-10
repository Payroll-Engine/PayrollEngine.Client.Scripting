/* CollectorEndFunction */

// ReSharper disable RedundantUsingDirective
using System;
using System.Linq;
using System.Collections.Generic;
using PayrollEngine.Client.Scripting;
// ReSharper restore RedundantUsingDirective
// ReSharper disable EmptyRegion

namespace PayrollEngine.Client.Scripting.Function;

/// <summary>
/// Executes when a collector is finalized in a payrun, after all wage type values have been applied.
/// </summary>
/// <remarks>
/// This function runs once per collector per employee, after all wage types have contributed
/// their values. It is the last opportunity to inspect or transform the final accumulated
/// result before it is committed to the payrun output.
/// <para>The complete collected state is available through the inherited properties:
/// <see cref="CollectorFunction.CollectorResult"/>, <see cref="CollectorFunction.CollectorSummary"/>,
/// <see cref="CollectorFunction.CollectorCount"/>, <see cref="CollectorFunction.CollectorMinimum"/>,
/// and <see cref="CollectorFunction.CollectorMaximum"/>.</para>
/// <para>Typical uses:</para>
/// <list type="bullet">
///   <item>Apply a final cap or floor to the collector result by overriding with <see cref="SetValues"/>.</item>
///   <item>Write a custom collector result via <see cref="CollectorFunction.AddCustomResult(string, decimal, System.Collections.Generic.IEnumerable{string}, System.Collections.Generic.Dictionary{string, object}, ValueType?, string)"/>.</item>
///   <item>Persist the final value as an employee runtime value for use in <see cref="PayrunEmployeeEndFunction"/>.</item>
///   <item>Schedule a retro payrun via <see cref="CollectorFunction.ScheduleRetroPayrun"/> if the
///   result crosses a threshold.</item>
/// </list>
/// <para><strong>Return value:</strong> Return <c>null</c> to commit the current accumulated result.
/// Return a numeric value to override the final result.
/// Return <c>null</c> after calling <see cref="CollectorFunction.Reset"/> to clear the result entirely.</para>
/// <para><strong>Low-Code / No-Code:</strong> End-of-collector logic can be expressed through
/// <c>CollectorEndAction</c> attributes. The <see cref="End"/> entry point invokes all
/// registered actions before executing any inline script body.</para>
/// </remarks>
/// <example>
/// <code language="c#">
/// // Cap the final collector result at 12 000
/// if (CollectorResult > 12000m)
///     SetValues(new[] { 12000m });
/// </code>
/// <code language="c#">
/// // Store the final value for use in PayrunEmployeeEndFunction
/// SetEmployeeRuntimeValue("SocialContrib", CollectorResult.ToString());
/// </code>
/// </example>
/// <seealso cref="CollectorStartFunction"/>
/// <seealso cref="CollectorApplyFunction"/>
// ReSharper disable once PartialTypeWithSinglePart
public partial class CollectorEndFunction : CollectorFunction
{
    /// <summary>Initializes a new instance with the function runtime</summary>
    /// <param name="runtime">The runtime</param>
    public CollectorEndFunction(object runtime) :
        base(runtime)
    {
    }

    /// <summary>New function instance without runtime (scripting development)</summary>
    /// <remarks>Use <see cref="Function.GetSourceFileName"/> in your constructor for the source file name</remarks>
    /// <param name="sourceFileName">The name of the source file</param>
    protected CollectorEndFunction(string sourceFileName) :
        base(sourceFileName)
    {
    }

    /// <summary>Get collector values</summary>
    public decimal[] GetValues() => Runtime.GetValues();

    /// <summary>Set collector values</summary>
    public void SetValues(decimal[] values) => Runtime.SetValues(values);

    #region Action
    #endregion

    /// <summary>Entry point for the runtime</summary>
    /// <remarks>Internal usage only, do not call this method</remarks>
    public object End()
    {
        #region ActionInvoke
        #endregion

        #region Function
        #endregion
        // compiler will optimize this out if the code provides a return
        return null;
    }
}