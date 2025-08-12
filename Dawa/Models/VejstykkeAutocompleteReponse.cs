using System.Text.Json.Serialization;

namespace Dawa.Models;

public class VejstykkeAutocompleteReponse
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Tekst { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Vejstykke? Vejstykke { get; set; }
}
