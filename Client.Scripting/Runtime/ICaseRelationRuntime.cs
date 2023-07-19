using System;

namespace PayrollEngine.Client.Scripting.Runtime;

/// <summary>Runtime for the case relation function <see cref="Function.CaseRelationFunction"/></summary>
public interface ICaseRelationRuntime : IPayrollRuntime
{

    #region Source Case

    /// <summary>Gets the name of the source case</summary>
    string SourceCaseName { get; }

    /// <summary>Gets the source case slot</summary>
    string SourceCaseSlot { get; }

    /// <summary>Get the source case cancellation date</summary>
    DateTime? SourceCaseCancellationDate { get; }

    /// <summary>Get source field names</summary>
    string[] GetSourceFieldNames();

    /// <summary>Test if the source case contains fields</summary>
    bool HasSourceFields();

    /// <summary>Test if a source case field is defined</summary>
    /// <param name="caseFieldName">The name of the case field</param>
    /// <returns>True if the source case field is available</returns>
    bool HasSourceField(string caseFieldName);

    /// <summary>Test if a source case field is complete</summary>
    /// <param name="caseFieldName">The name of the case field</param>
    /// <returns>True if the source case field is complete</returns>
    bool IsSourceFieldComplete(string caseFieldName);

    /// <summary>Test if a source case field is empty (no start, end and value)</summary>
    /// <param name="caseFieldName">The name of the case field</param>
    /// <returns>True if the case field is empty</returns>
    bool IsSourceFieldEmpty(string caseFieldName);

    /// <summary>Test if a source case start date is defined</summary>
    /// <param name="caseFieldName">The name of the case field</param>
    /// <returns>True if the source case start date is available</returns>
    bool HasSourceStart(string caseFieldName);

    /// <summary>Get the source case start date by his field name</summary>
    /// <param name="caseFieldName">The name of the case field</param>
    /// <returns>The source case start date</returns>
    DateTime? GetSourceStart(string caseFieldName);

    /// <summary>Test if a source case end date is defined</summary>
    /// <param name="caseFieldName">The name of the case field</param>
    /// <returns>True if the source case end date is available</returns>
    bool HasSourceEnd(string caseFieldName);

    /// <summary>Get the source case end date by his field name</summary>
    /// <param name="caseFieldName">The name of the case field</param>
    /// <returns>The source case end date</returns>
    DateTime? GetSourceEnd(string caseFieldName);

    /// <summary>Get the source case value type</summary>
    /// <param name="caseFieldName">The name of the case field</param>
    int GetSourceValueType(string caseFieldName);

    /// <summary>Test if the source case value is defined</summary>
    /// <param name="caseFieldName">The name of the case field</param>
    /// <returns>True if the case value is available</returns>
    bool HasSourceValue(string caseFieldName);

    /// <summary>Get the source case value by his field name</summary>
    /// <param name="caseFieldName">The name of the case field</param>
    /// <returns>The source case value</returns>
    object GetSourceValue(string caseFieldName);

    /// <summary>Get source case attribute value</summary>
    /// <param name="attributeName">Name of the attribute</param>
    /// <returns>The source case attribute value</returns>
    object GetSourceCaseAttribute(string attributeName);

    /// <summary>Get source case field attribute value</summary>
    /// <param name="attributeName">Name of the attribute</param>
    /// <param name="caseFieldName">The name of the case field</param>
    /// <returns>The source case field value</returns>
    object GetSourceCaseFieldAttribute(string caseFieldName, string attributeName);

    /// <summary>Get source case value attribute value</summary>
    /// <param name="attributeName">Name of the attribute</param>
    /// <param name="caseFieldName">The name of the case field</param>
    /// <returns>The source case field value</returns>
    object GetSourceCaseValueAttribute(string caseFieldName, string attributeName);

    #endregion

    #region Target Case

    /// <summary>Gets the name of the target case</summary>
    string TargetCaseName { get; }

    /// <summary>Gets the target case slot</summary>
    string TargetCaseSlot { get; }

    /// <summary>Get the target case cancellation date</summary>
    DateTime? TargetCaseCancellationDate { get; }

    /// <summary>Get target field names</summary>
    string[] GetTargetFieldNames();

    /// <summary>Test if the target case contains fields</summary>
    bool HasTargetFields();

    /// <summary>Test if a target case field is defined</summary>
    /// <param name="caseFieldName">The name of the case field</param>
    /// <returns>True if the target case field is available</returns>
    bool HasTargetField(string caseFieldName);

    /// <summary>Test if a target case field is complete</summary>
    /// <param name="caseFieldName">The name of the case field</param>
    /// <returns>True if the target case field is complete</returns>
    bool IsTargetFieldComplete(string caseFieldName);

    /// <summary>Test if a target case field is empty (no start, end and value)</summary>
    /// <param name="caseFieldName">The name of the case field</param>
    /// <returns>True if the case field is empty</returns>
    bool IsTargetFieldEmpty(string caseFieldName);

    /// <summary>Test if a target case start date is defined</summary>
    /// <param name="caseFieldName">The name of the case field</param>
    /// <returns>True if the target case start date is available</returns>
    bool HasTargetStart(string caseFieldName);

