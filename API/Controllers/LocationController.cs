using API.Controllers.Common;
using Application.Dtos.Location;
using Application.Features.Location.Requests.Commands;
using Application.Features.Location.Requests.Queries;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LocationController : BaseApiController
    {
        [HttpGet("get-with-param")]
        public async Task<IActionResult> GetLocationsWithParam([FromQuery] string? search, [FromQuery] string? sort_field, [FromQuery] string? sort_order, [FromQuery] int page = 1, [FromQuery] int per_page = 10)
        {
            var locations = await _mediator.Send(new GetListOfAllLocationsWithParamRequest
            {
                Search = search,
                SortBy = sort_field,
                SortDirection = sort_order,
                Page = page,
                PerPage = per_page
            });
            return Ok(new
            {
                data = locations.Data,
                meta = new
                {
                    total = locations.Total,
                    current_page = locations.CurrentPage,
                    per_page = locations.PerPage,
                    last_page = locations.LastPage,
                    from = locations.From,
                    to = locations.To
                }
            });
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetLocationsList()
        {
            var locations = await _mediator.Send(new GetLocationsListRequest());
            return Ok(locations);
        }

        [HttpPost("add-Location")]
        public async Task<IActionResult> AddLocation(AddLocationDto Location)
        {
            if (ModelState.IsValid)
            {
                await _mediator.Send(new AddLocationCommand { AddLocationDto = Location });
                return Ok(new { message = "ثبت معلومات با موفقیت شد" });
            }
            return BadRequest(new { message = "اضافه نمودن معلومات ناموفق بود. لطفا ورودی خود را بررسی کنید.", errors = ModelState });
        }

        [HttpPut("update-Location")]
        public async Task<IActionResult> UpdateLocation(UpdateLocationDto Location)
        {
            if (ModelState.IsValid)
            {
                await _mediator.Send(new UpdateLocationCommand { UpdateLocationDto = Location });
                return Ok(new { message = "تغییرات معلومات با موفقیت شد" });
            }
            return BadRequest(new { message = "تجدید معلومات ناموفق بود. لطفا ورودی خود را بررسی کنید.", errors = ModelState });
        }
    }
}
