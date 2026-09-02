using API.Controllers.Common;
using Application.Dtos.Medicine;
using Application.Features.Medicine.Handlers.Queries;
using Application.Features.Medicine.Requests.Commands;
using Application.Features.Medicine.Requests.Queries;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MedicineController : BaseApiController
    {
        [HttpGet("get-with-param")]
        public async Task<IActionResult> GetMedicinesWithParam([FromQuery] string? search, [FromQuery] string? sort_field, [FromQuery] string? sort_order, [FromQuery] int page = 1, [FromQuery] int per_page = 10)
        {
            var categories = await _mediator.Send(new GetListOfAllMedicinesWithParamRequest
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
        public async Task<IActionResult> GetMedicinesList()
        {
            var categories = await _mediator.Send(new GetMedicinesListRequest());
            return Ok(categories);
        }

        [HttpPost("add-category")]
        public async Task<IActionResult> AddMedicine(AddMedicineDto category)
        {
            if (ModelState.IsValid)
            {
                await _mediator.Send(new AddMedicineCommand { AddMedicineDto = category });
                return Ok(new { message = "ثبت معلومات با موفقیت شد" });
            }
            return BadRequest(new { message = "اضافه نمودن معلومات ناموفق بود. لطفا ورودی خود را بررسی کنید.", errors = ModelState });
        }

        [HttpPut("update-category")]
        public async Task<IActionResult> UpdateMedicine(UpdateMedicineDto category)
        {
            if (ModelState.IsValid)
            {
                await _mediator.Send(new UpdateMedicineCommand { UpdateMedicineDto = category });
                return Ok(new { message = "تغییرات معلومات با موفقیت شد" });
            }
            return BadRequest(new { message = "تجدید معلومات ناموفق بود. لطفا ورودی خود را بررسی کنید.", errors = ModelState });
        }
    }
}
