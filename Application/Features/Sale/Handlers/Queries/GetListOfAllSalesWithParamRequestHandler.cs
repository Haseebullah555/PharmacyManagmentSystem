using Application.Contracts.Interfaces.Common;
using Application.Dtos.Common;
using Application.Dtos.Sale;
using Application.Features.Sale.Requests.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Sale.Handlers.Queries
{
    public class GetListOfAllSalesWithParamRequestHandler(IUnitOfWork _unitOfWork) : IRequestHandler<GetListOfAllSalesWithParamRequest, PaginatedResult<SaleDto>>
    {
        public async Task<PaginatedResult<SaleDto>> Handle(GetListOfAllSalesWithParamRequest request, CancellationToken cancellationToken)
        {
            var query = _unitOfWork.Sales.Query().AsNoTracking();

            // Search
            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                query = query.Where(s => s.Medicine.GenericName.Contains(request.Search));
            }

            // Sorting
            if (!string.IsNullOrWhiteSpace(request.SortBy))
            {
                if (request.SortBy.Equals("name", StringComparison.OrdinalIgnoreCase))
                {
                    query = request.SortDirection == "desc"
                        ? query.OrderByDescending(s => s.Medicine.GenericName)
                        : query.OrderBy(s => s.Medicine.GenericName);
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
            var medicines = await query
                .Skip((request.Page - 1) * request.PerPage)
                .Take(request.PerPage)
                .Select(e => new SaleDto
                {
                    Id = e.Id,
                    SaleAmount = e.SaleAmount,
                    SaleDate = e.SaleDate,
                    TotalPrice = e.TotalPrice,
                    Paid = e.Paid,
                    Unpaid = e.Unpaid,
                    MedicineID = e.MedicineID,
                    Medicine = e.Medicine.GenericName,
                    CurrencyID = e.CurrencyID,
                    Currency = e.Currency.CurrencyName,
                    CustomerID = e.CustomerID,
                    Customer = e.Customer.CustomerName,
                }).ToListAsync(cancellationToken);

            return new PaginatedResult<SaleDto>
            {
                Data = medicines,
                Total = total,
                CurrentPage = request.Page,
                PerPage = request.PerPage
            };
        }

    }
}