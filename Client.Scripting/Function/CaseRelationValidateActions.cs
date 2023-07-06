/* CaseRelationValidateActions */

using System;

namespace PayrollEngine.Client.Scripting.Function;

/// <summary>Extension methods for the case change actions</summary>
[ActionProvider(Function.DefaultActionNamespace, typeof(CaseRelationFunction))]
public class CaseRelationValidateActions : CaseRelationActionsBase
{

    #region Validate Compare
    /// <summary>Validate for true value</summary>
    /// <param name="context">The action context</param>
    /// <param name="source">The source value</param>
    /// <param name="compareDate">The compare date for case values</param>
    [ActionParameter("source", "The source case field",
        valueTypes: new[] { BooleanType })]
    [ActionParameter("compareDate", "The compare date for case values",
        valueTypes: new[] { DateType })]
    [CaseValidateAction("IsTrue", "Validate for true value", "Validation", "Compare")]
    public void IsTrue(CaseRelationActionContext context, object source, object compareDate = null)
    {
        // value compare date
        var valueDate = compareDate == null ? null :
            ResolveActionValue<DateTime?>(context, compareDate);

        // source value
        var sourceValue = GetActionValue<bool?>(context, source, valueDate);
        if (sourceValue.ResolvedValue == null)
        {
            context.AddIssue($"Invalid compare source {sourceValue}");
        }

        if (sourceValue.ResolvedValue != true)
        {
            context.AddIssue($"{sourceValue} is not true");
        }
    }

    /// <summary>Validate for false value</summary>
    /// <param name="context">The action context</param>
    /// <param name="source">The source value</param>
    /// <param name="compareDate">The compare date for case values</param>
    [ActionParameter("source", "The source case field",
        valueTypes: new[] { BooleanType })]
    [ActionParameter("compareDate", "The compare date for case values",
        valueTypes: new[] { DateType })]
    [CaseValidateAction("IsFalse", "Validate for false value", "Validation", "Compare")]
    public void IsFalse(CaseRelationActionContext context, object source, object compareDate = null)
    {
        // value compare date
        var valueDate = compareDate == null ? null :
            ResolveActionValue<DateTime?>(context, compareDate);

        // source value
        var sourceValue = GetActionValue<bool?>(context, source, valueDate);
        if (sourceValue.ResolvedValue == null)
        {
            context.AddIssue($"Invalid compare source {sourceValue}");
        }

        if (sourceValue.ResolvedValue != false)
        {
            context.AddIssue($"{sourceValue} is not false");
        }
    }

