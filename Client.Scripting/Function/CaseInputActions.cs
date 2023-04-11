/* CaseInputActions */

namespace PayrollEngine.Client.Scripting.Function;

/// <summary>Extension methods for case input actions</summary>
[ActionProvider(Function.InputActionNamespace, typeof(CaseChangeFunction))]
public class CaseInputActions : CaseChangeActionsBase
{

    #region Field Input

    /// <summary>Enable field start input</summary>
    /// <param name="context">The action context</param>
    /// <param name="field">The target field</param>
    [ActionParameter("field", "The target field", valueTypes: new[] { StringType })]
    [CaseBuildAction("EnableFieldStart", "Enable field start input", "FieldInput", "FieldStart")]
    public void EnableFieldStart(CaseChangeActionContext context, object field) =>
        RemoveInputAttribute(context, field, "readOnlyStart");

    /// <summary>Disable field start input</summary>
    /// <param name="context">The action context</param>
    /// <param name="field">The target field</param>
    [ActionParameter("field", "The target field", valueTypes: new[] { StringType })]
    [CaseBuildAction("DisableFieldStart", "Disable field start input", "FieldInput", "FieldStart")]
    public void DisableFieldStart(CaseChangeActionContext context, object field) =>
        SetInputAttribute(context, field, "readOnlyStart", true);

    /// <summary>Enable field end input</summary>
    /// <param name="context">The action context</param>
    /// <param name="field">The target field</param>
    [ActionParameter("field", "The target field", valueTypes: new[] { StringType })]
    [CaseBuildAction("EnableFieldEnd", "Enable field end input", "FieldInput", "FieldEnd")]
    public void EnableFieldEnd(CaseChangeActionContext context, object field) =>
        RemoveInputAttribute(context, field, "readOnlyEnd");

    /// <summary>Disable field end input</summary>
    /// <param name="context">The action context</param>
    /// <param name="field">The target field</param>
    [ActionParameter("field", "The target field", valueTypes: new[] { StringType })]
    [CaseBuildAction("DisableFieldEnd", "Disable field end input", "FieldInput", "FieldEnd")]
    public void DisableFieldEnd(CaseChangeActionContext context, object field) =>
        SetInputAttribute(context, field, "readOnlyEnd", true);

    /// <summary>Enable field value input</summary>
    /// <param name="context">The action context</param>
    /// <param name="field">The target field</param>
    [ActionParameter("field", "The target field", valueTypes: new[] { StringType })]
    [CaseBuildAction("EnableFieldValue", "Enable field value input", "FieldInput", "FieldValue")]
    public void EnableFieldValue(CaseChangeActionContext context, object field) =>
        RemoveInputAttribute(context, field, "readOnly");

    /// <summary>Disable field value input</summary>
    /// <param name="context">The action context</param>
    /// <param name="field">The target field</param>
    [ActionParameter("field", "The target field", valueTypes: new[] { StringType })]
    [CaseBuildAction("DisableFieldValue", "Disable field value input", "FieldInput", "FieldValue")]
    public void DisableFieldValue(CaseChangeActionContext context, object field) =>
        SetInputAttribute(context, field, "readOnly", true);

    /// <summary>Hide all field inputs</summary>
    /// <param name="context">The action context</param>
    /// <param name="field">The target field</param>
    [ActionParameter("field", "The target field", valueTypes: new[] { StringType })]
    [CaseBuildAction("HideField", "Hide all field inputs", "FieldInput", "Field")]
    public void HideField(CaseChangeActionContext context, object field) =>
        SetInputAttribute(context, field, "hidden", true);

    /// <summary>Show all field inputs</summary>
    /// <param name="context">The action context</param>
    /// <param name="field">The target field</param>
    [ActionParameter("field", "The target field", valueTypes: new[] { StringType })]
    [CaseBuildAction("ShowField", "Show all field inputs", "FieldInput", "Field")]
    public void ShowField(CaseChangeActionContext context, object field) =>
        RemoveInputAttribute(context, field, "hidden");

    /// <summary>Hide field description</summary>
    /// <param name="context">The action context</param>
    /// <param name="field">The target field</param>
    [ActionParameter("field", "The target field", valueTypes: new[] { StringType })]
    [CaseBuildAction("HideFieldDescription", "Hide field description", "FieldInput", "Field")]
    public void HideFieldDescription(CaseChangeActionContext context, object field) =>
        SetInputAttribute(context, field, "hiddenDescription", true);

    /// <summary>Show all field inputs</summary>
    /// <param name="context">The action context</param>
    /// <param name="field">The target field</param>
    [ActionParameter("field", "The target field", valueTypes: new[] { StringType })]
    [CaseBuildAction("ShowFieldDescription", "Show all field inputs", "FieldInput", "Field")]
    public void ShowFieldDescription(CaseChangeActionContext context, object field) =>
        RemoveInputAttribute(context, field, "hiddenDescription");

    /// <summary>Set field text multi line</summary>
    /// <param name="context">The action context</param>
    /// <param name="field">The target field</param>
    [ActionParameter("field", "The target field", valueTypes: new[] { StringType })]
    [CaseBuildAction("SetFieldMultiLine", "Set field text multi line", "FieldInput", "FieldValue")]
    public void SetFieldMultiLine(CaseChangeActionContext context, object field) =>
        SetInputAttribute(context, field, "multiLine", true);

    /// <summary>Reset field text multi line</summary>
    /// <param name="context">The action context</param>
    /// <param name="field">The target field</param>
    [ActionParameter("field", "The target field", valueTypes: new[] { StringType })]
    [CaseBuildAction("ResetFieldMultiLine", "Reset field text multi line", "FieldInput", "FieldValue")]
    public void ResetFieldMultiLine(CaseChangeActionContext context, object field) =>
        RemoveInputAttribute(context, field, "multiLine");

