using System;
using System.Collections.Generic;

namespace AQPlunch.Sales.Domain.Entities;

public partial class PaymentMethod
{
    public Guid PaymentMethodId { get; set; }

    public string Name { get; set; } = null!;

    public bool IsActive { get; set; }

    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();
}
