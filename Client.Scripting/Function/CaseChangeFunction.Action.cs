/* CaseChangeFunction.Action */

namespace PayrollEngine.Client.Scripting.Function;

public partial class CaseChangeFunction
{

    #region Case Field General

    /// <summary>Hide all field inputs</summary>
    /// <param name="field">The case field name</param>
    [ActionParameter("field", "The target field", [StringType])]
    [CaseBuildAction("HiddenField", "Hide all field inputs", "FieldInput", "Field")]
    public void HiddenField(string field) =>
        SetCaseFieldAttribute(field, InputAttributes.Hidden, true);

    /// <summary>Show all field inputs</summary>
    /// <param name="field">The case field name</param>
    [ActionParameter("field", "The target field", [StringType])]
    [CaseBuildAction("VisibleField", "Show all field inputs", "FieldInput", "Field")]
    public void VisibleField(string field) =>
        SetCaseFieldAttribute(field, InputAttributes.Hidden, false);

    /// <summary>Show field description</summary>
    /// <param name="field">The target field</param>
    [ActionParameter("field", "The target field", [StringType])]
    [CaseBuildAction("ShowFieldDescription", "Show field description", "FieldInput", "Field")]
    public void ShowFieldDescription(string field) =>
        SetCaseFieldAttribute(field, InputAttributes.ShowDescription, true);

    #endregion

    #region Case Field Start

    /// <summary>Set field start label</summary>
    /// <param name="field">The target field</param>
    /// <param name="label">The field start label</param>
    [ActionParameter("field", "The target field", [StringType])]
    [ActionParameter("label", "The field start label", [StringType])]
    [CaseBuildAction("SetFieldStartLabel", "Set field start label", "FieldInput", "FieldStart")]
    public void SetFieldStartLabel(string field, string label) =>
        SetCaseFieldAttribute(field, InputAttributes.StartLabel, label);

    /// <summary>Set field start help</summary>
    /// <param name="field">The target field</param>
    /// <param name="help">The field start help</param>
    [ActionParameter("field", "The target field", [StringType])]
    [ActionParameter("help", "The field start help", [StringType])]
    [CaseBuildAction("SetFieldStartHelp", "Set field start help", "FieldInput", "FieldStart")]
    public void SetFieldStartHelp(string field, string help) =>
        SetCaseFieldAttribute(field, InputAttributes.StartHelp, help);

    /// <summary>Set field start required</summary>
    /// <param name="field">The target field</param>
    [ActionParameter("field", "The target field", [StringType])]
    [CaseBuildAction("SetFieldStartRequired", "Set field start required", "FieldInput", "FieldStart")]
    public void SetFieldStartRequired(string field) =>
        SetCaseFieldAttribute(field, InputAttributes.StartRequired, true);

    /// <summary>Set field start read only</summary>
    /// <param name="field">The target field</param>
    [ActionParameter("field", "The target field", [StringType])]
    [CaseBuildAction("SetFieldStartReadOnly", "Set field start read only", "FieldInput", "FieldStart")]
    public void SetFieldStartReadOnly(string field) =>
        SetCaseFieldAttribute(field, InputAttributes.StartReadOnly, true);

    /// <summary>Set field start format</summary>
    /// <param name="field">The target field</param>
    /// <param name="format">The format string</param>
    /// <remarks>see https://learn.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings</remarks>
    [ActionParameter("field", "The target field", [StringType])]
    [ActionParameter("format",
        "The format (https://learn.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings)", [StringType])]
    [CaseBuildAction("SetFieldStartFormat", "Set field start format", "FieldInput", "FieldStart")]
    public void SetFieldStartFormat(string field, string format) =>
        SetCaseFieldAttribute(field, InputAttributes.StartFormat, format);

    /// <summary>Set field start day date picker</summary>
    /// <param name="field">The target field</param>
    [ActionParameter("field", "The target field", [StringType])]
    [CaseBuildAction("SetFieldStartPickerOpenDay", "Set field start day date picker", "FieldInput", "FieldStart")]
    public void SetFieldStartPickerOpenDay(string field) =>
        SetCaseFieldAttribute(field, InputAttributes.StartPickerOpen, "day");

