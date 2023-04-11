using PayrollEngine.Client.Script;

namespace PayrollEngine.Client.Scripting.Script;

/// <summary>
/// Payroll script parser
/// </summary>
public class ScriptParser : IScriptParser
{
    /// <summary>The case script parser</summary>
    public ICaseScriptParser CaseParser { get; } = new CaseScriptParser();

    /// <summary>The case relation script parser</summary>
    public ICaseRelationScriptParser CaseRelationParser { get; } = new CaseRelationScriptParser();

    /// <summary>The case relation script parser</summary>
    public IWageTypeScriptParser WageTypeParser { get; } = new WageTypeScriptParser();

    /// <summary>The case relation script parser</summary>
    public ICollectorScriptParser CollectorParser { get; } = new CollectorScriptParser();

    /// <summary>The case relation script parser</summary>
    public IPayrunScriptParser PayrunParser { get; } = new PayrunScriptParser();

    /// <summary>The case relation script parser</summary>
    public IReportScriptParser ReportParser { get; } = new ReportScriptParser();
}