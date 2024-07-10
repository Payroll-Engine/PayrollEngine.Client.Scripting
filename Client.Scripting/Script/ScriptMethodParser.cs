using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PayrollEngine.Client.Scripting.Script;

/// <summary>Script method parser</summary>
public class ScriptMethodParser
{
    private static readonly string[] ScriptAttributeNames =
    [
        nameof(CaseAvailableScriptAttribute),
        nameof(CaseBuildScriptAttribute),
        nameof(CaseValidateScriptAttribute),

        nameof(CaseRelationBuildScriptAttribute),
        nameof(CaseRelationValidateScriptAttribute),

        nameof(CollectorStartScriptAttribute),
        nameof(CollectorApplyScriptAttribute),
        nameof(CollectorEndScriptAttribute),

        nameof(WageTypeValueScriptAttribute),
        nameof(WageTypeResultScriptAttribute),

        nameof(PayrunStartFunctionAttribute),
        nameof(PayrunEmployeeAvailableScriptAttribute),
        nameof(PayrunWageTypeAvailableScriptAttribute),
        nameof(PayrunEndFunctionAttribute),

        nameof(ReportBuildScriptAttribute),
        nameof(ReportStartScriptAttribute),
        nameof(ReportEndScriptAttribute)
    ];

    /// <summary>The syntax tree</summary>
    public ClassDeclarationSyntax ClassSyntax { get; }

    /// <summary>the function attribute</summary>
    public Dictionary<MethodDeclarationSyntax, ScriptAttribute> MethodAttributes { get; } = new();

    /// <summary>Initializes a new instance of the <see cref="ScriptMethodParser"/> class</summary>
    /// <param name="classSyntaxTree">The function class code syntax tree</param>
    public ScriptMethodParser(ClassDeclarationSyntax classSyntaxTree)
    {
        ClassSyntax = classSyntaxTree ?? throw new ArgumentNullException(nameof(classSyntaxTree));

        var methods = ClassSyntax.DescendantNodes().OfType<MethodDeclarationSyntax>();
        foreach (var method in methods)
        {
            foreach (var attributeList in method.AttributeLists)
            {
                ScriptAttribute scriptAttribute = null;
                var attributeParameters = new Dictionary<string, string>();

                foreach (var attribute in attributeList.Attributes)
                {
                    var attributeName = attribute.Name.ToString().EnsureEnd("Attribute");
                    if (!ScriptAttributeNames.Contains(attributeName))
                    {
                        throw new NotSupportedException($"Unsupported script  attribute {attributeName}");
                    }

                    if (attribute.ArgumentList != null)
                    {
                        foreach (var argument in attribute.ArgumentList.Arguments)
                        {
                            if (argument.NameColon != null)
                            {
                                var parameterName = argument.NameColon.Name.ToString();
                                var parameterValue = argument.Expression.ToString().Trim('"');
                                if (!string.IsNullOrWhiteSpace(parameterName) && !string.IsNullOrWhiteSpace(parameterValue))
                                {
                                    attributeParameters.Add(parameterName, parameterValue);
                                }
                            }
                        }
                    }

                    scriptAttribute = ScriptAttributeFactory.CreateScriptAttribute(attributeName, attributeParameters);
                }

                MethodAttributes.Add(method, scriptAttribute);
            }
        }
    }

    #region Factory

    private static class ScriptAttributeFactory
    {
        private sealed class ScriptAttributeInfo
        {
            internal string[] Parameters { get; }

            internal ScriptAttributeInfo(string[] parameters)
            {
                Parameters = parameters;
            }
        }

        // type registration
        // the dictionary value represents the script parameters required by the function attribute ctor
        private static readonly Dictionary<Type, ScriptAttributeInfo> ScriptAttributes = new()
        {
            // case
            { typeof(CaseAvailableScriptAttribute), new(["caseName"]) },
            { typeof(CaseBuildScriptAttribute), new(["caseName"]) },
            { typeof(CaseValidateScriptAttribute), new(["caseName"]) },
            // case relation
            { typeof(CaseRelationBuildScriptAttribute), new(["sourceCaseName", "targetCaseName", "sourceCaseSlot", "targetCaseSlot"
            ]) },
            { typeof(CaseRelationValidateScriptAttribute), new(["sourceCaseName", "targetCaseName", "sourceCaseSlot", "targetCaseSlot"
            ]) },
            // collector
            { typeof(CollectorStartScriptAttribute), new(["collectorName"]) },
            { typeof(CollectorApplyScriptAttribute), new(["collectorName"]) },
            { typeof(CollectorEndScriptAttribute), new(["collectorName"]) },
            // wage type
            { typeof(WageTypeValueScriptAttribute), new(["wageTypeNumber"]) },
            { typeof(WageTypeResultScriptAttribute), new(["wageTypeNumber"]) },
            // payrun
            { typeof(PayrunStartScriptAttribute), new(["payrunName"]) },
            { typeof(PayrunEmployeeAvailableScriptAttribute), new(["payrunName"]) },
            { typeof(PayrunEmployeeStartScriptAttribute), new(["payrunName"]) },
            { typeof(PayrunWageTypeAvailableScriptAttribute), new(["payrunName"]) },
            { typeof(PayrunEmployeeEndScriptAttribute), new(["payrunName"]) },
            { typeof(PayrunEndScriptAttribute), new(["payrunName"]) },
            // report
            { typeof(ReportBuildScriptAttribute), new(["reportName"]) },
            { typeof(ReportStartScriptAttribute), new(["reportName"]) },
            { typeof(ReportEndScriptAttribute), new(["reportName"]) }
        };

        internal static ScriptAttribute CreateScriptAttribute(string attributeName, IDictionary<string, string> parameters)
        {
            // function attribute type
            var type = ScriptAttributes.Keys.FirstOrDefault(x => string.Equals(x.Name, attributeName));
            if (type == null)
            {
                throw new NotSupportedException($"Unsupported script attribute {attributeName}");
            }

            // constructor parameters from attribute parameters
            var attributeInfo = ScriptAttributes[type];
            var typeParameters = new List<object>();
            foreach (var parameterName in attributeInfo.Parameters)
            {
                if (!parameters.TryGetValue(parameterName, out var parameter))
                {
                    throw new NotSupportedException($"Missing parameter {parameterName} in script function {attributeName}");
                }
                typeParameters.Add(parameter);
            }

            // create type
            var scriptAttribute = Activator.CreateInstance(type, typeParameters.ToArray()) as ScriptAttribute;
            return scriptAttribute;
        }
    }

    #endregion

}