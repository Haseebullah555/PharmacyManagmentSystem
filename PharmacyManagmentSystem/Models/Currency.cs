using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PharmacyManagmentSystem.Models
{
    public class Currency
    {
        public int CurrencyID { get; set; }
        [Required(ErrorMessage = "Currency Name Couldn't be Empty")]
        public string CurrencyName { get; set; }
        // Navigation Property

    }
}
