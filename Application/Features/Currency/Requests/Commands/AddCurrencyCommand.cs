using Application.Dtos.Currency;
using MediatR;

namespace Application.Features.Currency.Requests.Commands
{
    public class AddCurrencyCommand : IRequest
    {
        public AddCurrencyDto AddCurrencyDto { get; set; }
    }
}