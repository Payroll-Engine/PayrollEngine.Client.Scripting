/* PayrunEndFunction */

// ReSharper disable RedundantUsingDirective
using System;
using System.Linq;
using System.Collections.Generic;
using PayrollEngine.Client.Scripting;
// ReSharper restore RedundantUsingDirective

namespace PayrollEngine.Client.Scripting.Function;

/// <summary>
/// Executes once at the end of a payrun, after all employees have been processed.
/// </summary>
/// <remarks>
/// This function is the global finalizer for a payrun job. It runs exactly once, after
/// every employee's wage types, collectors, and employee-level functions have completed.
/// <para>Typical uses:</para>
/// <list type="bullet">
///   <item>Aggregate cross-employee results and write payrun-level results
///   (<see cref="PayrunFunction.SetPayrunResult(string, object, ValueType?, string, System.Collections.Generic.IEnumerable{string}, System.Collections.Generic.Dictionary{string, object}, string)"/>).</item>
///   <item>Read all employee runtime values via <see cref="GetRuntimeValuesEmployees"/>
///   and <see cref="GetEmployeeRuntimeValues"/> for summary calculations.</item>
///   <item>Log payrun completion statistics or trigger downstream webhooks.</item>
/// </list>
/// <para><strong>Return value:</strong> This function returns <c>void</c>; there is no early-abort
/// mechanism at payrun end.</para>
/// <para><strong>Runtime value access:</strong> <see cref="GetPayrunRuntimeValues"/> returns all
/// payrun-scoped key/value pairs written during the run. Per-employee values are accessible
/// via <see cref="GetRuntimeValuesEmployees"/> (list of identifiers) and
/// <see cref="GetEmployeeRuntimeValues"/> (values for a given employee).</para>
/// </remarks>
/// <example>
/// <code language="c#">
/// // Sum up a custom runtime value from all processed employees
/// var total = 0m;
/// foreach (var employeeId in GetRuntimeValuesEmployees())
/// {
///     var values = GetEmployeeRuntimeValues(employeeId);
///     if (values.TryGetValue("BonusAmount", out var raw))
///         total += decimal.Parse(raw);
/// }
/// SetPayrunResult("TotalBonus", total);
/// </code>
/// </example>
/// <seealso cref="PayrunStartFunction"/>
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

    /// <summary>Returns the identifiers of all employees that have runtime values set during this payrun</summary>
    /// <returns>List of employee identifiers with at least one runtime value</returns>
    public List<string> GetRuntimeValuesEmployees() =>
        Runtime.GetRuntimeValuesEmployees();

    /// <summary>Get employee runtime values</summary>
    /// <param name="employeeIdentifier">The employee identifier</param>
    /// <returns>Employee runtime values</returns>
    public Dictionary<string, string> GetEmployeeRuntimeValues(string employeeIdentifier) =>
        Runtime.GetEmployeeRuntimeValues(employeeIdentifier);

    #endregion

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