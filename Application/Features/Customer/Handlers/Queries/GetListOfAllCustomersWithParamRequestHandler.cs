using Application.Contracts.Interfaces.Common;
using Application.Dtos.Customer;
using Application.Dtos.Common;
using Application.Features.Customer.Requests.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Customer.Handlers.Queries
{
    public class GetListOfAllCustomersWithParamRequestHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetListOfAllCustomersWithParamRequest, PaginatedResult<CustomerDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<PaginatedResult<CustomerDto>> Handle(GetListOfAllCustomersWithParamRequest request, CancellationToken cancellationToken)
        {
             var query = _unitOfWork.Customers.Query().AsNoTracking();

            // Search
            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                query = query.Where(s => s.CustomerName.ToString().Contains(request.Search));
            }

            // Sorting
            if (!string.IsNullOrWhiteSpace(request.SortBy))
            {
                if (request.SortBy.Equals("name", StringComparison.OrdinalIgnoreCase))
                {
                    query = request.SortDirection == "desc"
                        ? query.OrderByDescending(s => s.CustomerName)
                        : query.OrderBy(s => s.CustomerName);
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
            var Customers = await query
                .Skip((request.Page - 1) * request.PerPage)
                .Take(request.PerPage)
                .Select(e => new CustomerDto
                {
                    Id = e.Id,
                    CustomerName = e.CustomerName,
                    Address = e.Address,
                    Phone = e.Phone,
                }).ToListAsync(cancellationToken);


            return new PaginatedResult<CustomerDto>
            {
                Data = Customers,
                Total = total,
                CurrentPage = request.Page,
                PerPage = request.PerPage
            };
        }
    }
}