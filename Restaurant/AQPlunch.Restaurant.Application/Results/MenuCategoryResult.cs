namespace AQPlunch.RestaurantService.Application.Results;

public class MenuCategoryResult
{
    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; } = null!;
    public List<MenuProductResult> Products { get; set; } = new List<MenuProductResult>();
}
