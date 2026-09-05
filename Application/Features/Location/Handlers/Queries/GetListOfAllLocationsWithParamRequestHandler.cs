using Application.Contracts.Interfaces.Common;
using Application.Dtos.Location;
using Application.Dtos.Common;
using Application.Features.Location.Requests.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Location.Handlers.Queries
{
    public class GetListOfAllLocationsWithParamRequestHandler : IRequestHandler<GetListOfAllLocationsWithParamRequest, PaginatedResult<LocationDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetListOfAllLocationsWithParamRequestHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<PaginatedResult<LocationDto>> Handle(GetListOfAllLocationsWithParamRequest request, CancellationToken cancellationToken)
        {
            var query = _unitOfWork.Locations.Query().AsNoTracking();

            // Search
            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                query = query.Where(s => s.LocationName.ToString().Contains(request.Search));
            }

            // Sorting
            if (!string.IsNullOrWhiteSpace(request.SortBy))
            {
                if (request.SortBy.Equals("name", StringComparison.OrdinalIgnoreCase))
                {
                    query = request.SortDirection == "desc"
                        ? query.OrderByDescending(s => s.LocationName)
                        : query.OrderBy(s => s.LocationName);
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
            var Locations = await query
                .Skip((request.Page - 1) * request.PerPage)
                .Take(request.PerPage)
                .Select(e => new LocationDto
                {
                    Id = e.Id,
                    LocationName = e.LocationName,
                    ParentLocationID = e.ParentLocationID,
                    Code = e.Code,
                }).ToListAsync(cancellationToken);


            return new PaginatedResult<LocationDto>
            {
                Data = Locations,
                Total = total,
                CurrentPage = request.Page,
                PerPage = request.PerPage
            };
        }
    }
}