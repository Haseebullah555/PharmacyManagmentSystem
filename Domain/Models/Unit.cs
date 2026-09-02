using System.ComponentModel.DataAnnotations;
using Domain.Common;

namespace Domain.Models
{
   public class Unit : BaseDomainEntity
{
    [Required]
    [MaxLength(50)]
    public string Name { get; set; }

    [Required]
    [MaxLength(20)]
    public string ShortName { get; set; }

    public bool IsActive { get; set; } = true;

    // Navigation
    public ICollection<MedicineUnit> MedicineUnits { get; set; }
        = new List<MedicineUnit>();
}
}