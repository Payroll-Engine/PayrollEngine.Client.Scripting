/* Report */

using System;
using System.Data;
using System.Linq;
using System.Text.Json;
using System.Globalization;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PayrollEngine.Client.Scripting.Report;

// see also PayrollEngine.Client.QueryExpression
#region QueryExpression

/// <summary>Report query</summary>
public class ReportQuery
{
    /// <summary>The object status (default: all status)</summary>
    public ObjectStatus? Status { get; set; }

    /// <summary>The OData filter expression (with support for attribute fields)</summary>
    public string Filter { get; set; }

    /// <summary>The OData order-by expression (with support for attribute fields)</summary>
    public string OrderBy { get; set; }

    /// <summary>The OData field selection expression</summary>
    public string Select { get; set; }

    /// <summary>The number of items in the queried collection</summary>
    public long? Top { get; set; }

    /// <summary>The number of items in the queried collection that are to be skipped</summary>
    public long? Skip { get; set; }
}

/// <summary>Query parameters</summary>
public class QueryParameters : Dictionary<string, string>
{
    /// <summary>Query filter</summary>
    public QueryParameters Filter(Filter filter) =>
        Parameter(nameof(Filter), filter?.Expression);

    /// <summary>Query equal id filter</summary>
    public QueryParameters EqualId(int value) =>
        Filter(new EqualId(value));

    /// <summary>Query equal name filter</summary>
    public QueryParameters EqualName(string value) =>
        Filter(new EqualName(value));

    /// <summary>Query equal identifier filter</summary>
    public QueryParameters EqualIdentifier(string value) =>
        Filter(new EqualIdentifier(value));

    /// <summary>Query equals filter</summary>
    public QueryParameters Equals(string field, object value) =>
        Filter(new Equals(field, value));

    /// <summary>Query not equals filter</summary>
    public QueryParameters NotEquals(string field, object value) =>
        Filter(new NotEquals(field, value));

    /// <summary>Query less filter</summary>
    public QueryParameters Less(string field, object value) =>
        Filter(new Less(field, value));

    /// <summary>Query greater filter</summary>
    public QueryParameters Greater(string field, object value) =>
        Filter(new Greater(field, value));

    /// <summary>Query less equals filter</summary>
    public QueryParameters LessEquals(string field, object value) =>
        Filter(new LessEquals(field, value));

    /// <summary>Query greater equals filter</summary>
    public QueryParameters GreaterEquals(string field, object value) =>
        Filter(new GreaterEquals(field, value));

    /// <summary>Query order by</summary>
    public QueryParameters OrderBy(OrderBy orderBy) =>
        Parameter(nameof(OrderBy), orderBy?.Expression);

    /// <summary>Query order by</summary>
    public QueryParameters OrderBy(string field) =>
        OrderBy(new OrderBy(field));

    /// <summary>Query order ascending</summary>
    public QueryParameters OrderAscending(string field) =>
        OrderBy(new OrderAscending(field));

    /// <summary>Query order descending</summary>
    public QueryParameters OrderDescending(string field) =>
        OrderBy(new OrderDescending(field));

    /// <summary>Query inactive objects</summary>
    public QueryParameters InactiveStatus() =>
        Status(new InactiveStatus());

    /// <summary>Query active objects</summary>
    public QueryParameters ActiveStatus() =>
        Status(new ActiveStatus());

    /// <summary>Query object status</summary>
    public QueryParameters Status(Status status) =>
        Parameter(nameof(Status), status?.Expression);

    /// <summary>Query select</summary>
    public QueryParameters Select(Select select) =>
        Parameter(nameof(Select), select?.Expression);

    /// <summary>Query select</summary>
    public QueryParameters Select(params string[] fields) =>
        Select(new Select(fields));

    /// <summary>Query top</summary>
    public QueryParameters Top(int top) =>
        Parameter(nameof(Top), top);

    /// <summary>Query skip</summary>
    public QueryParameters Skip(int skip) =>
        Parameter(nameof(Skip), skip);

    /// <summary>Query id parameter</summary>
    public QueryParameters Id(int id)
    {
        if (id == 0)
        {
            throw new ArgumentOutOfRangeException(nameof(id));
        }
        return Parameter("Id", id);
    }

    /// <summary>Query type id parameter</summary>
    public QueryParameters Id<T>(int id) where T : class
    {
        if (id == 0)
        {
            throw new ArgumentOutOfRangeException(nameof(id));
        }
        return Parameter($"{typeof(T).Name}Id", id);
    }

    /// <summary>Query name parameter</summary>
    public QueryParameters Name(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException(nameof(name));
        }
        return Parameter("Name", name);
    }

    /// <summary>Query identifier parameter</summary>
    public QueryParameters Identifier(string identifier)
    {
        if (string.IsNullOrWhiteSpace(identifier))
        {
            throw new ArgumentException(nameof(identifier));
        }
        return Parameter("Identifier", identifier);
    }

    /// <summary>Query parameter</summary>
    public QueryParameters Parameter<TKey, TValue>(KeyValuePair<TKey, TValue> value) =>
        Parameter(new(value.Key.ToString(), value.Value));

    /// <summary>Query parameter</summary>
    public QueryParameters Parameter(string name, object value) =>
        Parameter(new(name, value));

    /// <summary>Query parameter</summary>
    public QueryParameters Parameter(Parameter parameter)
    {
        if (parameter == null)
        {
            throw new ArgumentNullException(nameof(parameter));
        }

        if (parameter.Value == null)
        {
            Remove(parameter.Name);
        }
        else
        {
            this[parameter.Name] = parameter.Expression;
        }
        return this;
    }
}

/// <summary>Query parameter</summary>
public class Parameter : ExpressionBase
{
    /// <summary>The parameter name</summary>
    public string Name { get; }

    /// <summary>The parameter value</summary>
    public object Value { get; }

    /// <summary>Constructor</summary>
    /// <param name="name">The parameter name</param>
    /// <param name="value">The parameter value</param>
    public Parameter(string name, object value) :
        base(GetValueExpression(value))
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException(nameof(name));
        }
        Name = name;
        Value = value;
    }

    private static string GetValueExpression(object value)
    {
        if (value == null)
        {
            return string.Empty;
        }
        if (value is DateTime dateTime)
        {
            return dateTime.ToUtcString();
        }
        return value.GetType().IsArray ?
            JsonSerializer.Serialize(value) : value.ToString();
    }
}

/// <summary>Query order direction</summary>
public enum OrderDirection
{
    /// <summary>Ascending sort order</summary>
    Ascending,
    /// <summary>Descending sort order</summary>
    Descending
}

/// <summary>Query order ascending</summary>
public class OrderAscending : OrderBy
{
    /// <summary>Constructor</summary>
    /// <param name="field">The order field</param>
    public OrderAscending(string field) :
        base(field, OrderDirection.Ascending)
    {
    }
}

/// <summary>Query order descending</summary>
public class OrderDescending : OrderBy
{
    /// <summary>Constructor</summary>
    /// <param name="field">The order field</param>
    public OrderDescending(string field) :
        base(field, OrderDirection.Descending)
    {
    }
}

/// <summary>Query order by, default order is ascending</summary>
public class OrderBy
{
    /// <summary>The query order by expression</summary>
    public string Expression { get; }

