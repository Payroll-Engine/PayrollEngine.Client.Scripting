﻿/* CaseInputActions */

namespace PayrollEngine.Client.Scripting.Function;

/// <summary>Extension methods for case input actions</summary>
[ActionProvider(Function.InputActionNamespace, typeof(CaseChangeFunction))]
public class CaseInputActions : CaseChangeActionsBase
{

    #region Casse General

    /// <summary>Set case icon</summary>
    /// <param name="context">The action context</param>
    /// <param name="icon">The icon name</param>
    [ActionParameter("icon", "The icon name", valueTypes: [StringType])]
    [CaseBuildAction("SetIcon", "Set case icon", "Case")]
    public void SetIcon(CaseChangeActionContext context, object icon)
    {
        var iconValue = GetActionValue<string>(context, icon);
        if (iconValue == null || !iconValue.IsFulfilled)
        {
            return;
        }

        SetCaseAttribute(context, InputAttributes.Icon, iconValue.ResolvedValue);
    }

    #endregion

    #region Case Field General

    /// <summary>Hide all field inputs</summary>
    /// <param name="context">The action context</param>
    /// <param name="field">The target field</param>
    [ActionParameter("field", "The target field", valueTypes: [StringType])]
    [CaseBuildAction("HiddenField", "Hide all field inputs", "FieldInput", "Field")]
    public void HiddenField(CaseChangeActionContext context, object field) =>
        SetCaseFieldAttribute(context, field, InputAttributes.Hidden, true);

    /// <summary>Show field description</summary>
    /// <param name="context">The action context</param>
    /// <param name="field">The target field</param>
    [ActionParameter("field", "The target field", valueTypes: [StringType])]
    [CaseBuildAction("ShowFieldDescription", "Show field description", "FieldInput", "Field")]
    public void ShowFieldDescription(CaseChangeActionContext context, object field) =>
        SetCaseFieldAttribute(context, field, InputAttributes.ShowDescription, true);

    #endregion

    #region Case Field Start

    /// <summary>Set field start label</summary>
    /// <param name="context">The action context</param>
    /// <param name="field">The target field</param>
    /// <param name="label">The field start label</param>
    [ActionParameter("field", "The target field", valueTypes: [StringType])]
    [ActionParameter("label", "The field start label", valueTypes: [StringType])]
    [CaseBuildAction("SetFieldStartLabel", "Set field start label", "FieldInput", "FieldStart")]
    public void SetFieldStartLabel(CaseChangeActionContext context, object field, object label)
    {
        var labelValue = GetActionValue<string>(context, label);
        if (labelValue == null || !labelValue.IsFulfilled)
        {
            return;
        }

        SetCaseFieldAttribute(context, field, InputAttributes.StartLabel, labelValue.ResolvedValue);
    }

    /// <summary>Set field start help</summary>
    /// <param name="context">The action context</param>
    /// <param name="field">The target field</param>
    /// <param name="help">The field start help</param>
    [ActionParameter("field", "The target field", valueTypes: [StringType])]
    [ActionParameter("help", "The field start help", valueTypes: [StringType])]
    [CaseBuildAction("SetFieldStartHelp", "Set field start help", "FieldInput", "FieldStart")]
    public void SetFieldStartHelp(CaseChangeActionContext context, object field, object help)
    {
        var helpValue = GetActionValue<string>(context, help);
        if (helpValue == null || !helpValue.IsFulfilled)
        {
            return;
        }

        SetCaseFieldAttribute(context, field, InputAttributes.StartHelp, helpValue.ResolvedValue);
    }

    /// <summary>Set field start required</summary>
    /// <param name="context">The action context</param>
    /// <param name="field">The target field</param>
    [ActionParameter("field", "The target field", valueTypes: [StringType])]
    [CaseBuildAction("SetFieldStartRequired", "Set field start required", "FieldInput", "FieldStart")]
    public void SetFieldStartRequired(CaseChangeActionContext context, object field) =>
        SetCaseFieldAttribute(context, field, InputAttributes.StartRequired, true);

    /// <summary>Set field start read only</summary>
    /// <param name="context">The action context</param>
    /// <param name="field">The target field</param>
    [ActionParameter("field", "The target field", valueTypes: [StringType])]
    [CaseBuildAction("SetFieldStartReadOnly", "Set field start read only", "FieldInput", "FieldStart")]
    public void SetFieldStartReadOnly(CaseChangeActionContext context, object field) =>
        SetCaseFieldAttribute(context, field, InputAttributes.StartReadOnly, true);

    /// <summary>Set field start format</summary>
    /// <param name="context">The action context</param>
    /// <param name="field">The target field</param>
    /// <param name="format">The format string</param>
    /// <remarks>see https://learn.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings</remarks>
    [ActionParameter("field", "The target field", valueTypes: [StringType])]
    [ActionParameter("format",
        "The format (https://learn.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings)",
        valueTypes: [StringType])]
    [CaseBuildAction("SetFieldStartFormat", "Set field start format", "FieldInput", "FieldStart")]
    public void SetFieldStartFormat(CaseChangeActionContext context, object field, object format)
    {
        var formatValue = GetActionValue<string>(context, format);
        if (formatValue == null || !formatValue.IsFulfilled)
        {
            return;
        }

        SetCaseFieldAttribute(context, field, InputAttributes.StartFormat, formatValue.ResolvedValue);
    }

    /// <summary>Set field start day date picker</summary>
    /// <param name="context">The action context</param>
    /// <param name="field">The target field</param>
    [ActionParameter("field", "The target field", valueTypes: [StringType])]
    [CaseBuildAction("SetFieldStartPickerOpenDay", "Set field start day date picker", "FieldInput", "FieldStart")]
    public void SetFieldStartPickerOpenDay(CaseChangeActionContext context, object field) =>
        SetCaseFieldAttribute(context, field, InputAttributes.StartPickerOpen, "day");

