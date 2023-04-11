/* ReportFunction */

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.Json;
using PayrollEngine.Client.Scripting.Report;

namespace PayrollEngine.Client.Scripting.Function;

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
        Language = (Language)Runtime.Language;
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

    /// <summary>Gets the report language</summary>
    public Language Language { get; }

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
    public T GetReportAttribute<T>(string attributeName, T defaultValue = default)
    {
        var value = GetReportAttribute(attributeName);
        return value == null ? defaultValue : (T)Convert.ChangeType(value, typeof(T));
    }

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
    public T GetParameter<T>(string parameterName, T defaultValue = default)
    {
        var value = GetParameter(parameterName);
        return value == null ? defaultValue : (T)Convert.ChangeType(value, typeof(T));
    }

    /// <summary>Get report parameter attribute value</summary>
    /// <param name="attributeName">Name of the attribute</param>
    /// <returns>The report attribute value</returns>
    public object GetParameterAttribute(string attributeName) =>
        Runtime.GetParameterAttribute(attributeName);

    /// <summary>Get report parameter attribute typed value</summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="attributeName">Name of the attribute</param>
    /// <param name="defaultValue">The default value</param>
    /// <returns>The report attribute value</returns>
    public T GetParameterAttribute<T>(string attributeName, T defaultValue = default)
    {
        var value = GetParameterAttribute(attributeName);
        return value == null ? defaultValue : (T)Convert.ChangeType(value, typeof(T));
    }

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

    /// <summary>Query on Api web method</summary>
    /// <param name="tableName">Target table name</param>
    /// <param name="methodName">The query name</param>
    /// <param name="parameters">The method parameters</param>
    /// <returns>New data table, null on empty collection</returns>
    public DataTable ExecuteQuery(string tableName, string methodName, Dictionary<string, string> parameters = null) =>
        ExecuteQuery(tableName, methodName, Language, parameters);

    /// <summary>Query on Api web method</summary>
    /// <param name="tableName">Target table name</param>
    /// <param name="methodName">The query name</param>
    /// <param name="language">The content language</param>
    /// <param name="parameters">The method parameters</param>
    /// <returns>New data table, null on empty collection</returns>
    public DataTable ExecuteQuery(string tableName, string methodName, Language language, Dictionary<string, string> parameters = null) =>
        Runtime.ExecuteQuery(tableName, methodName, (int)language, parameters);

    /// <summary>Execute a query on the Api web method, table name extracted from method name</summary>
    /// <param name="methodName">The query name</param>
    /// <param name="parameters">The method parameters</param>
    /// <returns>New data table, null on empty collection</returns>
    public DataTable ExecuteQuery(string methodName, Dictionary<string, string> parameters = null) =>
        ExecuteQuery(GetOperationBaseName(methodName), methodName, Language, parameters);

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
            throw new ScriptException(ToReportMessage($"Missing division with id {divisionId}"));
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
            throw new ScriptException($"Empty result on single result value query on table {tableName}");
        }
        if (result.Rows.Count > 1)
        {
            throw new ScriptException($"Multiple results on single result value query on table {tableName} and attribute {attributeName}");
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
    /// <param name="language">The language</param>
    /// <param name="lookupName">The lookup name</param>
    /// <param name="regulationDate">The regulation date</param>
    /// <param name="evaluationDate">The evaluation date</param>
    /// <returns>Lookup values dictionary by lookup name, value is a key/value dictionary</returns>
    /// <code>
    /// var lookup = ExecuteLookupQuery(1, "MyLookupName", Language.Italian);
    /// var lookupValue = lookup["MyLookupKey"];
    /// </code>
    public Dictionary<string, string> ExecuteLookupQuery(int payrollId,
        string lookupName, Language language,
        DateTime? regulationDate = null, DateTime? evaluationDate = null)
    {
        var lookups = ExecuteLookupQuery(payrollId, new[] { lookupName },
            language, regulationDate, evaluationDate);
        return lookups.TryGetValue(lookupName, out var lookup) ? lookup : new();
    }

    /// <summary>Get lookup values, grouped by lookup</summary>
    /// <param name="payrollId">The payroll id</param>
    /// <param name="language">The language</param>
    /// <param name="lookupNames">The lookup names</param>
    /// <param name="regulationDate">The regulation date</param>
    /// <param name="evaluationDate">The evaluation date</param>
    /// <returns>Lookup values dictionary by lookup name, value is a key/value dictionary</returns>
    /// <code>
    /// var lookups = ExecuteLookupQuery(1, new[] { "MyLookupName" }, Language.Italian);
    /// var lookupValue = lookups["MyLookupName"]["MyLookupKey"];
    /// </code>
    public Dictionary<string, Dictionary<string, string>> ExecuteLookupQuery(int payrollId,
        IEnumerable<string> lookupNames, Language language,
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
            {"Language", language.ToString()},
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
        DataTable lookupValueTable = ExecuteQuery("LookupValues", "GetPayrollLookupValues", parameters);
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
                throw new ScriptException($"Duplicated lookup {lookupName}");
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

    #region Cases and Case Values

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
            throw new ScriptException("Case field query requires case fields or a cluster set");
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

    /// <summary>Get employees case values as table
    /// Table structure: first column is the employee id, and for any case field a column</summary>
    /// <param name="payrollId">The payroll id</param>
    /// <param name="employeeIds">The employee ids</param>
    /// <param name="tableName">The table name</param>
    /// <param name="columns">The table columns</param>
    /// <param name="language">The language</param>
    /// <param name="valueDate">The value date</param>
    /// <param name="regulationDate">The regulation date</param>
    /// <param name="evaluationDate">The evaluation date</param>
    /// <returns>Employees case values</returns>
    public DataTable ExecuteEmployeeTimeCaseValueQuery(string tableName, int payrollId,
        IEnumerable<int> employeeIds, IEnumerable<CaseValueColumn> columns,
        Language language, DateTime? valueDate = null,
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

        // columns
        var columnNames = new HashSet<string>(columnList.Select(x => x.Name));

        // lookups
        var lookups = GetLookups(payrollId, language, columnList, regulationDate, evaluationDate);

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
                {"CaseType", CaseType.Employee.ToString()},
                {"Language", language.ToString()},
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

    private Dictionary<string, Dictionary<string, string>> GetLookups(int payrollId, Language language, List<CaseValueColumn> columns,
        DateTime? regulationDate, DateTime? evaluationDate)
    {
        var lookups = new Dictionary<string, Dictionary<string, string>>();
        var lookupNames = columns.Where(x => !string.IsNullOrWhiteSpace(x.LookupName)).Select(x => x.LookupName).ToList();
        if (lookupNames.Any())
        {
            lookups = ExecuteLookupQuery(payrollId, lookupNames, language, regulationDate, evaluationDate);
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
                throw new ScriptException($"Unknown case field {column.Name}");
            }

            // column type
            var valueType = GetCaseValueType(caseFieldRow);
            if (!valueType.HasValue)
            {
                throw new ScriptException(
                    $"Unknown case field type in column {column.Name} (enum: {typeof(ValueType)})");
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
    protected int? ResolveIdentifierParameter(string queryName, string parameterName)
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
    protected int? ResolveNameParameter(string queryName, string parameterName)
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

    #region Webhooks

    /// <summary>Invoke report webhook</summary>
    /// <param name="requestOperation">The request operation</param>
    /// <param name="requestMessage">The webhook request message</param>
    /// <returns>The webhook response object</returns>
    public T InvokeWebhook<T>(string requestOperation, object requestMessage = null)
    {
        if (string.IsNullOrWhiteSpace(requestOperation))
        {
            throw new ArgumentException(nameof(requestOperation));
        }

        // webhook request
        var jsonRequest = requestMessage != null ? JsonSerializer.Serialize(requestMessage) : null;
        var jsonResponse = Runtime.InvokeWebhook(requestOperation, jsonRequest);
        if (string.IsNullOrWhiteSpace(jsonResponse))
        {
            return default;
        }
        var response = JsonSerializer.Deserialize<T>(jsonResponse);
        return response;
    }

    #endregion

}