using Application.Dtos.Common;
using Application.Dtos.Company;
using MediatR;

namespace Application.Features.Company.Requests.Queries
{
    public class GetListOfAllCompaniesWithParamRequest : IRequest<PaginatedResult<CompanyDto>>
    {
        public string Search { get; set; }
        public string SortBy { get; set; }
        public string SortDirection { get; set; }
        public int Page { get; set; } = 1;
        public int PerPage { get; set; } = 10;
    }
}
