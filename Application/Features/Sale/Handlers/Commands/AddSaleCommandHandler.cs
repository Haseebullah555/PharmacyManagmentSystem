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
                .Include(batch => batch.Stocks)
                .Where(batch => batch.MedicineID == sale.MedicineID && batch.Stocks.Any(stock => stock.Quantity > 0))
                .Where(batch => batch.ExpiryDate == null || batch.ExpiryDate >= sale.SaleDate)
                .OrderBy(batch => batch.ExpiryDate == null)
                .ThenBy(batch => batch.ExpiryDate)
                .ThenBy(batch => batch.CreatedAt)
                .ToListAsync(cancellationToken);

            if (batches.SelectMany(batch => batch.Stocks).Sum(stock => stock.Quantity) < quantityToSell)
                throw new InvalidOperationException("Insufficient non-expired stock for this medicine.");

            sale.CreatedAt = DateTime.UtcNow;
            sale.CreatedBy = currentUser.GetCurrentLoggedInUserId();
            var remainingQuantity = quantityToSell;
            foreach (var batch in batches)
            {
                if (remainingQuantity == 0)
                    break;

                var allocatedQuantity = Math.Min(decimal.ToInt32(batch.Stocks.Sum(stock => stock.Quantity)), remainingQuantity);
                var stockQuantityToReduce = allocatedQuantity;
                foreach (var stock in batch.Stocks)
                {
                    if (stockQuantityToReduce == 0)
                        break;

                    var stockAllocation = Math.Min(stock.Quantity, stockQuantityToReduce);
                    stock.Quantity -= stockAllocation;
                    // stockQuantityToReduce -= stockAllocation;
                }
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
