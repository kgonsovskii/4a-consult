using Chapter4.Topic2;
using FluentAssertions;

namespace Chapter4.Topic2.Tests;

public sealed class HangingWireTests
{
    [Fact]
    public void Distance_between_poles_is_zero()
    {
        var finalDistance = new HangingWire().DistanceBetweenPoles(184.2m, 100m, 7.9m);

        finalDistance.Should().Be(0m);
    }
}
