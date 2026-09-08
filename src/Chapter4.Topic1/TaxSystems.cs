namespace Chapter4.Topic1;

public sealed class TaxSystems
{
    public const decimal IncomeRate = 0.06m;
    public const decimal ProfitRate = 0.15m;

    public decimal ExpenseShareWhenEqual() =>
        1m - IncomeRate / ProfitRate;
}
