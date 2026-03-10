/* PayrunEmployeeStartFunction */

// ReSharper disable RedundantUsingDirective
using System;
using System.Linq;
using System.Collections.Generic;
using PayrollEngine.Client.Scripting;
// ReSharper restore RedundantUsingDirective

namespace PayrollEngine.Client.Scripting.Function;

/// <summary>
/// Executes at the start of each employee's payrun processing, before any wage types are evaluated.
/// </summary>
/// <remarks>
/// This function runs once per included employee, after <see cref="PayrunEmployeeAvailableFunction"/>
/// has returned <c>true</c> or <c>null</c>. It is used to initialize per-employee state that
/// wage type and collector functions will consume.
/// <para>Typical uses:</para>
/// <list type="bullet">
///   <item>Pre-compute derived employee data and store it via
///   <see cref="PayrunFunction.SetEmployeeRuntimeValue"/> for use in subsequent functions.</item>
///   <item>Log the start of employee processing for audit or debugging.</item>
///   <item>Conditionally abort the employee’s processing by returning <c>false</c>.</item>
/// </list>
/// <para><strong>Return value:</strong> Return <c>null</c> to continue normally.
/// Return <c>false</c> to abort this employee’s processing: no wage types or collectors
/// will execute, and <see cref="PayrunEmployeeEndFunction"/> will not run for this employee.</para>
/// </remarks>
/// <example>
/// <code language="c#">
/// // Cache a derived value for use in wage type functions
/// SetEmployeeRuntimeValue("FTE", GetCaseValue&lt;decimal&gt;("FTE").ToString());
/// </code>
/// </example>
/// <seealso cref="PayrunEmployeeAvailableFunction"/>
/// <seealso cref="PayrunEmployeeEndFunction"/>
// ReSharper disable once PartialTypeWithSinglePart
public partial class PayrunEmployeeStartFunction : PayrunFunction
{
    /// <summary>Initializes a new instance with the function runtime</summary>
    /// <param name="runtime">The runtime</param>
    public PayrunEmployeeStartFunction(object runtime) :
        base(runtime)
    {
    }

    /// <summary>New function instance without runtime (scripting development)</summary>
    /// <remarks>Use <see cref="Function.GetSourceFileName"/> in your constructor for the source file name</remarks>
    /// <param name="sourceFileName">The name of the source file</param>
    protected PayrunEmployeeStartFunction(string sourceFileName) :
        base(sourceFileName)
    {
    }

    /// <summary>Entry point for the runtime</summary>
    /// <remarks>Internal usage only, do not call this method</remarks>
    public bool? Start()
    {
        // ReSharper disable EmptyRegion
        #region Function
        #endregion
        // ReSharper restore EmptyRegion
        // compiler will optimize this out if the code provides a return
        return null;
    }
}