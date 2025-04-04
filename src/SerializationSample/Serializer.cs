using Newtonsoft.Json;
using System.Runtime.Serialization.Formatters.Binary;

namespace SerializationSample
{
    internal class Serializer
    {
        [Serializable]
        public class DataTransferObject
        {
            public string? Name { get; set; }

            [JsonRequired]
            public int Age { get; set; }

            public override string ToString() => $"Name: {Name}, Age: {Age}";
        }

        public static string Serialize<T>(T obj)
        {
            var serializer = new BinaryFormatter();
            using var memoryStream = new MemoryStream();
            serializer.Serialize(memoryStream, obj);
            return Convert.ToBase64String(memoryStream.ToArray());
        }

        internal static T Deserialize<T>(string str)
        {
            var serializer = new BinaryFormatter();
            using var memoryStream = new MemoryStream(Convert.FromBase64String(str));
            return (T)serializer.Deserialize(memoryStream);
        }
    }
}
