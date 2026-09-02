using System.ComponentModel.DataAnnotations;
using Domain.Common;

namespace Domain.Models
{
    public class Supplier : BaseDomainEntity
    {
        [Required]
        public string SupplierName { get; set; }
        public string Address { get; set; }
        [Required]
        public string Phone { get; set; }
        public string Description { get; set; }
   
    }
}
