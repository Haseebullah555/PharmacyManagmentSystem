using Application.Dtos.Expense;
using Application.Features.Response;
using MediatR;

namespace Application.Features.Expense.Requests.Commands
{
    public class AddExpenseCommand: IRequest<BaseCommandResponse>
    {
        public AddExpenseDto AddExpenseDto { get; set; }
    }
}