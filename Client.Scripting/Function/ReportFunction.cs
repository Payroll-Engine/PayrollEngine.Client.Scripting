/* ReportFunction */

using System;
using System.Data;
using System.Linq;
using System.Text.Json;
using System.Collections.Generic;
using PayrollEngine.Client.Scripting.Report;

namespace PayrollEngine.Client.Scripting.Function;

#region Data

/// <summary>Represents the API LookupValueData object</summary>
// ReSharper disable once ClassNeverInstantiated.Local
public sealed class LookupValueData
{
    /// <summary>The lookup key</summary>
    // ReSharper disable once UnusedAutoPropertyAccessor.Local
    public string Key { get; set; }

    /// <summary>The lookup value as JSON</summary>
    // ReSharper disable once UnusedAutoPropertyAccessor.Local
    public string Value { get; set; }
}

/// <summary>Case value column</summary>
public class CaseValueColumn
{
    /// <summary>The column name</summary>
    public string Name { get; }

    /// <summary>The lookup name</summary>
    public string LookupName { get; }

    /// <summary>The lookup type</summary>
    public Type LookupType { get; }

    /// <summary>New instance of case value column</summary>
    /// <param name="name">The column name</param>
    public CaseValueColumn(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException(nameof(name));
        }
        Name = name;
    }

    /// <summary>New instance of case value column</summary>
    /// <param name="name">The column name</param>
    /// <param name="lookupName">The lookup name</param>
    /// <param name="lookupType">The lookup type</param>
    public CaseValueColumn(string name, string lookupName, Type lookupType = null) :
        this(name)
    {
        if (string.IsNullOrWhiteSpace(lookupName))
        {
            throw new ArgumentException(nameof(lookupName));
        }
        LookupName = lookupName;
        LookupType = lookupType ?? typeof(string);
    }
}

#endregion

/// <summary>Base class for report functions</summary>
// ReSharper disable once PartialTypeWithSinglePart
public abstract partial class ReportFunction : Function
{
    /// <summary>Initializes a new instance with the function runtime</summary>
    /// <param name="runtime">The runtime</param>
    protected ReportFunction(object runtime) :
        base(runtime)
    {
        // report
        ReportName = Runtime.ReportName;
    }

    /// <summary>New function instance without runtime (scripting development)</summary>
    /// <remarks>Use <see cref="Function.GetSourceFileName"/> in your constructor for the source file name</remarks>
    /// <param name="sourceFileName">The name of the source file</param>
    protected ReportFunction(string sourceFileName) :
        base(sourceFileName)
    {
    }

    #region Report

    /// <summary>Gets the report name</summary>
    /// <value>The name of the case</value>
    public string ReportName { get; }

    /// <summary>Get report attribute value</summary>
    /// <param name="attributeName">Name of the attribute</param>
    /// <returns>The report attribute value</returns>
    public object GetReportAttribute(string attributeName) =>
        Runtime.GetReportAttribute(attributeName);

    /// <summary>Get report attribute typed value</summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="attributeName">Name of the attribute</param>
    /// <param name="defaultValue">The default value</param>
    /// <returns>The report attribute value</returns>
    public T GetReportAttribute<T>(string attributeName, T defaultValue = default) =>
        ChangeValueType(GetReportAttribute(attributeName), defaultValue);

    /// <summary>Set report attribute value</summary>
    /// <param name="attributeName">Name of the attribute</param>
    /// <param name="value">Attribute value</param>
    public void SetReportAttribute(string attributeName, object value) =>
        Runtime.SetReportAttribute(attributeName, value);

    /// <summary>Check for existing report parameter</summary>
    /// <param name="parameterName">The parameter name</param>
    public bool HasParameter(string parameterName) =>
        Runtime.HasParameter(parameterName);

    /// <summary>Get report parameter</summary>
    /// <param name="parameterName">The parameter name</param>
    /// <returns>The report parameter value as JSON</returns>
    public string GetParameter(string parameterName) =>
        Runtime.GetParameter(parameterName);

    /// <summary>Get report parameter typed value</summary>
    /// <param name="parameterName">The parameter name</param>
    /// <param name="defaultValue">The default value</param>
    /// <returns>The report parameter value</returns>
    public T GetParameter<T>(string parameterName, T defaultValue = default) =>
        ChangeValueType(GetParameter(parameterName), defaultValue);

    /// <summary>Get report parameter attribute value</summary>
    /// <param name="parameterName">The parameter name</param>
    /// <param name="attributeName">Name of the attribute</param>
    /// <returns>The report attribute value</returns>
    public object GetParameterAttribute(string parameterName, string attributeName) =>
        Runtime.GetParameterAttribute(parameterName, attributeName);

    /// <summary>Get report parameter attribute typed value</summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="parameterName">The parameter name</param>
    /// <param name="attributeName">Name of the attribute</param>
    /// <param name="defaultValue">The default value</param>
    /// <returns>The report attribute value</returns>
    public T GetParameterAttribute<T>(string parameterName, string attributeName, T defaultValue = default) =>
        ChangeValueType(GetParameterAttribute(parameterName, attributeName), defaultValue);

    /// <summary>Set report attribute value</summary>
    /// <param name="parameterName">The parameter name</param>
    /// <param name="attributeName">Name of the attribute</param>
    /// <param name="value">The attribute value</param>
    /// <returns>The report attribute value</returns>
    public void SetParameterAttribute(string parameterName, string attributeName, object value) =>
        Runtime.SetParameterAttribute(parameterName, attributeName, value);

