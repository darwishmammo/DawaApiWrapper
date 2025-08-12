using System.Collections.Generic;

namespace Dawa.Models.Parameters;

public class PostnummerOpslagQueryParams(string nr) : BaseDawaRequest
{
    public string Nr { get; } = Throw.IfNullOrWhitespace(nr);
    public string? Landpostnumre { get; set; }

    public override Dictionary<string, string?> ToQueryParameters()
    {
        var dict = new Dictionary<string, string?> { ["nr"] = Nr };

        if (Landpostnumre is not null) dict["landpostnumre"] = Landpostnumre;
        //if (Struktur is not null) dict["struktur"] = Struktur;

        return dict;
    }
}