    /// <summary>Set field start month date picker</summary>
    /// <param name="context">The action context</param>
    /// <param name="field">The target field</param>
    [ActionParameter("field", "The target field", valueTypes: [StringType])]
    [CaseBuildAction("SetFieldStartPickerOpenMonth", "Set field start month date picker", "FieldInput", "FieldStart")]
    public void SetFieldStartPickerOpenMonth(CaseChangeActionContext context, object field) =>
        SetCaseFieldAttribute(context, field, InputAttributes.StartPickerOpen, "month");

    /// <summary>Set field start year date picker</summary>
    /// <param name="context">The action context</param>
    /// <param name="field">The target field</param>
    [ActionParameter("field", "The target field", valueTypes: [StringType])]
    [CaseBuildAction("SetFieldStartPickerOpenYear", "Set field start year date picker", "FieldInput", "FieldStart")]
    public void SetFieldStartPickerOpenYear(CaseChangeActionContext context, object field) =>
        SetCaseFieldAttribute(context, field, InputAttributes.StartPickerOpen, "year");

    /// <summary>Set field start picker date time</summary>
    /// <param name="context">The action context</param>
    /// <param name="field">The target field</param>
    [ActionParameter("field", "The target field", valueTypes: [StringType])]
    [CaseBuildAction("SetFieldStartPickerTypeDateTime", "Set field start picker date time", "FieldInput", "FieldStart")]
    public void SetFieldStartPickerTypeDateTime(CaseChangeActionContext context, object field) =>
        SetCaseFieldAttribute(context, field, InputAttributes.StartPickerType, "DateTimePicker");

    #endregion

    #region Case Field End

    /// <summary>Set field end label</summary>
    /// <param name="context">The action context</param>
    /// <param name="field">The target field</param>
    /// <param name="label">The field end label</param>
    [ActionParameter("field", "The target field", valueTypes: [StringType])]
    [ActionParameter("label", "The field end label", valueTypes: [StringType])]
    [CaseBuildAction("SetFieldEndLabel", "Set field end label", "FieldInput", "FieldEnd")]
    public void SetFieldEndLabel(CaseChangeActionContext context, object field, object label)
    {
        var labelValue = GetActionValue<string>(context, label);
        if (labelValue == null || !labelValue.IsFulfilled)
        {
            return;
        }

        SetCaseFieldAttribute(context, field, InputAttributes.EndLabel, labelValue.ResolvedValue);
    }

    /// <summary>Set field end help</summary>
    /// <param name="context">The action context</param>
    /// <param name="field">The target field</param>
    /// <param name="help">The field end help</param>
    [ActionParameter("field", "The target field", valueTypes: [StringType])]
    [ActionParameter("help", "The field end help", valueTypes: [StringType])]
    [CaseBuildAction("SetFieldEndHelp", "Set field end help", "FieldInput", "FieldEnd")]
    public void SetFieldEndHelp(CaseChangeActionContext context, object field, object help)
    {
        var helpValue = GetActionValue<string>(context, help);
        if (helpValue == null || !helpValue.IsFulfilled)
        {
            return;
        }

        SetCaseFieldAttribute(context, field, InputAttributes.EndHelp, helpValue.ResolvedValue);
    }

    /// <summary>Set field end required</summary>
    /// <param name="context">The action context</param>
    /// <param name="field">The target field</param>
    [ActionParameter("field", "The target field", valueTypes: [StringType])]
    [CaseBuildAction("SetFieldEndRequired", "Set field end required", "FieldInput", "FieldEnd")]
    public void SetFieldEndRequired(CaseChangeActionContext context, object field) =>
        SetCaseFieldAttribute(context, field, InputAttributes.EndRequired, true);

    /// <summary>Set field end read only</summary>
    /// <param name="context">The action context</param>
    /// <param name="field">The target field</param>
    [ActionParameter("field", "The target field", valueTypes: [StringType])]
    [CaseBuildAction("SetFieldEndReadOnly", "Set field end read only", "FieldInput", "FieldEnd")]
    public void SetFieldEndReadOnly(CaseChangeActionContext context, object field) =>
        SetCaseFieldAttribute(context, field, InputAttributes.EndReadOnly, true);

    /// <summary>Set field end format</summary>
    /// <param name="context">The action context</param>
    /// <param name="field">The target field</param>
    /// <param name="format">The format string</param>
    /// <remarks>see https://learn.microsoft.com/en-us/openspecs/windows_protocols/ms-lcid/a9eac961-e77d-41a6-90a5-ce1a8b0cdb9c</remarks>
    [ActionParameter("field", "The target field", valueTypes: [StringType])]
    [ActionParameter("format",
        "The format (https://learn.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings)",
        valueTypes: [StringType])]
    [CaseBuildAction("SetFieldEndFormat", "Set field end format", "FieldInput", "FieldEnd")]
    public void SetFieldEndFormat(CaseChangeActionContext context, object field, object format)
    {
        var formatValue = GetActionValue<string>(context, format);
        if (formatValue == null || !formatValue.IsFulfilled)
        {
            return;
        }

        SetCaseFieldAttribute(context, field, InputAttributes.EndFormat, formatValue.ResolvedValue);
    }

    /// <summary>Set field end day date picker</summary>
    /// <param name="context">The action context</param>
    /// <param name="field">The target field</param>
    [ActionParameter("field", "The target field", valueTypes: [StringType])]
    [CaseBuildAction("SetFieldEndPickerOpenDay", "Set field end day date picker", "FieldInput", "FieldEnd")]
    public void SetFieldEndPickerOpenDay(CaseChangeActionContext context, object field) =>
        SetCaseFieldAttribute(context, field, InputAttributes.EndPickerOpen, "day");

