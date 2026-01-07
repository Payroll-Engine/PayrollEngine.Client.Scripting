
namespace PayrollEngine.Client.Scripting.Runtime;

/// <summary>Runtime for the collector apply function <see cref="Function.CollectorApplyFunction"/></summary>
public interface ICollectorApplyRuntime : ICollectorRuntime
{
    /// <summary>The wage type number</summary>
    decimal WageTypeNumber { get; }

    /// <summary>The wage type name</summary>
    string WageTypeName { get; }

    /// <summary>The wage type result value</summary>
    decimal WageTypeValue { get; }

    /// <summary>Get collector apply actions</summary>
    string[] GetApplyActions();
}