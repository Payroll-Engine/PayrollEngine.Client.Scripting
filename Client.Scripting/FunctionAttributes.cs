using System;

namespace PayrollEngine.Client.Scripting;

#region Case

/// <summary>Attribute for case validate function</summary>
[AttributeUsage(AttributeTargets.Class)]
public sealed class CaseValidateFunctionAttribute : CaseChangeFunctionAttribute
{
    /// <summary>Initializes a new instance of the <see cref="CaseValidateFunctionAttribute"/> class</summary>
    /// <param name="tenantIdentifier">The tenant identifier</param>
    /// <param name="userIdentifier">The user identifier</param>
    /// <param name="employeeIdentifier">The employee identifier</param>
    /// <param name="payrollName">Name of the payroll</param>
    /// <param name="regulationName">Name of the regulation</param>
    public CaseValidateFunctionAttribute(string tenantIdentifier, string userIdentifier,
        string employeeIdentifier, string payrollName, string regulationName) :
        base(tenantIdentifier, userIdentifier, employeeIdentifier, payrollName, regulationName)
    {
    }
}

/// <summary>Attribute for case build function</summary>
[AttributeUsage(AttributeTargets.Class)]
public sealed class CaseBuildFunctionAttribute : CaseChangeFunctionAttribute
{
    /// <summary>Initializes a new instance of the <see cref="CaseBuildFunctionAttribute"/> class</summary>
    /// <param name="tenantIdentifier">The tenant identifier</param>
    /// <param name="userIdentifier">The user identifier</param>
    /// <param name="employeeIdentifier">The employee identifier</param>
    /// <param name="payrollName">Name of the payroll</param>
    /// <param name="regulationName">Name of the regulation</param>
    public CaseBuildFunctionAttribute(string tenantIdentifier, string userIdentifier,
        string employeeIdentifier, string payrollName, string regulationName) :
        base(tenantIdentifier, userIdentifier, employeeIdentifier, payrollName, regulationName)
    {
    }
}

/// <summary>Attribute for case change function</summary>
public abstract class CaseChangeFunctionAttribute : CaseFunctionAttribute
{
    /// <summary>Initializes a new instance of the <see cref="CaseChangeFunctionAttribute"/> class</summary>
    /// <param name="tenantIdentifier">The tenant identifier</param>
    /// <param name="userIdentifier">The user identifier</param>
    /// <param name="employeeIdentifier">The employee identifier</param>
    /// <param name="payrollName">Name of the payroll</param>
    /// <param name="regulationName">Name of the regulation</param>
    protected CaseChangeFunctionAttribute(string tenantIdentifier, string userIdentifier,
        string employeeIdentifier, string payrollName, string regulationName) :
        base(tenantIdentifier, userIdentifier, employeeIdentifier, payrollName, regulationName)
    {
    }
}

/// <summary>Attribute for case available function</summary>
[AttributeUsage(AttributeTargets.Class)]
public sealed class CaseAvailableFunctionAttribute : CaseFunctionAttribute
{
    /// <summary>Initializes a new instance of the <see cref="CaseAvailableFunctionAttribute"/> class</summary>
    /// <param name="tenantIdentifier">The tenant identifier</param>
    /// <param name="userIdentifier">The user identifier</param>
    /// <param name="employeeIdentifier">The employee identifier</param>
    /// <param name="payrollName">Name of the payroll</param>
    /// <param name="regulationName">Name of the regulation</param>
    public CaseAvailableFunctionAttribute(string tenantIdentifier, string userIdentifier,
        string employeeIdentifier, string payrollName, string regulationName) :
        base(tenantIdentifier, userIdentifier, employeeIdentifier, payrollName, regulationName)
    {
    }
}

/// <summary>Attribute for case function</summary>
public abstract class CaseFunctionAttribute : RegulationFunctionAttribute
{
    /// <summary>Initializes a new instance of the <see cref="CaseFunctionAttribute"/> class</summary>
    /// <param name="tenantIdentifier">The tenant identifier</param>
    /// <param name="userIdentifier">The user identifier</param>
    /// <param name="employeeIdentifier">The employee identifier</param>
    /// <param name="payrollName">Name of the payroll</param>
    /// <param name="regulationName">Name of the regulation</param>
    protected CaseFunctionAttribute(string tenantIdentifier, string userIdentifier,
        string employeeIdentifier, string payrollName, string regulationName) :
        base(tenantIdentifier, userIdentifier, employeeIdentifier, payrollName, regulationName)
    {
    }
}

