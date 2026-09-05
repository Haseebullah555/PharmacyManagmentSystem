using API.Controllers.Common;
using Application.Dtos.Sale;
using Application.Features.Sale.Requests.Commands;
using Application.Features.Sale.Requests.Queries;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SaleController : BaseApiController
    {
        [HttpGet("get-with-param")]
        public async Task<IActionResult> GetSalesWithParam([FromQuery] string? search, [FromQuery] string? sort_field, [FromQuery] string? sort_order, [FromQuery] int page = 1, [FromQuery] int per_page = 10)
        {
            var categories = await _mediator.Send(new GetListOfAllSalesWithParamRequest
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
        [HttpPost("add-purchase")]
        public async Task<IActionResult> AddSale(AddSaleDto purchase)
        {
            if (ModelState.IsValid)
            {
                await _mediator.Send(new AddSaleCommand { AddSaleDto = purchase });
                return Ok(new { message = "ثبت معلومات با موفقیت شد" });
            }
            return BadRequest(new { message = "اضافه نمودن معلومات ناموفق بود. لطفا ورودی خود را بررسی کنید.", errors = ModelState });
        }

        [HttpPut("update-purchase")]
        public async Task<IActionResult> UpdateSale(UpdateSaleDto purchase)
        {
            if (ModelState.IsValid)
            {
                await _mediator.Send(new UpdateSaleCommand { UpdateSaleDto = purchase });
                return Ok(new { message = "تغییرات معلومات با موفقیت شد" });
           }
            return BadRequest(new { message = "تجدید معلومات ناموفق بود. لطفا ورودی خود را بررسی کنید.", errors = ModelState });
        }
    }
}
