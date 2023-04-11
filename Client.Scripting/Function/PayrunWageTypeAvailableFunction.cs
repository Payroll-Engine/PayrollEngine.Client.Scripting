/* PayrunWageTypeAvailableFunction */
// ReSharper disable RedundantUsingDirective
using System;
using System.Collections.Generic;
using System.Linq;
// ReSharper restore RedundantUsingDirective

namespace PayrollEngine.Client.Scripting.Function;

/// <summary>Test if a wage type is available (default: true) for a payrun</summary>
/// <example>
/// <code language="c#">
/// // Example with payrun period
/// WageTypeNumber == 2250 &amp;&amp; PeriodStart.Month == 12
/// </code>
/// </example>
/// <seealso cref="PayrunEmployeeAvailableFunction">Payrun Employee Available Function</seealso>
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
    public T GetWageTypeAttribute<T>(string attributeName, T defaultValue = default)
    {
        var value = Runtime.GetWageTypeAttribute(attributeName);
        return value == null ? defaultValue : (T)Convert.ChangeType(value, typeof(T));
    }

    /// <exclude />
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