    /// <summary>Set field boolean switch</summary>
    /// <param name="context">The action context</param>
    /// <param name="field">The target field</param>
    [ActionParameter("field", "The target field", valueTypes: new[] { StringType })]
    [CaseBuildAction("SetFieldSwitch", "Set field boolean switch", "FieldInput", "FieldValue")]
    public void SetFieldSwitch(CaseChangeActionContext context, object field) =>
        SetInputAttribute(context, field, "switch", true);

    /// <summary>Reset field boolean switch</summary>
    /// <param name="context">The action context</param>
    /// <param name="field">The target field</param>
    [ActionParameter("field", "The target field", valueTypes: new[] { StringType })]
    [CaseBuildAction("ResetFieldSwitch", "Reset field boolean switch", "FieldInput", "FieldValue")]
    public void ResetFieldSwitch(CaseChangeActionContext context, object field) =>
        RemoveInputAttribute(context, field, "switch");

    /// <summary>Set field multi lookup selection</summary>
    /// <param name="context">The action context</param>
    /// <param name="field">The target field</param>
    [ActionParameter("field", "The target field", valueTypes: new[] { StringType })]
    [CaseBuildAction("SetFieldMultiLookup", "Set field multi lookup selection", "FieldInput", "FieldValue")]
    public void SetFieldMultiLookup(CaseChangeActionContext context, object field) =>
        SetInputAttribute(context, field, "multiLookup", true);

    /// <summary>Reset field multi lookup selection</summary>
    /// <param name="context">The action context</param>
    /// <param name="field">The target field</param>
    [ActionParameter("field", "The target field", valueTypes: new[] { StringType })]
    [CaseBuildAction("ResetFieldMultiLookup", "Reset field multi lookup selection", "FieldInput", "FieldValue")]
    public void ResetFieldMultiLookup(CaseChangeActionContext context, object field) =>
        RemoveInputAttribute(context, field, "multiLookup");

    /// <summary>Set field custom lookup value</summary>
    /// <param name="context">The action context</param>
    /// <param name="field">The target field</param>
    [ActionParameter("field", "The target field", valueTypes: new[] { StringType })]
    [CaseBuildAction("SetFieldCustomLookupValue", "Set field custom lookup value")]
    public void SetFieldCustomLookupValue(CaseChangeActionContext context, object field) =>
        SetInputAttribute(context, field, "customValue", true);

    /// <summary>Reset field custom lookup value</summary>
    /// <param name="context">The action context</param>
    /// <param name="field">The target field</param>
    [ActionParameter("field", "The target field", valueTypes: new[] { StringType })]
    [CaseBuildAction("ResetFieldCustomLookupValue", "Reset field custom lookup value", "FieldInput", "FieldValue")]
    public void ResetFieldCustomLookupValue(CaseChangeActionContext context, object field) =>
        RemoveInputAttribute(context, field, "customValue");

    /// <summary>Set field value label</summary>
    /// <param name="context">The action context</param>
    /// <param name="field">The target field</param>
    /// <param name="text">The label text</param>
    [ActionParameter("text", "The label text",
        valueTypes: new[] { StringType })]
    [ActionParameter("field", "The target field", valueTypes: new[] { StringType })]
    [CaseBuildAction("SetFieldLabel", "Set field value label", "FieldInput", "FieldValue")]
    public void SetFieldLabel(CaseChangeActionContext context, object field, object text)
    {
        var textValue = NewActionValue<string>(context, text);
        if (textValue == null || !textValue.IsFulfilled)
        {
            return;
        }
        SetInputAttribute(context, field, "valueLabel", textValue.ResolvedValue);
    }

    /// <summary>Reset field value label</summary>
    /// <param name="context">The action context</param>
    /// <param name="field">The target field</param>
    [ActionParameter("field", "The target field", valueTypes: new[] { StringType })]
    [CaseBuildAction("ResetFieldLabel", "Reset field value label", "FieldInput", "FieldValue")]
    public void ResetFieldLabel(CaseChangeActionContext context, object field) =>
        RemoveInputAttribute(context, field, "valueLabel");

    /// <summary>Set field value culture</summary>
    /// <param name="context">The action context</param>
    /// <param name="field">The target field</param>
    /// <param name="culture">The culture</param>
    /// <remarks>see www.learn.microsoft.com/en-us/openspecs/windows_protocols/ms-lcid/a9eac961-e77d-41a6-90a5-ce1a8b0cdb9c</remarks>
    [ActionParameter("culture", "The culture (www.learn.microsoft.com/en-us/openspecs/windows_protocols/ms-lcid/a9eac961-e77d-41a6-90a5-ce1a8b0cdb9c)",
        valueTypes: new[] { StringType })]
    [ActionParameter("field", "The target field", valueTypes: new[] { StringType })]
    [CaseBuildAction("SetFieldCulture", "Set field value culture", "FieldInput", "FieldValue")]
    public void SetFieldCulture(CaseChangeActionContext context, object field, object culture)
    {
        var cultureValue = NewActionValue<string>(context, culture);
        if (cultureValue == null || !cultureValue.IsFulfilled)
        {
            return;
        }
        SetInputAttribute(context, field, "culture", cultureValue.ResolvedValue);
    }

    /// <summary>Reset field value culture</summary>
    /// <param name="context">The action context</param>
    /// <param name="field">The target field</param>
    [ActionParameter("field", "The target field", valueTypes: new[] { StringType })]
    [CaseBuildAction("ResetFieldCulture", "Reset field value culture", "FieldInput", "FieldValue")]
    public void ResetFieldCulture(CaseChangeActionContext context, object field) =>
        RemoveInputAttribute(context, field, "culture");

