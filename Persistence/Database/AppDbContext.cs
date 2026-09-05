using Domain.Models;
using Domain.Models.UserManagement;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // StaffSeeder.Seed(modelBuilder);

            modelBuilder.Entity<InventoryBatch>()
                .HasIndex(batch => new { batch.MedicineID, batch.BatchNumber })
                .IsUnique();

            // modelBuilder.Entity<InventoryBatch>()
            //     .Property(batch => batch.UnitCost)
            //     .HasPrecision(18, 2);
            // modelBuilder.Entity<InventoryBatch>()
            //     .Property(batch => batch.SalePrice)
            //     .HasPrecision(18, 2);
            // modelBuilder.Entity<Purchase>()
            //     .Property(purchase => purchase.UnitPrice)
            //     .HasPrecision(18, 2);
            // modelBuilder.Entity<Purchase>()
            //     .Property(purchase => purchase.SalePrice)
            //     .HasPrecision(18, 2);
            // modelBuilder.Entity<Sale>()
            //     .Property(sale => sale.UnitPrice)
            //     .HasPrecision(18, 2);

            modelBuilder.Entity<Medicine>()
                .HasOne(medicine => medicine.Dosage)
                .WithMany()
                .HasForeignKey(medicine => medicine.DosageId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Medicine>()
                .HasOne(medicine => medicine.Category)
                .WithMany()
                .HasForeignKey(medicine => medicine.CategoryID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Medicine>()
                .HasOne(medicine => medicine.Company)
                .WithMany()
                .HasForeignKey(medicine => medicine.CompanyID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<InventoryBatch>()
                .HasOne(batch => batch.Medicine)
                .WithMany(medicine => medicine.InventoryBatches)
                .HasForeignKey(batch => batch.MedicineID)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<InventoryAdjustment>()
                .HasOne(adjustment => adjustment.Medicine)
                .WithMany(medicine => medicine.InventoryAdjustments)
                .HasForeignKey(adjustment => adjustment.MedicineID)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<InventoryAdjustment>()
                .HasOne(adjustment => adjustment.InventoryBatch)
                .WithMany()
                .HasForeignKey(adjustment => adjustment.InventoryBatchID)
                .OnDelete(DeleteBehavior.Restrict);
            // modelBuilder.Entity<Purchase>()
            //     .HasOne(purchase => purchase.InventoryBatch)
            //     .WithMany()
            //     .HasForeignKey(purchase => purchase.InventoryBatchID)
            //     .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Sale>()
                .HasOne(sale => sale.InventoryBatch)
                .WithMany()
                .HasForeignKey(sale => sale.InventoryBatchID)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<SaleBatchAllocation>()
                .HasOne(allocation => allocation.Sale)
                .WithMany(sale => sale.BatchAllocations)
                .HasForeignKey(allocation => allocation.SaleID)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<SaleBatchAllocation>()
                .HasOne(allocation => allocation.InventoryBatch)
                .WithMany()
                .HasForeignKey(allocation => allocation.InventoryBatchID)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<SaleReturn>()
                .HasOne(returnItem => returnItem.SaleBatchAllocation)
                .WithMany(allocation => allocation.SaleReturns)
                .HasForeignKey(returnItem => returnItem.SaleBatchAllocationID)
                .OnDelete(DeleteBehavior.Restrict);
        }
        #region DbSets
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Dosage> Dosages { get; set; }
        public DbSet<Medicine> Medicines { get; set; }
        public DbSet<MedicineUnit> MedicineUnits { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<InventoryBatch> InventoryBatches { get; set; }
        public DbSet<InventoryAdjustment> InventoryAdjustments { get; set; }
        public DbSet<SaleBatchAllocation> SaleBatchAllocations { get; set; }
        public DbSet<SaleReturn> SaleReturns { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Location> Locations { get; set; }


        #endregion
    }
}
