using System;
using System.Collections.Generic;

namespace Dawa.Models.Parameters;

public class AdresseOpslagQueryParams(Guid id) : BaseDawaRequest
{

    public Guid Id { get; set; } = id;
    public Geometri? Geometri { get; set; }
    public string? Medtagugyldige { get; set; }
    public string? Medtagnedlagte { get; set; }

    public override Dictionary<string, string?> ToQueryParameters()
    {
        var dict = new Dictionary<string, string?>();

        if (Geometri is not null) dict["geometri"] = $"{Geometri}";
        if (Medtagugyldige is not null) dict["medtagugyldige"] = Medtagugyldige;
        if (Medtagnedlagte is not null) dict["medtagnedlagte"] = Medtagnedlagte;
        //if (Struktur is not null) dict["struktur"] = Struktur;

        return dict;
    }
}