    /// <summary>Set field end month date picker</summary>
    /// <param name="context">The action context</param>
    /// <param name="field">The target field</param>
    [ActionParameter("field", "The target field", valueTypes: [StringType])]
    [CaseBuildAction("SetFieldEndPickerOpenMonth", "Set field end month date picker", "FieldInput", "FieldEnd")]
    public void SetFieldEndPickerOpenMonth(CaseChangeActionContext context, object field) =>
        SetCaseFieldAttribute(context, field, InputAttributes.EndPickerOpen, "month");

    /// <summary>Set field end year date picker</summary>
    /// <param name="context">The action context</param>
    /// <param name="field">The target field</param>
    [ActionParameter("field", "The target field", valueTypes: [StringType])]
    [CaseBuildAction("SetFieldEndPickerOpenYear", "Set field end year date picker", "FieldInput", "FieldEnd")]
    public void SetFieldEndPickerOpenYear(CaseChangeActionContext context, object field) =>
        SetCaseFieldAttribute(context, field, InputAttributes.EndPickerOpen, "year");

    /// <summary>Set field end time picker</summary>
    /// <param name="context">The action context</param>
    /// <param name="field">The target field</param>
    [ActionParameter("field", "The target field", valueTypes: [StringType])]
    [CaseBuildAction("SetFieldEndPickerTypeDateTime", "Set field end time picker", "FieldInput", "FieldEnd")]
    public void SetFieldEndPickerTypeDateTime(CaseChangeActionContext context, object field) =>
        SetCaseFieldAttribute(context, field, InputAttributes.EndPickerType, "DateTimePicker");

    #endregion

    #region Case Field Value

    /// <summary>Set field value label</summary>
    /// <param name="context">The action context</param>
    /// <param name="field">The target field</param>
    /// <param name="label">The label text</param>
    [ActionParameter("field", "The target field", valueTypes: [StringType])]
    [ActionParameter("label", "The label text", valueTypes: [StringType])]
    [CaseBuildAction("SetFieldValueLabel", "Set field value label", "FieldInput", "FieldValue")]
    public void SetFieldValueLabel(CaseChangeActionContext context, object field, object label)
    {
        var textValue = GetActionValue<string>(context, label);
        if (textValue == null || !textValue.IsFulfilled)
        {
            return;
        }

        SetCaseFieldAttribute(context, field, InputAttributes.ValueLabel, textValue.ResolvedValue);
    }

    /// <summary>Set field value adornment</summary>
    /// <param name="context">The action context</param>
    /// <param name="field">The target field</param>
    /// <param name="adornment">The adornment text</param>
    [ActionParameter("field", "The target field", valueTypes: [StringType])]
    [ActionParameter("adornment", "The adornment text", valueTypes: [StringType])]
    [CaseBuildAction("SetFieldValueAdornment", "Set field value adornment", "FieldInput", "FieldValue")]
    public void SetFieldValueAdornment(CaseChangeActionContext context, object field, object adornment)
    {
        var adornmentValue = GetActionValue<string>(context, adornment);
        if (adornmentValue == null || !adornmentValue.IsFulfilled)
        {
            return;
        }

        SetCaseFieldAttribute(context, field, InputAttributes.ValueAdornment, adornmentValue.ResolvedValue);
    }

    /// <summary>Set field value help</summary>
    /// <param name="context">The action context</param>
    /// <param name="field">The target field</param>
    /// <param name="help">The adornment text</param>
    [ActionParameter("field", "The target field", valueTypes: [StringType])]
    [ActionParameter("help", "The help text", valueTypes: [StringType])]
    [CaseBuildAction("SetFieldValueHelp", "Set field value help", "FieldInput", "FieldValue")]
    public void SetFieldValueHelp(CaseChangeActionContext context, object field, object help)
    {
        var helpValue = GetActionValue<string>(context, help);
        if (helpValue == null || !helpValue.IsFulfilled)
        {
            return;
        }

        SetCaseFieldAttribute(context, field, InputAttributes.ValueHelp, helpValue.ResolvedValue);
    }

    /// <summary>Set field value mask</summary>
    /// <param name="context">The action context</param>
    /// <param name="field">The target field</param>
    /// <param name="mask">The text mask</param>
    /// <remarks>see https://learn.microsoft.com/en-us/dotnet/api/system.windows.forms.maskedtextbox.mask</remarks>
    [ActionParameter("field", "The target field", valueTypes: [StringType])]
    [ActionParameter("mask",
        "The value mask (https://learn.microsoft.com/en-us/dotnet/api/system.windows.forms.maskedtextbox.mask)",
        valueTypes: [StringType])]
    [CaseBuildAction("SetFieldValueMask", "Set field value mask", "FieldInput", "FieldValue")]
    public void SetFieldValueMask(CaseChangeActionContext context, object field, object mask)
    {
        var maskValue = GetActionValue<string>(context, mask);
        if (maskValue == null || !maskValue.IsFulfilled)
        {
            return;
        }

        SetCaseFieldAttribute(context, field, InputAttributes.ValueMask, maskValue.ResolvedValue);
    }

    /// <summary>Set field value required</summary>
    /// <param name="context">The action context</param>
    /// <param name="field">The target field</param>
    [ActionParameter("field", "The target field", valueTypes: [StringType])]
    [CaseBuildAction("SetFieldValueRequired", "Set field value required", "FieldInput", "FieldValue")]
    public void SetFieldValueRequired(CaseChangeActionContext context, object field) =>
        SetCaseFieldAttribute(context, field, InputAttributes.ValueRequired, true);

    /// <summary>Set field value read only</summary>
    /// <param name="context">The action context</param>
    /// <param name="field">The target field</param>
    [ActionParameter("field", "The target field", valueTypes: [StringType])]
    [CaseBuildAction("SetFieldValueReadOnly", "Set field value read only", "FieldInput", "FieldValue")]
    public void SetFieldValueReadOnly(CaseChangeActionContext context, object field) =>
        SetCaseFieldAttribute(context, field, InputAttributes.ValueReadOnly, true);

