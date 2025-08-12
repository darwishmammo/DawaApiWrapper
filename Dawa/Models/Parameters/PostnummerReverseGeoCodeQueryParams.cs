using System.Collections.Generic;
using System.Globalization;

namespace Dawa.Models.Parameters;

public class PostnummerReverseGeoCodeQueryParams(double x, double y) : BaseDawaRequest
{
    public double X { get; } = x;
    public double Y { get; } = y;
    public SRID? SRID { get; set; }
    public string? Noformat { get; set; }
    public string? Landpostnumre { get; set; }

    public override Dictionary<string, string?> ToQueryParameters()
    {
        var dict = new Dictionary<string, string?>
        {
            ["x"] = X.ToString(CultureInfo.InvariantCulture),
            ["y"] = Y.ToString(CultureInfo.InvariantCulture)
        };

        if (SRID is not null) dict["srid"] = SRID.Value.ToSRIDString();
        if (Noformat is not null) dict["noformat"] = Noformat;
        if (Landpostnumre is not null) dict["landpostnumre"] = Landpostnumre;
        //if (Struktur is not null) dict["struktur"] = Struktur;

        return dict;
    }
}