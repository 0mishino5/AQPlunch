namespace AQPlunch.RestaurantService.Application.Results;

public class ProductPriceResult
{
    public Guid ProductId { get; set; }
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public bool IsAvailable { get; set; }
    public Guid RestaurantId { get; set; }
}
