using System.ComponentModel.DataAnnotations;
using Domain.Common;

namespace Domain.Models
{
    public class Dosage : BaseDomainEntity
    {
        [Required]
        public string DosageName { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        
    }
}