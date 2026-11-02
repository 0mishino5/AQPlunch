namespace AQPlunch.RestaurantService.Application.Results;

public class MenuProductResult
{
    public Guid ProductId { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public decimal Price { get; set; }
}
