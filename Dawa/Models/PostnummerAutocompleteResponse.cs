using System.Text.Json.Serialization;

namespace Dawa.Models;

public record PostnummerAutocompleteResponse
{

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Tekst { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Postnummer? Postnummer { get; set; }
}