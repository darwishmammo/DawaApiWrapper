using System.Text.Json.Serialization;

namespace Dawa.Models;

public record Supplerendebynavn2
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Href { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Dagi_id { get; set; }
}