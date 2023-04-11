/* ReportEndFunction */
// ReSharper disable RedundantUsingDirective
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using PayrollEngine.Client.Scripting;
using PayrollEngine.Client.Scripting.Report;
// ReSharper restore RedundantUsingDirective

namespace PayrollEngine.Client.Scripting.Function;

/// <summary>Report end function</summary>
// ReSharper disable once PartialTypeWithSinglePart
public partial class ReportEndFunction : ReportFunction
{
    /// <summary>Initializes a new instance with the function runtime</summary>
    /// <param name="runtime">The runtime</param>
    public ReportEndFunction(object runtime) :
        base(runtime)
    {
        DataSet = Runtime.DataSet;
    }

    /// <summary>New function instance without runtime (scripting development)</summary>
    /// <param name="sourceFileName">The name of the source file</param>
    public ReportEndFunction(string sourceFileName) :
        base(sourceFileName)
    {
    }

    /// <summary>The report data set</summary>
    public DataSet DataSet { get; }

    #region Tables

    /// <summary>The report data tables</summary>
    public DataTableCollection Tables => DataSet.Tables;

    /// <summary>Determines whether the table contains the specified table</summary>
    /// <param name="tableName">Name of the table</param>
    /// <returns>True for an existing table</returns>
    public bool ContainsTable(string tableName) => Tables.Contains(tableName);

    /// <summary>Add a table to the data set</summary>
    /// <param name="tableName">Name of the table</param>
    public DataTable AddTable(string tableName) => Tables.Add(tableName);

    /// <summary>Add a table to the data set</summary>
    /// <param name="table">The table</param>
    public void AddTable(DataTable table) => Tables.Add(table);

    /// <summary>Remove a table from the data set</summary>
    /// <param name="tableName">Name of the table</param>
    public void RemoveTable(string tableName) => Tables.Remove(tableName);

    /// <summary>Remove multiple tables from the data set</summary>
    /// <param name="tableNames">Name of the tables</param>
    public void RemoveTables(params string[] tableNames)
    {
        foreach (var tableName in tableNames)
        {
            RemoveTable(tableName);
        }
    }
    /// <summary>Computes the given expression on the current rows that pass the filter criteria</summary>
    /// <param name="tableName">Name of the table</param>
    /// <param name="expression">The expression to compute</param>
    /// <param name="filter">The filter to limit the rows that evaluate in the expression</param>
    /// <returns>An Object, set to the result of the computation. If the expression evaluates to null, the return value will be Value</returns>
    public object Compute(string tableName, string expression, string filter = null) =>
        Tables[tableName]?.Compute(expression, filter);

    /// <summary>Computes the given expression on the current rows that pass the filter criteria</summary>
    /// <param name="tableName">Name of the table</param>
    /// <param name="expression">The expression to compute</param>
    /// <param name="filter">The filter to limit the rows that evaluate in the expression</param>
    /// <returns>An Object, set to the result of the computation. If the expression evaluates to null, the return value will be Value</returns>
    public T Compute<T>(string tableName, string expression, string filter = null)
    {
        var table = Tables[tableName];
        if (table == null)
        {
            return default;
        }
        return (T)Compute(expression, filter);
    }

    /// <summary>Execute a query on the Api web method, table name extracted from method name</summary>
    /// <param name="methodName">The query name</param>
    /// <param name="parameters">The method parameters</param>
    /// <returns>New data table, null on empty collection</returns>
    public DataTable ExecuteResultQuery(string methodName, Dictionary<string, string> parameters = null) =>
        ExecuteResultQuery(GetOperationBaseName(methodName), methodName, parameters);

    /// <summary>Execute a query on the Api web method</summary>
    /// <param name="tableName">Target table name</param>
    /// <param name="methodName">The query name</param>
    /// <param name="parameters">The method parameters</param>
    /// <returns>New data table, null on empty collection</returns>
    public DataTable ExecuteResultQuery(string tableName, string methodName,
        Dictionary<string, string> parameters = null) =>
        ExecuteResultQuery(tableName, methodName, Language, true, parameters);

