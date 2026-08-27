using Application.Contracts.Interfaces;
using Domain.Models;
using Persistence.Database;
using Persistence.Repositories.Common;

namespace Persistence.Repositories
{
    public class PurchaseRepository(AppDbContext context) : GenericRepository<Purchase>(context), IPurchaseRepository
    {
    }
}
