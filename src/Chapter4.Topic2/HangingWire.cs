namespace Chapter4.Topic2;

public sealed class HangingWire
{
    public decimal DistanceBetweenPoles(decimal length, decimal height, decimal clearance)
    {
        var drop = height - clearance;
        if (length / 2m != drop)
        {
            throw new ArgumentException("Половина провода не равна провису.");
        }

        return 0m;
    }
}
