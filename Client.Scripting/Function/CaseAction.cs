/* CaseAction */

using System;
using System.Text.Json;
using System.Reflection;
using System.Diagnostics;
using System.Globalization;
using System.Collections.Generic;

namespace PayrollEngine.Client.Scripting.Function;

#region Enum

/// <summary>Action value source</summary>
public enum ActionValueSource
{
    /// <summary>Case value</summary>
    Value,

    /// <summary>Case value start date</summary>
    Start,

    /// <summary>Case value end date</summary>
    End,

    /// <summary>Case date period</summary>
    Period,

    /// <summary>Case field attribute</summary>
    FieldAttribute,

    /// <summary>Case value attribute</summary>
    ValueAttribute,

    /// <summary>Lookup value</summary>
    Lookup,

    /// <summary>Range lookup value</summary>
    RangeLookup
}

/// <summary>Action value reference type</summary>
public enum ActionValueReferenceType
{
    /// <summary>None</summary>
    None,

    /// <summary>Case change</summary>
    CaseChange,

    /// <summary>Case value</summary>
    CaseValue,

    /// <summary>Lookup</summary>
    Lookup
}

/// <summary>Action value type</summary>
public enum ActionValueType
{
    /// <summary>No type</summary>
    None,
    /// <summary>String type</summary>
    String,
    /// <summary>Boolean type</summary>
    Boolean,
    /// <summary>Integer type</summary>
    Integer,
    /// <summary>Decimal type</summary>
    Decimal,
    /// <summary>DateTime type</summary>
    DateTime,
    /// <summary>TimeSpan type</summary>
    TimeSpan
}

/// <summary>Extensions for <see cref="ValueType"/></summary>
public static class ValueTypeExtensions
{
    /// <summary>Get the action value type</summary>
    /// <param name="valueType">The value type</param>
    /// <returns>The action value type</returns>
    public static ActionValueType ToActionValueType(this ValueType valueType)
    {
        if (valueType.IsInteger())
        {
            return ActionValueType.Integer;
        }
        if (valueType.IsDecimal())
        {
            return ActionValueType.Decimal;
        }
        if (valueType.IsString())
        {
            return ActionValueType.String;
        }
        if (valueType.IsDateTime())
        {
            return ActionValueType.DateTime;
        }
        if (valueType.IsBoolean())
        {
            return ActionValueType.Boolean;
        }
        return ActionValueType.None;
    }
}

#endregion

#region Action Context

/// <summary>Action case value</summary>
public interface IActionCaseValueContext
{
    /// <summary>The function</summary>
    PayrollFunction Function { get; }
}

/// <summary>Action case value context</summary>
public interface IActionCaseValueContext<out TFunc> : IActionCaseValueContext
    where TFunc : PayrollFunction
{
    /// <summary>The function</summary>
    new TFunc Function { get; }

    /// <summary>Get case value type</summary>
    /// <param name="caseFieldName">The case field name</param>
    /// <returns>The case value type</returns>
    ValueType? GetCaseValueType(string caseFieldName);

    /// <summary>Get case value</summary>
    /// <param name="caseFieldName">The case field name</param>
    /// <param name="caseValueDate">The case value date</param>
    /// <returns>The case value</returns>
    CaseValue GetCaseValue(string caseFieldName, DateTime caseValueDate);

    /// <summary>Get case field attribute value</summary>
    /// <param name="caseFieldName">The case field name</param>
    /// <param name="attributeName">The attribute name</param>
    /// <returns>The case value</returns>
    T GetCaseFieldAttribute<T>(string caseFieldName, string attributeName);

    /// <summary>Get case value attribute value</summary>
    /// <param name="caseFieldName">The case field name</param>
    /// <param name="attributeName">The attribute name</param>
    /// <returns>The case value</returns>
    T GetCaseValueAttribute<T>(string caseFieldName, string attributeName);

    /// <summary>Get case value</summary>
    /// <param name="caseFieldName">The case field name</param>
    /// <returns>The case value</returns>
    CaseValue GetCaseChangeValue(string caseFieldName);

    /// <summary>Get lookup value</summary>
    /// <param name="lookupName">The lookup name</param>
    /// <param name="lookupKey">The lookup key</param>
    /// <returns>The case value</returns>
    T GetLookup<T>(string lookupName, string lookupKey);

    /// <summary>Get lookup value</summary>
    /// <param name="lookupName">The lookup name</param>
    /// <param name="lookupKey">The lookup key</param>
    /// <param name="objectKey">The object key</param>
    /// <returns>The case value</returns>
    T GetLookup<T>(string lookupName, string lookupKey, string objectKey);

    /// <summary>Get range lookup value</summary>
    /// <param name="lookupName">The lookup name</param>
    /// <param name="rangeValue">The lookup range value</param>
    /// <param name="lookupKey">The object key</param>
    /// <returns>The case value</returns>
    T GetRangeLookup<T>(string lookupName, decimal rangeValue, string lookupKey = null);

    /// <summary>Get range lookup value</summary>
    /// <param name="lookupName">The lookup name</param>
    /// <param name="rangeValue">The lookup range value</param>
    /// <param name="objectKey">The object key</param>
    /// <param name="lookupKey">The object key</param>
    /// <returns>The case value</returns>
    T GetRangeObjectLookup<T>(string lookupName, decimal rangeValue,
        string objectKey, string lookupKey = null);
}

/// <summary>Action case value context base</summary>
public abstract class ActionCaseValueContextBase<TFunc> : IActionCaseValueContext<TFunc>
    where TFunc : PayrollFunction
{
    PayrollFunction IActionCaseValueContext.Function => Function;

    /// <inheritdoc />
    public TFunc Function { get; }

    /// <summary>Constructor</summary>
    /// <param name="function">The function</param>
    protected ActionCaseValueContextBase(TFunc function)
    {
        Function = function ?? throw new ArgumentNullException(nameof(function));
    }

    /// <inheritdoc />
    public virtual ValueType? GetCaseValueType(string caseFieldName)
    {
        throw new NotSupportedException();
    }

    /// <inheritdoc />
    public virtual CaseValue GetCaseValue(string caseFieldName, DateTime caseValueDate)
    {
        throw new NotSupportedException();
    }

    /// <inheritdoc />
    public virtual T GetCaseFieldAttribute<T>(string caseFieldName, string attributeName)
    {
        throw new NotSupportedException();
    }

    /// <inheritdoc />
    public virtual T GetCaseValueAttribute<T>(string caseFieldName, string attributeName)
    {
        throw new NotSupportedException();
    }

    /// <inheritdoc />
    public virtual CaseValue GetCaseChangeValue(string caseFieldName)
    {
        throw new NotSupportedException();
    }

    /// <inheritdoc />
    public virtual T GetLookup<T>(string lookupName, string lookupKey)
    {
        throw new NotSupportedException();
    }

    /// <inheritdoc />
    public virtual T GetLookup<T>(string lookupName, string lookupKey, string objectKey)
    {
        throw new NotSupportedException();
    }

    /// <inheritdoc />
    public virtual T GetRangeLookup<T>(string lookupName, decimal rangeValue, string lookupKey = null)
    {
        throw new NotSupportedException();
    }

    /// <inheritdoc />
    public virtual T GetRangeObjectLookup<T>(string lookupName, decimal rangeValue,
        string objectKey, string lookupKey = null)
    {
        throw new NotSupportedException();
    }
}

/// <summary>Payroll action case value</summary>
public abstract class PayrollActionValueContext : PayrollActionValueContext<PayrollFunction>
{
    /// <summary>Constructor</summary>
    /// <param name="function">The function</param>
    protected PayrollActionValueContext(PayrollFunction function) :
        base(function)
    {
    }
}

/// <summary>Payroll action case value</summary>
public abstract class PayrollActionValueContext<TFunc> : ActionCaseValueContextBase<TFunc>
    where TFunc : PayrollFunction
{
    /// <summary>Constructor</summary>
    /// <param name="function">The function</param>
    protected PayrollActionValueContext(TFunc function) :
        base(function)
    {
    }

    /// <inheritdoc />
    public override CaseValue GetCaseValue(string caseFieldName, DateTime caseValueDate) =>
        Function.GetRawCaseValue(caseFieldName, caseValueDate);

    /// <inheritdoc />
    public override ValueType? GetCaseValueType(string caseFieldName) =>
        Function.GetCaseValueType(caseFieldName);

    /// <inheritdoc />
    public override T GetCaseFieldAttribute<T>(string caseFieldName, string attributeName) =>
        Function.GetCaseFieldAttribute<T>(caseFieldName, attributeName);

    /// <inheritdoc />
    public override T GetCaseValueAttribute<T>(string caseFieldName, string attributeName) =>
        Function.GetCaseValueAttribute<T>(caseFieldName, attributeName);

    /// <inheritdoc />
    public override T GetLookup<T>(string lookupName, string lookupKey) =>
        Function.GetLookup<T>(lookupName, lookupKey);

    /// <inheritdoc />
    public override T GetLookup<T>(string lookupName, string lookupKey, string objectKey) =>
        Function.GetLookup<T>(lookupName, lookupKey, objectKey);

    /// <inheritdoc />
    public override T GetRangeLookup<T>(string lookupName, decimal rangeValue, string lookupKey = null) =>
        Function.GetRangeLookup<T>(lookupName, rangeValue, lookupKey);

    /// <inheritdoc />
    public override T GetRangeObjectLookup<T>(string lookupName, decimal rangeValue,
        string objectKey, string lookupKey = null) =>
        Function.GetRangeObjectLookup<T>(lookupName, rangeValue, objectKey, lookupKey);
}

#endregion

#region Action Method

/// <summary>Action method</summary>
public interface IActionMethod
{
    /// <summary>Get the value type</summary>
    ActionValueType GetValueType();

    /// <summary>Evaluate the value</summary>
    object EvaluateValue(object value);
}

/// <summary>Action method</summary>
public interface IActionMethod<in T> : IActionMethod
{
    /// <summary>Evaluate the value</summary>
    object EvaluateValue(T value);
}

