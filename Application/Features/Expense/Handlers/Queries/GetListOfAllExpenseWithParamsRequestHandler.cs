using Application.Contracts.Interfaces.Common;
using Application.Dtos.Common;
using Application.Dtos.Expense;
using Application.Features.Expense.Requests.Queries;
using Application.Helper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Management.Expense.Handlers.Queries
{
    public class GetListOfAllExpenseWithParamsRequestHandler(IUnitOfWork _unitOfWork): IRequestHandler<GetListOfAllExpenseWithParamsRequest, PaginatedResult<ExpenseDto>>
    {
          public async Task<PaginatedResult<ExpenseDto>> Handle(GetListOfAllExpenseWithParamsRequest request, CancellationToken cancellationToken)
        {
            var query = _unitOfWork.Expenses.Query().AsNoTracking();

            // // Search
            // if (!string.IsNullOrWhiteSpace(request.Search))
            // {
            //     query = query.Where(s => s.Brand.ToString().Contains(request.Search));
            // }

            // Sorting
            if (!string.IsNullOrWhiteSpace(request.SortBy))
            {
                if (request.SortBy.Equals("name", StringComparison.OrdinalIgnoreCase))
                {
                    query = request.SortDirection == "desc"
                        ? query.OrderByDescending(s => s.Id)
                        : query.OrderBy(s => s.Id);
                }
                else if (request.SortBy.Equals("id", StringComparison.OrdinalIgnoreCase))
                {
                    query = request.SortDirection == "desc"
                        ? query.OrderByDescending(s => s.Id)
                        : query.OrderBy(s => s.Id);
                }
            }
            else
            {
                // Default sort (optional)
                query = query.OrderBy(s => s.Id);
            }

            // Total count (before pagination)
            var total = await query.CountAsync(cancellationToken);

            // Pagination
            var buildingDtos = await query
                .Skip((request.Page - 1) * request.PerPage)
                .Take(request.PerPage)
                .Select(m => new ExpenseDto
                {
                    Id = m.Id,
                    ExpenseName = m.ExpenseName,
                    Amount = m.Amount,
                    Date = m.Date,
                    Description = m.Description,
                }).ToListAsync(cancellationToken);


            return new PaginatedResult<ExpenseDto>
            {
                Data = buildingDtos,
                Total = total,
                CurrentPage = request.Page,
                PerPage = request.PerPage
            };
        }
    }
}

