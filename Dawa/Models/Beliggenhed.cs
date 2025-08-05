using System.Text.Json.Serialization;

namespace Dawa.Models;

public record Beliggenhed
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Oprindelse? Oprindelse { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public object? Vejtilslutningspunkter { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Geometritype { get; set; }
}