    /// <summary>Constructor</summary>
    /// <param name="expression">The query order expression</param>
    /// <param name="direction">The order direction</param>
    public OrderBy(string expression, OrderDirection? direction = null)
    {
        if (string.IsNullOrWhiteSpace(expression))
        {
            throw new ArgumentException(nameof(expression));
        }

        if (direction == null)
        {
            Expression = expression;
        }
        else
        {
            Expression = direction.Value switch
            {
                OrderDirection.Ascending => $"{expression} ASC",
                OrderDirection.Descending => $"{expression} DESC",
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }

    /// <summary>And with another order by expression</summary>
    /// <param name="orderBy">The order by clause</param>
    /// <returns>Updated query order by</returns>
    public virtual OrderBy AndThenBy(OrderBy orderBy)
    {
        return new($"{Expression}, {orderBy.Expression}");
    }

    /// <summary>Implicit order by to string converter</summary>
    /// <param name="orderBy">The order by clause</param>
    public static implicit operator string(OrderBy orderBy) =>
        orderBy.Expression;

    /// <inheritdoc />
    public override string ToString() => Expression;
}

/// <summary>Inactive status expression</summary>
public class InactiveStatus : Status
{
    /// <summary>Constructor</summary>
    public InactiveStatus() :
        base(ObjectStatus.Inactive)
    {
    }
}

/// <summary>Active status expression</summary>
public class ActiveStatus : Status
{
    /// <summary>Constructor</summary>
    public ActiveStatus() :
        base(ObjectStatus.Active)
    {
    }
}

/// <summary>Object status expression</summary>
public class Status
{
    /// <summary>The status expression</summary>
    public string Expression { get; }

    /// <summary>Constructor</summary>
    /// <param name="status">The object status</param>
    public Status(ObjectStatus status)
    {
        Expression = Enum.GetName(status);
    }
}

/// <summary>Query select</summary>
public class Select
{
    /// <summary>The query select expression</summary>
    public string Expression { get; }

    /// <summary>Constructor</summary>
    /// <param name="fields">The query select fields</param>
    public Select(params string[] fields)
    {
        if (fields == null)
        {
            throw new ArgumentNullException(nameof(fields));
        }
        Expression = string.Join(',', fields);
    }

    /// <summary>Implicit select to string converter</summary>
    /// <param name="select">The select clause</param>
    public static implicit operator string(Select select) =>
        select.ToString();

    /// <inheritdoc />
    public override string ToString() => Expression;
}

/// <summary>Equal active status filter expression</summary>
public class Active : EqualStatus
{
    /// <summary>Constructor</summary>
    public Active() :
        base(ObjectStatus.Active)
    {
    }
}

/// <summary>Equal inactive status filter expression</summary>
public class Inactive : EqualStatus
{
    /// <summary>Constructor</summary>
    public Inactive() :
        base(ObjectStatus.Inactive)
    {
    }
}

/// <summary>Equal status filter expression</summary>
public class EqualStatus : Equals
{
    /// <summary>Constructor</summary>
    /// <param name="status">The object status</param>
    public EqualStatus(ObjectStatus status) :
        base("Status", Enum.GetName(status))
    {
    }
}

/// <summary>Equal id filter expression</summary>
public class EqualId : Equals
{
    /// <summary>Constructor</summary>
    /// <param name="value">The query id</param>
    public EqualId(int value) :
        base("Id", value)
    {
    }
}

/// <summary>Equal name filter expression</summary>
public class EqualName : Equals
{
    /// <summary>Constructor</summary>
    /// <param name="value">The query name</param>
    public EqualName(string value) :
        base("Name", value)
    {
    }

    /// <summary>Constructor</summary>
    /// <param name="value">The value function</param>
    public EqualName(FunctionBase value) :
        this(value.Expression)
    {
    }
}

/// <summary>Equal identifier filter expression</summary>
public class EqualIdentifier : Equals
{
    /// <summary>Constructor</summary>
    /// <param name="identifier">The query identifier</param>
    public EqualIdentifier(string identifier) :
        base("Identifier", identifier)
    {
    }

    /// <summary>Constructor</summary>
    /// <param name="identifier">The identifier function</param>
    public EqualIdentifier(FunctionBase identifier) :
        this(identifier.Expression)
    {
    }
}

/// <summary>Equals filter expression</summary>
public class Equals : ConditionFilter
{
    /// <summary>Constructor</summary>
    /// <param name="field">The query field name</param>
    /// <param name="value">The query value</param>
    public Equals(string field, object value) :
        base(field, "eq", value)
    {
    }

    /// <summary>Constructor</summary>
    /// <param name="field">The field function</param>
    /// <param name="value">The query value</param>
    public Equals(FunctionBase field, object value) :
        this(field.Expression, value)
    {
    }

    /// <summary>Constructor</summary>
    /// <param name="field">The query field name</param>
    /// <param name="value">The expression function</param>
    public Equals(string field, FunctionBase value) :
        this(field, value.Expression)
    {
    }

    /// <summary>Constructor</summary>
    /// <param name="field">The field function</param>
    /// <param name="value">The value function</param>
    public Equals(FunctionBase field, FunctionBase value) :
        this(field.Expression, value.Expression)
    {
    }
}

/// <summary>Not equals filter expression</summary>
public class NotEquals : ConditionFilter
{
    /// <summary>Constructor</summary>
    /// <param name="field">The query field name</param>
    /// <param name="value">The query value</param>
    public NotEquals(string field, object value) :
        base(field, "ne", value)
    {
    }

    /// <summary>Constructor</summary>
    /// <param name="field">The field function</param>
    /// <param name="value">The query value</param>
    public NotEquals(FunctionBase field, object value) :
        this(field.Expression, value)
    {
    }

    /// <summary>Constructor</summary>
    /// <param name="field">The query field name</param>
    /// <param name="value">The expression function</param>
    public NotEquals(string field, FunctionBase value) :
        this(field, value.Expression)
    {
    }

    /// <summary>Constructor</summary>
    /// <param name="field">The field function</param>
    /// <param name="value">The value function</param>
    public NotEquals(FunctionBase field, FunctionBase value) :
        this(field.Expression, value.Expression)
    {
    }
}

/// <summary>Less than filter expression</summary>
public class Less : ConditionFilter
{
    /// <summary>Constructor</summary>
    /// <param name="field">The query field name</param>
    /// <param name="value">The query value</param>
    public Less(string field, object value) :
        base(field, "lt", value)
    {
    }

    /// <summary>Constructor</summary>
    /// <param name="field">The field function</param>
    /// <param name="value">The query value</param>
    public Less(FunctionBase field, object value) :
        this(field.Expression, value)
    {
    }

    /// <summary>Constructor</summary>
    /// <param name="field">The query field name</param>
    /// <param name="value">The expression function</param>
    public Less(string field, FunctionBase value) :
        this(field, value.Expression)
    {
    }

    /// <summary>Constructor</summary>
    /// <param name="field">The field function</param>
    /// <param name="value">The value function</param>
    public Less(FunctionBase field, FunctionBase value) :
        this(field.Expression, value.Expression)
    {
    }
}

/// <summary>Greater than filter expression</summary>
public class Greater : ConditionFilter
{
    /// <summary>Constructor</summary>
    /// <param name="field">The query field name</param>
    /// <param name="value">The query value</param>
    public Greater(string field, object value) :
        base(field, "gt", value)
    {
    }

    /// <summary>Constructor</summary>
    /// <param name="field">The field function</param>
    /// <param name="value">The query value</param>
    public Greater(FunctionBase field, object value) :
        this(field.Expression, value)
    {
    }

    /// <summary>Constructor</summary>
    /// <param name="field">The query field name</param>
    /// <param name="value">The expression function</param>
    public Greater(string field, FunctionBase value) :
        this(field, value.Expression)
    {
    }

    /// <summary>Constructor</summary>
    /// <param name="field">The field function</param>
    /// <param name="value">The value function</param>
    public Greater(FunctionBase field, FunctionBase value) :
        this(field.Expression, value.Expression)
    {
    }
}

/// <summary>Less than or equals filter expression</summary>
public class LessEquals : ConditionFilter
{
    /// <summary>Constructor</summary>
    /// <param name="field">The query field name</param>
    /// <param name="value">The query value</param>
    public LessEquals(string field, object value) :
        base(field, "le", value)
    {
    }

    /// <summary>Constructor</summary>
    /// <param name="field">The field function</param>
    /// <param name="value">The query value</param>
    public LessEquals(FunctionBase field, object value) :
        this(field.Expression, value)
    {
    }

    /// <summary>Constructor</summary>
    /// <param name="field">The query field name</param>
    /// <param name="value">The expression function</param>
    public LessEquals(string field, FunctionBase value) :
        this(field, value.Expression)
    {
    }

    /// <summary>Constructor</summary>
    /// <param name="field">The field function</param>
    /// <param name="value">The value function</param>
    public LessEquals(FunctionBase field, FunctionBase value) :
        this(field.Expression, value.Expression)
    {
    }
}

/// <summary>Greater than or equals filter expression</summary>
public class GreaterEquals : ConditionFilter
{
    /// <summary>Constructor</summary>
    /// <param name="field">The query field name</param>
    /// <param name="value">The query value</param>
    public GreaterEquals(string field, object value) :
        base(field, "ge", value)
    {
    }

    /// <summary>Constructor</summary>
    /// <param name="field">The field function</param>
    /// <param name="value">The query value</param>
    public GreaterEquals(FunctionBase field, object value) :
        this(field.Expression, value)
    {
    }

    /// <summary>Constructor</summary>
    /// <param name="field">The query field name</param>
    /// <param name="value">The expression function</param>
    public GreaterEquals(string field, FunctionBase value) :
        this(field, value.Expression)
    {
    }

    /// <summary>Constructor</summary>
    /// <param name="field">The field function</param>
    /// <param name="value">The value function</param>
    public GreaterEquals(FunctionBase field, FunctionBase value) :
        this(field.Expression, value.Expression)
    {
    }
}

/// <summary>Condition filter</summary>
public abstract class ConditionFilter : Filter
{
    /// <summary>The query field name</summary>
    public string Field { get; }

    /// <summary>The query operator</summary>
    public string Operator { get; }

    /// <summary>The query field name</summary>
    public object Value { get; }

    /// <summary>Constructor</summary>
    /// <param name="field">The query field name</param>
    /// <param name="operator">The filter operator</param>
    /// <param name="value">The query value</param>
    protected ConditionFilter(string field, string @operator, object value) :
        base($"{field} {@operator} {GetFilterValue(value)}")
    {
        if (string.IsNullOrWhiteSpace(field))
        {
            throw new ArgumentException(nameof(field));
        }
        if (string.IsNullOrWhiteSpace(@operator))
        {
            throw new ArgumentException(nameof(@operator));
        }
        Field = field;
        Operator = @operator;
        Value = value;
    }

    private static string GetFilterValue(object value) =>
        value switch
        {
            null => null,
            string => $"'{value}'",
            DateTime dateTime => $"'{dateTime.ToUtcString()}'",
            _ => value.ToString()
        };
}

/// <summary>Contains filter expression</summary>
public class Contains : Filter
{
    /// <summary>Constructor</summary>
    /// <param name="field">The query field name</param>
    /// <param name="expression">The query expression</param>
    public Contains(string field, string expression) :
        base($"contains({field},'{expression}')")
    {
    }

    /// <summary>Constructor</summary>
    /// <param name="field">The field function</param>
    /// <param name="expression">The query expression</param>
    public Contains(FunctionBase field, string expression) :
        this(field.Expression, expression)
    {
    }

    /// <summary>Constructor</summary>
    /// <param name="field">The query field name</param>
    /// <param name="value">The expression function</param>
    public Contains(string field, FunctionBase value) :
        this(field, value.Expression)
    {
    }

    /// <summary>Constructor</summary>
    /// <param name="field">The field function</param>
    /// <param name="value">The value function</param>
    public Contains(FunctionBase field, FunctionBase value) :
        this(field.Expression, value.Expression)
    {
    }
}

/// <summary>Ends with filter expression</summary>
public class EndsWith : Filter
{
    /// <summary>Constructor</summary>
    /// <param name="field">The query field name</param>
    /// <param name="expression">The query expression</param>
    public EndsWith(string field, string expression) :
        base($"endswith({field},'{expression}')")
    {
    }

    /// <summary>Constructor</summary>
    /// <param name="field">The field function</param>
    /// <param name="expression">The query expression</param>
    public EndsWith(FunctionBase field, string expression) :
        this(field.Expression, expression)
    {
    }

    /// <summary>Constructor</summary>
    /// <param name="field">The query field name</param>
    /// <param name="value">The expression function</param>
    public EndsWith(string field, FunctionBase value) :
        this(field, value.Expression)
    {
    }

    /// <summary>Constructor</summary>
    /// <param name="field">The field function</param>
    /// <param name="value">The value function</param>
    public EndsWith(FunctionBase field, FunctionBase value) :
        this(field.Expression, value.Expression)
    {
    }
}

/// <summary>Starts with filter expression</summary>
public class StartsWith : Filter
{
    /// <summary>Constructor</summary>
    /// <param name="field">The query field name</param>
    /// <param name="expression">The query expression</param>
    public StartsWith(string field, string expression) :
        base($"startswith({field},'{expression}')")
    {
    }

    /// <summary>Constructor</summary>
    /// <param name="field">The field function</param>
    /// <param name="expression">The query expression</param>
    public StartsWith(FunctionBase field, string expression) :
        this(field.Expression, expression)
    {
    }

    /// <summary>Constructor</summary>
    /// <param name="field">The query field name</param>
    /// <param name="value">The expression function</param>
    public StartsWith(string field, FunctionBase value) :
        this(field, value.Expression)
    {
    }

    /// <summary>Constructor</summary>
    /// <param name="field">The field function</param>
    /// <param name="value">The value function</param>
    public StartsWith(FunctionBase field, FunctionBase value) :
        this(field.Expression, value.Expression)
    {
    }
}

/// <summary>Query filter</summary>
public class Filter : ExpressionBase
{
    /// <summary>Constructor</summary>
    /// <param name="expression">The filter expression</param>
    protected Filter(string expression) :
        base(expression)
    {
    }

    /// <summary>Group existing filters</summary>
    /// <returns>Updated query filter</returns>
    public virtual Filter Group()
    {
        if (Expression.StartsWith('(') && Expression.EndsWith(')'))
        {
            return this;
        }
        return new($"({Expression})");
    }

    /// <summary>And with another filter expression</summary>
    /// <returns>Updated query filter</returns>
    public virtual Filter And(Filter filter)
    {
        return new($"{Group()} and {filter.Group()}");
    }

    /// <summary>Or with another filter expression</summary>
    /// <returns>Updated query filter</returns>
    public virtual Filter Or(Filter expression)
    {
        return new($"{Expression} or {expression.Expression}");
    }
}

/// <summary>Time function expression</summary>
public class Time : FunctionBase
{
    /// <summary>Constructor</summary>
    /// <param name="expression">The query expression</param>
    public Time(string expression) :
        base($"time({expression})")
    {
    }

    /// <summary>Constructor</summary>
    /// <param name="innerFunction">The inner function</param>
    public Time(FunctionBase innerFunction) :
        base(innerFunction)
    {
    }
}

/// <summary>Date function expression</summary>
public class Date : FunctionBase
{
    /// <summary>Constructor</summary>
    /// <param name="expression">The query expression</param>
    public Date(string expression) :
        base($"date({expression})")
    {
    }

    /// <summary>Constructor</summary>
    /// <param name="innerFunction">The inner function</param>
    public Date(FunctionBase innerFunction) :
        base(innerFunction)
    {
    }
}

/// <summary>Minute function expression</summary>
public class Minute : FunctionBase
{
    /// <summary>Constructor</summary>
    /// <param name="expression">The query expression</param>
    public Minute(string expression) :
        base($"minute({expression})")
    {
    }

    /// <summary>Constructor</summary>
    /// <param name="innerFunction">The inner function</param>
    public Minute(FunctionBase innerFunction) :
        base(innerFunction)
    {
    }
}

/// <summary>Hour function expression</summary>
public class Hour : FunctionBase
{
    /// <summary>Constructor</summary>
    /// <param name="expression">The query expression</param>
    public Hour(string expression) :
        base($"hour({expression})")
    {
    }

    /// <summary>Constructor</summary>
    /// <param name="innerFunction">The inner function</param>
    public Hour(FunctionBase innerFunction) :
        base(innerFunction)
    {
    }
}

/// <summary>Day function expression</summary>
public class Day : FunctionBase
{
    /// <summary>Constructor</summary>
    /// <param name="expression">The query expression</param>
    public Day(string expression) :
        base($"day({expression})")
    {
    }

    /// <summary>Constructor</summary>
    /// <param name="innerFunction">The inner function</param>
    public Day(FunctionBase innerFunction) :
        base(innerFunction)
    {
    }
}

/// <summary>Month function expression</summary>
public class Month : FunctionBase
{
    /// <summary>Constructor</summary>
    /// <param name="expression">The query expression</param>
    public Month(string expression) :
        base($"month({expression})")
    {
    }

    /// <summary>Constructor</summary>
    /// <param name="innerFunction">The inner function</param>
    public Month(FunctionBase innerFunction) :
        base(innerFunction)
    {
    }
}

/// <summary>Year function expression</summary>
public class Year : FunctionBase
{
    /// <summary>Constructor</summary>
    /// <param name="expression">The query expression</param>
    public Year(string expression) :
        base($"year({expression})")
    {
    }

    /// <summary>Constructor</summary>
    /// <param name="innerFunction">The inner function</param>
    public Year(FunctionBase innerFunction) :
        base(innerFunction)
    {
    }
}

/// <summary>Query function</summary>
public abstract class FunctionBase : ExpressionBase
{
    /// <summary>Constructor</summary>
    /// <param name="expression">The function expression</param>
    protected FunctionBase(string expression) :
        base(expression)
    {
    }

    /// <summary>Constructor</summary>
    /// <param name="innerFunction">The inner function</param>
    protected FunctionBase(FunctionBase innerFunction) :
        this(innerFunction.Expression)
    {
    }
}

/// <summary>Query expression base</summary>
public abstract class ExpressionBase
{
    /// <summary>The query expression</summary>
    public string Expression { get; }

    /// <summary>Constructor</summary>
    /// <param name="expression">The query expression</param>
    protected ExpressionBase(string expression)
    {
        if (string.IsNullOrWhiteSpace(expression))
        {
            throw new ArgumentException(nameof(expression));
        }
        Expression = expression;
    }

    /// <summary>Implicit function to string converter</summary>
    /// <param name="function">The function</param>
    public static implicit operator string(ExpressionBase function) =>
        function.Expression;

    /// <inheritdoc />
    public override string ToString() => Expression;
}

#endregion

#region Extensions

// duplicated in PayrollEngine.DataTableExtensions
/// <summary>Data set extension methods</summary>
public static class DataSetExtensions
{
    /// <summary>Test for  data set rows</summary>
    /// <param name="dataSet">The system data set to convert</param>
    /// <returns>True if any row is available</returns>
    public static bool HasRows(this DataSet dataSet)
    {
        if (dataSet?.Tables == null || dataSet.Tables.Count == 0)
        {
            return false;
        }
        return dataSet.Tables.Cast<DataTable>().Any(table => table.Rows.Count > 0);
    }

    /// <summary>Get data set table rows values as dictionary</summary>
    /// <param name="dataSet">The payroll data set to convert</param>
    /// <returns>The data table values as dictionary, key is table column name</returns>
    public static Dictionary<string, List<Dictionary<string, object>>> AsDictionary(this DataSet dataSet)
    {
        var values = new Dictionary<string, List<Dictionary<string, object>>>();
        foreach (DataTable table in dataSet.Tables)
        {
            values.Add(table.TableName, table.AsDictionary());
        }
        return values;
    }

    /// <summary>Get data set as json</summary>
    /// <param name="dataSet">The payroll data set to convert</param>
    /// <param name="namingPolicy">Naming policy (default: camel case)</param>
    /// <param name="ignoreNull">Ignore null values (default: true)</param>
    public static string Json(this DataSet dataSet, JsonNamingPolicy namingPolicy = null,
        bool ignoreNull = true)
    {
        return JsonSerializer.Serialize(AsDictionary(dataSet), new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = namingPolicy ?? JsonNamingPolicy.CamelCase,
            DictionaryKeyPolicy = namingPolicy ?? JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = ignoreNull ? JsonIgnoreCondition.WhenWritingNull : default
        });
    }
}

// duplicated in PayrollEngine.DataTableExtensions
/// <summary>Data table extension methods</summary>
public static class DataTableExtensions
{
    /// <summary>Remove the table primary key</summary>
    /// <param name="table">The table</param>
    public static void RemovePrimaryKey(this DataTable table)
    {
        if (table != null)
        {
            table.PrimaryKey = [];
        }
    }