/// <summary>Action value method</summary>
public abstract class ActionMethodBase<TContext, TFunc, TValue> : IActionMethod<TValue>
    where TContext : IActionCaseValueContext<TFunc>
    where TFunc : PayrollFunction
{
    /// <summary>Value mode postfix</summary>
    private const string ValueModePostfix = ".Value";

    /// <summary>Start mode postfix</summary>
    private const string StartModePostfix = ".Start";

    /// <summary>End mode postfix</summary>
    private const string EndModePostfix = ".End";

    /// <summary>Period mode postfix</summary>
    private const string PeriodModePostfix = ".Period";

    /// <summary>Case field  attribute mode postfix</summary>
    private const string FieldAttributeModePostfix = ".FieldAtttribute";

    /// <summary>Case value attribute mode postfix</summary>
    private const string ValueAttributeModePostfix = ".ValueAtttribute";

    /// <summary>Lookup value attribute mode postfix</summary>
    private const string LookupModePostfix = ".Lookup";

    /// <summary>Range lookup value attribute mode postfix</summary>
    private const string RangeLookupModePostfix = ".RangeLookup";

    /// <summary>Case change reference</summary>
    private const char CaseChangeReference = '#';

    /// <summary>Case value reference</summary>
    private const char CaseValueReference = '@';

    /// <summary>Lookup value reference</summary>
    private const char LookupValueReference = '$';

    /// <summary>The method expression</summary>
    public string Expression { get; }

    /// <summary>The method name</summary>
    public string Name { get; }

    /// <summary>The function</summary>
    public TContext Context { get; }

    /// <summary>The case value date</summary>
    public DateTime CaseValueDate { get; }

    /// <summary>The method parameters</summary>
    public List<string> Parameters { get; } = [];

    /// <summary>Test for method parameters</summary>
    public bool HasParameters => Parameters.Count > 0;

    /// <summary>The method parameter count</summary>
    public int ParameterCount => Parameters.Count;

    /// <summary>The function culture</summary>
    public string UserCulture => Context.Function.UserCulture;

    /// <summary>Constructor</summary>
    protected ActionMethodBase(TContext context, string expression, DateTime caseValueDate)
    {
        if (string.IsNullOrWhiteSpace(expression))
        {
            throw new ArgumentException(nameof(expression));
        }
        Expression = expression;
        Name = ParseMethod(context, expression, Parameters);
        Context = context;
        CaseValueDate = caseValueDate;
    }

    #region Register

    /// <summary>The value functions</summary>
    private readonly Dictionary<string, Tuple<ActionValueType, Func<TValue, object>>> valueFunctions = new();

    /// <summary>Register value function</summary>
    /// <param name="name">The function name</param>
    /// <param name="valueType">The value type</param>
    /// <param name="evaluate">The function code</param>
    /// <exception cref="ScriptException"></exception>
    protected void RegisterFunction(string name, ActionValueType valueType, Func<TValue, object> evaluate)
    {
        if (valueFunctions.ContainsKey(name))
        {
            throw new ScriptException($"Duplicated action value function: {name}.");
        }
        valueFunctions.Add(name, new(valueType, evaluate));
    }

    private Tuple<ActionValueType, Func<TValue, object>> GetValueFunction()
    {
        if (!valueFunctions.TryGetValue(Name, out var function))
        {
            throw new ScriptException($"Missing action function {Name}.");
        }
        return function;
    }

    #endregion

    /// <summary>Get the value type</summary>
    public virtual ActionValueType GetValueType() =>
        GetValueFunction().Item1;

    /// <summary>Get the resolved value</summary>
    /// <param name="value">The value</param>
    /// <returns>The resolved value</returns>
    public virtual object EvaluateValue(TValue value) =>
        GetValueFunction().Item2(value);

    object IActionMethod.EvaluateValue(object value) =>
        EvaluateValue((TValue)value);

    /// <summary>>Get method parameter</summary>
    /// <param name="index">The parameter index</param>
    /// <param name="result">The parameter value</param>
    /// <returns>True for a valid parameter value</returns>
    protected bool TryGetParameter<T>(int index, out T result)
    {
        if (index >= Parameters.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }

        var startParameter = Parameters[index];
        if (string.IsNullOrWhiteSpace(startParameter))
        {
            result = default;
            return false;
        }

        var referenceType = GetReferenceType(startParameter, out var parameter);
        var valueSource = GetValueSource(parameter, out parameter);
        //Function.LogWarning($"GetParameter {Name} ({typeof(T)}): referenceType={referenceType}, valueSource={valueSource}, parameter={parameter}");

        // case value
        var caseValue = GetCaseValue<T>(referenceType, parameter);

        // value source
        object value = null;
        switch (valueSource)
        {
            case ActionValueSource.Value:
                value = caseValue != null ? caseValue.Value?.Value : parameter;
                break;
            case ActionValueSource.Start:
                if (caseValue != null)
                {
                    value = caseValue.Start;
                }
                else if (DateTime.TryParse(parameter, CultureInfo.InvariantCulture,
                             DateTimeStyles.AdjustToUniversal, out var start))
                {
                    value = start;
                }
                break;
            case ActionValueSource.End:
                if (caseValue != null)
                {
                    value = caseValue.End;
                }
                else if (DateTime.TryParse(parameter, CultureInfo.InvariantCulture,
                             DateTimeStyles.AdjustToUniversal, out var end))
                {
                    value = end;
                }
                break;
            case ActionValueSource.Period:
                if (caseValue != null && caseValue.Start.HasValue && caseValue.End.HasValue)
                {
                    value = caseValue.End.Value.Subtract(caseValue.Start.Value);
                }
                else if (TimeSpan.TryParse(parameter, CultureInfo.InvariantCulture, out var period))
                {
                    value = period;
                }
                break;
            case ActionValueSource.FieldAttribute:
            case ActionValueSource.ValueAttribute:
            case ActionValueSource.Lookup:
            case ActionValueSource.RangeLookup:
                throw new ScriptException("No parameter support for case value attributes.");
        }

        if (value == null)
        {
            result = default;
            return false;
        }

        // convert value
        var type = typeof(T);
        var underlyingType = Nullable.GetUnderlyingType(type);
        try
        {
            //Function.LogWarning($"GetParameter: value={value}, type={underlyingType ?? type}, parameter={startParameter}");
            result = (T)Convert.ChangeType(value, underlyingType ?? type, CultureInfo.InvariantCulture);
            //Function.LogWarning($"GetParameter: valueMode={ValueMode}, ReferenceType={ReferenceType}, result={result}");
            return true;
        }
        catch (Exception exception)
        {
            Context.Function.LogError($"Parameter convert error of value {value} (type={value.GetType()}): {exception.GetBaseException().Message}");
            result = default;
            return false;
        }
    }

    /// <summary>Get case value</summary>
    /// <param name="referenceType">The reference type</param>
    /// <param name="parameter">The parameter</param>
    protected virtual CaseValue GetCaseValue<T>(ActionValueReferenceType referenceType, string parameter)
    {
        CaseValue caseValue = null;
        switch (referenceType)
        {
            case ActionValueReferenceType.CaseChange:
                caseValue = Context.GetCaseChangeValue(parameter);
                break;
            case ActionValueReferenceType.CaseValue:
                caseValue = Context.GetCaseValue(parameter, CaseValueDate);
                break;
        }
        return caseValue;
    }

    private static ActionValueReferenceType GetReferenceType(string expression, out string result)
    {
        if (expression.StartsWith(CaseChangeReference))
        {
            result = expression.TrimStart(CaseChangeReference);
            return ActionValueReferenceType.CaseChange;
        }
        if (expression.StartsWith(CaseValueReference))
        {
            result = expression.TrimStart(CaseValueReference);
            return ActionValueReferenceType.CaseValue;
        }
        if (expression.StartsWith(LookupValueReference))
        {
            result = expression.TrimStart(LookupValueReference);
            return ActionValueReferenceType.Lookup;
        }

        result = expression;
        return ActionValueReferenceType.None;
    }

    private static ActionValueSource GetValueSource(string expression, out string result)
    {
        // start
        if (expression.EndsWith(StartModePostfix))
        {
            result = expression.RemoveFromEnd(StartModePostfix);
            return ActionValueSource.Start;
        }
        // end
        if (expression.EndsWith(EndModePostfix))
        {
            result = expression.RemoveFromEnd(EndModePostfix);
            return ActionValueSource.End;
        }
        // period
        if (expression.EndsWith(PeriodModePostfix))
        {
            result = expression.RemoveFromEnd(PeriodModePostfix);
            return ActionValueSource.Period;
        }
        // field attribute
        if (expression.EndsWith(FieldAttributeModePostfix))
        {
            result = expression.RemoveFromEnd(FieldAttributeModePostfix);
            return ActionValueSource.FieldAttribute;
        }
        // value attribute
        if (expression.EndsWith(ValueAttributeModePostfix))
        {
            result = expression.RemoveFromEnd(ValueAttributeModePostfix);
            return ActionValueSource.ValueAttribute;
        }
        // lookup attribute
        if (expression.EndsWith(LookupModePostfix))
        {
            result = expression.RemoveFromEnd(LookupModePostfix);
            return ActionValueSource.Lookup;
        }
        // range lookup attribute
        if (expression.EndsWith(RangeLookupModePostfix))
        {
            result = expression.RemoveFromEnd(RangeLookupModePostfix);
            return ActionValueSource.RangeLookup;
        }

        // value (default)
        result = expression.RemoveFromEnd(ValueModePostfix);
        return ActionValueSource.Value;
    }

    private static string ParseMethod(TContext context, string expression, List<string> parameters)
    {
        var paramStartIndex = expression.IndexOf('(');
        var paramEndIndex = expression.IndexOf(')');

        // method without parameters
        if (paramStartIndex < 0 && paramEndIndex < 0)
        {
            return expression;
        }

        // method with invalid parameters
        if ((paramStartIndex >= 0 || paramEndIndex >= 0) &&
            (paramStartIndex < 0 && paramEndIndex >= 0 ||
             paramEndIndex < 0 && paramStartIndex >= 0 ||
             paramStartIndex > paramEndIndex))
        {
            context.Function.LogError($"Invalid method expression: {expression}");
            return null;
        }

        // method name
        var name = expression.Substring(0, paramStartIndex);
        if (string.IsNullOrWhiteSpace(name))
        {
            context.Function.LogError($"Invalid method name in expression: {expression}");
            return null;
        }

        // method parameters
        var paramText = expression.Substring(paramStartIndex + 1, paramEndIndex - paramStartIndex - 1);
        if (!string.IsNullOrWhiteSpace(paramText))
        {
            foreach (var param in paramText.Split(','))
            {
                parameters.Add(param.Trim());
            }
        }
        return name;
    }

    /// <summary>String representation</summary>
    public override string ToString() => $"{Name} ({Parameters.Count} params)";
}

/// <summary>Boolean action value method</summary>
public class BooleanActionValueMethod<TContext, TFunc> : ActionMethodBase<TContext, TFunc, bool>
    where TContext : IActionCaseValueContext<TFunc>
    where TFunc : PayrollFunction
{
    /// <summary>Constructor</summary>
    public BooleanActionValueMethod(TContext context, string expression, DateTime caseValueDate) :
        base(context, expression, caseValueDate)
    {
        RegisterFunction("Negate", ActionValueType.Boolean, value => !value);
    }
}

/// <summary>Integer action value method</summary>
public class IntegerActionValueMethod<TContext, TFunc> : ActionMethodBase<TContext, TFunc, int>
    where TContext : IActionCaseValueContext<TFunc>
    where TFunc : PayrollFunction
{
    /// <summary>Constructor</summary>
    public IntegerActionValueMethod(TContext context, string expression, DateTime caseValueDate) :
        base(context, expression, caseValueDate)
    {
        RegisterTestFunctions();
        RegisterModificationFunctions();
        RegisterConvertFunctions();
    }

    private void RegisterTestFunctions()
    {
        RegisterFunction("IsNegative", ActionValueType.Boolean, value => value < 0);
        RegisterFunction("IsZero", ActionValueType.Boolean, value => value == 0);
        RegisterFunction("IsPositive", ActionValueType.Boolean, value => value > 0);
        RegisterFunction("Within", ActionValueType.Boolean, value =>
        {
            if (ParameterCount == 2 && TryGetParameter<int>(0, out var withinMin) &&
                TryGetParameter<int>(1, out var withinMax))
            {
                return value < withinMin ? withinMin : value > withinMax ? withinMax : value;
            }

            return null;
        });
    }

    private void RegisterModificationFunctions()
    {
        // negate
        RegisterFunction("Negate", ActionValueType.Integer, value => value * -1);
        // base math operations
        RegisterFunction("Add", ActionValueType.Integer, value =>
        {
            if (ParameterCount == 1 && TryGetParameter<int>(0, out var add))
            {
                return value + add;
            }

            return null;
        });
        RegisterFunction("Subtract", ActionValueType.Integer, value =>
        {
            if (ParameterCount == 1 && TryGetParameter<int>(0, out var subtract))
            {
                return value - subtract;
            }

            return null;
        });
        // range
        RegisterFunction("Limit", ActionValueType.Integer, value =>
        {
            if (ParameterCount == 2 && TryGetParameter<int>(0, out var limitMin) &&
                TryGetParameter<int>(1, out var limitMax))
            {
                return value < limitMin ? limitMin : value > limitMax ? limitMax : value;
            }

            return null;
        });
        RegisterFunction("Min", ActionValueType.Integer, value =>
        {
            if (ParameterCount == 1 && TryGetParameter<int>(0, out var min))
            {
                return value < min ? min : value;
            }

            return null;
        });
        RegisterFunction("Max", ActionValueType.Integer, value =>
        {
            if (ParameterCount == 1 && TryGetParameter<int>(0, out var max))
            {
                return value > max ? max : value;
            }

            return null;
        });
    }

    private void RegisterConvertFunctions()
    {
        RegisterFunction("ToDecimal", ActionValueType.Decimal, value => new decimal(value));
        RegisterFunction(nameof(int.ToString), ActionValueType.String, value => value.ToString());
    }
}

