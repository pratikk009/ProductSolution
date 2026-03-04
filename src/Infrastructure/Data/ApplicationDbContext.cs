using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Item> Items => Set<Item>();

    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<User> Users => Set<User>();



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Product");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.ProductName)
                  .IsRequired()
                  .HasMaxLength(255);

            entity.Property(x => x.CreatedBy)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(x => x.Price)
         .HasColumnType("decimal(18,2)");
        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.ToTable("Item");

            entity.HasKey(x => x.Id);

            entity.HasOne(x => x.Product)
                  .WithMany(p => p.Items)
                  .HasForeignKey(x => x.ProductId);
        });
    }
}