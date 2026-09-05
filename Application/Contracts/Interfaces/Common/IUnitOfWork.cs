using Application.Contracts.UserManagement;
using Microsoft.EntityFrameworkCore.Storage;

namespace Application.Contracts.Interfaces.Common
{
    public interface IUnitOfWork : IDisposable
    {
        #region User Management
        public IUserRepository Users { get; }
        public IRoleRepository Roles { get; }
        #endregion

        #region Main Entities
        public ICategoryRepository Categories { get; }
        public ICompanyRepository Companies { get; }
        public IDosageRepository Dosages { get; }
        public ISupplierRepository Suppliers { get; }
        public IMedicineRepository Medicines { get; }
        public IMedicineUnitRepository MedicineUnits { get; }
        public IInventoryStockRepository InventoryStocks { get; }
        public IInventoryTransactionRepository InventoryTransactions { get; }
        public IInventoryBatchRepository InventoryBatches { get; }
        public ILocationRepository Locations { get; }
        public IExpenseRepository Expenses { get; }
        public IPurchaseRepository Purchases { get; }
        public IPurchaseItemRepository PurchaseItems { get; }
        public ISaleRepository Sales { get; }
        public ISaleItemRepository SaleItems { get; }
        #endregion

        Task SaveAsync(CancellationToken cancellationToken);
        Task<IDbContextTransaction> BeginTransactionAsync();
    }
}
