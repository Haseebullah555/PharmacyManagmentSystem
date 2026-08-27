using Application.Dtos.Purchase;
using MediatR;

namespace Application.Features.Purchase.Requests.Commands
{
    public class UpdatePurchaseCommand : IRequest
    {
        public UpdatePurchaseDto UpdatePurchaseDto { get; set; }
    }
}
