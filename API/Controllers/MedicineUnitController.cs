using API.Controllers.Common;
using Application.Dtos.MedicineUnit;
using Application.Features.MedicineUnit.Requests.Commands;
using Application.Features.MedicineUnit.Requests.Queries;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MedicineUnitController : BaseApiController
    {
        [HttpGet("get-with-param")]
        public async Task<IActionResult> GetMedicineUnitsWithParam([FromQuery] string? search, [FromQuery] string? sort_field, [FromQuery] string? sort_order, [FromQuery] int page = 1, [FromQuery] int per_page = 10)
        {
            var medicineLocations = await _mediator.Send(new GetListOfAllMedicineUnitsWithParamRequest
            {
                Search = search,
                SortBy = sort_field,
                SortDirection = sort_order,
                Page = page,
                PerPage = per_page
            });
            return Ok(new
            {
                data = medicineLocations.Data,
                meta = new
                {
                    total = medicineLocations.Total,
                    current_page = medicineLocations.CurrentPage,
                    per_page = medicineLocations.PerPage,
                    last_page = medicineLocations.LastPage,
                    from = medicineLocations.From,
                    to = medicineLocations.To
                }
            });
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetMedicineUnitsList()
        {
            var medicineLocations = await _mediator.Send(new GetMedicineUnitsListRequest());
            return Ok(medicineLocations);
        }

        [HttpPost("add-medicine-unit")]
        public async Task<IActionResult> AddMedicineUnit(AddMedicineUnitDto MedicineUnit)
        {
            if (ModelState.IsValid)
            {
                await _mediator.Send(new AddMedicineUnitCommand { AddMedicineUnitDto = MedicineUnit });
                return Ok(new { message = "ثبت معلومات با موفقیت شد" });
            }
            return BadRequest(new { message = "اضافه نمودن معلومات ناموفق بود. لطفا ورودی خود را بررسی کنید.", errors = ModelState });
        }

        [HttpPut("update-medicine-unit")]
        public async Task<IActionResult> UpdateMedicineUnit(UpdateMedicineUnitDto MedicineUnit)
        {
            if (ModelState.IsValid)
            {
                await _mediator.Send(new UpdateMedicineUnitCommand { UpdateMedicineUnitDto = MedicineUnit });
                return Ok(new { message = "تغییرات معلومات با موفقیت شد" });
            }
            return BadRequest(new { message = "تجدید معلومات ناموفق بود. لطفا ورودی خود را بررسی کنید.", errors = ModelState });
        }
    }
}
