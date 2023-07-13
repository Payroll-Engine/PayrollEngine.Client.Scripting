
namespace PayrollEngine.Client.Scripting.Runtime;

/// <summary>Runtime for the report build function <see cref="Function.ReportBuildFunction"/></summary>
public interface IReportBuildRuntime : IReportRuntime
{
    /// <summary>Set report parameter value</summary>
    /// <param name="parameterName">The parameter name</param>
    /// <param name="value">The parameter value as JSON</param>
    void SetParameter(string parameterName, string value);

    /// <summary>Set report attribute value</summary>
    /// <param name="parameterName">The parameter name</param>
    /// <param name="attributeName">Name of the attribute</param>
    /// <param name="value">The attribute value</param>
    /// <returns>The report attribute value</returns>
    void SetParameterAttribute(string parameterName, string attributeName, object value);

    /// <summary>Set the report parameter hidden state</summary>
    /// <param name="parameterName">The parameter name</param>
    /// <param name="hidden">The hidden state</param>
    void SetParameterHidden(string parameterName, bool hidden);
}