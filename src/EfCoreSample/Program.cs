using System;
using System.Linq;
using EfCoreSample.Models;
using Microsoft.Data.SqlClient;

const string ConnectionString = "Server=(localdb)\\MSSQLLocalDB;Database=UsersDb;Trusted_Connection=True;TrustServerCertificate=True;";

// Create a new user
var user = new User
{
    Name = "John Doe",
    Email = "john.doe@example.com",
    CreatedAt = DateTime.Now,
    Status = new UserStatus
    {
        State = UserStatusState.Active,
        LastUpdated = DateTime.Now,
        Description = "User is active and verified."
    }
};

// Use the DbContext to add, query, and delete the user
using (var context = new UserDbContext(ConnectionString))
{
    // Ensure database is created
    context.Database.EnsureCreated();
    
    // Add the user
    Console.WriteLine("Adding a new user to the database...");
    context.Users.Add(user);
    context.SaveChanges();
    Console.WriteLine($"Added user with ID: {user.Id}");
    
    // Query users
    Console.WriteLine("\nQuerying users from the database using EF Core...");
    var users = context.Users.ToList();
    
    foreach (var u in users)
    {
        Console.WriteLine($"User ID: {u.Id}");
        Console.WriteLine($"Name: {u.Name}");
        Console.WriteLine($"Email: {u.Email}");
        Console.WriteLine($"Created At: {u.CreatedAt}");
        Console.WriteLine($"Status: {u.Status.State}");
        Console.WriteLine($"Status Description: {u.Status.Description}");
        Console.WriteLine();
    }
    
    // Now query the same data using Microsoft.Data.SqlClient directly
    Console.WriteLine("\nQuerying the same user using Microsoft.Data.SqlClient directly...");
    
    using (var connection = new SqlConnection(ConnectionString))
    {
        connection.Open();
        
        using (var command = connection.CreateCommand())
        {
            command.CommandText = "SELECT Id, Name, Email, CreatedAt, Status FROM Users WHERE Id = @id";
            command.Parameters.AddWithValue("@id", user.Id);
            
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Console.WriteLine("Direct SQL Server query results:");
                    Console.WriteLine($"User ID: {reader.GetInt32(0)}");
                    Console.WriteLine($"Name: {reader.GetString(1)}");
                    Console.WriteLine($"Email: {reader.GetString(2)}");
                    Console.WriteLine($"Created At: {reader.GetDateTime(3)}");
                    Console.WriteLine($"Status: {reader.GetString(4)}");
                    Console.WriteLine();
                }
            }
        }
    }
    
    // Delete the user
    Console.WriteLine("\nDeleting the user...");
    context.Users.Remove(user);
    context.SaveChanges();
    
    // Verify deletion
    var remainingUsers = context.Users.Count();
    Console.WriteLine($"Remaining users in the database: {remainingUsers}");
}