    /// <summary>Set field value day date picker</summary>
    /// <param name="context">The action context</param>
    /// <param name="field">The target field</param>
    [ActionParameter("field", "The target field", valueTypes: [StringType])]
    [CaseBuildAction("SetFieldValuePickerOpenDay", "Set field value day date picker", "FieldInput", "FieldValue")]
    public void SetFieldValuePickerOpenDay(CaseChangeActionContext context, object field) =>
        SetCaseFieldAttribute(context, field, InputAttributes.ValuePickerOpen, "day");

    /// <summary>Set field value month date picker</summary>
    /// <param name="context">The action context</param>
    /// <param name="field">The target field</param>
    [ActionParameter("field", "The target field", valueTypes: [StringType])]
    [CaseBuildAction("SetFieldValuePickerOpenMonth", "Set field value month date picker", "FieldInput", "FieldValue")]
    public void SetFieldValuePickerOpenMonth(CaseChangeActionContext context, object field) =>
        SetCaseFieldAttribute(context, field, InputAttributes.ValuePickerOpen, "month");

    /// <summary>Set field value year date picker</summary>
    /// <param name="context">The action context</param>
    /// <param name="field">The target field</param>
    [ActionParameter("field", "The target field", valueTypes: [StringType])]
    [CaseBuildAction("SetFieldValuePickerOpenYear", "Set field value year date picker", "FieldInput", "FieldValue")]
    public void SetFieldValuePickerOpenYear(CaseChangeActionContext context, object field) =>
        SetCaseFieldAttribute(context, field, InputAttributes.ValuePickerOpen, "year");

    /// <summary>Set field value culture</summary>
    /// <param name="context">The action context</param>
    /// <param name="field">The target field</param>
    /// <param name="culture">The culture</param>
    /// <remarks>see https://learn.microsoft.com/en-us/openspecs/windows_protocols/ms-lcid/a9eac961-e77d-41a6-90a5-ce1a8b0cdb9c</remarks>
    [ActionParameter("field", "The target field", valueTypes: [StringType])]
    [ActionParameter("culture",
        "The culture (https://learn.microsoft.com/en-us/openspecs/windows_protocols/ms-lcid/a9eac961-e77d-41a6-90a5-ce1a8b0cdb9c)",
        valueTypes: [StringType])]
    [CaseBuildAction("SetFieldCulture", "Set field value culture", "FieldInput", "FieldValue")]
    public void SetFieldCulture(CaseChangeActionContext context, object field, object culture)
    {
        var cultureValue = GetActionValue<string>(context, culture);
        if (cultureValue == null || !cultureValue.IsFulfilled)
        {
            return;
        }

        SetCaseFieldAttribute(context, field, InputAttributes.Culture, cultureValue.ResolvedValue);
    }

    /// <summary>Set field minimum value</summary>
    /// <param name="context">The action context</param>
    /// <param name="field">The target field</param>
    /// <param name="minimum">The minimum value</param>
    [ActionParameter("field", "The target field", valueTypes: [StringType])]
    [ActionParameter("minimum", "The minimum value", valueTypes: [IntType, DecimalType, DateType])]
    [CaseBuildAction("SetFieldMinValue", "Set field minimum value", "FieldInput", "FieldValue")]
    public void SetFieldMinValue(CaseChangeActionContext context, object field, object minimum)
    {
        var intValue = GetActionValue<int>(context, minimum);
        if (intValue != null && intValue.IsFulfilled)
        {
            SetCaseFieldAttribute(context, field, InputAttributes.MinValue, intValue.ResolvedValue);
            return;
        }

        var decimalValue = GetActionValue<int>(context, minimum);
        if (decimalValue != null && decimalValue.IsFulfilled)
        {
            SetCaseFieldAttribute(context, field, InputAttributes.MinValue, decimalValue.ResolvedValue);
            return;
        }

        var dateTimeValue = GetActionValue<int>(context, minimum);
        if (dateTimeValue != null && dateTimeValue.IsFulfilled)
        {
            SetCaseFieldAttribute(context, field, InputAttributes.MinValue, dateTimeValue.ResolvedValue);
        }
    }

    /// <summary>Set field maximum value</summary>
    /// <param name="context">The action context</param>
    /// <param name="field">The target field</param>
    /// <param name="maximum">The maximum value</param>
    [ActionParameter("field", "The target field", valueTypes: [StringType])]
    [ActionParameter("maximum", "The maximum value", valueTypes: [IntType, DecimalType, DateType])]
    [CaseBuildAction("SetFieldMaxValue", "Set field maximum value", "FieldInput", "FieldValue")]
    public void SetFieldMaxValue(CaseChangeActionContext context, object field, object maximum)
    {
        var intValue = GetActionValue<int>(context, maximum);
        if (intValue != null && intValue.IsFulfilled)
        {
            SetCaseFieldAttribute(context, field, InputAttributes.MaxValue, intValue.ResolvedValue);
            return;
        }

        var decimalValue = GetActionValue<int>(context, maximum);
        if (decimalValue != null && decimalValue.IsFulfilled)
        {
            SetCaseFieldAttribute(context, field, InputAttributes.MaxValue, decimalValue.ResolvedValue);
            return;
        }

        var dateTimeValue = GetActionValue<int>(context, maximum);
        if (dateTimeValue != null && dateTimeValue.IsFulfilled)
        {
            SetCaseFieldAttribute(context, field, InputAttributes.MaxValue, dateTimeValue.ResolvedValue);
        }
    }

    /// <summary>Set field numeric step size</summary>
    /// <param name="context">The action context</param>
    /// <param name="field">The target field</param>
    /// <param name="stepSize">The step size</param>
    [ActionParameter("field", "The target field", valueTypes: [StringType])]
    [ActionParameter("stepSize", "The step size", valueTypes: [IntType])]
    [CaseBuildAction("SetFieldStepSize", "Set field numeric step size", "FieldInput", "FieldValue")]
    public void SetFieldStepSize(CaseChangeActionContext context, object field, object stepSize)
    {
        var stepSizeValue = GetActionValue<int>(context, stepSize);
        if (stepSizeValue == null || !stepSizeValue.IsFulfilled)
        {
            return;
        }

        SetCaseFieldAttribute(context, field, InputAttributes.StepSize, stepSizeValue.ResolvedValue);
    }

