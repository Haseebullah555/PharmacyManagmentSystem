using Application.Contracts.Interfaces.Common;
using Application.Dtos.Supplier;
using Application.Dtos.Common;
using Application.Features.Supplier.Requests.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Supplier.Handlers.Queries
{
    public class GetListOfAllSuppliersWithParamRequestHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetListOfAllSuppliersWithParamRequest, PaginatedResult<SupplierDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<PaginatedResult<SupplierDto>> Handle(GetListOfAllSuppliersWithParamRequest request, CancellationToken cancellationToken)
        {
             var query = _unitOfWork.Suppliers.Query().AsNoTracking();

            // Search
            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                query = query.Where(s => s.SupplierName.ToString().Contains(request.Search));
            }

            // Sorting
            if (!string.IsNullOrWhiteSpace(request.SortBy))
            {
                if (request.SortBy.Equals("name", StringComparison.OrdinalIgnoreCase))
                {
                    query = request.SortDirection == "desc"
                        ? query.OrderByDescending(s => s.SupplierName)
                        : query.OrderBy(s => s.SupplierName);
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
            var Suppliers = await query
                .Skip((request.Page - 1) * request.PerPage)
                .Take(request.PerPage)
                .Select(e => new SupplierDto
                {
                    Id = e.Id,
                    SupplierName = e.SupplierName,
                    Address = e.Address,
                    Phone = e.Phone,
                    Description = e.Description
                }).ToListAsync(cancellationToken);


            return new PaginatedResult<SupplierDto>
            {
                Data = Suppliers,
                Total = total,
                CurrentPage = request.Page,
                PerPage = request.PerPage
            };
        }
    }
}