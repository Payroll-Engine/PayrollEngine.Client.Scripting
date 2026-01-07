/* CaseAvailableFunction */

// ReSharper disable RedundantUsingDirective
using System;
using System.Linq;
using System.Collections.Generic;
using PayrollEngine.Client.Scripting;
// ReSharper restore RedundantUsingDirective
// ReSharper disable EmptyRegion

namespace PayrollEngine.Client.Scripting.Function;

/// <summary>Test if a case is available (default: true), optionally considering related source case values</summary>
/// <example>
/// <code language="c#">
/// // Example with case value
/// (int)Employee["Level"] >= 2
/// </code>
/// <code language="c#">
/// // Example with related case value
/// HasCaseValue("Wage")
/// </code>
/// <code language="c#">
/// // Example with optional related case value
/// HasCaseValue("Wage") ? (int)Employee["Level"] >= 2 : false
/// </code>
/// </example>
// ReSharper disable once PartialTypeWithSinglePart
public partial class CaseAvailableFunction : CaseFunction
{
    /// <summary>Initializes a new instance with the function runtime</summary>
    /// <param name="runtime">The runtime</param>
    public CaseAvailableFunction(object runtime) :
        base(runtime)
    {
    }

    /// <summary>New function instance without runtime (scripting development)</summary>
    /// <remarks>Use <see cref="Function.GetSourceFileName"/> in your constructor for the source file name</remarks>
    /// <param name="sourceFileName">The name of the source file</param>
    protected CaseAvailableFunction(string sourceFileName) :
        base(sourceFileName)
    {
    }

    #region Action
    #endregion

    /// <summary>Entry point for the runtime</summary>
    /// <remarks>Internal usage only, do not call this method</remarks>
    public bool? IsAvailable()
    {
        #region ActionInvoke
        #endregion

        #region Function
        #endregion

        // compiler will optimize this out if the code provides a return
        return null;
    }
}