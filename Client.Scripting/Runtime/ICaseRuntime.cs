
namespace PayrollEngine.Client.Scripting.Runtime;

/// <summary>Runtime for the case function <see cref="Function.CaseFunction"/></summary>
public interface ICaseRuntime : IPayrollRuntime
{
    /// <summary>The case name</summary>
    string CaseName { get; }

    /// <summary>The case <see cref="CaseType"/></summary>
    int CaseType { get; }

    /// <summary>Get case attribute value</summary>
    /// <param name="attributeName">Name of the attribute</param>
    /// <returns>The case attribute value</returns>
    object GetCaseAttribute(string attributeName);

    /// <summary>Invoke case webhook and receive the response JSON data</summary>
    /// <param name="requestOperation">The request operation</param>
    /// <param name="requestMessage">The JSON request message</param>
    /// <returns>The webhook response object as JSON</returns>
    string InvokeWebhook(string requestOperation, string requestMessage = null);
}