    /// <summary>Test for table column</summary>
    /// <param name="table">The table</param>
    /// <param name="columnName">Name of the column</param>
    public static bool ContainsColumn(this DataTable table, string columnName) =>
        table.Columns.Contains(columnName);

    /// <summary>Add table column</summary>
    /// <param name="table">The table</param>
    /// <param name="columnName">Name of the column</param>
    /// <param name="expression">The compute expression</param>
    public static DataColumn AddColumn<T>(this DataTable table, string columnName, string expression = null) =>
        AddColumn(table, columnName, typeof(T), expression);

    /// <summary>Add table column</summary>
    /// <param name="table">The table</param>
    /// <param name="columnName">Name of the column</param>
    /// <param name="type">The column type</param>
    /// <param name="expression">The compute expression</param>
    public static DataColumn AddColumn(this DataTable table, string columnName, Type type, string expression = null)
    {
        if (string.IsNullOrWhiteSpace(columnName))
        {
            throw new ArgumentException(nameof(columnName));
        }

        if (expression == null)
        {
            return table.Columns.Add(columnName, type);
        }
        return table.Columns.Add(columnName, type, expression);
    }

    /// <summary>Add table relation column with initial value</summary>
    /// <param name="table">The table</param>
    /// <param name="columnName">Name of the column</param>
    /// <param name="relationValue">Value to set to all rows</param>
    public static DataColumn AddRelationColumn<T>(this DataTable table, string columnName, object relationValue)
    {
        var column = AddColumn(table, columnName, typeof(T));
        foreach (var row in table.AsEnumerable())
        {
            row[columnName] = relationValue;
        }
        return column;
    }

