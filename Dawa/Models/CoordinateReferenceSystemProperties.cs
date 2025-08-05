using System.Text.Json.Serialization;

namespace Dawa.Models;

public record CoordinateReferenceSystemProperties
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Name { get; set; }
}