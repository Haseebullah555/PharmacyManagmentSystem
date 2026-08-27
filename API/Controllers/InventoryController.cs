using Application.Dtos.Inventory;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace API.Controllers
{
    [ApiController]
    [Route("api/inventory")]
    public class InventoryController(AppDbContext context) : ControllerBase
    {
        [HttpPost("adjustments")]
        public async Task<IActionResult> CreateAdjustment(InventoryAdjustmentDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var batch = dto.InventoryBatchID.HasValue
                ? await context.InventoryBatches.FirstOrDefaultAsync(item => item.Id == dto.InventoryBatchID.Value && item.MedicineID == dto.MedicineID, cancellationToken)
                : await context.InventoryBatches.Where(item => item.MedicineID == dto.MedicineID && item.QuantityAvailable > 0)
                    .OrderBy(item => item.ExpiryDate == null).ThenBy(item => item.ExpiryDate).FirstOrDefaultAsync(cancellationToken);
            if (batch is null)
                return NotFound("No matching inventory batch was found.");
            if (dto.Quantity > batch.QuantityAvailable)
                return BadRequest("Adjustment quantity exceeds available stock.");

            batch.QuantityAvailable -= dto.Quantity;
            context.InventoryAdjustments.Add(new InventoryAdjustment
            {
                Quantity = dto.Quantity, Reason = dto.Reason, Notes = dto.Notes,
                AdjustmentDate = dto.AdjustmentDate ?? DateOnly.FromDateTime(DateTime.UtcNow),
                MedicineID = dto.MedicineID, InventoryBatchID = batch.Id, CreatedAt = DateTime.UtcNow
            });
            await context.SaveChangesAsync(cancellationToken);
            return Ok();
        }
        [HttpGet("low-stock")]
        public async Task<ActionResult<List<InventoryAlertDto>>> GetLowStock(CancellationToken cancellationToken)
        {
            var result = await context.Medicines
                .Select(medicine => new InventoryAlertDto
                {
                    MedicineID = medicine.Id,
                    GenericName = medicine.GenericName,
                    TradeName = medicine.TradeName,
                    Barcode = medicine.Barcode,
                    QuantityAvailable = medicine.InventoryBatches.Sum(batch => batch.QuantityAvailable),
                    ReorderLevel = medicine.ReorderLevel,
                    IsActive = medicine.IsActive,
                    NearestExpiryDate = medicine.InventoryBatches
                        .Where(batch => batch.QuantityAvailable > 0 && batch.ExpiryDate != null)
                        .OrderBy(batch => batch.ExpiryDate)
                        .Select(batch => batch.ExpiryDate)
                        .FirstOrDefault()
                })
                .Where(medicine => medicine.QuantityAvailable <= medicine.ReorderLevel && medicine.IsActive)
                .ToListAsync(cancellationToken);

            return Ok(result);
        }

        [HttpGet("expiring")]
        public async Task<ActionResult<List<InventoryAlertDto>>> GetExpiring(
            [FromQuery] int days = 30,
            CancellationToken cancellationToken = default)
        {
            if (days < 0)
                return BadRequest("Days must be zero or greater.");

            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var lastDate = today.AddDays(days);
            var result = await context.InventoryBatches
                .Where(batch => batch.QuantityAvailable > 0 && batch.ExpiryDate >= today && batch.ExpiryDate <= lastDate)
                .OrderBy(batch => batch.ExpiryDate)
                .Select(batch => new InventoryAlertDto
                {
                    MedicineID = batch.MedicineID,
                    GenericName = batch.Medicine.GenericName,
                    TradeName = batch.Medicine.TradeName,
                    Barcode = batch.Medicine.Barcode,
                    QuantityAvailable = batch.QuantityAvailable,
                    ReorderLevel = batch.Medicine.ReorderLevel,
                    NearestExpiryDate = batch.ExpiryDate
                })
                .ToListAsync(cancellationToken);

            return Ok(result);
        }

        [HttpGet("expired")]
        public async Task<ActionResult<List<InventoryAlertDto>>> GetExpired(CancellationToken cancellationToken)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var result = await context.InventoryBatches
                .Where(batch => batch.QuantityAvailable > 0 && batch.ExpiryDate < today)
                .OrderBy(batch => batch.ExpiryDate)
                .Select(batch => new InventoryAlertDto
                {
                    MedicineID = batch.MedicineID,
                    GenericName = batch.Medicine.GenericName,
                    TradeName = batch.Medicine.TradeName,
                    Barcode = batch.Medicine.Barcode,
                    QuantityAvailable = batch.QuantityAvailable,
                    ReorderLevel = batch.Medicine.ReorderLevel,
                    NearestExpiryDate = batch.ExpiryDate
                })
                .ToListAsync(cancellationToken);

            return Ok(result);
        }

        [HttpGet("daily-sales")]
        public async Task<ActionResult<DailySalesDto>> GetDailySales(
            [FromQuery] DateOnly? date = null,
            CancellationToken cancellationToken = default)
        {
            var salesDate = date ?? DateOnly.FromDateTime(DateTime.UtcNow);
            var result = await context.Sales
                .Where(sale => sale.SaleDate == salesDate)
                .GroupBy(sale => sale.SaleDate)
                .Select(group => new DailySalesDto
                {
                    Date = group.Key,
                    TransactionCount = group.Count(),
                    QuantitySold = group.Sum(sale => sale.SaleAmount),
                    TotalSales = group.Sum(sale => sale.TotalPrice),
                    TotalPaid = group.Sum(sale => sale.Paid),
                    TotalUnpaid = group.Sum(sale => sale.Unpaid)
                })
                .FirstOrDefaultAsync(cancellationToken);

            return Ok(result ?? new DailySalesDto { Date = salesDate });
        }

        [HttpGet("reports/profit")]
        public async Task<ActionResult<ProfitReportDto>> GetProfit(
            [FromQuery] DateOnly? from = null,
            [FromQuery] DateOnly? to = null,
            CancellationToken cancellationToken = default)
        {
            if (from.HasValue && to.HasValue && from > to)
                return BadRequest("From date must not be after To date.");

            var allocations = await context.SaleBatchAllocations
                .Where(allocation => (!from.HasValue || allocation.Sale.SaleDate >= from) &&
                                     (!to.HasValue || allocation.Sale.SaleDate <= to))
                .Include(allocation => allocation.Sale)
                .Include(allocation => allocation.InventoryBatch)
                .Include(allocation => allocation.SaleReturns)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
            var netAllocations = allocations.Select(item => new
            {
                Quantity = item.Quantity - item.SaleReturns.Sum(returnItem => returnItem.Quantity),
                Sale = item.Sale,
                Batch = item.InventoryBatch
            }).Where(item => item.Quantity > 0).ToList();
            var revenue = netAllocations.Sum(item => item.Quantity * item.Sale.UnitPrice);
            var cost = netAllocations.Sum(item => item.Quantity * item.Batch.UnitCost);
            var quantity = netAllocations.Sum(item => (decimal)item.Quantity);
            return Ok(new ProfitReportDto
            {
                From = from, To = to, Revenue = revenue, Cost = cost,
                GrossProfit = revenue - cost, QuantitySold = quantity
            });
        }

        [HttpGet("reports/stock-valuation")]
        public async Task<ActionResult<List<StockValuationDto>>> GetStockValuation(CancellationToken cancellationToken)
        {
            var result = await context.InventoryBatches
                .Where(batch => batch.QuantityAvailable > 0)
                .GroupBy(batch => new { batch.MedicineID, batch.Medicine.GenericName, batch.Medicine.TradeName })
                .Select(group => new StockValuationDto
                {
                    MedicineID = group.Key.MedicineID,
                    GenericName = group.Key.GenericName,
                    TradeName = group.Key.TradeName,
                    QuantityAvailable = group.Sum(batch => batch.QuantityAvailable),
                    CostValue = group.Sum(batch => batch.QuantityAvailable * batch.UnitCost),
                    RetailValue = group.Sum(batch => batch.QuantityAvailable * batch.SalePrice)
                })
                .OrderByDescending(item => item.CostValue)
                .ToListAsync(cancellationToken);
            return Ok(result);
        }
    }
}