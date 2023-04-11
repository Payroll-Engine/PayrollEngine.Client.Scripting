using System;
using PayrollEngine.Client.Script;

namespace PayrollEngine.Client.Scripting.Script;

internal sealed class CaseRelationScriptParser : ScriptParserBase, ICaseRelationScriptParser
{
    public string GetCaseRelationBuildScript(ScriptCodeQuery query, string regulationName,
        string sourceCaseName, string targetCaseName, string sourceCaseSlot = null, string targetCaseSlot = null)
    {
        if (string.IsNullOrWhiteSpace(regulationName))
        {
            throw new ArgumentException(nameof(regulationName));
        }
        if (string.IsNullOrWhiteSpace(sourceCaseName))
        {
            throw new ArgumentException(nameof(sourceCaseName));
        }
        if (string.IsNullOrWhiteSpace(targetCaseName))
        {
            throw new ArgumentException(nameof(targetCaseName));
        }

        return GetScript<CaseRelationBuildFunctionAttribute, CaseRelationScriptAttribute>
        (query.TenantIdentifier, query.SourceCode,
            x => string.Equals(x.RegulationName, regulationName),
            x => string.Equals(x.SourceCaseName, sourceCaseName) &&
                 string.Equals(x.SourceCaseSlot, sourceCaseSlot) &&
                 string.Equals(x.TargetCaseName, targetCaseName) &&
                 string.Equals(x.TargetCaseSlot, targetCaseSlot));
    }

    public string GetCaseRelationValidateScript(ScriptCodeQuery query, string regulationName,
        string sourceCaseName, string targetCaseName, string sourceCaseSlot = null, string targetCaseSlot = null)
    {
        if (string.IsNullOrWhiteSpace(regulationName))
        {
            throw new ArgumentException(nameof(regulationName));
        }
        if (string.IsNullOrWhiteSpace(sourceCaseName))
        {
            throw new ArgumentException(nameof(sourceCaseName));
        }
        if (string.IsNullOrWhiteSpace(targetCaseName))
        {
            throw new ArgumentException(nameof(targetCaseName));
        }

        return GetScript<CaseRelationValidateFunctionAttribute, CaseRelationValidateScriptAttribute>
        (query.TenantIdentifier, query.SourceCode,
            x => string.Equals(x.RegulationName, regulationName),
            x => string.Equals(x.SourceCaseName, sourceCaseName) &&
                 string.Equals(x.SourceCaseSlot, sourceCaseSlot) &&
                 string.Equals(x.TargetCaseName, targetCaseName) &&
                 string.Equals(x.TargetCaseSlot, targetCaseSlot));
    }
}