#endregion

#region Case Relation

/// <summary>Attribute for case relation validate function</summary>
[AttributeUsage(AttributeTargets.Class)]
public sealed class CaseRelationValidateFunctionAttribute : CaseRelationFunctionAttribute
{
    /// <summary>Initializes a new instance of the <see cref="CaseRelationValidateFunctionAttribute"/> class</summary>
    /// <param name="tenantIdentifier">The tenant identifier</param>
    /// <param name="userIdentifier">The user identifier</param>
    /// <param name="employeeIdentifier">The employee identifier</param>
    /// <param name="payrollName">Name of the payroll</param>
    /// <param name="regulationName">Name of the regulation</param>
    public CaseRelationValidateFunctionAttribute(string tenantIdentifier, string userIdentifier,
        string employeeIdentifier, string payrollName, string regulationName) :
        base(tenantIdentifier, userIdentifier, employeeIdentifier, payrollName, regulationName)
    {
    }
}

/// <summary>Attribute for case relation build function</summary>
[AttributeUsage(AttributeTargets.Class)]
public sealed class CaseRelationBuildFunctionAttribute : CaseRelationFunctionAttribute
{
    /// <summary>Initializes a new instance of the <see cref="CaseRelationBuildFunctionAttribute"/> class</summary>
    /// <param name="tenantIdentifier">The tenant identifier</param>
    /// <param name="userIdentifier">The user identifier</param>
    /// <param name="employeeIdentifier">The employee identifier</param>
    /// <param name="payrollName">Name of the payroll</param>
    /// <param name="regulationName">Name of the regulation</param>
    public CaseRelationBuildFunctionAttribute(string tenantIdentifier, string userIdentifier,
        string employeeIdentifier, string payrollName, string regulationName) :
        base(tenantIdentifier, userIdentifier, employeeIdentifier, payrollName, regulationName)
    {
    }
}

/// <summary>Attribute for case relation function</summary>
public abstract class CaseRelationFunctionAttribute : RegulationFunctionAttribute
{
    /// <summary>Initializes a new instance of the <see cref="CaseRelationFunctionAttribute"/> class</summary>
    /// <param name="tenantIdentifier">The tenant identifier</param>
    /// <param name="userIdentifier">The user identifier</param>
    /// <param name="employeeIdentifier">The employee identifier</param>
    /// <param name="payrollName">Name of the payroll</param>
    /// <param name="regulationName">Name of the regulation</param>
    protected CaseRelationFunctionAttribute(string tenantIdentifier, string userIdentifier,
        string employeeIdentifier, string payrollName, string regulationName) :
        base(tenantIdentifier, userIdentifier, employeeIdentifier, payrollName, regulationName)
    {
    }
}

#endregion

#region Wage Type

/// <summary>Attribute for wage type result function</summary>
[AttributeUsage(AttributeTargets.Class)]
public sealed class WageTypeResultFunctionAttribute : WageTypeFunctionAttribute
{
    /// <summary>Initializes a new instance of the <see cref="WageTypeResultFunctionAttribute"/> class</summary>
    /// <param name="tenantIdentifier">The tenant identifier</param>
    /// <param name="userIdentifier">The user identifier</param>
    /// <param name="employeeIdentifier">The employee identifier</param>
    /// <param name="payrollName">Name of the payroll</param>
    /// <param name="regulationName">Name of the regulation</param>
    public WageTypeResultFunctionAttribute(string tenantIdentifier, string userIdentifier,
        string employeeIdentifier, string payrollName, string regulationName) :
        base(tenantIdentifier, userIdentifier, employeeIdentifier, payrollName, regulationName)
    {
    }
}

/// <summary>Attribute for wage type value function</summary>
[AttributeUsage(AttributeTargets.Class)]
public sealed class WageTypeValueFunctionAttribute : WageTypeFunctionAttribute
{
    /// <summary>Initializes a new instance of the <see cref="WageTypeValueFunctionAttribute"/> class</summary>
    /// <param name="tenantIdentifier">The tenant identifier</param>
    /// <param name="userIdentifier">The user identifier</param>
    /// <param name="employeeIdentifier">The employee identifier</param>
    /// <param name="payrollName">Name of the payroll</param>
    /// <param name="regulationName">Name of the regulation</param>
    public WageTypeValueFunctionAttribute(string tenantIdentifier, string userIdentifier,
        string employeeIdentifier, string payrollName, string regulationName) :
        base(tenantIdentifier, userIdentifier, employeeIdentifier, payrollName, regulationName)
    {
    }
}

