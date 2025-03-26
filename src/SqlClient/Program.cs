using System;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace SqlClientSample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var connectionString = "Server=(LocalDb)\\MSSQLLocalDB;Database=SampleDatabase;Trusted_Connection=True;";
            var tableName = $"TestTable_{Guid.NewGuid().ToString("N")}";

            try
            {
                using var connection = new SqlConnection(connectionString);
                connection.Open();
                Console.WriteLine("Connected to SQL Server successfully.");

                // Create table
                var createTableQuery = $@"
                        CREATE TABLE {tableName} (
                            Id INT IDENTITY(1,1) PRIMARY KEY,
                            Name NVARCHAR(100),
                            CreatedDate DATETIME DEFAULT GETDATE()
                        )";

                using var createCommand = new SqlCommand(createTableQuery, connection);
                createCommand.ExecuteNonQuery();
                Console.WriteLine($"Table {tableName} created successfully.");

                // Insert data
                string[] names = { "Alice", "Bob", "Charlie", "David", "Eve" };
                var parameterNames = names.Select((_, index) => $"@Name{index + 1}").ToArray();

                var insertQuery = $"INSERT INTO {tableName} (Name) VALUES ({string.Join("),(", parameterNames)})";
                using var insertCommand = new SqlCommand(insertQuery, connection);
                for (int i = 0; i < names.Length; i++)
                {
                    insertCommand.Parameters.AddWithValue($"@Name{i + 1}", names[i]);
                }
                insertCommand.ExecuteNonQuery();
                Console.WriteLine("Data inserted successfully.");

                // Get row count
                var countQuery = $"SELECT COUNT(*) FROM {tableName}";
                using var countCommand = new SqlCommand(countQuery, connection);
                var rowCount = (int)countCommand.ExecuteScalar();
                Console.WriteLine($"Number of rows in the table: {rowCount}");

                // Drop table
                var dropTableQuery = $"DROP TABLE {tableName}";
                using var dropCommand = new SqlCommand(dropTableQuery, connection);
                dropCommand.ExecuteNonQuery();
                Console.WriteLine($"Table {tableName} dropped successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
