using System.Linq;
using System.Collections.Generic;
using PayrollEngine.Client.Scripting.Function;

namespace PayrollEngine.Client.Scripting;

/// <summary>Access to scripts and script names</summary>
public static class ScriptProvider
{

    /// <summary>
    /// The embedded c# scripting files
    /// </summary>
    /// <remarks>Keep this in sync with the PayrollEngine.Client.Scripting project</remarks>
    public static List<string> GetEmbeddedScriptNames() =>
    [
        "ClientScript.cs",
        "Extensions.cs",
        "Date.cs",
        "DatePeriod.cs",
        "HourPeriod.cs",
        "Tools.cs",
        "PayrollValue.cs",
        "PeriodValue.cs",
        "CaseObject.cs",
        "CaseValue.cs",
        "CasePayrollValue.cs",
        $"{nameof(Function)}\\{nameof(Function)}.cs",
        $"{nameof(Function)}\\{nameof(PayrollFunction)}.cs",
        "PayrollResults.cs"
    ];

    /// <summary>Get function script names by function type</summary>
    /// <param name="functionType">The function type</param>
    /// <param name="actionFunctionType">The action function type</param>
    public static List<string> GetScriptNames(FunctionType functionType, FunctionType actionFunctionType)
    {
        if (functionType == default)
        {
            return [];
        }
        var names = GetEmbeddedScriptNames(functionType);
        if (names.Any())
        {
            names.AddRange(GetActionScriptNames(actionFunctionType));
        }
        return names;
    }

    #region Embedded Scripts

    // basic
    private const string CacheScript = "Cache.cs";
    private const string PayrunFunctionScript = $"{nameof(Function)}\\{nameof(PayrunFunction)}.cs";

    // case
    private const string CaseFunctionScript = $"{nameof(Function)}\\{nameof(CaseFunction)}.cs";
    private const string CaseChangeFunctionScript = $"{nameof(Function)}\\{nameof(CaseChangeFunction)}.cs";
    private const string CaseAvailableFunctionScript = $"{nameof(Function)}\\{nameof(CaseAvailableFunction)}.cs";
    private const string CaseBuildFunctionScript = $"{nameof(Function)}\\{nameof(CaseBuildFunction)}.cs";
    private const string CaseValidateFunctionScript = $"{nameof(Function)}\\{nameof(CaseValidateFunction)}.cs";
    // case relation
    private const string CaseRelationFunctionScript = $"{nameof(Function)}\\{nameof(CaseRelationFunction)}.cs";
    private const string CaseRelationBuildFunctionScript = $"{nameof(Function)}\\{nameof(CaseRelationBuildFunction)}.cs";
    private const string CaseRelationValidateFunctionScript = $"{nameof(Function)}\\{nameof(CaseRelationValidateFunction)}.cs";
    // collector
    private const string CollectorFunctionScript = $"{nameof(Function)}\\{nameof(CollectorFunction)}.cs";
    private const string CollectorStartFunctionScript = $"{nameof(Function)}\\{nameof(CollectorStartFunction)}.cs";
    private const string CollectorApplyFunctionScript = $"{nameof(Function)}\\{nameof(CollectorApplyFunction)}.cs";
    private const string CollectorEndFunctionScript = $"{nameof(Function)}\\{nameof(CollectorEndFunction)}.cs";
    // wage type
    private const string WageTypeFunctionScript = $"{nameof(Function)}\\{nameof(WageTypeFunction)}.cs";
    private const string WageTypeValueFunctionScript = $"{nameof(Function)}\\{nameof(WageTypeValueFunction)}.cs";
    private const string WageTypeResultFunctionScript = $"{nameof(Function)}\\{nameof(WageTypeResultFunction)}.cs";

