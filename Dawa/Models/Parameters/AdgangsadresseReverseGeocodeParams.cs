using System.Collections.Generic;
using System.Globalization;

namespace Dawa.Models.Parameters;

public class AdgangsadresseReverseGeocodeParams(double x, double y) : BaseDawaRequest
{
    public double X { get; set; } = x;
    public double Y { get; set; } = y;
    public SRID? SRID { get; set; }
    public Geometri? Geometri { get; set; }
    public bool? Medtagugyldige { get; set; }
    public bool? Medtagnedlagte { get; set; }

    public override Dictionary<string, string?> ToQueryParameters()
    {
        var dict = new Dictionary<string, string?>
        {
            ["x"] = X.ToString(CultureInfo.InvariantCulture),
            ["y"] = Y.ToString(CultureInfo.InvariantCulture)
        };

        if (SRID is not null) dict["srid"] = SRID.Value.ToSRIDString();
        if (Geometri is not null) dict["geometri"] = $"{Geometri}";
        if (Medtagugyldige is not null) dict["medtagugyldige"] = Medtagugyldige.Value ? "true" : "false";
        if (Medtagnedlagte is not null) dict["medtagnedlagte"] = Medtagnedlagte.Value ? "true" : "false";
        //if (Struktur is not null) dict["struktur"] = $"{Struktur}";

        return dict;
    }
}