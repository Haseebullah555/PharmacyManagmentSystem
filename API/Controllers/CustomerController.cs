using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace API.Controllers
{
    [ApiController]
    [Route("api/customers")]
    public class CustomerController(AppDbContext context) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<object>>> GetAll([FromQuery] string? search = null, CancellationToken cancellationToken = default)
        {
            var query = context.Customers.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(item => item.CustomerName.Contains(search) || item.PhoneNo.Contains(search));

            var result = await query
                .OrderBy(item => item.CustomerName)
                .Select(item => new { id = item.Id, customerName = item.CustomerName, phoneNo = item.PhoneNo, address = item.Address })
                .ToListAsync(cancellationToken);

            return Ok(result);
        }

        [HttpGet("select-list")]
        public async Task<ActionResult<List<object>>> GetSelectList([FromQuery] string? search = null, CancellationToken cancellationToken = default)
        {
            var query = context.Customers.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(item => item.CustomerName.Contains(search) || item.PhoneNo.Contains(search));

            var result = await query
                .OrderBy(item => item.CustomerName)
                .Select(item => new { id = item.Id, text = item.CustomerName })
                .ToListAsync(cancellationToken);

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create(Customer dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            dto.CreatedAt = DateTime.UtcNow;
            context.Customers.Add(dto);
            await context.SaveChangesAsync(cancellationToken);
            return Ok(dto.Id);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, Customer dto, CancellationToken cancellationToken)
        {
            var customer = await context.Customers.FirstOrDefaultAsync(item => item.Id == id, cancellationToken);
            if (customer is null)
                return NotFound();

            customer.CustomerName = dto.CustomerName;
            customer.PhoneNo = dto.PhoneNo;
            customer.Address = dto.Address;
            customer.UpdatedAt = DateTime.UtcNow;
            await context.SaveChangesAsync(cancellationToken);
            return NoContent();
        }
    }
}
