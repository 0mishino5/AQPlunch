using System;
using System.Collections.Generic;

namespace AQPlunch.OrderService.Domain.Entities;

public partial class Order
{
    public Guid OrderId { get; set; }

    public Guid RestaurantId { get; set; }

    public Guid TableId { get; set; }

    public int TableNumber { get; set; }

    public string Status { get; set; } = null!;

    public decimal Total { get; set; }

    public string? Note { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