    /// <summary>Add table relation integer column with initial value</summary>
    /// <param name="table">The table</param>
    /// <param name="columnName">Name of the column</param>
    /// <param name="relationId">Value to set to all rows</param>
    public static DataColumn AddRelationColumn(this DataTable table, string columnName, int relationId) =>
        AddRelationColumn<int>(table, columnName, relationId);

    /// <summary>Add localization column</summary>
    /// <param name="table">The table</param>
    /// <param name="culture">Localization culture</param>
    /// <param name="columnName">Name of the source column</param>
    /// <param name="localizationsColumnName">Name of the localizations column (default: columnNameLocalizations)</param>
    /// <remarks>Requires </remarks>
    public static DataColumn AddLocalizationColumn(this DataTable table, string culture, string columnName,
        string localizationsColumnName = null)
    {
        localizationsColumnName ??= $"{columnName}Localizations";
        if (!table.Columns.Contains(columnName) || !table.Columns.Contains(localizationsColumnName))
        {
            return null;
        }

        var localizationColumnName = $"{columnName}Localization";
        var column = AddColumn(table, localizationColumnName, typeof(string));
        foreach (var row in table.AsEnumerable())
        {
            var localizationsJson = row[localizationsColumnName] as string;
            if (string.IsNullOrWhiteSpace(localizationsJson))
            {
                continue;
            }

            var original = row[columnName] as string;
            var localizations = JsonSerializer.Deserialize<Dictionary<string, string>>(localizationsJson);
            row[localizationColumnName] = culture.GetLocalization(localizations, original);
        }
        return column;
    }

    /// <summary>Insert table column at certain list position</summary>
    /// <param name="table">The table</param>
    /// <param name="index">The column list position</param>
    /// <param name="columnName">Name of the column</param>
    /// <param name="expression">The compute expression</param>
    public static DataColumn InsertColumn<T>(this DataTable table, int index, string columnName, string expression = null)
    {
        if (index > table.Columns.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }

        var column = AddColumn<T>(table, columnName, expression);
        // change column position
        column.SetOrdinal(index);
        return column;
    }

    /// <summary>Ensure table column</summary>
    /// <param name="table">The table</param>
    /// <param name="columnName">Name of the column</param>
    /// <param name="expression">The compute expression</param>
    public static DataColumn EnsureColumn<T>(this DataTable table, string columnName, string expression = null) =>
        EnsureColumn(table, columnName, typeof(T), expression);

    /// <summary>Ensure table column</summary>
    /// <param name="table">The table</param>
    /// <param name="columnName">Name of the column</param>
    /// <param name="type">The column type</param>
    /// <param name="expression">The compute expression</param>
    public static DataColumn EnsureColumn(this DataTable table, string columnName, Type type, string expression = null)
    {
        if (!ContainsColumn(table, columnName))
        {
            return AddColumn(table, columnName, type, expression);
        }
        return table.Columns[columnName];
    }

    /// <summary>Ensure table columns</summary>
    /// <param name="table">The table</param>
    /// <param name="columns">The columns to ensure</param>
    public static void EnsureColumns<T>(this DataTable table, IEnumerable<string> columns)
    {
        foreach (var column in columns)
        {
            if (!table.Columns.Contains(column))
            {
                table.Columns.Add(column, typeof(T));
            }
        }
    }

