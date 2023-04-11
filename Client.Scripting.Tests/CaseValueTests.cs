using System;
using Xunit;

namespace PayrollEngine.Client.Scripting.Tests;

public class CaseValueTests
{
    [Fact]
    public void ConvertStringTest()
    {
        var caseValue = new PayrollValue("foo");

        string result = caseValue;
        Assert.Equal("foo", result);
    }

    [Fact]
    public void ConvertIntTest()
    {
        var caseValue = new PayrollValue(22);

        int result = caseValue;
        Assert.Equal(22, result);
    }

    [Fact]
    public void ConvertDecimalTest()
    {
        var caseValue = new PayrollValue(200M);

        decimal result = caseValue;
        Assert.Equal(200M, result);
    }

    [Fact]
    public void ConvertDateTimeTest()
    {
        var caseValue = new PayrollValue(new DateTime(2020, 1, 1, 14, 25, 0));

        DateTime result = caseValue;
        Assert.Equal(new(2020, 1, 1, 14, 25, 0), result);
    }

    [Fact]
    public void ConvertBoolTest()
    {
        var caseValue = new PayrollValue(true);

        bool result = caseValue;
        Assert.True(result);
    }

    [Fact]
    public void ConditionBoolTest()
    {
        var caseValue = new PayrollValue(true);
        var result = false;
        // ReSharper disable once ConvertIfToOrExpression
        if (caseValue)
        {
            result = true;
        }
        Assert.True(result);
    }

    #region Unary operators

    [Fact]
    public void CaseValueOperatorBinaryPlusTest()
    {
        Assert.Equal(0, +new PayrollValue(0));
        Assert.Equal(20, +new PayrollValue(20));
        Assert.Equal(-20, +new PayrollValue(-20));
    }

    [Fact]
    public void CaseValueOperatorBinaryMinusTest()
    {
        Assert.Equal(0, -new PayrollValue(0));
        Assert.Equal(-20, -new PayrollValue(20));
        Assert.Equal(20, -new PayrollValue(-20));
    }

    [Fact]
    public void CaseValueOperatorLogicalNegationTest()
    {
        Assert.True(!new PayrollValue(false));
        Assert.False(!new PayrollValue(true));
    }

    #endregion

    #region Binary operators

    [Fact]
    public void CaseValueOperatorPlusTest()
    {
        Assert.Equal(30, new PayrollValue(10) + new PayrollValue(20));
        Assert.Equal(31M, new PayrollValue(10.5M) + new PayrollValue(20.5M));
        Assert.Equal(20.5M, new PayrollValue(10.5M) + new PayrollValue(10));
        Assert.Equal(20.5M, new PayrollValue(10) + new PayrollValue(10.5M));
        Assert.Equal("Hello world", new PayrollValue("Hello ") + new PayrollValue("world"));
    }

    [Fact]
    public void CaseValueOperatorMinusTest()
    {
        Assert.Equal(20, new PayrollValue(30) - new PayrollValue(10));
        Assert.Equal(20M, new PayrollValue(30.5M) - new PayrollValue(10.5M));
        Assert.Equal(20.5M, new PayrollValue(30.5M) - new PayrollValue(10));
        Assert.Equal(19.5M, new PayrollValue(30) - new PayrollValue(10.5M));
    }

    [Fact]
    public void CaseValueOperatorMultiplicationTest()
    {
        Assert.Equal(60, new PayrollValue(20) * new PayrollValue(3));
        Assert.Equal(16.5M, new PayrollValue(5.5M) * new PayrollValue(3M));
        Assert.Equal(16.5M, new PayrollValue(5.5M) * new PayrollValue(3));
        Assert.Equal(16.5M, new PayrollValue(3) * new PayrollValue(5.5M));
    }

    [Fact]
    public void CaseValueOperatorDivisionTest()
    {
        Assert.Equal(5, new PayrollValue(20) / new PayrollValue(4));
        Assert.Equal(5M, new PayrollValue(20M) / new PayrollValue(4M));
        Assert.Equal(5M, new PayrollValue(20M) / new PayrollValue(4));
    }

    [Fact]
    public void CaseValueOperatorRemainderDecimalTest()
    {
        Assert.Equal(1, new PayrollValue(5) % new PayrollValue(4));
        Assert.Equal(-1.2M, new PayrollValue(-5.2M) % new PayrollValue(2M));
        Assert.Equal(-1.2M, new PayrollValue(-5.2M) % new PayrollValue(2));
    }

    [Fact]
    public void CaseValueOperatorAndTest()
    {
        Assert.True(new PayrollValue(true) & new PayrollValue(true));
        Assert.False(new PayrollValue(true) & new PayrollValue(false));
        Assert.False(new PayrollValue(false) & new PayrollValue(true));

        // includes test for casting operators true and false
        Assert.True(new PayrollValue(true) && new PayrollValue(true));
        Assert.False(new PayrollValue(true) && new PayrollValue(false));
        Assert.False(new PayrollValue(false) && new PayrollValue(true));
    }

