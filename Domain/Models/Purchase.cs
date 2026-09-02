using System.ComponentModel.DataAnnotations;
using Domain.Common;

namespace Domain.Models
{
   public class Purchase : BaseDomainEntity
{
    [Required]
    public DateOnly PurchaseDate { get; set; }

    [MaxLength(100)]
    public string? InvoiceNumber { get; set; }

    [Required]
    public int SupplierID { get; set; }
    public Supplier Supplier { get; set; }

    [Required]
    public int CurrencyID { get; set; }
    public Currency Currency { get; set; }

    [Required]
    public decimal TotalAmount { get; set; }

    [Required]
    public decimal PaidAmount { get; set; }

    [Required]
    public decimal UnpaidAmount { get; set; }

    [MaxLength(500)]
    public string? Remarks { get; set; }

    // Navigation
    public ICollection<PurchaseItem> PurchaseItems { get; set; }
        = new List<PurchaseItem>();
}
}
