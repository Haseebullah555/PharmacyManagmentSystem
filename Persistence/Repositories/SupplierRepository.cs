using Application.Contracts.Interfaces;
using Domain.Models;
using Persistence.Database;
using Persistence.Repositories.Common;

namespace Persistence.Repositories
{
    public class SupplierRepository(AppDbContext context) : GenericRepository<Supplier>(context), ISupplierRepository
    {
    }
}