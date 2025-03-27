using System;
using PayrollEngine.Client.Script;

namespace PayrollEngine.Client.Scripting.Script;

internal sealed class CaseScriptParser : ScriptParserBase, ICaseScriptParser
{
    public string GetCaseAvailableScript(ScriptCodeQuery query, string regulationName, string caseName)
    {
        if (string.IsNullOrWhiteSpace(regulationName))
        {
            throw new ArgumentException(nameof(regulationName));
        }

        if (string.IsNullOrWhiteSpace(caseName))
        {
            throw new ArgumentException(nameof(caseName));
        }

        return GetScript<CaseAvailableFunctionAttribute, CaseAvailableScriptAttribute>
        (query.TenantIdentifier, query.SourceCode,
            x => string.Equals(x.RegulationName, regulationName),
            x => string.Equals(x.CaseName, caseName));
    }

    public string GetCaseBuildScript(ScriptCodeQuery query, string regulationName, string caseName)
    {
        if (string.IsNullOrWhiteSpace(regulationName))
        {
            throw new ArgumentException(nameof(regulationName));
        }

        if (string.IsNullOrWhiteSpace(caseName))
        {
            throw new ArgumentException(nameof(caseName));
        }

        return GetScript<CaseBuildFunctionAttribute, CaseBuildScriptAttribute>
        (query.TenantIdentifier, query.SourceCode,
            x => string.Equals(x.RegulationName, regulationName),
            x => string.Equals(x.CaseName, caseName));
    }

    public string GetCaseValidateScript(ScriptCodeQuery query, string regulationName, string caseName)
    {
        if (string.IsNullOrWhiteSpace(regulationName))
        {
            throw new ArgumentException(nameof(regulationName));
        }

        if (string.IsNullOrWhiteSpace(caseName))
        {
            throw new ArgumentException(nameof(caseName));
        }

        return GetScript<CaseValidateFunctionAttribute, CaseValidateScriptAttribute>
        (query.TenantIdentifier, query.SourceCode,
            x => string.Equals(x.RegulationName, regulationName),
            x => string.Equals(x.CaseName, caseName));
    }
}