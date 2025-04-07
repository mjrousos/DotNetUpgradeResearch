using Microsoft.EntityFrameworkCore;

namespace EfCoreSample.Models;

public class UserDbContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public UserDbContext(string connectionString) : base(new DbContextOptionsBuilder<UserDbContext>()
        .UseSqlServer(connectionString)
        .Options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .OwnsOne(u => u.Status, builder =>
            {
                builder.ToJson();
            });
    }
}
