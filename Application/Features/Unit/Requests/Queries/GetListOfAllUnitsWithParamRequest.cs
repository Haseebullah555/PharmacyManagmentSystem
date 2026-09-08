using Application.Dtos.Unit;
using Application.Dtos.Common;
using MediatR;

namespace Application.Features.Unit.Requests.Queries
{
    public class GetListOfAllUnitsWithParamRequest : IRequest<PaginatedResult<UnitDto>>
    {
        public string Search { get; set; }
        public string SortBy { get; set; }
        public string SortDirection { get; set; }
        public int Page { get; set; } = 1;
        public int PerPage { get; set; } = 10;
    }
}