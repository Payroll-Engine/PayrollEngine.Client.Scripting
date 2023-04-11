using System;
using System.Collections.Generic;

namespace PayrollEngine.Client.Scripting.Runtime;

/// <summary>Runtime  during the execution of a scripting function <see cref="Function"/></summary>
public interface IRuntime
{
    #region Tenant

    /// <summary>The tenant id</summary>
    int TenantId { get; }

    /// <summary>The tenant identifier</summary>
    string TenantIdentifier { get; }

    /// <summary>Get tenant attribute value</summary>
    /// <param name="attributeName">Name of the attribute</param>
    /// <returns>The tenant attribute value</returns>
    object GetTenantAttribute(string attributeName);

    #endregion

    #region User

    /// <summary>The user id</summary>
    int UserId { get; }

    /// <summary>The user language</summary>
    int UserLanguage { get; }

    /// <summary>The user identifier</summary>
    string UserIdentifier { get; }

    /// <summary>Get user attribute value</summary>
    /// <param name="attributeName">Name of the attribute</param>
    /// <returns>The user attribute value</returns>
    object GetUserAttribute(string attributeName);

    #endregion

    #region Log and Task

    /// <summary>Add a log</summary>
    /// <param name="level">The log level</param>
    /// <param name="message">The log message</param>
    /// <param name="error">The log error</param>
    /// <param name="comment">The log comment</param>
    void AddLog(int level, string message, string error = null, string comment = null);

    /// <summary>Add task</summary>
    /// <param name="name">The task name</param>
    /// <param name="instruction">The task instruction</param>
    /// <param name="scheduleDate">The task schedule date</param>
    /// <param name="category">The task category</param>
    /// <param name="attributes">The task attributes</param>
    void AddTask(string name, string instruction, DateTime scheduleDate, string category,
        Dictionary<string, object> attributes = null);

    #endregion

}