using Application.Contracts.Interfaces;
using Domain.Models;
using Persistence.Database;
using Persistence.Repositories.Common;

namespace Persistence.Repositories
{
    public class LocationRepository(AppDbContext context) : GenericRepository<Location>(context), ILocationRepository
    {
    }
}