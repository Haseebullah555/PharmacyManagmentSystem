using Application.Contracts.Interfaces;
using Domain.Models;
using Persistence.Database;
using Persistence.Repositories.Common;

namespace Persistence.Repositories
{
    public class InventoryBatchRepository(AppDbContext context)
        : GenericRepository<InventoryBatch>(context), IInventoryBatchRepository
    {
    }
}