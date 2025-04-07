using EfCoreSample.Models;
using System;

namespace EfCoreSample.Tests;

public class DatabaseFixture : IDisposable
{
    public const string ConnectionString = "Server=(localdb)\\MSSQLLocalDB;Database=UsersTestDb;Trusted_Connection=True;TrustServerCertificate=True;";

    public UserDbContext UserDbContext { get; private set; }

    public DatabaseFixture()
    {
        UserDbContext = new UserDbContext(ConnectionString);
        UserDbContext.Database.EnsureDeleted();
        UserDbContext.Database.EnsureCreated();
    }

    public void Dispose()
    {
        UserDbContext.Database.EnsureDeleted();
        UserDbContext.Dispose();
    }
}
