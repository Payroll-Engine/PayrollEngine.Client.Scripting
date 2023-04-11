
namespace PayrollEngine.Client.Scripting.Runtime;

/// <summary>Runtime for the case validate function <see cref="Function.CaseFunction"/></summary>
public interface ICaseValidateRuntime : ICaseChangeRuntime
{
    /// <summary>Get case validate actions</summary>
    string[] GetValidateActions();

    /// <summary>Get case field validate actions</summary>
    /// <param name="caseFieldName">The name of the case field</param>
    string[] GetFieldValidateActions(string caseFieldName);

    /// <summary>Test for issues</summary>
    bool HasIssues();

    /// <summary>Add a new case issue</summary>
    /// <param name="message">The issue message</param>
    void AddIssue(string message);

    /// <summary>Add a new case field issue</summary>
    /// <param name="caseFieldName">Name of the case field</param>
    /// <param name="message">The issue message</param>
    void AddIssue(string caseFieldName, string message);
}