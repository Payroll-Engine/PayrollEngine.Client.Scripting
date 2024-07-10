/* CaseBuildActions */

using System;

namespace PayrollEngine.Client.Scripting.Function;

/// <summary>Extension methods for the case change actions</summary>
[ActionProvider(Function.DefaultActionNamespace, typeof(CaseChangeFunction))]
public class CaseBuildActions : CaseChangeActionsBase
{
    /// <summary>Ensure lower limits</summary>
    /// <param name="context">The action context</param>
    /// <param name="minimum">The minimum value</param>
    [ActionParameter("minimum", "The minimum value",
        valueTypes: [IntType, DecimalType, DateType])]
    [CaseBuildAction("MinLimit", "Ensure lower limits", "FieldValue")]
    public void MinLimit(CaseChangeActionContext context, object minimum)
    {
        var valueType = context.Function.GetValueType(context.CaseFieldName).ToActionValueType();
        if (valueType == ActionValueType.Integer)
        {
            // integer
            var sourceValue = GetActionValue<int>(context);
            var minimumValue = GetActionValue<int?>(context, minimum);
            if (sourceValue == null || minimumValue == null || !minimumValue.IsFulfilled)
            {
                return;
            }
            if (sourceValue.ResolvedValue < minimumValue.ResolvedValue)
            {
                context.Function.SetValue(context.CaseFieldName, minimumValue.ResolvedValue);
            }
        }
        else if (valueType == ActionValueType.Decimal)
        {
            // decimal
            var sourceValue = GetActionValue<decimal>(context);
            var minimumValue = GetActionValue<decimal?>(context, minimum);
            if (sourceValue == null || minimumValue == null || !minimumValue.IsFulfilled)
            {
                return;
            }
            if (sourceValue.ResolvedValue < minimumValue.ResolvedValue)
            {
                context.Function.SetValue(context.CaseFieldName, minimumValue.ResolvedValue);
            }
        }
        else if (valueType == ActionValueType.DateTime)
        {
            // date time
            var sourceValue = GetActionValue<DateTime>(context);
            var minimumValue = GetActionValue<DateTime?>(context, minimum);
            if (sourceValue == null || minimumValue?.ResolvedValue == null || !minimumValue.IsFulfilled)
            {
                return;
            }
            if (sourceValue.ResolvedValue != minimumValue.ResolvedValue)
            {
                context.Function.SetValue(context.CaseFieldName, minimumValue.ResolvedValue);
            }
        }
    }

    /// <summary>Ensure higher limits</summary>
    /// <param name="context">The action context</param>
    /// <param name="maximum">The maximum value</param>
    [ActionParameter("maximum", "The maximum value",
        valueTypes: [IntType, DecimalType, DateType])]
    [CaseBuildAction("MaxLimit", "Ensure higher limits", "FieldValue")]
    public void MaxLimit(CaseChangeActionContext context, object maximum)
    {
        var valueType = context.Function.GetValueType(context.CaseFieldName).ToActionValueType();
        if (valueType == ActionValueType.Integer)
        {
            // integer
            var sourceValue = GetActionValue<int>(context);
            var maximumValue = GetActionValue<int?>(context, maximum);
            if (sourceValue == null || maximumValue == null || !maximumValue.IsFulfilled)
            {
                return;
            }
            if (sourceValue.ResolvedValue > maximumValue.ResolvedValue)
            {
                context.Function.SetValue(context.CaseFieldName, maximumValue.ResolvedValue);
            }
        }
        else if (valueType == ActionValueType.Decimal)
        {
            // decimal
            var sourceValue = GetActionValue<decimal>(context);
            var maximumValue = GetActionValue<decimal?>(context, maximum);
            if (sourceValue == null || maximumValue == null || !maximumValue.IsFulfilled)
            {
                return;
            }
            if (sourceValue.ResolvedValue > maximumValue.ResolvedValue)
            {
                context.Function.SetValue(context.CaseFieldName, maximumValue.ResolvedValue);
            }
        }
        else if (valueType == ActionValueType.DateTime)
        {
            // date time
            var sourceValue = GetActionValue<DateTime>(context);
            var maximumValue = GetActionValue<DateTime?>(context, maximum);
            if (sourceValue == null || maximumValue?.ResolvedValue == null || !maximumValue.IsFulfilled)
            {
                return;
            }
            if (sourceValue.ResolvedValue != maximumValue.ResolvedValue)
            {
                context.Function.SetValue(context.CaseFieldName, maximumValue.ResolvedValue);
            }
        }
    }

