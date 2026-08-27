using Application.Dtos.Purchase;
using MediatR;

namespace Application.Features.Purchase.Requests.Commands
{
    public class AddPurchaseCommand : IRequest
    {
        public AddPurchaseDto AddPurchaseDto { get; set; }
    }
}