    /// <summary>Set field value format</summary>
    /// <param name="context">The action context</param>
    /// <param name="field">The target field</param>
    /// <param name="format">The text format</param>
    /// <remarks>see https://learn.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings</remarks>
    [ActionParameter("field", "The target field", valueTypes: [StringType])]
    [ActionParameter("format",
        "The value format (https://learn.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings)",
        valueTypes: [StringType])]
    [CaseBuildAction("SetFieldFormat", "Set field value format", "FieldInput", "FieldValue")]
    public void SetFieldFormat(CaseChangeActionContext context, object field, object format)
    {
        var formatValue = GetActionValue<string>(context, format);
        if (formatValue == null || !formatValue.IsFulfilled)
        {
            return;
        }

        SetCaseFieldAttribute(context, field, InputAttributes.Format, formatValue.ResolvedValue);
    }

    /// <summary>Set field text line count</summary>
    /// <param name="context">The action context</param>
    /// <param name="field">The target field</param>
    /// <param name="count">The line count</param>
    [ActionParameter("field", "The target field", valueTypes: [StringType])]
    [ActionParameter("count", "The line count", valueTypes: [IntType])]
    [CaseBuildAction("SetFieldLineCount", "Set field text line count", "FieldInput", "FieldValue")]
    public void SetFieldLineCount(CaseChangeActionContext context, object field, object count)
    {
        var countValue = GetActionValue<int>(context, count);
        if (countValue == null || !countValue.IsFulfilled)
        {
            return;
        }

        SetCaseFieldAttribute(context, field, InputAttributes.LineCount, countValue.ResolvedValue);
    }

    /// <summary>Set field maximum text length</summary>
    /// <param name="context">The action context</param>
    /// <param name="field">The target field</param>
    /// <param name="length">The maximum length</param>
    [ActionParameter("field", "The target field", valueTypes: [StringType])]
    [ActionParameter("length", "The length", valueTypes: [IntType])]
    [CaseBuildAction("SetFieldMaxLength", "Set field maximum text length", "FieldInput", "FieldValue")]
    public void SetFieldMaxLength(CaseChangeActionContext context, object field, object length)
    {
        var lengthValue = GetActionValue<int>(context, length);
        if (lengthValue == null || !lengthValue.IsFulfilled)
        {
            return;
        }

        SetCaseFieldAttribute(context, field, InputAttributes.MaxLength, lengthValue.ResolvedValue);
    }

    /// <summary>Set field boolean as checkbox</summary>
    /// <param name="context">The action context</param>
    /// <param name="field">The target field</param>
    [ActionParameter("field", "The target field", valueTypes: [StringType])]
    [CaseBuildAction("SetFieldCheck", "Set field boolean as checkbox", "FieldInput", "FieldValue")]
    public void SetFieldCheck(CaseChangeActionContext context, object field) =>
        SetCaseFieldAttribute(context, field, InputAttributes.Check, true);


    #endregion

    #region Case Field Attachment

    /// <summary>Set field without file attachments</summary>
    /// <param name="context">The action context</param>
    /// <param name="field">The target field</param>
    [ActionParameter("field", "The target field", valueTypes: [StringType])]
    [CaseBuildAction("SetFieldAttachmentNone", "Set field without file attachments", "FieldInput", "Field")]
    public void SetFieldAttachmentNone(CaseChangeActionContext context, object field) =>
        SetCaseFieldAttribute(context, field, InputAttributes.Attachment, "none");

    /// <summary>Set field optional file attachments</summary>
    /// <param name="context">The action context</param>
    /// <param name="field">The target field</param>
    [ActionParameter("field", "The target field", valueTypes: [StringType])]
    [CaseBuildAction("SetFieldAttachmentOptional", "Set field optional file attachments", "FieldInput", "Field")]
    public void SetFieldAttachmentOptional(CaseChangeActionContext context, object field) =>
        SetCaseFieldAttribute(context, field, InputAttributes.Attachment, "optional");

    /// <summary>Set field mandatory file attachments</summary>
    /// <param name="context">The action context</param>
    /// <param name="field">The target field</param>
    [ActionParameter("field", "The target field", valueTypes: [StringType])]
    [CaseBuildAction("SetFieldAttachmentMandatory", "Set field mandatory file attachments", "FieldInput", "Field")]
    public void SetFieldAttachmentMandatory(CaseChangeActionContext context, object field) =>
        SetCaseFieldAttribute(context, field, InputAttributes.Attachment, "mandatory");

    /// <summary>Set field attachments file extensions</summary>
    /// <param name="context">The action context</param>
    /// <param name="field">The target field</param>
    /// <param name="extensions">The file extensions</param>
    [ActionParameter("extensions", "The file extensions", valueTypes: [StringType])]
    [ActionParameter("field", "The target field", valueTypes: [StringType])]
    [CaseBuildAction("SetFieldAttachmentExtensions", "Set field attachments file extensions", "FieldInput", "Field")]
    public void SetFieldAttachmentExtensions(CaseChangeActionContext context, object field, object extensions)
    {
        var extensionsValue = GetActionValue<string>(context, extensions);
        if (extensionsValue == null || !extensionsValue.IsFulfilled)
        {
            return;
        }

        SetCaseFieldAttribute(context, field, InputAttributes.AttachmentExtensions, extensionsValue.ResolvedValue);
    }

    #endregion

    #region Current Case Field General

    /// <summary>Hide all inputs</summary>
    /// <param name="context">The action context</param>
    [CaseBuildAction("Hidden", "Hide all inputs", "FieldInput", "Field")]
    public void Hidden(CaseChangeActionContext context) =>
        HiddenField(context, context.CaseFieldName);

