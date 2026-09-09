using Application.Contracts.Interfaces.seeders;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Persistence.Seeders
{
    public class MedicineSeeder : IMedicineSeeder
    {
        private readonly AppDbContext _context;

        public MedicineSeeder(AppDbContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            var medicines = new List<Medicine>
        {
            // Tablets
            new Medicine
            {
                GenericName = "Paracetamol",
                TradeName = "Paracetamol",
                DosageId = 2,       // 500mg
                CategoryID = 3,     // Tablet
                CompanyID = 1,
                IsActive = true,
                RequiresPrescription = false
            },

            new Medicine
            {
                GenericName = "Ibuprofen",
                TradeName = "Ibuprofen",
                DosageId = 2,       // 500mg
                CategoryID = 3,     // Tablet
                CompanyID = 1,
                IsActive = true,
                RequiresPrescription = false
            },

            new Medicine
            {
                GenericName = "Amoxicillin",
                TradeName = "Amoxicillin",
                DosageId = 2,       // 500mg
                CategoryID = 3,     // Tablet
                CompanyID = 1,
                IsActive = true,
                RequiresPrescription = true
            },

            new Medicine
            {
                GenericName = "Metformin",
                TradeName = "Metformin",
                DosageId = 2,       // 500mg
                CategoryID = 3,     // Tablet
                CompanyID = 1,
                IsActive = true,
                RequiresPrescription = true
            },

            // Capsules
            new Medicine
            {
                GenericName = "Omeprazole",
                TradeName = "Omeprazole",
                DosageId = 2,       // 500mg - change if you have 20mg dosage
                CategoryID = 2,     // Capsule
                CompanyID = 1,
                IsActive = true,
                RequiresPrescription = false
            },

            new Medicine
            {
                GenericName = "Azithromycin",
                TradeName = "Azithromycin",
                DosageId = 2,
                CategoryID = 2,     // Capsule
                CompanyID = 1,
                IsActive = true,
                RequiresPrescription = true
            },

            // Syrups
            new Medicine
            {
                GenericName = "Paracetamol",
                TradeName = "Paracetamol Syrup",
                DosageId = 6,       // 250ml
                CategoryID = 1,     // Syrup
                CompanyID = 1,
                IsActive = true,
                RequiresPrescription = false
            },

            new Medicine
            {
                GenericName = "Amoxicillin",
                TradeName = "Amoxicillin Syrup",
                DosageId = 5,       // 125ml
                CategoryID = 1,     // Syrup
                CompanyID = 1,
                IsActive = true,
                RequiresPrescription = true
            },

            // Injections
            new Medicine
            {
                GenericName = "Diclofenac",
                TradeName = "Diclofenac Injection",
                DosageId = 1,       // 250mg - change if you have correct dosage
                CategoryID = 4,     // Injection
                CompanyID = 1,
                IsActive = true,
                RequiresPrescription = true
            },

            new Medicine
            {
                GenericName = "Ceftriaxone",
                TradeName = "Ceftriaxone Injection",
                DosageId = 4,       // 1000mg
                CategoryID = 4,     // Injection
                CompanyID = 1,
                IsActive = true,
                RequiresPrescription = true
            }
        };

            foreach (var medicine in medicines)
            {
                if (!await _context.Medicines.AnyAsync(x =>
                    x.GenericName == medicine.GenericName &&
                    x.TradeName == medicine.TradeName &&
                    x.DosageId == medicine.DosageId &&
                    x.CategoryID == medicine.CategoryID &&
                    x.CompanyID == medicine.CompanyID))
                {
                    _context.Medicines.Add(medicine);
                }
            }

            await _context.SaveChangesAsync();
        }
    }

}