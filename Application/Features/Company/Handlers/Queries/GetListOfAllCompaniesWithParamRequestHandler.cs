using Application.Contracts.Interfaces.Common;
using Application.Dtos.Company;
using Application.Dtos.Common;
using Application.Features.Company.Requests.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Company.Handlers.Queries
{
    public class GetListOfAllCompaniesWithParamRequestHandler : IRequestHandler<GetListOfAllCompaniesWithParamRequest, PaginatedResult<CompanyDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetListOfAllCompaniesWithParamRequestHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<PaginatedResult<CompanyDto>> Handle(GetListOfAllCompaniesWithParamRequest request, CancellationToken cancellationToken)
        {
             var query = _unitOfWork.Companies.Query().AsNoTracking();

            // Search
            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                query = query.Where(s => s.CompanyName.ToString().Contains(request.Search));
            }

            // Sorting
            if (!string.IsNullOrWhiteSpace(request.SortBy))
            {
                if (request.SortBy.Equals("name", StringComparison.OrdinalIgnoreCase))
                {
                    query = request.SortDirection == "desc"
                        ? query.OrderByDescending(s => s.CompanyName)
                        : query.OrderBy(s => s.CompanyName);
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
            var Companies = await query
                .Skip((request.Page - 1) * request.PerPage)
                .Take(request.PerPage)
                .Select(e => new CompanyDto
                {
                    Id = e.Id,
                    CompanyName = e.CompanyName,
                }).ToListAsync(cancellationToken);


            return new PaginatedResult<CompanyDto>
            {
                Data = Companies,
                Total = total,
                CurrentPage = request.Page,
                PerPage = request.PerPage
            };
        }
    }
}