using System;
using System.Collections.Generic;

namespace PayrollEngine.Client.Scripting.Runtime;

/// <summary>Runtime for the payrun function <see cref="Function.PayrunFunction"/></summary>
public interface IPayrunRuntime : IPayrollRuntime
{
    #region Payrun

    /// <summary>The payrun id</summary>
    int PayrunId { get; }

    /// <summary>The payrun name</summary>
    string PayrunName { get; }

    #endregion

    #region PayrunJob

    /// <summary>The payrun execution phase: 0=setup, 1=reevaluation</summary>
    int ExecutionPhase { get; }

    /// <summary>The retro payrun period</summary>
    Tuple<DateTime, DateTime> RetroPeriod { get; }

    /// <summary>True for a forecast payrun</summary>
    string Forecast { get; }

    /// <summary>The cycle name</summary>
    string CycleName { get; }

    /// <summary>The period name</summary>
    string PeriodName { get; }

    /// <summary>Get payrun job attribute value</summary>
    /// <param name="attributeName">Name of the attribute</param>
    /// <returns>The payrun job attribute value</returns>
    object GetPayrunJobAttribute(string attributeName);

    /// <summary>Set payrun job attribute value</summary>
    /// <param name="attributeName">Name of the attribute</param>
    /// <param name="value">The attribute value</param>
    void SetPayrunJobAttribute(string attributeName, object value);

    /// <summary>Remove payrun job attribute</summary>
    /// <param name="attributeName">Name of the attribute</param>
    bool RemovePayrunJobAttribute(string attributeName);

    #endregion

    #region Runtime Values

    /// <summary>Test for existing payrun runtime value</summary>
    /// <param name="key">The value key</param>
    /// <returns>True if the runtime value exists</returns>
    bool HasPayrunRuntimeValue(string key);

    /// <summary>Get payrun runtime value</summary>
    /// <param name="key">The value key</param>
    /// <returns>The payrun runtime value</returns>
    string GetPayrunRuntimeValue(string key);

    /// <summary>Set payrun runtime value</summary>
    /// <param name="key">The value key</param>
    /// <param name="value">The payrun runtime value, use null to remove the runtime value</param>
    void SetPayrunRuntimeValue(string key, string value);

    /// <summary>Test for existing employee runtime value</summary>
    /// <param name="key">The value key</param>
    /// <returns>True if the runtime value exists</returns>
    bool HasEmployeeRuntimeValue(string key);

    /// <summary>Get employee runtime value</summary>
    /// <param name="key">The value key</param>
    /// <returns>The employee runtime value</returns>
    string GetEmployeeRuntimeValue(string key);

    /// <summary>Set employee runtime value</summary>
    /// <param name="key">The value key</param>
    /// <param name="value">The employee runtime value, use null to remove the runtime value</param>
    void SetEmployeeRuntimeValue(string key, string value);

    #endregion

    #region Wage Type Results

    /// <summary>Gets the wage type range results</summary>
    /// <param name="wageTypeNumbers">The wage type numbers</param>
    /// <param name="start">The range period start</param>
    /// <param name="end">The range period end</param>
    /// <param name="forecast">The forecast</param>
    /// <param name="jobStatus">The job status</param>
    /// <param name="tags">The result tags</param>
    /// <returns>The wage type date range results</returns>
    IList<Tuple<decimal, string, Tuple<DateTime, DateTime>, decimal, List<string>, Dictionary<string, object>>> GetWageTypeResults(
        IList<decimal> wageTypeNumbers, DateTime start, DateTime end,
        string forecast = null, int? jobStatus = null, IList<string> tags = null);

    /// <summary>Gets the consolidated wage type results, from the period moment until the current period</summary>
    /// <param name="wageTypeNumbers">The wage type numbers</param>
    /// <param name="periodMoment">The period moment</param>
    /// <param name="forecast">The forecast</param>
    /// <param name="jobStatus">The job status</param>
    /// <param name="tags">The result tags</param>
    /// <returns>The consolidate wage type results</returns>
    IList<Tuple<decimal, string, Tuple<DateTime, DateTime>, decimal, List<string>, Dictionary<string, object>>> GetConsolidatedWageTypeResults(
        IList<decimal> wageTypeNumbers, DateTime periodMoment,
        string forecast = null, int? jobStatus = null, IList<string> tags = null);

    /// <summary>Gets the wage type custom results from a time range</summary>
    /// <param name="wageTypeNumbers">The wage type numbers</param>
    /// <param name="start">The range period start</param>
    /// <param name="end">The range period end</param>
    /// <param name="forecast">The forecast</param>
    /// <param name="jobStatus">The job status</param>
    /// <param name="tags">The result tags</param>
    /// <returns>The consolidate wage type custom results</returns>
    IList<Tuple<decimal, string, string, Tuple<DateTime, DateTime>, decimal, List<string>, Dictionary<string, object>>> GetWageTypeCustomResults(
        IList<decimal> wageTypeNumbers, DateTime start, DateTime end,
        string forecast = null, int? jobStatus = null, IList<string> tags = null);

