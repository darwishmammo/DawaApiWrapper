using System.Text.Json.Serialization;

namespace Dawa.Models;

public record AdresseAutocompleteResponse
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Tekst { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Adresse? Adresse { get; set; }
}