/// <summary>Decimal action value method</summary>
public class DecimalActionValueMethod<TContext, TFunc> : ActionMethodBase<TContext, TFunc, decimal>
    where TContext : IActionCaseValueContext<TFunc>
    where TFunc : PayrollFunction
{
    /// <summary>Constructor</summary>
    public DecimalActionValueMethod(TContext context, string expression, DateTime caseValueDate) :
        base(context, expression, caseValueDate)
    {
        RegisterTestFunctions();
        RegisterModificationFunctions();
        RegisterConvertFunctions();
    }

    private void RegisterTestFunctions()
    {
        RegisterFunction("IsNegative", ActionValueType.Boolean, value => value < 0);
        RegisterFunction("IsZero", ActionValueType.Boolean, value => value == 0);
        RegisterFunction("IsPositive", ActionValueType.Boolean, value => value > 0);
        RegisterFunction("IsFraction", ActionValueType.Boolean, value => value - Math.Truncate(value) != 0);
        RegisterFunction("Within", ActionValueType.Boolean, value =>
        {
            if (ParameterCount == 2 && TryGetParameter<decimal>(0, out var withinMin) &&
                TryGetParameter<decimal>(1, out var withinMax))
            {
                return value < withinMin ? withinMin : value > withinMax ? withinMax : value;
            }
            return null;
        });
    }

    private void RegisterModificationFunctions()
    {
        // negate
        RegisterFunction("Negate", ActionValueType.Decimal, value => value * -1);
        // base math
        RegisterFunction(nameof(decimal.Add), ActionValueType.Decimal, value =>
        {
            if (ParameterCount == 1 && TryGetParameter<decimal>(0, out var add))
            {
                return decimal.Add(value, add);
            }
            return null;

        });
        RegisterFunction(nameof(decimal.Subtract), ActionValueType.Decimal, value =>
        {
            if (ParameterCount == 1 && TryGetParameter<decimal>(0, out var subtract))
            {
                return decimal.Subtract(value, subtract);
            }
            return null;
        });
        RegisterFunction(nameof(decimal.Divide), ActionValueType.Decimal, value =>
        {
            if (ParameterCount == 1 && TryGetParameter<decimal>(0, out var divide))
            {
                return decimal.Divide(value, divide);
            }
            return null;
        });
        RegisterFunction(nameof(decimal.Multiply), ActionValueType.Decimal, value =>
        {
            if (ParameterCount == 1 && TryGetParameter<decimal>(0, out var multiply))
            {
                return decimal.Multiply(value, multiply);
            }
            return null;
        });
        // range limits
        RegisterFunction("Limit", ActionValueType.Decimal, value =>
        {
            if (ParameterCount == 2 && TryGetParameter<decimal>(0, out var limitMin) &&
                TryGetParameter<decimal>(1, out var limitMax))
            {
                return value < limitMin ? limitMin : value > limitMax ? limitMax : value;
            }
            return null;
        });
        RegisterFunction("Min", ActionValueType.Decimal, value =>
        {
            if (ParameterCount == 1 && TryGetParameter<decimal>(0, out var min))
            {
                return value < min ? min : value;
            }
            return null;
        });
        RegisterFunction("Max", ActionValueType.Decimal, value =>
        {
            if (ParameterCount == 1 && TryGetParameter<decimal>(0, out var max))
            {
                return value > max ? max : value;
            }
            return null;
        });
        // round
        RegisterFunction(nameof(decimal.Round), ActionValueType.Decimal, value => decimal.Round(value));
        RegisterFunction("RoundUp", ActionValueType.Decimal, value =>
        {
            if (ParameterCount == 1 && TryGetParameter<decimal>(0, out var roundUp))
            {
                return roundUp == 0 ? value : Math.Ceiling(value / roundUp) * roundUp;
            }
            return null;
        });
        RegisterFunction("RoundDown", ActionValueType.Decimal, value =>
        {
            if (ParameterCount == 1 && TryGetParameter<decimal>(0, out var roundDown))
            {
                return roundDown == 0 ? value : Math.Floor(value / roundDown) * roundDown;
            }
            return null;
        });
        RegisterFunction("RoundTenth", ActionValueType.Decimal, value => Math.Round(value * 10, MidpointRounding.AwayFromZero) / 10);
        RegisterFunction("RoundTwentieth", ActionValueType.Decimal, value => Math.Round(value * 20, MidpointRounding.AwayFromZero) / 20);
        RegisterFunction("RoundPartOfOne", ActionValueType.Decimal, value =>
        {
            if (ParameterCount == 1 && TryGetParameter<int>(0, out var roundPartOfOne))
            {
                return Math.Round(value * roundPartOfOne, MidpointRounding.AwayFromZero) / roundPartOfOne;
            }
            return null;
        });
        // fraction
        RegisterFunction("Fraction", ActionValueType.Decimal, value => value - Math.Truncate(value));
        // truncate
        RegisterFunction(nameof(decimal.Truncate), ActionValueType.Decimal, value => decimal.Truncate(value));
    }

    private void RegisterConvertFunctions()
    {
        RegisterFunction("ToInteger", ActionValueType.Integer, value => Math.Round(value));
        RegisterFunction("ToIntegerTruncate", ActionValueType.Integer, value => Math.Truncate(value));
        RegisterFunction(nameof(decimal.ToString), ActionValueType.String, value => value.ToString(CultureInfo.InvariantCulture));
    }
}

/// <summary>String action value method</summary>
public class StringActionValueMethod<TContext, TFunc> : ActionMethodBase<TContext, TFunc, string>
    where TContext : IActionCaseValueContext<TFunc>
    where TFunc : PayrollFunction
{
    /// <summary>Constructor</summary>
    public StringActionValueMethod(TContext context, string expression, DateTime caseValueDate) :
        base(context, expression, caseValueDate)
    {
        RegisterTestFunctions();
        RegisterInfoFunctions();
        RegisterModificationFunctions();
        RegisterConvertFunctions();
    }

    private void RegisterTestFunctions()
    {
        RegisterFunction("IsEmpty", ActionValueType.Boolean, value => string.IsNullOrWhiteSpace(value));
        RegisterFunction(nameof(string.Contains), ActionValueType.Boolean, value =>
        {
            if (ParameterCount == 1 &&
                TryGetParameter<string>(0, out var contains))
            {
                return value.Contains(contains);
            }
            return null;
        });
        RegisterFunction(nameof(string.StartsWith), ActionValueType.Boolean, value =>
        {
            if (ParameterCount == 1 && TryGetParameter<string>(0, out var startsWith))
            {
                return value.StartsWith(startsWith);
            }
            return null;
        });
        RegisterFunction(nameof(string.EndsWith), ActionValueType.Boolean, value =>
        {
            if (ParameterCount == 1 && TryGetParameter<string>(0, out var endsWith))
            {
                return value.EndsWith(endsWith);
            }
            return null;
        });
    }

    private void RegisterInfoFunctions()
    {
        RegisterFunction(nameof(string.Length), ActionValueType.Integer, value => value.Length);
    }

    private void RegisterModificationFunctions()
    {
        // trim
        RegisterFunction(nameof(string.Trim), ActionValueType.String, value => value.Trim());
        RegisterFunction(nameof(string.TrimStart), ActionValueType.String, value => value.TrimStart());
        RegisterFunction(nameof(string.TrimEnd), ActionValueType.String, value => value.TrimEnd());
        // case change
        RegisterFunction(nameof(string.ToUpper), ActionValueType.String, value => value.ToUpper());
        RegisterFunction(nameof(string.ToLower), ActionValueType.String, value => value.ToLower());
        // string parts
        RegisterFunction("Append", ActionValueType.String, value =>
        {
            if (ParameterCount == 1 && TryGetParameter<string>(0, out var append))
            {
                return value + append;
            }
            return null;
        });
        RegisterFunction(nameof(string.Substring), ActionValueType.String, value =>
        {
            if (ParameterCount == 2 && TryGetParameter<int>(0, out var startIndex) &&
                TryGetParameter<int>(1, out var length))
            {
                return value.Substring(startIndex, length);
            }
            return null;
        });
        RegisterFunction(nameof(string.Remove), ActionValueType.String, value =>
        {
            if (ParameterCount == 2 && TryGetParameter<int>(0, out var removeStart) &&
                TryGetParameter<int>(1, out var removeCount))
            {
                return value.Remove(removeStart, removeCount);
            }
            return null;
        });
        RegisterFunction(nameof(string.Insert), ActionValueType.String, value =>
        {
            if (ParameterCount == 2 && TryGetParameter<int>(0, out var insertIndex) &&
                TryGetParameter<string>(1, out var insertValue))
            {
                return value.Insert(insertIndex, insertValue);
            }
            return null;
        });
        RegisterFunction(nameof(string.Replace), ActionValueType.String, value =>
        {
            if (ParameterCount == 2 && TryGetParameter<string>(0, out var replaceOld) &&
                TryGetParameter<string>(1, out var replaceNew))
            {
                return value.Replace(replaceOld, replaceNew);
            }
            return null;
        });
        RegisterFunction("RemoveAll", ActionValueType.String, value =>
        {
            if (ParameterCount == 1 && TryGetParameter<string>(0, out var remove))
            {
                // remove by replace with empty string
                return value.Replace(remove, string.Empty);
            }
            return null;
        });
    }

    private void RegisterConvertFunctions()
    {
        RegisterFunction("ToInteger", ActionValueType.Integer, value =>
        {
            if (int.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var intValue))
            {
                return intValue;
            }
            return null;
        });
        RegisterFunction("ToDecimal", ActionValueType.Decimal, value =>
        {
            if (decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var decimalValue))
            {
                return decimalValue;
            }
            return null;
        });
        RegisterFunction("ToDateTime", ActionValueType.DateTime, value =>
        {
            if (DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out var dateValue))
            {
                return dateValue;
            }
            return null;
        });
    }
}

