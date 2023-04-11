using System;
using System.Collections.Generic;

namespace PayrollEngine.Client.Scripting.Runtime;

/// <summary>Runtime for the wage type function <see cref="Function.WageTypeFunction"/></summary>
public interface IWageTypeRuntime : IPayrunRuntime
{
    /// <summary>The wage type number</summary>
    decimal WageTypeNumber { get; }

    /// <summary>The wage type name</summary>
    string WageTypeName { get; }

    /// <summary>The wage type description</summary>
    string WageTypeDescription { get; }

    /// <summary>The wage type collectors</summary>
    string[] Collectors { get; }

    /// <summary>The wage type collector groups</summary>
    string[] CollectorGroups { get; }

    /// <summary>Get the wage type result tags</summary>
    /// <returns>The wage type result tags</returns>
    List<string> GetResultTags();

    /// <summary>Set the wage type result tags</summary>
    /// <param name="tags">The wage type result tags</param>
    void SetResultTags(List<string> tags);

    /// <summary>Gets wage result attribute value</summary>
    /// <param name="name">The attribute name</param>
    /// <returns>The attribute value</returns>
    object GetResultAttribute(string name);

    /// <summary>Sets the wage result attribute value</summary>
    /// <param name="name">The attribute name</param>
    /// <param name="value">The attribute value</param>
    void SetResultAttribute(string name, object value);

    /// <summary>Get wage type attribute value</summary>
    /// <param name="attributeName">Name of the attribute</param>
    /// <returns>The wage type attribute value</returns>
    object GetWageTypeAttribute(string attributeName);

    /// <summary>Gets the wage type value</summary>
    /// <param name="wageTypeNumber">The wage type number</param>
    /// <returns>The wage type value</returns>
    decimal GetWageTypeValue(decimal wageTypeNumber);

    /// <summary>Gets the collector value</summary>
    /// <param name="collectorName">Name of the collector</param>
    /// <returns>The collector value</returns>
    decimal GetCollectorValue(string collectorName);

    /// <summary>Reenable disabled collector for the current employee job</summary>
    /// <param name="collectorName">Name of the collector</param>
    void EnableCollector(string collectorName);

    /// <summary>Disable collector for the current employee job</summary>
    /// <param name="collectorName">Name of the collector</param>
    void DisableCollector(string collectorName);

    #region Results

    /// <summary>Add a payrun result</summary>
    /// <param name="source">The result source</param>
    /// <param name="name">The result name</param>
    /// <param name="value">The result value</param>
    /// <param name="valueType">The result value type</param>
    /// <param name="startDate">The start date</param>
    /// <param name="endDate">The end date</param>
    /// <param name="slot">The result slot</param>
    /// <param name="tags">The result tags</param>
    /// <param name="attributes">The wage type custom result attributes</param>
    void AddPayrunResult(string source, string name, string value, int valueType,
        DateTime startDate, DateTime endDate, string slot, List<string> tags, Dictionary<string, object> attributes);

    /// <summary>Add a wage type custom result</summary>
    /// <param name="source">The value source</param>
    /// <param name="value">The period value</param>
    /// <param name="startDate">The start date</param>
    /// <param name="endDate">The end date</param>
    /// <param name="tags">The result tags</param>
    /// <param name="attributes">The wage type custom result attributes</param>
    /// <param name="valueType">The result value type (numeric), default is the wage type value type</param>
    void AddCustomResult(string source, decimal value, DateTime startDate, DateTime endDate,
        List<string> tags, Dictionary<string, object> attributes, int? valueType);

    #endregion

    #region Retro

    /// <summary>Schedule a retro payrun</summary>
    /// <param name="scheduleDate">The payrun schedule date, must be before the current period</param>
    /// <param name="resultTags">The result tags</param>
    void ScheduleRetroPayrun(DateTime scheduleDate, List<string> resultTags);

    #endregion

}