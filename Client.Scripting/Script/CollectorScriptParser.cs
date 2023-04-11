using System;
using PayrollEngine.Client.Script;

namespace PayrollEngine.Client.Scripting.Script;

internal sealed class CollectorScriptParser : ScriptParserBase, ICollectorScriptParser
{
    public string GetCollectorStartScript(ScriptCodeQuery query, string regulationName, string collectorName)
    {
        if (string.IsNullOrWhiteSpace(regulationName))
        {
            throw new ArgumentException(nameof(regulationName));
        }
        if (string.IsNullOrWhiteSpace(collectorName))
        {
            throw new ArgumentException(nameof(collectorName));
        }

        return GetScript<CollectorStartFunctionAttribute, CollectorStartScriptAttribute>
        (query.TenantIdentifier, query.SourceCode,
            x => string.Equals(x.RegulationName, regulationName),
            x => string.Equals(x.CollectorName, collectorName));
    }

    public string GetCollectorApplyScript(ScriptCodeQuery query, string regulationName, string collectorName)
    {
        if (string.IsNullOrWhiteSpace(regulationName))
        {
            throw new ArgumentException(nameof(regulationName));
        }
        if (string.IsNullOrWhiteSpace(collectorName))
        {
            throw new ArgumentException(nameof(collectorName));
        }

        return GetScript<CollectorApplyFunctionAttribute, CollectorApplyScriptAttribute>
        (query.TenantIdentifier, query.SourceCode,
            x => string.Equals(x.RegulationName, regulationName),
            x => string.Equals(x.CollectorName, collectorName));
    }

    public string GetCollectorEndScript(ScriptCodeQuery query, string regulationName, string collectorName)
    {
        if (string.IsNullOrWhiteSpace(regulationName))
        {
            throw new ArgumentException(nameof(regulationName));
        }
        if (string.IsNullOrWhiteSpace(collectorName))
        {
            throw new ArgumentException(nameof(collectorName));
        }

        return GetScript<CollectorEndFunctionAttribute, CollectorEndScriptAttribute>
        (query.TenantIdentifier, query.SourceCode,
            x => string.Equals(x.RegulationName, regulationName),
            x => string.Equals(x.CollectorName, collectorName));
    }
}