    [Fact]
    public void CaseValueOperatorOrTest()
    {
        Assert.True(new PayrollValue(true) | new PayrollValue(true));
        Assert.True(new PayrollValue(true) | new PayrollValue(false));
        Assert.True(new PayrollValue(false) | new PayrollValue(true));
        Assert.False(new PayrollValue(false) | new PayrollValue(false));

        // includes test for casting operators true and false
        Assert.True(new PayrollValue(true) || new PayrollValue(true));
        Assert.True(new PayrollValue(true) || new PayrollValue(false));
        Assert.True(new PayrollValue(false) || new PayrollValue(true));
        Assert.False(new PayrollValue(false) || new PayrollValue(false));
    }

    [Fact]
    public void CaseValueOperatorLessThanTest()
    {
        // int < int
        Assert.True(new PayrollValue(10) < new PayrollValue(20));
        Assert.False(new PayrollValue(20) < new PayrollValue(10));
        // decimal < decimal
        Assert.True(new PayrollValue(10M) < new PayrollValue(20M));
        Assert.False(new PayrollValue(20M) < new PayrollValue(10M));
        // decimal < int
        Assert.True(new PayrollValue(9.5M) < new PayrollValue(10));
        Assert.False(new PayrollValue(10.5M) < new PayrollValue(10));
        // int < decimal
        Assert.True(new PayrollValue(10) < new PayrollValue(10.5M));
        Assert.False(new PayrollValue(10) < new PayrollValue(9.5M));
    }

    [Fact]
    public void CaseValueOperatorGreaterThanTest()
    {
        // int > int
        Assert.True(new PayrollValue(20) > new PayrollValue(10));
        Assert.False(new PayrollValue(10) > new PayrollValue(20));
        // decimal > decimal
        Assert.True(new PayrollValue(20M) > new PayrollValue(10M));
        Assert.False(new PayrollValue(10M) > new PayrollValue(20M));
        // decimal > int
        Assert.True(new PayrollValue(10.5M) > new PayrollValue(10));
        Assert.False(new PayrollValue(9.5M) > new PayrollValue(10));
        // int > decimal
        Assert.True(new PayrollValue(10) > new PayrollValue(9.5M));
        Assert.False(new PayrollValue(10) > new PayrollValue(10.5M));
    }

    [Fact]
    public void CaseValueOperatorLessOrEqualsThanTest()
    {
        // int <= int
        Assert.True(new PayrollValue(10) <= new PayrollValue(20));
        // ReSharper disable once EqualExpressionComparison
        Assert.True(new PayrollValue(10) <= new PayrollValue(10));
        Assert.False(new PayrollValue(20) <= new PayrollValue(10));

        // int <= decimal
        Assert.True(new PayrollValue(10) <= new PayrollValue(10.5M));
        // ReSharper disable once EqualExpressionComparison
        Assert.True(new PayrollValue(10) <= new PayrollValue(10));
        Assert.False(new PayrollValue(10) <= new PayrollValue(9.5M));

        // decimal <= decimal
        Assert.True(new PayrollValue(10M) <= new PayrollValue(20M));
        // ReSharper disable once EqualExpressionComparison
        Assert.True(new PayrollValue(10M) <= new PayrollValue(10M));
        Assert.False(new PayrollValue(20M) <= new PayrollValue(10M));

        // decimal <= int
        Assert.True(new PayrollValue(10M) <= new PayrollValue(11));
        // ReSharper disable once EqualExpressionComparison
        Assert.True(new PayrollValue(10M) <= new PayrollValue(10));
        Assert.False(new PayrollValue(10M) <= new PayrollValue(9));
    }

    [Fact]
    public void CaseValueOperatorGreaterOrEqualsThanTest()
    {
        // int >= int
        Assert.True(new PayrollValue(20) >= new PayrollValue(10));
        // ReSharper disable once EqualExpressionComparison
        Assert.True(new PayrollValue(10) >= new PayrollValue(10));
        Assert.False(new PayrollValue(10) >= new PayrollValue(20));

        // int >= decimal
        Assert.True(new PayrollValue(10) >= new PayrollValue(9.5M));
        // ReSharper disable once EqualExpressionComparison
        Assert.True(new PayrollValue(10) >= new PayrollValue(10.0M));
        Assert.False(new PayrollValue(10) >= new PayrollValue(10.5M));

        // decimal >= decimal
        Assert.True(new PayrollValue(20M) >= new PayrollValue(10M));
        // ReSharper disable once EqualExpressionComparison
        Assert.True(new PayrollValue(10M) >= new PayrollValue(10M));
        Assert.False(new PayrollValue(10M) >= new PayrollValue(20M));

        // decimal >= int
        Assert.True(new PayrollValue(10) >= new PayrollValue(9.5M));
        // ReSharper disable once EqualExpressionComparison
        Assert.True(new PayrollValue(10) >= new PayrollValue(10.0M));
        Assert.False(new PayrollValue(10) >= new PayrollValue(10.5M));
    }

