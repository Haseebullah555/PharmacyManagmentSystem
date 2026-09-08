using API.Controllers.Common;
using Application.Dtos.Unit;
using Application.Features.Unit.Requests.Commands;
using Application.Features.Unit.Requests.Queries;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UnitController : BaseApiController
    {
        [HttpGet("get-with-param")]
        public async Task<IActionResult> GetUnitsWithParam([FromQuery] string? search, [FromQuery] string? sort_field, [FromQuery] string? sort_order, [FromQuery] int page = 1, [FromQuery] int per_page = 10)
        {
            var units = await _mediator.Send(new GetListOfAllUnitsWithParamRequest
            {
                Search = search,
                SortBy = sort_field,
                SortDirection = sort_order,
                Page = page,
                PerPage = per_page
            });
            return Ok(new
            {
                data = units.Data,
                meta = new
                {
                    total = units.Total,
                    current_page = units.CurrentPage,
                    per_page = units.PerPage,
                    last_page = units.LastPage,
                    from = units.From,
                    to = units.To
                }
            });
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetUnitsList()
        {
            var units = await _mediator.Send(new GetUnitsListRequest());
            return Ok(units);
        }

        [HttpPost("add-unit")]
        public async Task<IActionResult> AddUnit(AddUnitDto unit)
        {
            if (ModelState.IsValid)
            {
                await _mediator.Send(new AddUnitCommand { AddUnitDto = unit });
                return Ok(new { message = "ثبت معلومات با موفقیت شد" });
            }
            return BadRequest(new { message = "اضافه نمودن معلومات ناموفق بود. لطفا ورودی خود را بررسی کنید.", errors = ModelState });
        }

        [HttpPut("update-unit")]
        public async Task<IActionResult> UpdateUnit(UpdateUnitDto unit)
        {
            if (ModelState.IsValid)
            {
                await _mediator.Send(new UpdateUnitCommand { UpdateUnitDto = unit });
                return Ok(new { message = "تغییرات معلومات با موفقیت شد" });
            }
            return BadRequest(new { message = "تجدید معلومات ناموفق بود. لطفا ورودی خود را بررسی کنید.", errors = ModelState });
        }
    }
}
