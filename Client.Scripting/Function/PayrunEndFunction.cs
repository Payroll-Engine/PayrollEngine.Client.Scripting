/* PayrunEndFunction */
// ReSharper disable RedundantUsingDirective
using System;
using System.Collections.Generic;
using System.Linq;
// ReSharper restore RedundantUsingDirective

namespace PayrollEngine.Client.Scripting.Function;

/// <summary>Payrun end function</summary>
/// <seealso cref="PayrunWageTypeAvailableFunction">Payrun Wage Type Available Function</seealso>
// ReSharper disable once PartialTypeWithSinglePart
public partial class PayrunEndFunction : PayrunFunction
{
    /// <summary>Initializes a new instance with the function runtime</summary>
    /// <param name="runtime">The runtime</param>
    public PayrunEndFunction(object runtime) :
        base(runtime)
    {
    }

    /// <summary>New function instance without runtime (scripting development)</summary>
    /// <remarks>Use <see cref="Function.GetSourceFileName"/> in your constructor for the source file name</remarks>
    /// <param name="sourceFileName">The name of the source file</param>
    protected PayrunEndFunction(string sourceFileName) :
        base(sourceFileName)
    {
    }

    #region Runtime Values

    /// <summary>Get payrun runtime values</summary>
    /// <returns>Payrun runtime values</returns>
    public Dictionary<string, string> GetPayrunRuntimeValues() =>
        Runtime.GetPayrunRuntimeValues();

    /// <summary>Get the employees with runtime values</summary>
    /// <returns>Payrun runtime values</returns>
    public List<string> GetRuntimeValuesEmployees() =>
        Runtime.GetRuntimeValuesEmployees();

    /// <summary>Get employee runtime values</summary>
    /// <param name="employeeIdentifier">The employee identifier</param>
    /// <returns>Employee runtime values</returns>
    public Dictionary<string, string> GetEmployeeRuntimeValues(string employeeIdentifier) =>
        Runtime.GetEmployeeRuntimeValues(employeeIdentifier);

    #endregion

    /// <exclude />
    public void End()
    {
        // ReSharper disable EmptyRegion
        #region Function
        #endregion
        // ReSharper restore EmptyRegion
    }
}