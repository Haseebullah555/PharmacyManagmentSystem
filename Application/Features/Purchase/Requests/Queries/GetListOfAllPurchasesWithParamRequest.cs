using Application.Dtos.Common;
using Application.Dtos.Purchase;
using MediatR;

namespace Application.Features.Purchase.Requests.Queries
{
    public class GetListOfAllPurchasesWithParamRequest : IRequest<PaginatedResult<PurchaseDto>>
    {
        public string Search { get; set; }
        public string SortBy { get; set; }
        public string SortDirection { get; set; }
        public int Page { get; set; } = 1;
        public int PerPage { get; set; } = 10;
    }
}
