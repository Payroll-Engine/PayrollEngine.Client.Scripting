/* CaseValidateActions */

using System;

namespace PayrollEngine.Client.Scripting.Function;

/// <summary>Extension methods for the case change actions</summary>
[ActionProvider(Function.DefaultActionNamespace, typeof(CaseChangeFunction))]
public class CaseValidateActions : CaseChangeActionsBase
{

    #region Validate

    /// <summary>Validate email address</summary>
    /// <param name="context">The action context</param>
    [ActionIssue("MissingCaseValue", "Missing value (0)", 1)]
    [ActionIssue("InvalidEmail", "(0) with invalid E-Mail (1)", 2)]
    [CaseValidateAction("Email", "Validate field email address", "Validation", "FieldValue")]
    public void Email(CaseChangeActionContext context)
    {
        var sourceValue = GetActionValue<string>(context);
        if (sourceValue?.ResolvedValue == null)
        {
            AddIssue(context, "MissingCaseValue", context.CaseFieldName);
            return;
        }
        // net core email regex expression
        // https://learn.microsoft.com/en-us/dotnet/standard/base-types/how-to-verify-that-strings-are-in-valid-email-format
        if (!System.Text.RegularExpressions.Regex.IsMatch(sourceValue.ResolvedValue, @"^(.+)@(.+)$"))
        {
            AddIssue(context, "InvalidEmail", context.CaseFieldName, sourceValue.ResolvedValue);
        }
    }

    /// <summary>Validate regular expression</summary>
    /// <param name="context">The action context</param>
    /// <param name="pattern">The regular expression test pattern</param>
    [ActionIssue("MissingCaseValue", "Missing value (0)", 1)]
    [ActionIssue("InvalidRegexMatch", "(0) with invalid value (1)", 2)]
    [ActionParameter("pattern", "The regular expression test pattern",
        valueTypes: new[] { StringType })]
    [CaseValidateAction("Regex", "Validate regular expression pattern", "Validation", "FieldValue")]
    public void Regex(CaseChangeActionContext context, string pattern)
    {
        if (string.IsNullOrWhiteSpace(pattern))
        {
            throw new ScriptException(nameof(pattern));
        }
        var sourceValue = GetActionValue<string>(context);
        if (sourceValue?.ResolvedValue == null)
        {
            AddIssue(context, "MissingCaseValue", context.CaseFieldName);
            return;
        }
        // regex expression
        if (!System.Text.RegularExpressions.Regex.IsMatch(sourceValue.ResolvedValue, pattern))
        {
            AddIssue(context, "InvalidRegexMatch", context.CaseFieldName, sourceValue.ResolvedValue);
        }
    }

    /// <summary>Validate for defined value</summary>
    [ActionIssue("UndefinedValue", "(0) should be not empty", 1)]
    [CaseValidateAction("Defined", "Validate for available case value", "Validation", "FieldValue")]
    public void Defined(CaseChangeActionContext context)
    {
        var sourceValue = GetActionValue<object>(context);
        if (sourceValue?.ResolvedValue == null)
        {
            AddIssue(context, "UndefinedValue", context.CaseFieldName);
        }
    }

    /// <summary>Validate for undefined value</summary>
    [ActionIssue("DefinedValue", "(0) should be empty", 1)]
    [CaseValidateAction("Undefined", "Validate for unavailable case value", "Validation", "FieldValue")]
    public void Undefined(CaseChangeActionContext context)
    {
        var sourceValue = GetActionValue<object>(context);
        if (sourceValue?.ResolvedValue != null)
        {
            AddIssue(context, "DefinedValue", context.CaseFieldName);
        }
    }

    #endregion

    #region Validate Compare

    /// <summary>Validate for true value</summary>
    /// <param name="context">The action context</param>
    /// <param name="source">The source value</param>
    /// <param name="compareDate">The compare date for case values</param>
    [ActionIssue("CompareInvalidSource", "Invalid compare source (0)", 1)]
    [ActionIssue("CompareValueNotTrue", "(0) is not true", 1)]
    [ActionParameter("source", "The source case field",
        valueTypes: new[] { BooleanType })]
    [ActionParameter("compareDate", "The compare date for case values",
        valueTypes: new[] { DateType })]
    [CaseValidateAction("IsTrue", "Validate for true value", "Validation", "Compare")]
    public void IsTrue(CaseChangeActionContext context, object source, object compareDate = null)
    {
        // value compare date (maybe null)
        var valueDate = compareDate == null ? null : ResolveActionValue<DateTime?>(context, compareDate);

        // source value
        var sourceValue = GetActionValue<bool?>(context, source, valueDate);
        if (sourceValue?.ResolvedValue == null)
        {
            AddIssue(context, "CompareInvalidSource", sourceValue);
        }
        else if (sourceValue.ResolvedValue != true)
        {
            AddIssue(context, "CompareValueNotTrue", sourceValue);
        }
    }

    /// <summary>Validate for false value</summary>
    /// <param name="context">The action context</param>
    /// <param name="source">The source value</param>
    /// <param name="compareDate">The compare date for case values</param>
    [ActionIssue("CompareInvalidSource", "Invalid compare source (0)", 1)]
    [ActionIssue("CompareValueNotFalse", "(0) is not false", 1)]
    [ActionParameter("source", "The source case field",
        valueTypes: new[] { BooleanType })]
    [ActionParameter("compareDate", "The compare date for case values",
        valueTypes: new[] { DateType })]
    [CaseValidateAction("IsFalse", "Validate for false value", "Validation", "Compare")]
    public void IsFalse(CaseChangeActionContext context, object source, object compareDate = null)
    {
        // value compare date (maybe null)
        var valueDate = compareDate == null ? null : ResolveActionValue<DateTime?>(context, compareDate);

        // source value
        var sourceValue = GetActionValue<bool?>(context, source, valueDate);
        if (sourceValue?.ResolvedValue == null)
        {
            AddIssue(context, "CompareInvalidSource", sourceValue);
        }
        else if (sourceValue.ResolvedValue != false)
        {
            AddIssue(context, "CompareValueNotFalse", sourceValue);
        }
    }

    /// <summary>Validate for equal value</summary>
    /// <param name="context">The action context</param>
    /// <param name="source">The source value</param>
    /// <param name="compare">The compare value</param>
    /// <param name="compareDate">The compare date for case values</param>
    [ActionIssue("CompareInvalidSourceValue", "(0) invalid source value (1)", 2)]
    [ActionIssue("CompareMissingSourceValue", "(0) missing source value (1)", 2)]
    [ActionIssue("CompareInvalidCompareValue", "(0) invalid compare value (1)", 2)]
    [ActionIssue("CompareValueNotEqual", "(0) is not equal (1)", 2)]
    [ActionParameter("source", "The source case field",
        valueTypes: new[] { StringType, BooleanType, IntType, DecimalType, DateType })]
    [ActionParameter("compare", "The compare value",
        valueTypes: new[] { StringType, BooleanType, IntType, DecimalType, DateType })]
    [ActionParameter("compareDate", "The compare date for case values",
        valueTypes: new[] { DateType })]
    [CaseValidateAction("Equal", "Validate for equal value", "Validation", "Compare")]
    public void Equal(CaseChangeActionContext context, object source, object compare, object compareDate = null)
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

        // value compare date (maybe null)
        var valueDate = compareDate == null ? null : ResolveActionValue<DateTime?>(context, compareDate);

