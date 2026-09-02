using API.Controllers.Common;
using Application.Dtos.Dosage;
using Application.Features.Dosage.Requests.Commands;
using Application.Features.Dosage.Requests.Queries;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DosageController : BaseApiController
    {
        [HttpGet("get-with-param")]
        public async Task<IActionResult> GetDosagesWithParam([FromQuery] string? search, [FromQuery] string? sort_field, [FromQuery] string? sort_order, [FromQuery] int page = 1, [FromQuery] int per_page = 10)
        {
            var categories = await _mediator.Send(new GetListOfAllDosagesWithParamRequest
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
        public async Task<IActionResult> GetDosagesList()
        {
            var categories = await _mediator.Send(new GetDosagesListRequest());
            return Ok(categories);
        }

        [HttpPost("add-dosage")]
        public async Task<IActionResult> AddDosage(AddDosageDto dosage)
        {
            if (ModelState.IsValid)
            {
                await _mediator.Send(new AddDosageCommand { AddDosageDto = dosage });
                return Ok(new { message = "ثبت معلومات با موفقیت شد" });
            }
            return BadRequest(new { message = "اضافه نمودن معلومات ناموفق بود. لطفا ورودی خود را بررسی کنید.", errors = ModelState });
        }

        [HttpPut("update-dosage")]
        public async Task<IActionResult> UpdateDosage(UpdateDosageDto dosage)
        {
            if (ModelState.IsValid)
            {
                await _mediator.Send(new UpdateDosageCommand { UpdateDosageDto = dosage });
                return Ok(new { message = "تغییرات معلومات با موفقیت شد" });
            }
            return BadRequest(new { message = "تجدید معلومات ناموفق بود. لطفا ورودی خود را بررسی کنید.", errors = ModelState });
        }
    }
}
