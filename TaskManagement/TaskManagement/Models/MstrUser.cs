using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Models;

public partial class MstrUser
{
    public Guid UserId { get; set; }

    public string? Name { get; set; }

    [Required(ErrorMessage = "Date is required")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:d}")]
    public DateTime? DateOfBirth { get; set; }

    public string? Nationality { get; set; }

    public Guid? RoleId { get; set; }

    public string? PhoneNo { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public bool IsUserActivated { get; set; }

    public virtual ICollection<GnrlNote> GnrlNotes { get; set; } = new List<GnrlNote>();

    public virtual ICollection<MstrNotification> MstrNotifications { get; set; } = new List<MstrNotification>();

    public virtual ICollection<MstrProject> MstrProjectLastModifiedByNavigations { get; set; } = new List<MstrProject>();

    public virtual ICollection<MstrProject> MstrProjectManagers { get; set; } = new List<MstrProject>();

    public virtual ICollection<MstrTask> MstrTasks { get; set; } = new List<MstrTask>();
    public virtual ICollection<MstrTimeSheet> MstrTimeSheetApprovedByNavigations { get; set; } = new List<MstrTimeSheet>();
    public virtual ICollection<MstrTimeSheet> MstrTimeSheetUsers { get; set; } = new List<MstrTimeSheet>();

    public virtual ICollection<MstrUserStory> MstrUserStoryLastModifiedByNavigations { get; set; } = new List<MstrUserStory>();

    public virtual ICollection<MstrUserStory> MstrUserStoryOwners { get; set; } = new List<MstrUserStory>();

    public virtual MstrRole? Role { get; set; }
    
}
