using Application.Contracts.Interfaces.Common;
using Application.Dtos.Expense;
using Application.Features.Expense.Requests.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Expense.Handlers.Queries
{
    public class GetListOfExpenseRequestHandler(IUnitOfWork _unitOfWork): IRequestHandler<GetListOfExpenseRequest, List<ExpenseDto>>
    {
       public async Task<List<ExpenseDto>> Handle(GetListOfExpenseRequest request, CancellationToken cancellationToken)
        {
            var aparments = await _unitOfWork.Expenses
            .Query()
            .AsNoTracking()
            .Select(m => new ExpenseDto
            {
                Id = m.Id,
                ExpenseName = m.ExpenseName,
            }).ToListAsync();
            return aparments;
        }
    }
}