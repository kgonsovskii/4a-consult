using FluentAssertions;

namespace Chapter4.Topic2.Tests;

public sealed class HangingWireTests
{
    private readonly HangingWire _wire = new();

    [Fact]
    public void Distance_between_poles_is_zero()
    {
        var finalDistance = _wire.DistanceBetweenPoles(184.2m, 100m, 7.9m);

        finalDistance.Should().Be(0m);
    }

    [Fact]
    public void Height_50m_is_not_the_trick_case()
    {
        var finalDistance = _wire.DistanceBetweenPoles(184.2m, 50m, 7.9m);

        finalDistance.Should().BeApproximately(163.829m, 0.001m);
    }
}