    /// <summary>Validate for equal value</summary>
    /// <param name="context">The action context</param>
    /// <param name="source">The source value</param>
    /// <param name="compare">The compare value</param>
    /// <param name="compareDate">The compare date for case values</param>
    [ActionParameter("source", "The source case field",
        valueTypes: new[] { StringType, BooleanType, IntType, DecimalType, DateType })]
    [ActionParameter("compare", "The compare value",
        valueTypes: new[] { StringType, BooleanType, IntType, DecimalType, DateType })]
    [ActionParameter("compareDate", "The compare date for case values",
        valueTypes: new[] { DateType })]
    [CaseRelationValidateAction("Equal", "Validate for equal value", "Validation", "Compare")]
    public void Equal(CaseRelationActionContext context, object source, object compare, object compareDate = null)
    {
        // source type
        var sourceType = ResolveCaseValueType(context, source);
        if (sourceType == null)
        {
            return;
        }

        // compare type
        var compareType = ResolveCompareValueType(context, compare);
        if (compareType == null)
        {
            return;
        }

        // value compare date
        var valueDate = compareDate == null ? null :
            ResolveActionValue<DateTime?>(context, compareDate);

        // value compare
        switch (sourceType)
        {
            case ActionValueType.String:
                var sourceString = GetActionValue<string>(context, source);
                var compareString = GetActionValue<string>(context, compare, valueDate);
                if (sourceString.ResolvedValue != compareString.ResolvedValue)
                {
                    context.AddIssue($"{sourceString} is not equal {compareString}");
                }
                break;
            case ActionValueType.Boolean:
                var sourceBool = GetActionValue<bool>(context, source);
                var compareBool = GetActionValue<bool?>(context, compare, valueDate);
                if (sourceBool.ResolvedValue != compareBool.ResolvedValue)
                {
                    context.AddIssue($"{sourceBool} is not equal {compareBool}");
                }
                break;
            case ActionValueType.Integer:
                var sourceInteger = GetActionValue<int>(context, source);
                var compareInteger = GetActionValue<int?>(context, compare, valueDate);
                if (sourceInteger.ResolvedValue != compareInteger.ResolvedValue)
                {
                    context.AddIssue($"{sourceInteger} is not equal {compareInteger}");
                }
                break;
            case ActionValueType.Decimal:
                var sourceDecimal = GetActionValue<decimal>(context, source);
                var compareDecimal = GetActionValue<decimal?>(context, compare, valueDate);
                if (sourceDecimal.ResolvedValue != compareDecimal.ResolvedValue)
                {
                    context.AddIssue($"{sourceDecimal} is not equal {compareDecimal}");
                }
                break;
            case ActionValueType.DateTime:
                var sourceDateTime = GetActionValue<DateTime>(context, source);
                var compareDateTime = GetActionValue<DateTime?>(context, compare, valueDate);
                if (sourceDateTime.ResolvedValue != compareDateTime.ResolvedValue)
                {
                    context.AddIssue($"{sourceDateTime} is not equal {compareDateTime}");
                }
                break;
        }
    }

    /// <summary>Validate for different value</summary>
    /// <param name="context">The action context</param>
    /// <param name="source">The source value</param>
    /// <param name="compare">The compare value</param>
    /// <param name="compareDate">The compare date for case values</param>
    [ActionParameter("source", "The source case field",
        valueTypes: new[] { IntType, DecimalType, DateType })]
    [ActionParameter("compare", "The compare value",
        valueTypes: new[] { IntType, DecimalType, DateType })]
    [ActionParameter("compareDate", "The compare date for case values",
        valueTypes: new[] { DateType })]
    [CaseRelationValidateAction("NotEqual", "Validate for different value", "Validation", "Compare")]
    public void NotEqual(CaseRelationActionContext context, object source, object compare, object compareDate = null)
    {
        // source type
        var sourceType = ResolveCaseValueType(context, source);
        if (sourceType == null)
        {
            return;
        }

        // compare type
        var compareType = ResolveCompareValueType(context, compare);
        if (compareType == null)
        {
            return;
        }

        // value compare date
        var valueDate = compareDate == null ? null :
            ResolveActionValue<DateTime?>(context, compareDate);

        // value compare
        switch (sourceType)
        {
            case ActionValueType.String:
                var sourceString = GetActionValue<string>(context, source);
                var compareString = GetActionValue<string>(context, compare, valueDate);
                if (sourceString.ResolvedValue == compareString.ResolvedValue)
                {
                    context.AddIssue($"{sourceString} is equal {compareString}");
                }
                break;
            case ActionValueType.Integer:
                var sourceInteger = GetActionValue<int>(context, source);
                var compareInteger = GetActionValue<int?>(context, compare, valueDate);
                if (sourceInteger.ResolvedValue == compareInteger.ResolvedValue)
                {
                    context.AddIssue($"{sourceInteger} is equal {compareInteger}");
                }
                break;
            case ActionValueType.Decimal:
                var sourceDecimal = GetActionValue<decimal>(context, source);
                var compareDecimal = GetActionValue<decimal?>(context, compare, valueDate);
                if (sourceDecimal.ResolvedValue == compareDecimal.ResolvedValue)
                {
                    context.AddIssue($"{sourceDecimal} is equal {compareDecimal}");
                }
                break;
            case ActionValueType.DateTime:
                var sourceDateTime = GetActionValue<DateTime>(context, source);
                var compareDateTime = GetActionValue<DateTime?>(context, compare, valueDate);
                if (sourceDateTime.ResolvedValue == compareDateTime.ResolvedValue)
                {
                    context.AddIssue($"{sourceDateTime} is equal {compareDateTime}");
                }
                break;
        }
    }

