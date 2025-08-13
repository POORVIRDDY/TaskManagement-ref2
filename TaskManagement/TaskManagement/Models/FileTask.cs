using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Models
{
    public class FileTask
    {
        public Guid TaskId { get; set; }

        public string? TaskNo { get; set; }

        public Guid? AssignedTo { get; set; }

        public Guid? ProjectId { get; set; }

        public string? Description { get; set; }

        public string? Status { get; set; }

        public Guid? UserStoryId { get; set; }

        public Guid? ParentTaskId { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? StartDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? EndDate { get; set; }

        public decimal? TotalEstimatedHours { get; set; }

        public decimal? TotalHoursSpent { get; set; }

        public decimal? TotalRemainingHours { get; set; }

        public bool? IsNotesAttached { get; set; }

        public virtual MstrUser? AssignedToNavigation { get; set; }

        public virtual ICollection<MstrTask> InverseParentTask { get; set; } = new List<MstrTask>();

        public virtual MstrTask? ParentTask { get; set; }

        public virtual MstrProject? Project { get; set; }

        public virtual MstrUserStory? UserStory { get; set; }
        public IFormFile File { get; set; }
        public List<Notes> Notes { get; set; }
    }
}
