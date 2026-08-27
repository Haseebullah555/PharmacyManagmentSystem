using Application.Dtos.Sale;
using MediatR;

namespace Application.Features.Sale.Requests.Commands
{
    public class AddSaleCommand : IRequest
    {
        public AddSaleDto AddSaleDto { get; set; }
    }
}
