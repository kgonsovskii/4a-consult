namespace Chapter4.Topic2;

public sealed class HangingWire
{
    public decimal DistanceBetweenPoles(
        decimal length,
        decimal height,
        decimal clearance)
    {
        var halfLength = length / 2m;
        var vertical = height - clearance;

        var horizontalSquared =
            halfLength * halfLength - vertical * vertical;

        if (horizontalSquared < 0)
        {
            throw new ArgumentException(
                "The cable is too short for the given height and clearance.");
        }

        return 2m * (decimal)Math.Sqrt((double)horizontalSquared);
    }
}
