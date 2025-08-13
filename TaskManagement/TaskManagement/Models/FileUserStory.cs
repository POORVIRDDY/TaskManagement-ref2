using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Models
{
    public class FileUserStory
    {
        public Guid? UserStoryId { get; set; }

        public String? ProjectId { get; set; }

        public String? OwnerId { get; set; }

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
        public IFormFile File { get; set; }
        public List<Notes> Notes { get; set; }
    }
}
