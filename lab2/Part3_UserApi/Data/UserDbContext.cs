using Microsoft.EntityFrameworkCore;
using Part3_UserApi.Models;

namespace Part3_UserApi.Data;

public class UserDbContext : DbContext
{
    public UserDbContext(DbContextOptions<UserDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Login).HasMaxLength(100).IsRequired();
            entity.HasIndex(x => x.Login).IsUnique();
            entity.Property(x => x.PassHash).HasMaxLength(255).IsRequired();
        });
    }
}
