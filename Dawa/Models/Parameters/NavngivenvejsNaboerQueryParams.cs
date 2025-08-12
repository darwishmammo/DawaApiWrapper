using System;
using System.Collections.Generic;
using System.Globalization;

namespace Dawa.Models.Parameters;

public class NavngivenvejsNaboerQueryParams(string id) : BaseDawaRequest
{
    public Guid Id { get; } = Throw.IfInvalidGuid(id, nameof(id));
    public double? Afstand { get; set; }
    public string? Noformat { get; set; }
    public SRID? SRID { get; set; }
    public int? Side { get; set; }
    public int? Per_side { get; set; }

    public override Dictionary<string, string?> ToQueryParameters()
    {
        var dict = new Dictionary<string, string?> { { "id", Id.ToString() } };

        if (Afstand is not null) dict["afstand"] = Afstand.Value.ToString(CultureInfo.InvariantCulture);
        if (Noformat is not null) dict["noformat"] = Noformat;
        if (SRID is not null) dict["srid"] = SRID.Value.ToSRIDString();
        if (Side is not null) dict["side"] = Side.ToString();
        if (Per_side is not null) dict["per_side"] = Per_side.ToString();

        return dict;
    }
}