using System;
using System.Collections.Generic;

namespace Dawa.Models.Parameters;

public class JordstykkeOpslagQueryParams(string ejerlavkode, string matrikelnr) : BaseDawaRequest
{
    public string Ejerlavkode { get; set; } = ejerlavkode ?? throw new ArgumentNullException(nameof(ejerlavkode));
    public string Matrikelnr { get; set; } = matrikelnr ?? throw new ArgumentNullException(nameof(matrikelnr));

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