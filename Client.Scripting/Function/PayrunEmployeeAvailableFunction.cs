/* PayrunEmployeeAvailableFunction */
// ReSharper disable RedundantUsingDirective
using System;
using System.Collections.Generic;
using System.Linq;
// ReSharper restore RedundantUsingDirective

namespace PayrollEngine.Client.Scripting.Function;

/// <summary>Test if an employee is available (default: true) for a payrun</summary>
/// <example>
/// <code language="c#">
/// // Example with employee case value
/// (int)Employee["Level"] >= 2
/// </code>
/// <code language="c#">
/// // Example with payrun period
/// PeriodStart.Month == 12
/// </code>
/// </example>
/// <seealso cref="PayrunWageTypeAvailableFunction">Payrun Wage Type Available Function</seealso>
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
        return default;
    }
}