    /// <summary>Get the target case start date by his field name</summary>
    /// <param name="caseFieldName">The name of the case field</param>
    /// <returns>The target case start date</returns>
    DateTime? GetTargetStart(string caseFieldName);

    /// <summary>Set the target case start date by his field name</summary>
    /// <param name="caseFieldName">The name of the case field</param>
    /// <param name="start">The start date to set</param>
    void SetTargetStart(string caseFieldName, DateTime? start);

    /// <summary>Initialize the target start</summary>
    /// <param name="caseFieldName">The name of the case field</param>
    /// <param name="start">The start date to set</param>
    void InitTargetStart(string caseFieldName, DateTime? start);

    /// <summary>Test if a target case end date is defined</summary>
    /// <param name="caseFieldName">The name of the case field</param>
    /// <returns>True if the target case end date is available</returns>
    bool HasTargetEnd(string caseFieldName);

    /// <summary>Get the target case end date by his field name</summary>
    /// <param name="caseFieldName">The name of the case field</param>
    /// <returns>The target case end date</returns>
    DateTime? GetTargetEnd(string caseFieldName);

    /// <summary>Set the target case end date by his field name</summary>
    /// <param name="caseFieldName">The name of the case field</param>
    /// <param name="end">The end date to set</param>
    void SetTargetEnd(string caseFieldName, DateTime? end);

    /// <summary>Initialize the target end</summary>
    /// <param name="caseFieldName">The name of the case field</param>
    /// <param name="end">The end date to set</param>
    void InitTargetEnd(string caseFieldName, DateTime? end);

    /// <summary>Get the target case value type</summary>
    /// <param name="caseFieldName">The name of the case field</param>
    int GetTargetValueType(string caseFieldName);

    /// <summary>Test if the target case value is defined</summary>
    /// <param name="caseFieldName">The name of the case field</param>
    /// <returns>True if the target value is available</returns>
    bool HasTargetValue(string caseFieldName);

    /// <summary>Get the target case value by his field name</summary>
    /// <param name="caseFieldName">The name of the case field</param>
    /// <returns>The target case value</returns>
    object GetTargetValue(string caseFieldName);

    /// <summary>Set the target case value by his field name</summary>
    /// <param name="caseFieldName">The name of the case field</param>
    /// <param name="value">The value to set</param>
    void SetTargetValue(string caseFieldName, object value);

    /// <summary>Initialize the target end</summary>
    void InitTargetValue(string caseFieldName, object value);

    /// <summary>Test if a target case field is available</summary>
    /// <param name="caseFieldName">The name of the case field</param>
    /// <returns>True if the case field is available</returns>
    bool TargetFieldAvailable(string caseFieldName);

    /// <summary>Set if the target case field is available
    /// </summary>
    /// <param name="caseFieldName">The name of the case field</param>
    /// <param name="available">The value to set</param>
    void TargetFieldAvailable(string caseFieldName, bool available);

    /// <summary>Get target case attribute value</summary>
    /// <param name="attributeName">Name of the attribute</param>
    /// <returns>The target case attribute value</returns>
    object GetTargetCaseAttribute(string attributeName);

    /// <summary>Get target case field attribute value</summary>
    /// <param name="caseFieldName">The name of the case field</param>
    /// <param name="attributeName">Name of the attribute</param>
    /// <returns>The target case field attribute value</returns>
    object GetTargetCaseFieldAttribute(string caseFieldName, string attributeName);

    /// <summary>Get target case value attribute value</summary>
    /// <param name="caseFieldName">The name of the case field</param>
    /// <param name="attributeName">Name of the attribute</param>
    /// <returns>The target case field attribute value</returns>
    object GetTargetCaseValueAttribute(string caseFieldName, string attributeName);

    #endregion

    #region Init and Copy

    /// <summary>Initializes the target case field start date from the source case field</summary>
    /// <param name="sourceFieldName">The name of the source case field</param>
    /// <param name="targetFieldName">The name of the target case field</param>
    void InitStart(string sourceFieldName, string targetFieldName);

    /// <summary>Copy the case field start date from source to target</summary>
    /// <param name="sourceFieldName">The name of the source case field</param>
    /// <param name="targetFieldName">The name of the target case field</param>
    void CopyStart(string sourceFieldName, string targetFieldName);

    /// <summary>Initializes the target case field end date from the source case field</summary>
    /// <param name="sourceFieldName">The name of the source case field</param>
    /// <param name="targetFieldName">The name of the target case field</param>
    void InitEnd(string sourceFieldName, string targetFieldName);

    /// <summary>Copy the case field end date from source to target</summary>
    /// <param name="sourceFieldName">The name of the source case field</param>
    /// <param name="targetFieldName">The name of the target case field</param>
    void CopyEnd(string sourceFieldName, string targetFieldName);

    /// <summary>Initializes the target case field value from the source case field</summary>
    /// <param name="sourceFieldName">The name of the source case field</param>
    /// <param name="targetFieldName">The name of the target case field</param>
    void InitValue(string sourceFieldName, string targetFieldName);

    /// <summary>Copy the case field value from source to target</summary>
    /// <param name="sourceFieldName">The name of the source case field</param>
    /// <param name="targetFieldName">The name of the target case field</param>
    void CopyValue(string sourceFieldName, string targetFieldName);

    #endregion

}