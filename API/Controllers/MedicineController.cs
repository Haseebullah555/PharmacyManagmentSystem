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
            var medicines = await _mediator.Send(new GetListOfAllMedicinesWithParamRequest
            {
                Search = search,
                SortBy = sort_field,
                SortDirection = sort_order,
                Page = page,
                PerPage = per_page
            });
            return Ok(new
            {
                data = medicines.Data,
                meta = new
                {
                    total = medicines.Total,
                    current_page = medicines.CurrentPage,
                    per_page = medicines.PerPage,
                    last_page = medicines.LastPage,
                    from = medicines.From,
                    to = medicines.To
                }
            });
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetMedicinesList()
        {
            var medicines = await _mediator.Send(new GetMedicinesListRequest());
            return Ok(medicines);
        }

        [HttpPost("add-medicine")]
        public async Task<IActionResult> AddMedicine(AddMedicineDto medicine)
        {
            if (ModelState.IsValid)
            {
                await _mediator.Send(new AddMedicineCommand { AddMedicineDto = medicine });
                return Ok(new { message = "ثبت معلومات با موفقیت شد" });
            }
            return BadRequest(new { message = "اضافه نمودن معلومات ناموفق بود. لطفا ورودی خود را بررسی کنید.", errors = ModelState });
        }

        [HttpPut("update-medicine")]
        public async Task<IActionResult> UpdateMedicine(UpdateMedicineDto medicine)
        {
            if (ModelState.IsValid)
            {
                await _mediator.Send(new UpdateMedicineCommand { UpdateMedicineDto = medicine });
                return Ok(new { message = "تغییرات معلومات با موفقیت شد" });
            }
            return BadRequest(new { message = "تجدید معلومات ناموفق بود. لطفا ورودی خود را بررسی کنید.", errors = ModelState });
        }
    }
}
