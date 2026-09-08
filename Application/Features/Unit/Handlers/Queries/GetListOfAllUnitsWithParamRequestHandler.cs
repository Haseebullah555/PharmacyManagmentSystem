using Application.Contracts.Interfaces.Common;
using Application.Dtos.Unit;
using Application.Dtos.Common;
using Application.Features.Unit.Requests.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Unit.Handlers.Queries
{
    public class GetListOfAllUnitsWithParamRequestHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetListOfAllUnitsWithParamRequest, PaginatedResult<UnitDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<PaginatedResult<UnitDto>> Handle(GetListOfAllUnitsWithParamRequest request, CancellationToken cancellationToken)
        {
             var query = _unitOfWork.Units.Query().AsNoTracking();

            // Search
            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                query = query.Where(s => s.Name.ToString().Contains(request.Search));
            }

            // Sorting
            if (!string.IsNullOrWhiteSpace(request.SortBy))
            {
                if (request.SortBy.Equals("name", StringComparison.OrdinalIgnoreCase))
                {
                    query = request.SortDirection == "desc"
                        ? query.OrderByDescending(s => s.Name)
                        : query.OrderBy(s => s.Name);
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
            var Units = await query
                .Skip((request.Page - 1) * request.PerPage)
                .Take(request.PerPage)
                .Select(e => new UnitDto
                {
                    Id = e.Id,
                    Name = e.Name,
                    ShortName = e.ShortName,
                }).ToListAsync(cancellationToken);


            return new PaginatedResult<UnitDto>
            {
                Data = Units,
                Total = total,
                CurrentPage = request.Page,
                PerPage = request.PerPage
            };
        }
    }
}