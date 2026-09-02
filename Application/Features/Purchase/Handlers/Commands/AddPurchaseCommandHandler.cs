using Application.Contracts.Interfaces.Common;
using Application.Contracts.UserManagement;
using Application.Features.Purchase.Requests.Commands;
using AutoMapper;
using MediatR;

namespace Application.Features.Purchase.Handlers.Commands
{
    public class AddPurchaseCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserRepository currentUser) : IRequestHandler<AddPurchaseCommand>
    {
        public async Task Handle(AddPurchaseCommand request, CancellationToken cancellationToken)
        {
            var purchase = mapper.Map<Domain.Models.Purchase>(request.AddPurchaseDto);
            purchase.CreatedAt = DateTime.UtcNow;
            purchase.CreatedBy = currentUser.GetCurrentLoggedInUserId();
            await unitOfWork.Purchases.AddAsync(purchase);
            await unitOfWork.SaveAsync(cancellationToken);
        }
    }
}
