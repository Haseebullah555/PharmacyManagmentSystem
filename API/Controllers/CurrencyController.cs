using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace API.Controllers
{
    [ApiController]
    [Route("api/currencies")]
    public class CurrencyController(AppDbContext context) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<object>>> GetAll([FromQuery] string? search = null, CancellationToken cancellationToken = default)
        {
            var query = context.Currencies.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(item => item.CurrencyName.Contains(search));

            var result = await query
                .OrderBy(item => item.CurrencyName)
                .Select(item => new { id = item.Id, currencyName = item.CurrencyName })
                .ToListAsync(cancellationToken);

            return Ok(result);
        }

        [HttpGet("select-list")]
        public async Task<ActionResult<List<object>>> GetSelectList([FromQuery] string? search = null, CancellationToken cancellationToken = default)
        {
            var query = context.Currencies.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(item => item.CurrencyName.Contains(search));

            var result = await query
                .OrderBy(item => item.CurrencyName)
                .Select(item => new { id = item.Id, text = item.CurrencyName })
                .ToListAsync(cancellationToken);

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create(Currency dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            dto.CreatedAt = DateTime.UtcNow;
            context.Currencies.Add(dto);
            await context.SaveChangesAsync(cancellationToken);
            return Ok(dto.Id);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, Currency dto, CancellationToken cancellationToken)
        {
            var currency = await context.Currencies.FirstOrDefaultAsync(item => item.Id == id, cancellationToken);
            if (currency is null)
                return NotFound();

            currency.CurrencyName = dto.CurrencyName;
            currency.UpdatedAt = DateTime.UtcNow;
            await context.SaveChangesAsync(cancellationToken);
            return NoContent();
        }
    }
}
