/* PayrunFunction.Action */

using System;
using System.Linq;

namespace PayrollEngine.Client.Scripting.Function;

/// <summary>Payrun function</summary>
public partial class PayrunFunction
{

    #region Wage Type and Collectors

    /// <summary>Get wage type year-to-date value by wage type number</summary>
    [ActionParameter("number", "The wage type number", [DecimalType])]
    [PayrunAction("GetCycleWageTypeValue", "Get wage type year-to-date value by wage type number", "WageType")]
    public ActionValue GetCycleWageTypeValue(decimal number)
    {
        var results = GetConsolidatedWageTypeResults(new(number, CycleStart));
        return results.Any() ? results.Sum(x => x.Value) : 0;
    }

    /// <summary>Get wage type year-to-date value by wage type name</summary>
    [ActionParameter("name", "The wage type name", [StringType])]
    [PayrunAction("GetCycleWageTypeValue", "Get wage type year-to-date value by wage type name", "WageType")]
    public ActionValue GetCycleWageTypeValue(string name) =>
        GetCycleWageTypeValue(GetWageTypeNumber(name));

    /// <summary>Get sum of retro corrections for a wage type by number</summary>
    [ActionParameter("number", "The wage type number", [DecimalType])]
    [PayrunAction("GetRetroWageTypeValueSum", "Get sum of retro wage type corrections by number", "WageType")]
    public ActionValue GetRetroWageTypeValueSumByNumber(decimal number) =>
        GetWageTypeRetroResultSum(number);

    /// <summary>Get sum of retro corrections for a wage type by name</summary>
    [ActionParameter("name", "The wage type name", [StringType])]
    [PayrunAction("GetRetroWageTypeValueSum", "Get sum of retro wage type corrections by name", "WageType")]
    public ActionValue GetRetroWageTypeValueSumByName(string name) =>
        GetWageTypeRetroResultSum(GetWageTypeNumber(name));

    /// <summary>Get collector year-to-date value</summary>
    /// <param name="name">Collector name</param>
    [ActionParameter("name", "The collector name", [StringType])]
    [PayrunAction("GetCycleCollectorValue", "Get collector year-to-date value", "Collector")]
    public ActionValue GetCycleCollectorValue(string name)
    {
        var results = GetConsolidatedCollectorResults(new([name], CycleStart));
        return results.Any() ? results.Sum(x => x.Value) : 0;
    }

    #endregion

    #region Runtime Values

    /// <summary>Get runtime action value</summary>
    [ActionParameter("key", "The value key", [StringType])]
    [PayrunAction("GetRuntimeValue", "Get payrun runtime value", "Runtime")]
    public ActionValue GetRuntimeValue(string key) =>
        GetEmployeeRuntimeValue(key);

    /// <summary>Set runtime action value</summary>
    [ActionParameter("key", "The value key", [StringType])]
    [ActionParameter("value", "The value to set")]
    [PayrunAction("SetRuntimeValue", "Set payrun runtime value", "Runtime")]
    public void SetRuntimeValue(string key, object value) =>
        SetEmployeeRuntimeValue(key, ActionValue.From(value)?.AsString);

    /// <summary>Remove runtime action value</summary>
    [ActionParameter("key", "The value key", [StringType])]
    [PayrunAction("RemoveRuntimeValue", "Remove payrun runtime value", "Runtime")]
    public void RemoveRuntimeValue(string key) =>
        SetRuntimeValue(key, ActionValue.Null);

    #endregion

    #region Payrun Result

    /// <summary>Get payrun result value</summary>
    [ActionParameter("name", "The result name", [StringType])]
    [PayrunAction("GetPayrunResultValue", "Get payrun result value", "Payrun")]
    public ActionValue GetPayrunResultValue(string name) =>
        new(GetPayrunResult(name));

    /// <summary>Set payrun result value</summary>
    [ActionParameter("name", "The result name", [StringType])]
    [ActionParameter("value", "The value to set")]
    [ActionParameter("type", "The value type (default: Money)", [StringType])]
    [PayrunAction("SetPayrunResultValue", "Set payrun result value", "Payrun")]
    public void SetPayrunResultValue(string name, object value, string type = null)
    {
        var actionValue = ActionValue.From(value);
        if (actionValue == null || actionValue.IsNull)
        {
            return;
        }

        ValueType valueType = ValueType.Money;
        if (!string.IsNullOrWhiteSpace(type) && !Enum.TryParse(type, out valueType))
        {
            valueType = ValueType.Money;
        }
        SetPayrunResult(name, actionValue.Value, valueType);
    }

    #endregion

}
