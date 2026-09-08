using API.Controllers.Common;
using Application.Dtos.Currency;
using Application.Features.Currency.Requests.Commands;
using Application.Features.Currency.Requests.Queries;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CurrencyController : BaseApiController
    {
        [HttpGet("get-with-param")]
        public async Task<IActionResult> GetCurrenciesWithParam([FromQuery] string? search, [FromQuery] string? sort_field, [FromQuery] string? sort_order, [FromQuery] int page = 1, [FromQuery] int per_page = 10)
        {
            var currencies = await _mediator.Send(new GetListOfAllCurrenciesWithParamRequest
            {
                Search = search,
                SortBy = sort_field,
                SortDirection = sort_order,
                Page = page,
                PerPage = per_page
            });
            return Ok(new
            {
                data = currencies.Data,
                meta = new
                {
                    total = currencies.Total,
                    current_page = currencies.CurrentPage,
                    per_page = currencies.PerPage,
                    last_page = currencies.LastPage,
                    from = currencies.From,
                    to = currencies.To
                }
            });
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetCurrenciesList()
        {
            var currencies = await _mediator.Send(new GetCurrenciesListRequest());
            return Ok(currencies);
        }

        [HttpPost("add-supplier")]
        public async Task<IActionResult> AddCurrency(AddCurrencyDto supplier)
        {
            if (ModelState.IsValid)
            {
                await _mediator.Send(new AddCurrencyCommand { AddCurrencyDto = supplier });
                return Ok(new { message = "ثبت معلومات با موفقیت شد" });
            }
            return BadRequest(new { message = "اضافه نمودن معلومات ناموفق بود. لطفا ورودی خود را بررسی کنید.", errors = ModelState });
        }

        [HttpPut("update-supplier")]
        public async Task<IActionResult> UpdateCurrency(UpdateCurrencyDto supplier)
        {
            if (ModelState.IsValid)
            {
                await _mediator.Send(new UpdateCurrencyCommand { UpdateCurrencyDto = supplier });
                return Ok(new { message = "تغییرات معلومات با موفقیت شد" });
            }
            return BadRequest(new { message = "تجدید معلومات ناموفق بود. لطفا ورودی خود را بررسی کنید.", errors = ModelState });
        }
    }
}
