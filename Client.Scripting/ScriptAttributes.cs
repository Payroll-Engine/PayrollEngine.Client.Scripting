using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.Json;

namespace PayrollEngine.Client.Scripting;

#region Case

/// <summary>Attribute for case validate script</summary>
[AttributeUsage(AttributeTargets.Method)]
public sealed class CaseValidateScriptAttribute : CaseChangeScriptAttribute
{
    /// <summary>Initializes a new instance of the <see cref="CaseValidateScriptAttribute"/> class</summary>
    /// <param name="caseName">Name of the case</param>
    public CaseValidateScriptAttribute(string caseName) :
        base(caseName)
    {
    }
}

/// <summary>Attribute for case build script</summary>
[AttributeUsage(AttributeTargets.Method)]
public sealed class CaseBuildScriptAttribute : CaseChangeScriptAttribute
{
    /// <summary>Initializes a new instance of the <see cref="CaseBuildScriptAttribute"/> class</summary>
    /// <param name="caseName">Name of the case</param>
    public CaseBuildScriptAttribute(string caseName) :
        base(caseName)
    {
    }
}

/// <summary>Attribute for case script</summary>
public abstract class CaseChangeScriptAttribute : CaseScriptAttribute
{
    /// <summary>Initializes a new instance of the <see cref="CaseScriptAttribute"/> class</summary>
    /// <param name="caseName">Name of the case</param>
    protected CaseChangeScriptAttribute(string caseName) :
        base(caseName)
    {
    }
}

/// <summary>Attribute for case available script</summary>
[AttributeUsage(AttributeTargets.Method)]
public sealed class CaseAvailableScriptAttribute : CaseScriptAttribute
{
    /// <summary>Initializes a new instance of the <see cref="CaseAvailableScriptAttribute"/> class</summary>
    /// <param name="caseName">Name of the case</param>
    public CaseAvailableScriptAttribute(string caseName) :
        base(caseName)
    {
    }
}

/// <summary>Attribute for case script</summary>
public abstract class CaseScriptAttribute : ScriptAttribute
{
    /// <summary>The case name</summary>
    public string CaseName { get; }

    /// <summary>Gets the script key</summary>
    public override string ScriptKey => CaseName;

    /// <summary>Initializes a new instance of the <see cref="CaseScriptAttribute"/> class</summary>
    /// <param name="caseName">Name of the case</param>
    protected CaseScriptAttribute(string caseName)
    {
        if (string.IsNullOrWhiteSpace(caseName))
        {
            throw new ArgumentException(nameof(caseName));
        }

        CaseName = caseName;
    }
}

#endregion

#region Case Relation

/// <summary>Attribute for case relation validate script</summary>
[AttributeUsage(AttributeTargets.Method)]
public sealed class CaseRelationValidateScriptAttribute : CaseRelationScriptAttribute
{
    /// <summary>Initializes a new instance of the <see cref="CaseRelationValidateScriptAttribute"/> class</summary>
    /// <param name="sourceCaseName">Name of the source case</param>
    /// <param name="targetCaseName">Name of the target case</param>
    /// <param name="sourceCaseSlot">Name of the source case slot</param>
    /// <param name="targetCaseSlot">Name of the target case slot</param>
    public CaseRelationValidateScriptAttribute(string sourceCaseName, string targetCaseName,
        string sourceCaseSlot = null, string targetCaseSlot = null) :
        base(sourceCaseName, targetCaseName, sourceCaseSlot, targetCaseSlot)
    {
    }
}

/// <summary>Attribute for case relation build script</summary>
[AttributeUsage(AttributeTargets.Method)]
public sealed class CaseRelationBuildScriptAttribute : CaseRelationScriptAttribute
{
    /// <summary>Initializes a new instance of the <see cref="CaseRelationBuildScriptAttribute"/> class</summary>
    /// <param name="sourceCaseName">Name of the source case</param>
    /// <param name="targetCaseName">Name of the target case</param>
    /// <param name="sourceCaseSlot">Name of the source case slot</param>
    /// <param name="targetCaseSlot">Name of the target case slot</param>
    public CaseRelationBuildScriptAttribute(string sourceCaseName, string targetCaseName,
        string sourceCaseSlot = null, string targetCaseSlot = null) :
        base(sourceCaseName, targetCaseName, sourceCaseSlot, targetCaseSlot)
    {
    }
}

