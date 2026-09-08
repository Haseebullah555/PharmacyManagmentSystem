using Application.Dtos.Common;
using MediatR;

namespace Application.Features.Currency.Requests.Queries
{
    public class GetCurrenciesListRequest : IRequest<List<DropDownDto>>
    {
        
    }
}