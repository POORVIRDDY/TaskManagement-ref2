using System;
using System.Collections.Generic;

namespace TaskManagement.Models;

public partial class GnrlNote
{
    public Guid NotesId { get; set; }

    public Guid? ObjectId { get; set; }

    public string? FileName { get; set; }

    public byte[]? Data { get; set; }

    public DateTime? LastModifiedOn { get; set; }

    public Guid? LastModifiedBy { get; set; }

    public virtual MstrUser? LastModifiedByNavigation { get; set; }
}
