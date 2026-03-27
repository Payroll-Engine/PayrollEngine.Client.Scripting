/* PayrunFunction.Action */

using System;
using System.Linq;

namespace PayrollEngine.Client.Scripting.Function;

/// <summary>Payrun function</summary>
public partial class PayrunFunction
{

    #region Wage Type and Collectors

    /// <summary>Get wage type year-to-date value by wage type number</summary>
    [ActionParameter("number", "The wage type number", [DecimalType])]
    [PayrunAction("GetCycleWageTypeValue", "Get wage type year-to-date value by wage type number", "WageType")]
    public ActionValue GetCycleWageTypeValue(decimal number)
    {
        var results = GetConsolidatedWageTypeResults(new(number, CycleStart));
        return results.Any() ? results.Sum(x => x.Value) : 0;
    }

    /// <summary>Get wage type year-to-date value by wage type name</summary>
    [ActionParameter("name", "The wage type name", [StringType])]
    [PayrunAction("GetCycleWageTypeValue", "Get wage type year-to-date value by wage type name", "WageType")]
    public ActionValue GetCycleWageTypeValue(string name) =>
        GetCycleWageTypeValue(GetWageTypeNumber(name));

    /// <summary>Get wage type value of the previous period by wage type number</summary>
    [ActionParameter("number", "The wage type number", [DecimalType])]
    [PayrunAction("GetPrevPeriodWageTypeValue", "Get wage type value of the previous period by number", "WageType")]
    public ActionValue GetPrevPeriodWageTypeValue(decimal number)
    {
        var results = GetWageTypeResults([number], PreviousPeriod.Start, PreviousPeriod.End);
        return results.Any() ? results.Sum(x => x.Value) : 0;
    }

    /// <summary>Get wage type value of the previous period by wage type name</summary>
    [ActionParameter("name", "The wage type name", [StringType])]
    [PayrunAction("GetPrevPeriodWageTypeValue", "Get wage type value of the previous period by name", "WageType")]
    public ActionValue GetPrevPeriodWageTypeValue(string name) =>
        GetPrevPeriodWageTypeValue(GetWageTypeNumber(name));

    /// <summary>Get wage type value of the next period by wage type number</summary>
    [ActionParameter("number", "The wage type number", [DecimalType])]
    [PayrunAction("GetNextPeriodWageTypeValue", "Get wage type value of the next period by number", "WageType")]
    public ActionValue GetNextPeriodWageTypeValue(decimal number)
    {
        var results = GetWageTypeResults([number], NextPeriod.Start, NextPeriod.End);
        return results.Any() ? results.Sum(x => x.Value) : 0;
    }

    /// <summary>Get wage type value of the next period by wage type name</summary>
    [ActionParameter("name", "The wage type name", [StringType])]
    [PayrunAction("GetNextPeriodWageTypeValue", "Get wage type value of the next period by name", "WageType")]
    public ActionValue GetNextPeriodWageTypeValue(string name) =>
        GetNextPeriodWageTypeValue(GetWageTypeNumber(name));

    /// <summary>Get wage type total value of the previous cycle by wage type number</summary>
    [ActionParameter("number", "The wage type number", [DecimalType])]
    [PayrunAction("GetPrevCycleWageTypeValue", "Get wage type total value of the previous cycle by number", "WageType")]
    public ActionValue GetPrevCycleWageTypeValue(decimal number)
    {
        var results = GetWageTypeResults([number], PreviousCycle.Start, PreviousCycle.End);
        return results.Any() ? results.Sum(x => x.Value) : 0;
    }

    /// <summary>Get wage type total value of the previous cycle by wage type name</summary>
    [ActionParameter("name", "The wage type name", [StringType])]
    [PayrunAction("GetPrevCycleWageTypeValue", "Get wage type total value of the previous cycle by name", "WageType")]
    public ActionValue GetPrevCycleWageTypeValue(string name) =>
        GetPrevCycleWageTypeValue(GetWageTypeNumber(name));

    /// <summary>Get wage type total value of the next cycle by wage type number</summary>
    [ActionParameter("number", "The wage type number", [DecimalType])]
    [PayrunAction("GetNextCycleWageTypeValue", "Get wage type total value of the next cycle by number", "WageType")]
    public ActionValue GetNextCycleWageTypeValue(decimal number)
    {
        var results = GetWageTypeResults([number], NextCycle.Start, NextCycle.End);
        return results.Any() ? results.Sum(x => x.Value) : 0;
    }

