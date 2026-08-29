using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace API.Controllers
{
    [ApiController]
    [Route("api/suppliers")]
    public class SupplierController(AppDbContext context) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<object>>> GetAll([FromQuery] string? search = null, CancellationToken cancellationToken = default)
        {
            var query = context.Suppliers.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(item => item.SupplierName.Contains(search) || item.ContactNo.Contains(search));

            var result = await query
                .OrderBy(item => item.SupplierName)
                .Select(item => new { id = item.Id, supplierName = item.SupplierName, contactNo = item.ContactNo, address = item.Address, description = item.Description })
                .ToListAsync(cancellationToken);

            return Ok(result);
        }

        [HttpGet("select-list")]
        public async Task<ActionResult<List<object>>> GetSelectList([FromQuery] string? search = null, CancellationToken cancellationToken = default)
        {
            var query = context.Suppliers.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(item => item.SupplierName.Contains(search) || item.ContactNo.Contains(search));

            var result = await query
                .OrderBy(item => item.SupplierName)
                .Select(item => new { id = item.Id, text = item.SupplierName })
                .ToListAsync(cancellationToken);

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create(Supplier dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            dto.CreatedAt = DateTime.UtcNow;
            context.Suppliers.Add(dto);
            await context.SaveChangesAsync(cancellationToken);
            return Ok(dto.Id);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, Supplier dto, CancellationToken cancellationToken)
        {
            var supplier = await context.Suppliers.FirstOrDefaultAsync(item => item.Id == id, cancellationToken);
            if (supplier is null)
                return NotFound();

            supplier.SupplierName = dto.SupplierName;
            supplier.Address = dto.Address;
            supplier.ContactNo = dto.ContactNo;
            supplier.Description = dto.Description;
            supplier.UpdatedAt = DateTime.UtcNow;
            await context.SaveChangesAsync(cancellationToken);
            return NoContent();
        }
    }
}