    /// <summary>Set field start month date picker</summary>
    /// <param name="field">The target field</param>
    [ActionParameter("field", "The target field", [StringType])]
    [CaseBuildAction("SetFieldStartPickerOpenMonth", "Set field start month date picker", "FieldInput", "FieldStart")]
    public void SetFieldStartPickerOpenMonth(string field) =>
        SetCaseFieldAttribute(field, InputAttributes.StartPickerOpen, "month");

    /// <summary>Set field start year date picker</summary>
    /// <param name="field">The target field</param>
    [ActionParameter("field", "The target field", [StringType])]
    [CaseBuildAction("SetFieldStartPickerOpenYear", "Set field start year date picker", "FieldInput", "FieldStart")]
    public void SetFieldStartPickerOpenYear(string field) =>
        SetCaseFieldAttribute(field, InputAttributes.StartPickerOpen, "year");

    /// <summary>Set field start picker date time</summary>
    /// <param name="field">The target field</param>
    [ActionParameter("field", "The target field", [StringType])]
    [CaseBuildAction("SetFieldStartPickerTypeDateTime", "Set field start picker date time", "FieldInput", "FieldStart")]
    public void SetFieldStartPickerTypeDateTime(string field) =>
        SetCaseFieldAttribute(field, InputAttributes.StartPickerType, "DateTimePicker");

    #endregion

    #region Case Field End

    /// <summary>Set field end label</summary>
    /// <param name="field">The target field</param>
    /// <param name="label">The field end label</param>
    [ActionParameter("field", "The target field", [StringType])]
    [ActionParameter("label", "The field end label", [StringType])]
    [CaseBuildAction("SetFieldEndLabel", "Set field end label", "FieldInput", "FieldEnd")]
    public void SetFieldEndLabel(string field, string label) =>
        SetCaseFieldAttribute(field, InputAttributes.EndLabel, label);

    /// <summary>Set field end help</summary>
    /// <param name="field">The target field</param>
    /// <param name="help">The field end help</param>
    [ActionParameter("field", "The target field", [StringType])]
    [ActionParameter("help", "The field end help", [StringType])]
    [CaseBuildAction("SetFieldEndHelp", "Set field end help", "FieldInput", "FieldEnd")]
    public void SetFieldEndHelp(string field, string help) =>
        SetCaseFieldAttribute(field, InputAttributes.EndHelp, help);

    /// <summary>Set field end required</summary>
    /// <param name="field">The target field</param>
    [ActionParameter("field", "The target field", [StringType])]
    [CaseBuildAction("SetFieldEndRequired", "Set field end required", "FieldInput", "FieldEnd")]
    public void SetFieldEndRequired(string field) =>
        SetCaseFieldAttribute(field, InputAttributes.EndRequired, true);

    /// <summary>Set field end read only</summary>
    /// <param name="field">The target field</param>
    [ActionParameter("field", "The target field", [StringType])]
    [CaseBuildAction("SetFieldEndReadOnly", "Set field end read only", "FieldInput", "FieldEnd")]
    public void SetFieldEndReadOnly(string field) =>
        SetCaseFieldAttribute(field, InputAttributes.EndReadOnly, true);

    /// <summary>Set field end format</summary>
    /// <param name="field">The target field</param>
    /// <param name="format">The format string</param>
    /// <remarks>see https://learn.microsoft.com/en-us/openspecs/windows_protocols/ms-lcid/a9eac961-e77d-41a6-90a5-ce1a8b0cdb9c</remarks>
    [ActionParameter("field", "The target field", [StringType])]
    [ActionParameter("format",
        "The format (https://learn.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings)", [StringType])]
    [CaseBuildAction("SetFieldEndFormat", "Set field end format", "FieldInput", "FieldEnd")]
    public void SetFieldEndFormat(string field, string format) =>
        SetCaseFieldAttribute(field, InputAttributes.EndFormat, format);

