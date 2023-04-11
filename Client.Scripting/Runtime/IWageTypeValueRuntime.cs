
namespace PayrollEngine.Client.Scripting.Runtime;

/// <summary>Runtime for the wage type value function <see cref="Function.WageTypeValueFunction"/></summary>
public interface IWageTypeValueRuntime : IWageTypeRuntime
{
    /// <summary>The execution count</summary>
    int ExecutionCount { get; }

    /// <summary>Restart execution of wage type calculation</summary>
    void RestartExecution();
}