    /// <summary>Show description</summary>
    /// <param name="context">The action context</param>
    [CaseBuildAction("ShowDescription", "Show description", "FieldInput", "Field")]
    public void ShowDescription(CaseChangeActionContext context) =>
        ShowFieldDescription(context, context.CaseFieldName);

    #endregion

    #region Current Case Field Start

    /// <summary>Set start label</summary>
    /// <param name="context">The action context</param>
    /// <param name="label">The start label</param>
    [ActionParameter("label", "The start label", valueTypes: [StringType])]
    [CaseBuildAction("SetStartLabel", "Set start label", "FieldInput", "FieldStart")]
    public void SetStartLabel(CaseChangeActionContext context, object label) =>
        SetFieldStartLabel(context, context.CaseFieldName, label);

    /// <summary>Set start help</summary>
    /// <param name="context">The action context</param>
    /// <param name="help">The start help</param>
    [ActionParameter("help", "The start help", valueTypes: [StringType])]
    [CaseBuildAction("SetStartHelp", "Set start help", "FieldInput", "FieldStart")]
    public void SetStartHelp(CaseChangeActionContext context, object help) =>
        SetFieldStartHelp(context, context.CaseFieldName, help);

    /// <summary>Set start required</summary>
    /// <param name="context">The action context</param>
    [CaseBuildAction("SetStartRequired", "Set start required", "FieldInput", "FieldStart")]
    public void SetStartRequired(CaseChangeActionContext context) =>
        SetFieldStartRequired(context, context.CaseFieldName);

    /// <summary>Set  start to read only</summary>
    /// <param name="context">The action context</param>
    [CaseBuildAction("SetStartReadOnly", "Set start to read only", "FieldInput", "FieldStart")]
    public void SetStartReadOnly(CaseChangeActionContext context) =>
        SetFieldStartReadOnly(context, context.CaseFieldName);

    /// <summary>Set start format</summary>
    /// <param name="context">The action context</param>
    /// <param name="format">The format string</param>
    /// <remarks>see https://learn.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings</remarks>
    [ActionParameter("format",
        "The format (https://learn.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings)",
        valueTypes: [StringType])]
    [CaseBuildAction("SetStartFormat", "Set start format", "FieldInput", "FieldStart")]
    public void SetStartFormat(CaseChangeActionContext context, object format) =>
        SetFieldStartFormat(context, context.CaseFieldName, format);

    /// <summary>Set start day date picker</summary>
    /// <param name="context">The action context</param>
    [CaseBuildAction("SetStartPickerOpenDay", "Set start day date picker", "FieldInput", "FieldStart")]
    public void SetStartPickerOpenDay(CaseChangeActionContext context) =>
        SetFieldStartPickerOpenDay(context, context.CaseFieldName);

    /// <summary>Set start month date picker</summary>
    /// <param name="context">The action context</param>
    [CaseBuildAction("SetStartPickerOpenMonth", "Set start month date picker", "FieldInput", "FieldStart")]
    public void SetStartPickerOpenMonth(CaseChangeActionContext context) =>
        SetFieldStartPickerOpenMonth(context, context.CaseFieldName);

    /// <summary>Set start year date picker</summary>
    /// <param name="context">The action context</param>
    [CaseBuildAction("SetStartPickerOpenYear", "Set start year date picker", "FieldInput", "FieldStart")]
    public void SetStartPickerOpenYear(CaseChangeActionContext context) =>
        SetFieldStartPickerOpenYear(context, context.CaseFieldName);

    /// <summary>Set start time picker</summary>
    /// <param name="context">The action context</param>
    [CaseBuildAction("SetStartPickerTypeDateTime", "Set start time picker", "FieldInput", "FieldStart")]
    public void SetStartPickerTypeDateTime(CaseChangeActionContext context) =>
        SetFieldStartPickerTypeDateTime(context, context.CaseFieldName);

    #endregion

    #region Current Case Field End

    /// <summary>Set end label</summary>
    /// <param name="context">The action context</param>
    /// <param name="label">The end label</param>
    [ActionParameter("label", "The end label", valueTypes: [StringType])]
    [CaseBuildAction("SetEndLabel", "Set end label", "FieldInput", "FieldEnd")]
    public void SetEndLabel(CaseChangeActionContext context, object label) =>
        SetFieldEndLabel(context, context.CaseFieldName, label);

    /// <summary>Set end help</summary>
    /// <param name="context">The action context</param>
    /// <param name="help">The end help</param>
    [ActionParameter("help", "The end help", valueTypes: [StringType])]
    [CaseBuildAction("SetEndHelp", "Set end help", "FieldInput", "FieldEnd")]
    public void SetEndHelp(CaseChangeActionContext context, object help) =>
        SetFieldEndHelp(context, context.CaseFieldName, help);

    /// <summary>Set end required</summary>
    /// <param name="context">The action context</param>
    [CaseBuildAction("SetEndRequired", "Set end required", "FieldInput", "FieldEnd")]
    public void SetEndRequired(CaseChangeActionContext context) =>
        SetFieldEndRequired(context, context.CaseFieldName);

    /// <summary>Set end read only</summary>
    /// <param name="context">The action context</param>
    [CaseBuildAction("SetEndReadOnly", "Set end read only", "FieldInput", "FieldEnd")]
    public void SetEndReadOnly(CaseChangeActionContext context) =>
        SetFieldEndReadOnly(context, context.CaseFieldName);

    /// <summary>Set end format</summary>
    /// <param name="context">The action context</param>
    /// <param name="format">The format string</param>
    /// <remarks>see https://learn.microsoft.com/en-us/openspecs/windows_protocols/ms-lcid/a9eac961-e77d-41a6-90a5-ce1a8b0cdb9c</remarks>
    [ActionParameter("format",
        "The format (https://learn.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings)",
        valueTypes: [StringType])]
    [CaseBuildAction("SetEndFormat", "Set end format", "FieldInput", "FieldEnd")]
    public void SetEndFormat(CaseChangeActionContext context, object format) =>
        SetFieldEndFormat(context, context.CaseFieldName, format);