    /// <summary>Validate for greater value</summary>
    /// <param name="context">The action context</param>
    /// <param name="source">The source value</param>
    /// <param name="compare">The compare value</param>
    /// <param name="compareDate">The compare date for case values</param>
    [ActionParameter("source", "The source case field",
        valueTypes: new[] { IntType, DecimalType, DateType })]
    [ActionParameter("compare", "The compare value",
        valueTypes: new[] { IntType, DecimalType, DateType })]
    [ActionParameter("compareDate", "The compare date for case values",
        valueTypes: new[] { DateType })]
    [CaseRelationValidateAction("GreaterThan", "Validate for greater value", "Validation", "Compare")]
    public void GreaterThan(CaseRelationActionContext context, object source, object compare, object compareDate = null)
    {
        // source type
        var sourceType = ResolveCaseValueType(context, source);
        if (sourceType == null)
        {
            return;
        }

        // compare type
        var compareType = ResolveCompareValueType(context, compare);
        if (compareType == null)
        {
            return;
        }

        // value compare date
        var valueDate = compareDate == null ? null :
            ResolveActionValue<DateTime?>(context, compareDate);

        // value compare
        switch (sourceType)
        {
            case ActionValueType.Integer:
                var sourceInteger = GetActionValue<int>(context, source);
                var compareInteger = GetActionValue<int?>(context, compare, valueDate);
                if (sourceInteger.ResolvedValue <= compareInteger.ResolvedValue)
                {
                    context.AddIssue($"{sourceInteger} is less/equal than {compareInteger}");
                }
                break;
            case ActionValueType.Decimal:
                var sourceDecimal = GetActionValue<decimal>(context, source);
                var compareDecimal = GetActionValue<decimal?>(context, compare, valueDate);
                if (sourceDecimal.ResolvedValue <= compareDecimal.ResolvedValue)
                {
                    context.AddIssue($"{sourceDecimal} is less/equal than {compareDecimal}");
                }
                break;
            case ActionValueType.DateTime:
                var sourceDateTime = GetActionValue<DateTime>(context, source);
                var compareDateTime = GetActionValue<DateTime?>(context, compare, valueDate);
                if (sourceDateTime.ResolvedValue <= compareDateTime.ResolvedValue)
                {
                    context.AddIssue($"{sourceDateTime} is less/equal than {compareDateTime}");
                }
                break;
        }
    }

    /// <summary>Validate for greater or equal value</summary>
    /// <param name="context">The action context</param>
    /// <param name="source">The source value</param>
    /// <param name="compare">The compare value</param>
    /// <param name="compareDate">The compare date for case values</param>
    [ActionParameter("source", "The source case field",
        valueTypes: new[] { IntType, DecimalType, DateType })]
    [ActionParameter("compare", "The compare value",
        valueTypes: new[] { IntType, DecimalType, DateType })]
    [ActionParameter("compareDate", "The compare date for case values",
        valueTypes: new[] { DateType })]
    [CaseRelationValidateAction("GreaterEqualThan", "Validate for greater or equal value", "Validation", "Compare")]
    public void GreaterEqualThan(CaseRelationActionContext context, object source, object compare, object compareDate = null)
    {
        // source type
        var sourceType = ResolveCaseValueType(context, source);
        if (sourceType == null)
        {
            return;
        }

        // compare type
        var compareType = ResolveCompareValueType(context, compare);
        if (compareType == null)
        {
            return;
        }

        // value compare date
        var valueDate = compareDate == null ? null :
            ResolveActionValue<DateTime?>(context, compareDate);

        // value compare
        switch (sourceType)
        {
            case ActionValueType.Integer:
                var sourceInteger = GetActionValue<int>(context, source);
                var compareInteger = GetActionValue<int?>(context, compare, valueDate);
                if (sourceInteger.ResolvedValue < compareInteger.ResolvedValue)
                {
                    context.AddIssue($"{sourceInteger} is less than {compareInteger}");
                }
                break;
            case ActionValueType.Decimal:
                var sourceDecimal = GetActionValue<decimal>(context, source);
                var compareDecimal = GetActionValue<decimal?>(context, compare, valueDate);
                if (sourceDecimal.ResolvedValue < compareDecimal.ResolvedValue)
                {
                    context.AddIssue($"{sourceDecimal} is less than {compareDecimal}");
                }
                break;
            case ActionValueType.DateTime:
                var sourceDateTime = GetActionValue<DateTime>(context, source);
                var compareDateTime = GetActionValue<DateTime?>(context, compare, valueDate);
                if (sourceDateTime.ResolvedValue < compareDateTime.ResolvedValue)
                {
                    context.AddIssue($"{sourceDateTime} is less than {compareDateTime}");
                }
                break;
        }
    }