    /// <summary>Set field end day date picker</summary>
    /// <param name="field">The target field</param>
    [ActionParameter("field", "The target field", [StringType])]
    [CaseBuildAction("SetFieldEndPickerOpenDay", "Set field end day date picker", "FieldInput", "FieldEnd")]
    public void SetFieldEndPickerOpenDay(string field) =>
        SetCaseFieldAttribute(field, InputAttributes.EndPickerOpen, "day");

    /// <summary>Set field end month date picker</summary>
    /// <param name="field">The target field</param>
    [ActionParameter("field", "The target field", [StringType])]
    [CaseBuildAction("SetFieldEndPickerOpenMonth", "Set field end month date picker", "FieldInput", "FieldEnd")]
    public void SetFieldEndPickerOpenMonth(string field) =>
        SetCaseFieldAttribute(field, InputAttributes.EndPickerOpen, "month");

    /// <summary>Set field end year date picker</summary>
    /// <param name="field">The target field</param>
    [ActionParameter("field", "The target field", [StringType])]
    [CaseBuildAction("SetFieldEndPickerOpenYear", "Set field end year date picker", "FieldInput", "FieldEnd")]
    public void SetFieldEndPickerOpenYear(string field) =>
        SetCaseFieldAttribute(field, InputAttributes.EndPickerOpen, "year");

    /// <summary>Set field end time picker</summary>
    /// <param name="field">The target field</param>
    [ActionParameter("field", "The target field", [StringType])]
    [CaseBuildAction("SetFieldEndPickerTypeDateTime", "Set field end time picker", "FieldInput", "FieldEnd")]
    public void SetFieldEndPickerTypeDateTime(string field) =>
        SetCaseFieldAttribute(field, InputAttributes.EndPickerType, "DateTimePicker");

    #endregion

    #region Case Field Value

    /// <summary>Set field value label</summary>
    /// <param name="field">The target field</param>
    /// <param name="label">The label text</param>
    [ActionParameter("field", "The target field", [StringType])]
    [ActionParameter("label", "The label text", [StringType])]
    [CaseBuildAction("SetFieldValueLabel", "Set field value label", "FieldInput", "FieldValue")]
    public void SetFieldValueLabel(string field, string label) =>
        SetCaseFieldAttribute(field, InputAttributes.ValueLabel, label);

    /// <summary>Set field value adornment</summary>
    /// <param name="field">The target field</param>
    /// <param name="adornment">The adornment text</param>
    [ActionParameter("field", "The target field", [StringType])]
    [ActionParameter("adornment", "The adornment text", [StringType])]
    [CaseBuildAction("SetFieldValueAdornment", "Set field value adornment", "FieldInput", "FieldValue")]
    public void SetFieldValueAdornment(string field, string adornment) =>
        SetCaseFieldAttribute(field, InputAttributes.ValueAdornment, adornment);

    /// <summary>Set field value help</summary>
    /// <param name="field">The target field</param>
    /// <param name="help">The adornment text</param>
    [ActionParameter("field", "The target field", [StringType])]
    [ActionParameter("help", "The help text", [StringType])]
    [CaseBuildAction("SetFieldValueHelp", "Set field value help", "FieldInput", "FieldValue")]
    public void SetFieldValueHelp(string field, string help) =>
        SetCaseFieldAttribute(field, InputAttributes.ValueHelp, help);

    /// <summary>Set field value mask</summary>
    /// <param name="field">The target field</param>
    /// <param name="mask">The text mask</param>
    /// <remarks>see https://learn.microsoft.com/en-us/dotnet/api/system.windows.forms.maskedtextbox.mask</remarks>
    [ActionParameter("field", "The target field", [StringType])]
    [ActionParameter("mask",
        "The value mask (https://learn.microsoft.com/en-us/dotnet/api/system.windows.forms.maskedtextbox.mask)", [StringType])]
    [CaseBuildAction("SetFieldValueMask", "Set field value mask", "FieldInput", "FieldValue")]
    public void SetFieldValueMask(string field, string mask) =>
        SetCaseFieldAttribute(field, InputAttributes.ValueMask, mask);

