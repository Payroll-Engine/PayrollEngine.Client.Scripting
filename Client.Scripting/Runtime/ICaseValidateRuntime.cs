
namespace PayrollEngine.Client.Scripting.Runtime;

/// <summary>Runtime for the case validate function <see cref="Function.CaseFunction"/></summary>
public interface ICaseValidateRuntime : ICaseChangeRuntime
{
    /// <summary>Test for issues</summary>
    bool HasIssues();

    /// <summary>Add a new case issue</summary>
    /// <param name="message">The issue message</param>
    void AddCaseIssue(string message);

    /// <summary>Add a new case field issue</summary>
    /// <param name="caseFieldName">Name of the case field</param>
    /// <param name="message">The issue message</param>
    void AddCaseFieldIssue(string caseFieldName, string message);
}