    /// <summary>Get function script names by function type</summary>
    /// <param name="functionType">The function type</param>
    public static List<string> GetEmbeddedScriptNames(FunctionType functionType)
    {
        var actionScripts = new List<string>();

        // --- case ---
        var caseAvailable = functionType.HasFlag(FunctionType.CaseAvailable);
        var caseBuild = functionType.HasFlag(FunctionType.CaseBuild);
        var caseValidate = functionType.HasFlag(FunctionType.CaseValidate);
        // all case functions
        if (caseAvailable || caseBuild || caseValidate)
        {
            actionScripts.Add(CaseFunctionScript);
        }
        // case available
        if (caseAvailable)
        {
            actionScripts.Add(CaseAvailableFunctionScript);
        }
        // case change
        if (caseBuild || caseValidate)
        {
            actionScripts.Add(CaseChangeFunctionScript);
        }
        // case build
        if (caseBuild)
        {
            actionScripts.Add(CaseBuildFunctionScript);
        }
        // case validate
        if (caseValidate)
        {
            actionScripts.Add(CaseValidateFunctionScript);
        }

        // --- case relation ---
        var caseRelationBuild = functionType.HasFlag(FunctionType.CaseRelationBuild);
        var caseRelationValidate = functionType.HasFlag(FunctionType.CaseRelationValidate);
        // all case relation functions
        if (caseRelationBuild || caseRelationValidate)
        {
            actionScripts.Add(CaseFunctionScript);
            actionScripts.Add(CaseRelationFunctionScript);
        }
        // case relation build
        if (caseRelationBuild)
        {
            actionScripts.Add(CaseRelationBuildFunctionScript);
        }
        // case relation validate
        if (caseRelationValidate)
        {
            actionScripts.Add(CaseRelationValidateFunctionScript);
        }

        // --- payrun ---
        var payrunBase = functionType.HasFlag(FunctionType.PayrunBase);
        // all payrun base functions
        if (payrunBase)
        {
            actionScripts.Add(CacheScript);
            actionScripts.Add(PayrunFunctionScript);
        }

        // --- collector ---
        var collectorStart = functionType.HasFlag(FunctionType.CollectorStart);
        var collectorApply = functionType.HasFlag(FunctionType.CollectorApply);
        var collectorEnd = functionType.HasFlag(FunctionType.CollectorEnd);
        // all collector functions
        if (collectorStart || collectorApply || collectorEnd)
        {
            actionScripts.Add(CacheScript);
            actionScripts.Add(PayrunFunctionScript);
            actionScripts.Add(CollectorFunctionScript);
        }
        // collector start
        if (collectorStart)
        {
            actionScripts.Add(CollectorStartFunctionScript);
        }
        // collector apply
        if (collectorApply)
        {
            actionScripts.Add(CollectorApplyFunctionScript);
        }
        // collector end
        if (collectorEnd)
        {
            actionScripts.Add(CollectorEndFunctionScript);
        }

        // --- wage type ---
        var wageTypeValue = functionType.HasFlag(FunctionType.WageTypeValue);
        var wageTypeResult = functionType.HasFlag(FunctionType.WageTypeResult);
        // all wage type functions
        if (wageTypeValue || wageTypeResult)
        {
            actionScripts.Add(CacheScript);
            actionScripts.Add(PayrunFunctionScript);
            actionScripts.Add(WageTypeFunctionScript);
        }
        // wage type value
        if (wageTypeValue)
        {
            actionScripts.Add(WageTypeValueFunctionScript);
        }
        // wage type result
        if (wageTypeResult)
        {
            actionScripts.Add(WageTypeResultFunctionScript);
        }

        return actionScripts;
    }

    #endregion

    #region Action Scripts

    // basic actions
    private const string ActionScript = $"{nameof(Function)}\\{nameof(Action)}.cs";
    private const string PayrollActionScript = $"{nameof(Function)}\\{nameof(PayrollFunction)}.{nameof(Action)}.cs";
    private const string PayrunActionScript = $"{nameof(Function)}\\{nameof(PayrunFunction)}.{nameof(Action)}.cs";
    // case
    private const string CaseActionScript = $"{nameof(Function)}\\{nameof(CaseFunction)}.{nameof(Action)}.cs";
    private const string CaseChangeActionScript = $"{nameof(Function)}\\{nameof(CaseChangeFunction)}.{nameof(Action)}.cs";
    private const string CaseAvailableActionScript = $"{nameof(Function)}\\{nameof(CaseAvailableFunction)}.{nameof(Action)}.cs";
    private const string CaseBuildActionScript = $"{nameof(Function)}\\{nameof(CaseBuildFunction)}.{nameof(Action)}.cs";
    private const string CaseValidateActionScript = $"{nameof(Function)}\\{nameof(CaseValidateFunction)}.{nameof(Action)}.cs";
    // case relation
    private const string CaseRelationActionScript = $"{nameof(Function)}\\{nameof(CaseRelationFunction)}.{nameof(Action)}.cs";
    private const string CaseRelationBuildActionScript = $"{nameof(Function)}\\{nameof(CaseRelationBuildFunction)}.{nameof(Action)}.cs";
    private const string CaseRelationValidateActionScript = $"{nameof(Function)}\\{nameof(CaseRelationValidateFunction)}.{nameof(Action)}.cs";
    // collector
    private const string CollectorActionScript = $"{nameof(Function)}\\{nameof(CollectorFunction)}.{nameof(Action)}.cs";
    private const string CollectorStartActionScript = $"{nameof(Function)}\\{nameof(CollectorStartFunction)}.{nameof(Action)}.cs";
    private const string CollectorApplyActionScript = $"{nameof(Function)}\\{nameof(CollectorApplyFunction)}.{nameof(Action)}.cs";
    private const string CollectorEndActionScript = $"{nameof(Function)}\\{nameof(CollectorEndFunction)}.{nameof(Action)}.cs";
    // wage type
    private const string WageTypeActionScript = $"{nameof(Function)}\\{nameof(WageTypeFunction)}.{nameof(Action)}.cs";
    private const string WageTypeValueActionScript = $"{nameof(Function)}\\{nameof(WageTypeValueFunction)}.{nameof(Action)}.cs";
    private const string WageTypeResultActionScript = $"{nameof(Function)}\\{nameof(WageTypeResultFunction)}.{nameof(Action)}.cs";