    /// <summary>Ensure table column</summary>
    /// <param name="table">The table</param>
    /// <param name="columns">The columns to ensure</param>
    public static void EnsureColumns(this DataTable table, DataColumnCollection columns)
    {
        foreach (DataColumn column in columns)
        {
            if (!table.Columns.Contains(column.ColumnName))
            {
                table.Columns.Add(column.ColumnName, column.DataType);
            }
        }
    }

    /// <summary>Rename table column</summary>
    /// <param name="table">The table</param>
    /// <param name="oldColumnName">Existing name of the column</param>
    /// <param name="newColumnName">Existing name of the column</param>
    /// <returns>The column name</returns>
    public static string RenameColumn(this DataTable table, string oldColumnName, string newColumnName)
    {
        if (table == null)
        {
            return null;
        }
        var column = table.Columns[oldColumnName];
        if (column == null)
        {
            return null;
        }
        column.ColumnName = newColumnName;
        return newColumnName;
    }

    /// <summary>Remove table column</summary>
    /// <param name="table">The table</param>
    /// <param name="columnName">Name of the column</param>
    public static void RemoveColumn(this DataTable table, string columnName) =>
        table?.Columns.Remove(columnName);

    /// <summary>Set the table primary key column</summary>
    /// <param name="table">The table</param>
    /// <param name="columnName">Name of the column</param>
    public static void SetPrimaryKey(this DataTable table, string columnName)
    {
        if (table != null)
        {
            var column = table.Columns[columnName];
            table.PrimaryKey = [column];
        }
    }

    /// <summary>Get table rows</summary>
    /// <param name="table">The table</param>
    /// <returns>A row collection</returns>
    public static EnumerableRowCollection Rows(this DataTable table) =>
        table.AsEnumerable();

    /// <summary>Test for any table rows</summary>
    /// <param name="table">The table</param>
    /// <returns>True if rows are present</returns>
    public static bool HasRows(this DataTable table) =>
        table != null && table.Rows.Count > 0;

    /// <summary>Find row by value search</summary>
    /// <param name="table">The table</param>
    /// <param name="column">The search column name</param>
    /// <param name="value">The search key</param>
    /// <returns>The matching data ro or null</returns>
    public static DataRow FindFirstRow<T>(this DataTable table, string column, T value) =>
        table.AsEnumerable().FirstOrDefault(x => Equals(x.GetValue<T>(column), value));

    /// <summary>Get row table value by value search</summary>
    /// <param name="table">The table</param>
    /// <param name="keyColumn">The search column name</param>
    /// <param name="keyValue">The search key</param>
    /// <param name="valueColumn">The value column name</param>
    /// <param name="defaultValue">The default value</param>
    /// <returns>The data row value</returns>
    public static T GetRowValue<T>(this DataTable table, string keyColumn, object keyValue,
        string valueColumn, T defaultValue = default)
    {
        var dataRow = FindFirstRow(table, keyColumn, keyValue);
        return dataRow != null ? dataRow.GetValue(valueColumn, defaultValue) : defaultValue;
    }

    /// <summary>Test for single row table</summary>
    /// <param name="table">The table</param>
    /// <returns>True for a single row collection</returns>
    public static bool IsSingleRow(this DataTable table) =>
        table != null && table.Rows.Count == 1;

    /// <summary>Get single row table</summary>
    /// <param name="table">The table</param>
    /// <returns>The single row</returns>
    public static DataRow SingleRow(this DataTable table)
    {
        if (!IsSingleRow(table))
        {
            throw new ScriptException($"Table {table.TableName} is not single, count={table.Rows.Count}.");
        }
        return table.Rows[0];
    }

    /// <summary>Get as single row table</summary>
    /// <param name="table">The table</param>
    /// <returns>The single row, null on table with multiple rows</returns>
    public static DataRow AsSingleRow(this DataTable table) =>
        IsSingleRow(table) ? SingleRow(table) : null;

    /// <summary>Get single row id</summary>
    /// <param name="table">The table</param>
    /// <returns>The data row id</returns>
    public static int SingleRowId(this DataTable table) =>
        IsSingleRow(table) ? SingleRow(table).Id() : 0;

    /// <summary>Get single row name</summary>
    /// <param name="table">The table</param>
    /// <returns>The data row name</returns>
    public static string SingleRowName(this DataTable table) =>
        IsSingleRow(table) ? SingleRow(table).Name() : null;

    /// <summary>Get single row identifier</summary>
    /// <param name="table">The table</param>
    /// <returns>The data row identifier</returns>
    public static string SingleRowIdentifier(this DataTable table) =>
        IsSingleRow(table) ? SingleRow(table).Identifier() : null;

    /// <summary>Get single row table value</summary>
    /// <param name="table">The table</param>
    /// <param name="column">The column name</param>
    /// <param name="defaultValue">The default value</param>
    /// <returns>The data row value</returns>
    public static T SingleRowValue<T>(this DataTable table, string column, T defaultValue = default) =>
        IsSingleRow(table) ? SingleRow(table).GetValue(column, defaultValue) : defaultValue;

    /// <summary>Select table rows by filter</summary>
    /// <param name="table">The table</param>
    /// <param name="filterExpression">The filter matching the rows to delete</param>
    public static IEnumerable<DataRow> SelectRows(this DataTable table, string filterExpression) =>
        table.Select(filterExpression);

    /// <summary>Selects rows by function</summary>
    /// <param name="table">The table</param>
    /// <param name="selectFunc">Select functions</param>
    /// <returns>The selected data rows</returns>
    public static List<DataRow> SelectRows(this DataTable table, Func<DataRow, bool> selectFunc) =>
        Enumerable.Where(table.AsEnumerable(), selectFunc).ToList();

    /// <summary>Test for rows</summary>
    /// <param name="table">The table</param>
    public static bool Any(this DataTable table) =>
        table.Rows.Count > 0;

    /// <summary>Test for rows by function</summary>
    /// <param name="table">The table</param>
    /// <param name="anyFunc">Any functions</param>
    public static bool Any(this DataTable table, Func<DataRow, bool> anyFunc) =>
        table.AsEnumerable().Any(anyFunc);

    /// <summary>Delete table rows by filter</summary>
    /// <param name="table">The table</param>
    /// <param name="filterExpression">The filter matching the rows to delete</param>
    /// <returns>The deleted row count</returns>
    public static int DeleteRows(this DataTable table, string filterExpression)
    {
        var deleteCount = 0;
        var deleteRows = SelectRows(table, filterExpression);
        foreach (var deleteRow in deleteRows)
        {
            deleteRow.Delete();
            deleteCount++;
        }
        if (deleteCount > 0)
        {
            table.AcceptChanges();
        }
        return deleteCount;
    }

    /// <summary>Get data table as dictionary</summary>
    /// <param name="dataTable">The data table</param>
    /// <returns>List of row dictionaries</returns>
    public static List<Dictionary<string, object>> AsDictionary(this DataTable dataTable)
    {
        var values = new List<Dictionary<string, object>>();
        foreach (DataRow row in dataTable.AsEnumerable())
        {
            values.Add(row.AsDictionary());
        }
        return values;
    }

    /// <summary>Get data table as json</summary>
    /// <param name="dataTable">The data table</param>
    /// <param name="namingPolicy">Naming policy (default: camel case)</param>
    /// <param name="ignoreNull">Ignore null values (default: true)</param>
    public static string Json(this DataTable dataTable, JsonNamingPolicy namingPolicy = null,
        bool ignoreNull = true)
    {
        return JsonSerializer.Serialize(AsDictionary(dataTable), new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = namingPolicy ?? JsonNamingPolicy.CamelCase,
            DictionaryKeyPolicy = namingPolicy ?? JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = ignoreNull ? JsonIgnoreCondition.WhenWritingNull : default
        });
    }

    /// <summary>Get data table rows value</summary>
    /// <param name="table">The data table</param>
    /// <param name="column">The column name</param>
    /// <param name="defaultValue">The default value</param>
    /// <returns>The data table rows value</returns>
    public static List<T> GetValues<T>(this DataTable table, string column, T defaultValue = default) =>
        table.Select().GetValues(column, defaultValue);

    /// <summary>Get data table rows JSON value as dictionary</summary>
    /// <param name="table">The data table</param>
    /// <param name="column">The column name</param>
    /// <param name="keyField">The json object key field</param>
    /// <param name="valueField">The json object value field</param>
    /// <returns>The data table rows value</returns>
    public static Dictionary<string, string> GetDictionary(this DataTable table,
        string column, string keyField, string valueField)
    {
        var values = new Dictionary<string, string>();
        foreach (var row in table.AsEnumerable())
        {
            var objectValues = row.GetDictionary<string, string>(column);
            if (objectValues != null && objectValues.ContainsKey(keyField) &&
                objectValues.TryGetValue(valueField, out var value))
            {
                values.Add(objectValues[keyField], value);
            }
        }
        return values;
    }
}

