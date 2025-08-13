using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Models;

public partial class MstrProject
{
    public Guid ProjectId { get; set; }

    public int? SerialNumber { get; set; }

    public string? ProjectTitle { get; set; }

    public string? Description { get; set; }
    
    //[Required(ErrorMessage = "Date is required")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:d}")]
    public DateTime? StartDate { get; set; }

    //[Required(ErrorMessage = "Date is required")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:d}")]
    public DateTime? EndDate { get; set; }

    public Guid? ManagerId { get; set; }

    public String? Status { get; set; }

    public Guid? CustomerId { get; set; }

    public string? Type { get; set; }

    //[Required(ErrorMessage = "Date is required")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:d}")]
    public DateTime? LastModifiedOn { get; set; }

    public Guid? LastModifiedBy { get; set; }

    public virtual MstrCustomer? Customer { get; set; }

    public virtual MstrUser? LastModifiedByNavigation { get; set; }

    public virtual MstrUser? Manager { get; set; }

    public virtual ICollection<MstrTask> MstrTasks { get; set; } = new List<MstrTask>();
    public virtual ICollection<MstrTimeSheet> MstrTimeSheets { get; set; } = new List<MstrTimeSheet>();
    public virtual ICollection<MstrUserStory> MstrUserStories { get; set; } = new List<MstrUserStory>();
}
