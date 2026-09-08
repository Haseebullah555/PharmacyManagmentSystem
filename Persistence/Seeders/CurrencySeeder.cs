using Application.Contracts.Interfaces.seeders;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Persistence.Seeders
{
    public class CurrencySeeder : ICurrencySeeder
    {
        private readonly AppDbContext _context;

        public CurrencySeeder(AppDbContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            var roles = new List<Currency>
            {
                new Currency
                {
                    Id = 1,
                    CurrencyName = "AFG",

                },
                new Currency
                {
                    Id = 2,
                    CurrencyName = "USD",

                },
                new Currency
                {
                    Id = 3,
                    CurrencyName = "PKR",

                }

            };

            foreach (var role in roles)
            {
                if (!await _context.Currencies.AnyAsync(r => r.CurrencyName == role.CurrencyName))
                {
                    _context.Currencies.Add(role);
                }
            }

            await _context.SaveChangesAsync();
        }
    }

}