using System.Text.Json.Serialization;

namespace Dawa.Models;

public record AdgangsadresseAutocompleteResponse
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Tekst { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Adgangsadresse? Adgangsadresse { get; set; }
}