/// <summary>Date action value method</summary>
public class DateTimeActionValueMethod<TContext, TFunc> : ActionMethodBase<TContext, TFunc, DateTime>
    where TContext : IActionCaseValueContext<TFunc>
    where TFunc : PayrollFunction
{
    /// <summary>Constructor</summary>
    public DateTimeActionValueMethod(TContext context, string expression, DateTime caseValueDate) :
        base(context, expression, caseValueDate)
    {
        RegisterTestFunctions();
        RegisterInfoFunctions();
        RegisterModificationFunctions();
        RegisterConvertFunctions();
    }

    private void RegisterTestFunctions()
    {
        // date
        RegisterFunction("IsDate", ActionValueType.Boolean, value => value.Date == value);
        // hour
        RegisterFunction("IsPreviousHour", ActionValueType.Boolean, value => value.Hour == Date.Now.SubtractHours(1).Hour);
        RegisterFunction("IsCurrentHour", ActionValueType.Boolean, value => value.Hour == Date.Now.Hour);
        RegisterFunction("IsNextHour", ActionValueType.Boolean, value => value.Hour == Date.Now.AddHours(1).Hour);
        // day
        RegisterFunction("IsPreviousDay", ActionValueType.Boolean, value => value.Day == Date.Now.SubtractDays(1).Day);
        RegisterFunction("IsCurrentDay", ActionValueType.Boolean, value => value.Day == Date.Now.Day);
        RegisterFunction("IsNextDay", ActionValueType.Boolean, value => value.Day == Date.Now.AddDays(1).Day);
        // week days
        RegisterFunction("IsSunday", ActionValueType.Boolean, value => value.DayOfWeek == System.DayOfWeek.Sunday);
        RegisterFunction("IsMonday", ActionValueType.Boolean, value => value.DayOfWeek == System.DayOfWeek.Monday);
        RegisterFunction("IsTuesday", ActionValueType.Boolean, value => value.DayOfWeek == System.DayOfWeek.Tuesday);
        RegisterFunction("IsWednesday", ActionValueType.Boolean, value => value.DayOfWeek == System.DayOfWeek.Wednesday);
        RegisterFunction("IsThursday", ActionValueType.Boolean, value => value.DayOfWeek == System.DayOfWeek.Thursday);
        RegisterFunction("IsFriday", ActionValueType.Boolean, value => value.DayOfWeek == System.DayOfWeek.Friday);
        RegisterFunction("IsSaturday", ActionValueType.Boolean, value => value.DayOfWeek == System.DayOfWeek.Saturday);
        // week
        RegisterFunction("IsPreviousWeek", ActionValueType.Boolean, value =>
            value.GetIso8601WeekOfYear() == Date.Now.SubtractDays(7).GetIso8601WeekOfYear());
        RegisterFunction("IsCurrentWeek", ActionValueType.Boolean, value => value.GetIso8601WeekOfYear() == Date.Now.GetIso8601WeekOfYear());
        RegisterFunction("IsNextWeek", ActionValueType.Boolean, value =>
            value.GetIso8601WeekOfYear() == Date.Now.AddDays(7).GetIso8601WeekOfYear());
        RegisterFunction("IsWeek", ActionValueType.Boolean, value =>
        {
            if (ParameterCount == 1 && TryGetParameter<int>(0, out var isWeek))
            {
                return value.GetIso8601WeekOfYear() == isWeek;
            }
            return null;
        });
        // month
        RegisterFunction("IsPreviousMonth", ActionValueType.Boolean, value => value.Month == Date.Now.SubtractMonths(1).Month);
        RegisterFunction("IsCurrentMonth", ActionValueType.Boolean, value => value.Month == Date.Now.Month);
        RegisterFunction("IsNextMonth", ActionValueType.Boolean, value => value.Month == Date.Now.AddMonths(1).Month);
        // month year
        RegisterFunction("IsJanuary", ActionValueType.Boolean, value => value.Month == (int)Month.January);
        RegisterFunction("IsFebruary", ActionValueType.Boolean, value => value.Month == (int)Month.February);
        RegisterFunction("IsMarch", ActionValueType.Boolean, value => value.Month == (int)Month.March);
        RegisterFunction("IsApril", ActionValueType.Boolean, value => value.Month == (int)Month.April);
        RegisterFunction("IsMay", ActionValueType.Boolean, value => value.Month == (int)Month.May);
        RegisterFunction("IsJune", ActionValueType.Boolean, value => value.Month == (int)Month.June);
        RegisterFunction("IsJuly", ActionValueType.Boolean, value => value.Month == (int)Month.July);
        RegisterFunction("IsAugust", ActionValueType.Boolean, value => value.Month == (int)Month.August);
        RegisterFunction("IsSeptember", ActionValueType.Boolean, value => value.Month == (int)Month.September);
        RegisterFunction("IsOctober", ActionValueType.Boolean, value => value.Month == (int)Month.October);
        RegisterFunction("IsNovember", ActionValueType.Boolean, value => value.Month == (int)Month.November);
        RegisterFunction("IsDecember", ActionValueType.Boolean, value => value.Month == (int)Month.December);
        // month day
        RegisterFunction("IsFirstDayOfMonth", ActionValueType.Boolean, value => value.Day == 1);
        RegisterFunction("IsLastDayOfMonth", ActionValueType.Boolean, value => value.Day == DateTime.DaysInMonth(value.Year, value.Month));
        // year
        RegisterFunction("IsPreviousYear", ActionValueType.Boolean, value => value.Year == Date.Now.SubtractYears(1).Year);
        RegisterFunction("IsCurrentYear", ActionValueType.Boolean, value => value.Year == Date.Now.Year);
        RegisterFunction("IsCurrentLeapYear", ActionValueType.Boolean, value => DateTime.IsLeapYear(value.Year));
        RegisterFunction("IsNextYear", ActionValueType.Boolean, value => value.Year == Date.Now.AddYears(1).Year);
        // year month
        RegisterFunction("IsFirstMonthOfYear", ActionValueType.Boolean, value => value.Month == 1);
        RegisterFunction("IsLastMonthOfYear", ActionValueType.Boolean, value => value.Month == 12);
        // leap year
        RegisterFunction(nameof(DateTime.IsLeapYear), ActionValueType.Boolean, _ =>
        {
            if (ParameterCount == 1 && TryGetParameter<int>(0, out var isLeapYear))
            {
                return DateTime.IsLeapYear(isLeapYear);
            }
            return null;
        });
        // compare time and date
        RegisterFunction("EqualTime", ActionValueType.Boolean, value =>
        {
            if (ParameterCount == 1 && TryGetParameter<DateTime>(0, out var equalTime))
            {
                return value.Hour == equalTime.Hour && value.Minute == equalTime.Minute &&
                       value.Second == equalTime.Second;
            }
            return null;
        });
        RegisterFunction("EqualDate", ActionValueType.Boolean, value =>
        {
            if (ParameterCount == 1 && TryGetParameter<DateTime>(0, out var equalDate))
            {
                return value.Date == equalDate.Date;
            }
            return null;
        });
        // compare date part
        RegisterFunction("EqualHour", ActionValueType.Boolean, value =>
        {
            if (ParameterCount == 1 && TryGetParameter<DateTime>(0, out var equalHour))
            {
                return value.IsSameHour(equalHour);
            }
            return null;
        });
        RegisterFunction("EqualWeek", ActionValueType.Boolean, value =>
        {
            if (ParameterCount == 1 && TryGetParameter<DateTime>(0, out var equalWeek))
            {
                return value.Year == equalWeek.Year &&
                       value.GetIso8601WeekOfYear() == equalWeek.GetIso8601WeekOfYear();
            }
            return null;
        });
        RegisterFunction("EqualMonth", ActionValueType.Boolean, value =>
        {
            if (ParameterCount == 1 && TryGetParameter<DateTime>(0, out var equalMonth))
            {
                return value.IsSameMonth(equalMonth);
            }
            return null;
        });
        RegisterFunction("EqualYear", ActionValueType.Boolean, value =>
        {
            if (ParameterCount == 1 && TryGetParameter<DateTime>(0, out var equalYear))
            {
                return value.IsSameYear(equalYear);
            }
            return null;
        });
        // period
        RegisterFunction("Before", ActionValueType.Boolean, value =>
        {
            if (ParameterCount == 1 && TryGetParameter<DateTime>(0, out var before))
            {
                return value < before;
            }
            return null;
        });
        RegisterFunction("After", ActionValueType.Boolean, value =>
        {
            if (ParameterCount == 1 && TryGetParameter<DateTime>(0, out var after))
            {
                return value > after;
            }
            return null;
        });
        RegisterFunction("Within", ActionValueType.Boolean, value =>
        {
            if (ParameterCount == 2 && TryGetParameter<DateTime>(0, out var withinMin) &&
                TryGetParameter<DateTime>(1, out var withinMax))
            {
                return value >= withinMin && value <= withinMax;
            }
            return null;
        });
    }

    private void RegisterInfoFunctions()
    {
        // week
        RegisterFunction("Week", ActionValueType.Integer, value => value.GetIso8601WeekOfYear());
        // current date
        RegisterFunction("CurrentHour", ActionValueType.Integer, _ => Date.Now.Hour);
        RegisterFunction("CurrentDay", ActionValueType.Integer, _ => Date.Now.Day);
        RegisterFunction("CurrentWeek", ActionValueType.Integer, _ => Date.Now.GetIso8601WeekOfYear());
        RegisterFunction("CurrentMonth", ActionValueType.Integer, _ => Date.Now.Month);
        RegisterFunction("CurrentYear", ActionValueType.Integer, _ => Date.Now.Year);
        // month days
        RegisterFunction("DaysInCurrentMonth", ActionValueType.Integer, _ =>
        {
            var now = Date.Now;
            return DateTime.DaysInMonth(now.Year, now.Month);
        });
        RegisterFunction("RemainDaysInCurrentMonth", ActionValueType.Integer, _ =>
        {
            var now = Date.Now;
            return DateTime.DaysInMonth(now.Year, now.Month) - now.Day;
        });
        RegisterFunction(nameof(DateTime.DaysInMonth), ActionValueType.Integer, _ =>
        {
            if (ParameterCount == 2 && TryGetParameter<int>(0, out var daysInMonthYear) &&
                TryGetParameter<int>(1, out var daysInMonthMonth))
            {
                return DateTime.DaysInMonth(daysInMonthYear, daysInMonthMonth);
            }
            return null;
        });
        // date time part
        RegisterFunction(nameof(DateTime.Minute), ActionValueType.Integer, value => value.Minute);
        RegisterFunction(nameof(DateTime.Hour), ActionValueType.Integer, value => value.Hour);
        RegisterFunction(nameof(DateTime.Day), ActionValueType.Integer, value => value.Date);
        RegisterFunction(nameof(DateTime.Month), ActionValueType.Integer, value => value.Month);
        RegisterFunction(nameof(DateTime.Year), ActionValueType.Integer, value => value.Year);
        RegisterFunction(nameof(DateTime.Date), ActionValueType.DateTime, value => value.Date);
    }

    private void RegisterModificationFunctions()
    {
        // range
        RegisterFunction("Limit", ActionValueType.DateTime, value =>
        {
            if (ParameterCount == 2 && TryGetParameter<DateTime>(0, out var limitMin) &&
                TryGetParameter<DateTime>(1, out var limitMax))
            {
                return value < limitMin ? limitMin : value > limitMax ? limitMax : value;
            }
            return null;
        });
        RegisterFunction("Min", ActionValueType.DateTime, value =>
        {
            if (ParameterCount == 1 && TryGetParameter<DateTime>(0, out var min))
            {
                return value < min ? min : value;
            }
            return null;
        });
        RegisterFunction("Max", ActionValueType.DateTime, value =>
        {
            if (ParameterCount == 1 && TryGetParameter<DateTime>(0, out var max))
            {
                return value > max ? max : value;
            }
            return null;
        });
        // add time span
        RegisterFunction("AddTimeSpan", ActionValueType.DateTime, value =>
        {
            if (ParameterCount == 1 && TryGetParameter<TimeSpan>(0, out var addTimeSpan))
            {
                return value.Add(addTimeSpan);
            }
            return null;
        });
        // add date period
        RegisterFunction("AddPeriod", ActionValueType.DateTime, value =>
        {
            if (Parameters.Count == 2 && TryGetParameter<DateTime>(0, out var addPeriodStart) &&
                TryGetParameter<DateTime>(1, out var addPeriodEnd))
            {
                return value.Add(addPeriodEnd - addPeriodStart);
            }
            return null;
        });
        // add date part
        RegisterFunction(nameof(DateTime.AddMinutes), ActionValueType.DateTime, value =>
        {
            if (ParameterCount == 1 && TryGetParameter<int>(0, out var addMinutes))
            {
                return value.AddMinutes(addMinutes);
            }
            return null;
        });
        RegisterFunction(nameof(DateTime.AddHours), ActionValueType.DateTime, value =>
        {
            if (ParameterCount == 1 && TryGetParameter<int>(0, out var addHours))
            {
                return value.AddHours(addHours);
            }
            return null;
        });
        RegisterFunction(nameof(DateTime.AddDays), ActionValueType.DateTime, value =>
        {
            if (ParameterCount == 1 && TryGetParameter<int>(0, out var addDays))
            {
                return value.AddDays(addDays);
            }
            return null;
        });
        RegisterFunction(nameof(DateTime.AddMonths), ActionValueType.DateTime, value =>
        {
            // add month is limited to 12000
            if (ParameterCount == 1 && TryGetParameter<int>(0, out var addMonths) &&
                (addMonths <= -120000 || addMonths >= 120000))
            {
                return value.AddMonths(addMonths);
            }
            return null;
        });
        RegisterFunction(nameof(DateTime.AddYears), ActionValueType.DateTime, value =>
        {
            if (ParameterCount == 1 && TryGetParameter<int>(0, out var addYears))
            {
                return value.AddYears(addYears);
            }
            return null;
        });
        // subtract time span
        RegisterFunction("SubtractTimeSpan", ActionValueType.DateTime, value =>
        {
            if (ParameterCount == 1 && TryGetParameter<TimeSpan>(0, out var subtractTimeSpan))
            {
                return value.Subtract(subtractTimeSpan);
            }
            return null;
        });
        // subtract date period
        RegisterFunction("SubtractPeriod", ActionValueType.DateTime, value =>
        {
            if (Parameters.Count == 2 && TryGetParameter<DateTime>(0, out var subtractPeriodStart) &&
             TryGetParameter<DateTime>(1, out var subtractPeriodEnd))
            {
                return value.Subtract(subtractPeriodEnd - subtractPeriodStart);
            }
            return null;
        });
        // subtract date part
        RegisterFunction("SubtractMinutes", ActionValueType.DateTime, value =>
        {
            if (ParameterCount == 1 && TryGetParameter<int>(0, out var subMinutes))
            {
                return value.AddMinutes(subMinutes * -1);
            }
            return null;
        });
        RegisterFunction("SubtractHours", ActionValueType.DateTime, value =>
        {
            if (ParameterCount == 1 && TryGetParameter<int>(0, out var subHours))
            {
                return value.AddHours(subHours * -1);
            }
            return null;
        });
        RegisterFunction("SubtractDays", ActionValueType.DateTime, value =>
        {
            if (ParameterCount == 1 && TryGetParameter<int>(0, out var subDays))
            {
                return value.AddDays(subDays * -1);
            }
            return null;
        });
        RegisterFunction("SubtractMonths", ActionValueType.DateTime, value =>
        {
            // add month is limited to 12000
            if (ParameterCount == 1 && TryGetParameter<int>(0, out var subMonths) &&
                (subMonths <= -120000 || subMonths >= 120000))
            {
                return value.AddMonths(subMonths * -1);
            }
            return null;
        });
        RegisterFunction("SubtractYears", ActionValueType.DateTime, value =>
        {
            if (ParameterCount == 1 && TryGetParameter<int>(0, out var subYears))
            {
                return value.AddYears(subYears * -1);
            }
            return null;
        });
    }

    private void RegisterConvertFunctions()
    {
        RegisterFunction(nameof(DateTime.ToString), ActionValueType.Integer, value => value.ToString(CultureInfo.InvariantCulture));
    }
}

