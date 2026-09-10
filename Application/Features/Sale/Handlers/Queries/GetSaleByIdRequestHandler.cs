using Application.Contracts.Interfaces.Common;
using Application.Dtos.Sale;
using Application.Dtos.SaleItem;
using Application.Features.Sale.Requests.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Sale.Handlers.Queries
{
    public class GetSaleByIdHandler( IUnitOfWork unitOfWork) : IRequestHandler<GetSaleByIdRequest, SaleDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<SaleDto?> Handle(GetSaleByIdRequest request, CancellationToken cancellationToken)
        {
            var sale = await _unitOfWork.Sales
                .Query()
                .AsNoTracking()
                .Where(x => x.Id == request.Id)
                .Select(x => new SaleDto
                {
                    Id = x.Id,

                    SaleDate = x.SaleDate,

                    CustomerId = x.CustomerId,
                    Customer = x.Customer != null
                        ? x.Customer.CustomerName
                        : null,

                    CurrencyId = x.CurrencyId,
                    Currency = x.Currency.CurrencyName,

                    TotalAmount = x.TotalAmount,
                    PaidAmount = x.PaidAmount,
                    UnpaidAmount = x.UnpaidAmount,

                    Discount = x.Discount,

                    InvoiceNumber = x.InvoiceNumber,
                    Remarks = x.Remarks,

                    Items = x.Items
                        .Select(item => new SaleItemDto
                        {
                            Id = item.Id,

                            MedicineId = item.MedicineId,
                            MedicineName = item.Medicine.TradeName,

                            MedicineUnitId = item.MedicineUnitId,
                            MedicineUnitName =
                                item.MedicineUnit.Unit.Name,

                            InventoryBatchId =
                                item.InventoryBatchId,

                            BatchNumber =
                                item.InventoryBatch.BatchNumber,

                            ExpiryDate =
                                item.InventoryBatch.ExpiryDate,

                            LocationId =
                                item.LocationId,

                            LocationName =
                                item.Location.LocationName,

                            Quantity = item.Quantity,
                            UnitPrice = item.UnitPrice,
                            Discount = item.Discount,
                            TotalPrice = item.TotalPrice
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync(cancellationToken);

            return sale;
        }
    }
}