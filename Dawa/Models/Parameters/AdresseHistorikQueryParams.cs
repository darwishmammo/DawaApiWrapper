using System;
using System.Collections.Generic;

namespace Dawa.Models.Parameters;

public class AdresseHistorikQueryParams : BaseDawaRequest
{
    public Guid? Id { get; set; }
    public int? Postnr { get; set; }
    public int? Kommunekode { get; set; }
    public Guid? Adgangsadresseid { get; set; }
    public int? Side { get; set; }
    public int? PerSide { get; set; }

    public override Dictionary<string, string?> ToQueryParameters()
    {
        var dict = new Dictionary<string, string?>();

        if (Id is not null) dict["id"] = $"{Id}";
        if (Postnr is not null) dict["postnr"] = $"{Postnr}";
        if (Kommunekode is not null) dict["kommunekode"] = $"{Kommunekode}";
        if (Adgangsadresseid is not null) dict["adgangsadresseid"] = $"{Adgangsadresseid}";
        if (Side is not null) dict["side"] = $"{Side}";
        if (PerSide is not null) dict["per_side"] = $"{PerSide}";

        return dict;
    }
}