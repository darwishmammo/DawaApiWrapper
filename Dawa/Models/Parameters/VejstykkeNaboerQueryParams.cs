using System.Collections.Generic;
using System.Globalization;

namespace Dawa.Models.Parameters;

public class VejstykkeNaboerQueryParams(int kommunekode, int vejkode) : BaseDawaRequest
{
    public int Kommunekode { get; } = kommunekode;
    public int Vejkode { get; } = vejkode;
    public double? Afstand { get; set; }
    public SRID? SRID { get; set; }
    public int? Side { get; set; }
    public int? Per_side { get; set; }

    public override Dictionary<string, string?> ToQueryParameters()
    {
        var dict = new Dictionary<string, string?>
        {
            ["kommunekode"] = Kommunekode.ToString(),
            ["vejkode"] = Vejkode.ToString()
        };

        if (Afstand is not null) dict["afstand"] = Afstand.Value.ToString(CultureInfo.InvariantCulture);
        if (SRID is not null) dict["srid"] = SRID.Value.ToSRIDString();
        if (Side is not null) dict["side"] = Side.Value.ToString();
        if (Per_side is not null) dict["per_side"] = Per_side.Value.ToString();

        return dict;
    }
}