    /// <summary>Get wage type total value of the next cycle by wage type name</summary>
    [ActionParameter("name", "The wage type name", [StringType])]
    [PayrunAction("GetNextCycleWageTypeValue", "Get wage type total value of the next cycle by name", "WageType")]
    public ActionValue GetNextCycleWageTypeValue(string name) =>
        GetNextCycleWageTypeValue(GetWageTypeNumber(name));

    /// <summary>
    /// Get consolidated wage type value by number from a period offset to the current period.
    /// Uses the start date of the offset period as the consolidation moment.
    /// A negative offset covers past periods; for example, -11 consolidates the 12 most recent periods.
    /// </summary>
    [ActionParameter("number", "The wage type number", [DecimalType])]
    [ActionParameter("periodOffset", "Period offset relative to the current period (e.g. -11 for 12-period window)", [IntType])]
    [PayrunAction("GetConsolidatedWageTypeValue", "Get consolidated wage type value from a period offset by number", "WageType")]
    public ActionValue GetConsolidatedWageTypeValue(decimal number, int periodOffset)
    {
        var periodMoment = GetPeriod(periodOffset).Start;
        var results = GetConsolidatedWageTypeResults(new(number, periodMoment));
        return results.Any() ? results.Sum(x => x.Value) : 0;
    }

    /// <summary>
    /// Get consolidated wage type value by name from a period offset to the current period.
    /// Uses the start date of the offset period as the consolidation moment.
    /// A negative offset covers past periods; for example, -11 consolidates the 12 most recent periods.
    /// </summary>
    [ActionParameter("name", "The wage type name", [StringType])]
    [ActionParameter("periodOffset", "Period offset relative to the current period (e.g. -11 for 12-period window)", [IntType])]
    [PayrunAction("GetConsolidatedWageTypeValue", "Get consolidated wage type value from a period offset by name", "WageType")]
    public ActionValue GetConsolidatedWageTypeValue(string name, int periodOffset) =>
        GetConsolidatedWageTypeValue(GetWageTypeNumber(name), periodOffset);

    /// <summary>Get sum of retro corrections for a wage type by number</summary>
    [ActionParameter("number", "The wage type number", [DecimalType])]
    [PayrunAction("GetRetroWageTypeValueSum", "Get sum of retro wage type corrections by number", "WageType")]
    public ActionValue GetRetroWageTypeValueSumByNumber(decimal number) =>
        GetWageTypeRetroResultSum(number);

    /// <summary>Get sum of retro corrections for a wage type by name</summary>
    [ActionParameter("name", "The wage type name", [StringType])]
    [PayrunAction("GetRetroWageTypeValueSum", "Get sum of retro wage type corrections by name", "WageType")]
    public ActionValue GetRetroWageTypeValueSumByName(string name) =>
        GetWageTypeRetroResultSum(GetWageTypeNumber(name));

    /// <summary>Get collector year-to-date value</summary>
    [ActionParameter("name", "The collector name", [StringType])]
    [PayrunAction("GetCycleCollectorValue", "Get collector year-to-date value", "Collector")]
    public ActionValue GetCycleCollectorValue(string name)
    {
        var results = GetConsolidatedCollectorResults(new([name], CycleStart));
        return results.Any() ? results.Sum(x => x.Value) : 0;
    }

    /// <summary>Get collector value of the previous period</summary>
    [ActionParameter("name", "The collector name", [StringType])]
    [PayrunAction("GetPrevPeriodCollectorValue", "Get collector value of the previous period", "Collector")]
    public ActionValue GetPrevPeriodCollectorValue(string name)
    {
        var results = GetCollectorResults([name], PreviousPeriod.Start, PreviousPeriod.End);
        return results.Any() ? results.Sum(x => x.Value) : 0;
    }

    /// <summary>Get collector value of the next period</summary>
    [ActionParameter("name", "The collector name", [StringType])]
    [PayrunAction("GetNextPeriodCollectorValue", "Get collector value of the next period", "Collector")]
    public ActionValue GetNextPeriodCollectorValue(string name)
    {
        var results = GetCollectorResults([name], NextPeriod.Start, NextPeriod.End);
        return results.Any() ? results.Sum(x => x.Value) : 0;
    }

