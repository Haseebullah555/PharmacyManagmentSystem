using API.Controllers.Common;
using Application.Dtos.Company;
using Application.Features.Company.Requests.Commands;
using Application.Features.Company.Requests.Queries;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompanyController : BaseApiController
    {
        [HttpGet("get-with-param")]
        public async Task<IActionResult> GetCompaniesWithParam([FromQuery] string? search, [FromQuery] string? sort_field, [FromQuery] string? sort_order, [FromQuery] int page = 1, [FromQuery] int per_page = 10)
        {
            var categories = await _mediator.Send(new GetListOfAllCompaniesWithParamRequest
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
        public async Task<IActionResult> GetCompanysList()
        {
            var categories = await _mediator.Send(new GetCompaniesListRequest());
            return Ok(categories);
        }

        [HttpPost("add-company")]
        public async Task<IActionResult> AddCompany(AddCompanyDto company)
        {
            if (ModelState.IsValid)
            {
                await _mediator.Send(new AddCompanyCommand { AddCompanyDto = company });
                return Ok(new { message = "ثبت معلومات با موفقیت شد" });
            }
            return BadRequest(new { message = "اضافه نمودن معلومات ناموفق بود. لطفا ورودی خود را بررسی کنید.", errors = ModelState });
        }

        [HttpPut("update-company")]
        public async Task<IActionResult> UpdateCompany(UpdateCompanyDto company)
        {
            if (ModelState.IsValid)
            {
                await _mediator.Send(new UpdateCompanyCommand { UpdateCompanyDto = company });
                return Ok(new { message = "تغییرات معلومات با موفقیت شد" });
            }
            return BadRequest(new { message = "تجدید معلومات ناموفق بود. لطفا ورودی خود را بررسی کنید.", errors = ModelState });
        }
    }
}
