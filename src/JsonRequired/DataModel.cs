using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace JsonRequired;

public class DataModel
{
    [Required]
    [JsonRequired]
    [JsonPropertyName("id")]
    public string Name { get; set; }
}
