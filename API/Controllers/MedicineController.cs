using Application.Dtos.Medicine;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace API.Controllers
{
    [ApiController]
    [Route("api/medicines")]
    public class MedicineController(AppDbContext context) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<MedicineDto>>> GetAll(CancellationToken cancellationToken)
        {
            return Ok(await context.Medicines.AsNoTracking().Select(medicine => new MedicineDto
            {
                Id = medicine.Id,
                GenericName = medicine.GenericName,
                TradeName = medicine.TradeName,
                Capacity = medicine.Capacity,
                UnitOfMeasure = medicine.UnitOfMeasure,
                Barcode = medicine.Barcode,
                ReorderLevel = medicine.ReorderLevel,
                IsActive = medicine.IsActive,
                RequiresPrescription = medicine.RequiresPrescription,
                CategoryID = medicine.CategoryID,
                CompanyID = medicine.CompanyID
            }).ToListAsync(cancellationToken));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<MedicineDto>> GetById(int id, CancellationToken cancellationToken)
        {
            var medicine = await context.Medicines.AsNoTracking()
                .Where(item => item.Id == id)
                .Select(item => new MedicineDto
                {
                    Id = item.Id, GenericName = item.GenericName, TradeName = item.TradeName,
                    Capacity = item.Capacity, UnitOfMeasure = item.UnitOfMeasure, Barcode = item.Barcode,
                    ReorderLevel = item.ReorderLevel, IsActive = item.IsActive,
                    RequiresPrescription = item.RequiresPrescription, CategoryID = item.CategoryID,
                    CompanyID = item.CompanyID
                }).FirstOrDefaultAsync(cancellationToken);
            return medicine is null ? NotFound() : Ok(medicine);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create(AddMedicineDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);
            if (await context.Medicines.AnyAsync(item => item.Barcode == dto.Barcode && dto.Barcode != null, cancellationToken))
                return Conflict("A medicine with this barcode already exists.");

            var medicine = new Medicine
            {
                GenericName = dto.GenericName, TradeName = dto.TradeName, Capacity = dto.Capacity,
                UnitOfMeasure = dto.UnitOfMeasure, Barcode = dto.Barcode, ReorderLevel = dto.ReorderLevel,
                IsActive = dto.IsActive, RequiresPrescription = dto.RequiresPrescription,
                CategoryID = dto.CategoryID, CompanyID = dto.CompanyID, CreatedAt = DateTime.UtcNow
            };
            context.Medicines.Add(medicine);
            await context.SaveChangesAsync(cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = medicine.Id }, medicine.Id);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, UpdateMedicineDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);
            var medicine = await context.Medicines.FindAsync(new object[] { id }, cancellationToken);
            if (medicine is null)
                return NotFound();
            medicine.GenericName = dto.GenericName; medicine.TradeName = dto.TradeName;
            medicine.Capacity = dto.Capacity; medicine.UnitOfMeasure = dto.UnitOfMeasure;
            medicine.Barcode = dto.Barcode; medicine.ReorderLevel = dto.ReorderLevel;
            medicine.IsActive = dto.IsActive; medicine.RequiresPrescription = dto.RequiresPrescription;
            medicine.CategoryID = dto.CategoryID; medicine.CompanyID = dto.CompanyID;
            medicine.UpdatedAt = DateTime.UtcNow;
            await context.SaveChangesAsync(cancellationToken);
            return NoContent();
        }
    }
}