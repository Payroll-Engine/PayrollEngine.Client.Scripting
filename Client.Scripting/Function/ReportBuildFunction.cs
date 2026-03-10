/* ReportBuildFunction */

// ReSharper disable RedundantUsingDirective
using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Collections.Generic;
using PayrollEngine.Client.Scripting;
using PayrollEngine.Client.Scripting.Report;
// ReSharper restore RedundantUsingDirective

namespace PayrollEngine.Client.Scripting.Function;

/// <summary>
/// Configures the report before it is executed: populates parameter lists, sets defaults, and controls parameter visibility.
/// </summary>
/// <remarks>
/// This function runs when the user opens a report to prepare the parameter form.
/// It is called every time the form is refreshed. Use it to dynamically populate
/// drop-down lists, pre-select defaults, and show or hide parameters based on other
/// parameter values or the user context.
/// <para>Key capabilities:</para>
/// <list type="bullet">
///   <item><strong>Set a parameter value:</strong> <see cref="SetParameter"/> / <see cref="SetParameter{T}"/>.</item>
///   <item><strong>Show/hide parameters:</strong> <see cref="ShowParameter"/> / <see cref="HideParameter"/>.
///   Use <see cref="SetParameterReadOnly"/> to make a single-option parameter non-editable.</item>
///   <item><strong>Populate a list:</strong> <see cref="ExecuteInputListQuery"/> queries the API and
///   sets the <c>List</c> and <c>ListValues</c> parameter attributes in one call.
///   <see cref="BuildInputList"/> does the same from an already-fetched <see cref="System.Data.DataTable"/>.</item>
///   <item><strong>Mark as invalid:</strong> <see cref="BuildInvalid"/> blocks report execution
///   (e.g. when a required lookup is missing).</item>
/// </list>
/// <para><strong>Return value:</strong> Return <c>null</c> to indicate a successful build and
/// allow the user to edit parameters. Return <c>false</c> to mark the report as not buildable.</para>
/// <para>For queries, data assembly, and result post-processing see
/// <see cref="ReportStartFunction"/> and <see cref="ReportEndFunction"/>.</para>
/// </remarks>
/// <example>
/// <code language="c#">
/// // Resolve the payroll id from the PayrollId parameter, then build an employee list
/// var payrollId = ResolveParameterPayrollId() ?? 0;
/// ExecuteInputListQuery(
///     "QueryEmployees",
///     new QueryParameters()
///         .ActiveStatus()
///         .Parameter("TenantId", TenantId)
///         .Parameter("PayrollId", payrollId),
///     "EmployeeId",
///     row =&gt; row.GetValue&lt;int&gt;("EmployeeId"),
///     row =&gt; row.GetValue&lt;string&gt;("LastName") + ", " + row.GetValue&lt;string&gt;("FirstName"));
/// </code>
/// <code language="c#">
/// // Hide a parameter when only one payroll exists
/// var payrollId = ResolveParameterPayrollId();
/// if (payrollId.HasValue)
///     HideParameter("PayrollId");
/// </code>
/// </example>
/// <seealso cref="ReportStartFunction"/>
/// <seealso cref="ReportEndFunction"/>
// ReSharper disable once PartialTypeWithSinglePart
public partial class ReportBuildFunction : ReportFunction
{
    /// <summary>Initializes a new instance with the function runtime</summary>
    /// <param name="runtime">The runtime</param>
    public ReportBuildFunction(object runtime) :
        base(runtime)
    {
    }

    /// <summary>New function instance without runtime (scripting development)</summary>
    /// <param name="sourceFileName">The name of the source file</param>
    public ReportBuildFunction(string sourceFileName) :
        base(sourceFileName)
    {
    }

    #region Parameter

    /// <summary>Set report parameter value</summary>
    /// <param name="parameterName">The parameter name</param>
    /// <param name="value">The parameter value as JSON</param>
    public void SetParameter(string parameterName, string value) =>
        Runtime.SetParameter(parameterName, value);

    /// <summary>Set report parameter typed value</summary>
    /// <param name="parameterName">The parameter name</param>
    /// <param name="value">The default value</param>
    /// <returns>The report parameter value</returns>
    public void SetParameter<T>(string parameterName, T value) =>
        SetParameter(parameterName, value as string ?? JsonSerializer.Serialize(value));

    /// <summary>Show report parameters</summary>
    /// <param name="parameterNames">The parameter names</param>
    public void ShowParameter(params string[] parameterNames)
    {
        foreach (var parameterName in parameterNames)
        {
            SetParameterHidden(parameterName, hidden: false);
        }
    }

    /// <summary>Hide report parameter</summary>
    /// <param name="parameterNames">The parameter name</param>
    public void HideParameter(params string[] parameterNames)
    {
        foreach (var parameterName in parameterNames)
        {
            SetParameterHidden(parameterName, hidden: true);
        }
    }

    /// <summary>Set the report parameter hidden state</summary>
    /// <param name="parameterName">The parameter name</param>
    /// <param name="hidden">The hidden state</param>
    public void SetParameterHidden(string parameterName, bool hidden) =>
        Runtime.SetParameterHidden(parameterName, hidden);

    /// <summary>Set the report parameter read-only state</summary>
    /// <param name="parameterName">The parameter name</param>
    /// <param name="readOnly">The read-only state</param>
    public void SetParameterReadOnly(string parameterName, bool readOnly) =>
        SetParameterAttribute(parameterName, InputAttributes.ValueReadOnly, readOnly);

    #endregion

    #region Queries

