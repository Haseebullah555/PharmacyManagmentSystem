using Application.Dtos.Currency;
using Application.Dtos.Common;
using MediatR;

namespace Application.Features.Currency.Requests.Queries
{
    public class GetListOfAllCurrenciesWithParamRequest : IRequest<PaginatedResult<CurrencyDto>>
    {
        public string Search { get; set; }
        public string SortBy { get; set; }
        public string SortDirection { get; set; }
        public int Page { get; set; } = 1;
        public int PerPage { get; set; } = 10;
    }
}