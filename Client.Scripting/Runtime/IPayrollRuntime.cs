using System;
using System.Collections.Generic;

namespace PayrollEngine.Client.Scripting.Runtime;

/// <summary>Runtime  during the execution of a payroll scripting function</summary>
public interface IPayrollRuntime : IRuntime
{

    #region Employee

    /// <summary>The employee id</summary>
    int? EmployeeId { get; }

    /// <summary>The employee identifier</summary>
    string EmployeeIdentifier { get; }

    /// <summary>Get employee attribute value</summary>
    /// <param name="attributeName">Name of the attribute</param>
    /// <returns>The employee attribute value</returns>
    object GetEmployeeAttribute(string attributeName);

    #endregion

    #region Culture

    /// <summary>The payroll culture</summary>
    string PayrollCulture { get; }

    #endregion

    #region Payroll

    /// <summary>The payroll id</summary>
    int PayrollId { get; }

    #endregion

    #region Division

    /// <summary>The division id</summary>
    int DivisionId { get; }

    #endregion

    #region Date/Periods

    /// <summary>The evaluation date</summary>
    DateTime EvaluationDate { get; }

    /// <summary>Get evaluation period</summary>
    Tuple<DateTime, DateTime> GetEvaluationPeriod();

    /// <summary>Get period by moment and offset</summary>
    /// <param name="periodMoment">The period moment</param>
    /// <param name="offset">The period offset: 0=current, -1=previous, 1=next</param>
    /// <returns>The offset period including the moment</returns>
    Tuple<DateTime, DateTime> GetPeriod(DateTime periodMoment, int offset);

    /// <summary>Get cycle by moment and offset</summary>
    /// <param name="cycleMoment">The cycle moment</param>
    /// <param name="offset">The cycle offset: 0=current, -1=previous, 1=next</param>
    /// <returns>The offset cycle including the moment</returns>
    Tuple<DateTime, DateTime> GetCycle(DateTime cycleMoment, int offset);

    #endregion

    #region Case Field and Case Value

    /// <summary>Get case value type</summary>
    /// <param name="caseFieldName">The case field name</param>
    /// <returns>The case value type</returns>
    int? GetCaseValueType(string caseFieldName);

    /// <summary>Get case field attribute value</summary>
    /// <param name="caseFieldName">The name of the case field</param>
    /// <param name="attributeName">Name of the attribute</param>
    /// <returns>The case field attribute value</returns>
    object GetCaseFieldAttribute(string caseFieldName, string attributeName);

    /// <summary>Get case field value attribute value</summary>
    /// <param name="caseFieldName">The name of the case field</param>
    /// <param name="attributeName">Name of the attribute</param>
    /// <returns>The case field value attribute value</returns>
    object GetCaseValueAttribute(string caseFieldName, string attributeName);

    /// <summary>Get the case value slots</summary>
    /// <param name="caseFieldName">The name of the case field</param>
    /// <returns>The case slot names, empty on unknown case field</returns>
    List<string> GetCaseValueSlots(string caseFieldName);

    /// <summary>Get the case value tags</summary>
    /// <param name="caseFieldName">The name of the case field</param>
    /// <param name="valueDate">The value date</param>
    /// <returns>The case value tas, empty on unknown case field</returns>
    List<string> GetCaseValueTags(string caseFieldName, DateTime valueDate);

    /// <summary>Get case value from a specific moment</summary>
    /// <param name="caseFieldName">The case field name</param>
    /// <param name="valueDate">The value date</param>
    /// <remarks>Use nested tuples to reduce the tuple item count to 7</remarks>
    /// <returns>Case value from a specific date</returns>
    Tuple<string, DateTime, Tuple<DateTime?, DateTime?>, object, DateTime?, List<string>, Dictionary<string, object>> GetCaseValue(
        string caseFieldName, DateTime valueDate);

    /// <summary>Get case values from a specific moment</summary>
    /// <param name="caseFieldNames">The case field names</param>
    /// <param name="valueDate">The value date</param>
    /// <remarks>Use nested tuples to reduce the tuple item count to 7</remarks>
    /// <returns>Case value from a specific date</returns>
    List<Tuple<string, DateTime, Tuple<DateTime?, DateTime?>, object, DateTime?, List<string>, Dictionary<string, object>>> GetCaseValues(
        IList<string> caseFieldNames, DateTime valueDate);

    /// <summary>Get case values by date range</summary>
    /// <param name="caseFieldName">The case field name</param>
    /// <param name="startDate">The date after the case value was created</param>
    /// <param name="endDate">The date before the case value was created</param>
    /// <remarks>Use nested tuples to reduce the tuple item count to 7</remarks>
    /// <returns>Case values from the current period</returns>
    List<Tuple<string, DateTime, Tuple<DateTime?, DateTime?>, object, DateTime?, List<string>, Dictionary<string, object>>> GetCaseValues(
        string caseFieldName, DateTime? startDate = null, DateTime? endDate = null);

    /// <summary>Get case period values of multiple cases by date range and case field name</summary>
    /// <param name="startDate">The time period start date</param>
    /// <param name="endDate">The time period end date</param>
    /// <param name="caseFieldNames">The name of the case fields</param>
    /// <returns>The period values of multiple cases</returns>
    Dictionary<string, List<Tuple<DateTime, DateTime?, DateTime?, object>>> GetCasePeriodValues(DateTime startDate,
        DateTime endDate, params string[] caseFieldNames);

    #endregion

    #region Regulation Lookups

    /// <summary>Test for existing lookup</summary>
    /// <param name="lookupName">The name of the lookup</param>
    /// <returns>True on existing lookup</returns>
    bool HasLookup(string lookupName);

    /// <summary>Get lookup value by key</summary>
    /// <param name="lookupName">The name of the lookup</param>
    /// <param name="lookupKey">The lookup value key</param>
    /// <param name="culture">The value culture</param>
    /// <returns>The lookup value matching tho the lookup key</returns>
    string GetLookup(string lookupName, string lookupKey, string culture = null);

    /// <summary>Get lookup value by range</summary>
    /// <param name="lookupName">The name of the lookup</param>
    /// <param name="rangeValue">The range value</param>
    /// <param name="lookupKey">The lookup key</param>
    /// <param name="culture">The value culture</param>
    /// <returns>The lookup value matching tho the lookup key</returns>
    string GetRangeLookup(string lookupName, decimal rangeValue, string lookupKey = null, string culture = null);

    #endregion

}