using Application.Contracts.Interfaces;
using Domain.Models;
using Persistence.Database;
using Persistence.Repositories.Common;

namespace Persistence.Repositories
{
    public class MedicineUnitRepository(AppDbContext context) : GenericRepository<MedicineUnit>(context), IMedicineUnitRepository
    {
    }
}