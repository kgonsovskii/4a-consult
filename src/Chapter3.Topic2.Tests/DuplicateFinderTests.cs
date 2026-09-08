using FluentAssertions;

namespace Chapter3.Topic2.Tests;

public sealed class DuplicateFinderTests
{
    private readonly DuplicateFinder _finder = new();

    [Fact]
    public void Detects_duplicate()
    {
        _finder.HasDuplicates([1, 2, 1]).Should().BeTrue();
    }

    [Fact]
    public void Unique_has_no_duplicates()
    {
        _finder.HasDuplicates([1, 2, 3]).Should().BeFalse();
    }
}