    /// <summary>Set field value mask</summary>
    /// <param name="context">The action context</param>
    /// <param name="field">The target field</param>
    /// <param name="mask">The text mask</param>
    /// <remarks>see www.learn.microsoft.com/en-us/dotnet/api/system.windows.forms.maskedtextbox.mask</remarks>
    [ActionParameter("mask", "The value mask (www.learn.microsoft.com/en-us/dotnet/api/system.windows.forms.maskedtextbox.mask)",
        valueTypes: new[] { StringType })]
    [ActionParameter("field", "The target field", valueTypes: new[] { StringType })]
    [CaseBuildAction("SetFieldMask", "Set field value mask", "FieldInput", "FieldValue")]
    public void SetFieldMask(CaseChangeActionContext context, object field, object mask)
    {
        var maskValue = NewActionValue<string>(context, mask);
        if (maskValue == null || !maskValue.IsFulfilled)
        {
            return;
        }
        SetInputAttribute(context, field, "mask", maskValue.ResolvedValue);
    }

    /// <summary>Reset field value mask</summary>
    /// <param name="context">The action context</param>
    /// <param name="field">The target field</param>
    [ActionParameter("field", "The target field", valueTypes: new[] { StringType })]
    [CaseBuildAction("ResetFieldMask", "Reset field value mask", "FieldInput", "FieldValue")]
    public void ResetFieldMask(CaseChangeActionContext context, object field) =>
        RemoveInputAttribute(context, field, "mask");

    /// <summary>Set field sort order</summary>
    /// <param name="context">The action context</param>
    /// <param name="order">The sort order</param>
    /// <param name="field">The target field</param>
    [ActionParameter("field", "The target field", valueTypes: new[] { StringType })]
    [ActionParameter("order", "The sort order",
        valueTypes: new[] { IntType })]
    [CaseBuildAction("SetFieldOrder", "Set field sort order", "FieldInput", "Field")]
    public void SetFieldOrder(CaseChangeActionContext context, object field, object order)
    {
        var orderValue = NewActionValue<int>(context, order);
        if (orderValue == null || !orderValue.IsFulfilled)
        {
            return;
        }
        SetInputAttribute(context, field, "sortOrder", orderValue.ResolvedValue);
    }

    /// <summary>Reset field sort order</summary>
    /// <param name="context">The action context</param>
    /// <param name="field">The target field</param>
    [ActionParameter("field", "The target field", valueTypes: new[] { StringType })]
    [CaseBuildAction("ResetFieldOrder", "Reset field sort order", "FieldInput", "Field")]
    public void ResetFieldOrder(CaseChangeActionContext context, object field) =>
        RemoveInputAttribute(context, field, "sortOrder");

    /// <summary>Set field numeric step size</summary>
    /// <param name="context">The action context</param>
    /// <param name="field">The target field</param>
    /// <param name="stepSize">The step size</param>
    [ActionParameter("stepSize", "The step size",
        valueTypes: new[] { IntType })]
    [ActionParameter("field", "The target field", valueTypes: new[] { StringType })]
    [CaseBuildAction("SetFieldStepSize", "Set field numeric step size", "FieldInput", "FieldValue")]
    public void SetFieldStepSize(CaseChangeActionContext context, object field, object stepSize)
    {
        var stepSizeValue = NewActionValue<int>(context, stepSize);
        if (stepSizeValue == null || !stepSizeValue.IsFulfilled)
        {
            return;
        }
        SetInputAttribute(context, field, "stepValue", stepSizeValue.ResolvedValue);
    }

    /// <summary>Reset field numeric step size</summary>
    /// <param name="context">The action context</param>
    /// <param name="field">The target field</param>
    [ActionParameter("field", "The target field", valueTypes: new[] { StringType })]
    [CaseBuildAction("ResetFieldStepSize", "Reset field numeric step size", "FieldInput", "FieldValue")]
    public void ResetFieldStepSize(CaseChangeActionContext context, object field) =>
        RemoveInputAttribute(context, field, "stepValue");

    /// <summary>Set field value day date picker</summary>
    /// <param name="context">The action context</param>
    /// <param name="field">The target field</param>
    [ActionParameter("field", "The target field", valueTypes: new[] { StringType })]
    [CaseBuildAction("SetFieldValueDayPicker", "Set field value day date picker", "FieldInput", "FieldValue")]
    public void SetFieldValueDayPicker(CaseChangeActionContext context, object field) =>
        SetInputAttribute(context, field, "datePicker", "day");

    /// <summary>Set field value month date picker</summary>
    /// <param name="context">The action context</param>
    /// <param name="field">The target field</param>
    [ActionParameter("field", "The target field", valueTypes: new[] { StringType })]
    [CaseBuildAction("SetFieldValueMonthPicker", "Set field value month date picker", "FieldInput", "FieldValue")]
    public void SetFieldValueMonthPicker(CaseChangeActionContext context, object field) =>
        SetInputAttribute(context, field, "datePicker", "month");

    /// <summary>Set field value year date picker</summary>
    /// <param name="context">The action context</param>
    /// <param name="field">The target field</param>
    [ActionParameter("field", "The target field", valueTypes: new[] { StringType })]
    [CaseBuildAction("SetFieldValueYearPicker", "Set field value year date picker", "FieldInput", "FieldValue")]
    public void SetFieldValueYearPicker(CaseChangeActionContext context, object field) =>
        SetInputAttribute(context, field, "datePicker", "year");

