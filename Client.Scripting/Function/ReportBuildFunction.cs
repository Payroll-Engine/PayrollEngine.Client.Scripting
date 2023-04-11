/* ReportBuildFunction */
// ReSharper disable RedundantUsingDirective
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using PayrollEngine.Client.Scripting;
using PayrollEngine.Client.Scripting.Report;
// ReSharper restore RedundantUsingDirective

namespace PayrollEngine.Client.Scripting.Function;

/// <summary>Report build function</summary>
// ReSharper disable once PartialTypeWithSinglePart
public partial class ReportBuildFunction : ReportFunction
{
    /// <summary>Initializes a new instance with the function runtime</summary>
    /// <param name="runtime">The runtime</param>
    public ReportBuildFunction(object runtime) :
        base(runtime)
    {
    }

    /// <summary>New function instance without runtime (scripting development)</summary>
    /// <param name="sourceFileName">The name of the source file</param>
    public ReportBuildFunction(string sourceFileName) :
        base(sourceFileName)
    {
    }

    /// <summary>Set report parameter value</summary>
    /// <param name="parameterName">The parameter name</param>
    /// <param name="value">The parameter value as JSON</param>
    public void SetParameter(string parameterName, string value) =>
        Runtime.SetParameter(parameterName, value);

    /// <summary>Set report parameter typed value</summary>
    /// <param name="parameterName">The parameter name</param>
    /// <param name="value">The default value</param>
    /// <returns>The report parameter value</returns>
    public void SetParameter<T>(string parameterName, T value) =>
        SetParameter(parameterName, value as string ?? JsonSerializer.Serialize(value));

    /// <summary>Set report attribute value</summary>
    /// <param name="parameterName">The parameter name</param>
    /// <param name="attributeName">Name of the attribute</param>
    /// <param name="value">The attribute value</param>
    /// <returns>The report attribute value</returns>
    public void SetParameterAttribute(string parameterName, string attributeName, object value) =>
        Runtime.SetParameterAttribute(parameterName, attributeName, value);

    /// <exclude />
    public bool? Build()
    {
        // ReSharper disable EmptyRegion
        #region Function
        #endregion
        // ReSharper restore EmptyRegion
        // compiler will optimize this out if the code provides a return
        return default;
    }
}