    /// <summary>Test for hidden report parameter</summary>
    /// <param name="parameterName">The parameter name</param>
    /// <returns>True for hidden report attribute</returns>
    public bool ParameterHidden(string parameterName) =>
        Runtime.ParameterHidden(parameterName);

    /// <summary>Add report log</summary>
    /// <param name="message">The log message</param>
    /// <param name="key">The log key</param>
    /// <param name="reportDate">The report date (default: now)</param>
    public void AddReportLog(string message, string key = null, DateTime? reportDate = null) =>
        Runtime.AddReportLog(message, key, reportDate);

    #endregion

    #region Message

    /// <summary>Create report message</summary>
    /// <param name="message">The error message</param>
    public string ToReportMessage(string message) =>
        $"{ReportName}: {FirstCharacterToLower(message)}";

    /// <summary>Ensures first string character is lower</summary>
    /// <param name="value">The string value</param>
    /// <returns>String starting lowercase</returns>
    private static string FirstCharacterToLower(string value) =>
        char.ToLowerInvariant(value[0]) + value.Substring(1);

    #endregion

    #region Query

    /// <summary>Query on Api web method with the user culture</summary>
    /// <param name="tableName">Target table name</param>
    /// <param name="methodName">The query name</param>
    /// <param name="parameters">The method parameters</param>
    /// <returns>New data table, null on empty collection</returns>
    public DataTable ExecuteQuery(string tableName, string methodName, Dictionary<string, string> parameters = null) =>
        ExecuteQuery(tableName, methodName, UserCulture, parameters);

    /// <summary>Query on Api web method</summary>
    /// <param name="tableName">Target table name</param>
    /// <param name="methodName">The query name</param>
    /// <param name="culture">The content culture</param>
    /// <param name="parameters">The method parameters</param>
    /// <returns>New data table, null on empty collection</returns>
    public DataTable ExecuteQuery(string tableName, string methodName, string culture, Dictionary<string, string> parameters = null) =>
        Runtime.ExecuteQuery(tableName, methodName, culture, parameters);

    /// <summary>Execute a query on the Api web method, table name extracted from method name</summary>
    /// <param name="methodName">The query name</param>
    /// <param name="parameters">The method parameters</param>
    /// <returns>New data table, null on empty collection</returns>
    public DataTable ExecuteQuery(string methodName, Dictionary<string, string> parameters = null) =>
        ExecuteQuery(GetOperationBaseName(methodName), methodName, UserCulture, parameters);

    /// <summary>Query the division by id</summary>
    /// <param name="divisionId">The division id</param>
    /// <returns>Single row table</returns>
    public DataTable ExecuteDivisionQuery(int divisionId)
    {
        var division = ExecuteQuery("GetDivision",
            new QueryParameters()
                .Parameter(nameof(TenantId), TenantId)
                .Parameter(nameof(divisionId), divisionId));
        if (!division.IsSingleRow())
        {
            throw new ScriptException(ToReportMessage($"Missing division with id {divisionId}."));
        }
        return division;
    }

    /// <summary>Query the regulation id from the parameter RegulationId</summary>
    /// <param name="payrollId">The payroll id</param>
    /// <returns>The regulation id, null for unknown regulation</returns>
    public int? ExecutePayrollDivisionIdQuery(int payrollId)
    {
        var divisionId = ExecuteValueQuery<int?>("QueryPayrolls", "DivisionId",
            new QueryParameters()
                .ActiveStatus()
                .Parameter(nameof(TenantId), TenantId)
                .Id(payrollId));
        return divisionId;
    }

    /// <summary>Query the regulation id from the parameter RegulationId</summary>
    /// <param name="payrollId">The payroll id</param>
    /// <returns>Single row table</returns>
    public DataTable ExecutePayrollDivisionQuery(int payrollId)
    {
        var divisionId = ExecutePayrollDivisionIdQuery(payrollId);
        return !divisionId.HasValue ? null : ExecuteDivisionQuery(divisionId.Value);
    }

    /// <summary>Query Json lookup values by lookup name</summary>
    /// <param name="regulationId">The regulation id</param>
    /// <param name="lookupName">The lookup name</param>
    /// <param name="keyAttribute">The json object key attribute name</param>
    /// <param name="valueAttribute">The json object value attribute name</param>
    /// <returns>The lookup values dictionary</returns>
    public Dictionary<string, string> ExecuteLookupValueQuery(int regulationId, string lookupName, string keyAttribute, string valueAttribute) =>
        Runtime.ExecuteLookupValueQuery(regulationId, lookupName, keyAttribute, valueAttribute);

    /// <summary>Execute global case value query on the Api web method</summary>
    /// <param name="tableName">Target table name</param>
    /// <param name="query">The query</param>
    /// <returns>Resulting data table, existing will be removed</returns>
    public DataTable ExecuteGlobalCaseValueQuery(string tableName, ReportQuery query = null) =>
     Runtime.ExecuteGlobalCaseValueQuery(tableName, ReportQueryToTuple(query));

    /// <summary>Execute national case value query on the Api web method</summary>
    /// <param name="tableName">Target table name</param>
    /// <param name="query">The query</param>
    /// <returns>Resulting data table, existing will be removed</returns>
    public DataTable ExecuteNationalCaseValueQuery(string tableName, ReportQuery query = null) =>
        Runtime.ExecuteNationalCaseValueQuery(tableName, ReportQueryToTuple(query));

