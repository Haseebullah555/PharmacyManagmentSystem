using Application.Contracts.Interfaces.Common;
using Application.Dtos.Dashboard;
using Application.Features.Dashboard.Requests.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Dashboard.Handlers.Queries
{
    public class GetDashboardSummaryRequestHandler(IUnitOfWork unitOfWork)
        : IRequestHandler<GetDashboardSummaryRequest, DashboardSummaryDto>
    {
        public async Task<DashboardSummaryDto> Handle(
            GetDashboardSummaryRequest request,
            CancellationToken cancellationToken)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var expiryLimit = today.AddDays(30);
            var startDate = today.AddDays(-6);

            var stock = unitOfWork.InventoryStocks.Query().AsNoTracking()
                .Where(item => item.Quantity > 0);

            var summary = new DashboardSummaryDto
            {
                TodaySales = await unitOfWork.Sales.Query().AsNoTracking()
                    .Where(sale => sale.SaleDate == today)
                    .SumAsync(sale => (decimal?)sale.TotalAmount, cancellationToken) ?? 0,
                TodayPurchases = await unitOfWork.Purchases.Query().AsNoTracking()
                    .Where(purchase => purchase.PurchaseDate == today)
                    .SumAsync(purchase => (decimal?)purchase.TotalAmount, cancellationToken) ?? 0,
                TodaySaleCount = await unitOfWork.Sales.Query().AsNoTracking()
                    .CountAsync(sale => sale.SaleDate == today, cancellationToken),
                TodayPurchaseCount = await unitOfWork.Purchases.Query().AsNoTracking()
                    .CountAsync(purchase => purchase.PurchaseDate == today, cancellationToken),
                MedicinesInStock = await stock
                    .Select(item => item.InventoryBatch.MedicineId)
                    .Distinct()
                    .CountAsync(cancellationToken),
                TotalStockQuantity = await stock
                    .SumAsync(item => (decimal?)item.Quantity, cancellationToken) ?? 0,
                ExpiredMedicineCount = await stock
                    .Where(item => item.InventoryBatch.ExpiryDate < today)
                    .Select(item => item.InventoryBatch.MedicineId)
                    .Distinct()
                    .CountAsync(cancellationToken),
                ExpiringSoonCount = await stock
                    .Where(item => item.InventoryBatch.ExpiryDate >= today &&
                                   item.InventoryBatch.ExpiryDate <= expiryLimit)
                    .Select(item => item.InventoryBatch.MedicineId)
                    .Distinct()
                    .CountAsync(cancellationToken)
            };

            var salesByDate = await unitOfWork.Sales.Query().AsNoTracking()
                .Where(sale => sale.SaleDate >= startDate && sale.SaleDate <= today)
                .GroupBy(sale => sale.SaleDate)
                .Select(group => new { Date = group.Key, Total = group.Sum(sale => sale.TotalAmount) })
                .ToListAsync(cancellationToken);
            var purchasesByDate = await unitOfWork.Purchases.Query().AsNoTracking()
                .Where(purchase => purchase.PurchaseDate >= startDate && purchase.PurchaseDate <= today)
                .GroupBy(purchase => purchase.PurchaseDate)
                .Select(group => new { Date = group.Key, Total = group.Sum(purchase => purchase.TotalAmount) })
                .ToListAsync(cancellationToken);

            summary.DailySales = Enumerable.Range(0, 7)
                .Select(offset => startDate.AddDays(offset))
                .Select(date => new DashboardDailyTotalDto
                {
                    Date = date.ToString("MMM dd"),
                    Sales = salesByDate.FirstOrDefault(item => item.Date == date)?.Total ?? 0,
                    Purchases = purchasesByDate.FirstOrDefault(item => item.Date == date)?.Total ?? 0
                })
                .ToList();

            summary.ExpiryAlerts = await stock
                .Where(item => item.InventoryBatch.ExpiryDate < today ||
                               (item.InventoryBatch.ExpiryDate >= today &&
                                item.InventoryBatch.ExpiryDate <= expiryLimit))
                .OrderBy(item => item.InventoryBatch.ExpiryDate)
                .Take(6)
                .Select(item => new DashboardExpiryAlertDto
                {
                    MedicineName = item.InventoryBatch.Medicine.TradeName,
                    ExpiryDate = item.InventoryBatch.ExpiryDate,
                    Quantity = item.Quantity,
                    IsExpired = item.InventoryBatch.ExpiryDate < today
                })
                .ToListAsync(cancellationToken);

            return summary;
        }
    }
}
