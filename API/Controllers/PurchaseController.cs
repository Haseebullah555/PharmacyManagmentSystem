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
                    PurchaseDate = item.PurchaseDate,
                    InvoiceNumber = item.InvoiceNumber,
                    SupplierID = item.SupplierID,
                    CurrencyID = item.CurrencyID,
                    TotalAmount = item.TotalAmount,
                    PaidAmount = item.PaidAmount,
                    UnpaidAmount = item.UnpaidAmount,
                    Remarks = item.Remarks
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