        // value compare
        switch (sourceType)
        {
            case ActionValueType.String:
                var sourceString = GetActionValue<string>(context, source);
                var compareString = GetActionValue<string>(context, compare, valueDate);
                if (sourceString != null && sourceString.ResolvedValue != compareString?.ResolvedValue)
                {
                    AddIssue(context, "CompareValueNotEqual", sourceString, compareString);
                }
                break;
            case ActionValueType.Boolean:
                var sourceBool = GetActionValue<bool>(context, source);
                var compareBool = GetActionValue<bool?>(context, compare, valueDate);
                if (sourceBool != null && sourceBool.ResolvedValue != compareBool?.ResolvedValue)
                {
                    AddIssue(context, "CompareValueNotEqual", sourceBool, compareBool);
                }
                break;
            case ActionValueType.Integer:
                var sourceInteger = GetActionValue<int>(context, source);
                var compareInteger = GetActionValue<int?>(context, compare, valueDate);
                if (sourceInteger != null && sourceInteger.ResolvedValue != compareInteger?.ResolvedValue)
                {
                    AddIssue(context, "CompareValueNotEqual", sourceInteger, compareInteger);
                }
                break;
            case ActionValueType.Decimal:
                var sourceDecimal = GetActionValue<decimal>(context, source);
                var compareDecimal = GetActionValue<decimal?>(context, compare, valueDate);
                if (sourceDecimal != null && sourceDecimal.ResolvedValue != compareDecimal?.ResolvedValue)
                {
                    AddIssue(context, "CompareValueNotEqual", sourceDecimal, compareDecimal);
                }
                break;
            case ActionValueType.DateTime:
                var sourceDateTime = GetActionValue<DateTime>(context, source);
                var compareDateTime = GetActionValue<DateTime?>(context, compare, valueDate);
                if (sourceDateTime != null && sourceDateTime.ResolvedValue != compareDateTime?.ResolvedValue)
                {
                    AddIssue(context, "CompareValueNotEqual", sourceDateTime, compareDateTime);
                }
                break;
        }
    }

    /// <summary>Validate for different value</summary>
    /// <param name="context">The action context</param>
    /// <param name="source">The source value</param>
    /// <param name="compare">The compare value</param>
    /// <param name="compareDate">The compare date for case values</param>
    [ActionIssue("CompareInvalidSourceValue", "(0) invalid source value (1)", 2)]
    [ActionIssue("CompareMissingSourceValue", "(0) missing source value (1)", 2)]
    [ActionIssue("CompareInvalidCompareValue", "(0) invalid compare value (1)", 2)]
    [ActionIssue("CompareValueEqual", "(0) is equal (1)", 2)]
    [ActionParameter("source", "The source case field",
        valueTypes: new[] { StringType, BooleanType, IntType, DecimalType, DateType })]
    [ActionParameter("compare", "The compare value",
        valueTypes: new[] { StringType, BooleanType, IntType, DecimalType, DateType })]
    [ActionParameter("compareDate", "The compare date for case values",
        valueTypes: new[] { DateType })]
    [CaseValidateAction("NotEqual", "Validate for different value", "Validation", "Compare")]
    public void NotEqual(CaseChangeActionContext context, object source, object compare, object compareDate = null)
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

        // value compare date (maybe null)
        var valueDate = compareDate == null ? null : ResolveActionValue<DateTime?>(context, compareDate);

        // value compare
        switch (sourceType)
        {
            case ActionValueType.String:
                var sourceString = GetActionValue<string>(context, source);
                var compareString = GetActionValue<string>(context, compare, valueDate);
                if (sourceString != null && sourceString.ResolvedValue == compareString?.ResolvedValue)
                {
                    AddIssue(context, "CompareValueEqual", sourceString, compareString);
                }
                break;
            case ActionValueType.Boolean:
                var sourceBool = GetActionValue<bool>(context, source);
                var compareBool = GetActionValue<bool?>(context, compare, valueDate);
                if (sourceBool != null && sourceBool.ResolvedValue == compareBool?.ResolvedValue)
                {
                    AddIssue(context, "CompareValueEqual", sourceBool, compareBool);
                }
                break;
            case ActionValueType.Integer:
                var sourceInteger = GetActionValue<int>(context, source);
                var compareInteger = GetActionValue<int?>(context, compare, valueDate);
                if (sourceInteger != null && sourceInteger.ResolvedValue == compareInteger?.ResolvedValue)
                {
                    AddIssue(context, "CompareValueEqual", sourceInteger, compareInteger);
                }
                break;
            case ActionValueType.Decimal:
                var sourceDecimal = GetActionValue<decimal>(context, source);
                var compareDecimal = GetActionValue<decimal?>(context, compare, valueDate);
                if (sourceDecimal != null && sourceDecimal.ResolvedValue == compareDecimal?.ResolvedValue)
                {
                    AddIssue(context, "CompareValueEqual", sourceDecimal, compareDecimal);
                }
                break;
            case ActionValueType.DateTime:
                var sourceDateTime = GetActionValue<DateTime>(context, source);
                var compareDateTime = GetActionValue<DateTime?>(context, compare, valueDate);
                if (sourceDateTime != null && sourceDateTime.ResolvedValue == compareDateTime?.ResolvedValue)
                {
                    AddIssue(context, "CompareValueEqual", sourceDateTime, compareDateTime);
                }
                break;
        }
    }

    /// <summary>Validate for greater value</summary>
    /// <param name="context">The action context</param>
    /// <param name="source">The source value</param>
    /// <param name="compare">The compare value</param>
    /// <param name="compareDate">The compare date for case values</param>
    [ActionIssue("CompareInvalidSourceValue", "(0) invalid source value (1)", 2)]
    [ActionIssue("CompareMissingSourceValue", "(0) missing source value (1)", 2)]
    [ActionIssue("CompareInvalidCompareValue", "(0) invalid compare value (1)", 2)]
    [ActionIssue("CompareValueLessEqual", "(0) is less/equal than (1)", 2)]
    [ActionParameter("source", "The source case field",
        valueTypes: new[] { IntType, DecimalType, DateType })]
    [ActionParameter("compare", "The compare value",
        valueTypes: new[] { IntType, DecimalType, DateType })]
    [ActionParameter("compareDate", "The compare date for case values",
        valueTypes: new[] { DateType })]
    [CaseValidateAction("GreaterThan", "Validate for greater value", "Validation", "Compare")]
    public void GreaterThan(CaseChangeActionContext context, object source, object compare, object compareDate = null)
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

        // value compare date (maybe null)
        var valueDate = compareDate == null ? null : ResolveActionValue<DateTime?>(context, compareDate);

        // value compare
        switch (sourceType)
        {
            case ActionValueType.Integer:
                var sourceInteger = GetActionValue<int>(context, source);
                var compareInteger = GetActionValue<int?>(context, compare, valueDate);
                if (sourceInteger != null && sourceInteger.ResolvedValue <= compareInteger?.ResolvedValue)
                {
                    AddIssue(context, "CompareValueLessEqual", sourceInteger, compareInteger);
                }
                break;
            case ActionValueType.Decimal:
                var sourceDecimal = GetActionValue<decimal>(context, source);
                var compareDecimal = GetActionValue<decimal?>(context, compare, valueDate);
                if (sourceDecimal != null && sourceDecimal.ResolvedValue <= compareDecimal?.ResolvedValue)
                {
                    AddIssue(context, "CompareValueLessEqual", sourceDecimal, compareDecimal);
                }
                break;
            case ActionValueType.DateTime:
                var sourceDateTime = GetActionValue<DateTime>(context, source);
                var compareDateTime = GetActionValue<DateTime?>(context, compare, valueDate);
                if (sourceDateTime != null && sourceDateTime.ResolvedValue <= compareDateTime?.ResolvedValue)
                {
                    AddIssue(context, "CompareValueLessEqual", sourceDateTime, compareDateTime);
                }
                break;
        }
    }

    /// <summary>Validate for greater or equal value</summary>
    /// <param name="context">The action context</param>
    /// <param name="source">The source value</param>
    /// <param name="compare">The compare value</param>
    /// <param name="compareDate">The compare date for case values</param>
    [ActionIssue("CompareInvalidSourceValue", "(0) invalid source value (1)", 2)]
    [ActionIssue("CompareMissingSourceValue", "(0) missing source value (1)", 2)]
    [ActionIssue("CompareInvalidCompareValue", "(0) invalid compare value (1)", 2)]
    [ActionIssue("CompareValueLess", "(0) is less than (1)", 2)]
    [ActionParameter("source", "The source case field",
        valueTypes: new[] { IntType, DecimalType, DateType })]
    [ActionParameter("compare", "The compare value",
        valueTypes: new[] { IntType, DecimalType, DateType })]
    [ActionParameter("compareDate", "The compare date for case values",
        valueTypes: new[] { DateType })]
    [CaseValidateAction("GreaterEqualThan", "Validate for greater or equal value", "Validation", "Compare")]
    public void GreaterEqualThan(CaseChangeActionContext context, object source, object compare, object compareDate = null)
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

        // value compare date (maybe null)
        var valueDate = compareDate == null ? null : ResolveActionValue<DateTime?>(context, compareDate);

        // value compare
        switch (sourceType)
        {
            case ActionValueType.Integer:
                var sourceInteger = GetActionValue<int>(context, source);
                var compareInteger = GetActionValue<int?>(context, compare, valueDate);
                if (sourceInteger != null && sourceInteger.ResolvedValue < compareInteger?.ResolvedValue)
                {
                    AddIssue(context, "CompareValueLess", sourceInteger, compareInteger);
                }
                break;
            case ActionValueType.Decimal:
                var sourceDecimal = GetActionValue<decimal>(context, source);
                var compareDecimal = GetActionValue<decimal?>(context, compare, valueDate);
                if (sourceDecimal != null && sourceDecimal.ResolvedValue < compareDecimal?.ResolvedValue)
                {
                    AddIssue(context, "CompareValueLess", sourceDecimal, compareDecimal);
                }
                break;
            case ActionValueType.DateTime:
                var sourceDateTime = GetActionValue<DateTime>(context, source);
                var compareDateTime = GetActionValue<DateTime?>(context, compare, valueDate);
                if (sourceDateTime != null && sourceDateTime.ResolvedValue < compareDateTime?.ResolvedValue)
                {
                    AddIssue(context, "CompareValueLess", sourceDateTime, compareDateTime);
                }
                break;
        }
    }

    /// <summary>Validate for smaller value</summary>
    /// <param name="context">The action context</param>
    /// <param name="source">The source value</param>
    /// <param name="compare">The compare value</param>
    /// <param name="compareDate">The compare date for case values</param>
    [ActionIssue("CompareInvalidSourceValue", "(0) invalid source value (1)", 2)]
    [ActionIssue("CompareMissingSourceValue", "(0) missing source value (1)", 2)]
    [ActionIssue("CompareInvalidCompareValue", "(0) invalid compare value (1)", 2)]
    [ActionIssue("CompareValueGreaterEqual", "(0) is greater/equal than (1)", 2)]
    [ActionParameter("source", "The source case field",
        valueTypes: new[] { IntType, DecimalType, DateType })]
    [ActionParameter("compare", "The compare value",
        valueTypes: new[] { IntType, DecimalType, DateType })]
    [ActionParameter("compareDate", "The compare date for case values",
        valueTypes: new[] { DateType })]
    [CaseValidateAction("LessThan", "Validate for smaller value", "Validation", "Compare")]
    public void LessThan(CaseChangeActionContext context, object source, object compare, object compareDate = null)
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

        // value compare date (maybe null)
        var valueDate = compareDate == null ? null : ResolveActionValue<DateTime?>(context, compareDate);

        // value compare
        switch (sourceType)
        {
            case ActionValueType.Integer:
                var sourceInteger = GetActionValue<int>(context, source);
                var compareInteger = GetActionValue<int?>(context, compare, valueDate);
                if (sourceInteger != null && sourceInteger.ResolvedValue >= compareInteger?.ResolvedValue)
                {
                    AddIssue(context, "CompareValueGreaterEqual", sourceInteger, compareInteger);
                }
                break;
            case ActionValueType.Decimal:
                var sourceDecimal = GetActionValue<decimal>(context, source);
                var compareDecimal = GetActionValue<decimal?>(context, compare, valueDate);
                if (sourceDecimal != null && sourceDecimal.ResolvedValue >= compareDecimal?.ResolvedValue)
                {
                    AddIssue(context, "CompareValueGreaterEqual", sourceDecimal, compareDecimal);
                }
                break;
            case ActionValueType.DateTime:
                var sourceDateTime = GetActionValue<DateTime>(context, source);
                var compareDateTime = GetActionValue<DateTime?>(context, compare, valueDate);
                if (sourceDateTime != null && sourceDateTime.ResolvedValue >= compareDateTime?.ResolvedValue)
                {
                    AddIssue(context, "CompareValueGreaterEqual", sourceDateTime, compareDateTime);
                }
                break;
        }
    }

    /// <summary>Validate for smaller or equal value</summary>
    /// <param name="context">The action context</param>
    /// <param name="source">The source value</param>
    /// <param name="compare">The compare value</param>
    /// <param name="compareDate">The compare date for case values</param>
    [ActionIssue("CompareInvalidSourceValue", "(0) invalid source value (1)", 2)]
    [ActionIssue("CompareMissingSourceValue", "(0) missing source value (1)", 2)]
    [ActionIssue("CompareInvalidCompareValue", "(0) invalid compare value (1)", 2)]
    [ActionIssue("CompareValueGreater", "(0) is greater than (1)", 2)]
    [ActionParameter("source", "The source case field",
        valueTypes: new[] { IntType, DecimalType, DateType })]
    [ActionParameter("compare", "The compare value",
        valueTypes: new[] { IntType, DecimalType, DateType })]
    [ActionParameter("compareDate", "The compare date for case values",
        valueTypes: new[] { DateType })]
    [CaseValidateAction("LessEqualThan", "Validate for smaller or equal value", "Validation", "Compare")]
    public void LessEqualThan(CaseChangeActionContext context, object source, object compare, object compareDate = null)
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

        // value compare date (maybe null)
        var valueDate = compareDate == null ? null : ResolveActionValue<DateTime?>(context, compareDate);

        // value compare
        switch (sourceType)
        {
            case ActionValueType.Integer:
                var sourceInteger = GetActionValue<int>(context, source);
                var compareInteger = GetActionValue<int?>(context, compare, valueDate);
                if (sourceInteger != null && sourceInteger.ResolvedValue > compareInteger?.ResolvedValue)
                {
                    AddIssue(context, "CompareValueGreater", sourceInteger, compareInteger);
                }
                break;
            case ActionValueType.Decimal:
                var sourceDecimal = GetActionValue<decimal>(context, source);
                var compareDecimal = GetActionValue<decimal?>(context, compare, valueDate);
                if (sourceDecimal != null && sourceDecimal.ResolvedValue > compareDecimal?.ResolvedValue)
                {
                    AddIssue(context, "CompareValueGreater", sourceDecimal, compareDecimal);
                }
                break;
            case ActionValueType.DateTime:
                var sourceDateTime = GetActionValue<DateTime>(context, source);
                var compareDateTime = GetActionValue<DateTime?>(context, compare, valueDate);
                if (sourceDateTime != null && sourceDateTime.ResolvedValue > compareDateTime?.ResolvedValue)
                {
                    AddIssue(context, "CompareValueGreater", sourceDateTime, compareDateTime);
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
    [ActionIssue("CompareInvalidSourceValue", "(0) invalid source value (1)", 2)]
    [ActionIssue("CompareMissingSourceValue", "(0) missing source value (1)", 2)]
    [ActionIssue("CompareInvalidCompareValue", "(0) invalid compare value (1)", 2)]
    [ActionIssue("CompareValueLess", "(0) is less than (1)", 2)]
    [ActionIssue("CompareValueGreater", "(0) is greater than (1)", 2)]
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
    [CaseValidateAction("Between", "Validate range value", "Validation", "Compare")]
    public void Between(CaseChangeActionContext context, object source, object start, object end, object compareDate = null)
    {
        GreaterEqualThan(context, source, start, compareDate);
        if (!context.HasIssues)
        {
            LessEqualThan(context, source, end, compareDate);
        }
    }

    #endregion

    #region Validate Case Value

    /// <summary>Validate for equal case value</summary>
    /// <param name="context">The action context</param>
    /// <param name="compare">The compare value</param>
    /// <param name="compareDate">The compare date for case values</param>
    [ActionIssue("CompareValueNotEqual", "(0) is not equal (1)", 2)]
    [ActionParameter("compare", "The compare value",
        valueTypes: new[] { IntType, DecimalType, DateType },
        valueSources: new[] { ActionSourceValue })]
    [ActionParameter("compareDate", "The compare date for case values",
        valueTypes: new[] { DateType },
        valueSources: new[] { ActionSourceValue })]
    [CaseValidateAction("ValueEqual", "Validate for equal case value", "Validation", "FieldValue")]
    public void ValueEqual(CaseChangeActionContext context, object compare, object compareDate = null) =>
        Equal(context, ActionCaseValueBase.ToCaseChangeValueReference(context.CaseFieldName), compare, compareDate);

    /// <summary>Validate for not equal value</summary>
    /// <param name="context">The action context</param>
    /// <param name="compare">The compare value</param>
    /// <param name="compareDate">The compare date for case values</param>
    [ActionIssue("CompareValueEqual", "(0) is equal (1)", 2)]
    [ActionParameter("compare", "The compare value",
        valueTypes: new[] { IntType, DecimalType, DateType },
        valueSources: new[] { ActionSourceValue })]
    [ActionParameter("compareDate", "The compare date for case values",
        valueTypes: new[] { DateType },
        valueSources: new[] { ActionSourceValue })]
    [CaseValidateAction("ValueNotEqual", "Validate for different case value", "Validation", "FieldValue")]
    public void ValueNotEqual(CaseChangeActionContext context, object compare, object compareDate = null) =>
        NotEqual(context, ActionCaseValueBase.ToCaseChangeValueReference(context.CaseFieldName), compare, compareDate);

    /// <summary>Validate value greater than a criteria</summary>
    /// <param name="context">The action context</param>
    /// <param name="compare">The compare value</param>
    /// <param name="compareDate">The compare date for case values</param>
    [ActionIssue("CompareValueLessEqual", "(0) is less/equal than (1)", 2)]
    [ActionParameter("compare", "The compare value",
        valueTypes: new[] { IntType, DecimalType, DateType },
        valueSources: new[] { ActionSourceValue })]
    [ActionParameter("compareDate", "The compare date for case values",
        valueTypes: new[] { DateType },
        valueSources: new[] { ActionSourceValue })]
    [CaseValidateAction("ValueGreaterThan", "Validate for greater case value", "Validation", "FieldValue")]
    public void ValueGreaterThan(CaseChangeActionContext context, object compare, object compareDate = null) =>
        GreaterThan(context, ActionCaseValueBase.ToCaseChangeValueReference(context.CaseFieldName), compare, compareDate);

    /// <summary>Validate value greater equal than a criteria</summary>
    /// <param name="context">The action context</param>
    /// <param name="compare">The compare value</param>
    /// <param name="compareDate">The compare date for case values</param>
    [ActionIssue("CompareValueLess", "(0) is less than (1)", 2)]
    [ActionParameter("compare", "The compare value",
        valueTypes: new[] { IntType, DecimalType, DateType },
        valueSources: new[] { ActionSourceValue })]
    [ActionParameter("compareDate", "The compare date for case values",
        valueTypes: new[] { DateType },
        valueSources: new[] { ActionSourceValue })]
    [CaseValidateAction("ValueGreaterEqualThan", "Validate for greater or equal case value", "Validation", "FieldValue")]
    public void ValueGreaterEqualThan(CaseChangeActionContext context, object compare, object compareDate = null) =>
        GreaterEqualThan(context, ActionCaseValueBase.ToCaseChangeValueReference(context.CaseFieldName), compare, compareDate);

    /// <summary>Validate value less than a criteria</summary>
    /// <param name="context">The action context</param>
    /// <param name="compare">The compare value</param>
    /// <param name="compareDate">The compare date for case values</param>
    [ActionIssue("CompareValueGreaterEqual", "(0) is greater/equal than (1)", 2)]
    [ActionParameter("compare", "The compare value",
        valueTypes: new[] { IntType, DecimalType, DateType },
        valueSources: new[] { ActionSourceValue })]
    [ActionParameter("compareDate", "The compare date for case values",
        valueTypes: new[] { DateType },
        valueSources: new[] { ActionSourceValue })]
    [CaseValidateAction("ValueLessThan", "Validate for smaller case value", "Validation", "FieldValue")]
    public void ValueLessThan(CaseChangeActionContext context, object compare, object compareDate = null) =>
        LessThan(context, ActionCaseValueBase.ToCaseChangeValueReference(context.CaseFieldName), compare, compareDate);

    /// <summary>Validate value less equal than a criteria</summary>
    /// <param name="context">The action context</param>
    /// <param name="compare">The compare value</param>
    /// <param name="compareDate">The compare date for case values</param>
    [ActionIssue("CompareValueGreater", "(0) is greater than (1)", 2)]
    [ActionParameter("compare", "The compare value",
        valueTypes: new[] { IntType, DecimalType, DateType },
        valueSources: new[] { ActionSourceValue })]
    [ActionParameter("compareDate", "The compare date for case values",
        valueTypes: new[] { DateType },
        valueSources: new[] { ActionSourceValue })]
    [CaseValidateAction("ValueLessEqualThan", "Validate for smaller or equal case value")]
    public void ValueLessEqualThan(CaseChangeActionContext context, object compare, object compareDate = null) =>
        LessEqualThan(context, ActionCaseValueBase.ToCaseChangeValueReference(context.CaseFieldName), compare, compareDate);

    /// <summary>Validate range case value</summary>
    /// <param name="context">The action context</param>
    /// <param name="start">The range start date</param>
    /// <param name="end">The range end date</param>
    /// <param name="compareDate">The compare date for case values</param>
    [ActionIssue("CompareValueLess", "(0) is less than (1)", 2)]
    [ActionIssue("CompareValueGreater", "(0) is greater than (1)", 2)]
    [ActionParameter("start", "The range start value",
        valueTypes: new[] { IntType, DecimalType, DateType },
        valueSources: new[] { ActionSourceValue })]
    [ActionParameter("end", "The range end value",
        valueTypes: new[] { IntType, DecimalType, DateType },
        valueSources: new[] { ActionSourceValue })]
    [ActionParameter("compareDate", "The compare date for case values",
        valueTypes: new[] { DateType },
        valueSources: new[] { ActionSourceValue })]
    [CaseValidateAction("ValueBetween", "Validate range case value", "Validation", "FieldValue")]
    public void ValueBetween(CaseChangeActionContext context, object start, object end, object compareDate = null) =>
        Between(context, ActionCaseValueBase.ToCaseChangeValueReference(context.CaseFieldName), start, end, compareDate);

    #endregion

    #region Validate Case Value Start

    /// <summary>Validate for equal case value start</summary>
    /// <param name="context">The action context</param>
    /// <param name="compare">The compare value</param>
    [ActionIssue("CompareValueNotEqual", "(0) is not equal (1)", 2)]
    [ActionParameter("compare", "The compare value",
        valueTypes: new[] { DateType },
        valueSources: new[] { ActionSourceStart })]
    [CaseValidateAction("StartEqual", "Validate for equal case value start", "Validation", "FieldStart")]
    public void StartEqual(CaseChangeActionContext context, object compare) =>
        Equal(context, ActionCaseValueBase.ToCaseChangeStartReference(context.CaseFieldName), compare);

    /// <summary>Validate for different case value start</summary>
    /// <param name="context">The action context</param>
    /// <param name="compare">The compare value</param>
    [ActionIssue("CompareValueEqual", "(0) is equal (1)", 2)]
    [ActionParameter("compare", "The compare value",
        valueTypes: new[] { DateType },
        valueSources: new[] { ActionSourceStart })]
    [CaseValidateAction("StartNotEqual", "Validate for different case value start", "Validation", "FieldStart")]
    public void StartNotEqual(CaseChangeActionContext context, object compare) =>
        NotEqual(context, ActionCaseValueBase.ToCaseChangeStartReference(context.CaseFieldName), compare);

    /// <summary>Validate for greater case value start</summary>
    /// <param name="context">The action context</param>
    /// <param name="compare">The compare value</param>
    [ActionIssue("CompareValueLessEqual", "(0) is less/equal than (1)", 2)]
    [ActionParameter("compare", "The compare value",
        valueTypes: new[] { DateType },
        valueSources: new[] { ActionSourceStart })]
    [CaseValidateAction("StartGreaterThan", "Validate for greater case value start", "Validation", "FieldStart")]
    public void StartGreaterThan(CaseChangeActionContext context, object compare) =>
        GreaterThan(context, ActionCaseValueBase.ToCaseChangeStartReference(context.CaseFieldName), compare);

    /// <summary>Validate for greater or equal case value start</summary>
    /// <param name="context">The action context</param>
    /// <param name="compare">The compare value</param>
    [ActionIssue("CompareValueLess", "(0) is less than (1)", 2)]
    [ActionParameter("compare", "The compare value",
        valueTypes: new[] { DateType },
        valueSources: new[] { ActionSourceStart })]
    [CaseValidateAction("StartGreaterEqualThan", "Validate for greater or equal case value start", "Validation", "FieldStart")]
    public void StartGreaterEqualThan(CaseChangeActionContext context, object compare) =>
        GreaterEqualThan(context, ActionCaseValueBase.ToCaseChangeStartReference(context.CaseFieldName), compare);

    /// <summary>Validate for smaller case value start</summary>
    /// <param name="context">The action context</param>
    /// <param name="compare">The compare value</param>
    [ActionIssue("CompareValueGreaterEqual", "(0) is greater/equal than (1)", 2)]
    [ActionParameter("compare", "The compare value",
        valueTypes: new[] { DateType },
        valueSources: new[] { ActionSourceStart })]
    [CaseValidateAction("StartLessThan", "Validate for smaller case value start", "Validation", "FieldStart")]
    public void StartLessThan(CaseChangeActionContext context, object compare) =>
        LessThan(context, ActionCaseValueBase.ToCaseChangeStartReference(context.CaseFieldName), compare);

    /// <summary>Validate for smaller or equal case value start</summary>
    /// <param name="context">The action context</param>
    /// <param name="compare">The compare value</param>
    [ActionIssue("CompareValueGreater", "(0) is greater than (1)", 2)]
    [ActionParameter("compare", "The compare value",
        valueTypes: new[] { DateType },
        valueSources: new[] { ActionSourceStart })]
    [CaseValidateAction("StartLessEqualThan", "Validate for smaller or equal case value start", "Validation", "FieldStart")]
    public void StartLessEqualThan(CaseChangeActionContext context, object compare) =>
        LessEqualThan(context, ActionCaseValueBase.ToCaseChangeStartReference(context.CaseFieldName), compare);

    /// <summary>Validate range case value start</summary>
    /// <param name="context">The action context</param>
    /// <param name="start">The range start date</param>
    /// <param name="end">The range end date</param>
    [ActionIssue("CompareValueLess", "(0) is less than (1)", 2)]
    [ActionIssue("CompareValueGreater", "(0) is greater than (1)", 2)]
    [ActionParameter("start", "The range start date",
        valueTypes: new[] { DateType },
        valueSources: new[] { ActionSourceStart })]
    [ActionParameter("end", "The range end date",
        valueTypes: new[] { DateType },
        valueSources: new[] { ActionSourceStart })]
    [CaseValidateAction("StartBetween", "Validate range case value start", "Validation", "FieldStart")]
    public void StartBetween(CaseChangeActionContext context, object start, object end) =>
        Between(context, ActionCaseValueBase.ToCaseChangeStartReference(context.CaseFieldName), start, end);

    #endregion

    #region Validate Case Value End

    /// <summary>Validate for equal case value end</summary>
    /// <param name="context">The action context</param>
    /// <param name="compare">The compare value</param>
    [ActionIssue("CompareValueNotEqual", "(0) is not equal (1)", 2)]
    [ActionParameter("compare", "The compare value",
        valueTypes: new[] { DateType },
        valueSources: new[] { ActionSourceEnd })]
    [CaseValidateAction("EndEqual", "Validate for equal case value end", "Validation", "FieldEnd")]
    public void EndEqual(CaseChangeActionContext context, object compare) =>
        Equal(context, ActionCaseValueBase.ToCaseChangeEndReference(context.CaseFieldName), compare);

    /// <summary>Validate for different case value end</summary>
    /// <param name="context">The action context</param>
    /// <param name="compare">The compare value</param>
    [ActionIssue("CompareValueEqual", "(0) is equal (1)", 2)]
    [ActionParameter("compare", "The compare value",
        valueTypes: new[] { DateType },
        valueSources: new[] { ActionSourceEnd })]
    [CaseValidateAction("EndNotEqual", "Validate for different case value end", "Validation", "FieldEnd")]
    public void EndNotEqual(CaseChangeActionContext context, object compare) =>
        NotEqual(context, ActionCaseValueBase.ToCaseChangeEndReference(context.CaseFieldName), compare);

    /// <summary>Validate for greater case value end</summary>
    /// <param name="context">The action context</param>
    /// <param name="compare">The compare value</param>
    [ActionIssue("CompareValueLessEqual", "(0) is less/equal than (1)", 2)]
    [ActionParameter("compare", "The compare value",
        valueTypes: new[] { DateType },
        valueSources: new[] { ActionSourceEnd })]
    [CaseValidateAction("EndGreaterThan", "Validate for greater case value end", "Validation", "FieldEnd")]
    public void EndGreaterThan(CaseChangeActionContext context, object compare) =>
        GreaterThan(context, ActionCaseValueBase.ToCaseChangeEndReference(context.CaseFieldName), compare);

    /// <summary>Validate for greater or equal case value end</summary>
    /// <param name="context">The action context</param>
    /// <param name="compare">The compare value</param>
    [ActionIssue("CompareValueLess", "(0) is less than (1)", 2)]
    [ActionParameter("compare", "The compare value",
        valueTypes: new[] { DateType },
        valueSources: new[] { ActionSourceEnd })]
    [CaseValidateAction("EndGreaterEqualThan", "Validate for greater or equal case value end", "Validation", "FieldEnd")]
    public void EndGreaterEqualThan(CaseChangeActionContext context, object compare) =>
        GreaterEqualThan(context, ActionCaseValueBase.ToCaseChangeEndReference(context.CaseFieldName), compare);

    /// <summary>Validate for smaller case value end</summary>
    /// <param name="context">The action context</param>
    /// <param name="compare">The compare value</param>
    [ActionIssue("CompareValueGreaterEqual", "(0) is greater/equal than (1)", 2)]
    [ActionParameter("compare", "The compare value",
        valueTypes: new[] { DateType },
        valueSources: new[] { ActionSourceEnd })]
    [CaseValidateAction("EndLessThan", "Validate for smaller case value end", "Validation", "FieldEnd")]
    public void EndLessThan(CaseChangeActionContext context, object compare) =>
        LessThan(context, ActionCaseValueBase.ToCaseChangeEndReference(context.CaseFieldName), compare);

    /// <summary>Validate for smaller or equal case value end</summary>
    /// <param name="context">The action context</param>
    /// <param name="compare">The compare value</param>
    [ActionIssue("CompareValueGreater", "(0) is greater than (1)", 2)]
    [ActionParameter("compare", "The compare value",
        valueTypes: new[] { DateType },
        valueSources: new[] { ActionSourceEnd })]
    [CaseValidateAction("EndLessEqualThan", "Validate for smaller or equal case value end", "Validation", "FieldEnd")]
    public void EndLessEqualThan(CaseChangeActionContext context, object compare) =>
        LessEqualThan(context, ActionCaseValueBase.ToCaseChangeEndReference(context.CaseFieldName), compare);

    /// <summary>Validate range case value end</summary>
    /// <param name="context">The action context</param>
    /// <param name="start">The range start date</param>
    /// <param name="end">The range end date</param>
    [ActionIssue("CompareValueLess", "(0) is less than (1)", 2)]
    [ActionIssue("CompareValueGreater", "(0) is greater than (1)", 2)]
    [ActionParameter("start", "The range start date",
        valueTypes: new[] { DateType },
        valueSources: new[] { ActionSourceEnd })]
    [ActionParameter("end", "The range end date",
        valueTypes: new[] { DateType },
        valueSources: new[] { ActionSourceEnd })]
    [CaseValidateAction("EndBetween", "Validate range case value end", "Validation", "FieldEnd")]
    public void EndBetween(CaseChangeActionContext context, object start, object end) =>
        Between(context, ActionCaseValueBase.ToCaseChangeStartReference(context.CaseFieldName), start, end);

    #endregion

    #region Validate Field Case Period

    /// <summary>Validate for moment before field period</summary>
    /// <param name="context">The action context</param>
    /// <param name="fieldName">The field name</param>
    /// <param name="moment">The moment to test</param>
    [ActionIssue("ComparePeriodWithoutStartDate", "(0) period without start (1)", 2)]
    [ActionIssue("ComparePeriodInvalidTestDate", "(0) invalid test date (1)", 2)]
    [ActionIssue("ComparePeriodNotBefore", "(0) (1) is not before period (2)", 3)]
    [ActionParameter("fieldName", "The field name",
        valueReferences: new[] { ActionReferenceCaseChange, ActionReferenceCaseValue })]
    [ActionParameter("moment", "The moment to test",
        valueTypes: new[] { DateType })]
    [CaseValidateAction("FieldPeriodBefore", "Validate for moment before field period", "Validation", "FieldPeriod")]
    public void FieldPeriodBefore(CaseChangeActionContext context, string fieldName, object moment)
    {
        // period start
        var startValue = GetActionValue<DateTime?>(context, ActionCaseValueBase.ToStartReference(fieldName));
        var start = startValue?.ResolvedValue;
        if (!start.HasValue)
        {
            AddIssue(context, "ComparePeriodWithoutStartDate", fieldName, startValue);
            return;
        }
        // period end
        var endValue = GetActionValue<DateTime?>(context, ActionCaseValueBase.ToEndReference(fieldName));
        if (endValue?.ResolvedValue == null)
        {
            return;
        }

        // test moment
        var momentValue = GetActionValue<DateTime?>(context, moment);
        if (momentValue?.ResolvedValue == null)
        {
            AddIssue(context, "ComparePeriodInvalidTestDate", fieldName, moment);
            return;
        }

        // test
        if (momentValue.ResolvedValue >= start)
        {
            AddIssue(context, "ComparePeriodNotBefore", fieldName, momentValue, GetPeriodString(startValue, endValue));
        }
    }

    /// <summary>Validate for moment not before field period</summary>
    /// <param name="context">The action context</param>
    /// <param name="fieldName">The field name</param>
    /// <param name="moment">The moment to test</param>
    [ActionIssue("ComparePeriodWithoutStartDate", "(0) period without start (1)", 2)]
    [ActionIssue("ComparePeriodInvalidTestDate", "(0) invalid test date (1)", 2)]
    [ActionIssue("ComparePeriodBefore", "(0) (1) is before period (2)", 3)]
    [ActionParameter("fieldName", "The field name",
        valueReferences: new[] { ActionReferenceCaseChange, ActionReferenceCaseValue })]
    [ActionParameter("moment", "The moment to test",
        valueTypes: new[] { DateType })]
    [CaseValidateAction("FieldPeriodNotBefore", "Validate for moment not before field period", "Validation", "FieldPeriod")]
    public void FieldPeriodNotBefore(CaseChangeActionContext context, string fieldName, object moment)
    {
        // period start
        var startValue = GetActionValue<DateTime?>(context, ActionCaseValueBase.ToStartReference(fieldName));
        var start = startValue?.ResolvedValue;
        if (!start.HasValue)
        {
            AddIssue(context, "ComparePeriodWithoutStartDate", fieldName, startValue);
            return;
        }
        // period end
        var endValue = GetActionValue<DateTime?>(context, ActionCaseValueBase.ToEndReference(fieldName));
        if (endValue?.ResolvedValue == null)
        {
            return;
        }

        // test moment
        var momentValue = GetActionValue<DateTime?>(context, moment);
        if (momentValue?.ResolvedValue == null)
        {
            AddIssue(context, "ComparePeriodInvalidTestDate", fieldName, moment);
            return;
        }

        // test
        if (momentValue.ResolvedValue < start)
        {
            AddIssue(context, "ComparePeriodBefore", fieldName, momentValue, GetPeriodString(startValue, endValue));
        }
    }

    /// <summary>Validate for moment within field period</summary>
    /// <param name="context">The action context</param>
    /// <param name="fieldName">The field name</param>
    /// <param name="moment">The moment to test</param>
    /// <param name="closedPeriod">Enforce closed period</param>
    [ActionIssue("ComparePeriodWithoutStartDate", "(0) period without start (1)", 2)]
    [ActionIssue("ComparePeriodInvalidTestDate", "(0) invalid test date (1)", 2)]
    [ActionIssue("ComparePeriodOpenPeriod", "Value (0) with open period", 1)]
    [ActionIssue("ComparePeriodNotWithin", "(0) (1) is not within (2)", 3)]
    [ActionParameter("fieldName", "The field name",
        valueReferences: new[] { ActionReferenceCaseChange, ActionReferenceCaseValue })]
    [ActionParameter("moment", "The moment to test",
        valueTypes: new[] { DateType })]
    [ActionParameter("closedPeriod", "Enforce closed period",
        valueTypes: new[] { BooleanType })]
    [CaseValidateAction("FieldPeriodWithin", "Validate for moment within field period", "Validation", "FieldPeriod")]
    public void FieldPeriodWithin(CaseChangeActionContext context, string fieldName, object moment, bool closedPeriod = false)
    {
        // period start
        var startValue = GetActionValue<DateTime?>(context, ActionCaseValueBase.ToStartReference(fieldName));
        var start = startValue?.ResolvedValue;
        if (!start.HasValue)
        {
            AddIssue(context, "ComparePeriodWithoutStartDate", fieldName, startValue);
            return;
        }
        // period end
        var endValue = GetActionValue<DateTime?>(context, ActionCaseValueBase.ToEndReference(fieldName));
        var end = endValue?.ResolvedValue;
        if (closedPeriod && !end.HasValue)
        {
            AddIssue(context, "ComparePeriodOpenPeriod", fieldName);
            return;
        }
        end ??= Date.MaxValue;

        // test moment
        var momentValue = GetActionValue<DateTime?>(context, moment);
        if (momentValue?.ResolvedValue == null)
        {
            AddIssue(context, "ComparePeriodInvalidTestDate", fieldName, moment);
            return;
        }

        // test
        if (momentValue.ResolvedValue < start || momentValue.ResolvedValue > end)
        {
            AddIssue(context, "ComparePeriodNotWithin", fieldName, momentValue, GetPeriodString(startValue, endValue));
        }
    }

    /// <summary>Validate for moment not within field period</summary>
    /// <param name="context">The action context</param>
    /// <param name="fieldName">The field name</param>
    /// <param name="moment">The moment to test</param>
    /// <param name="closedPeriod">Enforce closed period</param>
    [ActionIssue("ComparePeriodWithoutStartDate", "(0) period without start (1)", 2)]
    [ActionIssue("ComparePeriodInvalidTestDate", "(0) invalid test date (1)", 2)]
    [ActionIssue("ComparePeriodOpenPeriod", "Value (0) with open period", 1)]
    [ActionIssue("ComparePeriodWithin", "(0) (1) is within (2)", 3)]
    [ActionParameter("fieldName", "The field name",
        valueReferences: new[] { ActionReferenceCaseChange, ActionReferenceCaseValue })]
    [ActionParameter("moment", "The moment to test",
        valueTypes: new[] { DateType })]
    [ActionParameter("closedPeriod", "Enforce closed period",
        valueTypes: new[] { BooleanType })]
    [CaseValidateAction("FieldPeriodNotWithin", "Validate for moment not within field period", "Validation", "FieldPeriod")]
    public void FieldPeriodNotWithin(CaseChangeActionContext context, string fieldName, object moment, bool closedPeriod = false)
    {
        // period start
        var startValue = GetActionValue<DateTime?>(context, ActionCaseValueBase.ToStartReference(fieldName));
        var start = startValue?.ResolvedValue;
        if (!start.HasValue)
        {
            AddIssue(context, "ComparePeriodWithoutStartDate", fieldName, startValue);
            return;
        }
        // period end
        var endValue = GetActionValue<DateTime?>(context, ActionCaseValueBase.ToEndReference(fieldName));
        var end = endValue?.ResolvedValue;
        if (closedPeriod && !end.HasValue)
        {
            AddIssue(context, "ComparePeriodOpenPeriod", fieldName);
            return;
        }
        end ??= Date.MaxValue;

        // test moment
        var momentValue = GetActionValue<DateTime?>(context, moment);
        if (momentValue?.ResolvedValue == null)
        {
            AddIssue(context, "ComparePeriodInvalidTestDate", fieldName, moment);
            return;
        }

        // test
        if (momentValue.ResolvedValue >= start && momentValue.ResolvedValue <= end)
        {
            AddIssue(context, "ComparePeriodWithin", fieldName, momentValue, GetPeriodString(startValue, endValue));
        }
    }

    /// <summary>Validate for moment after field period</summary>
    /// <param name="context">The action context</param>
    /// <param name="fieldName">The field name</param>
    /// <param name="moment">The moment to test</param>
    /// <param name="closedPeriod">Enforce closed period</param>
    [ActionIssue("ComparePeriodWithoutStartDate", "(0) period without start (1)", 2)]
    [ActionIssue("ComparePeriodInvalidTestDate", "(0) invalid test date (1)", 2)]
    [ActionIssue("ComparePeriodOpenPeriod", "Value (0) with open period", 1)]
    [ActionIssue("ComparePeriodNotAfter", "(0) (1) is not after (2)", 3)]
    [ActionParameter("fieldName", "The field name",
        valueReferences: new[] { ActionReferenceCaseChange, ActionReferenceCaseValue })]
    [ActionParameter("moment", "The moment to test",
        valueTypes: new[] { DateType })]
    [ActionParameter("closedPeriod", "Enforce closed period",
        valueTypes: new[] { BooleanType })]
    [CaseValidateAction("FieldPeriodAfter", "Validate for moment after field period", "Validation", "FieldPeriod")]
    public void FieldPeriodAfter(CaseChangeActionContext context, string fieldName, object moment, bool closedPeriod = false)
    {
        // period start
        var startValue = GetActionValue<DateTime?>(context, ActionCaseValueBase.ToStartReference(fieldName));
        var start = startValue?.ResolvedValue;
        if (!start.HasValue)
        {
            AddIssue(context, "ComparePeriodWithoutStartDate", fieldName, startValue);
            return;
        }
        // period end
        var endValue = GetActionValue<DateTime?>(context, ActionCaseValueBase.ToEndReference(fieldName));
        var end = endValue?.ResolvedValue;
        if (closedPeriod && !end.HasValue)
        {
            AddIssue(context, "ComparePeriodOpenPeriod", fieldName);
            return;
        }
        end ??= Date.MaxValue;

        // test moment
        var momentValue = GetActionValue<DateTime?>(context, moment);
        if (momentValue?.ResolvedValue == null)
        {
            AddIssue(context, "ComparePeriodInvalidTestDate", fieldName, moment);
            return;
        }

        // test
        if (momentValue.ResolvedValue <= end)
        {
            AddIssue(context, "ComparePeriodNotAfter", fieldName, momentValue, GetPeriodString(startValue, endValue));
        }
    }

    /// <summary>Validate for moment not after field period</summary>
    /// <param name="context">The action context</param>
    /// <param name="fieldName">The field name</param>
    /// <param name="moment">The moment to test</param>
    /// <param name="closedPeriod">Enforce closed period</param>
    [ActionIssue("ComparePeriodWithoutStartDate", "(0) period without start (1)", 2)]
    [ActionIssue("ComparePeriodInvalidTestDate", "(0) invalid test date (1)", 2)]
    [ActionIssue("ComparePeriodOpenPeriod", "Value (0) with open period", 1)]
    [ActionIssue("ComparePeriodAfter", "(0) (1) is after (2)", 3)]
    [ActionParameter("fieldName", "The field name",
        valueReferences: new[] { ActionReferenceCaseChange, ActionReferenceCaseValue })]
    [ActionParameter("moment", "The moment to test",
        valueTypes: new[] { DateType })]
    [ActionParameter("closedPeriod", "Enforce closed period",
        valueTypes: new[] { BooleanType })]
    [CaseValidateAction("FieldPeriodNotAfter", "Validate for moment not after field period", "Validation", "FieldPeriod")]
    public void FieldPeriodNotAfter(CaseChangeActionContext context, string fieldName, object moment, bool closedPeriod = false)
    {
        // period start
        var startValue = GetActionValue<DateTime?>(context, ActionCaseValueBase.ToStartReference(fieldName));
        var start = startValue?.ResolvedValue;
        if (!start.HasValue)
        {
            AddIssue(context, "ComparePeriodWithoutStartDate", fieldName, startValue);
            return;
        }
        // period end
        var endValue = GetActionValue<DateTime?>(context, ActionCaseValueBase.ToEndReference(fieldName));
        var end = endValue?.ResolvedValue;
        if (closedPeriod && !end.HasValue)
        {
            AddIssue(context, "ComparePeriodOpenPeriod", fieldName);
            return;
        }
        end ??= Date.MaxValue;

        // test moment
        var momentValue = GetActionValue<DateTime?>(context, moment);
        if (momentValue?.ResolvedValue == null)
        {
            AddIssue(context, "ComparePeriodInvalidTestDate", fieldName, moment);
            return;
        }

        // test
        if (momentValue.ResolvedValue > end)
        {
            AddIssue(context, "ComparePeriodAfter", fieldName, momentValue, GetPeriodString(startValue, endValue));
        }
    }

    /// <summary>Validate for period overlapping the field period</summary>
    /// <param name="context">The action context</param>
    /// <param name="fieldName">The field name</param>
    /// <param name="periodStart">The period start date</param>
    /// <param name="periodEnd">The period end date</param>
    /// <param name="closedPeriod">Enforce closed period</param>
    [ActionIssue("ComparePeriodWithoutStartDate", "(0) period without start (1)", 2)]
    [ActionIssue("ComparePeriodInvalidTestDate", "(0) invalid test date (1)", 2)]
    [ActionIssue("ComparePeriodOpenPeriod", "Value (0) with open period", 1)]
    [ActionIssue("ComparePeriodInvalidPeriodStart", "(0) Invalid period start (1)", 2)]
    [ActionIssue("ComparePeriodInvalidPeriodEnd", "(0) Invalid period end (1)", 2)]
    [ActionIssue("ComparePeriodNotOverlap", "(0) (1) is not overlapping (2)", 3)]
    [ActionParameter("fieldName", "The field name",
        valueReferences: new[] { ActionReferenceCaseChange, ActionReferenceCaseValue })]
    [ActionParameter("periodStart", "The period start date",
        valueTypes: new[] { DateType })]
    [ActionParameter("periodEnd", "The period end date",
        valueTypes: new[] { DateType })]
    [ActionParameter("closedPeriod", "Enforce closed period",
        valueTypes: new[] { BooleanType })]
    [CaseValidateAction("FieldPeriodOverlap", "Validate for period overlapping the field period", "Validation", "FieldPeriod")]
    public void FieldPeriodOverlap(CaseChangeActionContext context, string fieldName, object periodStart,
        object periodEnd, bool closedPeriod = false)
    {
        // period start
        var startValue = GetActionValue<DateTime?>(context, ActionCaseValueBase.ToStartReference(fieldName));
        var start = startValue?.ResolvedValue;
        if (!start.HasValue)
        {
            AddIssue(context, "ComparePeriodWithoutStartDate", fieldName, startValue);
            return;
        }
        // period end
        var endValue = GetActionValue<DateTime?>(context, ActionCaseValueBase.ToEndReference(fieldName));
        var end = endValue?.ResolvedValue;
        if (closedPeriod && !end.HasValue)
        {
            AddIssue(context, "ComparePeriodOpenPeriod", fieldName);
            return;
        }
        end ??= Date.MaxValue;

        // test period
        var periodStartValue = GetActionValue<DateTime?>(context, periodStart);
        if (periodStartValue?.ResolvedValue == null)
        {
            AddIssue(context, "ComparePeriodInvalidPeriodStart", fieldName, periodStart);
            return;
        }
        var periodEndValue = GetActionValue<DateTime?>(context, periodEnd);
        if (periodEndValue?.ResolvedValue == null)
        {
            AddIssue(context, "ComparePeriodInvalidPeriodEnd", fieldName, periodEnd);
            return;
        }

        // test
        if (start >= periodEndValue.ResolvedValue || periodStartValue.ResolvedValue >= end)
        {
            AddIssue(context, "ComparePeriodNotOverlap", fieldName,
                GetPeriodString(periodStartValue, periodEndValue), GetPeriodString(startValue, endValue));
        }
    }

    /// <summary>Validate for period not overlapping the field period</summary>
    /// <param name="context">The action context</param>
    /// <param name="fieldName">The field name</param>
    /// <param name="periodStart">The period start date</param>
    /// <param name="periodEnd">The period end date</param>
    /// <param name="closedPeriod">Enforce closed period</param>
    [ActionIssue("ComparePeriodWithoutStartDate", "(0) period without start (1)", 2)]
    [ActionIssue("ComparePeriodInvalidTestDate", "(0) invalid test date (1)", 2)]
    [ActionIssue("ComparePeriodOpenPeriod", "Value (0) with open period", 1)]
    [ActionIssue("ComparePeriodInvalidPeriodStart", "(0) invalid period start (1)", 2)]
    [ActionIssue("ComparePeriodInvalidPeriodEnd", "(0) invalid period end (1)", 2)]
    [ActionIssue("ComparePeriodOverlap", "(0) (1) is overlapping (2)", 3)]
    [ActionParameter("fieldName", "The field name",
        valueReferences: new[] { ActionReferenceCaseChange, ActionReferenceCaseValue })]
    [ActionParameter("periodStart", "The period start date",
        valueTypes: new[] { DateType })]
    [ActionParameter("periodEnd", "The period end date",
        valueTypes: new[] { DateType })]
    [ActionParameter("closedPeriod", "Enforce closed period",
        valueTypes: new[] { BooleanType })]
    [CaseValidateAction("FieldPeriodNotOverlap", "Validate for period not overlapping the field period", "Validation", "FieldPeriod")]
    public void FieldPeriodNotOverlap(CaseChangeActionContext context, string fieldName, object periodStart,
        object periodEnd, bool closedPeriod = false)
    {
        // period start
        var startValue = GetActionValue<DateTime?>(context, ActionCaseValueBase.ToStartReference(fieldName));
        var start = startValue?.ResolvedValue;
        if (!start.HasValue)
        {
            AddIssue(context, "ComparePeriodWithoutStartDate", fieldName, startValue);
            return;
        }
        // period end
        var endValue = GetActionValue<DateTime?>(context, ActionCaseValueBase.ToEndReference(fieldName));
        var end = endValue?.ResolvedValue;
        if (closedPeriod && !end.HasValue)
        {
            AddIssue(context, "ComparePeriodOpenPeriod", fieldName);
            return;
        }
        end ??= Date.MaxValue;

        // test period
        var periodStartValue = GetActionValue<DateTime?>(context, periodStart);
        if (periodStartValue?.ResolvedValue == null)
        {
            AddIssue(context, "ComparePeriodInvalidPeriodStart", fieldName, periodStart);
            return;
        }
        var periodEndValue = GetActionValue<DateTime?>(context, periodEnd);
        if (periodEndValue?.ResolvedValue == null)
        {
            AddIssue(context, "ComparePeriodInvalidPeriodEnd", fieldName, periodEnd);
            return;
        }

        // test
        if (start < periodEndValue.ResolvedValue && periodStartValue.ResolvedValue < end)
        {
            AddIssue(context, "ComparePeriodOverlap", fieldName,
                GetPeriodString(periodStartValue, periodEndValue), GetPeriodString(startValue, endValue));
        }
    }

    private static string GetPeriodString(ActionCaseValue<CaseChangeActionValueContext, CaseChangeFunction, DateTime?> startValue,
        ActionCaseValue<CaseChangeActionValueContext, CaseChangeFunction, DateTime?> endValue)
    {
        return endValue.ResolvedValue.HasValue ?
            $"[{startValue} - {endValue}]" :
            $"[{startValue} - open]";
    }

    #endregion

    #region Validate Case Period

    /// <summary>Validate for moment before period</summary>
    /// <param name="context">The action context</param>
    /// <param name="moment">The moment to test</param>
    [ActionParameter("moment", "The moment to test",
        valueTypes: new[] { DateType })]
    [CaseValidateAction("PeriodBefore", "Validate for moment before period", "Validation", "FieldPeriod")]
    public void PeriodBefore(CaseChangeActionContext context, object moment) =>
        FieldPeriodBefore(context, ActionCaseValueBase.ToCaseChangeReference(context.CaseFieldName), moment);

    /// <summary>Validate for moment not before period</summary>
    /// <param name="context">The action context</param>
    /// <param name="moment">The moment to test</param>
    [ActionParameter("moment", "The moment to test",
        valueTypes: new[] { DateType })]
    [CaseValidateAction("PeriodNotBefore", "Validate for moment not before period", "Validation", "FieldPeriod")]
    public void PeriodNotBefore(CaseChangeActionContext context, object moment) =>
        FieldPeriodNotBefore(context, ActionCaseValueBase.ToCaseChangeReference(context.CaseFieldName), moment);

    /// <summary>Validate for moment within period</summary>
    /// <param name="context">The action context</param>
    /// <param name="moment">The moment to test</param>
    /// <param name="closedPeriod">Enforce closed period</param>
    [ActionParameter("moment", "The moment to test",
        valueTypes: new[] { DateType })]
    [ActionParameter("closedPeriod", "Enforce closed period",
        valueTypes: new[] { BooleanType })]
    [CaseValidateAction("PeriodWithin", "Validate for moment within period", "Validation", "FieldPeriod")]
    public void PeriodWithin(CaseChangeActionContext context, object moment, bool closedPeriod = false) =>
        FieldPeriodWithin(context, ActionCaseValueBase.ToCaseChangeReference(context.CaseFieldName), moment, closedPeriod);

    /// <summary>Validate for moment not within period</summary>
    /// <param name="context">The action context</param>
    /// <param name="moment">The moment to test</param>
    /// <param name="closedPeriod">Enforce closed period</param>
    [ActionParameter("moment", "The moment to test",
        valueTypes: new[] { DateType })]
    [ActionParameter("closedPeriod", "Enforce closed period",
        valueTypes: new[] { BooleanType })]
    [CaseValidateAction("PeriodNotWithin", "Validate for moment not within period", "Validation", "FieldPeriod")]
    public void PeriodNotWithin(CaseChangeActionContext context, object moment, bool closedPeriod = false) =>
        FieldPeriodNotWithin(context, ActionCaseValueBase.ToCaseChangeReference(context.CaseFieldName), moment, closedPeriod);

    /// <summary>Validate for moment after period</summary>
    /// <param name="context">The action context</param>
    /// <param name="moment">The moment to test</param>
    /// <param name="closedPeriod">Enforce closed period</param>
    [ActionParameter("moment", "The moment to test",
        valueTypes: new[] { DateType })]
    [ActionParameter("closedPeriod", "Enforce closed period",
        valueTypes: new[] { BooleanType })]
    [CaseValidateAction("PeriodAfter", "Validate for moment after period", "Validation", "FieldPeriod")]
    public void PeriodAfter(CaseChangeActionContext context, object moment, bool closedPeriod = false) =>
        FieldPeriodAfter(context, ActionCaseValueBase.ToCaseChangeReference(context.CaseFieldName), moment, closedPeriod);

    /// <summary>Validate for moment not after period</summary>
    /// <param name="context">The action context</param>
    /// <param name="moment">The moment to test</param>
    /// <param name="closedPeriod">Enforce closed period</param>
    [ActionParameter("moment", "The moment to test",
        valueTypes: new[] { DateType })]
    [ActionParameter("closedPeriod", "Enforce closed period",
        valueTypes: new[] { BooleanType })]
    [CaseValidateAction("PeriodNotAfter", "Validate for moment not after period", "Validation", "FieldPeriod")]
    public void PeriodNotAfter(CaseChangeActionContext context, object moment, bool closedPeriod = false) =>
        FieldPeriodNotAfter(context, ActionCaseValueBase.ToCaseChangeReference(context.CaseFieldName), moment, closedPeriod);

    /// <summary>Validate for period overlapping the period</summary>
    /// <param name="context">The action context</param>
    /// <param name="periodStart">The period start date</param>
    /// <param name="periodEnd">The period end date</param>
    /// <param name="closedPeriod">Enforce closed period</param>
    [ActionParameter("periodStart", "The period start date",
        valueTypes: new[] { DateType })]
    [ActionParameter("periodEnd", "The period end date",
        valueTypes: new[] { DateType })]
    [ActionParameter("closedPeriod", "Enforce closed period",
        valueTypes: new[] { BooleanType })]
    [CaseValidateAction("PeriodOverlap", "Validate for period overlapping the period", "Validation", "FieldPeriod")]
    public void PeriodOverlap(CaseChangeActionContext context, object periodStart, object periodEnd, bool closedPeriod = false) =>
        FieldPeriodOverlap(context, ActionCaseValueBase.ToCaseChangeReference(context.CaseFieldName), periodStart, periodEnd, closedPeriod);

    /// <summary>Validate for period not overlapping the period</summary>
    /// <param name="context">The action context</param>
    /// <param name="periodStart">The period start date</param>
    /// <param name="periodEnd">The period end date</param>
    /// <param name="closedPeriod">Enforce closed period</param>
    [ActionParameter("periodStart", "The period start date",
        valueTypes: new[] { DateType })]
    [ActionParameter("periodEnd", "The period end date",
        valueTypes: new[] { DateType })]
    [ActionParameter("closedPeriod", "Enforce closed period",
        valueTypes: new[] { BooleanType })]
    [CaseValidateAction("PeriodNotOverlap", "Validate for period not overlapping the period", "Validation", "FieldPeriod")]
    public void PeriodNotOverlap(CaseChangeActionContext context, object periodStart, object periodEnd, bool closedPeriod = false) =>
        FieldPeriodNotOverlap(context, ActionCaseValueBase.ToCaseChangeReference(context.CaseFieldName), periodStart, periodEnd, closedPeriod);

    #endregion

    #region Validate String

    /// <summary>Validate for minimum string length</summary>
    /// <param name="context">The action context</param>
    /// <param name="minLength">The minimum string length</param>
    [ActionIssue("MissingCaseValue", "Missing value (0)", 1)]
    [ActionIssue("StringMinLength", "(0) must be a at least (1) characters", 2)]
    [ActionParameter("minLength", "The minimum string length",
        valueTypes: new[] { IntType })]
    [CaseValidateAction("MinLength", "Validate for minimum string length", "Validation", "FieldValue")]
    public void MinLength(CaseChangeActionContext context, object minLength)
    {
        var sourceValue = GetActionValue<string>(context);
        var minLengthValue = GetActionValue<int?>(context, minLength);
        if (sourceValue == null || minLengthValue == null || !minLengthValue.IsFulfilled)
        {
            return;
        }
        if (minLengthValue.MandatoryField && sourceValue.ResolvedValue == null)
        {
            AddIssue(context, "MissingCaseValue", context.CaseFieldName);
        }
        else if (sourceValue.ResolvedValue.Length < minLengthValue.ResolvedValue)
        {
            AddIssue(context, "StringMinLength", sourceValue, minLength);
        }
    }

    /// <summary>Validate string maximal length</summary>
    /// <param name="context">The action context</param>
    /// <param name="maxLength">The maximum string length</param>
    [ActionIssue("MissingCaseValue", "Missing value (0)", 1)]
    [ActionIssue("StringMaxLength", "(0) must be (1) characters or less", 2)]
    [ActionParameter("maxLength", "The maximum string length",
        valueTypes: new[] { IntType })]
    [CaseValidateAction("MaxLength", "Validate for maximum string length", "Validation", "FieldValue")]
    public void MaxLength(CaseChangeActionContext context, object maxLength)
    {
        var sourceValue = GetActionValue<string>(context);
        var maxLengthValue = GetActionValue<int?>(context, maxLength);
        if (sourceValue == null || maxLengthValue == null || !maxLengthValue.IsFulfilled)
        {
            return;
        }
        if (maxLengthValue.MandatoryField && sourceValue.ResolvedValue == null)
        {
            AddIssue(context, "MissingCaseValue", context.CaseFieldName);
        }
        else if (sourceValue.ResolvedValue.Length > maxLengthValue.ResolvedValue)
        {
            AddIssue(context, "StringMaxLength", sourceValue, maxLength);
        }
    }

    /// <summary>Validate string length</summary>
    /// <param name="context">The action context</param>
    /// <param name="length">The string length</param>
    [ActionIssue("MissingCaseValue", "Missing value (0)", 1)]
    [ActionIssue("StringLength", "(0) must be exactly (1) characters", 2)]
    [ActionParameter("length", "The string length",
        valueTypes: new[] { IntType })]
    [CaseValidateAction("Length", "Validate string length", "Validation", "FieldValue")]
    public void Length(CaseChangeActionContext context, object length)
    {
        var sourceValue = GetActionValue<string>(context);
        var lengthValue = GetActionValue<int?>(context, length);
        if (sourceValue == null || lengthValue == null || !lengthValue.IsFulfilled)
        {
            return;
        }
        if (lengthValue.MandatoryField && sourceValue.ResolvedValue == null)
        {
            AddIssue(context, "MissingCaseValue", context.CaseFieldName);
        }
        else if (sourceValue.ResolvedValue.Length != lengthValue.ResolvedValue)
        {
            AddIssue(context, "StringLength", sourceValue, length);
        }
    }

    /// <summary>Validate string length between a range</summary>
    /// <param name="context">The action context</param>
    /// <param name="minLength">The minimum string length</param>
    /// <param name="maxLength">The maximum string length</param>
    [ActionIssue("MissingCaseValue", "Missing value (0)", 1)]
    [ActionIssue("StringLengthBetween", "(0) must be at least (1) and at most (2) characters", 3)]
    [ActionParameter("minLength", "The minimum string length",
        valueTypes: new[] { IntType })]
    [ActionParameter("maxLength", "The maximum string length",
        valueTypes: new[] { IntType })]
    [CaseValidateAction("LengthBetween", "Validate string length between a range", "Validation", "FieldValue")]
    public void LengthBetween(CaseChangeActionContext context, object minLength, object maxLength)
    {
        var sourceValue = GetActionValue<string>(context);
        var minLengthValue = GetActionValue<int?>(context, minLength);
        var maxLengthValue = GetActionValue<int?>(context, maxLength);
        if (sourceValue == null || minLengthValue == null || maxLengthValue == null ||
            !minLengthValue.IsFulfilled || !maxLengthValue.IsFulfilled)
        {
            return;
        }
        if ((minLengthValue.MandatoryField || maxLengthValue.MandatoryField) && sourceValue.ResolvedValue == null)
        {
            AddIssue(context, "MissingCaseValue", context.CaseFieldName);
        }
        else if (sourceValue.ResolvedValue.Length < minLengthValue.ResolvedValue ||
                 sourceValue.ResolvedValue.Length > maxLengthValue.ResolvedValue)
        {
            AddIssue(context, "StringLengthBetween", sourceValue, minLength, maxLength);
        }
    }

    /// <summary>Validate text equal ignoring the character case</summary>
    /// <param name="context">The action context</param>
    /// <param name="compare">The compare value</param>
    /// <param name="ignoreCase">Ignore the character case</param>
    [ActionIssue("MissingCaseValue", "Missing value (0)", 1)]
    [ActionIssue("StringNotEqual", "(0) is not equal (1)", 2)]
    [ActionParameter("compare", "The compare value",
        valueTypes: new[] { StringType })]
    [ActionParameter("ignoreCase", "Ignore the character case",
        valueTypes: new[] { BooleanType })]
    [CaseValidateAction("EqualText", "Validate for equals text", "Validation", "FieldValue")]
    public void EqualText(CaseChangeActionContext context, object compare,
        bool ignoreCase = false)
    {
        var sourceValue = GetActionValue<string>(context);
        var compareValue = GetActionValue<string>(context, compare);
        if (sourceValue == null || compareValue == null || !compareValue.IsFulfilled)
        {
            return;
        }
        if (compareValue.MandatoryField && sourceValue.ResolvedValue == null)
        {
            AddIssue(context, "MissingCaseValue", context.CaseFieldName);
        }
        else if (!string.Equals(sourceValue.ResolvedValue, compareValue.ResolvedValue, GetCompareCulture(ignoreCase)))
        {
            AddIssue(context, "StringNotEqual", sourceValue, compareValue);
        }
    }

    /// <summary>Validate for different text</summary>
    /// <param name="context">The action context</param>
    /// <param name="compare">The compare value</param>
    /// <param name="ignoreCase">Ignore the character case</param>
    [ActionIssue("MissingCaseValue", "Missing value (0)", 1)]
    [ActionIssue("StringEqual", "(0) is equal (1)", 2)]
    [ActionParameter("compare", "The compare value",
        valueTypes: new[] { StringType })]
    [ActionParameter("ignoreCase", "Ignore the character case",
        valueTypes: new[] { BooleanType })]
    [CaseValidateAction("NotEqualText", "Validate for different text", "Validation", "FieldValue")]
    public void NotEqualText(CaseChangeActionContext context, object compare,
        bool ignoreCase = false)
    {
        var sourceValue = GetActionValue<string>(context);
        var compareValue = GetActionValue<string>(context, compare);
        if (sourceValue == null || compareValue == null || !compareValue.IsFulfilled)
        {
            return;
        }
        if (compareValue.MandatoryField && sourceValue.ResolvedValue == null)
        {
            AddIssue(context, "MissingCaseValue", context.CaseFieldName);
        }
        else if (string.Equals(sourceValue.ResolvedValue, compareValue.ResolvedValue, GetCompareCulture(ignoreCase)))
        {
            AddIssue(context, "StringEqual", sourceValue, compareValue);
        }
    }

    #endregion

    private ActionValueType? ResolveCaseValueType(CaseChangeActionContext context, object source)
    {
        var caseValue = GetActionValue<object>(context, source);
        if (caseValue == null || !caseValue.IsReference)
        {
            AddIssue(context, "CompareInvalidSourceValue", context.CaseFieldName, source);
            return null;
        }

        // optional compare
        if (caseValue.ResolvedValue == null)
        {
            if (caseValue.MandatoryField)
            {
                AddIssue(context, "CompareMissingSourceValue", context.CaseFieldName, source);
            }
            return null;
        }
        return caseValue.GetValueType();
    }

    private ActionValueType? ResolveCompareValueType(CaseChangeActionContext context, object compare)
    {
        var compareValue = GetActionValue<object>(context, compare);
        if (compareValue == null || !compareValue.IsFulfilled)
        {
            AddIssue(context, "CompareInvalidCompareValue", context.CaseFieldName, compare);
            return null;
        }
        return compareValue.GetValueType();
    }
}
