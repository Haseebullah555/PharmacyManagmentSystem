using Application.Contracts.Interfaces;
using Domain.Models;
using Persistence.Database;
using Persistence.Repositories.Common;

namespace Persistence.Repositories
{
    public class InventoryTransactionRepository(AppDbContext context) : GenericRepository<InventoryTransaction>(context), IInventoryTransactionRepository
    {
    }
}