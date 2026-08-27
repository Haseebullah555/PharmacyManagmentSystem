using Application.Contracts.UserManagement;
using Microsoft.EntityFrameworkCore.Storage;

namespace Application.Contracts.Interfaces.Common
{
    public interface IUnitOfWork : IDisposable
    {
        #region User Management
        public IUserRepository Users {get;}
        public IRoleRepository Roles {get;}
        #endregion

        Task SaveAsync(CancellationToken cancellationToken);
        Task<IDbContextTransaction> BeginTransactionAsync();
    }
}