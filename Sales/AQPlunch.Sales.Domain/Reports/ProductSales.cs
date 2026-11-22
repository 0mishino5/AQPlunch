namespace AQPlunch.Sales.Domain.Reports;

public class ProductSales
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = null!;
    public int QuantitySold { get; set; }
    public decimal Total { get; set; }
}
