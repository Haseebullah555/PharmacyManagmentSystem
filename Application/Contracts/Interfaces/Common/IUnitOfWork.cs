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

        #region Main Entities
        public ICategoryRepository Categories {get;}
        public ICompanyRepository Companies {get;}
        public IMedicineRepository Medicines {get;}
        public IPurchaseRepository Purchases {get;}
        public ISaleRepository Sales {get;}
        public IInventoryBatchRepository InventoryBatches { get; }
        #endregion

        Task SaveAsync(CancellationToken cancellationToken);
        Task<IDbContextTransaction> BeginTransactionAsync();
    }
}
