using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AQPlunch.Sales.Domain.Entities;

public partial class SaleDetail
{
    public Guid SaleDetailId { get; set; }

    public Guid SaleId { get; set; }

    public Guid ProductId { get; set; }

    public string ProductName { get; set; } = null!;

    public decimal UnitPrice { get; set; }

    public int Quantity { get; set; }

    public decimal Subtotal { get; set; }

    [JsonIgnore]
    public virtual Sale Sale { get; set; } = null!;
}
