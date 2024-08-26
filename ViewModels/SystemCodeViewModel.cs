using System.ComponentModel;

namespace HelpDeskSystem.ViewModels
{
    public class SystemCodeViewModel
    {
        [DisplayName("No")]
        public int Id { get; set; }
        [DisplayName("System Code")]
        public string Code { get; set; }
        [DisplayName("Description")]
        public string Description { get; set; }
    }
}
