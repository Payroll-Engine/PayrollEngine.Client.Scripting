/* WageTypeFunction.Actions */

namespace PayrollEngine.Client.Scripting.Function;

/// <summary>Wage type function</summary>
public partial class WageTypeFunction
{

    #region Wage Type and Collectors

    /// <summary>Get wage type value by number</summary>
    /// <param name="number">Wage type number</param>
    [ActionParameter("number", "Get wage type value by number", [DecimalType])]
    [WageTypeAction("GetWageTypeValueByNumber", "Get wage type value", "WageType")]
    public ActionValue GetWageTypeValueByNumber(decimal number) =>
        GetWageType(number);

    /// <summary>Get wage type value by name</summary>
    /// <param name="name">Wage type number</param>
    [ActionParameter("name", "The wage type name", [StringType])]
    [WageTypeAction("GetWageTypeValueByName", "Get wage type value by name", "WageType")]
    public ActionValue GetWageTypeValueByName(string name) =>
        GetWageType(name);

    /// <summary>Get collector value</summary>
    /// <param name="name">Collector name</param>
    [ActionParameter("name", "The collector name", [StringType])]
    [WageTypeAction("GetCollectorValue", "Get collector value", "WageType")]
    public ActionValue GetCollectorValue(string name) =>
        GetCollector(name);

    #endregion

}