    /// <summary>Get action script names by function type</summary>
    /// <param name="functionType">The function type</param>
    public static List<string> GetActionScriptNames(FunctionType functionType)
    {
        var actionScripts = new HashSet<string>();

        // --- case ---
        var caseAvailable = functionType.HasFlag(FunctionType.CaseAvailable);
        var caseBuild = functionType.HasFlag(FunctionType.CaseBuild);
        var caseValidate = functionType.HasFlag(FunctionType.CaseValidate);
        // all case functions
        if (caseAvailable || caseBuild || caseValidate)
        {
            actionScripts.Add(ActionScript);
            actionScripts.Add(PayrollActionScript);
            actionScripts.Add(CaseActionScript);
        }
        // case available
        if (caseAvailable)
        {
            actionScripts.Add(CaseAvailableActionScript);
        }
        // case build
        if (caseBuild)
        {
            actionScripts.Add(CaseChangeActionScript);
            actionScripts.Add(CaseBuildActionScript);
        }
        // case validate
        if (caseValidate)
        {
            actionScripts.Add(CaseChangeActionScript);
            actionScripts.Add(CaseValidateActionScript);
        }

        // --- case relation ---
        var caseRelationBuild = functionType.HasFlag(FunctionType.CaseRelationBuild);
        var caseRelationValidate = functionType.HasFlag(FunctionType.CaseRelationValidate);
        // all case relation functions
        if (caseRelationBuild || caseRelationValidate)
        {
            actionScripts.Add(ActionScript);
            actionScripts.Add(PayrollActionScript);
            actionScripts.Add(CaseActionScript);
            actionScripts.Add(CaseRelationActionScript);
        }
        // case relation build
        if (caseRelationBuild)
        {
            actionScripts.Add(CaseRelationBuildActionScript);
        }
        // case relation validate
        if (caseRelationValidate)
        {
            actionScripts.Add(CaseRelationBuildActionScript);
            actionScripts.Add(CaseRelationValidateActionScript);
        }

        // --- collector ---
        var collectorStart = functionType.HasFlag(FunctionType.CollectorStart);
        var collectorApply = functionType.HasFlag(FunctionType.CollectorApply);
        var collectorEnd = functionType.HasFlag(FunctionType.CollectorEnd);
        // all collector functions
        if (collectorStart || collectorApply || collectorEnd)
        {
            actionScripts.Add(ActionScript);
            actionScripts.Add(PayrollActionScript);
            actionScripts.Add(PayrunActionScript);
            actionScripts.Add(CollectorActionScript);
        }
        // collector start
        if (collectorStart)
        {
            actionScripts.Add(CollectorStartActionScript);
        }
        // collector apply
        if (collectorApply)
        {
            actionScripts.Add(CollectorApplyActionScript);
        }
        // collector end
        if (collectorEnd)
        {
            actionScripts.Add(CollectorEndActionScript);
        }

        // --- wage type ---
        var wageTypeValue = functionType.HasFlag(FunctionType.WageTypeValue);
        var wageTypeResult = functionType.HasFlag(FunctionType.WageTypeResult);
        // all wage type functions
        if (wageTypeValue || wageTypeResult)
        {
            actionScripts.Add(ActionScript);
            actionScripts.Add(PayrollActionScript);
            actionScripts.Add(PayrunActionScript);
            actionScripts.Add(WageTypeActionScript);
        }
        // wage type value
        if (wageTypeValue)
        {
            actionScripts.Add(WageTypeValueActionScript);
        }
        // wage type result
        if (wageTypeResult)
        {
            actionScripts.Add(WageTypeResultActionScript);
        }

        return actionScripts.ToList();
    }

    /// <summary>Get action script codes by function type</summary>
    /// <param name="functionType">The function type</param>
    public static List<string> GetActionScriptCodes(FunctionType functionType)
    {
        var names = GetActionScriptNames(functionType);
        return !names.Any() ? names : names.Select(GetEmbeddedScript).ToList();
    }

    #endregion

    private static string GetEmbeddedScript(string name) =>
        typeof(ScriptProvider).Assembly.GetEmbeddedFile(name);

}