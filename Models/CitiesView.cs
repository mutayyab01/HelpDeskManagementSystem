using System.ComponentModel;

namespace HelpDeskSystem.Models
{
    public class CitiesView
    {
        public int Id { get; set; }
        [DisplayName("City Name")]
        public string CityName { get; set; }
        [DisplayName("Postal Code")]
        public string PostalCode { get; set; }
        public int CountryId { get; set; }
        [DisplayName("Country Name")]
        public string CountryName { get; set; }
        [DisplayName("Created By")]
        public string CreatedBy { get; set; }
        [DisplayName("Created On")]
        public DateTime CreatedOn { get; set; }
        [DisplayName("Modified By")]
        public string ModifiedBy { get; set; }
        [DisplayName("Modified On")]
        public DateTime? ModifiedOn { get; set; }
   
    }
}
