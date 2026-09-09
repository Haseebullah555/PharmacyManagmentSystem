using Application.Contracts.Interfaces.seeders;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Persistence.Seeders
{
public class CompanySeeder(AppDbContext context) : ICompanySeeder
{
    private readonly AppDbContext _context = context;

        public async Task SeedAsync()
    {
        var companies = new List<Company>
        {
            new Company
            {
                Id = 1,
                CompanyName = "Acme Pharmaceuticals",
            },
            new Company
            {
                Id = 2,
                CompanyName = "Global Pharma",
            },
            new Company
            {
                Id = 3,
                CompanyName = "Afghan Pharma",
            },
            new Company
            {
                Id = 4,
                CompanyName = "Medica Pharmaceuticals",
            },
            new Company
            {
                Id = 5,
                CompanyName = "HealthCare Pharma",
            }
        };

        foreach (var company in companies)
        {
            if (!await _context.Companies.AnyAsync(x => x.Id == company.Id))
            {
                _context.Companies.Add(company);
            }
        }

        await _context.SaveChangesAsync();
    }
}

}