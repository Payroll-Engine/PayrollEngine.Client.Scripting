
namespace PayrollEngine.Client.Scripting.Runtime;

/// <summary>Runtime for the payrun wage type available function <see cref="Function.PayrunWageTypeAvailableFunction"/></summary>
public interface IPayrunWageTypeAvailableRuntime : IPayrunRuntime
{
    /// <summary>The wage type number</summary>
    decimal WageTypeNumber { get; }

    /// <summary>Get wage type attribute value</summary>
    /// <param name="attributeName">Name of the attribute</param>
    /// <returns>The wage type attribute value</returns>
    object GetWageTypeAttribute(string attributeName);
}