    /// <summary>Set field value required</summary>
    /// <param name="field">The target field</param>
    [ActionParameter("field", "The target field", [StringType])]
    [CaseBuildAction("SetFieldValueRequired", "Set field value required", "FieldInput", "FieldValue")]
    public void SetFieldValueRequired(string field) =>
        SetCaseFieldAttribute(field, InputAttributes.ValueRequired, true);

    /// <summary>Set field value read only</summary>
    /// <param name="field">The target field</param>
    [ActionParameter("field", "The target field", [StringType])]
    [CaseBuildAction("SetFieldValueReadOnly", "Set field value read only", "FieldInput", "FieldValue")]
    public void SetFieldValueReadOnly(string field) =>
        SetCaseFieldAttribute(field, InputAttributes.ValueReadOnly, true);

    /// <summary>Set field value day date picker</summary>
    /// <param name="field">The target field</param>
    [ActionParameter("field", "The target field", [StringType])]
    [CaseBuildAction("SetFieldValuePickerOpenDay", "Set field value day date picker", "FieldInput", "FieldValue")]
    public void SetFieldValuePickerOpenDay(string field) =>
        SetCaseFieldAttribute(field, InputAttributes.ValuePickerOpen, "day");

    /// <summary>Set field value month date picker</summary>
    /// <param name="field">The target field</param>
    [ActionParameter("field", "The target field", [StringType])]
    [CaseBuildAction("SetFieldValuePickerOpenMonth", "Set field value month date picker", "FieldInput", "FieldValue")]
    public void SetFieldValuePickerOpenMonth(string field) =>
        SetCaseFieldAttribute(field, InputAttributes.ValuePickerOpen, "month");

    /// <summary>Set field value year date picker</summary>
    /// <param name="field">The target field</param>
    [ActionParameter("field", "The target field", [StringType])]
    [CaseBuildAction("SetFieldValuePickerOpenYear", "Set field value year date picker", "FieldInput", "FieldValue")]
    public void SetFieldValuePickerOpenYear(string field) =>
        SetCaseFieldAttribute(field, InputAttributes.ValuePickerOpen, "year");

    /// <summary>Set field value culture</summary>
    /// <param name="field">The target field</param>
    /// <param name="culture">The culture</param>
    /// <remarks>see https://learn.microsoft.com/en-us/openspecs/windows_protocols/ms-lcid/a9eac961-e77d-41a6-90a5-ce1a8b0cdb9c</remarks>
    [ActionParameter("field", "The target field", [StringType])]
    [ActionParameter("culture",
        "The culture (https://learn.microsoft.com/en-us/openspecs/windows_protocols/ms-lcid/a9eac961-e77d-41a6-90a5-ce1a8b0cdb9c)", [StringType])]
    [CaseBuildAction("SetFieldCulture", "Set field value culture", "FieldInput", "FieldValue")]
    public void SetFieldCulture(string field, string culture) =>
        SetCaseFieldAttribute(field, InputAttributes.Culture, culture);

    /// <summary>Set field minimum value</summary>
    /// <param name="field">The target field</param>
    /// <param name="min">The minimum value</param>
    [ActionParameter("field", "The target field", [NumericType])]
    [ActionParameter("min", "The minimum value")]
    [CaseBuildAction("SetFieldMinValue", "Set field minimum value", "FieldInput", "FieldValue")]
    public void SetFieldMinValue(string field, object min) =>
        SetCaseFieldAttribute(field, InputAttributes.MinValue, min);

    /// <summary>Set field maximum value</summary>
    /// <param name="field">The target field</param>
    /// <param name="max">The maximum value</param>
    [ActionParameter("field", "The target field", [NumericType])]
    [ActionParameter("max", "The maximum value")]
    [CaseBuildAction("SetFieldMaxValue", "Set field maximum value", "FieldInput", "FieldValue")]
    public void SetFieldMaxValue(string field, object max) =>
       SetCaseFieldAttribute(field, InputAttributes.MaxValue, max);

