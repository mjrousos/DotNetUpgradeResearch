using SerializationSample;

var obj = new Serializer.DataTransferObject
{
    Name = "Joe Smith",
    Age = 42
};

Console.WriteLine($"Created sample object: {obj}");
Console.WriteLine();

var str = Serializer.Serialize(obj);

Console.WriteLine($"Serialized object: {str}");
Console.WriteLine();

var obj2 = Serializer.Deserialize<Serializer.DataTransferObject>(str);

Console.WriteLine($"Deserialized object: {obj2}");
Console.WriteLine();