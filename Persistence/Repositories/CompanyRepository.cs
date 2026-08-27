using Application.Contracts.Interfaces;
using Domain.Models;
using Persistence.Database;
using Persistence.Repositories.Common;

namespace Persistence.Repositories
{
    public class CompanyRepository(AppDbContext context) : GenericRepository<Company>(context), ICompanyRepository
    {
    }
}
