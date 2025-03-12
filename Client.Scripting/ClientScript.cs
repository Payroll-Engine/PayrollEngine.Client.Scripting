/* ClientScript */

using System;
using System.Data;

namespace PayrollEngine.Client.Scripting;

#region Enums

/// <summary>Decimal rounding types</summary>
public enum DecimalRounding
{
    /// <summary>No rounding</summary>
    None = 0,

    /// <summary>Round to nearest integer</summary>
    Integer = 1,

    /// <summary>Round to half (e.g. 50 cents)</summary>
    Half = 2,

    /// <summary>Round to one-fifth (e.g. 20 cents)</summary>
    Fifth = 5,

    /// <summary>Round to one-tenth (e.g. 10 cents)</summary>
    Tenth = 10,

    /// <summary>Round to one-twentieth (e.g. 5 cents)</summary>
    Twentieth = 20,

    /// <summary>Round to one-fiftieth (e.g. 2 cents)</summary>
    Fiftieth = 50,

    /// <summary>Round to one-hundredth (e.g. 1 cent)</summary>
    Hundredth = 100
}

/// <summary>The object status</summary>
public enum ObjectStatus
{
    /// <summary>Object is active</summary>
    Active = 0,
    /// <summary>Object is inactive</summary>
    Inactive = 1
}

/// <summary>The year months</summary>
public enum Month
{
    /// <summary>January</summary>
    January = 1,
    /// <summary>February</summary>
    February = 2,
    /// <summary>March</summary>
    March = 3,
    /// <summary>April</summary>
    April = 4,
    /// <summary>May</summary>
    May = 5,
    /// <summary>June</summary>
    June = 6,
    /// <summary>July</summary>
    July = 7,
    /// <summary>August</summary>
    August = 8,
    /// <summary>September</summary>
    September = 9,
    /// <summary>October</summary>
    October = 10,
    /// <summary>November</summary>
    November = 11,
    /// <summary>December</summary>
    December = 12
}

/// <summary>Specifies the meaning and relative importance of a log event</summary>
public enum LogLevel
{
    /// <summary>Anything and everything you might want to know about a running block of code</summary>
    Verbose,
    /// <summary>Internal system events that aren't necessarily observable from the outside</summary>
    Debug,
    /// <summary>The lifeblood of operational intelligence - things happen</summary>
    Information,
    /// <summary>Service is degraded or endangered</summary>
    Warning,
    /// <summary>Functionality is unavailable, invariants are broken or data is lost</summary>
    Error,
    /// <summary>If you have a pager, it goes off when one of these occurs/// </summary>
    Fatal
}

/// <summary>The type of the user</summary>
public enum UserType
{
    /// <summary>Regular user</summary>
    User = 0,
    /// <summary>User with employee self-service</summary>
    Employee = 1,
    /// <summary>Tenant administrator</summary>
    TenantAdministrator = 2,
    /// <summary>System administrator</summary>
    SystemAdministrator = 3
}

/// <summary>The case type</summary>
public enum CaseType
{
    /// <summary>Global case</summary>
    Global = 0,
    /// <summary>National case</summary>
    National = 1,
    /// <summary>Company case</summary>
    Company = 2,
    /// <summary>Employee case</summary>
    Employee = 3
}

/// <summary>The payroll value types for cases</summary>
public enum ValueType
{
    /// <summary>String (base type string)</summary>
    String = 0,
    /// <summary>Boolean (base type boolean)</summary>
    Boolean = 1,
    /// <summary>Integer (base type numeric)</summary>
    Integer = 2,
    /// <summary>Numeric boolean, any non-zero value means true (base type numeric)</summary>
    NumericBoolean = 3,
    /// <summary>Decimal (base type numeric)</summary>
    Decimal = 4,
    /// <summary>Date and time (base type string)</summary>
    DateTime = 5,
    /// <summary>No value type (base type null)</summary>
    None = 6,

    /// <summary>Date (base type string)</summary>
    Date = 10,

    /// <summary>Day of week (base type integer 0..6)</summary>
    Weekday = 20,
    /// <summary>Month (base type integer 0..11)</summary>
    Month = 21,

    /// <summary>Money (base type decimal)</summary>
    Money = 30,
    /// <summary>Percentage (base type decimal)</summary>
    Percent = 31,

    /// <summary>Web Resource e.g. Url (base type string)</summary>
    WebResource = 40
}

/// <summary>The payrun execution stage</summary>
public enum PayrunExecutionPhase
{
    /// <summary>Job setup execution phase</summary>
    Setup = 0,
    /// <summary>Job reevaluation execution phase</summary>
    Reevaluation = 1
}

