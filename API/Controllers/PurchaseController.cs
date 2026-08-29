using Application.Dtos.Purchase;
using Application.Features.Purchase.Requests.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace API.Controllers
{
    [ApiController]
    [Route("api/purchases")]
    public class PurchaseController(IMediator mediator, AppDbContext context) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<PurchaseDto>>> GetAll(CancellationToken cancellationToken)
        {
            var result = await context.Purchases
                .AsNoTracking()
                .OrderByDescending(item => item.PurchaseDate)
                .Select(item => new PurchaseDto
                {
                    Id = item.Id,
                    Amount = item.Amount,
                    UnitPrice = item.UnitPrice,
                    SalePrice = item.SalePrice,
                    TotalPrice = item.TotalPrice,
                    Paid = item.Paid,
                    Unpaid = item.Unpaid,
                    PurchaseDate = item.PurchaseDate,
                    ExpiryDate = item.ExpiryDate ?? DateOnly.MinValue,
                    MedicineID = item.MedicineID,
                    SupplierID = item.SupplierID,
                    CurrencyID = item.CurrencyID
                })
                .ToListAsync(cancellationToken);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AddPurchaseDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);
            try
            {
                await mediator.Send(new AddPurchaseCommand { AddPurchaseDto = dto }, cancellationToken);
                return Ok();
            }
            catch (Exception exception) when (exception is InvalidOperationException)
            {
                return BadRequest(exception.Message);
            }
        }
    }
}