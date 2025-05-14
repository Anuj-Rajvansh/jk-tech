using System;
using System.Collections.Generic;

namespace DocManagementSystem.Shared.Models;

public partial class Document
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Type { get; set; } = null!;

    public byte[] Content { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public DateTime ModifiedOn { get; set; }
}
