using System.Data.Entity;

namespace Vendor.Models
{
    public class AppFoodDbContext : DbContext
    {
        public AppFoodDbContext() : base("FoodDB")
        {

        }
        public DbSet<Products> Products { get; set; }
        public DbSet<SignupLogin> SignupLogin { get; set; }
        public DbSet<AdminLogin> adminLogin { get; set; }
        public DbSet<InvoiceModel> invoiceModel { get; set; }
        public DbSet<Order> orders { get; set; }
        public DbSet<ContactModel> contactModels { get; set; }
        public DbSet<BlogModel> blogModels { get; set; }
        public DbSet<SuperAdminLogin> SuperAdminLogins { get; set; }

        public DbSet<Category> Categories { get; set; }
        
        public DbSet<DeliveryInfo> DeliveryInfos { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Configure the one-to-one relationship between InvoiceModel and DeliveryInfo
            modelBuilder.Entity<InvoiceModel>()
                .HasOptional(i => i.DeliveryInfo)
                .WithRequired(d => d.Invoice)
                .Map(m => m.MapKey("FkInvoiceID"));

            base.OnModelCreating(modelBuilder);
        }
    }
}