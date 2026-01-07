/* WageTypeFunction */

using System;
using System.Linq;
using System.Collections.Generic;

namespace PayrollEngine.Client.Scripting.Function;

/// <summary>Base class for wage type functions</summary>
// ReSharper disable once PartialTypeWithSinglePart
public abstract partial class WageTypeFunction : PayrunFunction
{
    /// <summary>Initializes a new instance with the function runtime</summary>
    /// <param name="runtime">The runtime</param>
    protected WageTypeFunction(object runtime) :
        base(runtime)
    {
        // wage type
        WageTypeNumber = Runtime.WageTypeNumber;
        WageTypeName = Runtime.WageTypeName;
        WageTypeDescription = Runtime.WageTypeDescription;
        WageTypeCalendar = Runtime.WageTypeCalendar;

        // lookups
        Attribute = new(name => new PayrollValue(GetResultAttribute(name)), SetResultAttribute);
        AttributePayrollValue = new(name => new(GetResultAttribute(name)),
            (name, value) => SetResultAttribute(name, value.Value));
        WageType = new(GetWageType);
        Collector = new(GetCollector);
    }

    /// <summary>New function instance without runtime (scripting development)</summary>
    /// <param name="sourceFileName">The name of the source file</param>
    protected WageTypeFunction(string sourceFileName) :
        base(sourceFileName)
    {
    }

    /// <summary>Get or set a wage type result attribute value</summary>
    public ScriptDictionary<string, object> Attribute { get; }

    /// <summary>Get or set a wage type result attribute <see cref="PayrollValue"/></summary>
    public ScriptDictionary<string, PayrollValue> AttributePayrollValue { get; }

    /// <summary>Get a wage type value by wage type number</summary>
    public ScriptDictionary<decimal, decimal> WageType { get; }

    /// <summary>Get a collector value by collector name</summary>
    public ScriptDictionary<string, decimal> Collector { get; }

    /// <summary>The wage type number</summary>
    [ActionProperty("Wage type number")]
    public decimal WageTypeNumber { get; }

    /// <summary>The wage type name</summary>
    [ActionProperty("Wage type name")]
    public string WageTypeName { get; }

    /// <summary>The wage type description</summary>
    [ActionProperty("Wage type description")]
    public string WageTypeDescription { get; }

    /// <summary>The wage type calendar</summary>
    public string WageTypeCalendar { get; }

    /// <summary>Gets the wage type value</summary>
    /// <param name="wageTypeNumber">The wage type number</param>
    /// <returns>The wage type value</returns>
    public decimal GetWageType(decimal wageTypeNumber) => 
        Runtime.GetWageType(wageTypeNumber);

    /// <summary>Gets the wage type value</summary>
    /// <param name="wageTypeName">The wage type name</param>
    /// <returns>The wage type value</returns>
    public decimal GetWageType(string wageTypeName) => 
        Runtime.GetWageType(wageTypeName);

    /// <summary>The wage type collectors</summary>
    public string[] Collectors => Runtime.Collectors;

    /// <summary>The wage type collector groups</summary>
    public string[] CollectorGroups => Runtime.CollectorGroups;

    /// <summary>Gets the collector value</summary>
    /// <param name="collectorName">Name of the collector</param>
    /// <returns>The collector value</returns>
    public decimal GetCollector(string collectorName) => 
        Runtime.GetCollector(collectorName);

    /// <summary>Reenable disabled collector for the current employee job</summary>
    /// <param name="collectorName">Name of the collector</param>
    public void EnableCollector(string collectorName) =>
        Runtime.EnableCollector(collectorName);

    /// <summary>Disable collector for the current employee job</summary>
    /// <param name="collectorName">Name of the collector</param>
    public void DisableCollector(string collectorName) =>
        Runtime.DisableCollector(collectorName);

    /// <summary>Get the wage type result tags</summary>
    /// <returns>The wage type result tags</returns>
    public List<string> GetResultTags() => Runtime.GetResultTags();

    /// <summary>Set the collector result tags</summary>
    /// <param name="tags">The result tags</param>
    public void SetResultTags(IEnumerable<string> tags) =>
        Runtime.SetResultTags(tags.ToList());

    /// <summary>Get wage result attribute</summary>
    /// <param name="name">The attribute name</param>
    public object GetResultAttribute(string name) =>
        Runtime.GetResultAttribute(name);

    /// <summary>Get wage result attribute typed value</summary>
    public T GetResultAttribute<T>(string attributeName, T defaultValue = default) =>
        ChangeValueType(GetResultAttribute(attributeName), defaultValue);

    /// <summary>Sets the wage result attribute value</summary>
    /// <param name="name">The attribute name</param>
    /// <param name="value">The attribute value</param>
    public void SetResultAttribute(string name, object value) =>
        Runtime.SetResultAttribute(name, value);

    /// <summary>Get attribute value</summary>
    public object GetWageTypeAttribute(string attributeName) =>
        Runtime.GetWageTypeAttribute(attributeName);

