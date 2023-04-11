using System;
using System.Globalization;
using PayrollEngine.Client.Script;

namespace PayrollEngine.Client.Scripting.Script;

internal sealed class WageTypeScriptParser : ScriptParserBase, IWageTypeScriptParser
{
    public string GetWageTypeResultScript(ScriptCodeQuery query, string regulationName, decimal wageTypeNumber)
    {
        if (string.IsNullOrWhiteSpace(regulationName))
        {
            throw new ArgumentException(nameof(regulationName));
        }

        return GetScript<WageTypeResultFunctionAttribute, WageTypeResultScriptAttribute>
        (query.TenantIdentifier, query.SourceCode,
            x => string.Equals(x.RegulationName, regulationName),
            x => decimal.Parse(x.WageTypeNumber, CultureInfo.InvariantCulture) == wageTypeNumber);
    }

    public string GetWageTypeValueScript(ScriptCodeQuery query, string regulationName, decimal wageTypeNumber)
    {
        if (string.IsNullOrWhiteSpace(regulationName))
        {
            throw new ArgumentException(nameof(regulationName));
        }

        return GetScript<WageTypeValueFunctionAttribute, WageTypeValueScriptAttribute>
        (query.TenantIdentifier, query.SourceCode,
            x => string.Equals(x.RegulationName, regulationName),
            x => decimal.Parse(x.WageTypeNumber, CultureInfo.InvariantCulture) == wageTypeNumber);
    }
}