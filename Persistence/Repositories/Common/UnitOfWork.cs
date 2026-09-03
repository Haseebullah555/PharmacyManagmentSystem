using Application.Contracts.Interfaces;
using Application.Contracts.Interfaces.Common;
using Application.Contracts.UserManagement;
using Persistence.Database;
using Persistence.Repositories.UserManagement;
using Microsoft.EntityFrameworkCore.Storage;

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
        private IExpenseRepository _expenseRepository;
        private ICategoryRepository _categoryRepository;
        private ICompanyRepository _companyRepository;
        private IDosageRepository _dosageRepository;
        private ISupplierRepository _supplierRepository;
        private IMedicineRepository _medicineRepository;
        private IMedicineUnitRepository _medicineUnitRepository;
        private ILocationRepository _locationRepository;
        private IInventoryStockRepository _inventoryStockRepository;
        private IInventoryTransactionRepository _inventoryTransactionRepository;
        private IPurchaseRepository _purchaseRepository;
        private IPurchaseItemRepository _purchaseItemRepository;
        private ISaleRepository _saleRepository;
        private IInventoryBatchRepository _inventoryBatchRepository;
        #endregion

        #region User Management
        public IUserRepository Users => _userRepository ??= new UserRepository(_context);
        public IRoleRepository Roles => _roleRepository ??= new RoleRepository(_context);

        #endregion

        #region Main Entities
        public IExpenseRepository Expenses => _expenseRepository ??= new ExpenseRepository(_context);
        public ICategoryRepository Categories => _categoryRepository ??= new CategoryRepository(_context);
        public ICompanyRepository Companies => _companyRepository ??= new CompanyRepository(_context);
        public IDosageRepository Dosages => _dosageRepository ??= new DosageRepository(_context);
        public ISupplierRepository Suppliers => _supplierRepository ??= new SupplierRepository(_context);
        public IMedicineRepository Medicines => _medicineRepository ??= new MedicineRepository(_context);
        public IMedicineUnitRepository MedicineUnits => _medicineUnitRepository ??= new MedicineUnitRepository(_context);
        public IInventoryStockRepository InventoryStocks => _inventoryStockRepository ??= new InventoryStockRepository(_context);
        public IInventoryTransactionRepository InventoryTransactions => _inventoryTransactionRepository ??= new InventoryTransactionRepository(_context);
        public ILocationRepository Locations => _locationRepository ??= new LocationRepository(_context);
        public IPurchaseRepository Purchases => _purchaseRepository ??= new PurchaseRepository(_context);
        public IPurchaseItemRepository PurchaseItems => _purchaseItemRepository ??= new PurchaseItemRepository(_context);
        public ISaleRepository Sales => _saleRepository ??= new SaleRepository(_context);
        public IInventoryBatchRepository InventoryBatches => _inventoryBatchRepository ??= new InventoryBatchRepository(_context);
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
