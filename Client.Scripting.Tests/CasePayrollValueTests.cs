/* CasePayrollValueTests */

using System;
using Xunit;

namespace PayrollEngine.Client.Scripting.Tests;

/// <summary>
/// Tests for CasePayrollValue binary operators, focusing on
/// FindMatchingPeriodValue period-matching behaviour.
///
/// NovaTechRetro bug:
///   In a Retro-Payrun the left PeriodValue carries an open-ended Period
///   (End = Date.MaxValue), while the right PeriodValue carries a trimmed
///   CalendarPeriod (End = last hour of the month).
///   The original containment check only tested whether the RIGHT period
///   contains the LEFT period — the inverse case (open LEFT contains trimmed
///   RIGHT) was not handled, so FindMatchingPeriodValue returned null,
///   the operator loop silently skipped every period, and the Payrun
///   failed with "Missing results".
/// </summary>
public class CasePayrollValueTests
{
    // ── Shared period fixtures ──────────────────────────────────────────────

    /// <summary>Standard CalendarPeriod: January 2024, trimmed end (last full hour)</summary>
    private static readonly DateTime Jan2024Start = new(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    private static readonly DateTime Jan2024End   = new(2024, 1, 31, 23, 0, 0, DateTimeKind.Utc);

    // ── 1. Exact match ──────────────────────────────────────────────────────

    [Fact(DisplayName = "Operator + exact period match returns correct sum")]
    public void Add_ExactPeriodMatch_ReturnsSum()
    {
        // Both sides carry identical CalendarPeriods → exact Equals() match.
        var left  = MakeCaseValue("Salary", Jan2024Start, Jan2024End, 3000M);
        var right = MakeCaseValue("Bonus",  Jan2024Start, Jan2024End, 500M);

        decimal result = left + right;

        Assert.Equal(3500M, result);
    }

    // ── 2. Normal containment (right contains left) ─────────────────────────

    [Fact(DisplayName = "Operator + right-contains-left: open right covers trimmed left")]
    public void Add_RightContainsLeft_ReturnsSum()
    {
        // left  = trimmed CalendarPeriod  (End = Jan2024End)
        // right = open-ended Period        (End = Date.MaxValue)
        // The existing containment branch handles this case.
        var left  = MakeCaseValue("Salary", Jan2024Start, Jan2024End,   3000M);
        var right = MakeCaseValue("Salary", Jan2024Start, null,          500M); // open-ended

        decimal result = left + right;

        Assert.Equal(3500M, result);
    }

    // ── 3. NovaTechRetro bug – left contains right ──────────────────────────

    [Fact(DisplayName = "NovaTechRetro bug: Operator + open left vs. trimmed right returns correct sum")]
    public void Add_OpenLeftVsTrimmedRight_RetroScenario_ReturnsSum()
    {
        // This is the exact scenario that caused the NovaTechRetro failure.
        //
        // In a Retro-Payrun, the current-period case value is delivered with an
        // open-ended Period (End = Date.MaxValue), while the previously calculated
        // value still carries the trimmed CalendarPeriod end.
        //
        // left  = open-ended Period  (End = Date.MaxValue)   ← Retro-Payrun result
        // right = CalendarPeriod     (End = Jan2024End)      ← prior calculation
        //
        // Without the fix: FindMatchingPeriodValue returns null → operator skips
        //   the period → result stays Empty → PayrollException: Missing results.
        // With the fix: left-contains-right branch matches → result = 3000 + 100 = 3100.

        var left  = MakeCaseValue("Salary", Jan2024Start, null,        3000M); // open-ended
        var right = MakeCaseValue("Salary", Jan2024Start, Jan2024End,   100M); // CalendarPeriod

        decimal result = left + right;

        Assert.Equal(3100M, result);
    }

    [Fact(DisplayName = "NovaTechRetro bug: Operator - open left vs. trimmed right returns correct difference")]
    public void Subtract_OpenLeftVsTrimmedRight_RetroScenario_ReturnsDifference()
    {
        // Subtraction variant: retro diff = current salary – previously calculated salary.
        var left  = MakeCaseValue("Salary", Jan2024Start, null,        3000M);
        var right = MakeCaseValue("Salary", Jan2024Start, Jan2024End,  2800M);

        decimal result = left - right;

        Assert.Equal(200M, result);
    }

    [Fact(DisplayName = "NovaTechRetro bug: Operator * open left vs. trimmed right returns correct product")]
    public void Multiply_OpenLeftVsTrimmedRight_RetroScenario_ReturnsProduct()
    {
        var left  = MakeCaseValue("Rate",  Jan2024Start, null,       3000M);
        var right = MakeCaseValue("Factor", Jan2024Start, Jan2024End,  0.1M);

        decimal result = left * right;

        Assert.Equal(300M, result);
    }

    [Fact(DisplayName = "NovaTechRetro bug: Operator / open left vs. trimmed right returns correct quotient")]
    public void Divide_OpenLeftVsTrimmedRight_RetroScenario_ReturnsQuotient()
    {
        var left  = MakeCaseValue("Salary", Jan2024Start, null,       3000M);
        var right = MakeCaseValue("Divisor", Jan2024Start, Jan2024End,    3M);

        decimal result = left / right;

        Assert.Equal(1000M, result);
    }

    // ── 4. No-match → empty result ──────────────────────────────────────────

    [Fact(DisplayName = "Operator + non-overlapping periods returns empty")]
    public void Add_NonOverlappingPeriods_ReturnsEmpty()
    {
        // Completely different periods → no match expected from any branch.
        var feb2024Start = new DateTime(2024, 2, 1, 0, 0, 0, DateTimeKind.Utc);
        var feb2024End   = new DateTime(2024, 2, 29, 23, 0, 0, DateTimeKind.Utc);

        var left  = MakeCaseValue("Salary", Jan2024Start, Jan2024End, 3000M);
        var right = MakeCaseValue("Salary", feb2024Start, feb2024End, 500M);

        var result = left + right;

        Assert.False(result.HasValue);
    }

    // ── Helper ──────────────────────────────────────────────────────────────

    /// <summary>
    /// Creates a CasePayrollValue with a single PeriodValue.
    /// When <paramref name="end"/> is null the period is open-ended (End = Date.MaxValue).
    /// </summary>
    private static CasePayrollValue MakeCaseValue(
        string caseFieldName, DateTime start, DateTime? end, decimal value)
    {
        var periodValue = new PeriodValue(start, end, value);
        return new CasePayrollValue(caseFieldName, [periodValue]);
    }
}
