namespace AQPlunch.Sales.Application.Clients;

public class OrderInfo
{
    public Guid OrderId { get; set; }
    public Guid RestaurantId { get; set; }
    public int TableNumber { get; set; }
    public string Status { get; set; } = null!;
    public decimal Total { get; set; }
    public List<OrderDetailInfo> OrderDetails { get; set; } = new List<OrderDetailInfo>();
}
