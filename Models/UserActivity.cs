using System.ComponentModel;

namespace HelpDeskSystem.Models
{
    public class UserActivity
    {
        public string CreatedById { get; set; }
        [DisplayName("Created By ")]
        public ApplicationUser CreatedBy { get; set; }
        [DisplayName("Created On ")]
        public DateTime CreatedOn { get; set; }
        [DisplayName("Modified By ")]
        public string? ModifiedById { get; set; }
        public ApplicationUser ModifiedBy { get; set; }
        [DisplayName("Modified On ")]
        public DateTime? ModifiedOn { get; set; }
    }
}
