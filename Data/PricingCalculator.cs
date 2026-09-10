namespace AnimeStore.Data;

public static class PricingCalculator
{
    public static decimal CalculateShipping(Order order)
    {
        return 10.00m; // $10.00 flat for now rather than UPS API lookup
    }
    public static decimal CalculateTax(decimal subtotal)
    {
        return subtotal*0.05m; // 5% for now rather than building lookup
    }
}