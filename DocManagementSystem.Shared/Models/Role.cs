using System;
using System.Collections.Generic;

namespace DocManagementSystem.Shared.Models;

public partial class Role
{
    public string Id { get; set; } = null!;

    public string? Name { get; set; }

    public string? NormalizedName { get; set; }

    public string? ConcurrencyStamp { get; set; }

    public virtual ICollection<Roleclaim> Roleclaims { get; set; } = new List<Roleclaim>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
