using Domain.Common;

namespace Domain.Models
{
    public class Expense : BaseDomainEntity
    {
      public string ExpenseName { get; set; }  
      public DateOnly Date { get; set; }
      public decimal Amount { get; set; }
      public string Description { get; set; }
    }
}