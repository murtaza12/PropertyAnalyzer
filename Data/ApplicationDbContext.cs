using Microsoft.EntityFrameworkCore;
using PropertyApp.Models;

namespace PropertyApp.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Property> Properties { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Property>(entity =>
        {
            entity.HasKey(p => p.Id);

            entity.Property(p => p.Address).HasMaxLength(200);
            entity.Property(p => p.City).HasMaxLength(100);
            entity.Property(p => p.Price).HasColumnType("numeric(18,2)");
            entity.Property(p => p.AreaInSqFt).HasColumnType("double precision");
            entity.Property(p => p.ListedDate).IsRequired();

            // ✅ Add Indexes
            entity.HasIndex(p => p.City);
            entity.HasIndex(p => p.ListedDate);
            entity.HasIndex(p => p.Price);
        });
    }
}
