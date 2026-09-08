using Chapter4.Topic2;
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
    public void Height_200m_is_not_the_trick_case()
    {
        var act = () => _wire.DistanceBetweenPoles(184.2m, 200m, 7.9m);

        act.Should().Throw<ArgumentException>();
    }
}
