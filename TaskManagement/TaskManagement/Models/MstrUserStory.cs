using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Models;

public partial class MstrUserStory
{
    public Guid UserStoryId { get; set; }

    public Guid? ProjectId { get; set; }

    public Guid? OwnerId { get; set; }

    public int? Points { get; set; }

    public string? Description { get; set; }

   
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:d}")]
    public DateTime? LastModifiedOn { get; set; }

    public Guid? LastModifiedBy { get; set; }

    public bool? IsNotesAttached { get; set; }

    public virtual MstrUser? LastModifiedByNavigation { get; set; }

    public virtual ICollection<MstrTask> MstrTasks { get; set; } = new List<MstrTask>();

    public virtual MstrUser? Owner { get; set; }

    public virtual MstrProject? Project { get; set; }
}
