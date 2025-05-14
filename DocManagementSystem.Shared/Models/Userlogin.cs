using System;
using System.Collections.Generic;

namespace DocManagementSystem.Shared.Models;

public partial class Userlogin
{
    public string LoginProvider { get; set; } = null!;

    public string ProviderKey { get; set; } = null!;

    public string? ProviderDisplayName { get; set; }

    public string UserId { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