/// <summary>Attribute for case relation script</summary>
public abstract class CaseRelationScriptAttribute : ScriptAttribute
{
    /// <summary>The source case name</summary>
    public string SourceCaseName { get; }

    /// <summary>The source case slot</summary>
    public string SourceCaseSlot { get; }

    /// <summary>The target case name</summary>
    public string TargetCaseName { get; }

    /// <summary>The target case slot</summary>
    public string TargetCaseSlot { get; }

    /// <summary>Gets the script key</summary>
    public override string ScriptKey => SourceCaseName.ToCaseRelationKey(TargetCaseName);

    /// <summary>Initializes a new instance of the <see cref="CaseRelationScriptAttribute"/> class</summary>
    /// <param name="sourceCaseName">Name of the source case name</param>
    /// <param name="targetCaseName">Name of the target case name</param>
    /// <param name="sourceCaseSlot">Name of the source case slot</param>
    /// <param name="targetCaseSlot">Name of the target case slot</param>
    protected CaseRelationScriptAttribute(string sourceCaseName, string targetCaseName,
        string sourceCaseSlot = null, string targetCaseSlot = null)
    {
        if (string.IsNullOrWhiteSpace(sourceCaseName))
        {
            throw new ArgumentException(nameof(sourceCaseName));
        }
        if (string.IsNullOrWhiteSpace(targetCaseName))
        {
            throw new ArgumentException(nameof(targetCaseName));
        }

        SourceCaseName = sourceCaseName;
        SourceCaseSlot = sourceCaseSlot;
        TargetCaseName = targetCaseName;
        TargetCaseSlot = targetCaseSlot;
    }
}

#endregion

#region Wage Type

/// <summary>Attribute for wage type result script</summary>
[AttributeUsage(AttributeTargets.Method)]
public sealed class WageTypeResultScriptAttribute : WageTypeScriptAttribute
{
    /// <summary>Initializes a new instance of the <see cref="WageTypeResultScriptAttribute"/> class</summary>
    /// <param name="wageTypeNumber">The wage type number</param>
    public WageTypeResultScriptAttribute(string wageTypeNumber) :
        base(wageTypeNumber)
    {
    }
}

/// <summary>Attribute for wage type value script</summary>
[AttributeUsage(AttributeTargets.Method)]
public sealed class WageTypeValueScriptAttribute : WageTypeScriptAttribute
{
    /// <summary>Initializes a new instance of the <see cref="WageTypeValueScriptAttribute"/> class</summary>
    /// <param name="wageTypeNumber">The wage type number</param>
    public WageTypeValueScriptAttribute(string wageTypeNumber) :
        base(wageTypeNumber)
    {
    }
}

/// <summary>Attribute for case relation script</summary>
public abstract class WageTypeScriptAttribute : ScriptAttribute
{
    /// <summary>The wage type number</summary>
    public string WageTypeNumber { get; set; }

    /// <summary>Gets the script key</summary>
    public override string ScriptKey => WageTypeNumber.ToString(CultureInfo.InvariantCulture);

    /// <summary>Initializes a new instance of the <see cref="WageTypeScriptAttribute"/> class</summary>
    /// <param name="wageTypeNumber">The wage type number</param>
    protected WageTypeScriptAttribute(string wageTypeNumber)
    {
        WageTypeNumber = wageTypeNumber;
    }
}

#endregion

#region Collector

/// <summary>Attribute for collector end script</summary>
[AttributeUsage(AttributeTargets.Method)]
public sealed class CollectorEndScriptAttribute : CollectorScriptAttribute
{
    /// <summary>Initializes a new instance of the <see cref="CollectorEndScriptAttribute"/> class</summary>
    /// <param name="collectorName">Name of the collector</param>
    public CollectorEndScriptAttribute(string collectorName) :
        base(collectorName)
    {
    }
}

/// <summary>Attribute for collector apply script</summary>
[AttributeUsage(AttributeTargets.Method)]
public sealed class CollectorApplyScriptAttribute : CollectorScriptAttribute
{
    /// <summary>Initializes a new instance of the <see cref="CollectorApplyScriptAttribute"/> class</summary>
    /// <param name="collectorName">Name of the collector</param>
    public CollectorApplyScriptAttribute(string collectorName) :
        base(collectorName)
    {
    }
}

