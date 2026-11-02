using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AQPlunch.RestaurantService.Domain.Entities;

public partial class Product
{
    public Guid ProductId { get; set; }

    public Guid CategoryId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public bool IsAvailable { get; set; }

    public DateTime? CreatedAt { get; set; }

    [JsonIgnore]
    public virtual Category Category { get; set; } = null!;
}
