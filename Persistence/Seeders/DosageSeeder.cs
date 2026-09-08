using Application.Contracts.Interfaces.seeders;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Persistence.Seeders
{
    public class DosageSeeder : IDosageSeeder
    {
        private readonly AppDbContext _context;

        public DosageSeeder(AppDbContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            var roles = new List<Dosage>
            {
                new Dosage
                {
                    Id = 1,
                    DosageName = "250mg",
                    CategoryId = 3,

                },
                new Dosage
                {
                    Id = 2,
                    DosageName = "500mg",
                    CategoryId = 3,

                },
                new Dosage
                {
                    Id = 3,
                    DosageName = "750mg",
                    CategoryId = 3,

                },
                new Dosage
                {
                    Id = 4,
                    DosageName = "1000mg",
                    CategoryId = 3,

                },
                new Dosage
                {
                    Id = 5,
                    DosageName = "125ml",
                    CategoryId = 1,

                },
                new Dosage
                {
                    Id = 6,
                    DosageName = "250ml",
                    CategoryId = 1,

                },
                new Dosage
                {
                    Id = 7,
                    DosageName = "500mg",
                    CategoryId = 2,

                },

            };

            foreach (var role in roles)
            {
                if (!await _context.Dosages.AnyAsync(r => r.DosageName == role.DosageName))
                {
                    _context.Dosages.Add(role);
                }
            }

            await _context.SaveChangesAsync();
        }
    }

}