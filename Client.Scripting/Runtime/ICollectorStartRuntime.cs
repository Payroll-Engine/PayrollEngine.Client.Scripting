
namespace PayrollEngine.Client.Scripting.Runtime;

/// <summary>Runtime for the collector start function <see cref="Function.CollectorStartFunction"/></summary>
public interface ICollectorStartRuntime : ICollectorRuntime
{
    /// <summary>Get collector values</summary>
    decimal[] GetValues();

    /// <summary>Sets the collector values</summary>
    /// <param name="values">The values to set</param>
    void SetValues(decimal[] values);

    /// <summary>Get collector start actions</summary>
    string[] GetStartActions();
}