using Application.Contracts.Interfaces.Common;
using Application.Contracts.UserManagement;
using Application.Features.Purchase.Requests.Commands;
using AutoMapper;
using MediatR;

namespace Application.Features.Purchase.Handlers.Commands
{
    public class UpdatePurchaseCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserRepository currentUser) : IRequestHandler<UpdatePurchaseCommand>
    {
        public async Task Handle(UpdatePurchaseCommand request, CancellationToken cancellationToken)
        {
            var purchase = mapper.Map<Domain.Models.Purchase>(request.UpdatePurchaseDto);
            purchase.UpdatedAt = DateTime.UtcNow;
            purchase.UpdateBy = currentUser.GetCurrentLoggedInUserId();
            unitOfWork.Purchases.Update(purchase);
            await unitOfWork.SaveAsync(cancellationToken);
        }
    }
}
