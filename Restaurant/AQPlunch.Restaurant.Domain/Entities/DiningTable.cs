using System;
using System.Collections.Generic;

namespace AQPlunch.RestaurantService.Domain.Entities;

public partial class DiningTable
{
    public Guid TableId { get; set; }

    public Guid RestaurantId { get; set; }

    public int TableNumber { get; set; }

    public Guid QrToken { get; set; }

    public bool IsActive { get; set; }

    public virtual Restaurant Restaurant { get; set; } = null!;
}
