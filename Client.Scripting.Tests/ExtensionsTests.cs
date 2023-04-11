using System;
using Xunit;

namespace PayrollEngine.Client.Scripting.Tests;

public class ExtensionsTests
{
    [Fact]
    public void RoundTenthTest()
    {
        Assert.Equal(10.1m, 10.149999999999999m.RoundTenth());
        Assert.Equal(10.2m, 10.15m.RoundTenth());
    }

    [Fact]
    public void RoundRoundTwentiethTest()
    {
        Assert.Equal(10, 10.024m.RoundTwentieth());
        Assert.Equal(10.05m, 10.025m.RoundTwentieth());
    }

    [Fact]
    public void TruncateStepTest()
    {
        Assert.Equal(80, 99.0m.Truncate(20));
        Assert.Equal(3000, 3499.99999999m.Truncate(500));
        Assert.Equal(3500, 3500m.Truncate(500));
        Assert.Equal(3500, 3875m.Truncate(500));
    }

    [Fact]
    public void RoundUpTest()
    {
        Assert.Equal(106, 105.5m.RoundUp(1));
        Assert.Equal(110, 105.5m.RoundUp(10));
        Assert.Equal(112, 105.5m.RoundUp(7));
        Assert.Equal(200, 105.5m.RoundUp(100));
        Assert.Equal(105.6m, 105.5m.RoundUp(0.2m));
        Assert.Equal(105.6m, 105.5m.RoundUp(0.3m));
    }

    [Fact]
    public void RoundDownTest()
    {
        Assert.Equal(105, 105.5m.RoundDown(1));
        Assert.Equal(100, 105.5m.RoundDown(10));
        Assert.Equal(105, 105.5m.RoundDown(7));
        Assert.Equal(100, 105.5m.RoundDown(100));
        Assert.Equal(105.4m, 105.5m.RoundDown(0.2m));
        Assert.Equal(105.3m, 105.5m.RoundDown(0.3m));
    }

    [Fact]
    public void AgeTest()
    {
        Assert.Equal(50, new DateTime(1969, 6, 18).Age(new(2020, 1, 1)));
        Assert.Equal(50, new DateTime(1969, 6, 18).Age(new(2020, 6, 17)));
        Assert.Equal(51, new DateTime(1969, 6, 18).Age(new(2020, 6, 18)));
        Assert.Equal(51, new DateTime(1969, 6, 18).Age(new(2020, 6, 19)));
    }
}