using System;
using System.Collections.Generic;

namespace Dawa.Models.Parameters;

public class NavngivenvejOpslagQueryParams(string id) : BaseDawaRequest
{

    public Guid Id { get; } = Throw.IfInvalidGuid(id, nameof(id));
    public Geometri? Geometri { get; set; }
    public SRID? SRID { get; set; }
    public bool? Medtagnedlagte { get; set; }

    public override Dictionary<string, string?> ToQueryParameters()
    {
        var dict = new Dictionary<string, string?>
        {
            ["id"] = Id.ToString()
        };

        if (Geometri is not null) dict["geometri"] = Geometri.ToString();
        if (SRID is not null) dict["srid"] = SRID.Value.ToSRIDString();
        if (Medtagnedlagte is not null) dict["medtagnedlagte"] = Medtagnedlagte.Value ? "true" : "false";

        return dict;
    }
}