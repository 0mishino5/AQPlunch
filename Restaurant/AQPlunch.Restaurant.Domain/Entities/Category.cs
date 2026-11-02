using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AQPlunch.RestaurantService.Domain.Entities;

public partial class Category
{
    public Guid CategoryId { get; set; }

    public Guid RestaurantId { get; set; }

    public string Name { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    [JsonIgnore]
    public virtual Restaurant Restaurant { get; set; } = null!;
}
