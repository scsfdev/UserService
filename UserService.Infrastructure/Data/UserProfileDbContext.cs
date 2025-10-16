using Microsoft.EntityFrameworkCore;
using UserService.Domain.Entities;

namespace UserService.Infrastructure.Data
{
    public class UserProfileDbContext(DbContextOptions<UserProfileDbContext> options): DbContext(options)
    {
        public DbSet<UserProfile> UserProfiles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserProfile>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).IsRequired().HasDefaultValueSql("NEWID()");
                entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
                entity.Property(e => e.DisplayName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.IsActive).IsRequired().HasDefaultValue(true);
                entity.Property(e => e.CreatedAt).IsRequired().HasDefaultValueSql("GETDATE()").HasColumnType("datetime2(0)");
                entity.Property(e => e.UpdatedAt).IsRequired().HasDefaultValueSql("GETDATE()").HasColumnType("datetime2(0)");
            });
        }
    }
}
