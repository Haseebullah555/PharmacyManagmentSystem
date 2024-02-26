using System.ComponentModel.DataAnnotations;

namespace PharmacyManagmentSystem.Models
{
    public class Catagory
    {
        [Key]
        public int CatagoryID { get; set; }
        public string CatagoryName { get; set; }
    }
}
