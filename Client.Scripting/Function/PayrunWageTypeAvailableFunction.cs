/* PayrunWageTypeAvailableFunction */

// ReSharper disable RedundantUsingDirective
using System;
using System.Linq;
using System.Collections.Generic;
using PayrollEngine.Client.Scripting;
// ReSharper restore RedundantUsingDirective

namespace PayrollEngine.Client.Scripting.Function;

/// <summary>
/// Determines whether a specific wage type is evaluated for an employee in the current payrun (default: <c>true</c>).
/// </summary>
/// <remarks>
/// This function is evaluated once per wage type per employee, before
/// <see cref="WageTypeValueFunction"/> runs. Returning <c>false</c> skips the wage type
/// entirely for this employee in this payrun run: no value is calculated, no result is
/// stored, and no collectors are fed by that wage type.
/// <para>The current wage type is identified by <see cref="WageTypeNumber"/>.
/// Use this to write a single function body that handles multiple wage types differently
/// based on their number.</para>
/// <para>Typical uses:</para>
/// <list type="bullet">
///   <item>Restrict a bonus wage type to a specific period, for example December only.</item>
///   <item>Skip a wage type for employees who do not qualify (e.g. no relevant case value).</item>
///   <item>Disable a wage type during retro runs by testing <see cref="PayrunFunction.IsRetroPayrun"/>.</item>
/// </list>
/// <para><strong>Return value:</strong> Return <c>true</c> or <c>null</c> to evaluate the wage type.
/// Return <c>false</c> to skip it.</para>
/// </remarks>
/// <example>
/// <code language="c#">
/// // Only evaluate wage type 2250 in December
/// WageTypeNumber == 2250 &amp;&amp; PeriodStart.Month == 12
/// </code>
/// <code language="c#">
/// // Skip any wage type for employees without a salary case value
/// GetCaseValue&lt;decimal?&gt;("Salary").HasValue
/// </code>
/// </example>
/// <seealso cref="PayrunEmployeeAvailableFunction"/>
/// <seealso cref="WageTypeValueFunction"/>
// ReSharper disable once PartialTypeWithSinglePart
public partial class PayrunWageTypeAvailableFunction : PayrunFunction
{
    /// <summary>Initializes a new instance with the function runtime</summary>
    /// <param name="runtime">The runtime</param>
    public PayrunWageTypeAvailableFunction(object runtime) :
        base(runtime)
    {
        // wage type
        WageTypeNumber = Runtime.WageTypeNumber;
    }

    /// <summary>New function instance without runtime (scripting development)</summary>
    /// <remarks>Use <see cref="Function.GetSourceFileName"/> in your constructor for the source file name</remarks>
    /// <param name="sourceFileName">The name of the source file</param>
    protected PayrunWageTypeAvailableFunction(string sourceFileName) :
        base(sourceFileName)
    {
    }

    /// <summary>The wage type number</summary>
    public decimal WageTypeNumber { get; }

    /// <summary>Get wage type attribute value</summary>
    public object GetWageTypeAttribute(string attributeName) =>
        Runtime.GetWageTypeAttribute(attributeName);

    /// <summary>Get wage type attribute typed value</summary>
    public T GetWageTypeAttribute<T>(string attributeName, T defaultValue = default) =>
        ChangeValueType(Runtime.GetWageTypeAttribute(attributeName), defaultValue);

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