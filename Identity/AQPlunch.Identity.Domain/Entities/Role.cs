using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AQPlunch.Identity.Domain.Entities;

public partial class Role
{
    public Guid RoleId { get; set; }

    public string RoleName { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
