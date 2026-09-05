using Application.Dtos.Location;
using Application.Dtos.Common;
using MediatR;

namespace Application.Features.Location.Requests.Queries
{
    public class GetListOfAllLocationsWithParamRequest : IRequest<PaginatedResult<LocationDto>>
    {
        public string Search { get; set; }
        public string SortBy { get; set; }
        public string SortDirection { get; set; }
        public int Page { get; set; } = 1;
        public int PerPage { get; set; } = 10;
    }
}