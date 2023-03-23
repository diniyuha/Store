using Microsoft.EntityFrameworkCore;
using Store_entities.Entities;

namespace Store_entities
{
    public class StoreDbContext : DbContext
    {
        public StoreDbContext(DbContextOptions<StoreDbContext> options)
            : base(options)
        {
        }

        public DbSet<OrderEntity> Orders { get; set; }
        public DbSet<OrderItemEntity> OrderItems { get; set; }
        public DbSet<ProviderEntity> Providers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderEntity>()
                .HasMany(o => o.OrderItems)
                .WithOne(i => i.Order)
                .HasForeignKey(i => i.OrderId);

            modelBuilder.Entity<OrderEntity>()
                .HasIndex(o => new {o.Number, o.ProviderId})
                .IsUnique();

            modelBuilder.Entity<OrderEntity>()
                .HasOne(o => o.Provider)
                .WithMany(p => p.Orders)
                .HasForeignKey(o => o.ProviderId);

            modelBuilder.Entity<OrderItemEntity>()
                .HasKey(i => i.Id);
            modelBuilder.Entity<OrderItemEntity>()
                .Property(i => i.Quantity)
                .HasPrecision(18, 3);

            modelBuilder.Entity<ProviderEntity>()
                .HasKey(p => p.Id);
        }
    }
}