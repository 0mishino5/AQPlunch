using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AQPlunch.RestaurantService.Domain.Entities;

public partial class Restaurant
{
    public Guid RestaurantId { get; set; }

    public string Name { get; set; } = null!;

    public string? Address { get; set; }

    public string? Phone { get; set; }

    public bool IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    [JsonIgnore]
    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();

    [JsonIgnore]
    public virtual ICollection<DiningTable> DiningTables { get; set; } = new List<DiningTable>();
}
