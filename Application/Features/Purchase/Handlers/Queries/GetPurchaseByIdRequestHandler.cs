using Application.Contracts.Interfaces.Common;
using Application.Dtos.Purchase;
using Application.Dtos.PurchaseItem;
using Application.Features.Purchase.Requests.Queries;
using MediatR;

namespace Application.Features.Purchase.Handlers.Queries
{
    public class GetPurchaseByIdHandler(IUnitOfWork _unitOfWork): IRequestHandler<GetPurchaseByIdRequest, PurchaseDto?>
    {

        public async Task<PurchaseDto?> Handle(GetPurchaseByIdRequest request, CancellationToken cancellationToken)
        {
            var purchase = await _unitOfWork.Purchases.GetByIdAsync(request.Id);

            if (purchase == null)
            {
                return null;
            }

            return new PurchaseDto
            {
                Id = purchase.Id,
                PurchaseDate = purchase.PurchaseDate,
                InvoiceNumber = purchase.InvoiceNumber,
                SupplierId = purchase.SupplierId,
                CurrencyId = purchase.CurrencyId,
                TotalAmount = purchase.TotalAmount,
                PaidAmount = purchase.PaidAmount,
                UnpaidAmount = purchase.UnpaidAmount,
                Remarks = purchase.Remarks,

                Items = purchase.Items
                    .Select(item => new PurchaseItemDto
                    {
                        Id = item.Id,
                        MedicineId = item.MedicineId,
                        MedicineUnitId = item.MedicineUnitId,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice,
                        TotalPrice = item.TotalPrice,

                        // Map these according to your actual
                        // PurchaseItem / InventoryBatch structure.
                    })
                    .ToList()
            };
        }
    }
}