    /// <summary>Get collector total value of the previous cycle</summary>
    [ActionParameter("name", "The collector name", [StringType])]
    [PayrunAction("GetPrevCycleCollectorValue", "Get collector total value of the previous cycle", "Collector")]
    public ActionValue GetPrevCycleCollectorValue(string name)
    {
        var results = GetCollectorResults([name], PreviousCycle.Start, PreviousCycle.End);
        return results.Any() ? results.Sum(x => x.Value) : 0;
    }

    /// <summary>Get collector total value of the next cycle</summary>
    [ActionParameter("name", "The collector name", [StringType])]
    [PayrunAction("GetNextCycleCollectorValue", "Get collector total value of the next cycle", "Collector")]
    public ActionValue GetNextCycleCollectorValue(string name)
    {
        var results = GetCollectorResults([name], NextCycle.Start, NextCycle.End);
        return results.Any() ? results.Sum(x => x.Value) : 0;
    }

    /// <summary>
    /// Get consolidated collector value from a period offset to the current period.
    /// Uses the start date of the offset period as the consolidation moment.
    /// A negative offset covers past periods; for example, -11 consolidates the 12 most recent periods.
    /// </summary>
    [ActionParameter("name", "The collector name", [StringType])]
    [ActionParameter("periodOffset", "Period offset relative to the current period (e.g. -11 for 12-period window)", [IntType])]
    [PayrunAction("GetConsolidatedCollectorValue", "Get consolidated collector value from a period offset", "Collector")]
    public ActionValue GetConsolidatedCollectorValue(string name, int periodOffset)
    {
        var periodMoment = GetPeriod(periodOffset).Start;
        var results = GetConsolidatedCollectorResults(new([name], periodMoment));
        return results.Any() ? results.Sum(x => x.Value) : 0;
    }

    #endregion

    #region Payroll Calculation

    /// <summary>
    /// Project a per-period amount to a cycle basis, apply a progressive range lookup,
    /// and return the per-period share of the result.
    /// Formula: Round(ApplyRangeValue(lookup, amount × PeriodsInCycle) / PeriodsInCycle, 2)
    /// </summary>
    [ActionParameter("periodAmount", "Per-period gross amount to project", [DecimalType])]
    [ActionParameter("lookup", "Range lookup name containing the cycle-basis rate or amount table", [StringType])]
    [PayrunAction("AnnualProjection", "Project per-period amount via cycle range lookup and return per-period share", "Calculation")]
    public ActionValue AnnualProjection(decimal periodAmount, string lookup)
    {
        var cycleAmount = periodAmount * PeriodsInCycle;
        var cycleResult = ApplyRangeValue(lookup, cycleAmount);
        return Math.Round(cycleResult / PeriodsInCycle, 2);
    }

    /// <summary>
    /// Convert a cycle amount to the equivalent per-period amount.
    /// Formula: cycleAmount / PeriodsInCycle
    /// Applies to any amount defined on a full-cycle basis that must be distributed
    /// evenly across the payrun periods in the cycle.
    /// </summary>
    [ActionParameter("cycleAmount", "Amount defined on a full-cycle basis", [DecimalType])]
    [PayrunAction("CycleToPeriod", "Convert a cycle amount to the per-period equivalent", "Calculation")]
    public ActionValue CycleToPeriod(decimal cycleAmount) =>
        cycleAmount / PeriodsInCycle;

    /// <summary>
    /// Convert a per-period amount to the equivalent cycle amount.
    /// Formula: periodAmount × PeriodsInCycle
    /// Applies to any amount defined per period that must be projected to the full cycle
    /// for limit checks, rate lookups, or cycle-level reporting.
    /// </summary>
    [ActionParameter("periodAmount", "Amount defined on a per-period basis", [DecimalType])]
    [PayrunAction("PeriodToCycle", "Convert a per-period amount to the cycle equivalent", "Calculation")]
    public ActionValue PeriodToCycle(decimal periodAmount) =>
        periodAmount * PeriodsInCycle;

    /// <summary>
    /// Round an amount to the nearest fraction defined by a denominator.
    /// Formula: Round(amount × denominator) / denominator
    /// Common denominators: 100 = cents, 20 = twentieth, 4 = quarter, 2 = half.
    /// Returns 0 when denominator is 0 or negative.
    /// </summary>
    [ActionParameter("amount", "Amount to round", [DecimalType])]
    [ActionParameter("denominator", "Rounding denominator (e.g. 100 for cents, 20 for twentieth)", [DecimalType])]
    [PayrunAction("RoundToFraction", "Round an amount to the nearest 1/denominator", "Calculation")]
    public ActionValue RoundToFraction(decimal amount, decimal denominator)
    {
        if (denominator <= 0m)
        {
            return 0m;
        }

        return Math.Round(amount * denominator) / denominator;
    }

