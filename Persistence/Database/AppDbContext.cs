using Domain.Models;
using Domain.Models.UserManagement;
using Microsoft.EntityFrameworkCore;
using Persistence.Seeders;

namespace Persistence.Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var seedDate = DateTime.SpecifyKind(new DateTime(2026, 1, 1), DateTimeKind.Utc);

            modelBuilder.Entity<Currency>().HasData(
                new { Id = 1, CurrencyName = "AFG", CreatedBy = Guid.Empty, UpdateBy = Guid.Empty, CreatedAt = seedDate, UpdatedAt = seedDate },
                new { Id = 2, CurrencyName = "USD", CreatedBy = Guid.Empty, UpdateBy = Guid.Empty, CreatedAt = seedDate, UpdatedAt = seedDate },
                new { Id = 3, CurrencyName = "PKR", CreatedBy = Guid.Empty, UpdateBy = Guid.Empty, CreatedAt = seedDate, UpdatedAt = seedDate });

            modelBuilder.Entity<Company>().HasData(
                new { Id = 1, CompanyName = "Acme Pharmaceuticals", CreatedBy = Guid.Empty, UpdateBy = Guid.Empty, CreatedAt = seedDate, UpdatedAt = seedDate },
                new { Id = 2, CompanyName = "Global Pharma", CreatedBy = Guid.Empty, UpdateBy = Guid.Empty, CreatedAt = seedDate, UpdatedAt = seedDate },
                new { Id = 3, CompanyName = "Afghan Pharma", CreatedBy = Guid.Empty, UpdateBy = Guid.Empty, CreatedAt = seedDate, UpdatedAt = seedDate },
                new { Id = 4, CompanyName = "Medica Pharmaceuticals", CreatedBy = Guid.Empty, UpdateBy = Guid.Empty, CreatedAt = seedDate, UpdatedAt = seedDate },
                new { Id = 5, CompanyName = "HealthCare Pharma", CreatedBy = Guid.Empty, UpdateBy = Guid.Empty, CreatedAt = seedDate, UpdatedAt = seedDate });

            modelBuilder.Entity<Category>().HasData(
                new { Id = 1, CategoryName = "Syrup", CreatedBy = Guid.Empty, UpdateBy = Guid.Empty, CreatedAt = seedDate, UpdatedAt = seedDate },
                new { Id = 2, CategoryName = "Capsule", CreatedBy = Guid.Empty, UpdateBy = Guid.Empty, CreatedAt = seedDate, UpdatedAt = seedDate },
                new { Id = 3, CategoryName = "Tablet", CreatedBy = Guid.Empty, UpdateBy = Guid.Empty, CreatedAt = seedDate, UpdatedAt = seedDate },
                new { Id = 4, CategoryName = "Injection", CreatedBy = Guid.Empty, UpdateBy = Guid.Empty, CreatedAt = seedDate, UpdatedAt = seedDate },
                new { Id = 5, CategoryName = "Ointment", CreatedBy = Guid.Empty, UpdateBy = Guid.Empty, CreatedAt = seedDate, UpdatedAt = seedDate },
                new { Id = 6, CategoryName = "Cream", CreatedBy = Guid.Empty, UpdateBy = Guid.Empty, CreatedAt = seedDate, UpdatedAt = seedDate },
                new { Id = 7, CategoryName = "Powder", CreatedBy = Guid.Empty, UpdateBy = Guid.Empty, CreatedAt = seedDate, UpdatedAt = seedDate },
                new { Id = 8, CategoryName = "Drops", CreatedBy = Guid.Empty, UpdateBy = Guid.Empty, CreatedAt = seedDate, UpdatedAt = seedDate },
                new { Id = 9, CategoryName = "Lotion", CreatedBy = Guid.Empty, UpdateBy = Guid.Empty, CreatedAt = seedDate, UpdatedAt = seedDate });

            modelBuilder.Entity<Dosage>().HasData(
                new { Id = 1, DosageName = "250mg", CategoryId = 3, CreatedBy = Guid.Empty, UpdateBy = Guid.Empty, CreatedAt = seedDate, UpdatedAt = seedDate },
                new { Id = 2, DosageName = "500mg", CategoryId = 3, CreatedBy = Guid.Empty, UpdateBy = Guid.Empty, CreatedAt = seedDate, UpdatedAt = seedDate },
                new { Id = 3, DosageName = "750mg", CategoryId = 3, CreatedBy = Guid.Empty, UpdateBy = Guid.Empty, CreatedAt = seedDate, UpdatedAt = seedDate },
                new { Id = 4, DosageName = "1000mg", CategoryId = 3, CreatedBy = Guid.Empty, UpdateBy = Guid.Empty, CreatedAt = seedDate, UpdatedAt = seedDate },
                new { Id = 5, DosageName = "125ml", CategoryId = 1, CreatedBy = Guid.Empty, UpdateBy = Guid.Empty, CreatedAt = seedDate, UpdatedAt = seedDate },
                new { Id = 6, DosageName = "250ml", CategoryId = 1, CreatedBy = Guid.Empty, UpdateBy = Guid.Empty, CreatedAt = seedDate, UpdatedAt = seedDate },
                new { Id = 7, DosageName = "500mg", CategoryId = 2, CreatedBy = Guid.Empty, UpdateBy = Guid.Empty, CreatedAt = seedDate, UpdatedAt = seedDate });

            modelBuilder.Entity<Unit>().HasData(
                new { Id = 1, Name = "Strip", ShortName = "stp", IsActive = true, CreatedBy = Guid.Empty, UpdateBy = Guid.Empty, CreatedAt = seedDate, UpdatedAt = seedDate },
                new { Id = 2, Name = "Bottle", ShortName = "btl", IsActive = true, CreatedBy = Guid.Empty, UpdateBy = Guid.Empty, CreatedAt = seedDate, UpdatedAt = seedDate },
                new { Id = 3, Name = "Box", ShortName = "box", IsActive = true, CreatedBy = Guid.Empty, UpdateBy = Guid.Empty, CreatedAt = seedDate, UpdatedAt = seedDate },
                new { Id = 4, Name = "Packet", ShortName = "pkt", IsActive = true, CreatedBy = Guid.Empty, UpdateBy = Guid.Empty, CreatedAt = seedDate, UpdatedAt = seedDate },
                new { Id = 5, Name = "Tube", ShortName = "tub", IsActive = true, CreatedBy = Guid.Empty, UpdateBy = Guid.Empty, CreatedAt = seedDate, UpdatedAt = seedDate },
                new { Id = 6, Name = "Vial", ShortName = "vial", IsActive = true, CreatedBy = Guid.Empty, UpdateBy = Guid.Empty, CreatedAt = seedDate, UpdatedAt = seedDate },
                new { Id = 7, Name = "Ampoule", ShortName = "amp", IsActive = true, CreatedBy = Guid.Empty, UpdateBy = Guid.Empty, CreatedAt = seedDate, UpdatedAt = seedDate });

            modelBuilder.Entity<Role>().HasData(
                new { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Name = "Admin", CreatedBy = Guid.Empty, UpdateBy = Guid.Empty, CreatedAt = seedDate, UpdatedAt = seedDate },
                new { Id = Guid.Parse("22222222-2222-2222-2222-222222222222"), Name = "Manager", CreatedBy = Guid.Empty, UpdateBy = Guid.Empty, CreatedAt = seedDate, UpdatedAt = seedDate },
                new { Id = Guid.Parse("33333333-3333-3333-3333-333333333333"), Name = "User", CreatedBy = Guid.Empty, UpdateBy = Guid.Empty, CreatedAt = seedDate, UpdatedAt = seedDate });

            modelBuilder.Entity<InventoryBatch>()
                .HasIndex(batch => new { batch.MedicineId, batch.BatchNumber })
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
                .HasForeignKey(batch => batch.MedicineId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<InventoryAdjustment>()
                .HasOne(adjustment => adjustment.Medicine)
                .WithMany(medicine => medicine.InventoryAdjustments)
                .HasForeignKey(adjustment => adjustment.MedicineId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<InventoryAdjustment>()
                .HasOne(adjustment => adjustment.InventoryBatch)
                .WithMany()
                .HasForeignKey(adjustment => adjustment.InventoryBatchId)
                .OnDelete(DeleteBehavior.Restrict);
            // modelBuilder.Entity<Purchase>()
            //     .HasOne(purchase => purchase.InventoryBatch)
            //     .WithMany()
            //     .HasForeignKey(purchase => purchase.InventoryBatchId)
            //     .OnDelete(DeleteBehavior.Restrict);
            // modelBuilder.Entity<Sale>()
            //     .HasOne(sale => sale.InventoryBatch)
            //     .WithMany()
            //     .HasForeignKey(sale => sale.InventoryBatchId)
            //     .OnDelete(DeleteBehavior.Restrict);
            // modelBuilder.Entity<SaleBatchAllocation>()
            //     .HasOne(allocation => allocation.Sale)
            //     .WithMany(sale => sale.BatchAllocations)
            //     .HasForeignKey(allocation => allocation.SaleID)
            //     .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<SaleBatchAllocation>()
                .HasOne(allocation => allocation.InventoryBatch)
                .WithMany()
                .HasForeignKey(allocation => allocation.InventoryBatchId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<SaleReturn>()
                .HasOne(returnItem => returnItem.SaleBatchAllocation)
                .WithMany(allocation => allocation.SaleReturns)
                .HasForeignKey(returnItem => returnItem.SaleBatchAlLocationId)
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
        public DbSet<Unit> Units { get; set; }
        public DbSet<MedicineUnit> MedicineUnits { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<InventoryBatch> InventoryBatches { get; set; }
        public DbSet<InventoryTransaction> InventoryTransactions { get; set; }
        public DbSet<InventoryAdjustment> InventoryAdjustments { get; set; }
        public DbSet<SaleBatchAllocation> SaleBatchAllocations { get; set; }
        public DbSet<SaleReturn> SaleReturns { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Location> Locations { get; set; }


        #endregion
    }
}
