using System.Collections.Generic;

namespace PayrollEngine.Client.Scripting.Runtime;

/// <summary>Runtime for the payrun employee available function <see cref="Function.PayrunEndFunction"/></summary>
public interface IPayrunEndRuntime : IPayrunRuntime
{
    #region Runtime Values

    /// <summary>Get payrun runtime values</summary>
    /// <returns>Payrun runtime values</returns>
    Dictionary<string, string> GetPayrunRuntimeValues();

    /// <summary>Get the employees with runtime values</summary>
    /// <returns>Payrun runtime values</returns>
    List<string> GetRuntimeValuesEmployees();

    /// <summary>Get employee runtime values</summary>
    /// <param name="employeeIdentifier">The employee identifier</param>
    /// <returns>Employee runtime values</returns>
    Dictionary<string, string> GetEmployeeRuntimeValues(string employeeIdentifier);

    #endregion
}