    /// <summary>Ensure value range</summary>
    /// <param name="context">The action context</param>
    /// <param name="minimum">The minimum value</param>
    /// <param name="maximum">The maximum value</param>
    [ActionParameter("minimum", "The minimum value",
        valueTypes: [IntType, DecimalType, DateType])]
    [ActionParameter("maximum", "The maximum value",
        valueTypes: [IntType, DecimalType, DateType])]
    [CaseBuildAction("Limit", "Ensure value range", "FieldValue")]
    public void Limit(CaseChangeActionContext context, object minimum, object maximum)
    {
        MinLimit(context, minimum);
        MaxLimit(context, maximum);
    }

    /// <summary>Set the case change value</summary>
    /// <param name="context">The action context</param>
    /// <param name="value">The value to set</param>
    [ActionParameter("value", "The value to set",
        valueTypes: [StringType, BooleanType, IntType, DecimalType, DateType])]
    [CaseBuildAction("SetValue", "Set the case change value")]
    public void SetValue(CaseChangeActionContext context, object value) =>
        SetFieldValue(context, ActionCaseValueBase.ToCaseChangeReference(context.CaseFieldName), value);

    /// <summary>Set the case change start date</summary>
    /// <param name="context">The action context</param>
    /// <param name="start">The case start date to set</param>
    [ActionParameter("start", "The case start date to set",
        valueTypes: [DateType])]
    [CaseBuildAction("SetStart", "Set the case change start date", "FieldStart")]
    public void SetStart(CaseChangeActionContext context, object start) =>
        SetFieldStart(context, ActionCaseValueBase.ToCaseChangeStartReference(context.CaseFieldName), start);

    /// <summary>Set the case change end date</summary>
    /// <param name="context">The action context</param>
    /// <param name="end">The case end date to set</param>
    [ActionParameter("end", "The case end date to set",
        valueTypes: [DateType])]
    [CaseBuildAction("SetEnd", "Set the case change end date", "FieldEnd")]
    public void SetEnd(CaseChangeActionContext context, object end) =>
        SetFieldEnd(context, ActionCaseValueBase.ToCaseChangeStartReference(context.CaseFieldName), end);

    /// <summary>Set the case change start end date</summary>
    /// <param name="context">The action context</param>
    /// <param name="start">The date to set</param>
    /// <param name="end">The date to set</param>
    [ActionParameter("start", "The case start date to set",
        valueTypes: [DateType])]
    [ActionParameter("end", "The case end date to set",
        valueTypes: [DateType])]
    [CaseBuildAction("SetStartEnd", "Set the case change start end end date", "FieldStart", "FieldEnd")]
    public void SetStartEnd(CaseChangeActionContext context, object start, object end) =>
        SetFieldStartEnd(context, ActionCaseValueBase.ToCaseChangeStartReference(context.CaseFieldName), start, end);

    /// <summary>Set the case change field attribute value</summary>
    /// <param name="context">The action context</param>
    /// <param name="attributeName">The attribute name</param>
    /// <param name="value">The value to set (null=remove)</param>
    [ActionParameter("attributeName", "The attribute name",
        valueTypes: [StringType])]
    [ActionParameter("value", "The value to set (null=remove)",
        valueTypes: [StringType, BooleanType, IntType, DecimalType, DateType])]
    [CaseBuildAction("SetAttribute", "Set the case change field attribute value", "Field")]
    public void SetAttribute(CaseChangeActionContext context, object attributeName, object value) =>
        SetFieldAttribute(context, ActionCaseValueBase.ToCaseChangeFieldAttributeReference(context.CaseFieldName), attributeName, value);

