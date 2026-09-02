using Application.Contracts.Interfaces;
using Domain.Models;
using Persistence.Database;
using Persistence.Repositories.Common;

namespace Persistence.Repositories
{
    public class ExpenseRepository(AppDbContext _context) : GenericRepository<Expense>(_context), IExpenseRepository
    {
    }
}