    /// <summary>
    /// Calculate a contribution on a capped cycle basis and return the per-period amount.
    /// Formula: Round(Min(periodAmount × PeriodsInCycle, cycleCap) / PeriodsInCycle × rate, 2)
    /// Returns 0 when cycleCap is 0 or negative.
    /// </summary>
    [ActionParameter("periodAmount", "Per-period gross amount", [DecimalType])]
    [ActionParameter("rate", "Contribution rate (e.g. 0.0274 for 2.74%)", [DecimalType])]
    [ActionParameter("cycleCap", "Cycle cap on the contribution base (0 = no cap)", [DecimalType])]
    [PayrunAction("CappedContribution", "Contribution on capped cycle basis, returned as per-period amount", "Calculation")]
    public ActionValue CappedContribution(decimal periodAmount, decimal rate, decimal cycleCap)
    {
        if (cycleCap <= 0m)
        {
            return 0m;
        }

        var cycleBase = Math.Min(periodAmount * PeriodsInCycle, cycleCap);
        return Math.Round(cycleBase / PeriodsInCycle * rate, 2);
    }

    /// <summary>
    /// Apply a flat rate to a wage and return the rounded contribution amount.
    /// Formula: Round(wage × rate, 2)
    /// Returns 0 when wage or rate is 0.
    /// Use when the contribution base is already the correct period amount and no
    /// cycle projection or capping is required.
    /// </summary>
    [ActionParameter("wage", "Contribution base (wage or collector value)", [DecimalType])]
    [ActionParameter("rate", "Contribution rate (e.g. 0.1307 for 13.07%)", [DecimalType])]
    [PayrunAction("RateContribution", "Apply a flat rate to a wage and return the rounded contribution", "Calculation")]
    public ActionValue RateContribution(decimal wage, decimal rate) =>
        Math.Round(wage * rate, 2);

    /// <summary>
    /// Read a rate from a lookup table and apply it to a wage.
    /// Formula: Round(wage × GetLookup(lookup, key), 2)
    /// Returns 0 when the lookup value is not found or the wage is 0.
    /// Use when the contribution rate is stored in a data regulation lookup and may
    /// change from cycle to cycle without requiring a script update.
    /// </summary>
    [ActionParameter("wage", "Contribution base (wage or collector value)", [DecimalType])]
    [ActionParameter("lookup", "Lookup name containing the rate value", [StringType])]
    [ActionParameter("key", "Lookup key to retrieve the rate (e.g. year, code, or category)", [StringType])]
    [PayrunAction("LookupRateContribution", "Read a rate from a lookup table and apply it to a wage", "Calculation")]
    public ActionValue LookupRateContribution(decimal wage, string lookup, string key)
    {
        if (wage == 0m)
        {
            return 0m;
        }

        var rate = GetLookup<decimal>(lookup, key);
        return Math.Round(wage * rate, 2);
    }

    /// <summary>
    /// Single-phase income-dependent reduction (phase-out).
    /// Formula: Max(0, maxAmount - Max(0, income - threshold) × phaseOutRate)
    /// Returns 0 when the income exceeds the point at which the reduction is fully phased out.
    /// Both input and output use the same unit; the caller is responsible for any period conversion.
    /// </summary>
    [ActionParameter("income", "Income against which the phase-out is calculated", [DecimalType])]
    [ActionParameter("maxAmount", "Maximum reduction amount at or below the threshold", [DecimalType])]
    [ActionParameter("threshold", "Income level above which the reduction starts to decrease", [DecimalType])]
    [ActionParameter("phaseOutRate", "Rate at which the reduction decreases per unit of income above threshold", [DecimalType])]
    [PayrunAction("PhaseOut", "Income-dependent single-phase reduction: Max(0, max - Max(0, income - threshold) × rate)", "Calculation")]
    public ActionValue PhaseOut(decimal income, decimal maxAmount, decimal threshold, decimal phaseOutRate)
    {
        var reduction = maxAmount - Math.Max(0m, (income - threshold) * phaseOutRate);
        return Math.Max(0m, reduction);
    }