    /// <summary>Set end day date picker</summary>
    /// <param name="context">The action context</param>
    [CaseBuildAction("SetEndPickerOpenDay", "Set end day date picker", "FieldInput", "FieldEnd")]
    public void SetEndPickerOpenDay(CaseChangeActionContext context) =>
        SetFieldEndPickerOpenDay(context, context.CaseFieldName);

    /// <summary>Set end month date picker</summary>
    /// <param name="context">The action context</param>
    [CaseBuildAction("SetEndPickerOpenMonth", "Set end month date picker", "FieldInput", "FieldEnd")]
    public void SetEndPickerOpenMonth(CaseChangeActionContext context) =>
        SetFieldEndPickerOpenMonth(context, context.CaseFieldName);

    /// <summary>Set end year date picker</summary>
    /// <param name="context">The action context</param>
    [CaseBuildAction("SetEndPickerOpenYear", "Set end year date picker", "FieldInput", "FieldEnd")]
    public void SetEndPickerOpenYear(CaseChangeActionContext context) =>
        SetFieldEndPickerOpenYear(context, context.CaseFieldName);

    /// <summary>Set end time picker</summary>
    /// <param name="context">The action context</param>
    [CaseBuildAction("SetEndPickerTypeDateTime", "Set end time picker", "FieldInput", "FieldEnd")]
    public void SetEndPickerTypeDateTime(CaseChangeActionContext context) =>
        SetFieldEndPickerTypeDateTime(context, context.CaseFieldName);

    #endregion

    #region Current Case Field Value

    /// <summary>Set value label</summary>
    /// <param name="context">The action context</param>
    /// <param name="label">The label text</param>
    [ActionParameter("label", "The label text", valueTypes: [StringType])]
    [CaseBuildAction("SetValueLabel", "Set value label", "FieldInput", "FieldValue")]
    public void SetValueLabel(CaseChangeActionContext context, object label) =>
        SetFieldValueLabel(context, context.CaseFieldName, label);

    /// <summary>Set value adornment</summary>
    /// <param name="context">The action context</param>
    /// <param name="adornment">The adornment text</param>
    [ActionParameter("adornment", "The adornment text", valueTypes: [StringType])]
    [CaseBuildAction("SetValueAdornment", "Set value adornment", "FieldInput", "FieldValue")]
    public void SetValueAdornment(CaseChangeActionContext context, object adornment) =>
        SetFieldValueAdornment(context, context.CaseFieldName, adornment);

    /// <summary>Set value help</summary>
    /// <param name="context">The action context</param>
    /// <param name="help">The adornment text</param>
    [ActionParameter("help", "The label text", valueTypes: [StringType])]
    [CaseBuildAction("SetValueHelp", "Set value help", "FieldInput", "FieldValue")]
    public void SetValueHelp(CaseChangeActionContext context, object help) =>
        SetFieldValueHelp(context, context.CaseFieldName, help);

    /// <summary>Set value mask</summary>
    /// <param name="context">The action context</param>
    /// <param name="mask">The text mask</param>
    /// <remarks>see https://learn.microsoft.com/en-us/dotnet/api/system.windows.forms.maskedtextbox.mask</remarks>
    [ActionParameter("mask",
        "The value mask (https://learn.microsoft.com/en-us/dotnet/api/system.windows.forms.maskedtextbox.mask)",
        valueTypes: [StringType])]
    [CaseBuildAction("SetValueMask", "Set value mask", "FieldInput", "FieldValue")]
    public void SetValueMask(CaseChangeActionContext context, object mask) =>
        SetFieldValueMask(context, context.CaseFieldName, mask);

    /// <summary>Set value required</summary>
    /// <param name="context">The action context</param>
    [CaseBuildAction("SetValueRequired", "Set value required", "FieldInput", "FieldValue")]
    public void SetValueRequired(CaseChangeActionContext context) =>
        SetFieldValueRequired(context, context.CaseFieldName);

    /// <summary>Set value read only</summary>
    /// <param name="context">The action context</param>
    [CaseBuildAction("SetValueReadOnly", "Set value read only", "FieldInput", "FieldValue")]
    public void SetValueReadOnly(CaseChangeActionContext context) =>
        SetFieldValueReadOnly(context, context.CaseFieldName);

    /// <summary>Set value day date picker</summary>
    /// <param name="context">The action context</param>
    [CaseBuildAction("SetValueDayPicker", "Set value day date picker", "FieldInput", "FieldValue")]
    public void SetValueDayPicker(CaseChangeActionContext context) =>
        SetFieldValuePickerOpenDay(context, context.CaseFieldName);

    /// <summary>Set value month date picker</summary>
    /// <param name="context">The action context</param>
    [CaseBuildAction("SetValueMonthPicker", "Set value month date picker", "FieldInput", "FieldValue")]
    public void SetValueMonthPicker(CaseChangeActionContext context) =>
        SetFieldValuePickerOpenMonth(context, context.CaseFieldName);

    /// <summary>Set value year date picker</summary>
    /// <param name="context">The action context</param>
    [CaseBuildAction("SetValueYearPicker", "Set value year date picker", "FieldInput", "FieldValue")]
    public void SetValueYearPicker(CaseChangeActionContext context) =>
        SetFieldValuePickerOpenYear(context, context.CaseFieldName);

