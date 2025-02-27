﻿/* Function */

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace PayrollEngine.Client.Scripting.Function;

/// <summary>Base class for any scripting function</summary>
// ReSharper disable once PartialTypeWithSinglePart
public abstract partial class Function : IDisposable
{
    /// <summary>The function runtime</summary>
    protected dynamic Runtime { get; }

    /// <summary>The default namespace name for function actions</summary>
    public const string DefaultActionNamespace = "System";

    /// <summary>The input namespace name for function actions</summary>
    public const string InputActionNamespace = "Input";

    /// <summary>New function instance</summary>
    /// <param name="runtime">The function runtime</param>
    protected Function(object runtime)
    {
        Runtime = runtime ?? throw new ArgumentNullException(nameof(runtime));

        // culture
        UserCulture = Runtime.UserCulture;

        // tenant
        TenantId = Runtime.TenantId;
        TenantIdentifier = Runtime.TenantIdentifier;

        // user
        UserId = Runtime.UserId;
        UserIdentifier = Runtime.UserIdentifier;
    }

    #region Culture

    /// <summary>The user culture</summary>
    public string UserCulture { get; }

    #endregion

    #region Tenant

    /// <summary>The tenant id</summary>
    public int TenantId { get; }

    /// <summary>The tenant identifier</summary>
    public string TenantIdentifier { get; }

    /// <summary>Get tenant attribute value</summary>
    public object GetTenantAttribute(string attributeName) =>
        Runtime.GetTenantAttribute(attributeName);

    /// <summary>Get tenant attribute typed value</summary>
    public T GetTenantAttribute<T>(string attributeName, T defaultValue = default)
    {
        var value = GetTenantAttribute(attributeName);
        return value == null ? defaultValue : (T)Convert.ChangeType(value, typeof(T));
    }

    #endregion

    #region User

    /// <summary>The user id</summary>
    public int UserId { get; }

    /// <summary>The user identifier</summary>
    public string UserIdentifier { get; }

    /// <summary>Get user attribute value</summary>
    public object GetUserAttribute(string attributeName) =>
        Runtime.GetUserAttribute(attributeName);

    /// <summary>Get user attribute typed value</summary>
    public T GetUserAttribute<T>(string attributeName, T defaultValue = default)
    {
        var value = GetUserAttribute(attributeName);
        return value == null ? defaultValue : (T)Convert.ChangeType(value, typeof(T));
    }

    #endregion

    #region Log and Task

    /// <summary>Add a verbose log</summary>
    /// <param name="message">The log message</param>
    /// <param name="error">The log error</param>
    /// <param name="comment">The log comment</param>
    public void LogVerbose(string message, string error = null, string comment = null) =>
        Log(LogLevel.Verbose, message, error, comment);

    /// <summary>Add a debug log</summary>
    /// <param name="message">The log message</param>
    /// <param name="error">The log error</param>
    /// <param name="comment">The log comment</param>
    public void LogDebug(string message, string error = null, string comment = null) =>
        Log(LogLevel.Debug, message, error, comment);

    /// <summary>Add an information log</summary>
    /// <param name="message">The log message</param>
    /// <param name="error">The log error</param>
    /// <param name="comment">The log comment</param>
    public void LogInformation(string message, string error = null, string comment = null) =>
        Log(LogLevel.Information, message, error, comment);

    /// <summary>Add a warning log</summary>
    /// <param name="message">The log message</param>
    /// <param name="error">The log error</param>
    /// <param name="comment">The log comment</param>
    public void LogWarning(string message, string error = null, string comment = null) =>
        Log(LogLevel.Warning, message, error, comment);

    /// <summary>Add error log</summary>
    /// <param name="message">The log message</param>
    /// <param name="error">The log error</param>
    /// <param name="comment">The log comment</param>
    public void LogError(string message, string error = null, string comment = null) =>
        Log(LogLevel.Error, message, error, comment);