    /// <summary>Validate for smaller value</summary>
    /// <param name="context">The action context</param>
    /// <param name="source">The source value</param>
    /// <param name="compare">The compare value</param>
    /// <param name="compareDate">The compare date for case values</param>
    [ActionParameter("source", "The source case field",
        valueTypes: new[] { IntType, DecimalType, DateType })]
    [ActionParameter("compare", "The compare value",
        valueTypes: new[] { IntType, DecimalType, DateType })]
    [ActionParameter("compareDate", "The compare date for case values",
        valueTypes: new[] { DateType })]
    [CaseRelationValidateAction("LessThan", "Validate for smaller value", "Validation", "Compare")]
    public void LessThan(CaseRelationActionContext context, object source, object compare, object compareDate = null)
    {
        // source type
        var sourceType = ResolveCaseValueType(context, source);
        if (sourceType == null)
        {
            return;
        }

        // compare type
        var compareType = ResolveCompareValueType(context, compare);
        if (compareType == null)
        {
            return;
        }

        // value compare date
        var valueDate = compareDate == null ? null :
            ResolveActionValue<DateTime?>(context, compareDate);

        // value compare
        switch (sourceType)
        {
            case ActionValueType.Integer:
                var sourceInteger = GetActionValue<int>(context, source);
                var compareInteger = GetActionValue<int?>(context, compare, valueDate);
                if (sourceInteger.ResolvedValue >= compareInteger.ResolvedValue)
                {
                    context.AddIssue($"{sourceInteger} is greater/equal than {compareInteger}");
                }
                break;
            case ActionValueType.Decimal:
                var sourceDecimal = GetActionValue<decimal>(context, source);
                var compareDecimal = GetActionValue<decimal?>(context, compare, valueDate);
                if (sourceDecimal.ResolvedValue >= compareDecimal.ResolvedValue)
                {
                    context.AddIssue($"{sourceDecimal} is greater/equal than {compareDecimal}");
                }
                break;
            case ActionValueType.DateTime:
                var sourceDateTime = GetActionValue<DateTime>(context, source);
                var compareDateTime = GetActionValue<DateTime?>(context, compare, valueDate);
                if (sourceDateTime.ResolvedValue >= compareDateTime.ResolvedValue)
                {
                    context.AddIssue($"{sourceDateTime} is greater/equal than {compareDateTime}");
                }
                break;
        }
    }

