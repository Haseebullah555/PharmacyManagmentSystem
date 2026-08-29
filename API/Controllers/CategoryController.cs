using Application.Dtos.Category;
using Application.Features.Category.Requests.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace API.Controllers
{
    [ApiController]
    [Route("api/categories")]
    public class CategoryController(IMediator mediator, AppDbContext context) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<CategoryDto>>> GetAll([FromQuery] string? search = null, CancellationToken cancellationToken = default)
        {
            var query = context.Categories.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(item => item.CategoryName.Contains(search));

            var result = await query
                .OrderBy(item => item.CategoryName)
                .Select(item => new CategoryDto
                {
                    Id = item.Id,
                    CategoryName = item.CategoryName
                })
                .ToListAsync(cancellationToken);

            return Ok(result);
        }

        [HttpGet("select-list")]
        public async Task<ActionResult<List<object>>> GetSelectList([FromQuery] string? search = null, CancellationToken cancellationToken = default)
        {
            var query = context.Categories.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(item => item.CategoryName.Contains(search));

            var result = await query
                .OrderBy(item => item.CategoryName)
                .Select(item => new { id = item.Id, text = item.CategoryName })
                .ToListAsync(cancellationToken);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AddCategoryDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            await mediator.Send(new AddCategoryCommand { AddCategoryDto = dto }, cancellationToken);
            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] AddCategoryDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var category = await context.Categories.FirstOrDefaultAsync(item => item.Id == id, cancellationToken);
            if (category is null)
                return NotFound();

            category.CategoryName = dto.CategoryName;
            category.UpdatedAt = DateTime.UtcNow;
            await context.SaveChangesAsync(cancellationToken);
            return NoContent();
        }
    }
}