    /// <summary>
    /// Build report parameter input list based on query
    /// </summary>
    /// <remarks>Use the query order-by for sorting</remarks>
    /// <param name="queryMethod">Query method name</param>
    /// <param name="queryParameters">Query parameters</param>
    /// <param name="reportParameter">Report parameter name</param>
    /// <param name="identifierFunc">Identifier function</param>
    /// <param name="displayFunc">Display function: return null to use the identifier for display</param>
    /// <param name="selectFunc">Selection function: return the selected identifier or null</param>
    /// <returns>List of identifiers, selected by the identifier function</returns>
    public List<object> ExecuteInputListQuery(
            string queryMethod,
            Dictionary<string, string> queryParameters,
            string reportParameter,
            Func<DataRow, object> identifierFunc,
            Func<DataRow, string> displayFunc = null,
            Func<List<object>, object> selectFunc = null)
    {
        if (string.IsNullOrWhiteSpace(queryMethod))
        {
            throw new ArgumentException(nameof(queryMethod));
        }

        // query
        var table = ExecuteQuery(queryMethod, queryParameters);
        if (table.Rows.Count == 0)
        {
            LogDebug($"Empty input list {reportParameter}.");
            return [];
        }

        // build list
        return BuildInputList(table, reportParameter, identifierFunc, displayFunc, selectFunc);
    }

    /// <summary>
    /// Build report parameter input list
    /// </summary>
    /// <remarks>Use the query order-by for sorting</remarks>
    /// <param name="table">List data table</param>
    /// <param name="reportParameter">Report parameter name</param>
    /// <param name="identifierFunc">Identifier function</param>
    /// <param name="displayFunc">Display function: return null to use the identifier for display</param>
    /// <param name="selectFunc">Selection function: return the selected identifier or null</param>
    /// <returns>List of identifiers, selected by the identifier function</returns>
    public List<object> BuildInputList(
            DataTable table,
            string reportParameter,
            Func<DataRow, object> identifierFunc,
            Func<DataRow, string> displayFunc = null,
            Func<List<object>, object> selectFunc = null)
    {
        if (table == null)
        {
            throw new ArgumentNullException(nameof(table));
        }
        if (string.IsNullOrWhiteSpace(reportParameter))
        {
            throw new ArgumentException(nameof(reportParameter));
        }
        if (identifierFunc == null)
        {
            throw new ArgumentNullException(nameof(identifierFunc));
        }
        if (table.Rows.Count == 0)
        {
            LogDebug($"Empty input list {reportParameter}.");
            return [];
        }

        // existing list
        var listAttribute = GetParameterAttribute<string>(reportParameter, InputAttributes.List);
        var listValueAttribute = GetParameterAttribute<string>(reportParameter, InputAttributes.ListValues);
        if (!string.IsNullOrWhiteSpace(listAttribute) && !string.IsNullOrWhiteSpace(listValueAttribute))
        {
            var values = JsonSerializer.Deserialize<List<object>>(listValueAttribute);
            LogDebug($"Restore list {reportParameter}: {values.Count} count.");
            return values;
        }

        // lists
        var list = new List<string>();
        var identifiers = new List<object>();
        foreach (var dataRow in table.AsEnumerable())
        {
            var identifier = identifierFunc(dataRow);
            identifiers.Add(identifier);
            // display text with fallback to the identifier
            var display = displayFunc != null ? displayFunc(dataRow) : $"{identifier}";
            list.Add(display);
        }

        // single row list: no input list
        if (identifiers.Count == 1)
        {
            var selected = list.First();
            SetParameter(reportParameter, selected);
            // read-only
            SetParameterAttribute(reportParameter, InputAttributes.ValueReadOnly, true);
            LogDebug($"Single list selection {reportParameter}: {selected}.");
            return identifiers;
        }

        // parameter input attributes
        SetParameterAttribute(reportParameter, InputAttributes.List,
            JsonSerializer.Serialize(list));
        SetParameterAttribute(reportParameter, InputAttributes.ListValues,
            JsonSerializer.Serialize(identifiers));

        // selection
        var selectIdentifier = identifiers.Count == 1 ?
            // single row list
            identifiers.First() :
            // custom selector
            selectFunc?.Invoke(identifiers);
        if (selectIdentifier != null && identifiers.Contains(selectIdentifier))
        {
            var index = identifiers.IndexOf(selectIdentifier);
            var selection = list[index];
            LogDebug($"Set list selection {reportParameter}: {selection}.");
            // set parameter value
            SetParameter(reportParameter, selectIdentifier);
            // set selected display value
            SetParameterAttribute(reportParameter, InputAttributes.ListSelection, $"{selection}");
        }

        // result
        LogDebug($"New list {reportParameter}: {string.Join(',', identifiers)}.");
        return identifiers;
    }

    #endregion
    
    #region Validation

    /// <summary>
    /// Set report build to valid
    /// </summary>
    public void BuildValid() => BuildValidity(true);

    /// <summary>
    /// Set report build to invalid
    /// </summary>
    public void BuildInvalid() => BuildValidity(false);

    /// <summary>
    /// Set report build validity
    /// </summary>
    /// <param name="valid">Build validity</param>
    public void BuildValidity(bool valid) =>
        SetReportAttribute(InputAttributes.Validity, valid);

    #endregion

    /// <summary>Entry point for the runtime</summary>
    /// <remarks>Internal usage only, do not call this method</remarks>
    public bool? Build()
    {
        // ReSharper disable EmptyRegion
        #region Function
        #endregion
        // ReSharper restore EmptyRegion
        // compiler will optimize this out if the code provides a return
        return null;
    }
}