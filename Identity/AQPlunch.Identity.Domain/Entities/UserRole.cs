using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AQPlunch.Identity.Domain.Entities;

public partial class UserRole
{
    public Guid UserId { get; set; }

    public Guid RoleId { get; set; }

    public DateTime? AssignedAt { get; set; }

    public virtual Role Role { get; set; } = null!;

    [JsonIgnore]
    public virtual User User { get; set; } = null!;
}