    /// <summary>Get attribute typed value</summary>
    public T GetWageTypeAttribute<T>(string attributeName, T defaultValue = default) =>
        ChangeValueType(Runtime.GetWageTypeAttribute(attributeName), defaultValue);

    /// <summary>Get consolidated and current period employee collector results by query</summary>
    /// <param name="query">The result query</param>
    /// <returns>Consolidated employee collector results</returns>
    public decimal GetCollectorCurrentConsolidatedValue(CollectorConsolidatedResultQuery query)
    {
        var value = GetConsolidatedCollectorResults(query).Sum();
        foreach (var collector in query.Collectors)
        {
            value += Collector[collector];
        }
        return value;
    }

    /// <summary>Get consolidated and current period employee wage type results by query</summary>
    /// <param name="query">The result query</param>
    /// <returns>Consolidated employee collector results</returns>
    public decimal GetWageTypeCurrentConsolidatedValue(WageTypeConsolidatedResultQuery query)
    {
        var value = GetConsolidatedWageTypeResults(query).Sum();
        foreach (var wageType in query.WageTypes)
        {
            value += WageType[wageType];

        }
        return value;
    }

    #region Custom Results

    /// <summary>Add wage type custom result from case field values, using the current period</summary>
    /// <param name="source">The value source</param>
    /// <param name="tags">The result tags</param>
    /// <param name="attributes">The wage type custom result attributes</param>
    /// <param name="valueType">The result value type (numeric), default is the wage type value type</param>
    /// <param name="culture">The result culture</param>
    public void AddCustomResult(string source, IEnumerable<string> tags = null,
        Dictionary<string, object> attributes = null, ValueType? valueType = null, string culture = null) =>
        AddCustomResult(source, PeriodStart, PeriodEnd, tags, attributes, valueType, culture);

    /// <summary>Add wage type custom result from case field values, using the current period</summary>
    /// <param name="source">The value source</param>
    /// <param name="value">The period value</param>
    /// <param name="tags">The result tags</param>
    /// <param name="attributes">The wage type custom result attributes</param>
    /// <param name="valueType">The result value type (numeric), default is the wage type value type</param>
    /// <param name="culture">The result culture</param>
    public void AddCustomResult(string source, decimal value, IEnumerable<string> tags = null,
        Dictionary<string, object> attributes = null, ValueType? valueType = null, string culture = null) =>
        AddCustomResult(source, value, PeriodStart, PeriodEnd, tags, attributes, valueType, culture);

    /// <summary>Add wage type custom result from case field values</summary>
    /// <param name="source">The value source</param>
    /// <param name="startDate">The moment within the start period</param>
    /// <param name="endDate">The moment within the end period</param>
    /// <param name="tags">The result tags</param>
    /// <param name="attributes">The wage type custom result attributes</param>
    /// <param name="valueType">The result value type (numeric), default is the wage type value type</param>
    /// <param name="culture">The result culture</param>
    public void AddCustomResult(string source, DateTime startDate, DateTime endDate,
        IEnumerable<string> tags = null, Dictionary<string, object> attributes = null,
        ValueType? valueType = null, string culture = null)
    {
        var tagList = tags?.ToList();
        var caseValues = GetPeriodCaseValues(new DatePeriod(startDate, endDate), source);
        foreach (var caseValue in caseValues)
        {
            foreach (var periodValue in caseValue.Value.PeriodValues)
            {
                if (periodValue.Value is decimal decimalValue)
                {
                    var period = new DatePeriod(periodValue.Start, periodValue.End);
                    AddCustomResult(source, decimalValue, period.Start, period.End, tagList, attributes, valueType, culture);
                }
            }
        }
    }

    /// <summary>Add a wage type custom result</summary>
    /// <param name="source">The value source</param>
    /// <param name="value">The period value</param>
    /// <param name="startDate">The start date</param>
    /// <param name="endDate">The end date</param>
    /// <param name="tags">The result tags</param>
    /// <param name="attributes">The wage type custom result attributes</param>
    /// <param name="valueType">The result value type (numeric), default is the wage type value type</param>
    /// <param name="culture">The result culture</param>
    public void AddCustomResult(string source, decimal value, DateTime startDate,
        DateTime endDate, IEnumerable<string> tags = null, Dictionary<string, object> attributes = null,
        ValueType? valueType = null, string culture = null) =>
        Runtime.AddCustomResult(source, value, startDate, endDate, tags?.ToList(), attributes, (int?)valueType, culture);

    #endregion

    #region Retro

    /// <summary>Schedule a retro payrun</summary>
    /// <param name="scheduleDate">The payrun schedule date, must be before the current period</param>
    /// <param name="resultTags">The result tags</param>
    public void ScheduleRetroPayrun(DateTime scheduleDate, IEnumerable<string> resultTags = null) =>
        Runtime.ScheduleRetroPayrun(scheduleDate, resultTags?.ToList());

    #endregion

}