using Application.Contracts.Interfaces;
using Domain.Models;
using Persistence.Database;
using Persistence.Repositories.Common;

namespace Persistence.Repositories
{
    public class SaleRepository(AppDbContext context) : GenericRepository<Sale>(context), ISaleRepository
    {
    }
}