    /// <summary>Execute company case value query on the Api web method</summary>
    /// <param name="tableName">Target table name</param>
    /// <param name="query">The query</param>
    /// <returns>Resulting data table, existing will be removed</returns>
    public DataTable ExecuteCompanyCaseValueQuery(string tableName, ReportQuery query = null) =>
        Runtime.ExecuteCompanyCaseValueQuery(tableName, ReportQueryToTuple(query));

    /// <summary>Execute employee case value query on the Api web method</summary>
    /// <param name="tableName">Target table name</param>
    /// <param name="employeeId">The employee id</param>
    /// <param name="query">The query</param>
    /// <returns>Resulting data table, existing will be removed</returns>
    public DataTable ExecuteEmployeeCaseValueQuery(string tableName, int employeeId, ReportQuery query = null) =>
        Runtime.ExecuteEmployeeCaseValueQuery(tableName, employeeId, ReportQueryToTuple(query));

    /// <summary>Query regulation wage</summary>
    /// <param name="tableName">Target table name</param>
    /// <param name="regulationId">The regulation id</param>
    /// <param name="query">The query</param>
    /// <returns>Wage type data table, null on empty collection</returns>
    public DataTable ExecuteWageTypeQuery(string tableName, int regulationId, ReportQuery query = null) =>
        Runtime.ExecuteWageTypeQuery(tableName, regulationId, ReportQueryToTuple(query));

    /// <summary>Query payroll results</summary>
    /// <param name="tableName">Target table name</param>
    /// <param name="query">The query</param>
    /// <returns>Payroll result data table, null on empty collection</returns>
    public DataTable ExecutePayrollResultQuery(string tableName, ReportQuery query = null) =>
        Runtime.ExecutePayrollResultQuery(tableName, ReportQueryToTuple(query));

    /// <summary>Query wage type results</summary>
    /// <param name="tableName">Target table name</param>
    /// <param name="payrollResultId">The payroll result id</param>
    /// <param name="query">The query</param>
    /// <returns>Wage type result data table, null on empty collection</returns>
    public DataTable ExecuteWageTypeResultQuery(string tableName, int payrollResultId, ReportQuery query = null) =>
        Runtime.ExecuteWageTypeResultQuery(tableName, payrollResultId, ReportQueryToTuple(query));

    /// <summary>Query wage type custom results</summary>
    /// <param name="tableName">Target table name</param>
    /// <param name="payrollResultId">The payroll result id</param>
    /// <param name="wageTypeResultId">The wage type result id</param>
    /// <param name="query">The query</param>
    /// <returns>Wage type custom result data table, null on empty collection</returns>
    public DataTable ExecuteWageTypeCustomResultQuery(string tableName, int payrollResultId, int wageTypeResultId, ReportQuery query = null) =>
        Runtime.ExecuteWageTypeCustomResultQuery(tableName, payrollResultId, wageTypeResultId, ReportQueryToTuple(query));

    /// <summary>Query collector results</summary>
    /// <param name="tableName">Target table name</param>
    /// <param name="payrollResultId">The payroll result id</param>
    /// <param name="query">The query</param>
    /// <returns>Collector result data table, null on empty collection</returns>
    public DataTable ExecuteCollectorResultQuery(string tableName, int payrollResultId, ReportQuery query = null) =>
        Runtime.ExecuteCollectorResultQuery(tableName, payrollResultId, ReportQueryToTuple(query));

    /// <summary>Query collector custom results</summary>
    /// <param name="tableName">Target table name</param>
    /// <param name="payrollResultId">The payroll result id</param>
    /// <param name="collectorResultId">The collector result id</param>
    /// <param name="query">The query</param>
    /// <returns>Collector custom result data table, null on empty collection</returns>
    public DataTable ExecuteCollectorCustomResultQuery(string tableName, int payrollResultId, int collectorResultId, ReportQuery query = null) =>
        Runtime.ExecuteCollectorCustomResultQuery(tableName, payrollResultId, collectorResultId, ReportQueryToTuple(query));

    /// <summary>Query payrun results</summary>
    /// <param name="tableName">Target table name</param>
    /// <param name="payrollResultId">The payroll result id</param>
    /// <param name="query">The query</param>
    /// <returns>Payrun result data table, null on empty collection</returns>
    public DataTable ExecutePayrunResultQuery(string tableName, int payrollResultId, ReportQuery query = null) =>
        Runtime.ExecutePayrunResultQuery(tableName, payrollResultId, ReportQueryToTuple(query));

    private static Tuple<int?, string, string, string, long?, long?> ReportQueryToTuple(ReportQuery query)
    {
        if (query == null)
        {
            return new(null, null, null, null, null, null);
        }
        return new((int?)query.Status, query.Filter, query.OrderBy, query.Select, query.Top, query.Skip);
    }

    // see als PayrollEngine.Api.Core.ApiOperationTool.GetOperationBaseName()
    /// <summary>Get operation base name</summary>
    /// <param name="operation">The operation name</param>
    /// <returns>Operation base name</returns>
    public static string GetOperationBaseName(string operation)
    {
        if (string.IsNullOrWhiteSpace(operation))
        {
            throw new ArgumentException(nameof(operation));
        }
        if (operation.StartsWith("Query"))
        {
            return operation.RemoveFromStart("Query");
        }
        return operation.StartsWith("Get") ?
            operation.RemoveFromStart("Get") :
            operation;
    }

