namespace AQPlunch.Sales.Domain.Reports;

public class PaymentMethodSales
{
    public Guid PaymentMethodId { get; set; }
    public string PaymentMethodName { get; set; } = null!;
    public int SalesCount { get; set; }
    public decimal Total { get; set; }
}
