using FluentAssertions;

namespace Chapter8.Topic1.Tests;

public sealed class TaxSystemsTests
{
    private readonly TaxSystems _taxes = new();

    [Fact]
    public void Systems_are_equal_at_sixty_percent_expenses()
    {
        var share = _taxes.ExpenseShareWhenEqual();
        const decimal income = 1000m;
        var expenses = income * share;

        share.Should().Be(0.6m);
        (income * TaxSystems.IncomeRate).Should().Be((income - expenses) * TaxSystems.ProfitRate);
    }
}
