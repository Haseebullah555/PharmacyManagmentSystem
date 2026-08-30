using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace API.Controllers
{
    [ApiController]
    [Route("api/capacities")]
    public class DosageController(AppDbContext context) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<object>>> GetAll([FromQuery] string? search = null, CancellationToken cancellationToken = default)
        {
            var query = context.Dosages.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(item => item.Name.Contains(search));

            var result = await query
                .OrderBy(item => item.Name)
                .Select(item => new { id = item.Id, name = item.Name, categoryId = item.CategoryId })
                .ToListAsync(cancellationToken);

            return Ok(result);
        }

        [HttpGet("select-list")]
        public async Task<ActionResult<List<object>>> GetSelectList([FromQuery] string? search = null, CancellationToken cancellationToken = default)
        {
            var query = context.Dosages.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(item => item.Name.Contains(search));

            var result = await query
                .OrderBy(item => item.Name)
                .Select(item => new { id = item.Id, text = item.Name })
                .ToListAsync(cancellationToken);

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create(Dosage dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            dto.CreatedAt = DateTime.UtcNow;
            context.Dosages.Add(dto);
            await context.SaveChangesAsync(cancellationToken);
            return Ok(dto.Id);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, Dosage dto, CancellationToken cancellationToken)
        {
            var dosage = await context.Dosages.FirstOrDefaultAsync(item => item.Id == id, cancellationToken);
            if (dosage is null)
                return NotFound();

            dosage.Name = dto.Name;
            dosage.CategoryId = dto.CategoryId;
            dosage.UpdatedAt = DateTime.UtcNow;
            await context.SaveChangesAsync(cancellationToken);
            return NoContent();
        }
    }
}