/// <summary>Attribute for collector start script</summary>
[AttributeUsage(AttributeTargets.Method)]
public sealed class CollectorStartScriptAttribute : CollectorScriptAttribute
{
    /// <summary>Initializes a new instance of the <see cref="CollectorStartScriptAttribute"/> class</summary>
    /// <param name="collectorName">Name of the collector</param>
    public CollectorStartScriptAttribute(string collectorName) :
        base(collectorName)
    {
    }
}

/// <summary>Attribute for collector script</summary>
public abstract class CollectorScriptAttribute : ScriptAttribute
{
    /// <summary>The collector name</summary>
    public string CollectorName { get; }

    /// <summary>Gets the script key</summary>
    public override string ScriptKey => CollectorName;

    /// <summary>Initializes a new instance of the <see cref="CollectorScriptAttribute"/> class</summary>
    /// <param name="collectorName">Name of the collector</param>
    protected CollectorScriptAttribute(string collectorName)
    {
        if (string.IsNullOrWhiteSpace(collectorName))
        {
            throw new ArgumentException(nameof(collectorName));
        }

        CollectorName = collectorName;
    }
}

#endregion

#region Payrun

/// <summary>Attribute for payrun start script</summary>
[AttributeUsage(AttributeTargets.Method)]
public sealed class PayrunStartScriptAttribute : PayrunScriptAttribute
{
    /// <summary>Initializes a new instance of the <see cref="PayrunStartScriptAttribute"/> class</summary>
    /// <param name="payrunName">Name of the payrun</param>
    public PayrunStartScriptAttribute(string payrunName) :
        base(payrunName)
    {
    }
}

/// <summary>Attribute for payrun wage type available script</summary>
[AttributeUsage(AttributeTargets.Method)]
public sealed class PayrunWageTypeAvailableScriptAttribute : PayrunScriptAttribute
{
    /// <summary>Initializes a new instance of the <see cref="PayrunWageTypeAvailableScriptAttribute"/> class</summary>
    /// <param name="payrunName">Name of the payrun</param>
    public PayrunWageTypeAvailableScriptAttribute(string payrunName) :
        base(payrunName)
    {
    }
}

/// <summary>Attribute for payrun employee available script</summary>
[AttributeUsage(AttributeTargets.Method)]
public sealed class PayrunEmployeeAvailableScriptAttribute : PayrunScriptAttribute
{
    /// <summary>Initializes a new instance of the <see cref="PayrunEmployeeAvailableScriptAttribute"/> class</summary>
    /// <param name="payrunName">Name of the payrun</param>
    public PayrunEmployeeAvailableScriptAttribute(string payrunName) :
        base(payrunName)
    {
    }
}

/// <summary>Attribute for payrun employee start script</summary>
[AttributeUsage(AttributeTargets.Method)]
public sealed class PayrunEmployeeStartScriptAttribute : PayrunScriptAttribute
{
    /// <summary>Initializes a new instance of the <see cref="PayrunEmployeeStartScriptAttribute"/> class</summary>
    /// <param name="payrunName">Name of the payrun</param>
    public PayrunEmployeeStartScriptAttribute(string payrunName) :
        base(payrunName)
    {
    }
}

/// <summary>Attribute for payrun employee end script</summary>
[AttributeUsage(AttributeTargets.Method)]
public sealed class PayrunEmployeeEndScriptAttribute : PayrunScriptAttribute
{
    /// <summary>Initializes a new instance of the <see cref="PayrunEmployeeEndScriptAttribute"/> class</summary>
    /// <param name="payrunName">Name of the payrun</param>
    public PayrunEmployeeEndScriptAttribute(string payrunName) :
        base(payrunName)
    {
    }
}

/// <summary>Attribute for payrun end script</summary>
[AttributeUsage(AttributeTargets.Method)]
public sealed class PayrunEndScriptAttribute : PayrunScriptAttribute
{
    /// <summary>Initializes a new instance of the <see cref="PayrunEndScriptAttribute"/> class</summary>
    /// <param name="payrunName">Name of the payrun</param>
    public PayrunEndScriptAttribute(string payrunName) :
        base(payrunName)
    {
    }
}

/// <summary>Attribute for payrun script</summary>
public abstract class PayrunScriptAttribute : ScriptAttribute
{
    /// <summary>The payrun name</summary>
    public string PayrunName { get; }

