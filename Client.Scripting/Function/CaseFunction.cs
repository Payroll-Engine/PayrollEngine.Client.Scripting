/* CaseFunction */

using System;
using System.Text.Json;

namespace PayrollEngine.Client.Scripting.Function;

/// <summary>Base class for case functions</summary>
// ReSharper disable once PartialTypeWithSinglePart
public abstract partial class CaseFunction : PayrollFunction
{
    /// <summary>Initializes a new instance with the function runtime</summary>
    /// <param name="runtime">The runtime</param>
    protected CaseFunction(object runtime) :
        base(runtime)
    {
        // case
        CaseName = Runtime.CaseName;
        CaseType = (CaseType)Runtime.CaseType;
    }

    /// <summary>New function instance without runtime (scripting development)</summary>
    /// <remarks>Use <see cref="Function.GetSourceFileName"/> in your constructor for the source file name</remarks>
    /// <param name="sourceFileName">The name of the source file</param>
    protected CaseFunction(string sourceFileName) :
        base(sourceFileName)
    {
    }

    /// <summary>The case name</summary>
    public string CaseName { get; }

    /// <summary>The case type</summary>
    public CaseType CaseType { get; }

    /// <summary>Get case attribute value</summary>
    public object GetCaseAttribute(string attributeName) =>
        Runtime.GetCaseAttribute(attributeName);

    /// <summary>Get case attribute typed value</summary>
    public T GetCaseAttribute<T>(string attributeName, T defaultValue = default)
    {
        var value = Runtime.GetCaseAttribute(attributeName);
        return value == null ? defaultValue : (T)Convert.ChangeType(value, typeof(T));
    }
}