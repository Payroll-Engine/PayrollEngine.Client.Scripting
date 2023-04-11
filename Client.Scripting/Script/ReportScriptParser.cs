using System;
using PayrollEngine.Client.Script;

namespace PayrollEngine.Client.Scripting.Script;

internal sealed class ReportScriptParser : ScriptParserBase, IReportScriptParser
{
    public string GetReportBuildScript(ScriptCodeQuery query, string regulationName, string reportName)
    {
        if (string.IsNullOrWhiteSpace(regulationName))
        {
            throw new ArgumentException(nameof(regulationName));
        }

        if (string.IsNullOrWhiteSpace(reportName))
        {
            throw new ArgumentException(nameof(reportName));
        }

        return GetScript<ReportBuildFunctionAttribute, ReportBuildScriptAttribute>
        (query.TenantIdentifier, query.SourceCode,
            x => string.Equals(x.RegulationName, regulationName),
            x => string.Equals(x.ReportName, reportName));
    }

    public string GetReportStartScript(ScriptCodeQuery query, string regulationName, string reportName)
    {
        if (string.IsNullOrWhiteSpace(regulationName))
        {
            throw new ArgumentException(nameof(regulationName));
        }

        if (string.IsNullOrWhiteSpace(reportName))
        {
            throw new ArgumentException(nameof(reportName));
        }

        return GetScript<ReportStartFunctionAttribute, ReportStartScriptAttribute>
        (query.TenantIdentifier, query.SourceCode,
            x => string.Equals(x.RegulationName, regulationName),
            x => string.Equals(x.ReportName, reportName));
    }

    public string GetReportEndScript(ScriptCodeQuery query, string regulationName, string reportName)
    {
        if (string.IsNullOrWhiteSpace(regulationName))
        {
            throw new ArgumentException(nameof(regulationName));
        }

        if (string.IsNullOrWhiteSpace(reportName))
        {
            throw new ArgumentException(nameof(reportName));
        }

        return GetScript<ReportEndFunctionAttribute, ReportEndScriptAttribute>
        (query.TenantIdentifier, query.SourceCode,
            x => string.Equals(x.RegulationName, regulationName),
            x => string.Equals(x.ReportName, reportName));
    }
}