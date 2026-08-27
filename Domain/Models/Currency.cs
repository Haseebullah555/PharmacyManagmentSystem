using System.ComponentModel.DataAnnotations;
using Domain.Common;

namespace Domain.Models
{
    public class Currency : BaseDomainEntity
    {
        [Required]
        public string CurrencyName { get; set; }
        // Navigation Property

    }
}
