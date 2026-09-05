using Application.Contracts.Interfaces.Common;
using Application.Dtos.Common;
using Application.Dtos.Purchase;
using Application.Features.Purchase.Requests.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Purchase.Handlers.Queries
{
    public class GetListOfAllPurchasesWithParamRequestHandler(IUnitOfWork _unitOfWork) : IRequestHandler<GetListOfAllPurchasesWithParamRequest, PaginatedResult<PurchaseDto>>
    {
        public async Task<PaginatedResult<PurchaseDto>> Handle(GetListOfAllPurchasesWithParamRequest request, CancellationToken cancellationToken)
        {
            var query = _unitOfWork.Purchases.Query().AsNoTracking();

            // Search
            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                query = query.Where(s => s.InvoiceNumber.ToString().Contains(request.Search));
            }

            // Sorting
            if (!string.IsNullOrWhiteSpace(request.SortBy))
            {
                if (request.SortBy.Equals("name", StringComparison.OrdinalIgnoreCase))
                {
                    query = request.SortDirection == "desc"
                        ? query.OrderByDescending(s => s.InvoiceNumber)
                        : query.OrderBy(s => s.InvoiceNumber);
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
                .Select(e => new PurchaseDto
                {
                    Id = e.Id,
                    InvoiceNumber = e.InvoiceNumber,
                    PurchaseDate = e.PurchaseDate,
                    TotalAmount = e.TotalAmount,
                    PaidAmount = e.PaidAmount,
                    UnpaidAmount = e.UnpaidAmount,
                    Remarks = e.Remarks,
                    SupplierID = e.SupplierID,
                    Supplier = e.Supplier.SupplierName,
                    CurrencyID = e.CurrencyID,
                    Currency = e.Currency.CurrencyName,
                }).ToListAsync(cancellationToken);

            return new PaginatedResult<PurchaseDto>
            {
                Data = medicines,
                Total = total,
                CurrentPage = request.Page,
                PerPage = request.PerPage
            };
        }
    }
}