    /// <summary>
    /// Linear phase-out of an amount between a full-value threshold and a zero threshold.
    /// Returns maxAmount when value is at or below fullUpTo, zero when value is at or above zeroFrom,
    /// and a linearly interpolated amount in between.
    /// </summary>
    [ActionParameter("value", "Current value to evaluate", [DecimalType])]
    [ActionParameter("maxAmount", "Amount returned when value is at or below fullUpTo", [DecimalType])]
    [ActionParameter("fullUpTo", "Threshold at or below which the full amount applies", [DecimalType])]
    [ActionParameter("zeroFrom", "Threshold at or above which the amount is zero", [DecimalType])]
    [PayrunAction("LinearPhaseOut", "Linear phase-out between fullUpTo and zeroFrom thresholds", "Calculation")]
    public ActionValue LinearPhaseOut(decimal value, decimal maxAmount, decimal fullUpTo, decimal zeroFrom)
    {
        if (value <= fullUpTo)
        {
            return maxAmount;
        }

        if (value >= zeroFrom)
        {
            return 0m;
        }

        var phaseOutRange = zeroFrom - fullUpTo;
        var phaseOutAmount = (value - fullUpTo) / phaseOutRange * maxAmount;
        return Math.Max(0m, maxAmount - phaseOutAmount);
    }

    /// <summary>
    /// Multiphase build-up: accumulates an amount across ordered income thresholds, each with its own rate.
    /// Phases are evaluated in ascending order; each phase covers the income between its lower and upper bound.
    /// The first phase starts at 0. The phases parameter is a flat array of alternating (upperBound, rate) pairs.
    /// The caller is responsible for any period conversion of the result.
    /// </summary>
    [ActionParameter("income", "Income to evaluate against the phase thresholds", [DecimalType])]
    [ActionParameter("phases", "Alternating upperBound and rate pairs defining each phase", [DecimalType])]
    [PayrunAction("PhaseIn", "Multi-phase build-up accumulator across income thresholds", "Calculation")]
    public ActionValue PhaseIn(decimal income, params decimal[] phases)
    {
        if (phases == null || phases.Length < 2 || phases.Length % 2 != 0)
        {
            return 0m;
        }

        var total = 0m;
        var lowerBound = 0m;

        for (var i = 0; i < phases.Length; i += 2)
        {
            var upperBound = phases[i];
            var rate = phases[i + 1];

            if (income <= lowerBound)
            {
                break;
            }

            var bandIncome = Math.Min(income, upperBound) - lowerBound;
            total += bandIncome * rate;
            lowerBound = upperBound;
        }

        return total;
    }

    /// <summary>
    /// Pro-rate an amount from actual days to target days.
    /// Formula: actualDays > 0 ? Round(amount / actualDays × targetDays, 2) : 0
    /// Returns 0 when actualDays is 0 or negative, avoiding division by zero.
    /// Typical use: extrapolate a partial-period amount to a full reference period,
    /// or scale a full-period amount down to effective worked days.
    /// </summary>
    [ActionParameter("amount", "Amount to pro-rate", [DecimalType])]
    [ActionParameter("actualDays", "Actual number of days the amount covers", [DecimalType])]
    [ActionParameter("targetDays", "Target number of days to scale the amount to", [DecimalType])]
    [PayrunAction("ProRateByDays", "Pro-rate an amount from actual days to target days", "Calculation")]
    public ActionValue ProRateByDays(decimal amount, decimal actualDays, decimal targetDays)
    {
        if (actualDays <= 0m)
        {
            return 0m;
        }

        return Math.Round(amount / actualDays * targetDays, 2);
    }