    /// <summary>Set the case value attribute value</summary>
    /// <param name="context">The action context</param>
    /// <param name="attributeName">The attribute name</param>
    /// <param name="value">The value to set (null=remove)</param>
    [ActionParameter("attributeName", "The attribute name",
        valueTypes: [StringType])]
    [ActionParameter("value", "The value to set (null=remove)",
        valueTypes: [StringType, BooleanType, IntType, DecimalType, DateType])]
    [CaseBuildAction("SetValueAttribute", "Set the case value attribute value", "FieldValue")]
    public void SetValueAttribute(CaseChangeActionContext context, object attributeName, object value) =>
        SetFieldValueAttribute(context, ActionCaseValueBase.ToCaseChangeValueAttributeReference(context.CaseFieldName), attributeName, value);

    /// <summary>Set the case change field value</summary>
    /// <param name="context">The action context</param>
    /// <param name="target">The target case field</param>
    /// <param name="value">The value to set</param>
    [ActionParameter("target", "The target case field",
        valueTypes: [StringType, BooleanType, IntType, DecimalType, DateType])]
    [ActionParameter("value", "The value to set",
        valueTypes: [StringType, BooleanType, IntType, DecimalType, DateType])]
    [CaseBuildAction("SetFieldValue", "Set the case change field value", "FieldValue")]
    public void SetFieldValue(CaseChangeActionContext context, object target, object value)
    {
        // target, must be a case change reference
        var typeValue = GetActionValue<object>(context, target);
        if (typeValue == null || !typeValue.IsCaseChangeReference)
        {
            context.AddIssue($"Invalid target field reference {target}");
            return;
        }

        // set value by type
        var valueType = typeValue.GetValueType();
        switch (valueType)
        {
            case ActionValueType.String:
                SetValue<string>(context, value, typeValue);
                break;
            case ActionValueType.Integer:
                SetValue<int?>(context, value, typeValue);
                break;
            case ActionValueType.Decimal:
                SetValue<decimal?>(context, value, typeValue);
                break;
            case ActionValueType.Boolean:
                SetValue<bool?>(context, value, typeValue);
                break;
            case ActionValueType.DateTime:
                SetValue<DateTime?>(context, value, typeValue);
                break;
            case ActionValueType.TimeSpan:
                SetValue<TimeSpan?>(context, value, typeValue);
                break;
        }
    }

    private static void SetValue<TValue>(CaseChangeActionContext context, object value, ActionCaseChangeValue<object> typeValue)
    {
        var targetValue = GetActionValue<TValue>(context, value);
        if (targetValue == null || !targetValue.IsFulfilled)
        {
            return;
        }
        context.Function.SetValue(typeValue.ReferenceField, targetValue.ResolvedValue);
    }

    /// <summary>Set the case field change start date</summary>
    /// <param name="context">The action context</param>
    /// <param name="target">The target case field</param>
    /// <param name="start">The start date to set</param>
    [ActionParameter("target", "The target case field",
        valueTypes: [DateType])]
    [ActionParameter("start", "The start date to set",
        valueTypes: [DateType])]
    [CaseBuildAction("SetFieldStart", "Set the case field change start date", "FieldStart")]
    public void SetFieldStart(CaseChangeActionContext context, object target, object start)
    {
        // target, must be a case change reference
        var targetValue = GetActionValue<DateTime?>(context, target);
        if (targetValue == null || !targetValue.IsCaseChangeReference)
        {
            context.AddIssue($"Invalid target field reference {target}");
            return;
        }

        // start
        var startValue = GetActionValue<DateTime?>(context, start);
        if (startValue?.ResolvedValue == null || !startValue.IsFulfilled)
        {
            return;
        }

        // update, ignore start after end
        var end = context.Function.GetEnd(targetValue.ReferenceField);
        if (end == null || startValue.ResolvedValue < end.Value)
        {
            context.Function.SetStart(targetValue.ReferenceField, startValue.ResolvedValue);
        }
    }

