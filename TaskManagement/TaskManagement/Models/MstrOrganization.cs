using System;
using System.Collections.Generic;

namespace TaskManagement.Models;

public partial class MstrOrganization
{
    public Guid OrganizationId { get; set; }

    public string? Name { get; set; }

    public string? Address { get; set; }

    public string? Contact { get; set; }

    public string? Email { get; set; }

    public string? LogoUrl { get; set; }
}
