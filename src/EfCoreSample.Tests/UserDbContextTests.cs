using System;
using Microsoft.Data.SqlClient;
using EfCoreSample.Models;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Linq;

namespace EfCoreSample.Tests;

public class UserDbContextTests : IClassFixture<DatabaseFixture>
{
    private readonly UserDbContext dbContext;
    private readonly string connectionString;

    public UserDbContextTests(DatabaseFixture databaseFixture)
    {
        dbContext = databaseFixture.UserDbContext;
        connectionString = DatabaseFixture.ConnectionString;
    }

    [Fact]
    public void UsersAreStoredAsExpected()
    {
        // Arrange

        // Create the user using EF Core
        var user = new User
        {
            Name = "Test User",
            Email = "test@example.com",
            CreatedAt = DateTime.Now,
            Status = new UserStatus
            {
                State = UserStatusState.Active,
                LastUpdated = DateTime.Now,
                Description = "New user"
            }
        };

        var jsonSerializationOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter(allowIntegerValues: false) }
        };

        // Act
        try
        {
            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            // Assert - use SqlClient to verify the data was stored correctly
            using var connection = new SqlConnection(connectionString);
            connection.Open();

            using var command = new SqlCommand("SELECT TOP 1 * FROM Users", connection);
            using var reader = command.ExecuteReader();
            Assert.True(reader.Read(), "No user found in the database");

            Assert.Equal(user.Id, reader.GetInt32(reader.GetOrdinal("Id")));
            Assert.Equal(user.Name, reader.GetString(reader.GetOrdinal("Name")));
            Assert.Equal(user.Email, reader.GetString(reader.GetOrdinal("Email")));

            // Get the Status column (JSON) and deserialize it
            var statusJson = reader.GetString(reader.GetOrdinal("Status"));
            
            var status = JsonSerializer.Deserialize<UserStatus>(statusJson, jsonSerializationOptions);

            Assert.NotNull(status);
            Assert.Equal(UserStatusState.Active, status.State);
            Assert.Equal("New user", status.Description);
        }
        finally
        {
            // Clean up - remove the user from the database
            if (dbContext.Users.Any(u => u.Id == user.Id))
            {
                dbContext.Users.Remove(user);
                dbContext.SaveChanges();
            }
        }
    }
}
