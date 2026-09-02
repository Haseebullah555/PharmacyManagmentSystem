using API.Controllers.Common;
using Application.Dtos.Supplier;
using Application.Features.Supplier.Requests.Commands;
using Application.Features.Supplier.Requests.Queries;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SupplierController : BaseApiController
    {
        [HttpGet("get-with-param")]
        public async Task<IActionResult> GetSuppliersWithParam([FromQuery] string? search, [FromQuery] string? sort_field, [FromQuery] string? sort_order, [FromQuery] int page = 1, [FromQuery] int per_page = 10)
        {
            var categories = await _mediator.Send(new GetListOfAllSuppliersWithParamRequest
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

        [HttpGet("get-all")]
        public async Task<IActionResult> GetSuppliersList()
        {
            var categories = await _mediator.Send(new GetSuppliersListRequest());
            return Ok(categories);
        }

        [HttpPost("add-supplier")]
        public async Task<IActionResult> AddSupplier(AddSupplierDto supplier)
        {
            if (ModelState.IsValid)
            {
                await _mediator.Send(new AddSupplierCommand { AddSupplierDto = supplier });
                return Ok(new { message = "ثبت معلومات با موفقیت شد" });
            }
            return BadRequest(new { message = "اضافه نمودن معلومات ناموفق بود. لطفا ورودی خود را بررسی کنید.", errors = ModelState });
        }

        [HttpPut("update-supplier")]
        public async Task<IActionResult> UpdateSupplier(UpdateSupplierDto supplier)
        {
            if (ModelState.IsValid)
            {
                await _mediator.Send(new UpdateSupplierCommand { UpdateSupplierDto = supplier });
                return Ok(new { message = "تغییرات معلومات با موفقیت شد" });
            }
            return BadRequest(new { message = "تجدید معلومات ناموفق بود. لطفا ورودی خود را بررسی کنید.", errors = ModelState });
        }
    }
}
