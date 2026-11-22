namespace AQPlunch.Sales.Domain.Reports;

public class DailySales
{
    public DateTime Date { get; set; }
    public int SalesCount { get; set; }
    public decimal Total { get; set; }
}
