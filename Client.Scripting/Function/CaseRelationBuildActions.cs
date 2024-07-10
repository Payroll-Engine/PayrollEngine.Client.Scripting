/* CaseRelationBuildActions */

using System;
using System.Linq;

namespace PayrollEngine.Client.Scripting.Function;

/// <summary>Extension methods for the case change actions</summary>
[ActionProvider(Function.DefaultActionNamespace, typeof(CaseRelationFunction))]
public class CaseRelationBuildActions : CaseRelationActionsBase
{
    /// <summary>Set the case relation target field value</summary>
    /// <param name="context">The action context</param>
    /// <param name="target">The case field on the target case</param>
    /// <param name="value">The value to set</param>
    [ActionParameter("target", "The case field on the target case",
        valueTypes: [StringType, BooleanType, IntType, DecimalType, DateType])]
    [ActionParameter("value", "The value to set",
        valueTypes: [StringType, BooleanType, IntType, DecimalType, DateType])]
    [CaseRelationBuildAction("SetTargetFieldValue", "Set the case relation target field value", "FieldValue")]
    public void SetTargetFieldValue(CaseRelationActionContext context, object target, object value)
    {
        // target, must be a case change reference
        var targetValue = GetActionValue<object>(context, target);
        if (!targetValue.IsCaseChangeReference)
        {
            context.AddIssue($"Invalid target field {target}");
            return;
        }

        // test for known field
        if (!context.Function.GetTargetFieldNames().Any(x => string.Equals(x, targetValue.ReferenceField)))
        {
            context.AddIssue($"Unknown target field {target}");
            return;
        }

        // set value by type
        var valueType = targetValue.GetValueType();
        switch (valueType)
        {
            case ActionValueType.String:
                SetValue<string>(context, value);
                break;
            case ActionValueType.Boolean:
                SetValue<bool?>(context, value);
                break;
            case ActionValueType.Integer:
                SetValue<int?>(context, value);
                break;
            case ActionValueType.Decimal:
                SetValue<decimal?>(context, value);
                break;
            case ActionValueType.DateTime:
                SetValue<DateTime?>(context, value);
                break;
            case ActionValueType.TimeSpan:
                SetValue<TimeSpan?>(context, value);
                break;
        }
    }

    private static void SetValue<TValue>(CaseRelationActionContext context, object value)
    {
        var targetStringValue = GetActionValue<TValue>(context, value);
        if (targetStringValue == null || !targetStringValue.IsFulfilled)
        {
            return;
        }

        context.Function.SetTargetValue(targetStringValue.ReferenceField, targetStringValue.ResolvedValue);
    }

    /// <summary>Set the case relation target field start date</summary>
    /// <param name="context">The action context</param>
    /// <param name="target">The case field on the target case</param>
    /// <param name="start">The start date to set</param>
    [ActionParameter("target", "The case field on the target case",
        valueTypes: [DateType])]
    [ActionParameter("start", "The start date to set",
        valueTypes: [DateType])]
    [CaseRelationBuildAction("SetTargetFieldStart", "Set the case relation target field change start date", "FieldStart")]
    public void SetTargetFieldStart(CaseRelationActionContext context, object target, object start)
    {
        // target, must be a case change reference
        var targetValue = GetActionValue<DateTime?>(context, target);
        if (!targetValue.IsCaseChangeReference)
        {
            context.AddIssue($"Invalid target field reference {target}");
            return;
        }
        if (!context.Function.GetTargetFieldNames().Any(x => string.Equals(x, targetValue.ReferenceField)))
        {
            context.AddIssue($"Unknown target field {target}");
            return;
        }

        // start
        var startValue = GetActionValue<DateTime?>(context, start);
        if (startValue?.ResolvedValue == null || !startValue.IsFulfilled)
        {
            return;
        }

        // update, ignore start after end
        var end = context.Function.GetTargetEnd(targetValue.ReferenceField);
        if (end == null || startValue.ResolvedValue < end.Value)
        {
            context.Function.SetTargetStart(targetValue.ReferenceField, startValue.ResolvedValue);
        }
    }

    /// <summary>Set the case relation target field end date</summary>
    /// <param name="context">The action context</param>
    /// <param name="target">The case field on the target case</param>
    /// <param name="end">The end date to set</param>
    [ActionParameter("target", "The case field on the target case",
        valueTypes: [DateType])]
    [ActionParameter("end", "The end date to set",
        valueTypes: [DateType])]
    [CaseRelationBuildAction("SetTargetFieldEnd", "Set the case relation target field change end date", "FieldEnd")]
    public void SetTargetFieldEnd(CaseRelationActionContext context, object target, object end)
    {
        // target, must be a case change reference
        var targetValue = GetActionValue<DateTime?>(context, target);
        if (!targetValue.IsCaseChangeReference)
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
        var start = context.Function.GetTargetStart(targetValue.ReferenceField);
        if (start == null || endValue.ResolvedValue > start.Value)
        {
            context.Function.SetTargetEnd(targetValue.ReferenceField, endValue.ResolvedValue);
        }
    }

    /// <summary>Set the case relation target field start and end date</summary>
    /// <param name="context">The action context</param>
    /// <param name="target">The case field on the target case</param>
    /// <param name="start">The date to set</param>
    /// <param name="end">The date to set</param>
    [ActionParameter("target", "The case field on the target case",
        valueTypes: [DateType])]
    [ActionParameter("start", "The start date to set",
        valueTypes: [DateType])]
    [ActionParameter("end", "The end date to set",
        valueTypes: [DateType])]
    [CaseRelationBuildAction("SetTargetFieldStartEnd", "Set the case relation target field change start and end date", "FieldStart", "FieldEnd")]
    public void SetTargetFieldStartEnd(CaseRelationActionContext context, object target, object start, object end)
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
        context.Function.SetTargetStart(targetValue.ReferenceField,
            Date.Min(startValue.ResolvedValue.Value, endValue.ResolvedValue.Value));
        context.Function.SetTargetEnd(targetValue.ReferenceField,
            Date.Max(startValue.ResolvedValue.Value, endValue.ResolvedValue.Value));
    }

    /// <summary>Write log entry</summary>
    /// <param name="context">The action context</param>
    /// <param name="message">The log message</param>
    [ActionParameter("message", "The log message",
        valueTypes: [StringType])]
    [CaseRelationBuildAction("Log", "Write log entry", "Tool")]
    public void Log(CaseRelationActionContext context, string message) =>
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
    [CaseRelationBuildAction("AddTask", "Add a user task", "Tool")]
    public void AddTask(CaseRelationActionContext context, string name, string instruction,
        DateTime scheduleDate, string category = null) =>
        context.Function.AddTask(name, instruction, scheduleDate, category);
}
