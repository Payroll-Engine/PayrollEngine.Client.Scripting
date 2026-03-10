/* PayrunFunction */

using System;
using System.Linq;
using System.Text.Json;
using System.Collections.Generic;

namespace PayrollEngine.Client.Scripting.Function;

/// <summary>
/// Abstract base class for all payrun-scoped functions, providing access to payrun job context,
/// runtime value exchange between functions, payrun result writing, and wage type / collector result queries.
/// </summary>
/// <remarks>
/// <para>This class sits between <see cref="PayrollFunction"/> and the concrete payrun function types.
/// It adds the payrun execution context (job identity, retro period, forecast flag, cycle/period names)
/// and the shared-memory facilities that allow functions running at different lifecycle stages to
/// communicate without writing permanent data.</para>
/// <para><strong>Execution order within a payrun:</strong></para>
/// <list type="bullet">
///   <item><see cref="PayrunStartFunction"/> — once at the start of the entire job.</item>
///   <item><see cref="PayrunEmployeeAvailableFunction"/> — once per employee (gate).</item>
///   <item><see cref="PayrunEmployeeStartFunction"/> — once per included employee.</item>
///   <item><see cref="PayrunWageTypeAvailableFunction"/> — once per wage type per employee (gate).</item>
///   <item><see cref="WageTypeValueFunction"/> / <see cref="WageTypeResultFunction"/> — value calculation and post-processing.</item>
///   <item><see cref="CollectorStartFunction"/> / <see cref="CollectorApplyFunction"/> / <see cref="CollectorEndFunction"/> — collector lifecycle.</item>
///   <item><see cref="PayrunEmployeeEndFunction"/> — once per included employee.</item>
///   <item><see cref="PayrunEndFunction"/> — once at the end of the entire job.</item>
/// </list>
/// <para><strong>Runtime values</strong> are in-memory key/value string stores scoped to either the
/// entire payrun (<see cref="GetPayrunRuntimeValue"/> / <see cref="SetPayrunRuntimeValue"/>) or to a
/// single employee (<see cref="GetEmployeeRuntimeValue"/> / <see cref="SetEmployeeRuntimeValue"/>).
/// They exist only for the duration of the payrun job and are not persisted.</para>
/// <para><strong>Payrun results</strong> (<see cref="SetPayrunResult(string, object, ValueType?, string, System.Collections.Generic.IEnumerable{string}, System.Collections.Generic.Dictionary{string, object}, string)"/>) are durable named value entries
/// written to the payrun result store and accessible by reports and downstream processes.</para>
/// </remarks>
// ReSharper disable once PartialTypeWithSinglePart
public abstract partial class PayrunFunction : PayrollFunction
{
    /// <summary>Initializes a new instance with the function runtime</summary>
    /// <param name="runtime">The runtime</param>
    protected PayrunFunction(object runtime) :
        base(runtime)
    {
        // payrun
        PayrunId = Runtime.PayrunId;
        PayrunName = Runtime.PayrunName;

        // payrun job
        PreviewJob = Runtime.PreviewJob;
        ExecutionPhase = (PayrunExecutionPhase)Runtime.ExecutionPhase;
        RetroPeriod = Runtime.RetroPeriod is not Tuple<DateTime, DateTime> retroPeriod ? null :
            new DatePeriod(retroPeriod.Item1, retroPeriod.Item2);
        Forecast = Runtime.Forecast;
        CycleName = Runtime.CycleName;
        PeriodName = Runtime.PeriodName;
    }

    /// <summary>New function instance without runtime (scripting development)</summary>
    /// <param name="sourceFileName">The name of the source file</param>
    protected PayrunFunction(string sourceFileName) :
        base(sourceFileName)
    {
    }

    #region Payrun

    /// <summary>The payrun id</summary>
    public int PayrunId { get; }

    /// <summary>The payrun name</summary>
    [ActionProperty("Payrun name")]
    public string PayrunName { get; }

    #endregion

    #region PayrunJob

    /// <summary>True when the payrun job was started in preview mode; results are calculated but not committed</summary>
    /// <remarks>Use this flag to suppress side-effects (e.g. task creation, webhook calls) that must not fire during previews.</remarks>
    public bool PreviewJob { get; }

