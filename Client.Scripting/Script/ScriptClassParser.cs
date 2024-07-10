using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PayrollEngine.Client.Scripting.Script;

/// <summary>Script class parser</summary>
public class ScriptClassParser
{
    private static readonly string[] FunctionAttributeNames =
    [
        nameof(CaseAvailableFunctionAttribute),
        nameof(CaseBuildFunctionAttribute),
        nameof(CaseValidateFunctionAttribute),

        nameof(CaseRelationBuildFunctionAttribute),
        nameof(CaseRelationValidateFunctionAttribute),

        nameof(CollectorStartFunctionAttribute),
        nameof(CollectorApplyFunctionAttribute),
        nameof(CollectorEndFunctionAttribute),

        nameof(WageTypeValueFunctionAttribute),
        nameof(WageTypeResultFunctionAttribute),

        nameof(PayrunStartFunctionAttribute),
        nameof(PayrunEmployeeAvailableFunctionAttribute),
        nameof(PayrunWageTypeAvailableFunctionAttribute),
        nameof(PayrunEndFunctionAttribute),

        nameof(ReportBuildFunctionAttribute),
        nameof(ReportStartFunctionAttribute),
        nameof(ReportEndFunctionAttribute)
    ];

    /// <summary>The syntax tree</summary>
    public ClassDeclarationSyntax ClassSyntax { get; }

    /// <summary>The attribute syntax</summary>
    public AttributeSyntax AttributeSyntax { get; }

    /// <summary>the function attribute</summary>
    public FunctionAttribute FunctionAttribute { get; }

    /// <summary>The class name</summary>
    public string ClassName => ClassSyntax.Identifier.ValueText;

    /// <summary>Initializes a new instance of the <see cref="ScriptMethodParser"/> class</summary>
    /// <param name="classSyntaxTree">The function class code syntax tree</param>
    public ScriptClassParser(ClassDeclarationSyntax classSyntaxTree)
    {
        ClassSyntax = classSyntaxTree ?? throw new ArgumentNullException(nameof(classSyntaxTree));

        var attributeParameters = new Dictionary<string, string>();
        foreach (var attributeList in ClassSyntax.AttributeLists)
        {
            foreach (var attribute in attributeList.Attributes)
            {
                var attributeName = attribute.Name.ToString().EnsureEnd("Attribute");
                if (FunctionAttributeNames.Contains(attributeName))
                {
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

                    // tenant and user
                    var tenantIdentifier = attributeParameters["tenantIdentifier"];
                    if (string.IsNullOrWhiteSpace(tenantIdentifier))
                    {
                        throw new PayrollException($"Missing parameter 'tenantIdentifier' in script function {ClassName}");
                    }
                    var userIdentifier = attributeParameters["userIdentifier"];
                    if (string.IsNullOrWhiteSpace(userIdentifier))
                    {
                        throw new PayrollException($"Missing parameter 'userIdentifier' in script function {ClassName}");
                    }

                    AttributeSyntax = attribute;
                    FunctionAttribute = FunctionAttributeFactory.CreateFunctionAttribute(
                        tenantIdentifier, userIdentifier, attributeName, attributeParameters);
                }
            }
        }
    }

    /// <summary>Parses the source file</summary>
    /// <param name="sourceCode">Name of the file</param>
    /// <returns>The script classes</returns>
    public static IList<ScriptClass> FromSource(string sourceCode)
    {
        if (string.IsNullOrWhiteSpace(sourceCode))
        {
            throw new ArgumentException(nameof(sourceCode));
        }

        var syntaxTree = CSharpSyntaxTree.ParseText(sourceCode);

        var classes = new List<ScriptClass>();
        var classSyntaxTree = syntaxTree.GetRoot()
            .DescendantNodes()
            .OfType<ClassDeclarationSyntax>()
            .ToList();
        foreach (var classNode in classSyntaxTree)
        {
            var classParser = new ScriptClassParser(classNode);
            var methodParser = new ScriptMethodParser(classNode);
            if (classParser.AttributeSyntax != null && methodParser.MethodAttributes.Any())
            {
                classes.Add(new()
                {
                    AttributeSyntax = classParser.AttributeSyntax,
                    FunctionAttribute = classParser.FunctionAttribute,
                    Methods = methodParser.MethodAttributes
                });
            }
        }
        return classes;
    }