    /// <summary>Set the case field change end date</summary>
    /// <param name="context">The action context</param>
    /// <param name="target">The target case field</param>
    /// <param name="end">The end date to set</param>
    [ActionParameter("target", "The target case field",
        valueTypes: [DateType])]
    [ActionParameter("end", "The end date to set",
        valueTypes: [DateType])]
    [CaseBuildAction("SetFieldEnd", "Set the case field change end date", "FieldEnd")]
    public void SetFieldEnd(CaseChangeActionContext context, object target, object end)
    {
        // target, must be a case change reference
        var targetValue = GetActionValue<DateTime?>(context, target);
        if (targetValue == null || !targetValue.IsCaseChangeReference)
        {
            context.AddIssue($"Invalid target field reference {target}");
            return;
        }

        // end
        var endValue = GetActionValue<DateTime?>(context, end);
        if (endValue?.ResolvedValue == null || !endValue.IsFulfilled)
        {
            return;
        }

        // update, ignore end before start
        var start = context.Function.GetStart(targetValue.ReferenceField);
        if (start == null || endValue.ResolvedValue > start.Value)
        {
            context.Function.SetEnd(targetValue.ReferenceField, endValue.ResolvedValue);
        }
    }

    /// <summary>Set the case field change start and end date</summary>
    /// <param name="context">The action context</param>
    /// <param name="target">The target case field</param>
    /// <param name="start">The date to set</param>
    /// <param name="end">The date to set</param>
    [ActionParameter("target", "The target case field",
        valueTypes: [DateType])]
    [ActionParameter("start", "The start date to set",
        valueTypes: [DateType])]
    [ActionParameter("end", "The end date to set",
        valueTypes: [DateType])]
    [CaseBuildAction("SetFieldStartEnd", "Set the case field change start and end date", "FieldStart", "FieldEnd")]
    public void SetFieldStartEnd(CaseChangeActionContext context, object target, object start, object end)
    {
        // target, must be a case change reference
        var targetValue = GetActionValue<DateTime?>(context, target);
        if (!targetValue.IsCaseChangeReference)
        {
            context.AddIssue($"Invalid target field reference {target}");
            return;
        }

        // start
        var startValue = GetActionValue<DateTime?>(context, start);
        if (startValue?.ResolvedValue == null || !startValue.IsFulfilled)
        {
            return;
        }

        // end
        var endValue = GetActionValue<DateTime?>(context, end);
        if (endValue?.ResolvedValue == null || !endValue.IsFulfilled)
        {
            return;
        }

        // update
        context.Function.SetStart(targetValue.ReferenceField,
            Date.Min(startValue.ResolvedValue.Value, endValue.ResolvedValue.Value));
        context.Function.SetEnd(targetValue.ReferenceField,
            Date.Max(startValue.ResolvedValue.Value, endValue.ResolvedValue.Value));
    }

    /// <summary>Set the case field attribute value</summary>
    /// <param name="context">The action context</param>
    /// <param name="target">The target attribute</param>
    /// <param name="attributeName">The attribute name</param>
    /// <param name="value">The value to set (null=remove)</param>
    [ActionParameter("target", "The target attribute",
        valueTypes: [StringType, BooleanType, IntType, DecimalType, DateType])]
    [ActionParameter("attributeName", "The attribute name",
        valueTypes: [StringType])]
    [ActionParameter("value", "The value to set (null=remove)",
        valueTypes: [StringType, BooleanType, IntType, DecimalType, DateType])]
    [CaseBuildAction("SetFieldAttribute", "Set the case field attribute value", "Field")]
    public void SetFieldAttribute(CaseChangeActionContext context, object target, object attributeName, object value)
    {
        // target, must be a case field attribute
        var targetValue = GetActionValue<object>(context, target);
        if (targetValue == null || !targetValue.IsCaseFieldAttribute)
        {
            context.AddIssue($"Invalid target attribute reference {target}");
            return;
        }

        // attribute
        var attributeNameValue = GetActionValue<string>(context, attributeName);
        if (string.IsNullOrWhiteSpace(attributeNameValue.ResolvedValue))
        {
            context.AddIssue($"Invalid attribute {attributeName}");
            return;
        }

        // remove attribute
        var valueValue = GetActionValue<object>(context, value);
        if (valueValue?.ResolvedValue == null)
        {
            context.Function.RemoveCaseFieldAttribute(targetValue.ReferenceField, attributeNameValue.ResolvedValue);
            return;
        }

        // set attribute
        context.Function.SetCaseFieldAttribute(targetValue.ReferenceField,
            attributeNameValue.ResolvedValue, valueValue.ResolvedValue);
    }

