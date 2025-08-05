using System.Text.Json.Serialization;

namespace Dawa.Models;

public record CoordinateReferenceSystem
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Type { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public CoordinateReferenceSystemProperties? Properties { get; set; }
}