    /// <summary>Reset field value date picker</summary>
    /// <param name="context">The action context</param>
    /// <param name="field">The target field</param>
    [ActionParameter("field", "The target field", valueTypes: new[] { StringType })]
    [CaseBuildAction("ResetFieldValuePicker", "Reset field value date picker", "FieldInput", "FieldValue")]
    public void ResetFieldValuePicker(CaseChangeActionContext context, object field) =>
        RemoveInputAttribute(context, field, "datePicker");

    /// <summary>Set field start day date picker</summary>
    /// <param name="context">The action context</param>
    /// <param name="field">The target field</param>
    [ActionParameter("field", "The target field", valueTypes: new[] { StringType })]
    [CaseBuildAction("SetFieldStartDayPicker", "Set field start day date picker", "FieldInput", "FieldStart")]
    public void SetFieldStartDayPicker(CaseChangeActionContext context, object field) =>
        SetInputAttribute(context, field, "datePickerStart", "day");

    /// <summary>Set field start month date picker</summary>
    /// <param name="context">The action context</param>
    /// <param name="field">The target field</param>
    [ActionParameter("field", "The target field", valueTypes: new[] { StringType })]
    [CaseBuildAction("SetFieldStartMonthPicker", "Set field start month date picker", "FieldInput", "FieldStart")]
    public void SetFieldStartMonthPicker(CaseChangeActionContext context, object field) =>
        SetInputAttribute(context, field, "datePickerStart", "month");

    /// <summary>Set field start year date picker</summary>
    /// <param name="context">The action context</param>
    /// <param name="field">The target field</param>
    [ActionParameter("field", "The target field", valueTypes: new[] { StringType })]
    [CaseBuildAction("SetFieldStartYearPicker", "Set field start year date picker", "FieldInput", "FieldStart")]
    public void SetFieldStartYearPicker(CaseChangeActionContext context, object field) =>
        SetInputAttribute(context, field, "datePickerStart", "year");

    /// <summary>Reset field start date picker</summary>
    /// <param name="context">The action context</param>
    /// <param name="field">The target field</param>
    [ActionParameter("field", "The target field", valueTypes: new[] { StringType })]
    [CaseBuildAction("ResetFieldStartPicker", "Reset field start date picker", "FieldInput", "FieldStart")]
    public void ResetFieldStartPicker(CaseChangeActionContext context, object field) =>
        RemoveInputAttribute(context, field, "datePickerStart");

    /// <summary>Set field end day date picker</summary>
    /// <param name="context">The action context</param>
    /// <param name="field">The target field</param>
    [ActionParameter("field", "The target field", valueTypes: new[] { StringType })]
    [CaseBuildAction("SetFieldEndDayPicker", "Set field end day date picker", "FieldInput", "FieldEnd")]
    public void SetFieldEndDayPicker(CaseChangeActionContext context, object field) =>
        SetInputAttribute(context, field, "datePickerEnd", "day");

    /// <summary>Set field end month date picker</summary>
    /// <param name="context">The action context</param>
    /// <param name="field">The target field</param>
    [ActionParameter("field", "The target field", valueTypes: new[] { StringType })]
    [CaseBuildAction("SetFieldEndMonthPicker", "Set field end month date picker", "FieldInput", "FieldEnd")]
    public void SetFieldEndMonthPicker(CaseChangeActionContext context, object field) =>
        SetInputAttribute(context, field, "datePickerEnd", "month");

    /// <summary>Set field end year date picker</summary>
    /// <param name="context">The action context</param>
    /// <param name="field">The target field</param>
    [ActionParameter("field", "The target field", valueTypes: new[] { StringType })]
    [CaseBuildAction("SetFieldEndYearPicker", "Set field end year date picker", "FieldInput", "FieldEnd")]
    public void SetFieldEndYearPicker(CaseChangeActionContext context, object field) =>
        SetInputAttribute(context, field, "datePickerEnd", "year");

    /// <summary>Reset field end date picker</summary>
    /// <param name="context">The action context</param>
    /// <param name="field">The target field</param>
    [ActionParameter("field", "The target field", valueTypes: new[] { StringType })]
    [CaseBuildAction("ResetFieldEndPicker", "Reset field end date picker", "FieldInput", "FieldEnd")]
    public void ResetFieldEndPicker(CaseChangeActionContext context, object field) =>
        RemoveInputAttribute(context, field, "datePickerEnd");

    /// <summary>Set field start time picker</summary>
    /// <param name="context">The action context</param>
    /// <param name="field">The target field</param>
    [ActionParameter("field", "The target field", valueTypes: new[] { StringType })]
    [CaseBuildAction("SetFieldStartTimePicker", "Set field start time picker", "FieldInput", "FieldStart")]
    public void SetFieldStartTimePicker(CaseChangeActionContext context, object field) =>
        SetInputAttribute(context, field, "startPicker", "DateTimePicker");

    /// <summary>Reset field start time picker</summary>
    /// <param name="context">The action context</param>
    /// <param name="field">The target field</param>
    [ActionParameter("field", "The target field", valueTypes: new[] { StringType })]
    [CaseBuildAction("ResetFieldStartTimePicker", "Reset field start time picker", "FieldInput", "FieldStart")]
    public void ResetFieldStartTimePicker(CaseChangeActionContext context, object field) =>
        RemoveInputAttribute(context, field, "startPicker");

    /// <summary>Set field end time picker</summary>
    /// <param name="context">The action context</param>
    /// <param name="field">The target field</param>
    [ActionParameter("field", "The target field", valueTypes: new[] { StringType })]
    [CaseBuildAction("SetFieldEndTimePicker", "Set field end time picker", "FieldInput", "FieldEnd")]
    public void SetFieldEndTimePicker(CaseChangeActionContext context, object field) =>
        SetInputAttribute(context, field, "endPicker", "DateTimePicker");

