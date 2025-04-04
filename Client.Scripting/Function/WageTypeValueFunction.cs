﻿/* WageTypeValueFunction */

// ReSharper disable RedundantUsingDirective
using System;
using System.Linq;
using System.Collections.Generic;
using PayrollEngine.Client.Scripting;
// ReSharper restore RedundantUsingDirective

namespace PayrollEngine.Client.Scripting.Function;

/// <summary>Calculate the wage type value</summary>
/// <example>
/// <code language="c#">
/// // Example from case value
/// Employee["Wage"]
/// </code>
/// <code language="c#">
/// // Example from conditional case payroll value
/// (int)Employee["Wage"] > 0 ? Employee["ManagementWage"] : PayrollValue.Empty
/// </code>
/// <code language="c#">
/// // Example from running collector
/// Collector["MyCollector"]
/// </code>
/// <code language="c#">
/// // Example from running wage type
/// WageType[2300]
/// </code>
/// <code language="c#">
/// // Example with custom wage type result
/// SetResult("MyResult", 5300); return WageType[2300]
/// </code>
/// <code language="c#">
/// // Example with custom wage type result including the value type
/// SetResult("MyResult", 5300, 2); return WageType[2300]
/// </code>
/// <code language="c#">
/// // Example with custom wage type result including the value description
/// SetResult("MyResult", 5300, "KST1"); return WageType[2300]
/// </code>
/// <code language="c#">
/// // Example with custom wage type result including the value type and description
/// SetResult("MyResult", 5300, 2, "KST1"); return WageType[2300]
/// </code>
/// <code language="c#">
/// // Example with average wage type result from the last 3 periods on legal payrun jobs
/// GetWageTypeResults(2300, new PeriodResultQuery(3, PayrunJobStatus.Legal)).DefaultIfEmpty().Average()
/// </code>
/// </example>
/// <seealso cref="WageTypeResultFunction">Wage Type Result Function</seealso>
// ReSharper disable once PartialTypeWithSinglePart
public partial class WageTypeValueFunction : WageTypeFunction
{
    /// <summary>Initializes a new instance with the function runtime</summary>
    /// <param name="runtime">The runtime</param>
    public WageTypeValueFunction(object runtime) :
        base(runtime)
    {
    }

    /// <summary>New function instance without runtime (scripting development)</summary>
    /// <remarks>Use <see cref="Function.GetSourceFileName"/> in your constructor for the source file name</remarks>
    /// <param name="sourceFileName">The name of the source file</param>
    protected WageTypeValueFunction(string sourceFileName) :
        base(sourceFileName)
    {
    }

    /// <summary>The execution count</summary>
    public int ExecutionCount => Runtime.ExecutionCount;

    /// <summary>Restart execution of wage type calculation</summary>
    public void RestartExecution() => Runtime.RestartExecution();

    /// <summary>Entry point for the runtime</summary>
    /// <remarks>Internal usage only, do not call this method</remarks>
    public object GetValue()
    {
        // ReSharper disable EmptyRegion
        #region Function
        #endregion
        // ReSharper restore EmptyRegion
        // compiler will optimize this out if the code provides a return
        return null;
    }
}