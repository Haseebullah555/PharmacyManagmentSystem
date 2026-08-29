using Application.Dtos.Sale;
using Application.Dtos.Inventory;
using Application.Features.Sale.Requests.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Domain.Models;
using Persistence.Database;

namespace API.Controllers
{
    [ApiController]
    [Route("api/sales")]
    public class SaleController(IMediator mediator, AppDbContext context) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<SaleDto>>> GetAll(CancellationToken cancellationToken)
        {
            var result = await context.Sales
                .AsNoTracking()
                .OrderByDescending(item => item.SaleDate)
                .Select(item => new SaleDto
                {
                    Id = item.Id,
                    SaleAmount = item.SaleAmount,
                    UnitPrice = item.UnitPrice,
                    TotalPrice = item.TotalPrice,
                    Paid = item.Paid,
                    Unpaid = item.Unpaid,
                    SaleDate = item.SaleDate,
                    MedicineID = item.MedicineID,
                    CurrencyID = item.CurrencyID,
                    CustomerID = item.CustomerID
                })
                .ToListAsync(cancellationToken);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AddSaleDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);
            try
            {
                await mediator.Send(new AddSaleCommand { AddSaleDto = dto }, cancellationToken);
                return Ok();
            }
            catch (InvalidOperationException exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpPost("returns")]
        public async Task<IActionResult> Return(SaleReturnDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var allocation = await context.SaleBatchAllocations
                .Include(item => item.InventoryBatch)
                .FirstOrDefaultAsync(item => item.Id == dto.SaleBatchAllocationID, cancellationToken);
            if (allocation is null)
                return NotFound("Sale batch allocation was not found.");

            var returnedQuantity = await context.SaleReturns
                .Where(item => item.SaleBatchAllocationID == dto.SaleBatchAllocationID)
                .SumAsync(item => (int?)item.Quantity, cancellationToken) ?? 0;
            if (returnedQuantity + dto.Quantity > allocation.Quantity)
                return BadRequest("Return quantity exceeds the quantity sold from this batch.");

            if (dto.Restock)
                allocation.InventoryBatch.QuantityAvailable += dto.Quantity;
            context.SaleReturns.Add(new SaleReturn
            {
                Quantity = dto.Quantity, Reason = dto.Reason, Restock = dto.Restock,
                ReturnDate = dto.ReturnDate ?? DateOnly.FromDateTime(DateTime.UtcNow),
                SaleBatchAllocationID = dto.SaleBatchAllocationID, CreatedAt = DateTime.UtcNow
            });
            await context.SaveChangesAsync(cancellationToken);
            return Ok();
        }
    }
}