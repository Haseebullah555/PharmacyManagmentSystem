using Application.Contracts.Interfaces.Common;
using Application.Contracts.UserManagement;
using Microsoft.EntityFrameworkCore.Storage;
using Persistence.Database;
using Persistence.Repositories.UserManagement;

namespace Persistence.Repositories.Common
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }
        #region Private fields
        private IRoleRepository _roleRepository;
        private IUserRepository _userRepository;
        #endregion
        
        #region User Management
        public IUserRepository Users => _userRepository ??= new UserRepository(_context);
        public IRoleRepository Roles => _roleRepository ??= new RoleRepository(_context);
        
        #endregion


        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
        public async Task SaveAsync(CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }


    }
}