    /// <summary>Reset field end time picker</summary>
    /// <param name="context">The action context</param>
    /// <param name="field">The target field</param>
    [ActionParameter("field", "The target field", valueTypes: new[] { StringType })]
    [CaseBuildAction("ResetFieldEndTimePicker", "Reset field end time picker", "FieldInput", "FieldEnd")]
    public void ResetFieldEndTimePicker(CaseChangeActionContext context, object field) =>
        RemoveInputAttribute(context, field, "endPicker");

    /// <summary>Set field no file attachments</summary>
    /// <param name="context">The action context</param>
    /// <param name="field">The target field</param>
    [ActionParameter("field", "The target field", valueTypes: new[] { StringType })]
    [CaseBuildAction("SetFieldAttachmentNone", "Set field no file attachments", "FieldInput", "Field")]
    public void SetFieldAttachmentNone(CaseChangeActionContext context, object field) =>
        SetInputAttribute(context, field, "attachment", "none");

    /// <summary>Set field optional file attachments</summary>
    /// <param name="context">The action context</param>
    /// <param name="field">The target field</param>
    [ActionParameter("field", "The target field", valueTypes: new[] { StringType })]
    [CaseBuildAction("SetFieldAttachmentOptional", "Set field optional file attachments", "FieldInput", "Field")]
    public void SetFieldAttachmentOptional(CaseChangeActionContext context, object field) =>
        SetInputAttribute(context, field, "attachment", "optional");

    /// <summary>Set field mandatory file attachments</summary>
    /// <param name="context">The action context</param>
    /// <param name="field">The target field</param>
    [ActionParameter("field", "The target field", valueTypes: new[] { StringType })]
    [CaseBuildAction("SetFieldAttachmentMandatory", "Set field mandatory file attachments", "FieldInput", "Field")]
    public void SetFieldAttachmentMandatory(CaseChangeActionContext context, object field) =>
        SetInputAttribute(context, field, "attachment", "mandatory");

    /// <summary>Reset field file attachments</summary>
    /// <param name="context">The action context</param>
    /// <param name="field">The target field</param>
    [ActionParameter("field", "The target field", valueTypes: new[] { StringType })]
    [CaseBuildAction("ResetFieldAttachment", "Reset field file attachments", "FieldInput", "Field")]
    public void ResetFieldAttachment(CaseChangeActionContext context, object field) =>
        RemoveInputAttribute(context, field, "attachment");

    /// <summary>Set field attachments file extensions</summary>
    /// <param name="context">The action context</param>
    /// <param name="field">The target field</param>
    /// <param name="extensions">The file extensions</param>
    [ActionParameter("extensions", "The file extensions",
        valueTypes: new[] { StringType })]
    [ActionParameter("field", "The target field", valueTypes: new[] { StringType })]
    [CaseBuildAction("SetFieldAttachmentExtensions", "Set field attachments file extensions", "FieldInput", "Field")]
    public void SetFieldAttachmentExtensions(CaseChangeActionContext context, object field, object extensions)
    {
        var extensionsValue = NewActionValue<string>(context, extensions);
        if (extensionsValue == null || !extensionsValue.IsFulfilled)
        {
            return;
        }
        SetInputAttribute(context, field, "attachmentExtensions", extensionsValue.ResolvedValue);
    }

    /// <summary>Reset field attachments file extensions</summary>
    /// <param name="context">The action context</param>
    /// <param name="field">The target field</param>
    [ActionParameter("field", "The target field", valueTypes: new[] { StringType })]
    [CaseBuildAction("ResetFieldAttachmentExtensions", "Reset field attachments file extensions", "FieldInput", "Field")]
    public void ResetFieldAttachmentExtensions(CaseChangeActionContext context, object field) =>
        RemoveInputAttribute(context, field, "attachmentExtensions");

    #endregion

    #region Input

    /// <summary>Enable start input</summary>
    /// <param name="context">The action context</param>
    [CaseBuildAction("EnableStart", "Enable start input", "FieldInput", "FieldStart")]
    public void EnableStart(CaseChangeActionContext context) =>
        EnableFieldStart(context, context.CaseFieldName);

    /// <summary>Disable start input</summary>
    /// <param name="context">The action context</param>
    [CaseBuildAction("DisableStart", "Disable start input", "FieldInput", "FieldStart")]
    public void DisableStart(CaseChangeActionContext context) =>
        DisableFieldStart(context, context.CaseFieldName);

    /// <summary>Enable end input</summary>
    /// <param name="context">The action context</param>
    [CaseBuildAction("EnableEnd", "Enable end input", "FieldInput", "FieldEnd")]
    public void EnableEnd(CaseChangeActionContext context) =>
        EnableFieldEnd(context, context.CaseFieldName);

    /// <summary>Disable end input</summary>
    /// <param name="context">The action context</param>
    [CaseBuildAction("DisableEnd", "Disable end input", "FieldInput", "FieldEnd")]
    public void DisableEnd(CaseChangeActionContext context) =>
        DisableFieldEnd(context, context.CaseFieldName);

    /// <summary>Enable value input</summary>
    /// <param name="context">The action context</param>
    [CaseBuildAction("EnableValue", "Enable value input", "FieldInput", "FieldValue")]
    public void EnableValue(CaseChangeActionContext context) =>
        EnableFieldValue(context, context.CaseFieldName);

    /// <summary>Disable value input</summary>
    /// <param name="context">The action context</param>
    [CaseBuildAction("DisableValue", "Disable value input", "FieldInput", "FieldValue")]
    public void DisableValue(CaseChangeActionContext context) =>
        DisableFieldValue(context, context.CaseFieldName);

