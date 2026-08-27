using System.ComponentModel.DataAnnotations;
using Domain.Common;

namespace Domain.Models
{
    public class Customer : BaseDomainEntity
    {
        [Required]
        public string CustomerName { get; set; }
        [Required]
        public string PhoneNo { get; set; }
        public  string Address { get; set; }
     
    }
}
