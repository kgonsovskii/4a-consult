namespace Chapter3.Topic2;

public sealed class DuplicateFinder
{
    public bool HasDuplicates(int[] m)
    {
        var seen = new HashSet<int>();
        foreach (var value in m)
        {
            if (!seen.Add(value))
            {
                return true;
            }
        }

        return false;
    }
}
