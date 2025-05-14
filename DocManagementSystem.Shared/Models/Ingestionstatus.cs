using System;
using System.Collections.Generic;

namespace DocManagementSystem.Shared.Models;

public partial class Ingestionstatus
{
    public int Id { get; set; }

    public DateTime TriggeredAt { get; set; }

    public string Status { get; set; } = null!;

    public string? ErrorMessage { get; set; }
}
