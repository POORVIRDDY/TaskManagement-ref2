using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagement.Models;

public partial class MstrTimeSheet
{
    public Guid TimeSheetId { get; set; }

    public Guid? UserId { get; set; }

    public Guid? TaskId { get; set; }

    public Guid? ProjectId { get; set; }

    public DateTime? Date { get; set; }

    public bool? Status { get; set; }

    public decimal? Hours { get; set; }

    public Guid? ApprovedBy { get; set; }

    public DateTime? AddedOn { get; set; }
   
    public virtual MstrUser? ApprovedByNavigation { get; set; }

    public virtual MstrProject? Project { get; set; }

    public virtual MstrTask? Task { get; set; }

    public virtual MstrUser? User { get; set; }
}