    /// <summary>Gets the script key</summary>
    public override string ScriptKey => PayrunName;

    /// <summary>Initializes a new instance of the <see cref="PayrunScriptAttribute"/> class</summary>
    /// <param name="payrunName">Name of the collector</param>
    protected PayrunScriptAttribute(string payrunName)
    {
        if (string.IsNullOrWhiteSpace(payrunName))
        {
            throw new ArgumentException(nameof(payrunName));
        }

        PayrunName = payrunName;
    }
}

#endregion

#region Report

/// <summary>Attribute for report build script</summary>
[AttributeUsage(AttributeTargets.Method)]
public sealed class ReportBuildScriptAttribute : ReportScriptAttribute
{
    /// <summary>Initializes a new instance of the <see cref="ReportBuildScriptAttribute"/> class</summary>
    /// <param name="reportName">Name of the report</param>
    public ReportBuildScriptAttribute(string reportName) :
        base(reportName)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="ReportBuildScriptAttribute"/> class</summary>
    /// <param name="reportName">Name of the report</param>
    /// <param name="culture">The report culture</param>
    /// <param name="parameters">The report parameters as JSON</param>
    public ReportBuildScriptAttribute(string reportName, string culture, string parameters = null) :
        base(reportName, culture, parameters)
    {
    }
}

/// <summary>Attribute for report start script</summary>
[AttributeUsage(AttributeTargets.Method)]
public sealed class ReportStartScriptAttribute : ReportScriptAttribute
{
    /// <summary>Initializes a new instance of the <see cref="ReportStartScriptAttribute"/> class</summary>
    /// <param name="reportName">Name of the report</param>
    public ReportStartScriptAttribute(string reportName) :
        base(reportName)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="ReportStartScriptAttribute"/> class</summary>
    /// <param name="reportName">Name of the report</param>
    /// <param name="culture">The report culture</param>
    /// <param name="parameters">The report parameters as JSON</param>
    public ReportStartScriptAttribute(string reportName, string culture, string parameters = null) :
        base(reportName, culture, parameters)
    {
    }
}

/// <summary>Attribute for report end script</summary>
[AttributeUsage(AttributeTargets.Method)]
public sealed class ReportEndScriptAttribute : ReportScriptAttribute
{
    /// <summary>Initializes a new instance of the <see cref="ReportEndScriptAttribute"/> class</summary>
    /// <param name="reportName">Name of the report</param>
    public ReportEndScriptAttribute(string reportName) :
        base(reportName)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="ReportEndScriptAttribute"/> class</summary>
    /// <param name="reportName">Name of the report</param>
    /// <param name="culture">The report culture</param>
    /// <param name="parameters">The Report parameters as JSON</param>
    public ReportEndScriptAttribute(string reportName, string culture, string parameters = null) :
        base(reportName, culture, parameters)
    {
    }
}

/// <summary>Attribute for report script</summary>
public abstract class ReportScriptAttribute : ScriptAttribute
{
    /// <summary>The report name</summary>
    public string ReportName { get; }

    /// <summary>The report culture</summary>
    public string Culture { get; }

    /// <summary>The report parameters</summary>
    public Dictionary<string, string> Parameters { get; set; }

    /// <summary>Gets the script key</summary>
    public override string ScriptKey => ReportName;

    /// <summary>Initializes a new instance of the <see cref="ReportScriptAttribute"/> class</summary>
    /// <param name="reportName">Name of the report</param>
    protected ReportScriptAttribute(string reportName)
    {
        if (string.IsNullOrWhiteSpace(reportName))
        {
            throw new ArgumentException(nameof(reportName));
        }
        ReportName = reportName;
    }

    /// <summary>Initializes a new instance of the <see cref="ReportScriptAttribute"/> class</summary>
    /// <param name="reportName">Name of the report</param>
    /// <param name="culture">The report culture</param>
    /// <param name="parameters">The report parameters as JSON</param>
    protected ReportScriptAttribute(string reportName, string culture, string parameters = null) :
        this(reportName)
    {
        Culture = culture;
        Parameters = parameters != null ? JsonSerializer.Deserialize<Dictionary<string, string>>(parameters) : null;
    }
}

#endregion

/// <summary>Attribute for script</summary>
public abstract class ScriptAttribute : Attribute
{
    /// <summary>Gets the script key</summary>
    public abstract string ScriptKey { get; }
}