/// <summary>Attribute for wage type function</summary>
public abstract class WageTypeFunctionAttribute : RegulationFunctionAttribute
{
    /// <summary>Initializes a new instance of the <see cref="WageTypeFunctionAttribute"/> class</summary>
    /// <param name="tenantIdentifier">The tenant identifier</param>
    /// <param name="userIdentifier">The user identifier</param>
    /// <param name="employeeIdentifier">The employee identifier</param>
    /// <param name="payrollName">Name of the payroll</param>
    /// <param name="regulationName">Name of the regulation</param>
    protected WageTypeFunctionAttribute(string tenantIdentifier, string userIdentifier,
        string employeeIdentifier, string payrollName, string regulationName) :
        base(tenantIdentifier, userIdentifier, employeeIdentifier, payrollName, regulationName)
    {
    }
}

#endregion

#region Collector

/// <summary>Attribute for collector edn function</summary>
[AttributeUsage(AttributeTargets.Class)]
public sealed class CollectorEndFunctionAttribute : CollectorFunctionAttribute
{
    /// <summary>Initializes a new instance of the <see cref="CollectorEndFunctionAttribute"/> class</summary>
    /// <param name="tenantIdentifier">The tenant identifier</param>
    /// <param name="userIdentifier">The user identifier</param>
    /// <param name="employeeIdentifier">The employee identifier</param>
    /// <param name="payrollName">Name of the payroll</param>
    /// <param name="regulationName">Name of the regulation</param>
    public CollectorEndFunctionAttribute(string tenantIdentifier, string userIdentifier,
        string employeeIdentifier, string payrollName, string regulationName) :
        base(tenantIdentifier, userIdentifier, employeeIdentifier, payrollName, regulationName)
    {
    }
}

/// <summary>Attribute for collector apply tart function</summary>
[AttributeUsage(AttributeTargets.Class)]
public sealed class CollectorApplyFunctionAttribute : CollectorFunctionAttribute
{
    /// <summary>Initializes a new instance of the <see cref="CollectorApplyFunctionAttribute"/> class</summary>
    /// <param name="tenantIdentifier">The tenant identifier</param>
    /// <param name="userIdentifier">The user identifier</param>
    /// <param name="employeeIdentifier">The employee identifier</param>
    /// <param name="payrollName">Name of the payroll</param>
    /// <param name="regulationName">Name of the regulation</param>
    public CollectorApplyFunctionAttribute(string tenantIdentifier, string userIdentifier,
        string employeeIdentifier, string payrollName, string regulationName) :
        base(tenantIdentifier, userIdentifier, employeeIdentifier, payrollName, regulationName)
    {
    }
}

/// <summary>Attribute for collector start function</summary>
[AttributeUsage(AttributeTargets.Class)]
public sealed class CollectorStartFunctionAttribute : CollectorFunctionAttribute
{
    /// <summary>Initializes a new instance of the <see cref="CollectorStartFunctionAttribute"/> class</summary>
    /// <param name="tenantIdentifier">The tenant identifier</param>
    /// <param name="userIdentifier">The user identifier</param>
    /// <param name="employeeIdentifier">The employee identifier</param>
    /// <param name="payrollName">Name of the payroll</param>
    /// <param name="regulationName">Name of the regulation</param>
    public CollectorStartFunctionAttribute(string tenantIdentifier, string userIdentifier,
        string employeeIdentifier, string payrollName, string regulationName) :
        base(tenantIdentifier, userIdentifier, employeeIdentifier, payrollName, regulationName)
    {
    }
}

/// <summary>Attribute for collector function</summary>
public abstract class CollectorFunctionAttribute : RegulationFunctionAttribute
{
    /// <summary>Initializes a new instance of the <see cref="CollectorFunctionAttribute"/> class</summary>
    /// <param name="tenantIdentifier">The tenant identifier</param>
    /// <param name="userIdentifier">The user identifier</param>
    /// <param name="employeeIdentifier">The employee identifier</param>
    /// <param name="payrollName">Name of the payroll</param>
    /// <param name="regulationName">Name of the regulation</param>
    protected CollectorFunctionAttribute(string tenantIdentifier, string userIdentifier,
        string employeeIdentifier, string payrollName, string regulationName) :
        base(tenantIdentifier, userIdentifier, employeeIdentifier, payrollName, regulationName)
    {
    }
}