    /// <summary>Hide all inputs</summary>
    /// <param name="context">The action context</param>
    [CaseBuildAction("Hide", "Hide all inputs", "FieldInput", "Field")]
    public void Hide(CaseChangeActionContext context) =>
        HideField(context, context.CaseFieldName);

    /// <summary>Show all inputs</summary>
    /// <param name="context">The action context</param>
    [CaseBuildAction("Show", "Show all inputs", "FieldInput", "Field")]
    public void Show(CaseChangeActionContext context) =>
        ShowField(context, context.CaseFieldName);

    /// <summary>Hide field description</summary>
    /// <param name="context">The action context</param>
    [CaseBuildAction("HideDescription", "Hide field description", "FieldInput", "Field")]
    public void HideDescription(CaseChangeActionContext context) =>
        HideFieldDescription(context, context.CaseFieldName);

    /// <summary>Show description</summary>
    /// <param name="context">The action context</param>
    [CaseBuildAction("ShowDescription", "Show description", "FieldInput", "Field")]
    public void ShowDescription(CaseChangeActionContext context) =>
        ShowFieldDescription(context, context.CaseFieldName);

    /// <summary>Set text multi line</summary>
    /// <param name="context">The action context</param>
    [CaseBuildAction("SetMultiLine", "Set text multi line", "FieldInput", "FieldValue")]
    public void SetMultiLine(CaseChangeActionContext context) =>
        SetFieldMultiLine(context, context.CaseFieldName);

    /// <summary>Reset text multi line</summary>
    /// <param name="context">The action context</param>
    [CaseBuildAction("ResetMultiLine", "Reset text multi line", "FieldInput", "FieldValue")]
    public void ResetMultiLine(CaseChangeActionContext context) =>
        ResetFieldMultiLine(context, context.CaseFieldName);

    /// <summary>Set boolean switch</summary>
    /// <param name="context">The action context</param>
    [CaseBuildAction("SetSwitch", "Set boolean switch", "FieldInput", "FieldValue")]
    public void SetSwitch(CaseChangeActionContext context) =>
        SetFieldSwitch(context, context.CaseFieldName);

    /// <summary>Reset boolean switch</summary>
    /// <param name="context">The action context</param>
    [CaseBuildAction("ResetSwitch", "Reset boolean switch", "FieldInput", "FieldValue")]
    public void ResetSwitch(CaseChangeActionContext context) =>
        ResetFieldSwitch(context, context.CaseFieldName);

    /// <summary>Set multi lookup selection</summary>
    /// <param name="context">The action context</param>
    [CaseBuildAction("SetMultiLookup", "Set multi lookup selection", "FieldInput", "FieldValue")]
    public void SetMultiLookup(CaseChangeActionContext context) =>
        SetFieldMultiLookup(context, context.CaseFieldName);

    /// <summary>Reset multi lookup selection</summary>
    /// <param name="context">The action context</param>
    [CaseBuildAction("ResetMultiLookup", "Reset multi lookup selection", "FieldInput", "FieldValue")]
    public void ResetMultiLookup(CaseChangeActionContext context) =>
        ResetFieldMultiLookup(context, context.CaseFieldName);

    /// <summary>Set custom lookup value</summary>
    /// <param name="context">The action context</param>
    [CaseBuildAction("SetCustomLookupValue", "Set custom lookup value", "FieldInput", "FieldValue")]
    public void SetCustomLookupValue(CaseChangeActionContext context) =>
        SetFieldCustomLookupValue(context, context.CaseFieldName);

    /// <summary>Reset custom lookup value</summary>
    /// <param name="context">The action context</param>
    [CaseBuildAction("ResetCustomLookupValue", "Reset custom lookup value", "FieldInput", "FieldValue")]
    public void ResetCustomLookupValue(CaseChangeActionContext context) =>
        ResetFieldCustomLookupValue(context, context.CaseFieldName);

    /// <summary>Set value label</summary>
    /// <param name="context">The action context</param>
    /// <param name="text">The label text</param>
    [ActionParameter("text", "The label text",
        valueTypes: new[] { StringType })]
    [CaseBuildAction("SetLabel", "Set value label", "FieldInput", "Field")]
    public void SetLabel(CaseChangeActionContext context, object text) =>
        SetFieldLabel(context, context.CaseFieldName, text);

    /// <summary>Reset value label</summary>
    /// <param name="context">The action context</param>
    [CaseBuildAction("ResetLabel", "Reset value label", "FieldInput", "Field")]
    public void ResetLabel(CaseChangeActionContext context) =>
        ResetFieldLabel(context, context.CaseFieldName);

    /// <summary>Set value culture</summary>
    /// <param name="context">The action context</param>
    /// <param name="culture">The culture</param>
    /// <remarks>see www.learn.microsoft.com/en-us/openspecs/windows_protocols/ms-lcid/a9eac961-e77d-41a6-90a5-ce1a8b0cdb9c</remarks>
    [ActionParameter("culture", "The culture (www.learn.microsoft.com/en-us/openspecs/windows_protocols/ms-lcid/a9eac961-e77d-41a6-90a5-ce1a8b0cdb9c)",
        valueTypes: new[] { StringType })]
    [CaseBuildAction("SetCulture", "Set value culture", "FieldInput", "Field")]
    public void SetCulture(CaseChangeActionContext context, object culture) =>
        SetFieldCulture(context, context.CaseFieldName, culture);

    /// <summary>Reset value culture</summary>
    /// <param name="context">The action context</param>
    [CaseBuildAction("ResetCulture", "Reset value culture", "FieldInput", "Field")]
    public void ResetCulture(CaseChangeActionContext context) =>
        ResetFieldCulture(context, context.CaseFieldName);

