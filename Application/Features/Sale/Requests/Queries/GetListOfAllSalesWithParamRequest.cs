using Application.Dtos.Common;
using Application.Dtos.Sale;
using MediatR;

namespace Application.Features.Sale.Requests.Queries
{
    public class GetListOfAllSalesWithParamRequest : IRequest<PaginatedResult<SaleDto>>
    {
        public string Search { get; set; }
        public string SortBy { get; set; }
        public string SortDirection { get; set; }
        public int Page { get; set; } = 1;
        public int PerPage { get; set; } = 10;
    }
}