    /// <summary>Execute a result query on the Api web method</summary>
    /// <param name="tableName">Target table name</param>
    /// <param name="methodName">The query name</param>
    /// <param name="language">The content language</param>
    /// <param name="resultQuery">Add query table to the data set</param>
    /// <param name="parameters">The method parameters</param>
    /// <returns>New data table, null on empty collection</returns>
    public DataTable ExecuteResultQuery(string tableName, string methodName, Language language,
        bool resultQuery, Dictionary<string, string> parameters = null) =>
        Runtime.ExecuteQuery(tableName, methodName, (int)language, parameters, resultQuery);

    /// <summary>Execute a query on the Api web method and merge the table to the set</summary>
    /// <param name="tableName">Target table name</param>
    /// <param name="methodName">The query name</param>
    /// <param name="mergeColumn">The column used to merge (primary key column)</param>
    /// <param name="parameters">The method parameters</param>
    /// <param name="schemaChange">Action to take when the required data column is missing</param>
    /// <returns>New or expanded data table</returns>
    public DataTable ExecuteMergeQuery(string tableName, string methodName, string mergeColumn,
        Dictionary<string, string> parameters = null, DataMergeSchemaChange schemaChange = DataMergeSchemaChange.Add) =>
        ExecuteMergeQuery(tableName, methodName, Language, mergeColumn, parameters, schemaChange);

    /// <summary>Execute a query on the Api web method and merge the table to the set</summary>
    /// <param name="tableName">Target table name</param>
    /// <param name="methodName">The query name</param>
    /// <param name="language">The content language</param>
    /// <param name="mergeColumn">The column used to merge (primary key column)</param>
    /// <param name="parameters">The method parameters</param>
    /// <param name="schemaChange">Action to take when the required data column is missing</param>
    /// <returns>New or expanded data table</returns>
    public DataTable ExecuteMergeQuery(string tableName, string methodName, Language language, string mergeColumn,
        Dictionary<string, string> parameters = null, DataMergeSchemaChange schemaChange = DataMergeSchemaChange.Add) =>
        Runtime.ExecuteMergeQuery(tableName, methodName, (int)language, mergeColumn, parameters, (int)schemaChange);

    #endregion

    #region Relations

    /// <summary>Add relation between two table</summary>
    /// <param name="relationName">The relation name</param>
    /// <param name="parentTableName">The relation parent table name</param>
    /// <param name="childTableName">The relation child table name</param>
    /// <param name="childColumnName">The relation child table column name</param>
    public DataRelation AddRelation(string relationName, string parentTableName, string childTableName,
        string childColumnName) =>
        AddRelation(relationName, parentTableName, "Id", childTableName, childColumnName);

    /// <summary>Add relation between two table</summary>
    /// <param name="relationName">The relation name</param>
    /// <param name="parentTableName">The relation parent table name</param>
    /// <param name="parentColumnName">The relation parent table column name</param>
    /// <param name="childTableName">The relation child table name</param>
    /// <param name="childColumnName">The relation child table column name</param>
    public DataRelation AddRelation(string relationName, string parentTableName, string parentColumnName, string childTableName,
        string childColumnName)
    {
        if (string.IsNullOrWhiteSpace(relationName))
        {
            throw new ArgumentException(nameof(relationName));
        }

        var parentTable = DataSet.Tables[parentTableName];
        if (parentTable == null)
        {
            throw new ScriptException($"Missing relation parent table {parentTableName}");
        }
        var parentColumn = parentTable.Columns[parentColumnName];
        if (parentColumn == null)
        {
            throw new ScriptException($"Missing relation parent column {parentTableName}.{parentColumnName}");
        }
        var childTable = DataSet.Tables[childTableName];
        if (childTable == null)
        {
            throw new ScriptException($"Missing relation child table {childTableName}");
        }
        var childColumn = childTable.Columns[childColumnName];
        if (childColumn == null)
        {
            throw new ScriptException($"Missing relation parent column {childTableName}.{childColumnName}");
        }
        return DataSet.Relations.Add(relationName, parentColumn, childColumn);
    }

    #endregion

    /// <exclude />
    public object End()
    {
        // ReSharper disable EmptyRegion
        #region Function
        #endregion
        // ReSharper restore EmptyRegion
        // compiler will optimize this out if the code provides a return
        return default;
    }
}