using Application.Contracts.Interfaces;
using Domain.Models;
using Persistence.Database;
using Persistence.Repositories.Common;

namespace Persistence.Repositories
{
    public class CurrencyRepository(AppDbContext context) : GenericRepository<Currency>(context), ICurrencyRepository
    {
    }
}