    /// <summary>The current execution phase of the payrun job (e.g. Setup, Execution, Cleanup)</summary>
    public PayrunExecutionPhase ExecutionPhase { get; }

    /// <summary>The period being recalculated in a retro run; <c>null</c> for a normal payrun</summary>
    /// <remarks>Use <see cref="IsRetroPayrun"/> as a guard. The retro period precedes the current evaluation period.</remarks>
    public DatePeriod RetroPeriod { get; }

    /// <summary>True for a retro payrun</summary>
    [ActionProperty("Test for retro payrun")]
    public bool IsRetroPayrun => RetroPeriod != null;

    /// <summary>True for a retro payrun within the current cycle</summary>
    [ActionProperty("Test for cycle retro payrun")]
    public bool IsCycleRetroPayrun =>
        RetroPeriod != null && Cycle.IsWithin(RetroPeriod);

    /// <summary>Forecast name</summary>
    [ActionProperty("Forecast name")]
    public string Forecast { get; }

    /// <summary>True for a forecast payrun</summary>
    [ActionProperty("Test for forecast")]
    public bool IsForecast => !string.IsNullOrWhiteSpace(Forecast);

    /// <summary>The cycle name</summary>
    [ActionProperty("Cycle name")]
    public string CycleName { get; }

    /// <summary>The period name</summary>
    [ActionProperty("Period name")]
    public string PeriodName { get; }

    /// <summary>Returns the value of a payrun job attribute by name</summary>
    /// <param name="attributeName">The attribute name</param>
    /// <returns>The attribute value, or <c>null</c> when the attribute does not exist</returns>
    public object GetPayrunJobAttribute(string attributeName) =>
        Runtime.GetPayrunJobAttribute(attributeName);

    /// <summary>Returns the typed value of a payrun job attribute by name</summary>
    /// <typeparam name="T">The expected value type</typeparam>
    /// <param name="attributeName">The attribute name</param>
    /// <param name="defaultValue">Value returned when the attribute does not exist</param>
    /// <returns>The typed attribute value, or <paramref name="defaultValue"/> when not found</returns>
    public T GetPayrunJobAttribute<T>(string attributeName, T defaultValue = default) =>
        ChangeValueType(GetPayrunJobAttribute(attributeName), defaultValue);

    /// <summary>Creates or updates a payrun job attribute</summary>
    /// <param name="attributeName">The attribute name</param>
    /// <param name="value">The attribute value to store</param>
    public void SetPayrunJobAttribute(string attributeName, object value) =>
        Runtime.SetPayrunJobAttribute(attributeName, value);

    /// <summary>Removes a payrun job attribute by name</summary>
    /// <param name="attributeName">The attribute name</param>
    /// <returns><c>true</c> if the attribute existed and was removed; <c>false</c> otherwise</returns>
    public bool RemovePayrunJobAttribute(string attributeName) =>
        Runtime.RemovePayrunJobAttribute(attributeName);

    #endregion

    #region Runtime values

    /// <summary>Tests whether a payrun-scoped runtime value with the given key exists</summary>
    /// <param name="key">The value key</param>
    /// <returns><c>true</c> if the key is present; <c>false</c> otherwise</returns>
    public bool HasPayrunRuntimeValue(string key) =>
        Runtime.HasPayrunRuntimeValue(key);

    /// <summary>Returns the raw string value of a payrun-scoped runtime entry</summary>
    /// <param name="key">The value key</param>
    /// <returns>The stored string, or <c>null</c> when the key does not exist</returns>
    /// <remarks>
    /// Payrun runtime values are shared across all functions within the same payrun job execution.
    /// They are ephemeral — written in one function (e.g. <see cref="PayrunStartFunction"/>) and
    /// readable in any later function within the same job. They are not persisted after the job completes.
    /// For complex types use <see cref="GetPayrunRuntimeValue{T}"/> which deserializes from JSON.
    /// </remarks>
    public string GetPayrunRuntimeValue(string key) =>
        Runtime.GetPayrunRuntimeValue(key);

    /// <summary>Returns a payrun-scoped runtime value deserialized to the specified type</summary>
    /// <typeparam name="T">The target type; the stored JSON string is deserialized into this type</typeparam>
    /// <param name="key">The value key</param>
    /// <returns>The deserialized value, or <c>default(T)</c> when the key does not exist</returns>
    public T GetPayrunRuntimeValue<T>(string key)
    {
        var value = GetPayrunRuntimeValue(key);
        return value == null ? default : JsonSerializer.Deserialize<T>(value);
    }

