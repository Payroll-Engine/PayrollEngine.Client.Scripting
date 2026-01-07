/* CaseRelationBuildFunction.Action */

using System;

namespace PayrollEngine.Client.Scripting.Function;

public partial class CaseRelationBuildFunction
{

    #region Source

    /// <summary>Get the case relation source field value</summary>
    /// <param name="field">The case field on the source case</param>
    [ActionParameter("field", "The case field on the source case", [StringType])]
    [CaseRelationBuildAction("GetSourceFieldValue", "Det the case relation source field value", "RelationField")]
    public ActionValue GetSourceFieldValue(string field) =>
        new(GetSourceValue(field));

    /// <summary>Get the case relation source field start date</summary>
    /// <param name="field">The case field on the source case</param>
    [ActionParameter("field", "The case field on the target case", [StringType])]
    [CaseRelationBuildAction("GetSourceFieldStart", "Get the case relation source field start date", "RelationField")]
    public ActionValue GetSourceFieldStart(string field) =>
        new(GetSourceStart(field));

    /// <summary>Get the case relation source field end date</summary>
    /// <param name="field">The case field on the source case</param>
    [ActionParameter("field", "The case field on the target case", [StringType])]
    [CaseRelationBuildAction("GetSourceFieldEnd", "Get the case relation source field end date", "RelationField")]
    public ActionValue GetSourceFieldEnd(string field) =>
        new(GetSourceEnd(field));

    #endregion

    #region Target

    /// <summary>Get the case relation target field value</summary>
    /// <param name="field">The case field on the target case</param>
    [ActionParameter("field", "The case field on the target case", [StringType])]
    [CaseRelationBuildAction("GetTargetFieldValue", "Get the case relation target field value", "RelationField")]
    public ActionValue GetTargetFieldValue(string field) =>
        new(GetTargetValue(field));

    /// <summary>Set the case relation target field value</summary>
    /// <param name="field">The case field on the target case</param>
    /// <param name="value">The value to set</param>
    [ActionParameter("field", "The case field on the target case", [StringType])]
    [ActionParameter("value", "The value to set")]
    [CaseRelationBuildAction("SetTargetFieldValue", "Set the case relation target field value", "RelationField")]
    public void SetTargetFieldValue(string field, object value) =>
        SetTargetValue(field, value);

    /// <summary>Get the case relation target field start date</summary>
    /// <param name="field">The case field on the target case</param>
    [ActionParameter("field", "The case field on the target case", [StringType])]
    [CaseRelationBuildAction("GetTargetFieldStart", "Get the case relation target field start date", "RelationField")]
    public ActionValue GetTargetFieldStart(string field) =>
        new(GetTargetStart(field));

    /// <summary>Set the case relation target field start date</summary>
    /// <param name="field">The case field on the target case</param>
    /// <param name="start">The start date to set</param>
    [ActionParameter("field", "The case field on the target case", [StringType])]
    [ActionParameter("start", "The start date to set", [DateType])]
    [CaseRelationBuildAction("SetTargetFieldStart", "Set the case relation target field change start date", "RelationField")]
    public void SetTargetFieldStart(string field, DateTime? start) =>
        SetTargetStart(field, start);

    /// <summary>Get the case relation target field end date</summary>
    /// <param name="field">The case field on the target case</param>
    [ActionParameter("field", "The case field on the target case", [StringType])]
    [CaseRelationBuildAction("GetTargetFieldEnd", "Get the case relation target field end date", "RelationField")]
    public ActionValue GetTargetFieldEnd(string field) =>
        new(GetTargetEnd(field));

    /// <summary>Set the case relation target field end date</summary>
    /// <param name="field">The case field on the target case</param>
    /// <param name="end">The end date to set</param>
    [ActionParameter("field", "The case field on the target case", [StringType])]
    [ActionParameter("end", "The end date to set", [DateType])]
    [CaseRelationBuildAction("SetTargetFieldEnd", "Set the case relation target field change end date", "RelationField")]
    public void SetTargetFieldEnd(string field, DateTime? end) =>
         SetTargetEnd(field, end);

    #endregion

}