    /// <summary>Set value mask</summary>
    /// <param name="context">The action context</param>
    /// <param name="mask">The text mask</param>
    /// <remarks>see www.learn.microsoft.com/en-us/dotnet/api/system.windows.forms.maskedtextbox.mask</remarks>
    [ActionParameter("mask", "The value mask (www.learn.microsoft.com/en-us/dotnet/api/system.windows.forms.maskedtextbox.mask)",
        valueTypes: new[] { StringType })]
    [CaseBuildAction("SetMask", "Set value mask", "FieldInput", "FieldValue")]
    public void SetMask(CaseChangeActionContext context, object mask) =>
        SetFieldMask(context, context.CaseFieldName, mask);

    /// <summary>Reset value mask</summary>
    /// <param name="context">The action context</param>
    [CaseBuildAction("ResetMask", "Reset value mask", "FieldInput", "FieldValue")]
    public void ResetMask(CaseChangeActionContext context) =>
        ResetFieldMask(context, context.CaseFieldName);

    /// <summary>Set field sort order</summary>
    /// <param name="context">The action context</param>
    /// <param name="order">The sort order</param>
    [ActionParameter("order", "The sort order",
        valueTypes: new[] { IntType })]
    [CaseBuildAction("SetOrder", "Set field sort order", "FieldInput", "Field")]
    public void SetOrder(CaseChangeActionContext context, object order) =>
        SetFieldOrder(context, context.CaseFieldName, order);

    /// <summary>Reset field sort order</summary>
    /// <param name="context">The action context</param>
    [CaseBuildAction("ResetOrder", "Reset field sort order", "FieldInput", "Field")]
    public void ResetOrder(CaseChangeActionContext context) =>
        ResetFieldOrder(context, context.CaseFieldName);

    /// <summary>Set numeric step size</summary>
    /// <param name="context">The action context</param>
    /// <param name="stepSize">The step size</param>
    [ActionParameter("stepSize", "The step size",
        valueTypes: new[] { IntType })]
    [CaseBuildAction("SetStepSize", "Set numeric step size", "FieldInput", "FieldValue")]
    public void SetStepSize(CaseChangeActionContext context, object stepSize) =>
        SetFieldStepSize(context, context.CaseFieldName, stepSize);

    /// <summary>Reset numeric step size</summary>
    /// <param name="context">The action context</param>
    [CaseBuildAction("ResetStepSize", "Reset numeric step size", "FieldInput", "FieldValue")]
    public void ResetStepSize(CaseChangeActionContext context) =>
        ResetFieldStepSize(context, context.CaseFieldName);

    /// <summary>Set value day date picker</summary>
    /// <param name="context">The action context</param>
    [CaseBuildAction("SetValueDayPicker", "Set value day date picker", "FieldInput", "FieldValue")]
    public void SetValueDayPicker(CaseChangeActionContext context) =>
        SetFieldValueDayPicker(context, context.CaseFieldName);

    /// <summary>Set value month date picker</summary>
    /// <param name="context">The action context</param>
    [CaseBuildAction("SetValueMonthPicker", "Set value month date picker", "FieldInput", "FieldValue")]
    public void SetValueMonthPicker(CaseChangeActionContext context) =>
        SetFieldValueMonthPicker(context, context.CaseFieldName);

    /// <summary>Set value year date picker</summary>
    /// <param name="context">The action context</param>
    [CaseBuildAction("SetValueYearPicker", "Set value year date picker", "FieldInput", "FieldValue")]
    public void SetValueYearPicker(CaseChangeActionContext context) =>
        SetFieldValueYearPicker(context, context.CaseFieldName);

    /// <summary>Reset value date picker</summary>
    /// <param name="context">The action context</param>
    [CaseBuildAction("ResetValuePicker", "Reset value date picker", "FieldInput", "FieldValue")]
    public void ResetValuePicker(CaseChangeActionContext context) =>
        ResetFieldValuePicker(context, context.CaseFieldName);

    /// <summary>Set start day date picker</summary>
    /// <param name="context">The action context</param>
    [CaseBuildAction("SetStartDayPicker", "Set start day date picker", "FieldInput", "FieldStart")]
    public void SetStartDayPicker(CaseChangeActionContext context) =>
        SetFieldStartDayPicker(context, context.CaseFieldName);

    /// <summary>Set start month date picker</summary>
    /// <param name="context">The action context</param>
    [CaseBuildAction("SetStartMonthPicker", "Set start month date picker", "FieldInput", "FieldStart")]
    public void SetStartMonthPicker(CaseChangeActionContext context) =>
        SetFieldStartMonthPicker(context, context.CaseFieldName);

    /// <summary>Set start year date picker</summary>
    /// <param name="context">The action context</param>
    [CaseBuildAction("SetStartYearPicker", "Set start year date picker", "FieldInput", "FieldStart")]
    public void SetStartYearPicker(CaseChangeActionContext context) =>
        SetFieldStartYearPicker(context, context.CaseFieldName);

    /// <summary>Reset start date picker</summary>
    /// <param name="context">The action context</param>
    [CaseBuildAction("ResetStartPicker", "Reset start date picker", "FieldInput", "FieldStart")]
    public void ResetStartPicker(CaseChangeActionContext context) =>
        ResetFieldStartPicker(context, context.CaseFieldName);

    /// <summary>Set end day date picker</summary>
    /// <param name="context">The action context</param>
    [CaseBuildAction("SetEndDayPicker", "Set end day date picker", "FieldInput", "FieldEnd")]
    public void SetEndDayPicker(CaseChangeActionContext context) =>
        SetFieldEndDayPicker(context, context.CaseFieldName);

