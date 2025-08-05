using System.Text.Json.Serialization;

namespace Dawa.Models;

public record DDKN
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? M100 { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Km1 { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Km10 { get; set; }
}