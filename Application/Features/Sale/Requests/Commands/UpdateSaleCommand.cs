using Application.Dtos.Sale;
using MediatR;

namespace Application.Features.Sale.Requests.Commands
{
    public class UpdateSaleCommand : IRequest
    {
        public UpdateSaleDto UpdateSaleDto { get; set; }
    }
}
