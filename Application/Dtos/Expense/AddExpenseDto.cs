using Application.Dtos.Common;

namespace Application.Dtos.Expense
{
    public class AddExpenseDto : CreateBaseDto
    {
        public string ExpenseName { get; set; }
        public DateOnly Date { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
    }
}
