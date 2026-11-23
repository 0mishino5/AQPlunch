namespace AQPlunch.OrderService.Application.Clients;

public class TableInfo
{
    public Guid TableId { get; set; }
    public int TableNumber { get; set; }
    public Guid RestaurantId { get; set; }
    public string RestaurantName { get; set; } = null!;
}
