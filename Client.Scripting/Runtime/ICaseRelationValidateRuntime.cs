
namespace PayrollEngine.Client.Scripting.Runtime;

/// <summary>Runtime for the case relation validate function <see cref="Function.CaseRelationValidateFunction"/></summary>
public interface ICaseRelationValidateRuntime : ICaseRelationRuntime
{
    /// <summary>Get case relation validate actions</summary>
    string[] GetValidateActions();

    /// <summary>Test for issues</summary>
    bool HasIssues();

    /// <summary>Add a new case issue</summary>
    /// <param name="message">The issue message</param>
    /// <param name="number">The issue number</param>
    void AddIssue(string message, int number);

    /// <summary>Add a new case field issue</summary>
    /// <param name="caseFieldName">Name of the case field</param>
    /// <param name="message">The issue message</param>
    /// <param name="number">The issue number</param>
    void AddIssue(string caseFieldName, string message, int number);
}