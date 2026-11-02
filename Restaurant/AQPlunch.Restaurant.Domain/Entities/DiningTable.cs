using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AQPlunch.RestaurantService.Domain.Entities;

public partial class DiningTable
{
    public Guid TableId { get; set; }

    public Guid RestaurantId { get; set; }

    public int TableNumber { get; set; }

    public Guid QrToken { get; set; }

    public bool IsActive { get; set; }

    [JsonIgnore]
    public virtual Restaurant Restaurant { get; set; } = null!;
}
