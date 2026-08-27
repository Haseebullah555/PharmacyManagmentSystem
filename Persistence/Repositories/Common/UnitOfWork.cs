using Application.Contracts.Interfaces;
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
        private ICategoryRepository _categoryRepository;
        private ICompanyRepository _companyRepository;
        private IMedicineRepository _medicineRepository;
        private IPurchaseRepository _purchaseRepository;
        private ISaleRepository _saleRepository;
        private IInventoryBatchRepository _inventoryBatchRepository;
        #endregion
        
        #region User Management
        public IUserRepository Users => _userRepository ??= new UserRepository(_context);
        public IRoleRepository Roles => _roleRepository ??= new RoleRepository(_context);
        
        #endregion

        #region Main Entities
        public ICategoryRepository Categories => _categoryRepository ??= new CategoryRepository(_context);
        public ICompanyRepository Companies => _companyRepository ??= new CompanyRepository(_context);
        public IMedicineRepository Medicines => _medicineRepository ??= new MedicineRepository(_context);
        public IPurchaseRepository Purchases => _purchaseRepository ??= new PurchaseRepository(_context);
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
