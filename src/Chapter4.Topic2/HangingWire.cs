namespace Chapter4.Topic2;

public sealed class HangingWire
{
    public decimal DistanceBetweenPoles(decimal length, decimal height, decimal clearance)
    {
        var drop = height - clearance;
        return length / 2m == drop ? 0m : throw new ArgumentException("Провод длиннее провиса.");
    }
}
