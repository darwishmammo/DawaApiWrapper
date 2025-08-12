using System.Collections.Generic;

namespace Dawa.Models.Parameters;

public class VejstykkeOpslagQueryParams(int kommunekode, int vejkode) : BaseDawaRequest
{
    public int Kommunekode { get; } = kommunekode;
    public int Vejkode { get; } = vejkode;
    public bool? Medtagnedlagte { get; set; }
    public SRID? SRID { get; set; }

    public override Dictionary<string, string?> ToQueryParameters()
    {
        var dict = new Dictionary<string, string?>();

        if (Medtagnedlagte is not null) dict["medtagnedlagte"] = Medtagnedlagte.Value ? "true" : "false";
        if (SRID is not null) dict["srid"] = SRID.Value.ToSRIDString();

        return dict;
    }
}