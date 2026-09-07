using API.Controllers.Common;
using Application.Dtos.Customer;
using Application.Features.Customer.Requests.Commands;
using Application.Features.Customer.Requests.Queries;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : BaseApiController
    {
        [HttpGet("get-with-param")]
        public async Task<IActionResult> GetCustomersWithParam([FromQuery] string? search, [FromQuery] string? sort_field, [FromQuery] string? sort_order, [FromQuery] int page = 1, [FromQuery] int per_page = 10)
        {
            var categories = await _mediator.Send(new GetListOfAllCustomersWithParamRequest
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
        public async Task<IActionResult> GetCustomersList()
        {
            var categories = await _mediator.Send(new GetCustomersListRequest());
            return Ok(categories);
        }

        [HttpPost("add-customer")]
        public async Task<IActionResult> AddCustomer(AddCustomerDto customer)
        {
            if (ModelState.IsValid)
            {
                await _mediator.Send(new AddCustomerCommand { AddCustomerDto = customer });
                return Ok(new { message = "ثبت معلومات با موفقیت شد" });
            }
            return BadRequest(new { message = "اضافه نمودن معلومات ناموفق بود. لطفا ورودی خود را بررسی کنید.", errors = ModelState });
        }

        [HttpPut("update-customer")]
        public async Task<IActionResult> UpdateCustomer(UpdateCustomerDto customer)
        {
            if (ModelState.IsValid)
            {
                await _mediator.Send(new UpdateCustomerCommand { UpdateCustomerDto = customer });
                return Ok(new { message = "تغییرات معلومات با موفقیت شد" });
            }
            return BadRequest(new { message = "تجدید معلومات ناموفق بود. لطفا ورودی خود را بررسی کنید.", errors = ModelState });
        }
    }
}
