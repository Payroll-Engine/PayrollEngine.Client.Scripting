using System;
using PayrollEngine.Client.Script;

namespace PayrollEngine.Client.Scripting.Script;

internal sealed class PayrunScriptParser : ScriptParserBase, IPayrunScriptParser
{
    public string GetPayrunStartScript(ScriptCodeQuery query, string payrunName)
    {
        if (string.IsNullOrWhiteSpace(payrunName))
        {
            throw new ArgumentException(nameof(payrunName));
        }

        return GetScript<PayrunStartFunctionAttribute, PayrunStartScriptAttribute>
        (query.TenantIdentifier, query.SourceCode,
            _ => true,
            x => string.Equals(x.PayrunName, payrunName));
    }

    public string GetPayrunWageTypeAvailableScript(ScriptCodeQuery query, string payrunName)
    {
        if (string.IsNullOrWhiteSpace(payrunName))
        {
            throw new ArgumentException(nameof(payrunName));
        }

        return GetScript<PayrunWageTypeAvailableFunctionAttribute, PayrunWageTypeAvailableScriptAttribute>
        (query.TenantIdentifier, query.SourceCode,
            _ => true,
            x => string.Equals(x.PayrunName, payrunName));
    }

    public string GetPayrunEmployeeAvailableScript(ScriptCodeQuery query, string payrunName)
    {
        if (string.IsNullOrWhiteSpace(payrunName))
        {
            throw new ArgumentException(nameof(payrunName));
        }

        return GetScript<PayrunEmployeeAvailableFunctionAttribute, PayrunEmployeeAvailableScriptAttribute>
        (query.TenantIdentifier, query.SourceCode,
            _ => true,
            x => string.Equals(x.PayrunName, payrunName));
    }

    public string GetPayrunEmployeeStartScript(ScriptCodeQuery query, string payrunName)
    {
        if (string.IsNullOrWhiteSpace(payrunName))
        {
            throw new ArgumentException(nameof(payrunName));
        }

        return GetScript<PayrunEmployeeStartFunctionAttribute, PayrunEmployeeStartScriptAttribute>
        (query.TenantIdentifier, query.SourceCode,
            _ => true,
            x => string.Equals(x.PayrunName, payrunName));
    }

    public string GetPayrunEmployeeEndScript(ScriptCodeQuery query, string payrunName)
    {
        if (string.IsNullOrWhiteSpace(payrunName))
        {
            throw new ArgumentException(nameof(payrunName));
        }

        return GetScript<PayrunEmployeeEndFunctionAttribute, PayrunEmployeeEndScriptAttribute>
        (query.TenantIdentifier, query.SourceCode,
            _ => true,
            x => string.Equals(x.PayrunName, payrunName));
    }

    public string GetPayrunEndScript(ScriptCodeQuery query, string payrunName)
    {
        if (string.IsNullOrWhiteSpace(payrunName))
        {
            throw new ArgumentException(nameof(payrunName));
        }

        return GetScript<PayrunEndFunctionAttribute, PayrunEndScriptAttribute>
        (query.TenantIdentifier, query.SourceCode,
            _ => true,
            x => string.Equals(x.PayrunName, payrunName));
    }
}