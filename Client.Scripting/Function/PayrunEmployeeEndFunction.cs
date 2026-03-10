/* PayrunEmployeeEndFunction */

// ReSharper disable RedundantUsingDirective
using System;
using System.Linq;
using System.Collections.Generic;
using PayrollEngine.Client.Scripting;
// ReSharper restore RedundantUsingDirective

namespace PayrollEngine.Client.Scripting.Function;

/// <summary>
/// Executes at the end of each employee's payrun processing, after all wage types and collectors have completed.
/// </summary>
/// <remarks>
/// This function runs once per included employee, after all wage types and their collectors
/// have been evaluated. It is the last hook in the per-employee lifecycle and is suitable
/// for post-processing, aggregation, and result enrichment at the employee level.
/// <para>Typical uses:</para>
/// <list type="bullet">
///   <item>Compute final derived results from completed wage type and collector values.</item>
///   <item>Write cross-wage-type summaries as payrun results
///   (<see cref="PayrunFunction.SetPayrunResult(string, object, ValueType?, string, System.Collections.Generic.IEnumerable{string}, System.Collections.Generic.Dictionary{string, object}, string)"/>).</item>
///   <item>Store per-employee summary data as a runtime value for <see cref="PayrunEndFunction"/>.</item>
///   <item>Log completion or trigger per-employee tasks via <see cref="Function.AddTask"/>.</item>
/// </list>
/// <para><strong>Return value:</strong> This function returns <c>void</c>.
/// Wage type results are already committed; this hook is for side effects only.</para>
/// </remarks>
/// <example>
/// <code language="c#">
/// // Persist total net pay for use in PayrunEndFunction
/// var netPay = WageType[9000m]; // net pay wage type number
/// SetEmployeeRuntimeValue("NetPay", netPay.ToString());
/// </code>
/// </example>
/// <seealso cref="PayrunEmployeeStartFunction"/>
/// <seealso cref="PayrunEndFunction"/>
// ReSharper disable once PartialTypeWithSinglePart
public partial class PayrunEmployeeEndFunction : PayrunFunction
{
    /// <summary>Initializes a new instance with the function runtime</summary>
    /// <param name="runtime">The runtime</param>
    public PayrunEmployeeEndFunction(object runtime) :
        base(runtime)
    {
    }

    /// <summary>New function instance without runtime (scripting development)</summary>
    /// <remarks>Use <see cref="Function.GetSourceFileName"/> in your constructor for the source file name</remarks>
    /// <param name="sourceFileName">The name of the source file</param>
    protected PayrunEmployeeEndFunction(string sourceFileName) :
        base(sourceFileName)
    {
    }

    /// <summary>Entry point for the runtime</summary>
    /// <remarks>Internal usage only, do not call this method</remarks>
    public void End()
    {
        // ReSharper disable EmptyRegion
        #region Function
        #endregion
        // ReSharper restore EmptyRegion
    }
}