    /// <summary>Add an error log using an exception</summary>
    /// <param name="exception">The exception</param>
    /// <param name="message">The log message, default is the exception message</param>
    /// <param name="comment">The log comment</param>
    public void LogError(Exception exception, string message = null, string comment = null)
    {
        message ??= exception.GetBaseException().Message;
        LogError(message, exception.ToString(), comment);
    }

    /// <summary>Add a fatal log</summary>
    /// <param name="message">The log message</param>
    /// <param name="error">The log error</param>
    /// <param name="comment">The log comment</param>
    public void LogFatal(string message, string error = null, string comment = null) =>
        Log(LogLevel.Fatal, message, error, comment);

    /// <summary>Add a fatal log using an exception</summary>
    /// <param name="exception">The exception</param>
    /// <param name="message">The log message, default is the exception message</param>
    /// <param name="comment">The log comment</param>
    public void LogFatal(Exception exception, string message = null, string comment = null)
    {
        message ??= exception.GetBaseException().Message;
        LogFatal(message, exception.ToString(), comment);
    }

    /// <summary>Add a log</summary>
    /// <param name="level">The log level</param>
    /// <param name="message">The log message</param>
    /// <param name="error">The log error</param>
    /// <param name="comment">The log comment</param>
    public void Log(LogLevel level, string message, string error = null, string comment = null) =>
        Runtime.AddLog((int)level, message, error, comment);

    /// <summary>Add task</summary>
    /// <param name="name">The task name</param>
    /// <param name="instruction">The task instruction</param>
    /// <param name="scheduleDate">The task schedule date</param>
    /// <param name="category">The task category</param>
    /// <param name="attributes">The task attributes</param>
    public void AddTask(string name, string instruction, DateTime scheduleDate, string category = null,
        Dictionary<string, object> attributes = null) =>
        Runtime.AddTask(name, instruction, scheduleDate, category, attributes);

    #endregion

    #region Webhooks

    /// <summary>Invoke report webhook</summary>
    /// <param name="requestOperation">The request operation</param>
    /// <param name="requestMessage">The webhook request message</param>
    /// <returns>The webhook response object</returns>
    public T InvokeWebhook<T>(string requestOperation, object requestMessage = null)
    {
        if (string.IsNullOrWhiteSpace(requestOperation))
        {
            throw new ArgumentException(nameof(requestOperation));
        }

        // webhook request
        var jsonRequest = requestMessage != null ? JsonSerializer.Serialize(requestMessage) : null;
        var jsonResponse = Runtime.InvokeWebhook(requestOperation, jsonRequest);
        if (string.IsNullOrWhiteSpace(jsonResponse))
        {
            return default;
        }
        var response = JsonSerializer.Deserialize<T>(jsonResponse);
        return response;
    }

    #endregion

    #region Scripting Development

    /// <summary>The name of the source file (scripting development)</summary>
    /// <value>The name of the source file</value>
    public string SourceFileName { get; }

    /// <summary>New function instance without runtime (scripting development)</summary>
    /// <remarks>Use <see cref="GetSourceFileName"/> in your constructor for the source file name</remarks>
    /// <param name="sourceFileName">The name of the source file</param>
    protected Function(string sourceFileName)
    {
        if (string.IsNullOrWhiteSpace(sourceFileName))
        {
            throw new ArgumentException(nameof(sourceFileName));
        }
        SourceFileName = sourceFileName;
    }

    /// <summary>Initialize the source file path (scripting development)</summary>
    /// <param name="sourceFilePath">The source file path (do not provide a value)</param>
    /// <returns>Source code file name</returns>
    protected static string GetSourceFileName(
        [CallerFilePath] string sourceFilePath = "")
    {
        return sourceFilePath;
    }

    #endregion

    /// <summary>Dispose the function</summary>
    public virtual void Dispose() =>
        GC.SuppressFinalize(this);
}