    /// <summary>Set field numeric step size</summary>
    /// <param name="field">The target field</param>
    /// <param name="size">The step size</param>
    [ActionParameter("field", "The target field", [IntType])]
    [ActionParameter("size", "The step size")]
    [CaseBuildAction("SetFieldStepSize", "Set field numeric step size", "FieldInput", "FieldValue")]
    public void SetFieldStepSize(string field, int size) =>
        SetCaseFieldAttribute(field, InputAttributes.StepSize, size);

    /// <summary>Set field value format</summary>
    /// <param name="field">The target field</param>
    /// <param name="format">The text format</param>
    /// <remarks>see https://learn.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings</remarks>
    [ActionParameter("field", "The target field", [StringType])]
    [ActionParameter("format",
        "The value format (https://learn.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings)", [StringType])]
    [CaseBuildAction("SetFieldFormat", "Set field value format", "FieldInput", "FieldValue")]
    public void SetFieldFormat(string field, string format) =>
        SetCaseFieldAttribute(field, InputAttributes.Format, format);

    /// <summary>Set field text line count</summary>
    /// <param name="field">The target field</param>
    /// <param name="count">The line count</param>
    [ActionParameter("field", "The target field", [StringType])]
    [ActionParameter("count", "The line count", [IntType])]
    [CaseBuildAction("SetFieldLineCount", "Set field text line count", "FieldInput", "FieldValue")]
    public void SetFieldLineCount(string field, int count) =>
        SetCaseFieldAttribute(field, InputAttributes.LineCount, count);

    /// <summary>Set field maximum text length</summary>
    /// <param name="field">The target field</param>
    /// <param name="length">The maximum length</param>
    [ActionParameter("field", "The target field", [StringType])]
    [ActionParameter("length", "The length", [IntType])]
    [CaseBuildAction("SetFieldMaxLength", "Set field maximum text length", "FieldInput", "FieldValue")]
    public void SetFieldMaxLength(string field, int length) =>
        SetCaseFieldAttribute(field, InputAttributes.MaxLength, length);

    /// <summary>Set field boolean as checkbox</summary>
    /// <param name="field">The target field</param>
    [ActionParameter("field", "The target field", [StringType])]
    [CaseBuildAction("SetFieldCheck", "Set field boolean as checkbox", "FieldInput", "FieldValue")]
    public void SetFieldCheck(string field) =>
        SetCaseFieldAttribute(field, InputAttributes.Check, true);

    #endregion

    #region Case Field Attachment

    /// <summary>Set field without file attachments</summary>
    /// <param name="field">The target field</param>
    [ActionParameter("field", "The target field", [StringType])]
    [CaseBuildAction("SetFieldAttachmentNone", "Set field without file attachments", "FieldInput", "Field")]
    public void SetFieldAttachmentNone(string field) =>
        SetCaseFieldAttribute(field, InputAttributes.Attachment, "none");

    /// <summary>Set field optional file attachments</summary>
    /// <param name="field">The target field</param>
    [ActionParameter("field", "The target field", [StringType])]
    [CaseBuildAction("SetFieldAttachmentOptional", "Set field optional file attachments", "FieldInput", "Field")]
    public void SetFieldAttachmentOptional(string field) =>
        SetCaseFieldAttribute(field, InputAttributes.Attachment, "optional");

    /// <summary>Set field mandatory file attachments</summary>
    /// <param name="field">The target field</param>
    [ActionParameter("field", "The target field", [StringType])]
    [CaseBuildAction("SetFieldAttachmentMandatory", "Set field mandatory file attachments", "FieldInput", "Field")]
    public void SetFieldAttachmentMandatory(string field) =>
        SetCaseFieldAttribute(field, InputAttributes.Attachment, "mandatory");

    /// <summary>Set field attachments file extensions</summary>
    /// <param name="field">The target field</param>
    /// <param name="extensions">The file extensions</param>
    [ActionParameter("field", "The target field", [StringType])]
    [ActionParameter("extensions", "The file extensions", [StringType])]
    [CaseBuildAction("SetFieldAttachmentExtensions", "Set field attachments file extensions", "FieldInput", "Field")]
    public void SetFieldAttachmentExtensions(string field, string extensions) =>
        SetCaseFieldAttribute(field, InputAttributes.AttachmentExtensions, extensions);

    #endregion

}