using System.Collections.Generic;
using System.Data;

namespace PayrollEngine.Client.Scripting.Runtime;

/// <summary>Runtime for the report end function <see cref="Function.ReportEndFunction"/></summary>
public interface IReportEndRuntime : IReportRuntime
{
    /// <summary>Gets the report data set</summary>
    DataSet DataSet { get; }

    /// <summary>Execute a query on the Api web method and add the table to the set</summary>
    /// <param name="tableName">Target table name</param>
    /// <param name="methodName">The query name</param>
    /// <param name="language">The content language</param>
    /// <param name="parameters">The method parameters</param>
    /// <param name="resultQuery">Add query table to the data set</param>
    /// <returns>Resulting data table, existing will be removed</returns>
    DataTable ExecuteQuery(string tableName, string methodName, int language,
        Dictionary<string, string> parameters, bool resultQuery);

    /// <summary>Execute a query on the Api web method and merge the table to the set</summary>
    /// <param name="tableName">Target table name</param>
    /// <param name="methodName">The query name</param>
    /// <param name="language">The content language</param>
    /// <param name="mergeColumn">The column used to merge (primary key column)</param>
    /// <param name="parameters">The method parameters</param>
    /// <param name="schemaChange">Action to take when the required data column is missing <see cref="DataMergeSchemaChange"/></param>
    /// <returns>New or expanded data table</returns>
    DataTable ExecuteMergeQuery(string tableName, string methodName, int language, string mergeColumn,
        Dictionary<string, string> parameters, int schemaChange);
}