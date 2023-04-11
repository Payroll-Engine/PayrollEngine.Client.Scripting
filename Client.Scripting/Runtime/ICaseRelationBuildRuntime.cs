﻿
namespace PayrollEngine.Client.Scripting.Runtime;

/// <summary>Runtime for the case relation build function <see cref="Function.CaseRelationBuildFunction"/></summary>
public interface ICaseRelationBuildRuntime : ICaseRelationRuntime
{
    /// <summary>Get case relation build actions</summary>
    string[] GetBuildActions();
}