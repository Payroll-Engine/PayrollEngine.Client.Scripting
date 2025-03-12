
namespace PayrollEngine.Client.Scripting.Runtime;

/// <summary>Runtime for the report build function <see cref="Function.ReportBuildFunction"/></summary>
public interface IReportBuildRuntime : IReportRuntime
{
    /// <summary>Set report parameter value</summary>
    /// <param name="parameterName">The parameter name</param>
    /// <param name="value">The parameter value as JSON</param>
    void SetParameter(string parameterName, string value);

    /// <summary>Set the report parameter hidden state</summary>
    /// <param name="parameterName">The parameter name</param>
    /// <param name="hidden">The hidden state</param>
    void SetParameterHidden(string parameterName, bool hidden);
}