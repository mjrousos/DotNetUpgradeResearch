using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;
using System.Text.Json;
using System.Runtime.Serialization.Formatters.Binary;

namespace DuplicateUsingStatement
{
    class Program
    {
        [Serializable]
        public class SerializableTest
        {
            public string Message { get; set; }
        }

        static async Task Main()
        {
            // Using List<T> (Collections.Generic)
            var numbers = new List<int> { 1, 2, 3, 4, 5 };

            // Using LINQ
            var evenNumbers = numbers.Where(n => n % 2 == 0).ToList();
            Console.WriteLine("Even numbers: " + string.Join(", ", evenNumbers));

            // Using StringBuilder
            var sb = new StringBuilder();
            sb.AppendLine("Building a string");
            sb.AppendLine("Line by line");
            Console.WriteLine(sb.ToString());

            // Using File I/O
            string tempFile = "temp.txt";
            await File.WriteAllTextAsync(tempFile, "Hello from file!");
            string fileContent = await File.ReadAllTextAsync(tempFile);
            Console.WriteLine($"File content: {fileContent}");

            // Using HttpClient
            using var client = new HttpClient();
            try
            {
                var response = await client.GetStringAsync("https://api.github.com/zen");
                Console.WriteLine($"GitHub Zen: {response}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"API request failed: {ex.Message}");
            }

            // Using JSON serialization
            var person = new { Name = "John", Age = 30 };
            string json = JsonSerializer.Serialize(person);
            Console.WriteLine($"JSON: {json}");

            // Using Task and Thread
            await Task.Delay(1000);
            Console.WriteLine("After one second delay");

            var testObject = new SerializableTest { Message = "Test serialization" };
            var formatter = new BinaryFormatter();

            using (var stream = new MemoryStream())
            {
                formatter.Serialize(stream, testObject);
                stream.Position = 0;

                var deserializedObject = (SerializableTest)formatter.Deserialize(stream);
                Console.WriteLine($"Deserialized message: {deserializedObject.Message}");
            }

            // Cleanup
            if (File.Exists(tempFile))
            {
                File.Delete(tempFile);
            }
        }
    }
}