    /// <summary>
    /// Calculate the insurance wage for a period based on accumulated collector and prior wage,
    /// bounded by pro-rated annual minimum and maximum wage limits.
    /// This implements the standard SV insurance wage calculation pattern used across
    /// multiple insurance types and jurisdictions.
    ///
    /// The annual limits are pro-rated to the accumulated SV days using the configured
    /// days-in-year basis (e.g. 360 for Swiss, 365 for other jurisdictions).
    ///
    /// Behaviour by accumulated collector value:
    ///   collector ≥ pro-rated max  → result = proRatedMax − proRatedMin − prevWage
    ///   collector ≥ pro-rated min  → result = collector − proRatedMin − prevWage
    ///   collector ≥ 0              → result = 0 − prevWage  (only a refund of prior overpayment)
    ///   collector &lt; 0           → result = collector − prevWage  (negative correction)
    ///
    /// Returns 0 when collector is 0.
    /// </summary>
    [ActionParameter("collector", "Total accumulated collector value for the period (current + prior periods)", [DecimalType])]
    [ActionParameter("prevWage", "Sum of insurance wages already calculated in prior periods of this cycle", [DecimalType])]
    [ActionParameter("annualMinWage", "Annual minimum insured wage (0 = no minimum)", [DecimalType])]
    [ActionParameter("annualMaxWage", "Annual maximum insured wage (0 = no cap)", [DecimalType])]
    [ActionParameter("svDays", "Accumulated SV days from the accumulation period start to the current period end", [DecimalType])]
    [ActionParameter("daysInYear", "Day basis used to pro-rate annual limits (e.g. 360 or 365)", [DecimalType])]
    [PayrunAction("InsuranceWage", "Insurance wage bounded by pro-rated annual min/max limits", "Calculation")]
    public ActionValue InsuranceWage(
        decimal collector,
        decimal prevWage,
        decimal annualMinWage,
        decimal annualMaxWage,
        decimal svDays,
        decimal daysInYear)
    {
        if (collector == 0m)
        {
            return 0m;
        }

        if (daysInYear <= 0m || svDays <= 0m)
        {
            return 0m;
        }

        var proRatedMin = annualMinWage / daysInYear * svDays;
        var proRatedMax = annualMaxWage / daysInYear * svDays;

        decimal result;
        if (annualMaxWage > 0m && collector >= proRatedMax)
        {
            result = proRatedMax - proRatedMin - prevWage;
        }
        else if (collector >= proRatedMin)
        {
            result = collector - proRatedMin - prevWage;
        }
        else if (collector >= 0m)
        {
            result = -prevWage;
        }
        else
        {
            result = collector - prevWage;
        }

        return result;
    }

    /// <summary>
    /// Calculate the period delta for an accumulating tax or contribution using the D2 correction formula.
    /// Formula: (ytdWage + currentWage) × rate − ytdAlreadyPaid
    ///
    /// Returns a positive value when the cumulative liability increases (additional deduction),
    /// and a negative value when it decreases (refund, e.g. after a downward correction).
    ///
    /// Typical use: income tax and social-contribution calculations that accumulate over the
    /// cycle and must produce the exact delta for the current period rather than a flat rate
    /// applied to the current period's income alone.
    ///
    /// The caller supplies:
    ///   ytdWage         — sum of the taxable wage from all prior periods (via ^$.Cycle or GetConsolidatedWageTypeValue)
    ///   currentWage     — taxable wage of the current period
    ///   ytdAlreadyPaid  — sum of the tax or contribution already deducted in prior periods (via ^$.Cycle)
    ///   rate            — applicable rate for the current period (from lookup or case value)
    /// </summary>
    [ActionParameter("ytdWage", "Sum of the taxable wage from all prior periods of the current cycle", [DecimalType])]
    [ActionParameter("currentWage", "Taxable wage of the current period", [DecimalType])]
    [ActionParameter("ytdAlreadyPaid", "Sum of the tax or contribution already deducted in prior periods", [DecimalType])]
    [ActionParameter("rate", "Applicable rate for the full year-to-date liability calculation", [DecimalType])]
    [PayrunAction("D2Delta", "Period delta for accumulating tax via D2 formula: (ytdWage + currentWage) × rate − ytdAlreadyPaid", "Calculation")]
    public ActionValue D2Delta(decimal ytdWage, decimal currentWage, decimal ytdAlreadyPaid, decimal rate) =>
        (ytdWage + currentWage) * rate - ytdAlreadyPaid;

