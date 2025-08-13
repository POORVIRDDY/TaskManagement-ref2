using System;
using System.Collections.Generic;

namespace TaskManagement.Models;

public partial class MstrRole
{
    public Guid RoleId { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<MstrUser> MstrUsers { get; set; } = new List<MstrUser>();
}