    /// <summary>Writes a payrun-scoped runtime value as a raw string</summary>
    /// <param name="key">The value key</param>
    /// <param name="value">The string value to store; pass <c>null</c> to delete the entry</param>
    /// <remarks>
    /// Values written here are visible to all subsequent functions within the same payrun job.
    /// Use <see cref="SetPayrunRuntimeValue{T}"/> to serialize complex types to JSON automatically.
    /// </remarks>
    public void SetPayrunRuntimeValue(string key, string value) =>
        Runtime.SetPayrunRuntimeValue(key, value);

    /// <summary>Serializes a value to JSON and writes it as a payrun-scoped runtime entry</summary>
    /// <typeparam name="T">The value type to serialize</typeparam>
    /// <param name="key">The value key</param>
    /// <param name="value">The value to serialize and store</param>
    public void SetPayrunRuntimeValue<T>(string key, T value) =>
        SetPayrunRuntimeValue(key, JsonSerializer.Serialize(value));

    /// <summary>Tests whether an employee-scoped runtime value with the given key exists for the current employee</summary>
    /// <param name="key">The value key</param>
    /// <returns><c>true</c> if the key is present; <c>false</c> otherwise</returns>
    public bool HasEmployeeRuntimeValue(string key) =>
        Runtime.HasEmployeeRuntimeValue(key);

    /// <summary>Returns the raw string value of an employee-scoped runtime entry for the current employee</summary>
    /// <param name="key">The value key</param>
    /// <returns>The stored string, or <c>null</c> when the key does not exist</returns>
    /// <remarks>
    /// Employee runtime values are scoped to the current employee and isolated from other employees.
    /// They allow data to flow from early functions (e.g. <see cref="PayrunEmployeeStartFunction"/>)
    /// to later ones (e.g. <see cref="CollectorEndFunction"/>, <see cref="PayrunEmployeeEndFunction"/>)
    /// for the same employee. In <see cref="PayrunEndFunction"/>, all employees' values are accessible
    /// via <see cref="PayrunEndFunction.GetEmployeeRuntimeValues"/>.
    /// </remarks>
    public string GetEmployeeRuntimeValue(string key) =>
        Runtime.GetEmployeeRuntimeValue(key);

    /// <summary>Returns an employee-scoped runtime value deserialized to the specified type</summary>
    /// <typeparam name="T">The target type; the stored JSON string is deserialized into this type</typeparam>
    /// <param name="key">The value key</param>
    /// <returns>The deserialized value, or <c>default(T)</c> when the key does not exist</returns>
    public T GetEmployeeRuntimeValue<T>(string key)
    {
        var value = GetEmployeeRuntimeValue(key);
        return value == null ? default : JsonSerializer.Deserialize<T>(value);
    }

    /// <summary>Writes an employee-scoped runtime value as a raw string for the current employee</summary>
    /// <param name="key">The value key</param>
    /// <param name="value">The string value to store; pass <c>null</c> to delete the entry</param>
    public void SetEmployeeRuntimeValue(string key, string value) =>
        Runtime.SetEmployeeRuntimeValue(key, value);

    /// <summary>Serializes a value to JSON and writes it as an employee-scoped runtime entry for the current employee</summary>
    /// <typeparam name="T">The value type to serialize</typeparam>
    /// <param name="key">The value key</param>
    /// <param name="value">The value to serialize and store; pass <c>null</c> to delete the entry</param>
    public void SetEmployeeRuntimeValue<T>(string key, T value) =>
        SetEmployeeRuntimeValue(key, value != null ? JsonSerializer.Serialize(value) : null);

    #endregion

    #region Payrun Results

    /// <summary>Returns a previously written payrun result value by name</summary>
    /// <param name="name">The result name as passed to <see cref="SetPayrunResult(string, object, ValueType?, string, IEnumerable{string}, Dictionary{string, object}, string)"/></param>
    /// <returns>The raw result value, or <c>null</c> when no result with that name exists</returns>
    public object GetPayrunResult(string name) =>
        Runtime.GetPayrunResult(GetType().Name, name);