    /// <summary>Set value culture</summary>
    /// <param name="context">The action context</param>
    /// <param name="culture">The culture</param>
    /// <remarks>see https://learn.microsoft.com/en-us/openspecs/windows_protocols/ms-lcid/a9eac961-e77d-41a6-90a5-ce1a8b0cdb9c</remarks>
    [ActionParameter("culture",
        "The culture (https://learn.microsoft.com/en-us/openspecs/windows_protocols/ms-lcid/a9eac961-e77d-41a6-90a5-ce1a8b0cdb9c)",
        valueTypes: [StringType])]
    [CaseBuildAction("SetCulture", "Set value culture", "FieldInput", "FieldValue")]
    public void SetCulture(CaseChangeActionContext context, object culture) =>
        SetFieldCulture(context, context.CaseFieldName, culture);

    /// <summary>Set minimum value</summary>
    /// <param name="context">The action context</param>
    /// <param name="minimum">The minimum value</param>
    [ActionParameter("minimum", "The minimum value",
        valueTypes: [IntType, DecimalType, DateType])]
    [CaseBuildAction("SetMinValue", "Set minimum value", "FieldInput", "FieldValue")]
    public void SetMinValue(CaseChangeActionContext context, object minimum) =>
        SetFieldMinValue(context, context.CaseFieldName, minimum);

    /// <summary>Set maximum value</summary>
    /// <param name="context">The action context</param>
    /// <param name="maximum">The maximum value</param>
    [ActionParameter("maximum", "The maximum value",
        valueTypes: [IntType, DecimalType, DateType])]
    [CaseBuildAction("SetMaxValue", "Set maximum value", "FieldInput", "FieldValue")]
    public void SetMaxValue(CaseChangeActionContext context, object maximum) =>
        SetFieldMaxValue(context, context.CaseFieldName, maximum);

    /// <summary>Set numeric step size</summary>
    /// <param name="context">The action context</param>
    /// <param name="stepSize">The step size</param>
    [ActionParameter("stepSize", "The step size", valueTypes: [IntType])]
    [CaseBuildAction("SetStepSize", "Set numeric step size", "FieldInput", "FieldValue")]
    public void SetStepSize(CaseChangeActionContext context, object stepSize) =>
        SetFieldStepSize(context, context.CaseFieldName, stepSize);

    /// <summary>Set value format</summary>
    /// <param name="context">The action context</param>
    /// <param name="format">The text format</param>
    /// <remarks>see https://learn.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings</remarks>
    [ActionParameter("format",
        "The value format (https://learn.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings)",
        valueTypes: [StringType])]
    [CaseBuildAction("SetFormat", "Set value format", "FieldInput", "FieldValue")]
    public void SetFormat(CaseChangeActionContext context, object format) =>
        SetFieldFormat(context, context.CaseFieldName, format);

    /// <summary>Set text line count</summary>
    /// <param name="context">The action context</param>
    /// <param name="count">The line count</param>
    [ActionParameter("count", "The line count", valueTypes: [IntType])]
    [CaseBuildAction("SetLineCount", "Set text line count", "FieldInput", "FieldValue")]
    public void SetLineCount(CaseChangeActionContext context, object count) =>
        SetFieldLineCount(context, context.CaseFieldName, count);

    /// <summary>Set maximum text length</summary>
    /// <param name="context">The action context</param>
    /// <param name="length">The maximum length</param>
    [ActionParameter("length", "The length", valueTypes: [IntType])]
    [CaseBuildAction("SetMaxLength", "Set maximum text length", "FieldInput", "FieldValue")]
    public void SetMaxLength(CaseChangeActionContext context, object length) =>
        SetFieldMaxLength(context, context.CaseFieldName, length);

    /// <summary>Set boolean as checkbox</summary>
    /// <param name="context">The action context</param>
    [CaseBuildAction("SetCheck", "Set boolean as checkbox", "FieldInput", "FieldValue")]
    public void SetCheck(CaseChangeActionContext context) =>
        SetFieldCheck(context, context.CaseFieldName);

    #endregion

    #region Current Case Field Attachment

    /// <summary>Set without file attachments</summary>
    /// <param name="context">The action context</param>
    [CaseBuildAction("SetAttachmentNone", "Set without file attachments", "FieldInput", "Field")]
    public void SetAttachmentNone(CaseChangeActionContext context) =>
        SetFieldAttachmentNone(context, context.CaseFieldName);

    /// <summary>Set optional file attachments</summary>
    /// <param name="context">The action context</param>
    [CaseBuildAction("SeAttachmentOptional", "Set optional file attachments", "FieldInput", "Field")]
    public void SeAttachmentOptional(CaseChangeActionContext context) =>
        SetFieldAttachmentOptional(context, context.CaseFieldName);

    /// <summary>Set mandatory file attachments</summary>
    /// <param name="context">The action context</param>
    [CaseBuildAction("SetAttachmentMandatory", "Set mandatory file attachments", "FieldInput", "Field")]
    public void SetAttachmentMandatory(CaseChangeActionContext context) =>
        SetFieldAttachmentMandatory(context, context.CaseFieldName);

    /// <summary>Set attachments file extensions</summary>
    /// <param name="context">The action context</param>
    /// <param name="extensions">The file extensions</param>
    [ActionParameter("extensions", "The file extensions", valueTypes: [StringType])]
    [CaseBuildAction("SetAttachmentExtensions", "Set attachments file extensions", "FieldInput", "Field")]
    public void SetAttachmentExtensions(CaseChangeActionContext context, object extensions) =>
        SetFieldAttachmentExtensions(context, context.CaseFieldName, extensions);

    #endregion

    private static void SetCaseAttribute(CaseChangeActionContext context, string attribute, object value) =>
        context.Function.SetCaseAttribute(context.Function.CaseName, attribute, value);

    private static void SetCaseFieldAttribute(CaseChangeActionContext context, object fieldName,
        string attribute, object value)
    {
        var caseFieldName = fieldName as string;
        if (string.IsNullOrWhiteSpace(caseFieldName))
        {
            context.Function.LogError($"Invalid case field {fieldName} for attribute {attribute}={value}");
        }
        context.Function.SetCaseFieldAttribute(caseFieldName, attribute, value);
    }
}
