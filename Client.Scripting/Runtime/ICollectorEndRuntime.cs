
namespace PayrollEngine.Client.Scripting.Runtime;

/// <summary>Runtime for the collector end function <see cref="Function.CollectorEndFunction"/></summary>
public interface ICollectorEndRuntime : ICollectorRuntime
{
    /// <summary>Get collector values</summary>
    decimal[] GetValues();

    /// <summary>Sets the collector values</summary>
    /// <param name="values">The values to set</param>
    void SetValues(decimal[] values);

    /// <summary>Get collector end actions</summary>
    string[] GetEndActions();
}