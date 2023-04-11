using System;
using System.Collections.Generic;

namespace PayrollEngine.Client.Scripting.Runtime;

/// <summary>Runtime for the collector function <see cref="Function.CollectorFunction"/></summary>
public interface ICollectorRuntime : IPayrunRuntime
{
    /// <summary>The collector name</summary>
    string CollectorName { get; }

    /// <summary>The collector groups</summary>
    string[] CollectorGroups { get; }

    /// <summary>The collect type</summary>
    string CollectType { get; }

    /// <summary>The threshold value</summary>
    decimal? CollectorThreshold { get; }

    /// <summary>The minimum allowed value</summary>
    decimal? CollectorMinResult { get; }

    /// <summary>The maximum allowed value</summary>
    decimal? CollectorMaxResult { get; }

    /// <summary>The current collector result</summary>
    decimal CollectorResult { get; }

    /// <summary>Collected values count</summary>
    decimal CollectorCount { get; }

    /// <summary>The summary of the collected value</summary>
    decimal CollectorSum { get; }

    /// <summary>The minimum collected value</summary>
    decimal CollectorMin { get; }

    /// <summary>The maximum collected value</summary>
    decimal CollectorMax { get; }

    /// <summary>The average of the collected value</summary>
    decimal CollectorAverage { get; }

    /// <summary>Get the wage type result tags</summary>
    /// <returns>The wage type result tags</returns>
    List<string> GetResultTags();

    /// <summary>Set the wage type result tags</summary>
    /// <param name="tags">The wage type result tags</param>
    void SetResultTags(List<string> tags);

    /// <summary>Get attribute value</summary>
    /// <param name="attributeName">Name of the attribute</param>
    /// <returns>The collector attribute value</returns>
    object GetCollectorAttribute(string attributeName);

    /// <summary>Gets the collector value</summary>
    /// <param name="collectorName">Name of the collector</param>
    /// <returns>The collector value</returns>
    decimal GetCollectorValue(string collectorName);

    /// <summary>Resets the collector to his initial state</summary>
    void Reset();

    #region Result Attributes

    /// <summary>Gets the result attribute value</summary>
    /// <param name="name">The attribute name</param>
    /// <returns>The attribute value</returns>
    object GetResultAttribute(string name);

    /// <summary>Sets the result attribute value</summary>
    /// <param name="name">The attribute name</param>
    /// <param name="value">The attribute value</param>
    void SetResultAttribute(string name, object value);

    #endregion

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

    /// <summary>Add a custom collector result</summary>
    /// <param name="source">The value source</param>
    /// <param name="value">The period value</param>
    /// <param name="startDate">The start date</param>
    /// <param name="endDate">The end date</param>
    /// <param name="tags">The result tags</param>
    /// <param name="attributes">The collector result attributes</param>
    /// <param name="valueType">The result value type (numeric), default is the collector value type</param>
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