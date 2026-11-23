using System;
using System.Collections.Generic;

namespace AQPlunch.Sales.Domain.Entities;

public partial class Sale
{
    public Guid SaleId { get; set; }

    public Guid OrderId { get; set; }

    public Guid RestaurantId { get; set; }

    public Guid PaymentMethodId { get; set; }

    public Guid RegisteredBy { get; set; }

    public decimal Total { get; set; }

    public DateTime? SoldAt { get; set; }

    public virtual PaymentMethod PaymentMethod { get; set; } = null!;

    public virtual ICollection<SaleDetail> SaleDetails { get; set; } = new List<SaleDetail>();
}
