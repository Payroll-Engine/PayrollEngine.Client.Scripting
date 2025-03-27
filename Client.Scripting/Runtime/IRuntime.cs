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

    /// <summary>The user identifier</summary>
    string UserIdentifier { get; }

    /// <summary>The user culture</summary>
    string UserCulture { get; }

    /// <summary>The user type</summary>
    int UserType { get; }

    /// <summary>Get user attribute value</summary>
    /// <param name="attributeName">Name of the attribute</param>
    /// <returns>The user attribute value</returns>
    object GetUserAttribute(string attributeName);

    #endregion

    #region Culture and Calendar

    /// <summary>Gets the derived culture name by priority:
    /// - employee (optional)
    /// - division (optional)
    /// - tenant (fallback)</summary>
    /// <param name="divisionId">The division id, use 0 to ignore the division culture (mandatory with employee id)</param>
    /// <param name="employeeId">The employee id, use 0 to ignore the employee culture</param>
    /// <returns>The most derived culture name</returns>
    string GetDerivedCulture(int divisionId, int employeeId);

    /// <summary>Gets the derived calendar name by priority:
    /// - employee (optional)
    /// - division (optional)
    /// - tenant (fallback)</summary>
    /// <param name="divisionId">The division id, use 0 to ignore the division calendar</param>
    /// <param name="employeeId">The employee id, use 0 to ignore the employee calendar</param>
    /// <returns>The most derived calendar name</returns>
    string GetDerivedCalendar(int divisionId, int employeeId);

    /// <summary>Test for calendar working day</summary>
    /// <param name="calendarName">The calendar name</param>
    /// <param name="moment">Test day</param>
    bool IsCalendarWorkDay(string calendarName, DateTime moment);

    /// <summary>Get previous working days</summary>
    /// <param name="calendarName">The calendar name</param>
    /// <param name="moment">The start moment (not included in results)</param>
    /// <param name="count">The number of days (default: 1)</param>
    List<DateTime> GetPreviousWorkDays(string calendarName, DateTime moment, int count);

    /// <summary>Get next working days</summary>
    /// <param name="calendarName">The calendar name</param>
    /// <param name="moment">The start moment (not included in results)</param>
    /// <param name="count">The number of days (default: 1)</param>
    /// <returns>Returns true for valid time units</returns>
    List<DateTime> GetNextWorkDays(string calendarName, DateTime moment, int count);

    /// <summary>Gets the calendar period</summary>
    /// <param name="calendarName">The calendar name</param>
    /// <param name="moment">Moment within the period</param>
    /// <param name="offset">The period offset</param>
    /// <param name="culture">The calendar culture</param>
    Tuple<DateTime, DateTime> GetCalendarPeriod(string calendarName, DateTime moment, int offset, string culture);

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

    #region Webhook

    /// <summary>Invoke case relation webhook and receive the response JSON data</summary>
    /// <param name="requestOperation">The request operation</param>
    /// <param name="requestMessage">The JSON request message</param>
    /// <returns>The webhook response object as JSON</returns>
    string InvokeWebhook(string requestOperation, string requestMessage = null);

    #endregion

}