/// <summary>The payrun job type</summary>
[Flags]
public enum PayrunJobStatus
{
    /// <summary>Draft Legal results (default)</summary>
    Draft = 0x0000,
    /// <summary>Legal results are released for processing</summary>
    Release = 0x0001,
    /// <summary>Legal results are processed</summary>
    Process = 0x0002,
    /// <summary>Legal results has been processed successfully</summary>
    Complete = 0x0004,
    /// <summary>Forecast results</summary>
    Forecast = 0x0008,
    /// <summary>Unreleased Job has been aborted</summary>
    Abort = 0x0010,
    /// <summary>Released Job has been canceled</summary>
    Cancel = 0x0020,
    /// <summary>Working status, including draft, release and process</summary>
    Working = Draft | Release | Process,
    /// <summary>Legal status, including release, process and complete</summary>
    Legal = Release | Process | Complete,
    /// <summary>Final status, including complete, forecast, abort and cancel</summary>
    Final = Complete | Forecast | Abort | Cancel
}

/// <summary>The data merge schema change</summary>
public enum DataMergeSchemaChange
{
    /// <summary>Adds the necessary columns to complete the schema</summary>
    Add = MissingSchemaAction.Add,
    /// <summary>Ignores the extra columns</summary>
    Ignore = MissingSchemaAction.Ignore,
    /// <summary>An <see cref="InvalidOperationException"/> is generated if the specified column mapping is missing.</summary>
    Error = MissingSchemaAction.Error
}

#endregion

#region Input

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

/// <summary>Predefined input attributes</summary>
public static class InputAttributes
{
    public static readonly string Prefix = "input.";

    // transient
    public static readonly string EditInfo = $"{Prefix}editInfo";
    public static readonly string Validity = $"{Prefix}validity";

    // case general
    public static readonly string Icon = $"{Prefix}icon";
    public static readonly string Priority = $"{Prefix}priority";

    // case field general
    public static readonly string Group = $"{Prefix}group";
    public static readonly string Separator = $"{Prefix}separator";
    public static readonly string Hidden = $"{Prefix}hidden";
    public static readonly string HiddenName = $"{Prefix}hiddenName";
    public static readonly string HiddenDates = $"{Prefix}hiddenDates";
    public static readonly string ShowDescription = $"{Prefix}showDescription";
    public static readonly string Variant = $"{Prefix}variant";

    // case field start
    public static readonly string StartLabel = $"{Prefix}startLabel";
    public static readonly string StartHelp = $"{Prefix}startHelp";
    public static readonly string StartRequired = $"{Prefix}startRequired";
    public static readonly string StartReadOnly = $"{Prefix}startReadOny";
    public static readonly string StartHidden = $"{Prefix}startHidden";
    public static readonly string StartFormat = $"{Prefix}startFormat";
    public static readonly string StartPickerOpen = $"{Prefix}startPickerOpen";
    public static readonly string StartPickerType = $"{Prefix}startPickerType";

    // case field end
    public static readonly string EndLabel = $"{Prefix}endLabel";
    public static readonly string EndHelp = $"{Prefix}endHelp";
    public static readonly string EndRequired = $"{Prefix}endRequired";
    public static readonly string EndReadOnly = $"{Prefix}endReadOny";
    public static readonly string EndHidden = $"{Prefix}endHidden";
    public static readonly string EndFormat = $"{Prefix}endFormat";
    public static readonly string EndPickerOpen = $"{Prefix}endPickerOpen";
    public static readonly string EndPickerType = $"{Prefix}endPickerType";

    // case field value
    public static readonly string ValueLabel = $"{Prefix}valueLabel";
    public static readonly string ValueAdornment = $"{Prefix}valueAdornment";
    public static readonly string ValueHelp = $"{Prefix}valueHelp";
    public static readonly string ValueMask = $"{Prefix}valueMask";
    public static readonly string ValueRequired = $"{Prefix}valueRequired";
    public static readonly string ValueReadOnly = $"{Prefix}valueReadOnly";
    public static readonly string ValuePickerOpen = $"{Prefix}valuePickerOpen";
    public static readonly string ValuePickerStatic = $"{Prefix}valuePickerStatic";
    public static readonly string ValueTimePicker = $"{Prefix}valueTimePicker";
    public static readonly string Culture = $"{Prefix}culture";
    public static readonly string MinValue = $"{Prefix}minValue";
    public static readonly string MaxValue = $"{Prefix}maxValue";
    public static readonly string StepSize = $"{Prefix}stepSize";
    public static readonly string Format = $"{Prefix}format";
    public static readonly string LineCount = $"{Prefix}lineCount";
    public static readonly string MaxLength = $"{Prefix}maxLength";
    public static readonly string Check = $"{Prefix}check";
    public static readonly string ValueHistory = $"{Prefix}valueHistory";

