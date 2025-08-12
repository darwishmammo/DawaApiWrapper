using System.Collections.Generic;

namespace Dawa.Models.Parameters;

public class JordstykkeOpslagQueryParams(string ejerlavkode, string matrikelnr) : BaseDawaRequest
{
    public string Ejerlavkode { get; } = Throw.IfNullOrWhitespace(ejerlavkode, nameof(ejerlavkode)).Trim();
    public string Matrikelnr { get; } = Throw.IfNullOrWhitespace(matrikelnr, nameof(matrikelnr)).Trim();

    public override Dictionary<string, string?> ToQueryParameters()
    {
        var dict = new Dictionary<string, string?>
        {
            { "ejerlavkode", Ejerlavkode },
            { "matrikelnr", Matrikelnr }
        };

        //if (Struktur is not null) dict["struktur"] = Struktur;

        return dict;
    }
}