/// <summary>Timespan action value method</summary>
public class TimeSpanActionValueMethod<TContext, TFunc> : ActionMethodBase<TContext, TFunc, TimeSpan>
    where TContext : IActionCaseValueContext<TFunc>
    where TFunc : PayrollFunction
{
    /// <summary>Constructor</summary>
    public TimeSpanActionValueMethod(TContext context, string expression, DateTime caseValueDate) :
        base(context, expression, caseValueDate)
    {
        RegisterTestFunctions();
        RegisterInfoFunctions();
        RegisterModificationFunctions();
        RegisterConvertFunctions();
    }

    private void RegisterTestFunctions()
    {
        // date
        RegisterFunction("IsFullHour", ActionValueType.Boolean, value => value.TotalHours >= 1);
        RegisterFunction("IsFullDay", ActionValueType.Boolean, value => value.TotalDays >= 1);
        RegisterFunction("IsZero", ActionValueType.Boolean, value => value == TimeSpan.Zero);
        RegisterFunction("IsPositive", ActionValueType.Boolean, value => value == value.Duration());
        RegisterFunction("IsNegative", ActionValueType.Boolean, value => value != value.Duration());
        RegisterFunction("IsGreater", ActionValueType.Boolean, value =>
        {
            if (ParameterCount == 1 && TryGetParameter<TimeSpan>(0, out var isGreater))
            {
                return value > isGreater;
            }
            return null;
        });
        RegisterFunction("IsLess", ActionValueType.Boolean, value =>
        {
            if (ParameterCount == 1 && TryGetParameter<TimeSpan>(0, out var isLess))
            {
                return value < isLess;
            }
            return null;
        });
    }

    private void RegisterInfoFunctions()
    {
        // minutes
        RegisterFunction(nameof(TimeSpan.Minutes), ActionValueType.Integer, value => value.Minutes);
        RegisterFunction(nameof(TimeSpan.TotalMinutes), ActionValueType.Decimal, value => value.TotalMinutes);
        // hours
        RegisterFunction(nameof(TimeSpan.Hours), ActionValueType.Integer, value => value.Hours);
        RegisterFunction(nameof(TimeSpan.TotalHours), ActionValueType.Decimal, value => value.TotalHours);
        // days
        RegisterFunction(nameof(TimeSpan.Days), ActionValueType.Integer, value => value.Days);
        RegisterFunction(nameof(TimeSpan.TotalDays), ActionValueType.Decimal, value => value.TotalDays);
    }

    private void RegisterModificationFunctions()
    {
        // negate
        RegisterFunction(nameof(TimeSpan.Negate), ActionValueType.TimeSpan, value => value.Negate());
        // add time span
        RegisterFunction(nameof(TimeSpan.Add), ActionValueType.TimeSpan, value =>
        {
            if (ParameterCount == 1 && TryGetParameter<TimeSpan>(0, out var add))
            {
                return value.Add(add);
            }
            return null;
        });
        // add time span part
        RegisterFunction("AddMinutes", ActionValueType.TimeSpan, value =>
        {
            if (ParameterCount == 1 && TryGetParameter<int>(0, out var addMinutes))
            {
                return value.Add(TimeSpan.FromMinutes(addMinutes));
            }
            return null;
        });
        RegisterFunction("AddHours", ActionValueType.TimeSpan, value =>
        {
            if (ParameterCount == 1 && TryGetParameter<int>(0, out var addHours))
            {
                return value.Add(TimeSpan.FromHours(addHours));
            }
            return null;
        });
        RegisterFunction("AddDays", ActionValueType.TimeSpan, value =>
        {
            if (ParameterCount == 1 && TryGetParameter<int>(0, out var addDays))
            {
                return value.Add(TimeSpan.FromDays(addDays));
            }
            return null;
        });
        RegisterFunction("AddDate", ActionValueType.TimeSpan, value =>
        {
            if (ParameterCount == 1 && TryGetParameter<DateTime>(0, out var addDate))
            {
                return addDate.Add(value);
            }
            return null;
        });
        // subtract time span
        RegisterFunction(nameof(TimeSpan.Subtract), ActionValueType.TimeSpan, value =>
        {
            if (ParameterCount == 1 && TryGetParameter<TimeSpan>(0, out var subtract))
            {
                return value.Subtract(subtract);
            }
            return null;
        });
        // subtract time span part
        RegisterFunction("SubtractMinutes", ActionValueType.TimeSpan, value =>
        {
            if (ParameterCount == 1 && TryGetParameter<int>(0, out var subtractMinutes))
            {
                return value.Add(TimeSpan.FromMinutes(subtractMinutes));
            }
            return null;
        });
        RegisterFunction("SubtractHours", ActionValueType.TimeSpan, value =>
        {
            if (ParameterCount == 1 && TryGetParameter<int>(0, out var subtractHours))
            {
                return value.Add(TimeSpan.FromHours(subtractHours));
            }
            return null;
        });
        RegisterFunction("SubtractDays", ActionValueType.TimeSpan, value =>
        {
            if (ParameterCount == 1 && TryGetParameter<int>(0, out var subtractDays))
            {
                return value.Add(TimeSpan.FromDays(subtractDays));
            }
            return null;
        });
        RegisterFunction("SubtractDate", ActionValueType.TimeSpan, value =>
        {
            if (ParameterCount == 1 && TryGetParameter<DateTime>(0, out var subtractDate))
            {
                return subtractDate.Subtract(value);
            }
            return null;
        });
    }

    private void RegisterConvertFunctions()
    {
        RegisterFunction(nameof(TimeSpan.ToString), ActionValueType.Integer, value => value.ToString());
    }
}

/// <summary>Attribute action value method</summary>
public abstract class CaseFieldActionMethodBase<TContext, TFunc, TValue> : ActionMethodBase<TContext, TFunc, TValue>
    where TContext : IActionCaseValueContext<TFunc>
    where TFunc : PayrollFunction
{
    /// <summary>The case field name</summary>
    public string CaseFieldName { get; }

    /// <summary>The attribute key</summary>
    public string AttributeKey { get; }

    /// <summary>The attribute type</summary>
    public ActionValueType AttributeType { get; }

    /// <summary>Constructor</summary>
    protected CaseFieldActionMethodBase(TContext context, string caseFieldName, string expression) :
        base(context, expression, Date.Now)
    {
        if (string.IsNullOrWhiteSpace(caseFieldName))
        {
            throw new ArgumentException(nameof(caseFieldName));
        }
        CaseFieldName = caseFieldName;

        // parameter 0: attribute key
        if (!TryGetParameter<string>(0, out var attributeKey))
        {
            Context.Function.LogError("Missing attribute key");
        }
        AttributeKey = attributeKey;

        // method name: value type
        if (!Enum.TryParse<ActionValueType>(Name, out var attributeType))
        {
            Context.Function.LogError($"Invalid attribute type: {Name}");
        }
        AttributeType = attributeType;
    }

    /// <summary>Constructor</summary>
    protected CaseFieldActionMethodBase(TContext context, string caseFieldName, string attributeKey,
        ActionValueType attributeType) :
        base(context, $"{attributeType}({attributeKey})", Date.Now)
    {
        if (string.IsNullOrWhiteSpace(caseFieldName))
        {
            throw new ArgumentException(nameof(caseFieldName));
        }
        if (string.IsNullOrWhiteSpace(attributeKey))
        {
            throw new ArgumentException(nameof(attributeKey));
        }

        CaseFieldName = caseFieldName;
        AttributeKey = attributeKey;
        AttributeType = attributeType;
    }

    /// <summary>Get the value type</summary>
    public override ActionValueType GetValueType() => AttributeType;
}

/// <summary>Case field attribute action value method</summary>
public class CaseFieldAttributeActionMethod<TContext, TFunc> : CaseFieldActionMethodBase<TContext, TFunc, object>
    where TContext : IActionCaseValueContext<TFunc>
    where TFunc : PayrollFunction
{
    /// <summary>Constructor</summary>
    public CaseFieldAttributeActionMethod(TContext context, string caseFieldName, string expression) :
        base(context, caseFieldName, expression)
    {
    }

    /// <summary>Constructor</summary>
    public CaseFieldAttributeActionMethod(TContext context, string caseFieldName, string attributeKey,
        ActionValueType attributeType) :
        base(context, caseFieldName, attributeKey, attributeType)
    {
    }

    /// <summary>Get the resolved value</summary>
    public override object EvaluateValue(object value)
    {
        //Function.LogWarning($"CaseFieldAttributeActionMethod: CaseFieldName={CaseFieldName}, AttributeKey={AttributeKey}, AttributeType={AttributeType}");
        switch (AttributeType)
        {
            case ActionValueType.String:
                return Context.GetCaseFieldAttribute<string>(CaseFieldName, AttributeKey);
            case ActionValueType.Boolean:
                return Context.GetCaseFieldAttribute<bool>(CaseFieldName, AttributeKey);
            case ActionValueType.Integer:
                return Context.GetCaseFieldAttribute<int>(CaseFieldName, AttributeKey);
            case ActionValueType.Decimal:
                return Context.GetCaseFieldAttribute<decimal>(CaseFieldName, AttributeKey);
            case ActionValueType.DateTime:
                return Context.GetCaseFieldAttribute<DateTime>(CaseFieldName, AttributeKey);
            default:
                return null;
        }
    }
}

