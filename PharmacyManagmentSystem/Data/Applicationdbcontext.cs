using Microsoft.EntityFrameworkCore;
using PharmacyManagmentSystem.Models;

namespace PharmacyManagmentSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Medicine> Medicines { get; set; }
        public DbSet<Category> Catagories { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Relation between medicine and category(Many-to-One)
            modelBuilder.Entity<Medicine>(entity =>
            {
                entity.HasKey(m => m.MedicineID);
                entity.HasMany(m => m.Categories)
                .WithOne(c => c.Medicine)
                .HasForeignKey(c => c.MedicineID)
                .OnDelete(DeleteBehavior.NoAction);
            });
            //Relation between Medicine, Purchase and Supplier(Many-to-Many)
            modelBuilder.Entity<Purchase>()
                .HasKey(p => p.PurchaseID);
            modelBuilder.Entity<Purchase>()
                .HasKey(p => new { p.MedicineID, p.SupplierID });
            // Relation between Purchase and Medicine
            modelBuilder.Entity<Purchase>()
                .HasOne(p => p.Medicine)
                .WithMany(m => m.Purchases)
                .HasForeignKey(p => p.MedicineID);
            // Relation between Purchase and Supplier
            modelBuilder.Entity<Purchase>()
                .HasOne(p => p.Supplier)
                .WithMany(s => s.Purchases)
                .HasForeignKey(p => p.SupplierID);
            // Configure the many-to-many relationship between Medicine and Customer
            modelBuilder.Entity<Sale>()
                .HasKey(s => new { s.MedicineID, s.CustomerID });
            // Relation between Medicine and sale
            modelBuilder.Entity<Sale>()
                .HasOne(s => s.Medicine)
                .WithMany(m => m.Sales)
                .HasForeignKey(s => s.MedicineID);
            // Relation between Customer and sale
            modelBuilder.Entity<Sale>()
                .HasOne(s => s.Customer)
                .WithMany(c => c.Sales)
                .HasForeignKey(s => s.CustomerID);
            // Relationship between Sale and Currency
            modelBuilder.Entity<Sale>()
                .HasOne(s => s.Currency)
                .WithOne()
                .HasForeignKey<Sale>(s => s.CurrencyID);
            // Relationship between Purchase and Currency
            modelBuilder.Entity<Purchase>()
                .HasOne(p => p.Currency)
                .WithOne()
                .HasForeignKey<Purchase>(p => p.CurrencyID);



        }
    }
}
