using Application.Dtos.Common;

namespace Application.Dtos.Expense
{
    public class UpdateExpenseDto : UpdateBaseDto
    {
        public string ExpenseName { get; set; }
        public string Date { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
    }
}