    /// <summary>
    /// Clamp a wage to an optional minimum and maximum, then apply a contribution rate.
    /// Formula: Round(Min(Max(wage, minWage), maxWage) × rate, 2)
    ///
    /// The wage is first raised to minWage if it falls below it (e.g. statutory minimum wage floor),
    /// then capped at maxWage if it exceeds it (e.g. social insurance ceiling).
    /// Pass 0 for minWage to skip the minimum, or 0 for maxWage to skip the maximum.
    ///
    /// Returns 0 when wage is 0.
    /// </summary>
    [ActionParameter("wage", "Base wage before applying the contribution", [DecimalType])]
    [ActionParameter("rate", "Contribution rate (e.g. 0.05 for 5%)", [DecimalType])]
    [ActionParameter("minWage", "Minimum contribution base; wage is raised to this value if below it (0 = no minimum)", [DecimalType])]
    [ActionParameter("maxWage", "Maximum contribution base; wage is capped at this value if above it (0 = no maximum)", [DecimalType])]
    [PayrunAction("MinMaxContribution", "Apply a rate to a wage clamped between optional minimum and maximum bounds", "Calculation")]
    public ActionValue MinMaxContribution(decimal wage, decimal rate, decimal minWage, decimal maxWage)
    {
        if (wage == 0m)
        {
            return 0m;
        }

        var base_ = wage;

        if (minWage > 0m)
        {
            base_ = Math.Max(base_, minWage);
        }

        if (maxWage > 0m)
        {
            base_ = Math.Min(base_, maxWage);
        }

        return Math.Round(base_ * rate, 2);
    }

    /// <summary>
    /// Apply a contribution rate to a wage only when a boolean case field indicates obligation.
    /// Returns Round(wage × rate, 2) when the obligation field is true, and 0 otherwise.
    ///
    /// Typical use: social insurance contributions that are only applicable to certain
    /// employee categories (e.g. mandatory pension, unemployment insurance, accident insurance),
    /// where the obligation status is stored as a case field and may change over time.
    /// </summary>
    [ActionParameter("wage", "Contribution base (wage or collector value)", [DecimalType])]
    [ActionParameter("rate", "Contribution rate to apply when obligated", [DecimalType])]
    [ActionParameter("obligationField", "Fully qualified case field name of the boolean obligation flag", [StringType])]
    [PayrunAction("ContributionIfObligated", "Apply a contribution rate only when the obligation case field is true", "Calculation")]
    public ActionValue ContributionIfObligated(decimal wage, decimal rate, string obligationField)
    {
        if (!GetCaseValue<bool>(obligationField))
        {
            return 0m;
        }

        return Math.Round(wage * rate, 2);
    }

    #endregion

    #region Runtime Values

    /// <summary>Get runtime action value</summary>
    [ActionParameter("key", "The value key", [StringType])]
    [PayrunAction("GetRuntimeValue", "Get payrun runtime value", "Runtime")]
    public ActionValue GetRuntimeValue(string key) =>
        GetEmployeeRuntimeValue(key);

    /// <summary>Set runtime action value</summary>
    [ActionParameter("key", "The value key", [StringType])]
    [ActionParameter("value", "The value to set")]
    [PayrunAction("SetRuntimeValue", "Set payrun runtime value", "Runtime")]
    public void SetRuntimeValue(string key, object value) =>
        SetEmployeeRuntimeValue(key, ActionValue.From(value)?.AsString);

    /// <summary>Remove runtime action value</summary>
    [ActionParameter("key", "The value key", [StringType])]
    [PayrunAction("RemoveRuntimeValue", "Remove payrun runtime value", "Runtime")]
    public void RemoveRuntimeValue(string key) =>
        SetRuntimeValue(key, ActionValue.Null);

    #endregion

    #region Payrun Result

    /// <summary>Get payrun result value</summary>
    [ActionParameter("name", "The result name", [StringType])]
    [PayrunAction("GetPayrunResultValue", "Get payrun result value", "Payrun")]
    public ActionValue GetPayrunResultValue(string name) =>
        new(GetPayrunResult(name));

    /// <summary>Set payrun result value</summary>
    [ActionParameter("name", "The result name", [StringType])]
    [ActionParameter("value", "The value to set")]
    [ActionParameter("type", "The value type (default: Money)", [StringType])]
    [PayrunAction("SetPayrunResultValue", "Set payrun result value", "Payrun")]
    public void SetPayrunResultValue(string name, object value, string type = null)
    {
        var actionValue = ActionValue.From(value);
        if (actionValue == null || actionValue.IsNull)
        {
            return;
        }

        ValueType valueType = ValueType.Money;
        if (!string.IsNullOrWhiteSpace(type) && !Enum.TryParse(type, out valueType))
        {
            valueType = ValueType.Money;
        }
        SetPayrunResult(name, actionValue.Value, valueType);
    }

    #endregion

}