    /// <summary>Gets the consolidated wage type custom results, from the period moment until the current period</summary>
    /// <param name="wageTypeNumbers">The wage type numbers</param>
    /// <param name="periodMoment">The period moment</param>
    /// <param name="forecast">The forecast</param>
    /// <param name="jobStatus">The job status</param>
    /// <param name="tags">The result tags</param>
    /// <returns>The consolidate wage type custom results</returns>
    IList<Tuple<decimal, string, string, Tuple<DateTime, DateTime>, decimal, List<string>, Dictionary<string, object>>> GetConsolidatedWageTypeCustomResults(
        IList<decimal> wageTypeNumbers, DateTime periodMoment,
        string forecast = null, int? jobStatus = null, IList<string> tags = null);

    /// <summary>Gets the retro wage type results</summary>
    /// <param name="wageTypeNumber">The wage type number</param>
    /// <param name="forecast">The forecast</param>
    /// <param name="jobStatus">The job status</param>
    /// <param name="tags">The result tags</param>
    /// <returns>The wage type cycle results</returns>
    IList<decimal> GetRetroWageTypeResults(decimal wageTypeNumber,
        string forecast = null, int? jobStatus = null, IList<string> tags = null);

    #endregion

    #region Collector Results

    /// <summary>Gets the collector range results</summary>
    /// <param name="collectorNames">Name of the collectors</param>
    /// <param name="start">The range period start</param>
    /// <param name="end">The range period end</param>
    /// <param name="forecast">The forecast</param>
    /// <param name="jobStatus">The job status</param>
    /// <param name="tags">The result tags</param>
    /// <returns>The collector date range results</returns>
    IList<Tuple<string, Tuple<DateTime, DateTime>, decimal, List<string>, Dictionary<string, object>>> GetCollectorResults(
        IList<string> collectorNames, DateTime start, DateTime end,
        string forecast = null, int? jobStatus = null, IList<string> tags = null);

    /// <summary>Gets the consolidated collector results, from the period moment until the current period</summary>
    /// <param name="collectorNames">Name of the collectors</param>
    /// <param name="periodMoment">The period moment</param>
    /// <param name="forecast">The forecast</param>
    /// <param name="jobStatus">The job status</param>
    /// <param name="tags">The result tags</param>
    /// <returns>The consolidated collector results</returns>
    IList<Tuple<string, Tuple<DateTime, DateTime>, decimal, List<string>, Dictionary<string, object>>> GetConsolidatedCollectorResults(
        IList<string> collectorNames, DateTime periodMoment,
        string forecast = null, int? jobStatus = null, IList<string> tags = null);

    /// <summary>Gets the collector results from a time range</summary>
    /// <param name="collectorNames">Name of the collectors</param>
    /// <param name="start">The range period start</param>
    /// <param name="end">The range period end</param>
    /// <param name="forecast">The forecast</param>
    /// <param name="jobStatus">The job status</param>
    /// <param name="tags">The result tags</param>
    /// <returns>The consolidated collector custom results</returns>
    IList<Tuple<string, string, Tuple<DateTime, DateTime>, decimal, List<string>, Dictionary<string, object>>> GetCollectorCustomResults(
        IList<string> collectorNames, DateTime start, DateTime end, string forecast = null,
        int? jobStatus = null, IList<string> tags = null);

    /// <summary>Gets the consolidated collector results, from the period moment until the current period</summary>
    /// <param name="collectorNames">Name of the collectors</param>
    /// <param name="periodMoment">The period moment</param>
    /// <param name="forecast">The forecast</param>
    /// <param name="jobStatus">The job status</param>
    /// <param name="tags">The result tags</param>
    /// <returns>The consolidated collector custom results</returns>
    IList<Tuple<string, string, Tuple<DateTime, DateTime>, decimal, List<string>, Dictionary<string, object>>> GetConsolidatedCollectorCustomResults(
        IList<string> collectorNames, DateTime periodMoment, string forecast = null,
        int? jobStatus = null, IList<string> tags = null);

    #endregion

    #region Webhook

    /// <summary>Invoke payrun webhook and receive the response JSON data</summary>
    /// <param name="requestOperation">The request operation</param>
    /// <param name="requestMessage">The JSON request message</param>
    /// <returns>The webhook response object as JSON</returns>
    string InvokeWebhook(string requestOperation, string requestMessage = null);

    #endregion

}