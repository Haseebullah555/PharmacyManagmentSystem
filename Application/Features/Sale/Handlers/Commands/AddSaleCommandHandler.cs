using Application.Contracts.Interfaces.Common;
using Application.Contracts.UserManagement;
using Application.Features.Sale.Requests.Commands;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Sale.Handlers.Commands
{
    public class AddSaleCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserRepository currentUser) : IRequestHandler<AddSaleCommand>
    {
        public async Task Handle(AddSaleCommand request, CancellationToken cancellationToken)
        {
            var sale = mapper.Map<Domain.Models.Sale>(request.AddSaleDto);
            if (sale.SaleAmount <= 0 || decimal.Truncate(sale.SaleAmount) != sale.SaleAmount)
                throw new InvalidOperationException("Sale quantity must be a positive whole number.");

            var quantityToSell = decimal.ToInt32(sale.SaleAmount);
            var batches = await unitOfWork.InventoryBatches.Query()
                .Where(batch => batch.MedicineID == sale.MedicineID && batch.QuantityAvailable > 0)
                .Where(batch => batch.ExpiryDate == null || batch.ExpiryDate >= sale.SaleDate)
                .OrderBy(batch => batch.ExpiryDate == null)
                .ThenBy(batch => batch.ExpiryDate)
                .ThenBy(batch => batch.ReceivedDate)
                .ToListAsync(cancellationToken);

            if (batches.Sum(batch => batch.QuantityAvailable) < quantityToSell)
                throw new InvalidOperationException("Insufficient non-expired stock for this medicine.");

            sale.CreatedAt = DateTime.UtcNow;
            sale.CreatedBy = currentUser.GetCurrentLoggedInUserId();
            var remainingQuantity = quantityToSell;
            foreach (var batch in batches)
            {
                if (remainingQuantity == 0)
                    break;

                var allocatedQuantity = Math.Min(batch.QuantityAvailable, remainingQuantity);
                batch.QuantityAvailable -= allocatedQuantity;
                sale.BatchAllocations.Add(new Domain.Models.SaleBatchAllocation
                {
                    Quantity = allocatedQuantity,
                    InventoryBatch = batch,
                    CreatedAt = sale.CreatedAt,
                    CreatedBy = sale.CreatedBy
                });
                remainingQuantity -= allocatedQuantity;
            }
            await unitOfWork.Sales.AddAsync(sale);
            await unitOfWork.SaveAsync(cancellationToken);
        }
    }
}