#endregion

#region Report

/// <summary>Attribute for report build function</summary>
[AttributeUsage(AttributeTargets.Class)]
public sealed class ReportBuildFunctionAttribute : ReportFunctionAttribute
{
    /// <summary>Initializes a new instance of the <see cref="ReportBuildFunctionAttribute"/> class</summary>
    /// <param name="tenantIdentifier">The tenant identifier</param>
    /// <param name="userIdentifier">The user identifier</param>
    /// <param name="regulationName">Name of the regulation</param>
    public ReportBuildFunctionAttribute(string tenantIdentifier, string userIdentifier, string regulationName) :
        base(tenantIdentifier, userIdentifier, regulationName)
    {
    }
}

/// <summary>Attribute for report start function</summary>
[AttributeUsage(AttributeTargets.Class)]
public sealed class ReportStartFunctionAttribute : ReportFunctionAttribute
{
    /// <summary>Initializes a new instance of the <see cref="ReportStartFunctionAttribute"/> class</summary>
    /// <param name="tenantIdentifier">The tenant identifier</param>
    /// <param name="userIdentifier">The user identifier</param>
    /// <param name="regulationName">Name of the regulation</param>
    public ReportStartFunctionAttribute(string tenantIdentifier, string userIdentifier, string regulationName) :
        base(tenantIdentifier, userIdentifier, regulationName)
    {
    }
}

/// <summary>Attribute for report end function</summary>
[AttributeUsage(AttributeTargets.Class)]
public sealed class ReportEndFunctionAttribute : ReportFunctionAttribute
{
    /// <summary>Initializes a new instance of the <see cref="ReportEndFunctionAttribute"/> class</summary>
    /// <param name="tenantIdentifier">The tenant identifier</param>
    /// <param name="userIdentifier">The user identifier</param>
    /// <param name="regulationName">Name of the regulation</param>
    public ReportEndFunctionAttribute(string tenantIdentifier, string userIdentifier, string regulationName) :
        base(tenantIdentifier, userIdentifier, regulationName)
    {
    }
}

/// <summary>Attribute for case function</summary>
public abstract class ReportFunctionAttribute : FunctionAttribute
{
    /// <summary>The regulation name</summary>
    public string RegulationName { get; }

    /// <summary>Initializes a new instance of the <see cref="CaseFunctionAttribute"/> class</summary>
    /// <param name="tenantIdentifier">The tenant identifier</param>
    /// <param name="userIdentifier">The user identifier</param>
    /// <param name="regulationName">Name of the regulation</param>
    protected ReportFunctionAttribute(string tenantIdentifier, string userIdentifier, string regulationName) :
        base(tenantIdentifier, userIdentifier)
    {
        if (string.IsNullOrWhiteSpace(regulationName))
        {
            throw new ArgumentException(nameof(regulationName));
        }

        RegulationName = regulationName;
    }
}

#endregion

#region Regulation

/// <summary>Attribute for regulation function</summary>
public abstract class RegulationFunctionAttribute : PayrollAttribute
{
    /// <summary>The regulation name</summary>
    public string RegulationName { get; }

    /// <summary>Initializes a new instance of the <see cref="RegulationFunctionAttribute"/> class</summary>
    /// <param name="tenantIdentifier">The tenant identifier</param>
    /// <param name="userIdentifier">The user identifier</param>
    /// <param name="employeeIdentifier">The employee identifier</param>
    /// <param name="payrollName">Name of the payroll</param>
    /// <param name="regulationName">Name of the regulation</param>
    protected RegulationFunctionAttribute(string tenantIdentifier, string userIdentifier,
        string employeeIdentifier, string payrollName, string regulationName) :
        base(tenantIdentifier, userIdentifier, employeeIdentifier, payrollName)
    {
        if (string.IsNullOrWhiteSpace(regulationName))
        {
            throw new ArgumentException(nameof(regulationName));
        }

        RegulationName = regulationName;
    }
}

#endregion

#region Payrun

