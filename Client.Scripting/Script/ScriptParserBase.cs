using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PayrollEngine.Client.Scripting.Script;

internal abstract class ScriptParserBase
{
    protected string GetScript<TFunc, TScript>(string tenantIdentifier,
        string sourceCode, Func<TFunc, bool> functionHandler, Func<TScript, bool> scriptHandler)
        where TFunc : FunctionAttribute
        where TScript : ScriptAttribute
    {
        if (string.IsNullOrWhiteSpace(tenantIdentifier))
        {
            throw new ArgumentException(nameof(tenantIdentifier));
        }

        if (string.IsNullOrWhiteSpace(sourceCode))
        {
            throw new ArgumentException(nameof(sourceCode));
        }

        if (functionHandler == null)
        {
            throw new ArgumentNullException(nameof(functionHandler));
        }

        if (scriptHandler == null)
        {
            throw new ArgumentNullException(nameof(scriptHandler));
        }

        var classes = GetSourceClasses(sourceCode);

        // functions/classes by tenant
        foreach (var @class in classes)
        {
            // tenant
            if (!string.Equals(@class.FunctionAttribute.TenantIdentifier, tenantIdentifier))
            {
                continue;
            }

            // matching function
            if (@class.FunctionAttribute is not TFunc functionAttribute || !functionHandler(functionAttribute))
            {
                continue;
            }

            // methods
            foreach (var method in @class.Methods)
            {
                // matching script
                if (method.Value is not TScript scriptAttribute || !scriptHandler(scriptAttribute))
                {
                    continue;
                }

                // method body
                var body = GetMethodBody(method);
                if (!string.IsNullOrWhiteSpace(body))
                {
                    return body;
                }
            }
        }

        return null;
    }

    private static IList<ScriptClass> GetSourceClasses(string sourceCode)
    {
        // source code
        var classes = ScriptClassParser.FromSource(sourceCode);
        if (!classes.Any())
        {
            throw new ScriptPublishException("Missing scripting classes in source.");
        }

        return classes;
    }

    private string GetMethodBody(KeyValuePair<MethodDeclarationSyntax, ScriptAttribute> method)
    {
        var methodBody = method.Key.Body?.ToString();
        if (string.IsNullOrWhiteSpace(methodBody))
        {
            return null;
        }

        // ensure same content as imported from JSON
        methodBody = methodBody.TrimStart('{', '\r', '\n');
        methodBody = methodBody.TrimEnd(' ', '}');
        return methodBody;
    }
}