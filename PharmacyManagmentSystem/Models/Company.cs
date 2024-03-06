namespace PharmacyManagmentSystem.Models
{
    public class Company
    {
        public int CompanyID { get; set; }
        public string CompanyName { get; set; }
        public ICollection<Medicine >Medicines { get; set; }
    }
}
