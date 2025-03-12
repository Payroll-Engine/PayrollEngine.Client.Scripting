using System.Collections.Generic;

namespace PayrollEngine.Client.Scripting;

/// <summary>Provide the system actions</summary>
public static class SystemActionProvider
{
    // case
    private const string CaseAvailableActionsScript = "CaseAvailableActions.cs";
    private const string CaseBuildActionsScript = "CaseBuildActions.cs";
    private const string CaseInputActionsScript = "CaseInputActions.cs";
    private const string CaseValidateActionsScript = "CaseValidateActions.cs";
    // case relation
    private const string CaseRelationBuildActionsScript = "CaseRelationBuildActions.cs";
    private const string CaseRelationValidateActionsScript = "CaseRelationValidateActions.cs";

    /// <summary>Get the system action scripts</summary>
    /// <param name="functionType">The function type</param>
    public static List<string> GetSystemActionScripts(FunctionType functionType)
    {
        var actionScripts = new List<string>();

        // case
        var caseAvailable = functionType.HasFlag(FunctionType.CaseAvailable);
        var caseBuild = functionType.HasFlag(FunctionType.CaseBuild);
        var caseValidate = functionType.HasFlag(FunctionType.CaseValidate);
        // case available
        if (caseAvailable)
        {
            actionScripts.Add(GetEmbeddedScript(CaseAvailableActionsScript));
        }
        // case build and case validate
        if (caseBuild || caseValidate)
        {
            actionScripts.Add(GetEmbeddedScript(CaseBuildActionsScript));
            actionScripts.Add(GetEmbeddedScript(CaseInputActionsScript));
            actionScripts.Add(GetEmbeddedScript(CaseValidateActionsScript));
        }

        // case relation
        var caseRelationBuild = functionType.HasFlag(FunctionType.CaseRelationBuild);
        var caseRelationValidate = functionType.HasFlag(FunctionType.CaseRelationValidate);
        // case relation build and case relation validate
        if (caseRelationBuild || caseRelationValidate)
        {
            actionScripts.Add(GetEmbeddedScript(CaseRelationBuildActionsScript));
            actionScripts.Add(GetEmbeddedScript(CaseRelationValidateActionsScript));
        }

        return actionScripts;
    }

    private static string GetEmbeddedScript(string name)
    {
        var resource = $"Function\\{name}";
        return typeof(SystemActionProvider).Assembly.GetEmbeddedFile(resource);
    }
}