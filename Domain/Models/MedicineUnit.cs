using System.ComponentModel.DataAnnotations;
using Domain.Common;

namespace Domain.Models
{
    public class MedicineUnit : BaseDomainEntity
{
    [Required]
    public int MedicineID { get; set; }
    public Medicine Medicine { get; set; }

    [Required]
    public int UnitID { get; set; }
    public Unit Unit { get; set; }

    [Required]
    public decimal ConversionFactor { get; set; }

    public bool IsBaseUnit { get; set; }

    public bool CanPurchase { get; set; }

    public bool CanSell { get; set; }
}
}