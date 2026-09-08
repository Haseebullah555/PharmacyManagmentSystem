using Application.Contracts.Interfaces.seeders;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Persistence.Seeders
{
    public class UnitSeeder : IUnitSeeder
    {
        private readonly AppDbContext _context;

        public UnitSeeder(AppDbContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            var roles = new List<Unit>
            {
                new Unit
                {
                    Id = 1,
                    Name = "Strip",
                    ShortName = "stp"
                },
                new Unit
                {
                    Id = 2,
                    Name = "Bottle",
                    ShortName = "btl"
                },
                new Unit
                {
                    Id = 3,
                    Name = "Box",
                    ShortName = "box"
                },
                new Unit
                {
                    Id = 4,
                    Name = "Packet",
                    ShortName = "pkt"
                },
                new Unit
                {
                    Id = 5,
                    Name = "Tube",
                    ShortName = "tub"
                },
                new Unit
                {
                    Id = 6,
                    Name = "Vial",
                    ShortName = "vial"
                },
                new Unit
                {
                    Id = 7,
                    Name = "Ampoule",
                    ShortName = "amp"
                },

            };

            foreach (var role in roles)
            {
                if (!await _context.Units.AnyAsync(r => r.Name == role.Name))
                {
                    _context.Units.Add(role);
                }
            }

            await _context.SaveChangesAsync();
        }
    }

}