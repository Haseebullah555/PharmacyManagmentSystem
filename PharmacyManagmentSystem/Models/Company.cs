using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PharmacyManagmentSystem.Models
{
    public class Company
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CompanyID { get; set; }
        [Required(ErrorMessage = "Company Name Couldn't be Empty")]
        public string CompanyName { get; set;}
    }
}
