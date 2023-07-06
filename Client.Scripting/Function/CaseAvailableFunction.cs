/* CaseAvailableFunction */
// ReSharper disable RedundantUsingDirective
using System;
using System.Collections.Generic;
using System.Linq;
// ReSharper restore RedundantUsingDirective

namespace PayrollEngine.Client.Scripting.Function;

#region Action

/// <summary>Case available action context</summary>
public class CaseAvailableActionContext : PayrollActionContext<CaseAvailableFunction>
{
    /// <summary>Constructor</summary>
    public CaseAvailableActionContext(CaseAvailableFunction function) :
        base(function)
    {
    }
}

/// <summary>Case available action case value</summary>
public class CaseAvailableActionValueContext : PayrollActionValueContext<CaseAvailableFunction>
{
    /// <summary>Constructor</summary>
    /// <param name="function">The function</param>
    public CaseAvailableActionValueContext(CaseAvailableFunction function) :
        base(function)
    {
    }
}

/// <summary>Action case available value</summary>
public class CaseAvailableActionValue<TValue> : ActionCaseValue<CaseAvailableActionValueContext, CaseAvailableFunction, TValue>
{
    /// <summary>Default constructor</summary>
    public CaseAvailableActionValue(CaseAvailableActionValueContext context, object sourceValue, DateTime? valueDate = null) :
        base(context, sourceValue, valueDate)
    {
    }
}

/// <summary>Base class for case available actions</summary>
public abstract class CaseAvailableActionsBase : CaseActionsBase
{
    /// <summary>Compare culture</summary>
    protected static StringComparison GetCompareCulture(bool ignoreCase) =>
        ignoreCase ? StringComparison.InvariantCultureIgnoreCase : StringComparison.InvariantCulture;

    /// <summary>New source action</summary>
    protected static CaseAvailableActionValue<TValue> GetSourceActionValue<TValue>(CaseAvailableActionContext context,
        string source)
    {
        if (string.IsNullOrWhiteSpace(source))
        {
            throw new ArgumentException("Invalid case available source", nameof(source));
        }

        try
        {
            return new CaseAvailableActionValue<TValue>(new(context.Function), ActionCaseValueBase.ToCaseValueReference(source));
        }
        catch (Exception exception)
        {
            context.Function.LogError($"Invalid case field name {source}: {exception.GetBaseException().Message}");
            return default;
        }
    }

    /// <summary>New action</summary>
    protected static CaseAvailableActionValue<TValue> GetActionValue<TValue>(CaseAvailableActionContext context,
        object value, DateTime? valueDate = null)
    {
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        try
        {
            return new(new(context.Function), value, valueDate);
        }
        catch (Exception exception)
        {
            context.Function.LogError($"Invalid case action value {value}: {exception.GetBaseException().Message}");
            return default;
        }
    }

    /// <summary>Resolve action value</summary>
    protected static TValue ResolveActionValue<TValue>(CaseAvailableActionContext context, object value) =>
        GetActionValue<TValue>(context, value).ResolvedValue;
}

#endregion

/// <summary>Test if a case is available (default: true), optionally considering related source case values</summary>
/// <example>
/// <code language="c#">
/// // Example with case value
/// (int)Employee["Level"] >= 2
/// </code>
/// <code language="c#">
/// // Example with related case value
/// HasCaseValue("Wage")
/// </code>
/// <code language="c#">
/// // Example with optional related case value
/// HasCaseValue("Wage") ? (int)Employee["Level"] >= 2 : false
/// </code>
/// </example>
// ReSharper disable once PartialTypeWithSinglePart
public partial class CaseAvailableFunction : CaseFunction
{
    /// <summary>Initializes a new instance with the function runtime</summary>
    /// <param name="runtime">The runtime</param>
    public CaseAvailableFunction(object runtime) :
        base(runtime)
    {
    }

    /// <summary>New function instance without runtime (scripting development)</summary>
    /// <remarks>Use <see cref="Function.GetSourceFileName"/> in your constructor for the source file name</remarks>
    /// <param name="sourceFileName">The name of the source file</param>
    protected CaseAvailableFunction(string sourceFileName) :
        base(sourceFileName)
    {
    }

    /// <summary>Get case available actions</summary>
    public string[] GetAvailableActions() =>
        Runtime.GetAvailableActions();

    /// <exclude />
    public bool? IsAvailable()
    {
        // case available actions
        if (!InvokeAvailableActions())
        {
            return false;
        }

        // ReSharper disable EmptyRegion
        #region Function
        #endregion
        // ReSharper restore EmptyRegion

        // compiler will optimize this out if the code provides a return
        return default;
    }

    private bool InvokeAvailableActions()
    {
        var context = new CaseAvailableActionContext(this);
        foreach (var action in GetAvailableActions())
        {
            InvokeConditionAction<CaseAvailableActionContext, CaseAvailableActionAttribute>(context, action);
            if (context.HasIssues)
            {
                return false;
            }
        }
        return true;
    }
}