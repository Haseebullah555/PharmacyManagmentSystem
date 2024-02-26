using System.ComponentModel.DataAnnotations;

namespace PharmacyManagmentSystem.Models
{
    public class Currency
    {
        [Key]
        public int CurrencyID { get; set; }
        public string CurrencyName { get; set; }
    }
}
