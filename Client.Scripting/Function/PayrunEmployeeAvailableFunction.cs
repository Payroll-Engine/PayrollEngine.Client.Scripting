/* PayrunEmployeeAvailableFunction */

// ReSharper disable RedundantUsingDirective
using System;
using System.Linq;
using System.Collections.Generic;
using PayrollEngine.Client.Scripting;
// ReSharper restore RedundantUsingDirective

namespace PayrollEngine.Client.Scripting.Function;

/// <summary>
/// Determines whether an employee participates in the current payrun (default: <c>true</c>).
/// </summary>
/// <remarks>
/// This function is evaluated once per employee, before any wage types or collectors are
/// processed for that employee. Returning <c>false</c> skips the employee entirely: no
/// <see cref="PayrunEmployeeStartFunction"/>, no wage types, no collectors, no
/// <see cref="PayrunEmployeeEndFunction"/> will execute for the skipped employee.
/// <para>Typical uses:</para>
/// <list type="bullet">
///   <item>Exclude employees based on a case field attribute (e.g. employment status, level).</item>
///   <item>Restrict processing to specific periods or cycle phases (e.g. annual bonus payrun).</item>
///   <item>Skip employees without a required case value in the current period.</item>
/// </list>
/// <para><strong>Return value:</strong> Return <c>true</c> or <c>null</c> to include the employee.
/// Return <c>false</c> to exclude the employee from this payrun.</para>
/// </remarks>
/// <example>
/// <code language="c#">
/// // Include only senior employees
/// (int)Employee["Level"] >= 2
/// </code>
/// <code language="c#">
/// // Include only in the last period of the cycle (e.g. annual bonus payrun)
/// LastCyclePeriod
/// </code>
/// <code language="c#">
/// // Exclude employees without an active contract case value
/// GetCaseValue&lt;string&gt;("ContractStatus") == "Active"
/// </code>
/// </example>
/// <seealso cref="PayrunWageTypeAvailableFunction"/>
/// <seealso cref="PayrunEmployeeStartFunction"/>
// ReSharper disable once PartialTypeWithSinglePart
public partial class PayrunEmployeeAvailableFunction : PayrunFunction
{
    /// <summary>Initializes a new instance with the function runtime</summary>
    /// <param name="runtime">The runtime</param>
    public PayrunEmployeeAvailableFunction(object runtime) :
        base(runtime)
    {
    }

    /// <summary>New function instance without runtime (scripting development)</summary>
    /// <remarks>Use <see cref="Function.GetSourceFileName"/> in your constructor for the source file name</remarks>
    /// <param name="sourceFileName">The name of the source file</param>
    protected PayrunEmployeeAvailableFunction(string sourceFileName) :
        base(sourceFileName)
    {
    }

    /// <summary>Entry point for the runtime</summary>
    /// <remarks>Internal usage only, do not call this method</remarks>
    public bool? IsAvailable()
    {
        // ReSharper disable EmptyRegion
        #region Function
        #endregion
        // ReSharper restore EmptyRegion
        // compiler will optimize this out if the code provides a return
        return null;
    }
}