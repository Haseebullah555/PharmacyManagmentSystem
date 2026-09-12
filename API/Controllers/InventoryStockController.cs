using API.Controllers.Common;
using Application.Features.InventoryStock.Requests.Queries;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryStockController : BaseApiController
    {
        [HttpGet("get-with-param")]
        public async Task<IActionResult> GetInventoryStocksWithParam([FromQuery] string? search, [FromQuery] string? sort_field, [FromQuery] string? sort_order, [FromQuery] int page = 1, [FromQuery] int per_page = 10)
        {
            var categories = await _mediator.Send(new GetInventoryStockWithParamsRequest
            {
                Search = search,
                SortBy = sort_field,
                SortDirection = sort_order,
                Page = page,
                PerPage = per_page
            });
            return Ok(new
            {
                data = categories.Data,
                meta = new
                {
                    total = categories.Total,
                    current_page = categories.CurrentPage,
                    per_page = categories.PerPage,
                    last_page = categories.LastPage,
                    from = categories.From,
                    to = categories.To
                }
            });
        }

        [HttpGet("stocks-for-sale")]
        public async Task<IActionResult> GetStocksForSale([FromQuery] int? MedicineId)
        {
            var result = await _mediator.Send(
                new GetInventoryStocksForSaleQuery
                {
                    MedicineId = MedicineId
                });

            return Ok(result);
        }
    }
}