    /// <summary>Returns a previously written payrun result value cast to the specified type</summary>
    /// <typeparam name="T">The expected value type</typeparam>
    /// <param name="name">The result name</param>
    /// <param name="defaultValue">Value returned when the result does not exist</param>
    /// <returns>The typed result value, or <paramref name="defaultValue"/> when not found</returns>
    public T GetPayrunResult<T>(string name, T defaultValue = default) =>
        ChangeValueType(GetPayrunResult(name), defaultValue);

    /// <summary>Writes a named payrun result for the current period to the durable payrun result store</summary>
    /// <param name="name">Unique result name within this payrun execution</param>
    /// <param name="value">The result value; serialized to JSON internally</param>
    /// <param name="valueType">Explicit value type; inferred from <paramref name="value"/> when <c>null</c></param>
    /// <param name="slot">Optional slot name for multi-slot results</param>
    /// <param name="tags">Optional result tags for filtering</param>
    /// <param name="attributes">Optional key/value metadata attached to the result</param>
    /// <param name="culture">Optional culture for value formatting</param>
    /// <remarks>
    /// Payrun results are <strong>durable</strong> — unlike runtime values, they are persisted after
    /// the payrun completes and are accessible by reports via <c>ExecutePayrunResultQuery</c>.
    /// Use them to publish derived metrics (totals, counts, flags) that reports or downstream
    /// processes need to consume. The <paramref name="name"/> must be unique within the payrun job.
    /// </remarks>
    public void SetPayrunResult(string name, object value, ValueType? valueType = null,
        string slot = null, IEnumerable<string> tags = null,
        Dictionary<string, object> attributes = null, string culture = null) =>
        SetPayrunResult(name, value, PeriodStart, PeriodEnd, valueType, slot, tags, attributes, culture);

