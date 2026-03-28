
namespace PayrollEngine.Client.Scripting.Runtime;

/// <summary>Runtime for the wage type value function <see cref="Function.WageTypeValueFunction"/></summary>
public interface IWageTypeValueRuntime : IWageTypeRuntime
{
    /// <summary>The execution count</summary>
    int ExecutionCount { get; }

    /// <summary>Restart execution of wage type calculation</summary>
    void RestartExecution();

    /// <summary>
    /// Aborts the wage type sequence for the current employee immediately.
    /// All subsequent wage types are skipped. CollectorEnd and PayrunEmployeeEnd
    /// continue normally.
    /// </summary>
    /// <param name="reason">Optional reason written to the payrun log.</param>
    void AbortExecution(string reason = null);

    /// <summary>Get wage type value actions</summary>
    string[] GetValueActions();
}
