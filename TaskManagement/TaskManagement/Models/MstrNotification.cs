using System;
using System.Collections.Generic;

namespace TaskManagement.Models;

public partial class MstrNotification
{
    public Guid NotificationId { get; set; }

    public string? Subject { get; set; }

    public DateTime? NotificationDate { get; set; }

    public Guid? UserId { get; set; }

    public virtual MstrUser? User { get; set; }
}
