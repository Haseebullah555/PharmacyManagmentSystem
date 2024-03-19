using Microsoft.AspNetCore.Mvc.Rendering;
using PharmacyManagmentSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace PharmacyManagmentSystem.ViewModel
{
    public class SaleViewModel
    {
        public int SaleID { get; set; }
        [Required(ErrorMessage = "Amount Couldn't be Empty")]
        public decimal SaleAmount { get; set; }
        [Required(ErrorMessage = "Unite Price Couldn't be Empty")]
        public decimal UnitPrice { get; set; }
        public decimal SubTotalPrice { get; set; }
        public decimal TotalPrice { get; set; }
        [Required(ErrorMessage = "Paid Amount Couldn't be Empty")]
        public decimal Paid { get; set; }
        [Required(ErrorMessage = "Unpaid Amount Couldn't be Empty")]
        public decimal Unpaid { get; set; }
        [Required(ErrorMessage = "Sale Date Couldn't be Empty")]
        [DataType(DataType.DateTime)]
        public DateTime SaleDate { get; set; }
        //Navigation Property
        public List<SelectListItem> SelectedMedicines { get; set; }
        public Purchase purchase { get; set; }
        public int MedicineID { get; set; }
        public int CurrencyID { get; set; }
        public int CustomerID { get; set; }
    }
}
