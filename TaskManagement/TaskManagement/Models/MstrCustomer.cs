using System;
using System.Collections.Generic;

namespace TaskManagement.Models;

public partial class MstrCustomer
{
    public Guid CustomerId { get; set; }

    public string? CustomerName { get; set; }

    public string? Address { get; set; }

    public string? Country { get; set; }

    public bool? Status { get; set; }

    public virtual ICollection<MstrProject> MstrProjects { get; set; } = new List<MstrProject>();
}
