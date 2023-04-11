using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PayrollEngine.Client.Scripting.Script;

/// <summary>Scripting class</summary>
public class ScriptClass
{
    /// <summary>The attribute syntax</summary>
    public AttributeSyntax AttributeSyntax { get; set; }

    /// <summary>The function attribute</summary>
    public FunctionAttribute FunctionAttribute { get; set; }

    /// <summary>The methods with attributes</summary>
    public Dictionary<MethodDeclarationSyntax, ScriptAttribute> Methods { get; set; }
}