    /// <summary>Writes a named payrun result for an explicit date range to the durable payrun result store</summary>
    /// <param name="name">Unique result name within this payrun execution</param>
    /// <param name="value">The result value; serialized to JSON internally</param>
    /// <param name="startDate">The result validity start date</param>
    /// <param name="endDate">The result validity end date</param>
    /// <param name="valueType">Explicit value type; inferred from <paramref name="value"/> when <c>null</c></param>
    /// <param name="slot">Optional slot name for multi-slot results</param>
    /// <param name="tags">Optional result tags for filtering</param>
    /// <param name="attributes">Optional key/value metadata attached to the result</param>
    /// <param name="culture">Optional culture for value formatting</param>
    /// <remarks>Use this overload when the result must cover a date range different from the current payroll period.</remarks>
    public void SetPayrunResult(string name, object value, DateTime startDate, DateTime endDate,
        ValueType? valueType = null, string slot = null,
        IEnumerable<string> tags = null, Dictionary<string, object> attributes = null, string culture = null)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException(nameof(name));
        }
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        var source = GetType().Name;
        var json = JsonSerializer.Serialize(value);
        valueType ??= value.GetValueType();
        Runtime.SetPayrunResult(source, name, json, (int)valueType.Value, startDate, endDate, slot, tags?.ToList(), attributes, culture);
    }

    #endregion

    #region Wage Type

    /// <summary>Returns the numeric identifier of a wage type by its name</summary>
    /// <param name="wageTypeName">The PascalCase wage type name</param>
    /// <returns>The wage type number</returns>
    /// <remarks>Use this to convert a readable name to a number before calling result query methods.</remarks>
    public decimal GetWageTypeNumber(string wageTypeName) => Runtime.GetWageTypeNumber(wageTypeName);

    /// <summary>Returns the name of a wage type by its numeric identifier</summary>
    /// <param name="wageTypeNumber">The wage type number</param>
    /// <returns>The PascalCase wage type name</returns>
    public string GetWageTypeName(decimal wageTypeNumber) => Runtime.GetWageTypeName(wageTypeNumber);

    /// <summary>Returns wage type results for the current employee spanning one or more complete payroll cycles</summary>
    /// <param name="query">Cycle query specifying wage type numbers, cycle count offset, forecast, job status, and tags</param>
    /// <returns>List of <see cref="WageTypeResult"/> covering the requested cycles</returns>
    /// <remarks>
    /// Use <c>query.CycleCount = 1</c> for the previous cycle (YTD prior year),
    /// <c>query.CycleCount = 0</c> for the current cycle up to the evaluation period.
    /// For finer period-level control use <see cref="GetPeriodWageTypeResults"/>.
    /// </remarks>
    public IList<WageTypeResult> GetWageTypeCycleResults(WageTypeCycleResultQuery query)
    {
        var period = GetCycle(query.CycleCount * -1);
        return GetWageTypeResults(query.WageTypes, period.Start, period.End, query.Forecast, query.JobStatus, query.Tags);
    }

    /// <summary>Returns wage type results for the current employee spanning a number of payroll periods back from the current period</summary>
    /// <param name="query">Period query specifying wage type numbers, period count offset, forecast, job status, and tags</param>
    /// <returns>List of <see cref="WageTypeResult"/> covering the requested periods</returns>
    /// <remarks>
    /// <c>query.PeriodCount = 1</c> returns the previous period, <c>query.PeriodCount = 3</c> the last three periods.
    /// For full-cycle queries use <see cref="GetWageTypeCycleResults"/>; for an explicit date range use
    /// <see cref="GetWageTypeResults(WageTypeRangeResultQuery)"/>.
    /// </remarks>
    public IList<WageTypeResult> GetPeriodWageTypeResults(WageTypePeriodResultQuery query)
    {
        var period = GetPeriod(query.PeriodCount * -1);
        return GetWageTypeResults(query.WageTypes, period.Start, period.End, query.Forecast, query.JobStatus, query.Tags);
    }

    /// <summary>Returns wage type results for the current employee within an explicit date range</summary>
    /// <param name="query">Range query specifying wage type numbers, start/end dates, forecast, job status, and tags</param>
    /// <returns>List of <see cref="WageTypeResult"/> within the date range</returns>
    public IList<WageTypeResult> GetWageTypeResults(WageTypeRangeResultQuery query) =>
        GetWageTypeResults(query.WageTypes, query.Start, query.End, query.Forecast, query.JobStatus, query.Tags);

    /// <summary>Returns wage type results for the current employee within an explicit date range (low-level overload)</summary>
    /// <param name="wageTypeNumbers">The wage type numbers to query</param>
    /// <param name="start">Range start date (inclusive)</param>
    /// <param name="end">Range end date (inclusive)</param>
    /// <param name="forecast">Optional forecast name; <c>null</c> for committed results</param>
    /// <param name="jobStatus">Optional job status filter; <c>null</c> returns all statuses</param>
    /// <param name="tags">Optional tag filter; only results carrying all listed tags are returned</param>
    /// <returns>List of <see cref="WageTypeResult"/> within the date range</returns>
    public IList<WageTypeResult> GetWageTypeResults(IEnumerable<decimal> wageTypeNumbers, DateTime start, DateTime end,
        string forecast = null, PayrunJobStatus? jobStatus = null, IEnumerable<string> tags = null) =>
        TupleExtensions.TupleToWageTypeResults(Runtime.GetWageTypeResults(wageTypeNumbers.ToList(), start, end,
            forecast, (int?)jobStatus, tags?.ToList()));

    /// <summary>Returns consolidated wage type results for the current employee, merging retro corrections into the base values</summary>
    /// <param name="query">Query specifying wage type numbers, the reference period moment, forecast, job status, and tags</param>
    /// <returns>List of <see cref="WageTypeResult"/> with retro differences already applied</returns>
    /// <remarks>
    /// "Consolidated" means the engine merges any retro-payrun corrections for the queried periods
    /// into the base results, so the returned values reflect the net committed amount.
    /// Set <c>query.NoRetro = true</c> to suppress retro merging and return only base results.
    /// Use <see cref="GetWageTypeCycleResults"/> or <see cref="GetPeriodWageTypeResults"/> when you
    /// need raw per-period results without retro consolidation.
    /// </remarks>
    public IList<WageTypeResult> GetConsolidatedWageTypeResults(WageTypeConsolidatedResultQuery query) =>
        TupleExtensions.TupleToWageTypeResults(Runtime.GetConsolidatedWageTypeResults(query.WageTypes, query.PeriodMoment,
            query.Forecast, (int?)query.JobStatus, query.Tags, query.NoRetro));

    /// <summary>Returns custom wage type results (additional named values) for the current employee within an explicit date range</summary>
    /// <param name="query">Range query specifying wage type numbers, start/end dates, forecast, job status, and tags</param>
    /// <returns>List of <see cref="WageTypeCustomResult"/> within the date range</returns>
    /// <remarks>
    /// Custom results are the extra named values a <see cref="WageTypeValueFunction"/> stores via
    /// <c>AddCustomResult</c>. Use this method to read them back in later functions.
    /// </remarks>
    public IList<WageTypeCustomResult> GetWageTypeCustomResults(WageTypeRangeResultQuery query) =>
        TupleExtensions.TupleToWageTypeCustomResults(Runtime.GetWageTypeCustomResults(query.WageTypes, query.Start,
            query.End, query.Forecast, (int?)query.JobStatus, query.Tags));

    /// <summary>Returns consolidated custom wage type results for the current employee, merging retro corrections into the base values</summary>
    /// <param name="query">Consolidated query specifying wage type numbers, reference period moment, forecast, job status, and tags</param>
    /// <returns>List of <see cref="WageTypeCustomResult"/> with retro differences already applied</returns>
    /// <remarks>Set <c>query.NoRetro = true</c> to suppress retro merging.</remarks>
    public IList<WageTypeCustomResult> GetConsolidatedWageTypeCustomResults(WageTypeConsolidatedResultQuery query) =>
        TupleExtensions.TupleToWageTypeCustomResults(Runtime.GetConsolidatedWageTypeCustomResults(query.WageTypes, query.PeriodMoment,
            query.Forecast, (int?)query.JobStatus, query.Tags, query.NoRetro));

    /// <summary>Returns the individual retro-correction amounts for a wage type across all pending retro periods</summary>
    /// <param name="query">Query identifying the wage type number and optional forecast/status/tag filters</param>
    /// <returns>List of correction deltas (positive = underpayment correction, negative = overpayment correction)</returns>
    /// <remarks>
    /// Each entry represents the difference between the recalculated and the originally committed result
    /// for one retro period. Use <see cref="GetRetroWageTypeValueSum(decimal)"/> to get the total
    /// correction in a single call.
    /// </remarks>
    public IList<decimal> GetWageTypeRetroResults(WageTypeResultQuery query) =>
        Runtime.GetRetroWageTypeResults(query.WageTypes[0], query.Forecast, (int?)query.JobStatus, query.Tags);

    /// <summary>Get summary of retro wage type results</summary>
    /// <param name="wageTypeNumber">The wage type number</param>
    /// <returns>Retro employee wage type value (difference)</returns>
    public decimal GetWageTypeRetroResultSum(decimal wageTypeNumber) =>
        GetWageTypeRetroResults(new(wageTypeNumber)).DefaultIfEmpty().Sum();

    /// <summary>Returns the net sum of all pending retro corrections for a wage type identified by number</summary>
    /// <param name="number">The wage type number</param>
    /// <returns>Sum of all retro correction deltas; zero when no corrections are pending</returns>
    /// <remarks>
    /// Typical use: add the retro correction to the current period's wage type value so the employee
    /// receives the missed amount in the current payslip.
    /// </remarks>
    /// <example>
    /// <code language="c#">
    /// // Include retro correction in the current period's gross salary
    /// var retro = GetRetroWageTypeValueSum(1000m);
    /// return GetCaseValue&lt;decimal&gt;("MonthlySalary") + retro;
    /// </code>
    /// </example>
    public decimal GetRetroWageTypeValueSum(decimal number) =>
        GetWageTypeRetroResultSum(number);

    /// <summary>Returns the net sum of all pending retro corrections for a wage type identified by name</summary>
    /// <param name="name">The wage type name</param>
    /// <returns>Sum of all retro correction deltas; zero when no corrections are pending</returns>
    public decimal GetRetroWageTypeValueSum(string name) =>
        GetWageTypeRetroResultSum(GetWageTypeNumber(name));

    #endregion

    #region Collector

    /// <summary>Returns collector results for the current employee spanning one or more complete payroll cycles</summary>
    /// <param name="query">Cycle query specifying collector names, cycle count offset, forecast, job status, and tags</param>
    /// <returns>List of <see cref="CollectorResult"/> covering the requested cycles</returns>
    /// <remarks>Collector equivalent of <see cref="GetWageTypeCycleResults"/>.</remarks>
    public IList<CollectorResult> GetCollectorCycleResults(CollectorCycleResultQuery query)
    {
        var period = GetCycle(query.CycleCount * -1);
        return GetCollectorResults(query.Collectors, period.Start, period.End, query.Forecast, query.JobStatus, query.Tags);
    }

    /// <summary>Returns collector results for the current employee spanning a number of payroll periods back from the current period</summary>
    /// <param name="query">Period query specifying collector names, period count offset, forecast, job status, and tags</param>
    /// <returns>List of <see cref="CollectorResult"/> covering the requested periods</returns>
    /// <remarks>Collector equivalent of <see cref="GetPeriodWageTypeResults"/>.</remarks>
    public IList<CollectorResult> GetCollectorPeriodResults(CollectorPeriodResultQuery query)
    {
        var period = GetPeriod(query.PeriodCount * -1);
        return GetCollectorResults(query.Collectors, period.Start, period.End, query.Forecast, query.JobStatus, query.Tags);
    }

    /// <summary>Returns collector results for the current employee within an explicit date range</summary>
    /// <param name="query">Range query specifying collector names, start/end dates, forecast, job status, and tags</param>
    /// <returns>List of <see cref="CollectorResult"/> within the date range</returns>
    public IList<CollectorResult> GetCollectorResults(CollectorRangeResultQuery query) =>
        GetCollectorResults(query.Collectors, query.Start, query.End, query.Forecast, query.JobStatus, query.Tags);

    /// <summary>Returns collector results for the current employee within an explicit date range (low-level overload)</summary>
    /// <param name="collectorNames">The PascalCase collector names to query</param>
    /// <param name="start">Range start date (inclusive)</param>
    /// <param name="end">Range end date (inclusive)</param>
    /// <param name="forecast">Optional forecast name; <c>null</c> for committed results</param>
    /// <param name="jobStatus">Optional job status filter; <c>null</c> returns all statuses</param>
    /// <param name="tags">Optional tag filter</param>
    /// <returns>List of <see cref="CollectorResult"/> within the date range</returns>
    public IList<CollectorResult> GetCollectorResults(IEnumerable<string> collectorNames, DateTime start, DateTime end,
        string forecast = null, PayrunJobStatus? jobStatus = null, IEnumerable<string> tags = null) =>
        TupleExtensions.TupleToCollectorResults(Runtime.GetCollectorResults(collectorNames.ToList(),
            start, end, forecast, (int?)jobStatus, tags?.ToList()));

    /// <summary>Returns consolidated collector results for the current employee, merging retro corrections into the base values</summary>
    /// <param name="query">Query specifying collector names, the reference period moment, forecast, job status, and tags</param>
    /// <returns>List of <see cref="CollectorResult"/> with retro differences already applied</returns>
    /// <remarks>
    /// Equivalent to <see cref="GetConsolidatedWageTypeResults"/> but for collectors.
    /// Retro merging can be suppressed with <c>query.NoRetro = true</c>.
    /// </remarks>
    public IList<CollectorResult> GetConsolidatedCollectorResults(CollectorConsolidatedResultQuery query) =>
        TupleExtensions.TupleToCollectorResults(Runtime.GetConsolidatedCollectorResults(query.Collectors,
            query.PeriodMoment, query.Forecast, (int?)query.JobStatus, query.Tags, query.NoRetro));

    /// <summary>Returns consolidated custom collector results for the current employee, merging retro corrections into the base values</summary>
    /// <param name="query">Consolidated query specifying collector names, reference period moment, forecast, job status, and tags</param>
    /// <returns>List of <see cref="CollectorCustomResult"/> with retro differences already applied</returns>
    /// <remarks>Set <c>query.NoRetro = true</c> to suppress retro merging.</remarks>
    public IList<CollectorCustomResult> GetConsolidatedCollectorCustomResults(CollectorConsolidatedResultQuery query) =>
        TupleExtensions.TupleToCollectorCustomResults(Runtime.GetConsolidatedCollectorCustomResults(query.Collectors,
            query.PeriodMoment, query.Forecast, (int?)query.JobStatus, query.Tags, query.NoRetro));

    #endregion

}