    #region Factory

    private static class FunctionAttributeFactory
    {
        // type registration
        // the dictionary value represents the script parameters required by the function attribute ctor
        private static readonly Dictionary<Type, string[]> FunctionAttributes = new()
        {
            // case
            { typeof(CaseAvailableFunctionAttribute), ["employeeIdentifier", "payrollName", "regulationName"] },
            { typeof(CaseBuildFunctionAttribute), ["employeeIdentifier", "payrollName", "regulationName"] },
            { typeof(CaseValidateFunctionAttribute), ["employeeIdentifier", "payrollName", "regulationName"] },
            // case relation
            { typeof(CaseRelationBuildFunctionAttribute), ["employeeIdentifier", "payrollName", "regulationName"] },
            { typeof(CaseRelationValidateFunctionAttribute), ["employeeIdentifier", "payrollName", "regulationName"] },
            // collector
            { typeof(CollectorStartFunctionAttribute), ["employeeIdentifier", "payrollName", "regulationName"] },
            { typeof(CollectorApplyFunctionAttribute), ["employeeIdentifier", "payrollName", "regulationName"] },
            { typeof(CollectorEndFunctionAttribute), ["employeeIdentifier", "payrollName", "regulationName"] },
            // wage type
            { typeof(WageTypeValueFunctionAttribute), ["employeeIdentifier", "payrollName", "regulationName"] },
            { typeof(WageTypeResultFunctionAttribute), ["employeeIdentifier", "payrollName", "regulationName"] },
            // report
            { typeof(ReportBuildFunctionAttribute), ["regulationName"] },
            { typeof(ReportStartFunctionAttribute), ["regulationName"] },
            { typeof(ReportEndFunctionAttribute), ["regulationName"] },
            // payrun
            { typeof(PayrunStartFunctionAttribute), ["employeeIdentifier", "payrollName", "payrunName"] },
            { typeof(PayrunEmployeeAvailableFunctionAttribute), ["employeeIdentifier", "payrollName", "payrunName"] },
            { typeof(PayrunEmployeeStartFunctionAttribute), ["employeeIdentifier", "payrollName", "payrunName"] },
            { typeof(PayrunWageTypeAvailableFunctionAttribute), ["employeeIdentifier", "payrollName", "payrunName"] },
            { typeof(PayrunEmployeeEndFunctionAttribute), ["employeeIdentifier", "payrollName", "payrunName"] },
            { typeof(PayrunEndFunctionAttribute), ["employeeIdentifier", "payrollName", "payrunName"] }
        };

        internal static FunctionAttribute CreateFunctionAttribute(string tenantIdentifier, string userIdentifier,
            string attributeName, Dictionary<string, string> parameters)
        {
            // function attribute type
            var type = FunctionAttributes.Keys.FirstOrDefault(x => string.Equals(x.Name, attributeName));
            if (type == null)
            {
                throw new NotSupportedException($"Unsupported script function attribute {attributeName}");
            }

            // type parameters
            var typeParameters = new List<object> {
                // first ctor parameter
                tenantIdentifier,
                // second ctor parameter
                userIdentifier
            };

            // following constructor parameters from attribute parameters
            var parameterNames = FunctionAttributes[type];
            foreach (var parameterName in parameterNames)
            {
                if (!parameters.TryGetValue(parameterName, out var parameter))
                {
                    throw new NotSupportedException($"Missing parameter {parameterName} in script function {attributeName}");
                }
                typeParameters.Add(parameter);
            }

            // create type
            var functionAttribute = Activator.CreateInstance(type, typeParameters.ToArray()) as FunctionAttribute;
            return functionAttribute;
        }
    }

    #endregion

}