using Application.Contracts.Interfaces;
using Domain.Models;
using Persistence.Database;
using Persistence.Repositories.Common;

namespace Persistence.Repositories
{
    public class SaleItemRepository(AppDbContext context) : GenericRepository<SaleItem>(context), ISaleItemRepository
    {
    }
}