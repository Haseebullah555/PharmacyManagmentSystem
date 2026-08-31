using Application.Dtos.Common;
using Application.Dtos.Expense;
using MediatR;

namespace Application.Features.Expense.Requests.Queries
{
    public class GetListOfAllExpenseWithParamsRequest : IRequest<PaginatedResult<ExpenseDto>>
    {
        public string SortBy { get; set; }
        public string SortDirection { get; set; }
        public int Page { get; set; } = 1;
        public int PerPage { get; set; } = 10;
    }
}