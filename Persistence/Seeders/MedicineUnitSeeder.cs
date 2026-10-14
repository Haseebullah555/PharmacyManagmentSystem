using Application.Contracts.Interfaces.seeders;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Persistence.Seeders
{
    public class MedicineUnitSeeder : IMedicineUnitSeeder
    {
        private readonly AppDbContext _context;

        public MedicineUnitSeeder(AppDbContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            var medicineUnits = new List<MedicineUnit>
        {
            // --------------------------------------------------
            // Paracetamol Tablet
            // --------------------------------------------------
            new MedicineUnit
            {
                MedicineId = 1,
                UnitId = 1,             // Strip
                ConversionFactor = 10,
            },

            new MedicineUnit
            {
                MedicineId = 1,
                UnitId = 3,             // Box
                ConversionFactor = 100,
            },

            // --------------------------------------------------
            // Ibuprofen Tablet
            // --------------------------------------------------
            new MedicineUnit
            {
                MedicineId = 2,
                UnitId = 1,             // Strip
                ConversionFactor = 10,
            },

            new MedicineUnit
            {
                MedicineId = 2,
                UnitId = 3,             // Box
                ConversionFactor = 100,
            },

            // --------------------------------------------------
            // Amoxicillin Tablet
            // --------------------------------------------------
            new MedicineUnit
            {
                MedicineId = 3,
                UnitId = 1,             // Strip
                ConversionFactor = 10,
            },

            new MedicineUnit
            {
                MedicineId = 3,
                UnitId = 3,             // Box
                ConversionFactor = 100,
            },

            // --------------------------------------------------
            // Metformin Tablet
            // --------------------------------------------------
            new MedicineUnit
            {
                MedicineId = 4,
                UnitId = 1,             // Strip
                ConversionFactor = 10,
            },

            new MedicineUnit
            {
                MedicineId = 4,
                UnitId = 3,             // Box
                ConversionFactor = 100,
            },

            // --------------------------------------------------
            // Omeprazole Capsule
            // --------------------------------------------------
            new MedicineUnit
            {
                MedicineId = 5,
                UnitId = 1,             // Strip
                ConversionFactor = 10,
            },

            new MedicineUnit
            {
                MedicineId = 5,
                UnitId = 3,             // Box
                ConversionFactor = 100,
            },

            // --------------------------------------------------
            // Azithromycin Capsule
            // --------------------------------------------------
            new MedicineUnit
            {
                MedicineId = 6,
                UnitId = 1,             // Strip
                ConversionFactor = 6,
            },

            new MedicineUnit
            {
                MedicineId = 6,
                UnitId = 3,             // Box
                ConversionFactor = 60,
            },

            // --------------------------------------------------
            // Paracetamol Syrup
            // --------------------------------------------------
            new MedicineUnit
            {
                MedicineId = 7,
                UnitId = 2,             // Bottle
                ConversionFactor = 1,
            },

            // --------------------------------------------------
            // Amoxicillin Syrup
            // --------------------------------------------------
            new MedicineUnit
            {
                MedicineId = 8,
                UnitId = 2,             // Bottle
                ConversionFactor = 1,
            },

            // --------------------------------------------------
            // Diclofenac Injection
            // --------------------------------------------------
            new MedicineUnit
            {
                MedicineId = 9,
                UnitId = 7,             // Ampoule
                ConversionFactor = 1,
            },

            // --------------------------------------------------
            // Ceftriaxone Injection
            // --------------------------------------------------
            new MedicineUnit
            {
                MedicineId = 10,
                UnitId = 6,             // Vial
                ConversionFactor = 1,
            }
        };

            foreach (var medicineUnit in medicineUnits)
            {
                if (!await _context.MedicineUnits.AnyAsync(x =>
                    x.MedicineId == medicineUnit.MedicineId &&
                    x.UnitId == medicineUnit.UnitId))
                {
                    _context.MedicineUnits.Add(medicineUnit);
                }
            }

            await _context.SaveChangesAsync();
        }
    }

}