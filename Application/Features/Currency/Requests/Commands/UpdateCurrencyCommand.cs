using Application.Dtos.Currency;
using MediatR;

namespace Application.Features.Currency.Requests.Commands
{
    public class UpdateCurrencyCommand : IRequest
    {
        public UpdateCurrencyDto UpdateCurrencyDto { get; set; }
    }
}