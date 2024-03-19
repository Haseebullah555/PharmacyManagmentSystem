using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace PharmacyManagmentSystem.ViewModel
{
    public class MedicineViewModel
    {
        public int MedicineID { get; set; }
        [Required(ErrorMessage = "Generic Name Couldn't be Empty")]
        public string GenericName { get; set; }

        [Required(ErrorMessage = "Trade Name Couldn't be Empty")]
        public string TradeName { get; set; }

        [Required(ErrorMessage = "Capacity Couldn't be Empty")]
        public string Capacity { get; set; }

        [Required(ErrorMessage = "Category is required")]
        public int CategoryID { get; set; }

        [Required(ErrorMessage = "Company is required")]
        public int CompanyID { get; set; }
    }
}
