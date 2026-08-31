using Application.Dtos.Expense;
using Application.Features.Response;
using MediatR;

namespace Application.Features.Expense.Requests.Commands
{
    public class UpdateExpenseCommand: IRequest<BaseCommandResponse>
    {
        public UpdateExpenseDto UpdateExpenseDto { get; set; }
    }
}