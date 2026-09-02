using Application.Dtos.Supplier;
using Application.Dtos.Common;
using MediatR;

namespace Application.Features.Supplier.Requests.Queries
{
    public class GetListOfAllSuppliersWithParamRequest : IRequest<PaginatedResult<SupplierDto>>
    {
        public string Search { get; set; }
        public string SortBy { get; set; }
        public string SortDirection { get; set; }
        public int Page { get; set; } = 1;
        public int PerPage { get; set; } = 10;
    }
}