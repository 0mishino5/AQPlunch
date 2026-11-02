using System;
using System.Collections.Generic;

namespace AQPlunch.RestaurantService.Domain.Entities;

public partial class Category
{
    public Guid CategoryId { get; set; }

    public Guid RestaurantId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    public virtual Restaurant Restaurant { get; set; } = null!;
}
