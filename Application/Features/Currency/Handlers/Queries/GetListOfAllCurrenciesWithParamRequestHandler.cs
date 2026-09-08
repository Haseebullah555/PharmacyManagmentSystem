using Application.Contracts.Interfaces.Common;
using Application.Dtos.Currency;
using Application.Dtos.Common;
using Application.Features.Currency.Requests.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Currency.Handlers.Queries
{
    public class GetListOfAllCurrenciesWithParamRequestHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetListOfAllCurrenciesWithParamRequest, PaginatedResult<CurrencyDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<PaginatedResult<CurrencyDto>> Handle(GetListOfAllCurrenciesWithParamRequest request, CancellationToken cancellationToken)
        {
             var query = _unitOfWork.Currencies.Query().AsNoTracking();

            // Search
            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                query = query.Where(s => s.CurrencyName.ToString().Contains(request.Search));
            }

            // Sorting
            if (!string.IsNullOrWhiteSpace(request.SortBy))
            {
                if (request.SortBy.Equals("name", StringComparison.OrdinalIgnoreCase))
                {
                    query = request.SortDirection == "desc"
                        ? query.OrderByDescending(s => s.CurrencyName)
                        : query.OrderBy(s => s.CurrencyName);
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
            var Currencys = await query
                .Skip((request.Page - 1) * request.PerPage)
                .Take(request.PerPage)
                .Select(e => new CurrencyDto
                {
                    Id = e.Id,
                    CurrencyName = e.CurrencyName,
                }).ToListAsync(cancellationToken);


            return new PaginatedResult<CurrencyDto>
            {
                Data = Currencys,
                Total = total,
                CurrentPage = request.Page,
                PerPage = request.PerPage
            };
        }
    }
}