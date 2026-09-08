using Application.Contracts.Interfaces.seeders;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Persistence.Seeders
{
    public class CategorySeeder : ICategorySeeder
    {
        private readonly AppDbContext _context;

        public CategorySeeder(AppDbContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            var roles = new List<Category>
            {
                new Category
                {
                    Id = 1,
                    CategoryName = "Syrup"
                },
                new Category
                {
                    Id = 2,
                    CategoryName = "Capsule"
                },
                new Category
                {
                    Id = 3,
                    CategoryName = "Tablet"
                },
                new Category
                {
                    Id = 4,
                    CategoryName = "Injection"
                },
                new Category
                {
                    Id = 5,
                    CategoryName = "Ointment"
                },
                new Category
                {
                    Id = 6,
                    CategoryName = "Cream"
                },
                new Category
                {
                    Id = 7,
                    CategoryName = "Powder"
                },
                new Category
                {
                    Id = 8,
                    CategoryName = "Drops"
                },
                new Category
                {
                    Id = 9,
                    CategoryName = "Lotion"
                },
                

            };

            foreach (var role in roles)
            {
                if (!await _context.Categories.AnyAsync(r => r.CategoryName == role.CategoryName))
                {
                    _context.Categories.Add(role);
                }
            }

            await _context.SaveChangesAsync();
        }
    }

}