using Application.Dtos.Expense;
using MediatR;

namespace Application.Features.Expense.Requests.Queries
{
    public class GetListOfExpenseRequest: IRequest<List<ExpenseDto>>
    {
    }
}