/// <summary>Case value attribute action value method</summary>
public class CaseValueAttributeActionMethod<TContext, TFunc> : CaseFieldActionMethodBase<TContext, TFunc, object>
    where TContext : IActionCaseValueContext<TFunc>
    where TFunc : PayrollFunction
{
    /// <summary>Constructor</summary>
    public CaseValueAttributeActionMethod(TContext context, string caseFieldName, string expression) :
        base(context, caseFieldName, expression)
    {
    }

    /// <summary>Constructor</summary>
    public CaseValueAttributeActionMethod(TContext context, string caseFieldName, string attributeKey,
        ActionValueType attributeType) :
        base(context, caseFieldName, attributeKey, attributeType)
    {
    }

    /// <summary>Get the resolved value</summary>
    public override object EvaluateValue(object value)
    {
        //Function.LogWarning($"CaseValueAttributeActionMethod: CaseFieldName={CaseFieldName}, AttributeKey={AttributeKey}, AttributeType={AttributeType}");
        switch (AttributeType)
        {
            case ActionValueType.String:
                return Context.GetCaseValueAttribute<string>(CaseFieldName, AttributeKey);
            case ActionValueType.Boolean:
                return Context.GetCaseValueAttribute<bool>(CaseFieldName, AttributeKey);
            case ActionValueType.Integer:
                return Context.GetCaseValueAttribute<int>(CaseFieldName, AttributeKey);
            case ActionValueType.Decimal:
                return Context.GetCaseValueAttribute<decimal>(CaseFieldName, AttributeKey);
            case ActionValueType.DateTime:
                return Context.GetCaseValueAttribute<DateTime>(CaseFieldName, AttributeKey);
            default:
                return null;
        }
    }
}


/// <summary>Lookup action value method base</summary>
public abstract class LookupActionMethodBase<TContext, TFunc> : ActionMethodBase<TContext, TFunc, object>
    where TContext : IActionCaseValueContext<TFunc>
    where TFunc : PayrollFunction
{
    /// <summary>The lookupName name</summary>
    public string LookupName { get; }

    /// <summary>The lookup value type</summary>
    public ActionValueType ValueType { get; }

    /// <summary>Constructor</summary>
    protected LookupActionMethodBase(TContext context, string lookupName, string expression) :
        base(context, expression, Date.Now)
    {
        if (string.IsNullOrWhiteSpace(lookupName))
        {
            throw new ArgumentException(nameof(lookupName));
        }
        LookupName = lookupName;

        // method name: value type
        if (!Enum.TryParse<ActionValueType>(Name, out var valueType))
        {
            Context.Function.LogError($"Invalid attribute type: {Name}");
        }
        ValueType = valueType;
    }

    /// <summary>Get the value type</summary>
    public override ActionValueType GetValueType() => ValueType;

    /// <summary>Get the lookup object value</summary>
    protected object GetObjectValue(string lookupValue, string objectKey)
    {
        if (string.IsNullOrWhiteSpace(lookupValue))
        {
            throw new ArgumentException(nameof(lookupValue));
        }
        if (string.IsNullOrWhiteSpace(objectKey))
        {
            throw new ArgumentException(nameof(objectKey));
        }

        if (!lookupValue.StartsWith('{') || !lookupValue.EndsWith('}'))
        {
            Context.Function.LogError($"Invalid lookup object value: {lookupValue}");
            return null;
        }

        var dictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(lookupValue);
        if (dictionary == null || !dictionary.TryGetValue(objectKey, out var value))
        {
            Context.Function.LogError($"Invalid lookup object key: {objectKey}");
            return null;
        }
        return value;
    }
}

/// <summary>Lookup action value method</summary>
public class LookupActionMethod<TContext, TFunc> : LookupActionMethodBase<TContext, TFunc>
    where TContext : IActionCaseValueContext<TFunc>
    where TFunc : PayrollFunction
{
    /// <summary>The lookup key</summary>
    public string LookupKey { get; }

    /// <summary>The lookup object value key</summary>
    public string ObjectKey { get; }

    /// <summary>Constructor</summary>
    public LookupActionMethod(TContext context, string lookupName, string expression) :
        base(context, lookupName, expression)
    {
        // parameter 0: lookup key
        if (!TryGetParameter<string>(0, out var lookupKey))
        {
            Context.Function.LogError("Missing lookup key");
        }
        LookupKey = lookupKey;
        // parameter 1: lookup object value key (optional)
        if (ParameterCount > 1)
        {
            if (TryGetParameter<string>(1, out var objectKey))
            {
                ObjectKey = objectKey;
            }
        }
    }

    /// <summary>Get the resolved value</summary>
    public override object EvaluateValue(object value)
    {
        // object value
        if (!string.IsNullOrWhiteSpace(ObjectKey))
        {
            var lookupValue = Context.Function.GetLookup<string>(LookupName, LookupKey, Context.Function.UserCulture);
            if (string.IsNullOrWhiteSpace(lookupValue))
            {
                return null;
            }
            return GetObjectValue(lookupValue, ObjectKey);
        }

        // type lookup value
        switch (ValueType)
        {
            case ActionValueType.String:
                return Context.Function.GetLookup<string>(LookupName, LookupKey, UserCulture);
            case ActionValueType.Boolean:
                return Context.Function.GetLookup<bool>(LookupName, LookupKey, UserCulture);
            case ActionValueType.Integer:
                return Context.Function.GetLookup<int>(LookupName, LookupKey, UserCulture);
            case ActionValueType.Decimal:
                return Context.Function.GetLookup<decimal>(LookupName, LookupKey, UserCulture);
            case ActionValueType.DateTime:
                return Context.Function.GetLookup<DateTime>(LookupName, LookupKey, UserCulture);
            default:
                return null;
        }
    }
}

/// <summary>Range lookup action value method</summary>
public class RangeLookupActionMethod<TContext, TFunc> : LookupActionMethodBase<TContext, TFunc>
    where TContext : IActionCaseValueContext<TFunc>
    where TFunc : PayrollFunction
{
    /// <summary>The lookup object value key</summary>
    public string ObjectKey { get; }

    /// <summary>The lookup range value</summary>
    public decimal RangeValue { get; }

    /// <summary>Constructor</summary>
    public RangeLookupActionMethod(TContext context, string lookupName, string expression) :
        base(context, lookupName, expression)
    {
        // parameter 0: lookup range value
        if (!TryGetParameter<decimal>(0, out var rangeValue))
        {
            Context.Function.LogError("Missing lookup range value");
        }
        RangeValue = rangeValue;
        // parameter 1: lookup object value key (optional)
        if (ParameterCount > 1)
        {
            if (TryGetParameter<string>(1, out var objectKey))
            {
                ObjectKey = objectKey;
            }
        }
    }

    /// <summary>Get the resolved value</summary>
    public override object EvaluateValue(object value)
    {
        // object value
        if (!string.IsNullOrWhiteSpace(ObjectKey))
        {
            var lookupValue = Context.Function.GetRangeLookup<string>(LookupName, RangeValue, UserCulture);
            if (string.IsNullOrWhiteSpace(lookupValue))
            {
                return null;
            }
            return GetObjectValue(lookupValue, ObjectKey);
        }

        // type lookup value
        return ValueType switch
        {
            ActionValueType.String => Context.Function.GetRangeLookup<string>(LookupName, RangeValue, UserCulture),
            ActionValueType.Boolean => Context.Function.GetRangeLookup<bool>(LookupName, RangeValue, UserCulture),
            ActionValueType.Integer => Context.Function.GetRangeLookup<int>(LookupName, RangeValue, UserCulture),
            ActionValueType.Decimal => Context.Function.GetRangeLookup<decimal>(LookupName, RangeValue, UserCulture),
            ActionValueType.DateTime => Context.Function.GetRangeLookup<DateTime>(LookupName, RangeValue, UserCulture),
            _ => null
        };
    }
}

#endregion

#region Action Case Value

/// <summary>Action case value</summary>
public abstract class ActionCaseValueBase
{
    /// <summary>Value mode postfix</summary>
    protected static readonly string ValueModePostfix = ".Value";

    /// <summary>Start mode postfix</summary>
    protected static readonly string StartModePostfix = ".Start";

    /// <summary>End mode postfix</summary>
    protected static readonly string EndModePostfix = ".End";

    /// <summary>Period mode postfix</summary>
    protected static readonly string PeriodModePostfix = ".Period";

    /// <summary>Case field attribute mode postfix</summary>
    protected static readonly string FieldAttributeModePostfix = ".FieldAttribute";

    /// <summary>Case value attribute mode postfix</summary>
    protected static readonly string ValueAttributeModePostfix = ".ValueAttribute";

    /// <summary>Lookup value attribute mode postfix</summary>
    protected static readonly string LookupModePostfix = ".Lookup";

    /// <summary>Range lookup value attribute mode postfix</summary>
    protected static readonly string RangeLookupModePostfix = ".RangeLookup";

    /// <summary>Case change reference</summary>
    protected static readonly char CaseChangeReference = '#';

    /// <summary>Case value reference</summary>
    protected static readonly char CaseValueReference = '@';

    /// <summary>Lookup value reference</summary>
    protected static readonly char LookupValueReference = '$';

    /// <summary>Value optional marker</summary>
    protected static readonly char OptionalFieldMarker = '?';

    /// <summary>Convert to case change value reference</summary>
    public static string ToValueReference(string reference) =>
        reference.EnsureEnd(ValueModePostfix);

    /// <summary>Convert to case change field attribute reference</summary>
    public static string ToFieldAttributeReference(string reference) =>
        reference.EnsureEnd(FieldAttributeModePostfix);

    /// <summary>Convert to case change value attribute reference</summary>
    public static string ToValueAttributeReference(string reference) =>
        reference.EnsureEnd(ValueAttributeModePostfix);

    /// <summary>Convert to case change reference</summary>
    public static string ToCaseChangeReference(string reference) =>
        reference.EnsureStart($"{CaseChangeReference}");

    /// <summary>Convert to case value reference</summary>
    public static string ToCaseValueReference(string reference) =>
        reference.EnsureStart($"{CaseValueReference}");

    /// <summary>Convert to case change value start reference</summary>
    public static string ToStartReference(string reference) =>
        reference.EnsureEnd(StartModePostfix);

    /// <summary>Convert to case change value end reference</summary>
    public static string ToEndReference(string reference) =>
        reference.EnsureEnd(EndModePostfix);

    /// <summary>Convert to case change value reference</summary>
    public static string ToCaseChangeValueReference(string reference) =>
        ToValueReference(ToCaseChangeReference(reference));

    /// <summary>Convert to case change value start reference</summary>
    public static string ToCaseChangeStartReference(string reference) =>
        ToStartReference(ToCaseChangeReference(reference));

    /// <summary>Convert to case change value end reference</summary>
    public static string ToCaseChangeEndReference(string reference) =>
        ToEndReference(ToCaseChangeReference(reference));

    /// <summary>Convert to case change field attribute value reference</summary>
    public static string ToCaseChangeFieldAttributeReference(string reference) =>
        ToFieldAttributeReference(ToCaseChangeReference(reference));