// duplicated in PayrollEngine.DataRowExtensions
/// <summary>Data row extension methods</summary>
public static class DataRowExtensions
{
    /// <summary>Get data row id</summary>
    /// <param name="dataRow">The data row</param>
    /// <returns>The data row id</returns>
    public static int Id(this DataRow dataRow) =>
        GetValue<int>(dataRow, "Id");

    /// <summary>Get data row name</summary>
    /// <param name="dataRow">The data row</param>
    /// <returns>The data row name</returns>
    public static string Name(this DataRow dataRow) =>
        GetValue<string>(dataRow, "Name");

    /// <summary>Get data row identifier</summary>
    /// <param name="dataRow">The data row</param>
    /// <returns>The data row identifier</returns>
    public static string Identifier(this DataRow dataRow) =>
        GetValue<string>(dataRow, "Identifier");

    /// <summary>Get data row object status</summary>
    /// <param name="dataRow">The data row</param>
    /// <returns>The data row object status</returns>
    public static ObjectStatus ObjectStatus(this DataRow dataRow) =>
        GetEnumValue(dataRow, "Status", Scripting.ObjectStatus.Inactive);

    /// <summary>Get data row values as dictionary</summary>
    /// <param name="dataRow">The data row</param>
    /// <returns>The data rows values as dictionary, key is the column name</returns>
    public static Dictionary<string, object> AsDictionary(this DataRow dataRow)
    {
        var values = new Dictionary<string, object>();
        foreach (DataColumn column in dataRow.Table.Columns)
        {
            values.Add(column.ColumnName, GetValue<object>(dataRow, column.ColumnName));
        }
        return values;
    }