    /// <summary>Validate for smaller or equal value</summary>
    /// <param name="context">The action context</param>
    /// <param name="source">The source value</param>
    /// <param name="compare">The compare value</param>
    /// <param name="compareDate">The compare date for case values</param>
    [ActionParameter("source", "The source case field",
        valueTypes: new[] { IntType, DecimalType, DateType })]
    [ActionParameter("compare", "The compare value",
        valueTypes: new[] { IntType, DecimalType, DateType })]
    [ActionParameter("compareDate", "The compare date for case values",
        valueTypes: new[] { DateType })]
    [CaseRelationValidateAction("LessEqualThan", "Validate for smaller or equal value", "Validation", "Compare")]
    public void LessEqualThan(CaseRelationActionContext context, object source, object compare, object compareDate = null)
    {
        // source type
        var sourceType = ResolveCaseValueType(context, source);
        if (sourceType == null)
        {
            return;
        }

        // compare type
        var compareType = ResolveCompareValueType(context, compare);
        if (compareType == null)
        {
            return;
        }

        // value compare date
        var valueDate = compareDate == null ? null :
            ResolveActionValue<DateTime?>(context, compareDate);

        // value compare
        switch (sourceType)
        {
            case ActionValueType.Integer:
                var sourceInteger = GetActionValue<int>(context, source);
                var compareInteger = GetActionValue<int?>(context, compare, valueDate);
                if (sourceInteger.ResolvedValue > compareInteger.ResolvedValue)
                {
                    context.AddIssue($"{sourceInteger} is greater than {compareInteger}");
                }
                break;
            case ActionValueType.Decimal:
                var sourceDecimal = GetActionValue<decimal>(context, source);
                var compareDecimal = GetActionValue<decimal?>(context, compare, valueDate);
                if (sourceDecimal.ResolvedValue > compareDecimal.ResolvedValue)
                {
                    context.AddIssue($"{sourceDecimal} is greater than {compareDecimal}");
                }
                break;
            case ActionValueType.DateTime:
                var sourceDateTime = GetActionValue<DateTime>(context, source);
                var compareDateTime = GetActionValue<DateTime?>(context, compare, valueDate);
                if (sourceDateTime.ResolvedValue > compareDateTime.ResolvedValue)
                {
                    context.AddIssue($"{sourceDateTime} is greater than {compareDateTime}");
                }
                break;
        }
    }

    /// <summary>Validate range value</summary>
    /// <param name="context">The action context</param>
    /// <param name="source">The source value</param>
    /// <param name="start">The range start date</param>
    /// <param name="end">The range end date</param>
    /// <param name="compareDate">The compare date for case values</param>
    [ActionParameter("source", "The source case field",
        valueTypes: new[] { IntType, DecimalType, DateType },
        valueReferences: new[] { ActionReferenceCaseValue },
        valueSources: new[] { ActionSourceValue })]
    [ActionParameter("start", "The range start date",
        valueTypes: new[] { IntType, DecimalType, DateType },
        valueSources: new[] { ActionSourceValue })]
    [ActionParameter("end", "The range end date",
        valueTypes: new[] { IntType, DecimalType, DateType },
        valueSources: new[] { ActionSourceValue })]
    [ActionParameter("compareDate", "The compare date for case values",
        valueTypes: new[] { DateType },
        valueSources: new[] { ActionSourceValue })]
    [CaseRelationValidateAction("Between", "Validate range value", "Validation", "Compare")]
    public void Between(CaseRelationActionContext context, object source, object start, object end, object compareDate = null)
    {
        GreaterEqualThan(context, source, start, compareDate);
        if (!context.HasIssues)
        {
            LessEqualThan(context, source, end, compareDate);
        }
    }

    #endregion


    private static ActionValueType? ResolveCaseValueType(CaseRelationActionContext context, object source)
    {
        var caseValue = GetActionValue<object>(context, source);
        if (!caseValue.IsReference)
        {
            context.AddIssue($"Invalid compare source value: {source}");
            return null;
        }

        // optional compare
        if (caseValue.ResolvedValue == null)
        {
            if (caseValue.MandatoryField)
            {
                context.AddIssue($"Missing compare source value: {source}");
            }
            return null;
        }
        return caseValue.GetValueType();
    }

    private static ActionValueType? ResolveCompareValueType(CaseRelationActionContext context, object compare)
    {
        var compareValue = GetActionValue<object>(context, compare);
        if (compareValue == null || !compareValue.IsFulfilled ||
            compareValue.IsCaseChangeReference)
        {
            context.AddIssue($"Invalid compare value: {compare}");
            return null;
        }
        return compareValue.GetValueType();
    }
}
