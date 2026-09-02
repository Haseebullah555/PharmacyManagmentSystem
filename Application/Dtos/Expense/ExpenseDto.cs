namespace Application.Dtos.Expense
{
    public class ExpenseDto
    {
        public int Id { get; set; }
        public string ExpenseName { get; set; }
        public DateOnly Date { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
    }
}