    /// <summary>Set the case value attribute value</summary>
    /// <param name="context">The action context</param>
    /// <param name="target">The target attribute</param>
    /// <param name="attributeName">The attribute name</param>
    /// <param name="value">The value to set (null=remove)</param>
    [ActionParameter("target", "The target attribute",
        valueTypes: [StringType, BooleanType, IntType, DecimalType, DateType])]
    [ActionParameter("attributeName", "The attribute name",
        valueTypes: [StringType])]
    [ActionParameter("value", "The value to set (null=remove)",
        valueTypes: [StringType, BooleanType, IntType, DecimalType, DateType])]
    [CaseBuildAction("SetFieldValueAttribute", "Set the case value attribute value", "FieldValue")]
    public void SetFieldValueAttribute(CaseChangeActionContext context, object target, object attributeName, object value)
    {
        // target, must be a case field attribute
        var targetValue = GetActionValue<object>(context, target);
        if (targetValue == null || !targetValue.IsCaseValueAttribute)
        {
            context.AddIssue($"Invalid target attribute reference {target}");
            return;
        }

        // attribute
        var attributeNameValue = GetActionValue<string>(context, attributeName);
        if (string.IsNullOrWhiteSpace(attributeNameValue.ResolvedValue))
        {
            context.AddIssue($"Invalid attribute {attributeName}");
            return;
        }

        // remove attribute
        var valueValue = GetActionValue<object>(context, value);
        if (valueValue?.ResolvedValue == null)
        {
            context.Function.RemoveCaseValueAttribute(targetValue.ReferenceField, attributeNameValue.ResolvedValue);
            return;
        }

        // set attribute
        context.Function.SetCaseValueAttribute(targetValue.ReferenceField,
            attributeNameValue.ResolvedValue, valueValue.ResolvedValue);
    }

    /// <summary>Write log entry</summary>
    /// <param name="context">The action context</param>
    /// <param name="message">The log message</param>
    [ActionParameter("message", "The log message",
        valueTypes: [StringType])]
    [CaseBuildAction("Log", "Write log entry", "Tool")]
    public void Log(CaseChangeActionContext context, string message) =>
        context.Function.LogInformation(message);

    /// <summary>Add a user task</summary>
    /// <param name="context">The action context</param>
    /// <param name="name">The task name</param>
    /// <param name="instruction">The task instruction</param>
    /// <param name="scheduleDate">The task schedule date</param>
    /// <param name="category">The task category</param>
    [ActionParameter("name", "The task name",
        valueTypes: [StringType])]
    [ActionParameter("instruction", "The task instruction",
        valueTypes: [StringType])]
    [ActionParameter("scheduleDate", "The task schedule date",
        valueTypes: [StringType])]
    [ActionParameter("category", "The task category",
        valueTypes: [StringType])]
    [CaseBuildAction("AddTask", "Add a user task", "Tool")]
    public void AddTask(CaseChangeActionContext context, string name, string instruction,
        DateTime scheduleDate, string category = null) =>
        context.Function.AddTask(name, instruction, scheduleDate, category);
}
