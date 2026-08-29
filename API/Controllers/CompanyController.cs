using Application.Dtos.Company;
using Application.Features.Company.Requests.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace API.Controllers
{
    [ApiController]
    [Route("api/companies")]
    public class CompanyController(IMediator mediator, AppDbContext context) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<CompanyDto>>> GetAll([FromQuery] string? search = null, CancellationToken cancellationToken = default)
        {
            var query = context.Companies.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(item => item.CompanyName.Contains(search));

            var result = await query
                .OrderBy(item => item.CompanyName)
                .Select(item => new CompanyDto
                {
                    Id = item.Id,
                    CompanyName = item.CompanyName
                })
                .ToListAsync(cancellationToken);

            return Ok(result);
        }

        [HttpGet("select-list")]
        public async Task<ActionResult<List<object>>> GetSelectList([FromQuery] string? search = null, CancellationToken cancellationToken = default)
        {
            var query = context.Companies.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(item => item.CompanyName.Contains(search));

            var result = await query
                .OrderBy(item => item.CompanyName)
                .Select(item => new { id = item.Id, text = item.CompanyName })
                .ToListAsync(cancellationToken);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AddCompanyDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            await mediator.Send(new AddCompanyCommand { AddCompanyDto = dto }, cancellationToken);
            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] AddCompanyDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var company = await context.Companies.FirstOrDefaultAsync(item => item.Id == id, cancellationToken);
            if (company is null)
                return NotFound();

            company.CompanyName = dto.CompanyName;
            company.UpdatedAt = DateTime.UtcNow;
            await context.SaveChangesAsync(cancellationToken);
            return NoContent();
        }
    }
}
