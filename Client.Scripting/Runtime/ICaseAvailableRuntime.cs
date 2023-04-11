﻿
namespace PayrollEngine.Client.Scripting.Runtime;

/// <summary>Runtime for the case available function <see cref="Function.CaseAvailableFunction"/></summary>
public interface ICaseAvailableRuntime : ICaseRuntime
{
    /// <summary>Get case available actions</summary>
    string[] GetAvailableActions();
}