    /// <summary>Convert to case change value attribute value reference</summary>
    public static string ToCaseChangeValueAttributeReference(string reference) =>
        ToValueAttributeReference(ToCaseChangeReference(reference));

    /// <summary>Convert to case value reference</summary>
    public static string ToCaseValueValueReference(string reference) =>
        ToValueReference(ToCaseValueReference(reference));

    /// <summary>Convert to case value start reference</summary>
    public static string ToCaseValueStartReference(string reference) =>
        ToStartReference(ToCaseValueReference(reference));

    /// <summary>Convert to case value end reference</summary>
    public static string ToCaseValueEndReference(string reference) =>
        ToEndReference(ToCaseValueReference(reference));

    /// <summary>Convert to case value field attribute value reference</summary>
    public static string ToCaseValueFieldAttributeReference(string reference) =>
        ToFieldAttributeReference(ToCaseValueReference(reference));

    /// <summary>Convert to case value attribute value reference</summary>
    public static string ToCaseValueValueAttributeReference(string reference) =>
        ToValueAttributeReference(ToCaseValueReference(reference));

    /// <summary>Get the marker index</summary>
    /// <param name="expression"></param>
    /// <param name="marker"></param>
    protected static int GetMarkerIndex(string expression, string marker)
    {
        for (var index = 0; index < expression.Length; index++)
        {
            var c = expression[index];

            // valid reference characters are letter/number
            if (char.IsLetter(c) || char.IsNumber(c))
            {
                continue;
            }

            // first non letter/number/dot
            if (!expression.Substring(index).StartsWith(marker))
            {
                // support references with dot
                if (c == '.')
                {
                    continue;
                }
                // marker is not matching
                break;
            }

            // valid index
            return index;
        }
        return -1;
    }

    /// <summary>Get case value type</summary>
    public static ActionValueType? GetActionValueType(ValueType? valueType) =>
        valueType?.ToActionValueType();
}

/// <summary>Action case value</summary>
public abstract class ActionCaseValue<TContext, TFunc, TValue> : ActionCaseValueBase
    where TContext : IActionCaseValueContext<TFunc>
    where TFunc : PayrollFunction
{
    /// <summary>Value reference</summary>
    private sealed class ValueReference
    {
        /// <summary>The action value mode</summary>
        internal ActionValueReferenceType ReferenceType { get; init; }

        /// <summary>The action value source</summary>
        internal ActionValueSource ValueSource { get; init; }

        /// <summary>Reference as field name</summary>
        internal string ReferenceField { get; init; }

        /// <summary>The action case value</summary>
        internal CaseValue CaseValue { get; init; }

        /// <summary>The action case value type</summary>
        internal ActionValueType? CaseValueType { get; init; }

        /// <summary>The action value method</summary>
        internal IActionMethod ValueMethod { get; init; }

        /// <summary>Attribute key</summary>
        internal string AttributeKey { get; init; }

        /// <summary>Attribute type</summary>
        internal ActionValueType? AttributeType { get; init; }

        /// <summary>Lookup key</summary>
        internal string LookupKey { get; init; }

        /// <summary>Lookup object key</summary>
        internal string LookupObjectKey { get; init; }

        /// <summary>Lookup range value</summary>
        internal decimal? LookupRangeValue { get; init; }

        /// <summary>Lookup value type</summary>
        internal ActionValueType? LookupType { get; init; }

        /// <summary>Mandatory field</summary>
        internal bool MandatoryField { get; init; }
    }

    /// <summary>The context</summary>
    public TContext Context { get; }

    /// <summary>The source value</summary>
    public object SourceValue { get; }

    /// <summary>The value date</summary>
    public DateTime ValueDate { get; }

    /// <summary>True for case change reference value</summary>
    public bool IsCaseChangeReference =>
        ReferenceType == ActionValueReferenceType.CaseChange;

    /// <summary>True for case value reference value</summary>
    public bool IsCaseValueReference =>
        ReferenceType == ActionValueReferenceType.CaseValue;

    /// <summary>True for lookup reference value</summary>
    public bool IsLookupReference =>
        ReferenceType == ActionValueReferenceType.Lookup;

    /// <summary>True for case field attribute value</summary>
    public bool IsCaseFieldAttribute =>
        ValueSource == ActionValueSource.FieldAttribute;

    /// <summary>True for case value attribute value</summary>
    public bool IsCaseValueAttribute =>
        ValueSource == ActionValueSource.ValueAttribute;

    /// <summary>True for lookup value</summary>
    public bool IsLookup =>
        ValueSource == ActionValueSource.Lookup;

    /// <summary>True for range lookup value</summary>
    public bool IsRangeLookup =>
        ValueSource == ActionValueSource.RangeLookup;

    /// <summary>True for reference value</summary>
    public bool IsReference => !string.IsNullOrWhiteSpace(ReferenceField);

    /// <summary>Case change reference as field name</summary>
    public string ReferenceField => Reference.ReferenceField;

    /// <summary>Mandatory field value</summary>
    public bool MandatoryField => Reference.MandatoryField;

    /// <summary>The action value mode</summary>
    protected ActionValueReferenceType ReferenceType => Reference.ReferenceType;

    /// <summary>The action value source</summary>
    protected ActionValueSource ValueSource => Reference.ValueSource;

    /// <summary>The action case value</summary>
    public CaseValue CaseValue => Reference.CaseValue;

    /// <summary>The action case value type</summary>
    public ActionValueType? CaseValueType => Reference.CaseValueType;

    /// <summary>The action value method</summary>
    protected IActionMethod Method => Reference.ValueMethod;

    /// <summary>The action attribute key</summary>
    protected string AttributeKey => Reference.AttributeKey;

    /// <summary>The action attribute type</summary>
    protected ActionValueType? AttributeType => Reference.AttributeType;

    /// <summary>The action lookup key</summary>
    protected string LookupKey => Reference.LookupKey;

    /// <summary>The action lookup object key</summary>
    protected string LookupObjectKey => Reference.LookupObjectKey;

    /// <summary>The action lookup range value</summary>
    protected decimal? LookupRangeValue => Reference.LookupRangeValue;

    /// <summary>The action lookup type</summary>
    protected ActionValueType? LookupType => Reference.LookupType;

    /// <summary>The value reference</summary>
    private ValueReference Reference { get; }

    /// <summary>The resolved value</summary>
    public TValue ResolvedValue { get; init; }

    /// <summary>Constructor</summary>
    protected ActionCaseValue(TContext context, object sourceValue, DateTime? valueDate)
    {
        Context = context ?? throw new ArgumentNullException(nameof(context));
        SourceValue = sourceValue ?? throw new ArgumentNullException(nameof(sourceValue));

        // value date
        ValueDate = valueDate ?? Date.Now.AddMinutes(1);

        // reference
        if (sourceValue is string stringValue && !string.IsNullOrWhiteSpace(stringValue))
        {
            Reference = ParseReference(context, stringValue.Trim(), ValueDate);
        }
        else
        {
            Reference = new ValueReference();
        }

        // resolved value
        ResolvedValue = GetResolvedValue();
    }

    /// <summary>Check for fulfilled value</summary>
    public bool IsFulfilled
    {
        get
        {
            if (!MandatoryField)
            {
                return true;
            }

            // not null on nullable type
            if (Nullable.GetUnderlyingType(typeof(TValue)) != null)
            {
                return ResolvedValue != null;
            }

            // string
            if (typeof(TValue) == typeof(string))
            {
                return !string.IsNullOrWhiteSpace(ResolvedValue as string);
            }
            return true;
        }
    }

    /// <summary>Get case value type</summary>
    public ActionValueType GetValueType()
    {
        // type by sub method
        if (Method != null)
        {
            return Method.GetValueType();
        }

        // type by case value
        if (CaseValueType.HasValue)
        {
            return CaseValueType.Value;
        }

        // type by value source
        switch (ValueSource)
        {
            case ActionValueSource.Value:
                return GetSourceValueType();
            case ActionValueSource.Start:
            case ActionValueSource.End:
                return ActionValueType.DateTime;
            case ActionValueSource.Period:
                return ActionValueType.TimeSpan;
            case ActionValueSource.FieldAttribute:
            case ActionValueSource.ValueAttribute:
                if (!AttributeType.HasValue)
                {
                    throw new ScriptException("Missing case field or value attribute.");
                }
                return AttributeType.Value;
            case ActionValueSource.Lookup:
            case ActionValueSource.RangeLookup:
                if (!LookupType.HasValue)
                {
                    throw new ScriptException("Missing lookup value attribute.");
                }
                return LookupType.Value;
            default:
                throw new ScriptException($"Unsupported value type {ValueSource}.");
        }
    }

    private ActionValueType GetSourceValueType()
    {
        return SourceValue switch
        {
            bool => ActionValueType.Boolean,
            int => ActionValueType.Integer,
            decimal => ActionValueType.Decimal,
            string => ActionValueType.String,
            DateTime => ActionValueType.DateTime,
            _ => ActionValueType.None
        };
    }

    /// <summary>Resolved action value</summary>
    private TValue GetResolvedValue()
    {
        object value = null;
        var methodValue = false;
        switch (ValueSource)
        {
            case ActionValueSource.Value:
                switch (ReferenceType)
                {
                    case ActionValueReferenceType.CaseChange:
                    case ActionValueReferenceType.CaseValue:
                        value = CaseValue?.Value != null ? CaseValue.Value.Value : null;
                        break;
                    default:
                        value = SourceValue;
                        break;
                }
                break;
            case ActionValueSource.Start:
                switch (ReferenceType)
                {
                    case ActionValueReferenceType.CaseChange:
                    case ActionValueReferenceType.CaseValue:
                        value = CaseValue?.Start;
                        break;
                    default:
                        throw new ScriptException("Start mode not allowed.");
                }
                break;
            case ActionValueSource.End:
                switch (ReferenceType)
                {
                    case ActionValueReferenceType.CaseChange:
                    case ActionValueReferenceType.CaseValue:
                        value = CaseValue?.End;
                        break;
                    default:
                        throw new ScriptException("End mode not allowed.");
                }
                break;
            case ActionValueSource.Period:
                switch (ReferenceType)
                {
                    case ActionValueReferenceType.CaseChange:
                    case ActionValueReferenceType.CaseValue:
                        if (CaseValue?.Start != null && CaseValue?.End != null)
                        {
                            value = CaseValue.End.Value.Subtract(CaseValue.Start.Value);
                        }
                        break;
                    default:
                        throw new ScriptException("End mode not allowed.");
                }
                break;
            case ActionValueSource.FieldAttribute:
            case ActionValueSource.ValueAttribute:
            case ActionValueSource.Lookup:
            case ActionValueSource.RangeLookup:
                methodValue = true;
                break;
        }
        if (!methodValue && value == null)
        {
            return default;
        }

        // resolve method
        if (Method != null)
        {
            //Context.Function.LogWarning($"GetResolvedValue method: input value={value}, method={Method}");
            value = Method.EvaluateValue(value);
            //Context.Function.LogWarning($"GetResolvedValue method: output value={value}");
            if (value == null)
            {
                return default;
            }
        }

        // convert value
        var type = typeof(TValue);
        var convertType = Nullable.GetUnderlyingType(type) ?? typeof(TValue);
        try
        {
            if (value is JsonElement jsonElement)
            {
                value = jsonElement.GetValue();
            }

            //Context.Function.LogWarning($"GetResolvedValue: value={value}, type={convertType}, Reference={ReferenceField}");
            var result = (TValue)Convert.ChangeType(value, convertType, CultureInfo.InvariantCulture);
            //Context.Function.LogWarning($"GetResolvedValue: result={result}");
            return result;
        }
        catch (Exception exception)
        {
            Context.Function.LogError($"Convert error of value {value} (type={value?.GetType()}, ValueMode={ValueSource})"+
                                      $"\n{exception.GetBaseException().Message}" +
                                      $"\n{new StackTrace()}");
            return default;
        }
    }

    /// <summary>Parse value reference</summary>
    private static ValueReference ParseReference(TContext context, string value, DateTime valueDate)
    {
        string referenceField = null;
        value = value.Trim();
        CaseValue caseValue = null;

        // reference type
        ActionValueReferenceType referenceType = ActionValueReferenceType.None;
        if (value.StartsWith(CaseChangeReference))
        {
            referenceType = ActionValueReferenceType.CaseChange;
            referenceField = value.TrimStart(CaseChangeReference);
        }
        else if (value.StartsWith(CaseValueReference))
        {
            referenceType = ActionValueReferenceType.CaseValue;
            referenceField = value.TrimStart(CaseValueReference);
        }
        else if (value.StartsWith(LookupValueReference))
        {
            referenceType = ActionValueReferenceType.Lookup;
            referenceField = value.TrimStart(LookupValueReference);
        }

        var mandatoryField = true;
        ActionValueSource valueSource = ActionValueSource.Value;
        string method = null;
        if (referenceField != null)
        {
            // mandatory field
            if (referenceField.EndsWith(OptionalFieldMarker))
            {
                mandatoryField = false;
                referenceField = referenceField.TrimEnd(OptionalFieldMarker);
            }

            // value mode
            if (TryParseReference(context, referenceField, ValueModePostfix, out referenceField, out method))
            {
                valueSource = ActionValueSource.Value;
            }
            else if (TryParseReference(context, referenceField, StartModePostfix, out referenceField, out method))
            {
                valueSource = ActionValueSource.Start;
            }
            else if (TryParseReference(context, referenceField, EndModePostfix, out referenceField, out method))
            {
                valueSource = ActionValueSource.End;
            }
            else if (TryParseReference(context, referenceField, PeriodModePostfix, out referenceField, out method))
            {
                valueSource = ActionValueSource.Period;
            }
            else if (TryParseReference(context, referenceField, FieldAttributeModePostfix, out referenceField, out method))
            {
                valueSource = ActionValueSource.FieldAttribute;
            }
            else if (TryParseReference(context, referenceField, ValueAttributeModePostfix, out referenceField, out method))
            {
                valueSource = ActionValueSource.ValueAttribute;
            }
            else if (TryParseReference(context, referenceField, LookupModePostfix, out referenceField, out method))
            {
                valueSource = ActionValueSource.Lookup;
            }
            else if (TryParseReference(context, referenceField, RangeLookupModePostfix, out referenceField, out method))
            {
                valueSource = ActionValueSource.RangeLookup;
            }
        }

        // case value
        ActionValueType? caseValueType = null;
        switch (referenceType)
        {
            case ActionValueReferenceType.CaseChange:
                caseValue = context.GetCaseChangeValue(referenceField);
                caseValueType = GetActionValueType(context.GetCaseValueType(referenceField));
                break;
            case ActionValueReferenceType.CaseValue:
                caseValue = context.GetCaseValue(referenceField, valueDate);
                caseValueType = GetActionValueType(context.GetCaseValueType(referenceField));
                break;
        }
        //context.Function.LogWarning($"ParseReference: CaseValue={caseValue?.CaseFieldName}={caseValue?.Value.Value}, value={value}, method={method}, valueSource={valueSource}");

        // method
        IActionMethod valueMethod = null;
        string attributeKey = null;
        ActionValueType? attributeType = null;
        string lookupKey = null;
        string lookupObjectKey = null;
        decimal? lookupRange = null;
        ActionValueType? lookupType = null;

        // method reference
        try
        {
            if (!string.IsNullOrWhiteSpace(method))
            {
                switch (valueSource)
                {
                    // value
                    case ActionValueSource.Value:
                        if (caseValueType.HasValue)
                        {
                            valueMethod = caseValueType.Value switch
                            {
                                ActionValueType.String => new StringActionValueMethod<TContext, TFunc>(context, method, valueDate),
                                ActionValueType.Boolean => new BooleanActionValueMethod<TContext, TFunc>(context, method, valueDate),
                                ActionValueType.Integer => new IntegerActionValueMethod<TContext, TFunc>(context, method, valueDate),
                                ActionValueType.Decimal => new DecimalActionValueMethod<TContext, TFunc>(context, method, valueDate),
                                ActionValueType.DateTime => new DateTimeActionValueMethod<TContext, TFunc>(context, method, valueDate),
                                _ => null
                            };
                        }
                        break;

                    // start and end
                    case ActionValueSource.Start:
                    case ActionValueSource.End:
                        valueMethod = new DateTimeActionValueMethod<TContext, TFunc>(context, method, valueDate);
                        break;

                    // period
                    case ActionValueSource.Period:
                        valueMethod = new TimeSpanActionValueMethod<TContext, TFunc>(context, method, valueDate);
                        break;

                    // attributes
                    case ActionValueSource.FieldAttribute:
                        if (referenceField != null)
                        {
                            var attributeMethod = new CaseFieldAttributeActionMethod<TContext, TFunc>(context, referenceField, method);
                            attributeKey = attributeMethod.AttributeKey;
                            attributeType = attributeMethod.AttributeType;
                            valueMethod = attributeMethod;
                        }
                        break;
                    case ActionValueSource.ValueAttribute:
                        if (referenceField != null)
                        {
                            var attributeMethod = new CaseValueAttributeActionMethod<TContext, TFunc>(context, referenceField, method);
                            attributeKey = attributeMethod.AttributeKey;
                            attributeType = attributeMethod.AttributeType;
                            valueMethod = attributeMethod;
                        }
                        break;

                    // lookups
                    case ActionValueSource.Lookup:
                        {
                            var attributeMethod = new LookupActionMethod<TContext, TFunc>(context, referenceField, method);
                            lookupKey = attributeMethod.LookupKey;
                            lookupType = attributeMethod.ValueType;
                            valueMethod = attributeMethod;
                        }
                        break;
                    case ActionValueSource.RangeLookup:
                        {
                            var attributeMethod = new RangeLookupActionMethod<TContext, TFunc>(context, referenceField, method);
                            lookupRange = attributeMethod.RangeValue;
                            lookupObjectKey = attributeMethod.ObjectKey;
                            lookupType = attributeMethod.ValueType;
                            valueMethod = attributeMethod;
                        }
                        break;
                }
            }
        }
        catch (Exception exception)
        {
            context.Function.LogError($"**** Error: {exception.GetBaseException().Message}");
        }

        //context.Function.LogWarning($"ParseReference: method={valueMethod}, case value type={caseValueType}");
        return new()
        {
            ReferenceType = referenceType,
            ValueSource = valueSource,
            ReferenceField = referenceField,
            CaseValue = caseValue,
            CaseValueType = caseValueType,
            ValueMethod = valueMethod,
            AttributeType = attributeType,
            AttributeKey = attributeKey,
            LookupKey = lookupKey,
            LookupObjectKey = lookupObjectKey,
            LookupRangeValue = lookupRange,
            LookupType = lookupType,
            MandatoryField = mandatoryField
        };
    }

    private static bool TryParseReference(TContext context, string reference, string marker,
        out string result, out string method)
    {
        var index = GetMarkerIndex(reference, marker);
        if (index < 0)
        {
            result = reference;
            method = null;
            return false;
        }

        // expression without method
        if (reference.EndsWith(marker))
        {
            result = reference.RemoveFromEnd(marker);
            method = null;
            return true;
        }

        // method name end
        var nextChar = reference[index + marker.Length];
        //function.LogWarning($"TryParseReference: {reference} index={index + marker.Length} nextChar={nextChar}");
        if (char.IsLetter(nextChar))
        {
            result = reference;
            method = null;
            return false;
        }

        // expression with method
        method = reference.Substring(index + marker.Length);

        // method separator
        if (string.IsNullOrWhiteSpace(method) || !method.StartsWith('.'))
        {
            context.Function.LogError($"Invalid method expression: {reference} method={method} sourceMarker={marker}");
            result = reference;
            method = null;
            return false;
        }
        method = method.TrimStart('.');

        // method parameters (check only)
        var paramStartIndex = method.IndexOf('(');
        var paramEndIndex = method.IndexOf(')');
        if ((paramStartIndex >= 0 || paramEndIndex >= 0) &&
            (paramStartIndex < 0 && paramEndIndex >= 0 ||
            paramEndIndex < 0 && paramStartIndex >= 0 ||
            paramStartIndex > paramEndIndex))
        {
            context.Function.LogError($"Invalid method expression: {reference}");
            result = reference;
            method = null;
            return false;
        }

        result = reference.Substring(0, index);
        return true;
    }

    /// <summary>Action text</summary>
    public override string ToString()
    {
        object value = ResolvedValue;
        if (value is DateTime dateTimeValue)
        {
            value = dateTimeValue.ToCompactString();
        }
        if (string.IsNullOrWhiteSpace(ReferenceField))
        {
            return $"{value}";
        }
        return ValueSource == ActionValueSource.Value ?
            $"{ReferenceField} {value}" :
            $"{ReferenceField}.{ValueSource} {value}";
    }
}

