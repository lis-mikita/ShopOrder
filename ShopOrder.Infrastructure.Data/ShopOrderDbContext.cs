using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ShopOrder.Domain.Core.Models.OrderDetails;
using ShopOrder.Domain.Core.Models.Orders;
using ShopOrder.Domain.Core.Models.Users; 

namespace ShopOrder.Infrastructure.Data
{
    public partial class ShopOrderDbContext : DbContext
    {
        public ShopOrderDbContext() { }
        public ShopOrderDbContext(DbContextOptions<ShopOrderDbContext> options)
            : base(options) { }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.HasIndex(e => e.Email)
                    .IsUnique();
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.OrderId);

                entity.HasOne(o => o.User)
                    .WithMany(u => u.Orders)
                    .HasForeignKey(o => o.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.HasKey(e => e.OrderDetailId);

                entity.HasOne(od => od.Order)
                    .WithMany(o => o.OrderDetails)
                    .HasForeignKey(od => od.OrderId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.Property(od => od.Subtotal)
                    .HasComputedColumnSql("[Quantity] * [Price]");
            });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string configFile = Path.Combine(currentDirectory, "..\\ShopOrder\\appsettings.json");

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .AddJsonFile(configFile)
                .Build();

            string migrationConnectionString = configuration.GetConnectionString("MigrationConnection") ?? "DefaultConnectionString";
            optionsBuilder.UseSqlServer(migrationConnectionString);
        }
    }
}
