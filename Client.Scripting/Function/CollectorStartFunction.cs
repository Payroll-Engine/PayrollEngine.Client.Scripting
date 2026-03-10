/* CollectorStartFunction */

// ReSharper disable RedundantUsingDirective
using System;
using System.Linq;
using System.Collections.Generic;
using PayrollEngine.Client.Scripting;
// ReSharper restore RedundantUsingDirective
// ReSharper disable EmptyRegion

namespace PayrollEngine.Client.Scripting.Function;

/// <summary>
/// Executes when a collector is first activated in a payrun, before any wage type values are applied.
/// </summary>
/// <remarks>
/// This function runs once per collector per employee, at the moment the collector is
/// initialised. It is used to set an initial state or override the starting value set.
/// <para>Typical uses:</para>
/// <list type="bullet">
///   <item>Override the initial value set with <see cref="SetValues"/> — for example, to carry
///   a residual amount forward from a previous period.</item>
///   <item>Read an employee runtime value (<see cref="PayrunFunction.GetEmployeeRuntimeValue"/>)
///   to seed the collector from pre-computed data.</item>
///   <item>Log collector initialisation for audit purposes.</item>
/// </list>
/// <para><strong>Return value:</strong> Return <c>null</c> to proceed normally.
/// Return any numeric value to override the initial collector value directly.
/// Returning <c>false</c> (cast to object) suppresses further processing for this collector.</para>
/// <para><strong>Low-Code / No-Code:</strong> Simple initialization can be expressed through
/// <c>CollectorStartAction</c> attributes. The <see cref="Start"/> entry point invokes all
/// registered actions before executing any inline script body.</para>
/// </remarks>
/// <example>
/// <code language="c#">
/// // Carry forward a residual amount from the previous period
/// SetValues(new[] { GetCaseValue&lt;decimal&gt;("CollectorCarry") });
/// </code>
/// </example>
/// <seealso cref="CollectorApplyFunction"/>
/// <seealso cref="CollectorEndFunction"/>
// ReSharper disable once PartialTypeWithSinglePart
public partial class CollectorStartFunction : CollectorFunction
{
    /// <summary>Initializes a new instance with the function runtime</summary>
    /// <param name="runtime">The runtime</param>
    public CollectorStartFunction(object runtime) :
        base(runtime)
    {
    }

    /// <summary>New function instance without runtime (scripting development)</summary>
    /// <remarks>Use <see cref="Function.GetSourceFileName"/> in your constructor for the source file name</remarks>
    /// <param name="sourceFileName">The name of the source file</param>
    protected CollectorStartFunction(string sourceFileName) :
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
    public object Start()
    {
        #region ActionInvoke
        #endregion

        #region Function
        #endregion
        // compiler will optimize this out if the code provides a return
        return null;
    }
}