    // case field attachments
    public static readonly string Attachment = $"{Prefix}attachment";
    public static readonly string AttachmentExtensions = $"{Prefix}attachmentExtensions";

    // list
    // no actions for the list attributes List, ListSelection and ListResult
    public static readonly string List = $"{Prefix}list";
    public static readonly string ListValues = $"{Prefix}listValues";
    public static readonly string ListSelection = $"{Prefix}listSelection";
}

#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

#endregion

#region Error

/// <summary>Payroll script exception</summary>
public class ScriptException : Exception
{
    /// <summary>Initializes a new instance of the exception</summary>
    public ScriptException(string message) :
        base(message)
    {
    }

    /// <summary>Initializes a new instance of the exception</summary>
    public ScriptException(string message, Exception innerException) :
        base(message, innerException)
    {
    }
}

#endregion

#region Collections

/// <summary>Script value dictionary</summary>
public class ScriptDictionary<TKey, TValue>
{
    private Func<TKey, TValue> GetValueHandler { get; }
    private Action<TKey, TValue> SetValueHandler { get; }

    /// <summary>New read-only dictionary</summary>
    public ScriptDictionary(Func<TKey, TValue> getValueHandler)
    {
        GetValueHandler = getValueHandler ?? throw new ArgumentNullException(nameof(getValueHandler));
    }

    /// <summary>New dictionary</summary>
    public ScriptDictionary(Func<TKey, TValue> getValueHandler, Action<TKey, TValue> setValueHandler) :
        this(getValueHandler)
    {
        SetValueHandler = setValueHandler ?? throw new ArgumentNullException(nameof(setValueHandler));
    }

    /// <summary>Query value by index</summary>
    public TValue this[TKey key]
    {
        get => GetValueHandler(key);
        set
        {
            if (SetValueHandler == null)
            {
                throw new ScriptException($"Write operation on read-only scripting dictionary: {key}={value}.");
            }
            SetValueHandler(key, value);
        }
    }
}

#endregion

#region Actions

/// <summary>Action provider type</summary>
[AttributeUsage(AttributeTargets.Class)]
public sealed class ActionProviderAttribute : Attribute
{
    /// <summary>The extension function type</summary>
    public Type Type { get; }

    /// <summary>The extension namespace</summary>
    public string Namespace { get; }

    /// <summary>Initializes a new instance of the <see cref="ActionProviderAttribute"/> class</summary>
    /// <param name="namespace">The extension namespace</param>
    /// <param name="type">The extension function type</param>
    public ActionProviderAttribute(string @namespace, Type type)
    {
        if (string.IsNullOrWhiteSpace(@namespace))
        {
            throw new ArgumentException(nameof(@namespace));
        }
        Namespace = @namespace;
        Type = type ?? throw new ArgumentNullException(nameof(type));
    }
}

/// <summary>Attribute for action parameter</summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public sealed class ActionParameterAttribute : Attribute
{
    /// <summary>The action parameter name</summary>
    public string Name { get; }

    /// <summary>The action description</summary>
    public string Description { get; }

    /// <summary>The action parameter types</summary>
    public string[] ValueTypes { get; }

    /// <summary>The action parameter source types</summary>
    public string[] ValueSources { get; }

    /// <summary>The action parameter reference types</summary>
    public string[] ValueReferences { get; }

    /// <summary>Initializes a new instance of the <see cref="ActionAttribute"/> class</summary>
    /// <param name="name">The action signature</param>
    /// <param name="description">The action description</param>
    /// <param name="valueTypes">The action parameter types</param>
    /// <param name="valueSources">The action parameter source types</param>
    /// <param name="valueReferences">The action parameter reference types</param>
    public ActionParameterAttribute(string name, string description = null, string[] valueTypes = null,
        string[] valueSources = null, string[] valueReferences = null)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException(nameof(name));
        }
        Name = name;
        Description = description;
        ValueTypes = valueTypes;
        ValueSources = valueSources;
        ValueReferences = valueReferences;
    }
}

/// <summary>Attribute for action issue</summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public sealed class ActionIssueAttribute : Attribute
{
    /// <summary>The action name</summary>
    public string Name { get; }

    /// <summary>The issue message</summary>
    public string Message { get; }

    /// <summary>The message parameter count</summary>
    public int ParameterCount { get; }

    /// <summary>Initializes a new instance of the <see cref="ActionIssueAttribute"/> class</summary>
    /// <param name="name">The action name</param>
    /// <param name="message">The issue message</param>
    /// <param name="parameterCount">The message parameter count</param>
    public ActionIssueAttribute(string name, string message, int parameterCount)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException(nameof(name));
        }
        if (string.IsNullOrWhiteSpace(message))
        {
            throw new ArgumentException(nameof(message));
        }
        if (parameterCount < 0 || parameterCount > 9)
        {
            throw new ArgumentOutOfRangeException(nameof(parameterCount));
        }
        Name = name;
        Message = message;
        ParameterCount = parameterCount;
    }
}