#endregion

#region Case Actions

/// <summary>Base class for case actions</summary>
public abstract class CaseActionsBase
{
    /// <summary>String type name</summary>
    protected const string StringType = $"{nameof(String)}";

    /// <summary>Integer type name</summary>
    protected const string IntType = "Int";

    /// <summary>Decimal type name</summary>
    protected const string DecimalType = "Dec";

    /// <summary>Date type name</summary>
    protected const string DateType = "Date";

    /// <summary>Boolean type name</summary>
    protected const string BooleanType = "Bool";

    /// <summary>Action value source start</summary>
    protected const string ActionSourceStart = $"{nameof(ActionValueSource.Start)}";

    /// <summary>Action value source end</summary>
    protected const string ActionSourceEnd = $"{nameof(ActionValueSource.End)}";

    /// <summary>Action value source period</summary>
    protected const string ActionSourcePeriod = $"{nameof(ActionValueSource.Period)}";

    /// <summary>Action value source value</summary>
    protected const string ActionSourceValue = $"{nameof(ActionValueSource.Value)}";

    /// <summary>Action value source field attribute</summary>
    protected const string ActionSourceFieldAttribute = $"{nameof(ActionValueSource.FieldAttribute)}";

    /// <summary>Action value source value attribute</summary>
    protected const string ActionSourceValueAttribute = $"{nameof(ActionValueSource.ValueAttribute)}";

    /// <summary>Action lookup source value</summary>
    protected const string ActionSourceLookup = $"{nameof(ActionValueSource.Lookup)}";

    /// <summary>Action range lookup source value</summary>
    protected const string ActionSourceRangeLookup = $"{nameof(ActionValueSource.RangeLookup)}";

    /// <summary>Action reference none</summary>
    protected const string ActionReferenceNone = $"{nameof(ActionValueReferenceType.None)}";

    /// <summary>Action reference case value</summary>
    protected const string ActionReferenceCaseValue = $"{nameof(ActionValueReferenceType.CaseValue)}";

    /// <summary>Action reference case change</summary>
    protected const string ActionReferenceCaseChange = $"{nameof(ActionValueReferenceType.CaseChange)}";

    /// <summary>Action reference lookup</summary>
    protected const string ActionReferenceLookup = $"{nameof(ActionValueReferenceType.Lookup)}";

    /// <summary>The action namespace</summary>
    public string Namespace { get; }

    /// <summary>Default constructor</summary>
    protected CaseActionsBase()
    {
        var attribute = GetType().GetCustomAttribute<ActionProviderAttribute>();
        if (attribute == null)
        {
            throw new ScriptException($"Missing action provider attribute on type {GetType()}.");
        }
        Namespace = attribute.Namespace;
    }
}

#endregion