using System.ComponentModel.DataAnnotations;
using Domain.Common;

namespace Domain.Models
{
    public class Sale : BaseDomainEntity
    {
        [Required]
        public DateOnly SaleDate { get; set; }

        public int? CustomerID { get; set; }
        public Customer? Customer { get; set; }

        [Required]
        public int CurrencyID { get; set; }
        public Currency Currency { get; set; }

        [Required]
        public decimal TotalAmount { get; set; }

        [Required]
        public decimal PaidAmount { get; set; }

        [Required]
        public decimal UnpaidAmount { get; set; }

        public decimal Discount { get; set; }

        public string? InvoiceNumber { get; set; }

        public string? Remarks { get; set; }

        public ICollection<SaleItem> Items { get; set; }
            = new List<SaleItem>();
    }
}
