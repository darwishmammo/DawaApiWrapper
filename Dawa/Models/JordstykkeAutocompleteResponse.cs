using System.Text.Json.Serialization;

namespace Dawa.Models;

public record JordstykkeAutocompleteResponse
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Tekst { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Jordstykke? Jordstykke { get; set; }
}