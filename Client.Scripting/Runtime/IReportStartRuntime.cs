
namespace PayrollEngine.Client.Scripting.Runtime;

/// <summary>Runtime for the report start function <see cref="Function.ReportStartFunction"/></summary>
public interface IReportStartRuntime : IReportRuntime
{
    /// <summary>Set report parameter value</summary>
    /// <param name="parameterName">The parameter name</param>
    /// <param name="value">The parameter value as JSON</param>
    void SetParameter(string parameterName, string value);

    /// <summary>Check for existing report query</summary>
    /// <param name="queryName">The query name</param>
    bool HasQuery(string queryName);

    /// <summary>Get report query</summary>
    /// <param name="queryName">The query name</param>
    /// <returns>The report parameter value as JSON</returns>
    string GetQuery(string queryName);

    /// <summary>Set report query value as JSON</summary>
    /// <param name="queryName">The query name</param>
    /// <param name="value">The query value as JSON</param>
    void SetQuery(string queryName, string value);
}