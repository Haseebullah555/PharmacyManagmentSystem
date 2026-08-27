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
            var batch = new Domain.Models.InventoryBatch
            {
                BatchNumber = string.IsNullOrWhiteSpace(purchase.BatchNumber)
                    ? $"PUR-{Guid.NewGuid():N}"
                    : purchase.BatchNumber,
                ExpiryDate = purchase.ExpiryDate,
                QuantityReceived = purchase.Amount,
                QuantityAvailable = purchase.Amount,
                UnitCost = purchase.UnitPrice,
                SalePrice = purchase.SalePrice,
                ReceivedDate = purchase.PurchaseDate,
                MedicineID = purchase.MedicineID,
                SupplierID = purchase.SupplierID,
                CreatedAt = purchase.CreatedAt,
                CreatedBy = purchase.CreatedBy
            };
            await unitOfWork.InventoryBatches.AddAsync(batch);
            purchase.InventoryBatch = batch;
            await unitOfWork.Purchases.AddAsync(purchase);
            await unitOfWork.SaveAsync(cancellationToken);
        }
    }
}
