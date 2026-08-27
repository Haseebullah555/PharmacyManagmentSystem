using Application.Dtos.Purchase;
using Application.Features.Purchase.Requests.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/purchases")]
    public class PurchaseController(IMediator mediator) : ControllerBase
    {
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