    /// <summary>Get data row as json</summary>
    /// <param name="dataRow">The data row</param>
    /// <param name="namingPolicy">Naming policy (default: camel case)</param>
    /// <param name="ignoreNull">Ignore null values (default: true)</param>
    public static string Json(this DataRow dataRow, JsonNamingPolicy namingPolicy = null,
        bool ignoreNull = true)
    {
        ArgumentNullException.ThrowIfNull(dataRow);
        return JsonSerializer.Serialize(AsDictionary(dataRow), new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = namingPolicy ?? JsonNamingPolicy.CamelCase,
            DictionaryKeyPolicy = namingPolicy ?? JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = ignoreNull ? JsonIgnoreCondition.WhenWritingNull : default
        });
    }

    /// <summary>Get data row enum value</summary>
    /// <param name="dataRow">The data row</param>
    /// <param name="column">The column name</param>
    /// <param name="defaultValue">The default value</param>
    /// <returns>The data row enum value</returns>
    public static T GetEnumValue<T>(this DataRow dataRow, string column, T defaultValue = default)
        where T : struct
    {
        if (!typeof(T).IsEnum)
        {
            throw new ScriptException($"Invalid enum value type: {typeof(T)}.");
        }
        var valueText = GetValue(dataRow, column, defaultValue.ToString());
        if (string.IsNullOrWhiteSpace(valueText) || !Enum.TryParse(valueText, true, out T enumValue))
        {
            return defaultValue;
        }
        return enumValue;
    }

    /// <summary>Get data row value</summary>
    /// <param name="dataRow">The data row</param>
    /// <param name="column">The column name</param>
    /// <param name="defaultValue">The default value</param>
    /// <returns>The data row value</returns>
    public static T GetValue<T>(this DataRow dataRow, string column, T defaultValue = default)
    {
        if (dataRow == null)
        {
            throw new ArgumentNullException(nameof(dataRow));
        }
        if (string.IsNullOrWhiteSpace(column))
        {
            throw new ArgumentException(nameof(column));
        }

        var value = dataRow[column];
        if (value is null or DBNull)
        {
            return defaultValue;
        }
        if (value is T typeValue)
        {
            return typeValue;
        }
        if (value is string stringValue)
        {
            // json escaping
            stringValue = stringValue.Trim('"');
            return (T)JsonSerializer.Deserialize(stringValue, typeof(T));
        }

        try
        {
            return (T)Convert.ChangeType(value, typeof(T));
        }
        catch (Exception exception)
        {
            throw new ScriptException($"Error in column {column}: convert value {value} to type {typeof(T)}.", exception);
        }
    }

    /// <summary>Set data row value</summary>
    /// <param name="dataRow">The data row</param>
    /// <param name="column">The column name</param>
    /// <param name="value">The value to set</param>
    public static void SetValue<T>(this DataRow dataRow, string column, T value) =>
        SetValue(dataRow, column, value, typeof(T));

    /// <summary>Set data row value</summary>
    /// <param name="dataRow">The data row</param>
    /// <param name="column">The column name</param>
    /// <param name="value">The value to set</param>
    /// <param name="type">The value type</param>
    public static void SetValue(this DataRow dataRow, string column, object value, Type type = null)
    {
        if (string.IsNullOrWhiteSpace(column))
        {
            throw new ArgumentException(nameof(column));
        }

        type ??= typeof(string);
        dataRow.Table.EnsureColumn(column, type);
        dataRow[column] = value;
    }

    /// <summary>Get data row json value</summary>
    /// <param name="dataRow">The data row</param>
    /// <param name="column">The column name</param>
    /// <param name="defaultValue">The default value</param>
    /// <returns>The data row value</returns>
    public static T GetJsonValue<T>(this DataRow dataRow, string column, object defaultValue = null) =>
        (T)GetJsonValue(dataRow, column, typeof(T), defaultValue);

    /// <summary>Get data row json value</summary>
    /// <param name="dataRow">The data row</param>
    /// <param name="column">The column name</param>
    /// <param name="type">The value type</param>
    /// <param name="defaultValue">The default value</param>
    /// <returns>The data row value</returns>
    public static object GetJsonValue(this DataRow dataRow, string column,
        Type type, object defaultValue = null)
    {
        if (string.IsNullOrWhiteSpace(column))
        {
            throw new ArgumentException(nameof(column));
        }
        if (type == null)
        {
            throw new ArgumentNullException(nameof(type));
        }

        if (dataRow[column] is not string json)
        {
            return defaultValue;
        }
        if (type == typeof(string) && !json.StartsWith('"'))
        {
            return json;
        }
        if (type == typeof(DateTime) && DateTime.TryParse(json, out var dateValue))
        {
            return dateValue.ToUtc();
        }
        return string.IsNullOrWhiteSpace(json) ? defaultValue :
            JsonSerializer.Deserialize(json, type);
    }

    /// <summary>Set data row json value</summary>
    /// <param name="dataRow">The data row</param>
    /// <param name="column">The column name</param>
    /// <param name="value">The value to set</param>
    public static void SetJsonValue<T>(this DataRow dataRow, string column, T value) =>
        SetJsonValue(dataRow, typeof(T), column, value);

    /// <summary>Set data row json value</summary>
    /// <param name="dataRow">The data row</param>
    /// <param name="type">The value type</param>
    /// <param name="column">The column name</param>
    /// <param name="value">The value to set</param>
    public static void SetJsonValue(this DataRow dataRow, Type type, string column, object value)
    {
        if (type == null)
        {
            throw new ArgumentNullException(nameof(type));
        }
        if (string.IsNullOrWhiteSpace(column))
        {
            throw new ArgumentException(nameof(column));
        }
        if (value == null)
        {
            return;
        }
        dataRow[column] = JsonSerializer.Serialize(value);
    }

    /// <summary>Get default payroll value type</summary>
    /// <param name="dataRow">The data row</param>
    /// <returns>The payroll value tye</returns>
    public static ValueType GetPayrollValueType(this DataRow dataRow) =>
        GetPayrollValueType(dataRow, nameof(ValueType));

    /// <summary>Get payroll value type</summary>
    /// <param name="dataRow">The data row</param>
    /// <param name="column">The column name</param>
    /// <param name="defaultType">The default value type</param>
    /// <returns>The payroll value tye</returns>
    public static ValueType GetPayrollValueType(this DataRow dataRow,
        string column, ValueType defaultType = ValueType.String) =>
        GetEnumValue(dataRow, column, defaultType);

    /// <summary>Get default payroll value</summary>
    /// <param name="dataRow">The data row</param>
    /// <param name="defaultValue">The default value</param>
    /// <returns>The payroll value</returns>
    public static T GetPayrollValue<T>(this DataRow dataRow, T defaultValue = default) =>
        (T)GetPayrollValue(dataRow, (object)defaultValue);

    /// <summary>Get payroll value</summary>
    /// <param name="dataRow">The data row</param>
    /// <param name="valueColumn">The value column name</param>
    /// <param name="valueTypeColumn">The value type column name</param>
    /// <param name="defaultValue">The default value</param>
    /// <returns>The payroll value</returns>
    public static T GetPayrollValue<T>(this DataRow dataRow,
        string valueColumn, string valueTypeColumn, T defaultValue = default) =>
        (T)GetPayrollValue(dataRow, valueColumn, valueTypeColumn, (object)defaultValue);

    /// <summary>Get default payroll value</summary>
    /// <param name="dataRow">The data row</param>
    /// <param name="defaultValue">The default value</param>
    /// <returns>The payroll value</returns>
    public static object GetPayrollValue(this DataRow dataRow, object defaultValue = null) =>
        GetPayrollValue(dataRow, "Value", nameof(ValueType), defaultValue);

    /// <summary>Get payroll value</summary>
    /// <param name="dataRow">The data row</param>
    /// <param name="valueColumn">The value column name</param>
    /// <param name="valueTypeColumn">The value type column name</param>
    /// <param name="defaultValue">The default value</param>
    /// <returns>The payroll value</returns>
    public static object GetPayrollValue(this DataRow dataRow,
        string valueColumn, string valueTypeColumn, object defaultValue = null) =>
        GetJsonValue(dataRow, valueColumn,
            GetPayrollValueType(dataRow, valueTypeColumn).GetDataType(), defaultValue);

    /// <summary>Get data row localized name</summary>
    /// <param name="dataRow">The data row</param>
    /// <param name="culture">The culture</param>
    /// <returns>The localized data row name</returns>
    public static string GetLocalizedName(this DataRow dataRow, string culture) =>
        GetLocalizedValue(dataRow, "Name", culture);

    /// <summary>Get data row localized identifier</summary>
    /// <param name="dataRow">The data row</param>
    /// <param name="culture">The culture</param>
    /// <returns>The localized data row identifier</returns>
    public static string GetLocalizedIdentifier(this DataRow dataRow, string culture) =>
        GetLocalizedValue(dataRow, "Identifier", culture);

    /// <summary>Get data row localized value using the ValueColumnLocalizations column</summary>
    /// <param name="dataRow">The data row</param>
    /// <param name="valueColumn">The value column name</param>
    /// <param name="culture">The culture</param>
    /// <returns>The localized data row value</returns>
    public static string GetLocalizedValue(this DataRow dataRow, string valueColumn, string culture) =>
        GetLocalizedValue(dataRow, valueColumn, $"{valueColumn}Localizations", culture);

    /// <summary>Get data row localized value</summary>
    /// <param name="dataRow">The data row</param>
    /// <param name="valueColumn">The value column name</param>
    /// <param name="localizationColumn">The localization column name</param>
    /// <param name="culture">The culture</param>
    /// <returns>The localized data row value</returns>
    public static string GetLocalizedValue(this DataRow dataRow, string valueColumn, string localizationColumn,
        string culture)
    {
        if (dataRow == null)
        {
            throw new ArgumentNullException(nameof(dataRow));
        }

        var value = GetValue<string>(dataRow, valueColumn);

        culture ??= CultureInfo.CurrentCulture.Name;
        var valueLocalization = JsonSerializer.Deserialize<
            Dictionary<string, string>>(GetValue<string>(dataRow, localizationColumn));
        if (valueLocalization != null && valueLocalization.TryGetValue(culture, out var localValue))
        {
            value = localValue;
        }

        return value;
    }

    /// <summary>Get data row values</summary>
    /// <param name="dataRows">The data rows</param>
    /// <returns>The data rows values</returns>
    public static List<object> GetValues(this IEnumerable<DataRow> dataRows)
    {
        if (dataRows == null)
        {
            throw new ArgumentNullException(nameof(dataRows));
        }
        var values = new List<object>();
        foreach (DataRow row in dataRows)
        {
            foreach (DataColumn column in row.Table.Columns)
            {
                values.Add(row[column.ColumnName]);
            }
        }
        return values;
    }

    /// <summary>Get data rows value</summary>
    /// <param name="dataRows">The data rows</param>
    /// <param name="column">The column name</param>
    /// <param name="defaultValue">The default value</param>
    /// <returns>The data rows value</returns>
    public static List<T> GetValues<T>(this IEnumerable<DataRow> dataRows, string column, T defaultValue = default)
    {
        if (dataRows == null)
        {
            throw new ArgumentNullException(nameof(dataRows));
        }
        if (string.IsNullOrWhiteSpace(column))
        {
            throw new ArgumentException(nameof(column));
        }

        var values = new List<T>();
        foreach (DataRow dataRow in dataRows)
        {
            values.Add(GetValue(dataRow, column, defaultValue));
        }
        return values;
    }

    /// <summary>Get data row JSON value as list</summary>
    /// <param name="dataRow">The data row</param>
    /// <param name="column">The column name</param>
    /// <returns>The list</returns>
    public static List<T> GetListValue<T>(this DataRow dataRow, string column)
    {
        if (dataRow == null)
        {
            throw new ArgumentNullException(nameof(dataRow));
        }
        if (string.IsNullOrWhiteSpace(column))
        {
            throw new ArgumentException(nameof(column));
        }

        var value = dataRow[column];
        if (value is null or DBNull)
        {
            return [];
        }
        if (value is IEnumerable<T> enumerable)
        {
            return [.. enumerable];
        }
        if (value is string json)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                return [];
            }
            return JsonSerializer.Deserialize<List<T>>(json);
        }

        throw new ArgumentException($"{value} from column {column} is not a JSON list.", nameof(column));
    }

    /// <summary>Get data row JSON value as dictionary</summary>
    /// <param name="dataRow">The data row</param>
    /// <param name="column">The column name</param>
    /// <returns>The dictionary</returns>
    public static Dictionary<TKey, TValue> GetDictionary<TKey, TValue>(this DataRow dataRow, string column)
    {
        if (dataRow == null)
        {
            throw new ArgumentNullException(nameof(dataRow));
        }
        if (string.IsNullOrWhiteSpace(column))
        {
            throw new ArgumentException(nameof(column));
        }

        var value = dataRow[column];
        return value switch
        {
            null or DBNull => new(),
            IDictionary<TKey, TValue> dictionary => new(dictionary),
            string json => string.IsNullOrWhiteSpace(json)
                ? new()
                : JsonSerializer.Deserialize<Dictionary<TKey, TValue>>(json),
            _ => throw new ArgumentException($"{value} from column {column} is not a JSON dictionary.", nameof(column))
        };
    }

    /// <summary>Get attributes column value as attribute dictionary</summary>
    /// <param name="dataRow">The data row</param>
    /// <returns>The attributes dictionary</returns>
    public static Dictionary<string, object> GetAttributes(this DataRow dataRow) =>
        GetAttributes(dataRow, "Attributes");

    /// <summary>Get data row json value as attribute dictionary</summary>
    /// <param name="dataRow">The data row</param>
    /// <param name="column">The column name</param>
    /// <returns>The attributes dictionary</returns>
    public static Dictionary<string, object> GetAttributes(this DataRow dataRow, string column) =>
        GetDictionary<string, object>(dataRow, column);

    /// <summary>Get value from attributes column</summary>
    /// <param name="dataRow">The data row</param>
    /// <param name="attribute">The attribute name</param>
    /// <param name="defaultValue">The default value</param>
    /// <returns>The attribute value</returns>
    public static T GetAttribute<T>(this DataRow dataRow, string attribute, T defaultValue = default) =>
        GetAttribute(dataRow, "Attributes", attribute, defaultValue);

    /// <summary>Get attribute from a data row json value</summary>
    /// <param name="dataRow">The data row</param>
    /// <param name="column">The column name</param>
    /// <param name="attribute">The attribute name</param>
    /// <param name="defaultValue">The default value</param>
    /// <returns>The attribute value</returns>
    public static T GetAttribute<T>(this DataRow dataRow, string column, string attribute, T defaultValue = default) =>
        (T)Convert.ChangeType(GetAttribute(dataRow, column, attribute, (object)defaultValue), typeof(T));

    /// <summary>Get value from attributes column</summary>
    /// <param name="dataRow">The data row</param>
    /// <param name="attribute">The attribute name</param>
    /// <param name="defaultValue">The default value</param>
    /// <returns>The attribute value</returns>
    public static object GetAttribute(this DataRow dataRow, string attribute, object defaultValue = null) =>
        GetAttribute(dataRow, "Attributes", attribute, defaultValue);

    /// <summary>Get attribute from a data row json value</summary>
    /// <param name="dataRow">The data row</param>
    /// <param name="column">The column name</param>
    /// <param name="attribute">The attribute name</param>
    /// <param name="defaultValue">The default value</param>
    /// <returns>The attribute value</returns>
    public static object GetAttribute(this DataRow dataRow, string column, string attribute, object defaultValue = null)
    {
        if (string.IsNullOrWhiteSpace(attribute))
        {
            throw new ArgumentException(nameof(attribute));
        }

        var attributes = GetAttributes(dataRow, column);
        if (!attributes.TryGetValue(attribute, out var value))
        {
            return defaultValue;
        }

        if (value is JsonElement jsonElement)
        {
            value = jsonElement.GetValue();
        }
        return value ?? defaultValue;
    }

    /// <summary>Get data row json value as localizations dictionary</summary>
    /// <param name="dataRow">The data row</param>
    /// <param name="column">The column name</param>
    /// <returns>The attributes dictionary</returns>
    public static Dictionary<string, string> GetLocalizations(this DataRow dataRow, string column) =>
        GetDictionary<string, string>(dataRow, column);

    /// <summary>Get attribute from a data row json value</summary>
    /// <param name="dataRow">The data row</param>
    /// <param name="column">The column name</param>
    /// <param name="culture">The culture</param>
    /// <param name="defaultValue">The default value</param>
    /// <returns>The attribute value</returns>
    public static string GetLocalization(this DataRow dataRow, string column, string culture, string defaultValue = null) =>
        culture.GetLocalization(GetLocalizations(dataRow, column), defaultValue);

    /// <summary>Transpose item collection to table columns with values</summary>
    /// <remarks>Use the function return value null, to suppress further item operations</remarks>
    /// <param name="target">The target data row</param>
    /// <param name="items">The items to transpose</param>
    /// <param name="columnName">The column name function (mandatory)</param>
    /// <param name="itemValue">The item value function (mandatory)</param>
    /// <param name="columnType">The column type function, default is string</param>
    /// <param name="defaultValue">The default value function, default is none</param>
    public static void TransposeFrom<T>(this DataRow target, IEnumerable<T> items,
        Func<T, string> columnName, Func<T, object> itemValue,
        Func<T, Type> columnType = null, Func<T, object> defaultValue = null)
    {
        if (items == null)
        {
            throw new ArgumentNullException(nameof(items));
        }
        if (columnName == null)
        {
            throw new ArgumentNullException(nameof(columnName));
        }
        if (itemValue == null)
        {
            throw new ArgumentNullException(nameof(itemValue));
        }

        foreach (var item in items)
        {
            // column name
            var name = columnName(item);
            if (string.IsNullOrWhiteSpace(name))
            {
                continue;
            }

            //  column
            DataColumn column;
            if (!target.Table.Columns.Contains(name))
            {
                // new column
                Type type = columnType?.Invoke(item) ?? typeof(T);
                column = new(name, type);

                // default value
                if (defaultValue != null)
                {
                    column.DefaultValue = defaultValue.Invoke(item);
                }
                target.Table.Columns.Add(column);
            }
            else
            {
                column = target.Table.Columns[name];
            }
            if (column == null)
            {
                continue;
            }

            // item/row value
            var value = itemValue(item);
            if (value == null)
            {
                continue;
            }
            if (value is JsonElement jsonElement)
            {
                value = jsonElement.GetValue();
            }
            target[name] = value;
        }
    }

    /// <summary>Transpose dictionary to table columns with values</summary>
    /// <remarks>Use the function return value null, to suppress further item operations
    /// Transpose dynamic object: row.TransposeFrom((IDictionary&lt;string, object&gt;)dynamicObject);</remarks>
    /// <param name="target">The target data row</param>
    /// <param name="items">The items to transpose</param>
    /// <param name="columnName">The column name function (mandatory)</param>
    /// <param name="itemValue">The item value function (mandatory)</param>
    /// <param name="columnType">The column type function, default is string</param>
    /// <param name="defaultValue">The default value function, default is none</param>
    public static void TransposeFrom<TKey, TValue>(this DataRow target, IDictionary<TKey, TValue> items,
        Func<TKey, string> columnName = null, Func<TValue, object> itemValue = null,
        Func<TKey, Type> columnType = null, Func<TKey, object> defaultValue = null)
    {
        if (items == null)
        {
            throw new ArgumentNullException(nameof(items));
        }

        foreach (var item in items)
        {
            // column name
            var name = item.Key.ToString();
            if (columnName != null)
            {
                name = columnName(item.Key);
            }
            if (string.IsNullOrWhiteSpace(name))
            {
                continue;
            }

            //  column
            DataColumn column;
            if (!target.Table.Columns.Contains(name))
            {
                // new column
                Type type = columnType?.Invoke(item.Key) ?? typeof(TValue);
                column = new(name, type);

                // default value
                if (defaultValue != null)
                {
                    column.DefaultValue = defaultValue.Invoke(item.Key);
                }
                target.Table.Columns.Add(column);
            }
            else
            {
                column = target.Table.Columns[name];
            }
            if (column == null)
            {
                continue;
            }

            // item/row value
            var value = item.Value as object;
            if (itemValue != null)
            {
                value = itemValue(item.Value);
            }
            if (value == null)
            {
                continue;
            }
            if (value is JsonElement jsonElement)
            {
                value = jsonElement.GetValue();
            }
            target[name] = value;
        }
    }

    /// <summary>Transpose data row items to table columns with values</summary>
    /// <remarks>Use the function return value null, to suppress further item operations</remarks>
    /// <param name="target">The target data row</param>
    /// <param name="source">The source data row</param>
    /// <param name="columnName">The column name function, default is the source column name</param>
    /// <param name="itemValue">The item value function, default is the source column value</param>
    /// <param name="columnType">The column type function, default is the source column type</param>
    /// <param name="defaultValue">The default value function, default is none</param>
    public static void TransposeFrom(this DataRow target, DataRow source,
        Func<DataColumn, string> columnName = null, Func<object, object> itemValue = null,
        Func<DataColumn, Type> columnType = null, Func<DataColumn, object> defaultValue = null)
    {
        if (source == null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        foreach (DataColumn sourceColumn in source.Table.Columns)
        {
            // column name
            var name = sourceColumn.ColumnName;
            if (columnName != null)
            {
                name = columnName(sourceColumn);
            }
            if (string.IsNullOrWhiteSpace(name))
            {
                continue;
            }

            //  new column
            DataColumn targetColumn;
            if (!target.Table.Columns.Contains(name))
            {
                // column type
                Type type = sourceColumn.DataType;
                if (columnType != null)
                {
                    type = columnType.Invoke(sourceColumn);
                }
                if (columnType == null)
                {
                    continue;
                }

                // create column
                targetColumn = new(name, type);

                // default value
                if (defaultValue != null)
                {
                    targetColumn.DefaultValue = defaultValue.Invoke(sourceColumn);
                }
                target.Table.Columns.Add(targetColumn);
            }
            else
            {
                targetColumn = target.Table.Columns[name];
            }
            if (targetColumn == null)
            {
                continue;
            }
            if (targetColumn.DataType != sourceColumn.DataType)
            {
                throw new ScriptException($"Mismatching types in column {sourceColumn.ColumnName}:" +
                                           $" source: {sourceColumn.DataType.Name}, target: {targetColumn.DataType.Name}.");
            }

            // item/row value
            var value = source[sourceColumn];
            if (itemValue != null)
            {
                value = itemValue(value);
            }
            if (value == null)
            {
                continue;
            }
            if (value is JsonElement jsonElement)
            {
                value = jsonElement.GetValue();
            }
            target[name] = value;
        }
    }
}

#endregion
