using Application.Dtos.Dosage;
using Application.Dtos.Common;
using MediatR;

namespace Application.Features.Dosage.Requests.Queries
{
    public class GetListOfAllDosagesWithParamRequest : IRequest<PaginatedResult<DosageDto>>
    {
        public string Search { get; set; }
        public string SortBy { get; set; }
        public string SortDirection { get; set; }
        public int Page { get; set; } = 1;
        public int PerPage { get; set; } = 10;
    }
}