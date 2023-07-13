using System;
using System.Collections.Generic;
using System.Data;

namespace PayrollEngine.Client.Scripting.Runtime;

/// <summary>Runtime for the payrun function <see cref="PayrollEngine.Client.Scripting.Function.ReportStartFunction"/></summary>
public interface IReportRuntime : IRuntime
{
    /// <summary>Gets the report name</summary>
    /// <value>The name of the case</value>
    string ReportName { get; }

    /// <summary>Get report attribute value</summary>
    /// <param name="attributeName">Name of the attribute</param>
    /// <returns>The report attribute value</returns>
    object GetReportAttribute(string attributeName);

    /// <summary>Check for existing report parameter</summary>
    /// <param name="parameterName">The parameter name</param>
    bool HasParameter(string parameterName);

    /// <summary>Get report parameter</summary>
    /// <param name="parameterName">The parameter name</param>
    /// <returns>The report parameter value as JSON</returns>
    string GetParameter(string parameterName);

    /// <summary>Get report parameter attribute value</summary>
    /// <param name="parameterName">The parameter name</param>
    /// <param name="attributeName">Name of the attribute</param>
    /// <returns>The report attribute value</returns>
    object GetParameterAttribute(string parameterName, string attributeName);

    /// <summary>Test for hidden report parameter</summary>
    /// <param name="parameterName">The parameter name</param>
    /// <returns>True for hidden report attribute</returns>
    bool ParameterHidden(string parameterName);

    /// <summary>Execute a query on the Api web method</summary>
    /// <param name="tableName">Target table name</param>
    /// <param name="methodName">The query name</param>
    /// <param name="culture">The content culture</param>
    /// <param name="parameters">The method parameters</param>
    /// <returns>Resulting data table, existing will be removed</returns>
    DataTable ExecuteQuery(string tableName, string methodName, string culture, Dictionary<string, string> parameters);

    /// <summary>Query Json lookup values by lookup name</summary>
    /// <param name="regulationId">The regulation id</param>
    /// <param name="lookupName">The lookup name</param>
    /// <param name="keyAttribute">The json object key attribute name</param>
    /// <param name="valueAttribute">The json object value attribute name</param>
    /// <returns>The lookup values dictionary</returns>
    Dictionary<string, string> ExecuteLookupValueQuery(int regulationId, string lookupName, string keyAttribute, string valueAttribute);

    /// <summary>Execute global case value query on the Api web method</summary>
    /// <param name="tableName">Target table name</param>
    /// <param name="queryValues">The query</param>
    /// <returns>Resulting data table, existing will be removed</returns>
    DataTable ExecuteGlobalCaseValueQuery(string tableName, Tuple<int?, string, string, string, long?, long?> queryValues);

    /// <summary>Execute national case value query on the Api web method</summary>
    /// <param name="tableName">Target table name</param>
    /// <param name="queryValues">The query</param>
    /// <returns>Resulting data table, existing will be removed</returns>
    DataTable ExecuteNationalCaseValueQuery(string tableName, Tuple<int?, string, string, string, long?, long?> queryValues);

    /// <summary>Execute company case value query on the Api web method</summary>
    /// <param name="tableName">Target table name</param>
    /// <param name="queryValues">The query</param>
    /// <returns>Resulting data table, existing will be removed</returns>
    DataTable ExecuteCompanyCaseValueQuery(string tableName, Tuple<int?, string, string, string, long?, long?> queryValues);

    /// <summary>Execute employee case value query on the Api web method</summary>
    /// <param name="tableName">Target table name</param>
    /// <param name="employeeId">The employee id</param>
    /// <param name="queryValues">The query</param>
    /// <returns>Resulting data table, existing will be removed</returns>
    DataTable ExecuteEmployeeCaseValueQuery(string tableName, int employeeId, Tuple<int?, string, string, string, long?, long?> queryValues);

    /// <summary>Execute a wage type query on the Api web method</summary>
    /// <param name="tableName">Target table name</param>
    /// <param name="regulationId">The regulation id</param>
    /// <param name="queryValues">The query</param>
    /// <returns>Resulting data table, existing will be removed</returns>
    DataTable ExecuteWageTypeQuery(string tableName, int regulationId, Tuple<int?, string, string, string, long?, long?> queryValues);

    /// <summary>Execute a payroll result query on the Api web method</summary>
    /// <param name="tableName">Target table name</param>
    /// <param name="queryValues">The query</param>
    /// <returns>Resulting data table, existing will be removed</returns>
    DataTable ExecutePayrollResultQuery(string tableName, Tuple<int?, string, string, string, long?, long?> queryValues);

    /// <summary>Execute a wage type result query on the Api web method</summary>
    /// <param name="tableName">Target table name</param>
    /// <param name="payrollResultId">The payroll result id</param>
    /// <param name="queryValues">The query</param>
    /// <returns>Resulting data table, existing will be removed</returns>
    DataTable ExecuteWageTypeResultQuery(string tableName, int payrollResultId, Tuple<int?, string, string, string, long?, long?> queryValues);

    /// <summary>Execute a wage type custom result query on the Api web method</summary>
    /// <param name="tableName">Target table name</param>
    /// <param name="payrollResultId">The payroll result id</param>
    /// <param name="wageTypeResultId">The wage type result id</param>
    /// <param name="queryValues">The query</param>
    /// <returns>Resulting data table, existing will be removed</returns>
    DataTable ExecuteWageTypeCustomResultQuery(string tableName, int payrollResultId, int wageTypeResultId,
        Tuple<int?, string, string, string, long?, long?> queryValues);

    /// <summary>Execute a collector result query on the Api web method</summary>
    /// <param name="tableName">Target table name</param>
    /// <param name="payrollResultId">The payroll result id</param>
    /// <param name="queryValues">The query</param>
    /// <returns>Resulting data table, existing will be removed</returns>
    DataTable ExecuteCollectorResultQuery(string tableName, int payrollResultId, Tuple<int?, string, string, string, long?, long?> queryValues);

    /// <summary>Execute a collector custom result query on the Api web method</summary>
    /// <param name="tableName">Target table name</param>
    /// <param name="payrollResultId">The payroll result id</param>
    /// <param name="collectorResultId">The collector result id</param>
    /// <param name="queryValues">The query</param>
    /// <returns>Resulting data table, existing will be removed</returns>
    DataTable ExecuteCollectorCustomResultQuery(string tableName, int payrollResultId, int collectorResultId,
        Tuple<int?, string, string, string, long?, long?> queryValues);

    /// <summary>Execute a payrun result query on the Api web method</summary>
    /// <param name="tableName">Target table name</param>
    /// <param name="payrollResultId">The payroll result id</param>
    /// <param name="queryValues">The query</param>
    /// <returns>Resulting data table, existing will be removed</returns>
    DataTable ExecutePayrunResultQuery(string tableName, int payrollResultId, Tuple<int?, string, string, string, long?, long?> queryValues);

    /// <summary>Add report log</summary>
    /// <param name="message">The log message</param>
    /// <param name="key">The log key</param>
    /// <param name="reportDate">The report date (default: now)</param>
    void AddReportLog(string message, string key = null, DateTime? reportDate = null);

    /// <summary>Invoke report webhook and receive the response JSON data</summary>
    /// <param name="requestOperation">The request operation</param>
    /// <param name="requestMessage">The JSON request message</param>
    /// <returns>The webhook response object as JSON</returns>
    string InvokeWebhook(string requestOperation, string requestMessage = null);
}