/// <summary>Attribute for action</summary>
[AttributeUsage(AttributeTargets.Method)]
public abstract class ActionAttribute : Attribute
{
    /// <summary>The action name</summary>
    public string Name { get; }

    /// <summary>The action description</summary>
    public string Description { get; }

    /// <summary>The action categories</summary>
    public string[] Categories { get; }

    /// <summary>Initializes a new instance of the <see cref="ActionAttribute"/> class</summary>
    /// <param name="name">The action name</param>
    /// <param name="description">The action description</param>
    /// <param name="categories">The action categories</param>
    protected ActionAttribute(string name, string description = null, params string[] categories)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException(nameof(name));
        }
        Name = name;
        Description = description;
        Categories = categories;
    }
}

/// <summary>Attribute for case available action</summary>
public sealed class CaseAvailableActionAttribute : ActionAttribute
{
    /// <summary>Initializes a new instance of the <see cref="CaseAvailableActionAttribute"/> class</summary>
    /// <param name="name">The action name</param>
    /// <param name="description">The action description</param>
    /// <param name="categories">The action categories</param>
    public CaseAvailableActionAttribute(string name, string description = null, params string[] categories) :
        base(name, description, categories)
    {
    }
}

/// <summary>Attribute for case change action</summary>
public abstract class CaseChangeActionAttribute : ActionAttribute
{
    /// <summary>Initializes a new instance of the <see cref="CaseBuildActionAttribute"/> class</summary>
    /// <param name="name">The action name</param>
    /// <param name="description">The action description</param>
    /// <param name="categories">The action categories</param>
    protected CaseChangeActionAttribute(string name, string description = null, params string[] categories) :
        base(name, description, categories)
    {
    }
}

/// <summary>Attribute for case build action</summary>
[AttributeUsage(AttributeTargets.Method)]
public sealed class CaseBuildActionAttribute : CaseChangeActionAttribute
{
    /// <summary>Initializes a new instance of the <see cref="CaseBuildActionAttribute"/> class</summary>
    /// <param name="name">The action name</param>
    /// <param name="description">The action description</param>
    /// <param name="categories">The action categories</param>
    public CaseBuildActionAttribute(string name, string description = null, params string[] categories) :
        base(name, description, categories)
    {
    }
}

/// <summary>Attribute for case validate action</summary>
[AttributeUsage(AttributeTargets.Method)]
public sealed class CaseValidateActionAttribute : CaseChangeActionAttribute
{
    /// <summary>Initializes a new instance of the <see cref="CaseValidateActionAttribute"/> class</summary>
    /// <param name="name">The action name</param>
    /// <param name="description">The action description</param>
    /// <param name="categories">The action categories</param>
    public CaseValidateActionAttribute(string name, string description = null, params string[] categories) :
        base(name, description, categories)
    {
    }
}

/// <summary>Attribute for case change action</summary>
public abstract class CaseRelationActionAttribute : ActionAttribute
{
    /// <summary>Initializes a new instance of the <see cref="CaseRelationActionAttribute"/> class</summary>
    /// <param name="name">The action name</param>
    /// <param name="description">The action description</param>
    /// <param name="categories">The action categories</param>
    protected CaseRelationActionAttribute(string name, string description = null, params string[] categories) :
        base(name, description, categories)
    {
    }
}

/// <summary>Attribute for case relation build action</summary>
[AttributeUsage(AttributeTargets.Method)]
public sealed class CaseRelationBuildActionAttribute : CaseRelationActionAttribute
{
    /// <summary>Initializes a new instance of the <see cref="CaseRelationBuildActionAttribute"/> class</summary>
    /// <param name="name">The action name</param>
    /// <param name="description">The action description</param>
    /// <param name="categories">The action categories</param>
    public CaseRelationBuildActionAttribute(string name, string description = null, params string[] categories) :
        base(name, description, categories)
    {
    }
}

/// <summary>Attribute for case validate action</summary>
[AttributeUsage(AttributeTargets.Method)]
public sealed class CaseRelationValidateActionAttribute : CaseRelationActionAttribute
{
    /// <summary>Initializes a new instance of the <see cref="CaseRelationValidateActionAttribute"/> class</summary>
    /// <param name="name">The action name</param>
    /// <param name="description">The action description</param>
    /// <param name="categories">The action categories</param>
    public CaseRelationValidateActionAttribute(string name, string description = null, params string[] categories) :
        base(name, description, categories)
    {
    }
}

#endregion