    /// <summary>Execute a value query on the Api web method</summary>
    /// <param name="methodName">The query name</param>
    /// <param name="attributeName">Name of the attribute</param>
    /// <param name="parameters">The method parameters</param>
    /// <returns>Resulting data table, existing will be removed</returns>
    public T ExecuteValueQuery<T>(string methodName, string attributeName, Dictionary<string, string> parameters)
    {
        if (string.IsNullOrWhiteSpace(attributeName))
        {
            throw new ArgumentException(nameof(attributeName));
        }
        var tableName = $"Temp{methodName}{attributeName}";
        var result = ExecuteQuery(tableName, methodName, parameters);
        if (result == null)
        {
            throw new ScriptException($"Empty result on single result value query on table {tableName}.");
        }
        if (result.Rows.Count > 1)
        {
            throw new ScriptException($"Multiple results on single result value query on table {tableName} and attribute {attributeName}.");
        }
        return result.Rows[0].GetValue<T>(attributeName);
    }

    /// <summary>Execute a value query on the Api web method</summary>
    /// <param name="methodName">The query name</param>
    /// <param name="attributeName">Name of the attribute</param>
    /// <param name="parameters">The method parameters</param>
    /// <param name="defaultValue">The method parameters</param>
    /// <returns>Resulting data table, existing will be removed</returns>
    public T ExecuteValueQuery<T>(string methodName, string attributeName,
        Dictionary<string, string> parameters, T defaultValue)
    {
        if (string.IsNullOrWhiteSpace(attributeName))
        {
            throw new ArgumentException(nameof(attributeName));
        }
        var tableName = $"Temp{methodName}{attributeName}";
        var result = ExecuteQuery(tableName, methodName, parameters);
        if (result == null || result.Rows.Count != 1)
        {
            return defaultValue;
        }
        return result.Rows[0].GetValue(attributeName, defaultValue);
    }

    #endregion

    #region Lookups

    /// <summary>Get lookup values, grouped by lookup</summary>
    /// <param name="payrollId">The payroll id</param>
    /// <param name="culture">The culture</param>
    /// <param name="lookupName">The lookup name</param>
    /// <param name="regulationDate">The regulation date</param>
    /// <param name="evaluationDate">The evaluation date</param>
    /// <returns>Lookup values dictionary by lookup name, value is a key/value dictionary</returns>
    /// <code>
    /// var lookup = ExecuteLookupQuery(1, "MyLookupName", Language.Italian);
    /// var lookupValue = lookup["MyLookupKey"];
    /// </code>
    public Dictionary<string, string> ExecuteLookupQuery(int payrollId,
        string lookupName, string culture,
        DateTime? regulationDate = null, DateTime? evaluationDate = null)
    {
        var lookups = ExecuteLookupQuery(payrollId, [lookupName],
            culture, regulationDate, evaluationDate);
        return lookups.TryGetValue(lookupName, out var lookup) ? lookup : new();
    }

    /// <summary>Get lookup values, grouped by lookup</summary>
    /// <param name="payrollId">The payroll id</param>
    /// <param name="culture">The culture</param>
    /// <param name="lookupNames">The lookup names</param>
    /// <param name="regulationDate">The regulation date</param>
    /// <param name="evaluationDate">The evaluation date</param>
    /// <returns>Lookup values dictionary by lookup name, value is a key/value dictionary</returns>
    public Dictionary<string, Dictionary<string, string>> ExecuteLookupQuery(int payrollId,
        IEnumerable<string> lookupNames, string culture,
        DateTime? regulationDate = null, DateTime? evaluationDate = null)
    {
        if (payrollId <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(payrollId));
        }
        if (lookupNames == null)
        {
            throw new ArgumentNullException(nameof(lookupNames));
        }
        var names = new HashSet<string>(lookupNames);
        if (!names.Any())
        {
            throw new ArgumentException("Missing lookup names", nameof(lookupNames));
        }

        // query parameters
        var parameters = new Dictionary<string, string>
        {
            {"TenantId", TenantId.ToString()},
            {"PayrollId", payrollId.ToString()},
            // fallback culture
            {"Culture", culture ?? UserCulture},
            {"LookupNames", JsonSerializer.Serialize(names)}
        };
        if (regulationDate.HasValue)
        {
            parameters.Add("RegulationDate", regulationDate.Value.ToUtcString());
        }
        if (evaluationDate.HasValue)
        {
            parameters.Add("EvaluationDate", evaluationDate.Value.ToUtcString());
        }

