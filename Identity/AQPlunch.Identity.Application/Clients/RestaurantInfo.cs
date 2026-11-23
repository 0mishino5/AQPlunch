namespace AQPlunch.Identity.Application.Clients;

public class RestaurantInfo
{
    public Guid RestaurantId { get; set; }
    public string Name { get; set; } = null!;
    public bool IsActive { get; set; }
}