/// <summary>Attribute for payrun start function</summary>
[AttributeUsage(AttributeTargets.Class)]
public sealed class PayrunStartFunctionAttribute : PayrunFunctionAttribute
{
    /// <summary>Initializes a new instance of the <see cref="PayrunStartFunctionAttribute"/> class</summary>
    /// <param name="tenantIdentifier">The tenant identifier</param>
    /// <param name="userIdentifier">The user identifier</param>
    /// <param name="employeeIdentifier">The employee identifier</param>
    /// <param name="payrollName">Name of the payroll</param>
    /// <param name="payrunName">Name of the payrun</param>
    public PayrunStartFunctionAttribute(string tenantIdentifier, string userIdentifier,
        string employeeIdentifier, string payrollName, string payrunName) :
        base(tenantIdentifier, userIdentifier, employeeIdentifier, payrollName, payrunName)
    {
    }
}

/// <summary>Attribute for payrun wage type available function</summary>
[AttributeUsage(AttributeTargets.Class)]
public sealed class PayrunWageTypeAvailableFunctionAttribute : PayrunFunctionAttribute
{
    /// <summary>Initializes a new instance of the <see cref="PayrunWageTypeAvailableFunctionAttribute"/> class</summary>
    /// <param name="tenantIdentifier">The tenant identifier</param>
    /// <param name="userIdentifier">The user identifier</param>
    /// <param name="employeeIdentifier">The employee identifier</param>
    /// <param name="payrollName">Name of the payroll</param>
    /// <param name="payrunName">Name of the payrun</param>
    public PayrunWageTypeAvailableFunctionAttribute(string tenantIdentifier, string userIdentifier,
        string employeeIdentifier, string payrollName, string payrunName) :
        base(tenantIdentifier, userIdentifier, employeeIdentifier, payrollName, payrunName)
    {
    }
}

/// <summary>Attribute for payrun employee available function</summary>
[AttributeUsage(AttributeTargets.Class)]
public sealed class PayrunEmployeeAvailableFunctionAttribute : PayrunFunctionAttribute
{
    /// <summary>Initializes a new instance of the <see cref="PayrunEmployeeAvailableFunctionAttribute"/> class</summary>
    /// <param name="tenantIdentifier">The tenant identifier</param>
    /// <param name="userIdentifier">The user identifier</param>
    /// <param name="employeeIdentifier">The employee identifier</param>
    /// <param name="payrollName">Name of the payroll</param>
    /// <param name="payrunName">Name of the payrun</param>
    public PayrunEmployeeAvailableFunctionAttribute(string tenantIdentifier, string userIdentifier,
        string employeeIdentifier, string payrollName, string payrunName) :
        base(tenantIdentifier, userIdentifier, employeeIdentifier, payrollName, payrunName)
    {
    }
}

/// <summary>Attribute for payrun employee start function</summary>
[AttributeUsage(AttributeTargets.Class)]
public sealed class PayrunEmployeeStartFunctionAttribute : PayrunFunctionAttribute
{
    /// <summary>Initializes a new instance of the <see cref="PayrunEmployeeStartFunctionAttribute"/> class</summary>
    /// <param name="tenantIdentifier">The tenant identifier</param>
    /// <param name="userIdentifier">The user identifier</param>
    /// <param name="employeeIdentifier">The employee identifier</param>
    /// <param name="payrollName">Name of the payroll</param>
    /// <param name="payrunName">Name of the payrun</param>
    public PayrunEmployeeStartFunctionAttribute(string tenantIdentifier, string userIdentifier,
        string employeeIdentifier, string payrollName, string payrunName) :
        base(tenantIdentifier, userIdentifier, employeeIdentifier, payrollName, payrunName)
    {
    }
}

/// <summary>Attribute for payrun employee end function</summary>
[AttributeUsage(AttributeTargets.Class)]
public sealed class PayrunEmployeeEndFunctionAttribute : PayrunFunctionAttribute
{
    /// <summary>Initializes a new instance of the <see cref="PayrunEmployeeEndFunctionAttribute"/> class</summary>
    /// <param name="tenantIdentifier">The tenant identifier</param>
    /// <param name="userIdentifier">The user identifier</param>
    /// <param name="employeeIdentifier">The employee identifier</param>
    /// <param name="payrollName">Name of the payroll</param>
    /// <param name="payrunName">Name of the payrun</param>
    public PayrunEmployeeEndFunctionAttribute(string tenantIdentifier, string userIdentifier,
        string employeeIdentifier, string payrollName, string payrunName) :
        base(tenantIdentifier, userIdentifier, employeeIdentifier, payrollName, payrunName)
    {
    }
}

