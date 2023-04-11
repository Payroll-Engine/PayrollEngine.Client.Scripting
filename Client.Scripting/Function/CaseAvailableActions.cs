/* CaseAvailableActions */

using System;

namespace PayrollEngine.Client.Scripting.Function;

/// <summary>Extension methods for the case change actions</summary>
[ActionProvider(Function.DefaultActionNamespace, typeof(CaseAvailableFunction))]
public class CaseAvailableActions : CaseAvailableActionsBase
{
    /// <summary>Validate for equal value</summary>
    /// <param name="context">The action context</param>
    /// <param name="source">The source case field</param>
    /// <param name="compare">The compare value</param>
    /// <param name="compareDate">The compare date for case values</param>
    [ActionParameter("source", "The source case field",
        valueTypes: new[] { StringType, BooleanType, IntType, DecimalType, DateType },
        valueReferences: new[] { ActionReferenceCaseValue },
        valueSources: new[] { ActionSourceValue })]
    [ActionParameter("compare", "The compare value",
        valueTypes: new[] { StringType, BooleanType, IntType, DecimalType, DateType },
        valueSources: new[] { ActionSourceValue })]
    [ActionParameter("compareDate", "The compare date for case values",
        valueTypes: new[] { DateType },
        valueSources: new[] { ActionSourceValue })]
    [CaseAvailableAction("CaseValueEqual", "Validate for equal case value", "Compare")]
    public void CaseValueEqual(CaseAvailableActionContext context, string source, object compare, object compareDate = null)
    {
        if (string.IsNullOrWhiteSpace(source))
        {
            context.AddIssue("Missing compare source");
            return;
        }

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
                var sourceString = NewActionValue<string>(context, source);
                var compareString = NewActionValue<string>(context, compare, valueDate);
                if (sourceString.ResolvedValue != compareString.ResolvedValue)
                {
                    context.AddIssue($"{sourceString} is not equal {compareString}");
                }
                break;
            case ActionValueType.Boolean:
                var sourceBool = NewActionValue<bool>(context, source);
                var compareBool = NewActionValue<bool?>(context, compare, valueDate);
                if (sourceBool.ResolvedValue != compareBool.ResolvedValue)
                {
                    context.AddIssue($"{sourceBool} is not equal {compareBool}");
                }
                break;
            case ActionValueType.Integer:
                var sourceInteger = NewActionValue<int>(context, source);
                var compareInteger = NewActionValue<int?>(context, compare, valueDate);
                if (sourceInteger.ResolvedValue != compareInteger.ResolvedValue)
                {
                    context.AddIssue($"{sourceInteger} is not equal {compareInteger}");
                }
                break;
            case ActionValueType.Decimal:
                var sourceDecimal = NewActionValue<decimal>(context, source);
                var compareDecimal = NewActionValue<decimal?>(context, compare, valueDate);
                if (sourceDecimal.ResolvedValue != compareDecimal.ResolvedValue)
                {
                    context.AddIssue($"{sourceDecimal} is not equal {compareDecimal}");
                }
                break;
            case ActionValueType.DateTime:
                var sourceDateTime = NewActionValue<DateTime>(context, source);
                var compareDateTime = NewActionValue<DateTime?>(context, compare, valueDate);
                if (sourceDateTime.ResolvedValue != compareDateTime.ResolvedValue)
                {
                    context.AddIssue($"{sourceDateTime} is not equal {compareDateTime}");
                }
                break;
        }
    }

    /// <summary>Validate for different value</summary>
    /// <param name="context">The action context</param>
    /// <param name="source">The source case field</param>
    /// <param name="compare">The compare value</param>
    /// <param name="compareDate">The compare date for case values</param>
    [ActionParameter("source", "The source case field",
        valueTypes: new[] { StringType, BooleanType, IntType, DecimalType, DateType },
        valueReferences: new[] { ActionReferenceCaseValue },
        valueSources: new[] { ActionSourceValue })]
    [ActionParameter("compare", "The compare value",
        valueTypes: new[] { StringType, BooleanType, IntType, DecimalType, DateType },
        valueSources: new[] { ActionSourceValue })]
    [ActionParameter("compareDate", "The compare date for case values",
        valueTypes: new[] { DateType },
        valueSources: new[] { ActionSourceValue })]
    [CaseAvailableAction("CaseValueNotEqual", "Validate for different value", "Compare")]
    public void CaseValueNotEqual(CaseAvailableActionContext context, string source, object compare, object compareDate = null)
    {
        if (string.IsNullOrWhiteSpace(source))
        {
            context.AddIssue("Missing compare source");
            return;
        }

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
                var sourceString = NewActionValue<string>(context, source);
                var compareString = NewActionValue<string>(context, compare, valueDate);
                if (sourceString.ResolvedValue == compareString.ResolvedValue)
                {
                    context.AddIssue($"{sourceString} is equal {compareString}");
                }
                break;
            case ActionValueType.Boolean:
                var sourceBool = NewActionValue<bool>(context, source);
                var compareBool = NewActionValue<bool?>(context, compare, valueDate);
                if (sourceBool.ResolvedValue == compareBool.ResolvedValue)
                {
                    context.AddIssue($"{sourceBool} is equal {compareBool}");
                }
                break;
            case ActionValueType.Integer:
                var sourceInteger = NewActionValue<int>(context, source);
                var compareInteger = NewActionValue<int?>(context, compare, valueDate);
                if (sourceInteger.ResolvedValue == compareInteger.ResolvedValue)
                {
                    context.AddIssue($"{sourceInteger} is equal {compareInteger}");
                }
                break;
            case ActionValueType.Decimal:
                var sourceDecimal = NewActionValue<decimal>(context, source);
                var compareDecimal = NewActionValue<decimal?>(context, compare, valueDate);
                if (sourceDecimal.ResolvedValue == compareDecimal.ResolvedValue)
                {
                    context.AddIssue($"{sourceDecimal} is equal {compareDecimal}");
                }
                break;
            case ActionValueType.DateTime:
                var sourceDateTime = NewActionValue<DateTime>(context, source);
                var compareDateTime = NewActionValue<DateTime?>(context, compare, valueDate);
                if (sourceDateTime.ResolvedValue == compareDateTime.ResolvedValue)
                {
                    context.AddIssue($"{sourceDateTime} is equal {compareDateTime}");
                }
                break;
        }
    }

    /// <summary>Validate for greater value</summary>
    /// <param name="context">The action context</param>
    /// <param name="source">The source case field</param>
    /// <param name="compare">The compare value</param>
    /// <param name="compareDate">The compare date for case values</param>
    [ActionParameter("source", "The source case field",
        valueTypes: new[] { IntType, DecimalType, DateType },
        valueReferences: new[] { ActionReferenceCaseValue },
        valueSources: new[] { ActionSourceValue })]
    [ActionParameter("compare", "The compare value",
        valueTypes: new[] { IntType, DecimalType, DateType },
        valueSources: new[] { ActionSourceValue })]
    [ActionParameter("compareDate", "The compare date for case values",
        valueTypes: new[] { DateType },
        valueSources: new[] { ActionSourceValue })]
    [CaseAvailableAction("CaseValueGreaterThan", "Validate for greater value", "Compare")]
    public void CaseValueGreaterThan(CaseAvailableActionContext context, string source, object compare, object compareDate = null)
    {
        if (string.IsNullOrWhiteSpace(source))
        {
            context.AddIssue("Missing compare source");
            return;
        }

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
                var sourceInteger = NewActionValue<int>(context, source);
                var compareInteger = NewActionValue<int?>(context, compare, valueDate);
                if (sourceInteger.ResolvedValue <= compareInteger.ResolvedValue)
                {
                    context.AddIssue($"{sourceInteger} is less/equal than {compareInteger}");
                }
                break;
            case ActionValueType.Decimal:
                var sourceDecimal = NewActionValue<decimal>(context, source);
                var compareDecimal = NewActionValue<decimal?>(context, compare, valueDate);
                if (sourceDecimal.ResolvedValue <= compareDecimal.ResolvedValue)
                {
                    context.AddIssue($"{sourceDecimal} is less/equal than {compareDecimal}");
                }
                break;
            case ActionValueType.DateTime:
                var sourceDateTime = NewActionValue<DateTime>(context, source);
                var compareDateTime = NewActionValue<DateTime?>(context, compare, valueDate);
                if (sourceDateTime.ResolvedValue <= compareDateTime.ResolvedValue)
                {
                    context.AddIssue($"{sourceDateTime} is less/equal than {compareDateTime}");
                }
                break;
        }
    }

    /// <summary>Validate for greater or equal value</summary>
    /// <param name="context">The action context</param>
    /// <param name="source">The source case field</param>
    /// <param name="compare">The compare value</param>
    /// <param name="compareDate">The compare date for case values</param>
    [ActionParameter("source", "The source case field",
        valueTypes: new[] { IntType, DecimalType, DateType },
        valueReferences: new[] { ActionReferenceCaseValue },
        valueSources: new[] { ActionSourceValue })]
    [ActionParameter("compare", "The compare value",
        valueTypes: new[] { IntType, DecimalType, DateType },
        valueSources: new[] { ActionSourceValue })]
    [ActionParameter("compareDate", "The compare date for case values",
        valueTypes: new[] { DateType },
        valueSources: new[] { ActionSourceValue })]
    [CaseAvailableAction("CaseValueGreaterEqualThan", "Validate for greater or equal value", "Compare")]
    public void CaseValueGreaterEqualThan(CaseAvailableActionContext context, string source, object compare, object compareDate = null)
    {
        if (string.IsNullOrWhiteSpace(source))
        {
            context.AddIssue("Missing compare source");
            return;
        }

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
                var sourceInteger = NewActionValue<int>(context, source);
                var compareInteger = NewActionValue<int?>(context, compare, valueDate);
                if (sourceInteger.ResolvedValue < compareInteger.ResolvedValue)
                {
                    context.AddIssue($"{sourceInteger} is less than {compareInteger}");
                }
                break;
            case ActionValueType.Decimal:
                var sourceDecimal = NewActionValue<decimal>(context, source);
                var compareDecimal = NewActionValue<decimal?>(context, compare, valueDate);
                if (sourceDecimal.ResolvedValue < compareDecimal.ResolvedValue)
                {
                    context.AddIssue($"{sourceDecimal} is less than {compareDecimal}");
                }
                break;
            case ActionValueType.DateTime:
                var sourceDateTime = NewActionValue<DateTime>(context, source);
                var compareDateTime = NewActionValue<DateTime?>(context, compare, valueDate);
                if (sourceDateTime.ResolvedValue < compareDateTime.ResolvedValue)
                {
                    context.AddIssue($"{sourceDateTime} is less than {compareDateTime}");
                }
                break;
        }
    }

    /// <summary>Validate for smaller value</summary>
    /// <param name="context">The action context</param>
    /// <param name="source">The source case field</param>
    /// <param name="compare">The compare value</param>
    /// <param name="compareDate">The compare date for case values</param>
    [ActionParameter("source", "The source case field",
        valueTypes: new[] { IntType, DecimalType, DateType },
        valueReferences: new[] { ActionReferenceCaseValue },
        valueSources: new[] { ActionSourceValue })]
    [ActionParameter("compare", "The compare value",
        valueTypes: new[] { IntType, DecimalType, DateType },
        valueSources: new[] { ActionSourceValue })]
    [ActionParameter("compareDate", "The compare date for case values",
        valueTypes: new[] { DateType },
        valueSources: new[] { ActionSourceValue })]
    [CaseAvailableAction("CaseValueLessThan", "Validate for smaller value", "Compare")]
    public void CaseValueLessThan(CaseAvailableActionContext context, string source, object compare, object compareDate = null)
    {
        if (string.IsNullOrWhiteSpace(source))
        {
            context.AddIssue("Missing compare source");
            return;
        }

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
                var sourceInteger = NewActionValue<int>(context, source);
                var compareInteger = NewActionValue<int?>(context, compare, valueDate);
                if (sourceInteger.ResolvedValue >= compareInteger.ResolvedValue)
                {
                    context.AddIssue($"{sourceInteger} is greater/equal than {compareInteger}");
                }
                break;
            case ActionValueType.Decimal:
                var sourceDecimal = NewActionValue<decimal>(context, source);
                var compareDecimal = NewActionValue<decimal?>(context, compare, valueDate);
                if (sourceDecimal.ResolvedValue >= compareDecimal.ResolvedValue)
                {
                    context.AddIssue($"{sourceDecimal} is greater/equal than {compareDecimal}");
                }
                break;
            case ActionValueType.DateTime:
                var sourceDateTime = NewActionValue<DateTime>(context, source);
                var compareDateTime = NewActionValue<DateTime?>(context, compare, valueDate);
                if (sourceDateTime.ResolvedValue >= compareDateTime.ResolvedValue)
                {
                    context.AddIssue($"{sourceDateTime} is greater/equal than {compareDateTime}");
                }
                break;
        }
    }

    /// <summary>Validate for smaller or equal value</summary>
    /// <param name="context">The action context</param>
    /// <param name="source">The source case field</param>
    /// <param name="compare">The compare value</param>
    /// <param name="compareDate">The compare date for case values</param>
    [ActionParameter("source", "The source case field",
        valueTypes: new[] { IntType, DecimalType, DateType },
        valueReferences: new[] { ActionReferenceCaseValue },
        valueSources: new[] { ActionSourceValue })]
    [ActionParameter("compare", "The compare value",
        valueTypes: new[] { IntType, DecimalType, DateType },
        valueSources: new[] { ActionSourceValue })]
    [ActionParameter("compareDate", "The compare date for case values",
        valueTypes: new[] { DateType },
        valueSources: new[] { ActionSourceValue })]
    [CaseAvailableAction("CaseValueLessEqualThan", "Validate for smaller or equal value", "Compare")]
    public void CaseValueLessEqualThan(CaseAvailableActionContext context, string source, object compare, object compareDate = null)
    {
        if (string.IsNullOrWhiteSpace(source))
        {
            context.AddIssue("Missing compare source");
            return;
        }

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
                var sourceInteger = NewActionValue<int>(context, source);
                var compareInteger = NewActionValue<int?>(context, compare, valueDate);
                if (sourceInteger.ResolvedValue > compareInteger.ResolvedValue)
                {
                    context.AddIssue($"{sourceInteger} is greater than {compareInteger}");
                }
                break;
            case ActionValueType.Decimal:
                var sourceDecimal = NewActionValue<decimal>(context, source);
                var compareDecimal = NewActionValue<decimal?>(context, compare, valueDate);
                if (sourceDecimal.ResolvedValue > compareDecimal.ResolvedValue)
                {
                    context.AddIssue($"{sourceDecimal} is greater than {compareDecimal}");
                }
                break;
            case ActionValueType.DateTime:
                var sourceDateTime = NewActionValue<DateTime>(context, source);
                var compareDateTime = NewActionValue<DateTime?>(context, compare, valueDate);
                if (sourceDateTime.ResolvedValue > compareDateTime.ResolvedValue)
                {
                    context.AddIssue($"{sourceDateTime} is greater than {compareDateTime}");
                }
                break;
        }
    }

    /// <summary>Validate range value</summary>
    /// <param name="context">The action context</param>
    /// <param name="source">The source case field</param>
    /// <param name="start">The range start value</param>
    /// <param name="end">The range end value</param>
    /// <param name="compareDate">The compare date for case values</param>
    [ActionParameter("source", "The source case field",
        valueTypes: new[] { IntType, DecimalType, DateType },
        valueReferences: new[] { ActionReferenceCaseValue },
        valueSources: new[] { ActionSourceValue })]
    [ActionParameter("start", "The range start value",
        valueTypes: new[] { IntType, DecimalType, DateType },
        valueSources: new[] { ActionSourceValue })]
    [ActionParameter("end", "The range end value",
        valueTypes: new[] { IntType, DecimalType, DateType },
        valueSources: new[] { ActionSourceValue })]
    [ActionParameter("compareDate", "The compare date for case values",
        valueTypes: new[] { DateType },
        valueSources: new[] { ActionSourceValue })]
    [CaseAvailableAction("CaseValueBetween", "Validate range value", "Compare")]
    public void CaseValueBetween(CaseAvailableActionContext context, string source, object start, object end,
        object compareDate = null)
    {
        CaseValueGreaterEqualThan(context, source, start);
        if (!context.HasIssues)
        {
            CaseValueLessEqualThan(context, source, end);
        }
    }

    private static ActionValueType? ResolveCaseValueType(CaseAvailableActionContext context, string source)
    {
        var caseValue = NewCaseActionValue<object>(context, source);
        if (caseValue == null || !caseValue.IsCaseValueReference)
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

    private static ActionValueType? ResolveCompareValueType(CaseAvailableActionContext context, object compare)
    {
        var compareValue = NewActionValue<object>(context, compare);
        if (compareValue == null || !compareValue.IsFulfilled ||
            compareValue.IsCaseChangeReference)
        {
            context.AddIssue($"Invalid compare value: {compare}");
            return null;
        }
        return compareValue.GetValueType();
    }
}