    #endregion

    #region Compound assignment operators
    // ReSharper disable RedundantAssignment

    [Fact]
    public void CaseValueOperatorCompoundPlusTest()
    {
        // int += int
        var intValue = new PayrollValue(10);
        Assert.Equal(30, intValue += new PayrollValue(20));

        // int += decimal
        intValue = new(10);
        Assert.Equal(30.5M, intValue += new PayrollValue(20.5M));

        // decimal += decimal
        var decimalValue = new PayrollValue(10M);
        Assert.Equal(30M, decimalValue += new PayrollValue(20M));

        // decimal += int
        decimalValue = new(10.5M);
        Assert.Equal(30.5M, decimalValue += new PayrollValue(20));
    }

    [Fact]
    public void CaseValueOperatorCompoundMinusTest()
    {
        // int -= int
        var intValue = new PayrollValue(30);
        Assert.Equal(20, intValue -= new PayrollValue(10));

        // int -= decimal (invalid)
        intValue = new(30);
        Assert.Equal(20M, intValue -= new PayrollValue(10M));

        // decimal -= decimal
        var decimalValue = new PayrollValue(30M);
        Assert.Equal(20M, decimalValue -= new PayrollValue(10M));

        // decimal -= int
        decimalValue = new(30M);
        Assert.Equal(20M, decimalValue -= new PayrollValue(10));
    }

    [Fact]
    public void CaseValueOperatorCompoundMultiplicationTest()
    {
        // int *= int
        var intValue = new PayrollValue(30);
        Assert.Equal(60, intValue *= new PayrollValue(2));

        // int *= decimal
        intValue = new(30);
        Assert.Equal(60M, intValue *= new PayrollValue(2M));

        // decimal *= decimal
        var decimalValue = new PayrollValue(30M);
        Assert.Equal(60M, decimalValue *= new PayrollValue(2M));

        // decimal *= int
        decimalValue = new(30M);
        Assert.Equal(60M, decimalValue *= new PayrollValue(2));
    }

    [Fact]
    public void CaseValueOperatorCompoundDivisionTest()
    {
        // int /= int
        var intValue = new PayrollValue(60);
        Assert.Equal(30, intValue /= new PayrollValue(2));

        // int /= decimal
        intValue = new(60);
        Assert.Equal(30M, intValue /= new PayrollValue(2M));

        // decimal /= decimal
        var decimalValue = new PayrollValue(60M);
        Assert.Equal(30M, decimalValue /= new PayrollValue(2M));

        // decimal /= int
        decimalValue = new(60M);
        Assert.Equal(30M, decimalValue /= new PayrollValue(2));
    }

    [Fact]
    public void CaseValueOperatorCompoundRemainderTest()
    {
        // int %= int
        var intValue = new PayrollValue(5);
        Assert.Equal(1, intValue %= new PayrollValue(4));

        // int %= decimal
        intValue = new(5);
        Assert.Equal(1M, intValue %= new PayrollValue(4M));

        // decimal %= decimal
        var decimalValue = new PayrollValue(-5.2M);
        Assert.Equal(-1.2M, decimalValue %= new PayrollValue(2M));

        // decimal %= int
        decimalValue = new(-5.2M);
        Assert.Equal(-1.2M, decimalValue %= new PayrollValue(2));
    }

    [Fact]
    public void CaseValueOperatorCompoundAndTest()
    {
        var boolValue = new PayrollValue(true);
        Assert.True(boolValue &= new PayrollValue(true));
        boolValue = new(true);
        Assert.False(boolValue &= new PayrollValue(false));

        boolValue = new(false);
        Assert.False(boolValue &= new PayrollValue(true));
        boolValue = new(false);
        Assert.False(boolValue &= new PayrollValue(false));
    }

    [Fact]
    public void CaseValueOperatorCompoundOrTest()
    {
        var boolValue = new PayrollValue(true);
        Assert.True(boolValue |= new PayrollValue(true));
        boolValue = new(true);
        Assert.True(boolValue |= new PayrollValue(false));

        boolValue = new(false);
        Assert.True(boolValue |= new PayrollValue(true));
        boolValue = new(false);
        Assert.False(boolValue |= new PayrollValue(false));
    }

    // ReSharper restore RedundantAssignment
    #endregion
}