    /// <summary>Set end month date picker</summary>
    /// <param name="context">The action context</param>
    [CaseBuildAction("SetEndMonthPicker", "Set end month date picker", "FieldInput", "FieldEnd")]
    public void SetEndMonthPicker(CaseChangeActionContext context) =>
        SetFieldEndMonthPicker(context, context.CaseFieldName);

    /// <summary>Set end year date picker</summary>
    /// <param name="context">The action context</param>
    [CaseBuildAction("SetEndYearPicker", "Set end year date picker", "FieldInput", "FieldEnd")]
    public void SetEndYearPicker(CaseChangeActionContext context) =>
        SetFieldEndYearPicker(context, context.CaseFieldName);

    /// <summary>Reset end date picker</summary>
    /// <param name="context">The action context</param>
    [CaseBuildAction("ResetEndPicker", "Reset end date picker", "FieldInput", "FieldEnd")]
    public void ResetEndPicker(CaseChangeActionContext context) =>
        ResetFieldEndPicker(context, context.CaseFieldName);

    /// <summary>Set start time picker</summary>
    /// <param name="context">The action context</param>
    [CaseBuildAction("SetStartTimePicker", "Set start time picker", "FieldInput", "FieldStart")]
    public void SetStartTimePicker(CaseChangeActionContext context) =>
        SetFieldStartTimePicker(context, context.CaseFieldName);

    /// <summary>Reset start time picker</summary>
    /// <param name="context">The action context</param>
    [CaseBuildAction("ResetStartTimePicker", "Reset start time picker", "FieldInput", "FieldStart")]
    public void ResetStartTimePicker(CaseChangeActionContext context) =>
        ResetFieldStartTimePicker(context, context.CaseFieldName);

    /// <summary>Set end time picker</summary>
    /// <param name="context">The action context</param>
    [CaseBuildAction("SetEndTimePicker", "Set end time picker", "FieldInput", "FieldStart")]
    public void SetEndTimePicker(CaseChangeActionContext context) =>
        SetFieldEndTimePicker(context, context.CaseFieldName);

    /// <summary>Reset end time picker</summary>
    /// <param name="context">The action context</param>
    [CaseBuildAction("ResetEndTimePicker", "Reset end time picker", "FieldInput", "FieldStart")]
    public void ResetEndTimePicker(CaseChangeActionContext context) =>
        ResetFieldEndTimePicker(context, context.CaseFieldName);

    /// <summary>Set no file attachments</summary>
    /// <param name="context">The action context</param>
    [CaseBuildAction("SetAttachmentNone", "Set no file attachments", "FieldInput", "Field")]
    public void SetAttachmentNone(CaseChangeActionContext context) =>
        SetFieldAttachmentNone(context, context.CaseFieldName);

    /// <summary>Set optional file attachments</summary>
    /// <param name="context">The action context</param>
    [CaseBuildAction("SetAttachmentOptional", "Set optional file attachments", "FieldInput", "Field")]
    public void SetAttachmentOptional(CaseChangeActionContext context) =>
        SetFieldAttachmentOptional(context, context.CaseFieldName);

    /// <summary>Set mandatory file attachments</summary>
    /// <param name="context">The action context</param>
    [CaseBuildAction("SetAttachmentMandatory", "Set mandatory file attachments", "FieldInput", "Field")]
    public void SetAttachmentMandatory(CaseChangeActionContext context) =>
        SetFieldAttachmentMandatory(context, context.CaseFieldName);

    /// <summary>Reset file attachments</summary>
    /// <param name="context">The action context</param>
    [CaseBuildAction("ResetAttachment", "Reset file attachments", "FieldInput", "Field")]
    public void ResetAttachment(CaseChangeActionContext context) =>
        ResetFieldAttachment(context, context.CaseFieldName);

    /// <summary>Set attachments file extensions</summary>
    /// <param name="context">The action context</param>
    /// <param name="extensions">The file extensions</param>
    [ActionParameter("extensions", "The file extensions",
        valueTypes: new[] { StringType })]
    [CaseBuildAction("SetAttachmentExtensions", "Set attachments file extensions", "FieldInput", "Field")]
    public void SetAttachmentExtensions(CaseChangeActionContext context, object extensions) =>
        SetFieldAttachmentExtensions(context, context.CaseFieldName, extensions);

    /// <summary>Reset attachments file extensions</summary>
    /// <param name="context">The action context</param>
    [CaseBuildAction("ResetAttachmentExtensions", "Reset attachments file extensions", "FieldInput", "Field")]
    public void ResetAttachmentExtensions(CaseChangeActionContext context) =>
        ResetFieldAttachmentExtensions(context, context.CaseFieldName);

    #endregion

    private static void SetInputAttribute(CaseChangeActionContext context, object field, string attribute, object value)
    {
        var fieldValue = NewActionValue<string>(context, field);
        if (fieldValue == null || !fieldValue.IsReference || string.IsNullOrWhiteSpace(fieldValue.ResolvedValue))
        {
            return;
        }
        context.Function.SetCaseFieldAttribute(fieldValue.ResolvedValue, $"input.{attribute}", value);
    }

    private static void RemoveInputAttribute(CaseChangeActionContext context, object field, string attribute)
    {
        var fieldValue = NewActionValue<string>(context, field);
        if (fieldValue == null || !fieldValue.IsReference || string.IsNullOrWhiteSpace(fieldValue.ResolvedValue))
        {
            return;
        }
        context.Function.RemoveCaseFieldAttribute(fieldValue.ResolvedValue, $"input.{attribute}");
    }

}
