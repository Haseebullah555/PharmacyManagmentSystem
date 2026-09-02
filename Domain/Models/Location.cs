using System.ComponentModel.DataAnnotations;
using Domain.Common;

namespace Domain.Models
{
    public class Location : BaseDomainEntity
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; }

    [MaxLength(50)]
    public string? Code { get; set; }

    public int? ParentLocationID { get; set; }
    public Location? ParentLocation { get; set; }

    public bool IsActive { get; set; } = true;

    // Navigation
    public ICollection<Location> ChildLocations { get; set; }
        = new List<Location>();

    public ICollection<InventoryBatch> InventoryBatches { get; set; }
        = new List<InventoryBatch>();
}
}