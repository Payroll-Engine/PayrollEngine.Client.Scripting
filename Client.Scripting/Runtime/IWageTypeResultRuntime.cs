
namespace PayrollEngine.Client.Scripting.Runtime;

/// <summary>Runtime for the wage type result function <see cref="Function.WageTypeResultFunction"/></summary>
public interface IWageTypeResultRuntime : IWageTypeRuntime
{
    /// <summary>The wage type value</summary>
    decimal WageTypeValue { get; }
}