        // lookup values
        var lookups = new Dictionary<string, Dictionary<string, string>>();
        DataTable lookupValueTable = ExecuteQuery("LookupValues", "GetPayrollLookupData", parameters);
        foreach (var lookupValuesRow in lookupValueTable.AsEnumerable())
        {
            var lookupName = lookupValuesRow.GetValue<string>("Name");
            // unknown lookup
            if (string.IsNullOrWhiteSpace(lookupName))
            {
                continue;
            }
            // duplicated lookup
            if (lookups.ContainsKey(lookupName))
            {
                throw new ScriptException($"Duplicated lookup {lookupName}.");
            }
            var values = lookupValuesRow.GetValue<LookupValueData[]>("Values");
            if (values != null)
            {
                lookups.Add(lookupName, values.ToDictionary(x => x.Key, x => x.Value));
            }
        }
        return lookups;
    }

    #endregion

    #region Cases

    /// <summary>Get payroll case fields</summary>
    /// <param name="payrollId">The payroll id</param>
    /// <param name="caseFieldNames">The case field names</param>
    /// <param name="clusterSetName">The cluster set</param>
    /// <param name="regulationDate">The regulation date</param>
    /// <param name="evaluationDate">The evaluation date</param>
    /// <returns>Payroll case fields</returns>
    public DataTable ExecuteCaseFieldQuery(int payrollId, IEnumerable<string> caseFieldNames = null,
        string clusterSetName = null, DateTime? regulationDate = null, DateTime? evaluationDate = null)
    {
        if (caseFieldNames == null && string.IsNullOrWhiteSpace(clusterSetName))
        {
            throw new ScriptException("Case field query requires case fields or a cluster set.");
        }

        var parameters = new Dictionary<string, string>
        {
            {"TenantId", TenantId.ToString()},
            {"PayrollId", payrollId.ToString()}
        };
        if (caseFieldNames != null)
        {
            parameters.Add("CaseFieldNames", JsonSerializer.Serialize(caseFieldNames));
        }
        if (!string.IsNullOrWhiteSpace(clusterSetName))
        {
            parameters.Add("ClusterSetName", clusterSetName);
        }
        if (regulationDate.HasValue)
        {
            parameters.Add("RegulationDate", regulationDate.Value.ToUtcString());
        }
        if (evaluationDate.HasValue)
        {
            parameters.Add("EvaluationDate", evaluationDate.Value.ToUtcString());
        }
        return ExecuteQuery("CaseFields", "GetPayrollCaseFields", parameters);
    }

    /// <summary>Get employees case values as table.
    /// Table structure: first column is the employee id, and for any case field a column</summary>
    /// <param name="payrollId">The payroll id</param>
    /// <param name="employeeIds">The employee ids</param>
    /// <param name="tableName">The table name</param>
    /// <param name="columns">The table columns</param>
    /// <param name="culture">The culture</param>
    /// <param name="valueDate">The value date</param>
    /// <param name="regulationDate">The regulation date</param>
    /// <param name="evaluationDate">The evaluation date</param>
    /// <returns>Employees case values</returns>
    public DataTable ExecuteEmployeeTimeCaseValueQuery(string tableName, int payrollId,
        IEnumerable<int> employeeIds, IEnumerable<CaseValueColumn> columns,
        string culture, DateTime? valueDate = null,
        DateTime? regulationDate = null, DateTime? evaluationDate = null)
    {
        if (string.IsNullOrWhiteSpace(tableName))
        {
            throw new ArgumentException(nameof(tableName));
        }
        if (payrollId <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(payrollId));
        }
        if (employeeIds == null)
        {
            throw new ArgumentNullException(nameof(employeeIds));
        }
        if (columns == null)
        {
            throw new ArgumentNullException(nameof(columns));
        }
        var columnList = columns.ToList();
        if (!columnList.Any())
        {
            throw new ArgumentException(nameof(columns));
        }

        // fallback culture
        culture ??= UserCulture;

        // columns
        var columnNames = new HashSet<string>(columnList.Select(x => x.Name));

        // lookups
        var lookups = GetLookups(payrollId, culture, columnList, regulationDate, evaluationDate);

        // result table
        DataTable caseValuesTable = new(tableName);

        // table columns
        caseValuesTable.Columns.Add("EmployeeId", typeof(int));
        var caseFields = ExecuteCaseFieldQuery(payrollId, columnNames,
            regulationDate: regulationDate, evaluationDate: evaluationDate);
        SetupColumns(columnList, caseFields, caseValuesTable);

        // employee case values rows
        foreach (var employeeId in employeeIds)
        {
            // case values
            var parameters = new Dictionary<string, string>
            {
                {"TenantId", TenantId.ToString()},
                {"PayrollId", payrollId.ToString()},
                {"EmployeeId", employeeId.ToString()},
                {"CaseType", nameof(CaseType.Employee)},
                // fallback culture
                {"Culture", culture ?? UserCulture},
                {"CaseFieldNames", JsonSerializer.Serialize(columnNames)}
            };
            if (valueDate.HasValue)
            {
                parameters.Add("ValueDate", valueDate.Value.ToUtcString());
            }
            if (regulationDate.HasValue)
            {
                parameters.Add("RegulationDate", regulationDate.Value.ToUtcString());
            }
            if (evaluationDate.HasValue)
            {
                parameters.Add("EvaluationDate", evaluationDate.Value.ToUtcString());
            }
            var caseTimeValuesTable = ExecuteQuery("CaseValues", "GetPayrollTimeCaseValues", parameters);
            if (caseTimeValuesTable.Rows.Count == 0)
            {
                return caseValuesTable;
            }

            // case values row
            var caseValuesRow = GetCaseValueRow(employeeId, columnList, caseValuesTable, caseTimeValuesTable, lookups);
            caseValuesTable.Rows.Add(caseValuesRow);
        }

        return caseValuesTable;
    }

    private Dictionary<string, Dictionary<string, string>> GetLookups(int payrollId, string culture, List<CaseValueColumn> columns,
        DateTime? regulationDate, DateTime? evaluationDate)
    {
        var lookups = new Dictionary<string, Dictionary<string, string>>();
        var lookupNames = columns.Where(x => !string.IsNullOrWhiteSpace(x.LookupName)).Select(x => x.LookupName).ToList();
        if (lookupNames.Any())
        {
            lookups = ExecuteLookupQuery(payrollId, lookupNames, culture, regulationDate, evaluationDate);
        }
        return lookups;
    }

    private DataRow GetCaseValueRow(int employeeId, List<CaseValueColumn> columns, DataTable caseValuesTable,
        DataTable caseTimeValuesTable, Dictionary<string, Dictionary<string, string>> lookups)
    {
        var caseValuesRow = caseValuesTable.NewRow();
        foreach (var timeValueRow in caseTimeValuesTable.AsEnumerable())
        {
            // case field name and column name
            var caseFieldName = timeValueRow.GetValue<string>("CaseFieldName");
            if (string.IsNullOrWhiteSpace(caseFieldName))
            {
                continue;
            }

            // column
            var column = columns.FirstOrDefault(x => string.Equals(x.Name, caseFieldName));
            if (column == null)
            {
                continue;
            }

            DataColumn dataColumn = caseValuesTable.Columns[column.Name];
            if (dataColumn == null)
            {
                continue;
            }

            // employee id
            caseValuesRow["EmployeeId"] = employeeId;

            // value
            var jsonValue = timeValueRow.GetValue<string>("Value");
            // lookup value
            if (!string.IsNullOrWhiteSpace(column.LookupName))
            {
                if (lookups.ContainsKey(column.LookupName) &&
                    lookups[column.LookupName].ContainsKey(jsonValue))
                {
                    jsonValue = lookups[column.LookupName][jsonValue];
                }
                else
                {
                    jsonValue = string.Empty;
                }
            }

            // value type
            var valueType = timeValueRow.GetValue<int>("ValueType");
            if (Enum.IsDefined(typeof(ValueType), valueType))
            {
                var value = ((ValueType)valueType).JsonToValue(jsonValue);
                caseValuesRow[column.Name] = Convert.ChangeType(value, dataColumn.DataType);
            }
        }

        return caseValuesRow;
    }

    private static void SetupColumns(List<CaseValueColumn> columnList, DataTable caseFields, DataTable caseValuesTable)
    {
        foreach (var column in columnList)
        {
            // find case field row by column name, equals to case field name
            DataRow caseFieldRow = GetCaseFieldRow(caseFields, column);
            if (caseFieldRow == null)
            {
                throw new ScriptException($"Unknown case field {column.Name}.");
            }

            // column type
            var valueType = GetCaseValueType(caseFieldRow);
            if (!valueType.HasValue)
            {
                throw new ScriptException(
                    $"Unknown case field type in column {column.Name} (enum: {typeof(ValueType)}).");
            }

            // add column
            caseValuesTable.Columns.Add(column.Name, valueType.Value.GetDataType());
        }
    }

    private static ValueType? GetCaseValueType(DataRow caseFieldRow)
    {
        var rawValueType = caseFieldRow["ValueType"];
        return rawValueType switch
        {
            string stringValue => (ValueType)Enum.Parse(typeof(ValueType), stringValue),
            int intValue when Enum.IsDefined(typeof(ValueType), intValue) => (ValueType)rawValueType,
            _ => null
        };
    }

    private static DataRow GetCaseFieldRow(DataTable caseFields, CaseValueColumn column)
    {
        foreach (var caseFieldRow in caseFields.AsEnumerable())
        {
            if (string.Equals(caseFieldRow["Name"] as string, column.Name))
            {
                return caseFieldRow;
            }
        }
        return null;
    }

    #endregion

    #region Case Values

    /// <summary>
    /// Execute case value query on period
    /// </summary>
    /// <param name="tableName">Table name</param>
    /// <param name="payrollId">Payroll id</param>
    /// <param name="caseFieldNames">The case field names</param>
    /// <param name="period">Value period</param>
    /// <param name="evaluationDate">The evaluation date (default: UTC now)</param>
    /// <param name="regulationDate">The regulation date (default: UTC now)</param>
    /// <returns>Data table including for any case filed a column</returns>
    public DataTable ExecuteRawCaseValueQuery(string tableName, int payrollId,
        IEnumerable<string> caseFieldNames, DatePeriod period,
        DateTime? regulationDate = null, DateTime? evaluationDate = null) =>
        ExecuteRawCaseValueQuery(tableName, payrollId, employeeId: 0, caseFieldNames,
            period.Start, period.End, regulationDate, evaluationDate);

    /// <summary>
    /// Execute employee case value query on period
    /// </summary>
    /// <param name="tableName">Table name</param>
    /// <param name="payrollId">Payroll id</param>
    /// <param name="employeeId">Employee id</param>
    /// <param name="caseFieldNames">The case field names</param>
    /// <param name="period">Value period</param>
    /// <param name="evaluationDate">The evaluation date (default: UTC now)</param>
    /// <param name="regulationDate">The regulation date (default: UTC now)</param>
    /// <returns>Data table including for any case filed a column</returns>
    public DataTable ExecuteRawCaseValueQuery(string tableName, int payrollId,
        int employeeId, IEnumerable<string> caseFieldNames, DatePeriod period,
        DateTime? regulationDate = null, DateTime? evaluationDate = null) =>
        ExecuteRawCaseValueQuery(tableName, payrollId, employeeId: employeeId, caseFieldNames,
            period.Start, period.End, regulationDate, evaluationDate);

    /// <summary>
    /// Execute case value query
    /// </summary>
    /// <param name="tableName">Table name</param>
    /// <param name="payrollId">Payroll id</param>
    /// <param name="caseFieldNames">The case field names</param>
    /// <param name="periodStart">Value period start date</param>
    /// <param name="periodEnd">Value period end date</param>
    /// <param name="evaluationDate">The evaluation date (default: UTC now)</param>
    /// <param name="regulationDate">The regulation date (default: UTC now)</param>
    /// <returns>Data table including for any case filed a column</returns>
    public DataTable ExecuteRawCaseValueQuery(string tableName, int payrollId,
        IEnumerable<string> caseFieldNames,
        DateTime? periodStart = null, DateTime? periodEnd = null,
        DateTime? regulationDate = null, DateTime? evaluationDate = null) =>
        ExecuteRawCaseValueQuery(tableName, payrollId, employeeId: 0, caseFieldNames,
            periodStart, periodEnd, regulationDate, evaluationDate);

    /// <summary>
    /// Execute employee case value query
    /// </summary>
    /// <param name="tableName">Table name</param>
    /// <param name="payrollId">Payroll id</param>
    /// <param name="employeeId">Employee id</param>
    /// <param name="caseFieldNames">The case field names</param>
    /// <param name="startDate">Value period start date</param>
    /// <param name="endDate">Value period end  date</param>
    /// <param name="evaluationDate">The evaluation date (default: UTC now)</param>
    /// <param name="regulationDate">The regulation date (default: UTC now)</param>
    /// <returns>Data table including for any case filed a column</returns>
    public DataTable ExecuteRawCaseValueQuery(string tableName, int payrollId,
        int employeeId, IEnumerable<string> caseFieldNames,
        DateTime? startDate = null, DateTime? endDate = null,
        DateTime? regulationDate = null, DateTime? evaluationDate = null)
    {
        if (string.IsNullOrWhiteSpace(tableName))
        {
            throw new ArgumentException(nameof(tableName));
        }

        // table
        var table = new DataTable(tableName);

        // query case values (list of case field names, with column type and case value table)
        var fieldTables = new List<(string CaseFieldName, Type DbType, DataTable Table)>();
        foreach (var caseFieldName in caseFieldNames)
        {
            // query parameter
            var queryParameter = new QueryParameters()
                .Parameter("TenantId", TenantId)
                .Parameter("PayrollId", payrollId)
                .Parameter("CaseFieldNames", new[] { caseFieldName });
            if (employeeId > 0)
            {
                queryParameter = queryParameter.Parameter("EmployeeId", employeeId);
            }
            if (startDate != null)
            {
                queryParameter = queryParameter.Parameter("StartDate", startDate.Value);
            }
            if (endDate != null)
            {
                queryParameter = queryParameter.Parameter("EndDate", endDate.Value);
            }
            if (regulationDate != null)
            {
                queryParameter = queryParameter.Parameter("RegulationDate", regulationDate.Value);
            }
            if (evaluationDate != null)
            {
                queryParameter = queryParameter.Parameter("EvaluationDate", evaluationDate.Value);
            }

            // query payroll case values
            var fieldTable = ExecuteQuery("GetPayrollCaseValues", queryParameter);
            if (!fieldTable.Any())
            {
                continue;
            }
            var dbType = fieldTable.Rows[0].GetPayrollValueType().GetDataType();
            fieldTables.Add(new(caseFieldName, dbType, fieldTable));
        }
        if (!fieldTables.Any())
        {
            return table;
        }

        // case values (dictionary of case field name/value, grouped by creation date)
        var caseValuesByDate = GetCaseValuesByDate(fieldTables);
        if (!caseValuesByDate.Any())
        {
            return table;
        }

        // columns (date and case field columns)
        table.Columns.Add("Date", typeof(DateTime));
        foreach (var fieldTable in fieldTables)
        {
            if (!table.Columns.Contains(fieldTable.CaseFieldName))
            {
                table.Columns.Add(fieldTable.CaseFieldName, fieldTable.DbType);
            }
        }

        // rows ordered by date
        var dates = caseValuesByDate.Keys.Order().ToList();
        foreach (var date in dates)
        {
            var row = table.NewRow();
            var caseValues = caseValuesByDate[date];
            row["Date"] = date;
            foreach (var caseValue in caseValues)
            {
                row[caseValue.Key] = caseValue.Value;
            }
            table.Rows.Add(row);
        }

        return table;
    }

    private static Dictionary<DateTime, Dictionary<string, object>> GetCaseValuesByDate(
        List<(string CaseFieldName, Type DbType, DataTable Table)> fieldTables)
    {
        // case values (dictionary of case field name/value, grouped by creation date)
        var values = new Dictionary<DateTime, Dictionary<string, object>>();
        foreach (var fieldTable in fieldTables)
        {
            foreach (var dataRow in fieldTable.Table.AsEnumerable())
            {
                var value = dataRow.GetPayrollValue();
                if (value == null)
                {
                    continue;
                }
                var date = dataRow.GetValue<DateTime>("Created");
                if (!values.ContainsKey(date))
                {
                    values.Add(date, new());
                }
                values[date][fieldTable.CaseFieldName] = value;
            }
        }
        return values;
    }

    #endregion

    #region Report Parameters

    /// <summary>Resolve the user id from the parameter UserId by id or identifier</summary>
    /// <returns>The employee id, null for unknown user</returns>
    public int? ResolveParameterUserId() =>
        ResolveIdentifierParameter("QueryUsers", "UserId");

    /// <summary>Resolve the employee id from the parameter EmployeeId by id or identifier</summary>
    /// <returns>The employee id, null for unknown employee</returns>
    public int? ResolveParameterEmployeeId() =>
        ResolveIdentifierParameter("QueryEmployees", "EmployeeId");

    /// <summary>Resolve the regulation id from the parameter RegulationId by id or name</summary>
    /// <returns>The regulation id, null for unknown regulation</returns>
    public int? ResolveParameterRegulationId() =>
        ResolveNameParameter("QueryRegulations", "RegulationId");

    /// <summary>Resolve the payroll id from the parameter PayrollId by id or name</summary>
    /// <returns>The regulation id, null for unknown payroll</returns>
    public int? ResolveParameterPayrollId() =>
        ResolveNameParameter("QueryPayrolls", "PayrollId");

    /// <summary>Resolve the payrun id from the parameter PayrunId by id or name</summary>
    /// <returns>The regulation id, null for unknown payrun</returns>
    public int? ResolveParameterPayrunId() =>
        ResolveNameParameter("QueryPayruns", "PayrunId");

    /// <summary>Resolve the object id from the identifier/id parameter</summary>
    /// <param name="queryName">The query name</param>
    /// <param name="parameterName">The parameter name</param>
    /// <returns>The object id, null for unknown payrun</returns>
    public int? ResolveIdentifierParameter(string queryName, string parameterName)
    {
        if (string.IsNullOrWhiteSpace(queryName))
        {
            throw new ArgumentException(nameof(queryName));
        }
        if (string.IsNullOrWhiteSpace(parameterName))
        {
            throw new ArgumentException(nameof(parameterName));
        }

        // parameter value
        var param = GetParameter(parameterName);
        if (string.IsNullOrWhiteSpace(param))
        {
            LogWarning(ToReportMessage($"Missing parameter {parameterName}"));
            return null;
        }

        // query
        if (!int.TryParse(param, out var id) && !string.IsNullOrWhiteSpace(param))
        {
            var queryId = ExecuteValueQuery<int?>(queryName, "Id",
                new QueryParameters()
                    .ActiveStatus()
                    .Parameter(nameof(TenantId), TenantId)
                    .EqualIdentifier(param));
            if (!queryId.HasValue)
            {
                LogWarning(ToReportMessage($"Error on query {queryName} with identifier {param}"));
                return null;
            }
            id = queryId.Value;
        }
        if (id != 0)
        {
            return id;
        }

        LogWarning(ToReportMessage($"Invalid value for parameter {parameterName}"));
        return null;
    }

    /// <summary>Resolve the object id from the name/id parameter</summary>
    /// <summary>Get the object id from the parameter</summary>
    /// <param name="queryName">The query name</param>
    /// <param name="parameterName">The parameter name</param>
    /// <returns>The object id, null for unknown payrun</returns>
    public int? ResolveNameParameter(string queryName, string parameterName)
    {
        if (string.IsNullOrWhiteSpace(queryName))
        {
            throw new ArgumentException(nameof(queryName));
        }
        if (string.IsNullOrWhiteSpace(parameterName))
        {
            throw new ArgumentException(nameof(parameterName));
        }

        // parameter value
        var param = GetParameter(parameterName);
        if (string.IsNullOrWhiteSpace(param))
        {
            LogWarning(ToReportMessage($"Missing parameter {parameterName}"));
            return null;
        }

        // query
        if (!int.TryParse(param, out var id) && !string.IsNullOrWhiteSpace(param))
        {
            var queryId = ExecuteValueQuery<int?>(queryName, "Id",
                new QueryParameters()
                    .ActiveStatus()
                    .Parameter(nameof(TenantId), TenantId)
                    .EqualName(param));
            if (!queryId.HasValue)
            {
                LogWarning(ToReportMessage($"Error on query {queryName} with name {param}"));
                return null;
            }
            id = queryId.Value;
        }
        if (id != 0)
        {
            return id;
        }

        LogWarning(ToReportMessage($"Invalid value for parameter {parameterName}"));
        return null;
    }

    #endregion

    #region Info

    /// <summary>
    /// Add build info
    /// </summary>
    /// <param name="name">Info name</param>
    /// <param name="value">Info value</param>
    public void AddInfo(string name, object value)
    {
        // info values
        var values = new Dictionary<string, object>();
        var attribute = GetReportAttribute(InputAttributes.EditInfo) as string;
        if (!string.IsNullOrWhiteSpace(attribute))
        {
            values = JsonSerializer.Deserialize<Dictionary<string, object>>(attribute);
        }

        // set/replace value
        values[name] = value;
        SetReportAttribute(InputAttributes.EditInfo, JsonSerializer.Serialize(values));
    }

    /// <summary>
    /// Remove build info
    /// </summary>
    /// <param name="name">Info name</param>
    public void RemoveInfo(string name)
    {
        // info values
        var attribute = GetReportAttribute(InputAttributes.EditInfo) as string;
        if (string.IsNullOrWhiteSpace(attribute))
        {
            return;
        }
        var values = JsonSerializer.Deserialize<Dictionary<string, object>>(attribute);
        if (!values.Remove(name))
        {
            return;
        }

        // remove value
        SetReportAttribute(InputAttributes.EditInfo, values.Count > 0 ? JsonSerializer.Serialize(values) : null);
    }

    #endregion

}