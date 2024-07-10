using System;

namespace PayrollEngine.Client.Scripting.Runtime;

/// <summary>Runtime for the case change function <see cref="Function.CaseChangeFunction"/></summary>
public interface ICaseChangeRuntime : ICaseRuntime
{
    #region Case

    /// <summary>Get the cancellation date</summary>
    public DateTime? CancellationDate { get; }

    /// <summary>Test if a case is available</summary>
    /// <param name="caseName">The name of the case</param>
    /// <returns>True if the case is available</returns>
    bool CaseAvailable(string caseName);

    /// <summary>Set case attribute value</summary>
    /// <param name="caseName">The name of the case</param>
    /// <param name="attributeName">The name of the case attribute</param>
    /// <param name="value">The value of the case attribute</param>
    void SetCaseAttribute(string caseName, string attributeName, object value);

    /// <summary>Remove case attribute</summary>
    /// <param name="caseName">The name of the case</param>
    /// <param name="attributeName">The name of the case attribute</param>
    /// <returns>True if the case attribute has been removed</returns>
    bool RemoveCaseAttribute(string caseName, string attributeName);

    #endregion

    #region Case Fields

    /// <summary>Get field names</summary>
    string[] GetFieldNames();

    /// <summary>Test if the case contains fields</summary>
    bool HasFields();

    /// <summary>Test if a case field is defined</summary>
    /// <param name="caseFieldName">The name of the case field</param>
    /// <returns>True if the case field is available</returns>
    bool HasField(string caseFieldName);

    /// <summary>Test if a case field is complete</summary>
    /// <param name="caseFieldName">The name of the case field</param>
    /// <returns>True if the case field is complete</returns>
    bool IsFieldComplete(string caseFieldName);

    /// <summary>Test if a case field is empty (no start, end and value)</summary>
    /// <param name="caseFieldName">The name of the case field</param>
    /// <returns>True if the case field is empty</returns>
    bool IsFieldEmpty(string caseFieldName);

    /// <summary>Test if a case field is available</summary>
    /// <param name="caseFieldName">The name of the case field</param>
    /// <returns>True if the case field is available</returns>
    bool FieldAvailable(string caseFieldName);

    /// <summary>Set if the case field is available</summary>
    /// <param name="caseFieldName">The name of the case field</param>
    /// <param name="available">The case field available value</param>
    void FieldAvailable(string caseFieldName, bool available);

    /// <summary>Test if a case start date is defined</summary>
    /// <param name="caseFieldName">The name of the case field</param>
    /// <returns>True if the case start date is available</returns>
    bool HasStart(string caseFieldName);

    /// <summary>Get the case start date by his field name</summary>
    /// <param name="caseFieldName">The name of the case field</param>
    /// <returns>The source case start date</returns>
    DateTime? GetStart(string caseFieldName);

    /// <summary>Set the case start date by his field name</summary>
    /// <param name="caseFieldName">The name of the case field</param>
    /// <param name="start">The start date to set</param>
    void SetStart(string caseFieldName, DateTime? start);

    /// <summary>Initialize the case start date by his field name</summary>
    /// <param name="caseFieldName">The name of the case field</param>
    /// <param name="start">The start date to initialize</param>
    void InitStart(string caseFieldName, DateTime? start);

    /// <summary>Test if a case end date is defined</summary>
    /// <param name="caseFieldName">The name of the case field</param>
    /// <returns>True if the case end date is available</returns>
    bool HasEnd(string caseFieldName);

    /// <summary>Get the case end date by his field name</summary>
    /// <param name="caseFieldName">The name of the case field</param>
    /// <returns>The case end date</returns>
    DateTime? GetEnd(string caseFieldName);

    /// <summary>Set the case end date by his field name</summary>
    /// <param name="caseFieldName">The name of the case field</param>
    /// <param name="end">The end date to set</param>
    void SetEnd(string caseFieldName, DateTime? end);

    /// <summary>Initialize the case end date by his field name</summary>
    /// <param name="caseFieldName">The name of the case field</param>
    /// <param name="end">The end date to initialize</param>
    void InitEnd(string caseFieldName, DateTime? end);

    /// <summary>Get a case value type</summary>
    /// <param name="caseFieldName">The name of the case field</param>
    int GetValueType(string caseFieldName);

    /// <summary>Test if a case value is defined</summary>
    /// <param name="caseFieldName">The name of the case field</param>
    bool HasValue(string caseFieldName);

    /// <summary>Get a case value by his field name, in context of the script</summary>
    /// <param name="caseFieldName">The name of the case field</param>
    object GetValue(string caseFieldName);

    /// <summary>Set the target case value by his field name</summary>
    /// <param name="caseFieldName">The name of the case field</param>
    /// <param name="value">The value to set</param>
    void SetValue(string caseFieldName, object value);

    /// <summary>Initialize the case value by his field name</summary>
    /// <param name="caseFieldName">The name of the case field</param>
    /// <param name="value">The value to initialize</param>
    void InitValue(string caseFieldName, object value);

    /// <summary>Add case value tag</summary>
    /// <param name="caseFieldName">The name of the case field</param>
    /// <param name="tag">The tag to add</param>
    /// <returns>True if the case value tag has been added</returns>
    bool AddCaseValueTag(string caseFieldName, string tag);

    /// <summary>Remove case value tag</summary>
    /// <param name="caseFieldName">The name of the case field</param>
    /// <param name="tag">The tag to remove</param>
    /// <returns>True if the case value tag has been removed</returns>
    bool RemoveCaseValueTag(string caseFieldName, string tag);

    /// <summary>Set case field attribute value</summary>
    /// <param name="caseFieldName">The name of the case field</param>
    /// <param name="attributeName">The name of the case field attribute</param>
    /// <param name="value">The value of the case field attribute</param>
    void SetCaseFieldAttribute(string caseFieldName, string attributeName, object value);

    /// <summary>Remove case field attribute</summary>
    /// <param name="caseFieldName">The name of the case field</param>
    /// <param name="attributeName">The name of the case field attribute</param>
    /// <returns>True if the case field attribute has been removed</returns>
    bool RemoveCaseFieldAttribute(string caseFieldName, string attributeName);

    /// <summary>Set case field value attribute value</summary>
    /// <param name="caseFieldName">The name of the case field</param>
    /// <param name="attributeName">The name of the case field attribute</param>
    /// <param name="value">The value of the case field value attribute</param>
    void SetCaseValueAttribute(string caseFieldName, string attributeName, object value);

    /// <summary>Remove case field value attribute</summary>
    /// <param name="caseFieldName">The name of the case field</param>
    /// <param name="attributeName">The name of the case field attribute</param>
    /// <returns>True if the case field attribute has been removed</returns>
    bool RemoveCaseValueAttribute(string caseFieldName, string attributeName);

    #endregion

}