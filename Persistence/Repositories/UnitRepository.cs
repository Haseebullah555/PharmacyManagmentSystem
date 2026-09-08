using Application.Contracts.Interfaces;
using Domain.Models;
using Persistence.Database;
using Persistence.Repositories.Common;

namespace Persistence.Repositories
{
    public class UnitRepository(AppDbContext context) : GenericRepository<Unit>(context), IUnitRepository
    {
    }
}