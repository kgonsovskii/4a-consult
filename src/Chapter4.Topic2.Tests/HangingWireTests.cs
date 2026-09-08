using Chapter4.Topic2;
using FluentAssertions;

namespace Chapter4.Topic2.Tests;

public sealed class HangingWireTests
{
    [Fact]
    public void Poles_stand_together_when_half_wire_equals_drop()
    {
        const decimal length = 184.2m;
        const decimal height = 100m;
        const decimal clearance = 7.9m;

        (length / 2m).Should().Be(height - clearance);
        new HangingWire().DistanceBetweenPoles(length, height, clearance).Should().Be(0m);
    }
}
