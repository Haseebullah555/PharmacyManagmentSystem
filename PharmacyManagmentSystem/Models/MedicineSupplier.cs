namespace PharmacyManagmentSystem.Models
{
    public class MedicineSupplier
    {
        public int MedicineID { get; set; }
        public Medicine Medicine { get; set; }

        public int SupplierID { get; set; }
        public Supplier Supplier { get; set; }

    }
}