/// <summary>Attribute for payrun end function</summary>
[AttributeUsage(AttributeTargets.Class)]
public sealed class PayrunEndFunctionAttribute : PayrunFunctionAttribute
{
    /// <summary>Initializes a new instance of the <see cref="PayrunEndFunctionAttribute"/> class</summary>
    /// <param name="tenantIdentifier">The tenant identifier</param>
    /// <param name="userIdentifier">The user identifier</param>
    /// <param name="employeeIdentifier">The employee identifier</param>
    /// <param name="payrollName">Name of the payroll</param>
    /// <param name="payrunName">Name of the payrun</param>
    public PayrunEndFunctionAttribute(string tenantIdentifier, string userIdentifier,
        string employeeIdentifier, string payrollName, string payrunName) :
        base(tenantIdentifier, userIdentifier, employeeIdentifier, payrollName, payrunName)
    {
    }
}

/// <summary>Attribute for payrun function</summary>
public abstract class PayrunFunctionAttribute : PayrollAttribute
{
    /// <summary>The regulation name</summary>
    public string PayrunName { get; }

    /// <summary>Initializes a new instance of the <see cref="PayrunFunctionAttribute"/> class</summary>
    /// <param name="tenantIdentifier">The tenant identifier</param>
    /// <param name="userIdentifier">The user identifier</param>
    /// <param name="employeeIdentifier">The employee identifier</param>
    /// <param name="payrollName">Name of the payroll</param>
    /// <param name="payrunName">Name of the payrun</param>
    protected PayrunFunctionAttribute(string tenantIdentifier, string userIdentifier,
        string employeeIdentifier, string payrollName, string payrunName) :
        base(tenantIdentifier, userIdentifier, employeeIdentifier, payrollName)
    {
        if (string.IsNullOrWhiteSpace(payrunName))
        {
            throw new ArgumentException(nameof(payrunName));
        }

        PayrunName = payrunName;
    }
}

#endregion

/// <summary>Attribute for payroll function</summary>
public abstract class PayrollAttribute : FunctionAttribute
{
    /// <summary>Rhe employee identifier</summary>
    public string EmployeeIdentifier { get; }

    /// <summary>The name of the payroll</summary>
    public string PayrollName { get; }

    /// <summary>Initializes a new instance of the <see cref="PayrollAttribute"/> class</summary>
    /// <param name="tenantIdentifier">The tenant identifier</param>
    /// <param name="userIdentifier">The user identifier</param>
    /// <param name="employeeIdentifier">The employee identifier</param>
    /// <param name="payrollName">Name of the payroll</param>
    protected PayrollAttribute(string tenantIdentifier, string userIdentifier, string employeeIdentifier, string payrollName) :
        base(tenantIdentifier, userIdentifier)
    {
        if (string.IsNullOrWhiteSpace(employeeIdentifier))
        {
            throw new ArgumentException(nameof(employeeIdentifier));
        }
        if (string.IsNullOrWhiteSpace(payrollName))
        {
            throw new ArgumentException(nameof(payrollName));
        }

        EmployeeIdentifier = employeeIdentifier;
        PayrollName = payrollName;
    }
}

/// <summary>Attribute for function</summary>
public abstract class FunctionAttribute : Attribute
{
    /// <summary>The tenant identifier</summary>
    public string TenantIdentifier { get; }

    /// <summary>The user identifier</summary>
    public string UserIdentifier { get; }

    /// <summary>Initializes a new instance of the <see cref="PayrollAttribute"/> class</summary>
    /// <param name="tenantIdentifier">The tenant identifier</param>
    /// <param name="userIdentifier">The user identifier</param>
    protected FunctionAttribute(string tenantIdentifier, string userIdentifier)
    {
        if (string.IsNullOrWhiteSpace(tenantIdentifier))
        {
            throw new ArgumentException(nameof(tenantIdentifier));
        }
        if (string.IsNullOrWhiteSpace(userIdentifier))
        {
            throw new ArgumentException(nameof(userIdentifier));
        }

        TenantIdentifier = tenantIdentifier;
        UserIdentifier = userIdentifier;
    }
}