
namespace PayrollEngine.Client.Scripting.Runtime;

/// <summary>Runtime for the case build function  <see cref="Function.CaseBuildFunction"/></summary>
public interface ICaseBuildRuntime : ICaseChangeRuntime
{
    /// <summary>Get case build actions</summary>
    string[] GetBuildActions();

    /// <summary>Get case field build actions</summary>
    /// <param name="caseFieldName">The name of the case field</param>
    string[] GetFieldBuildActions(string caseFieldName);
}