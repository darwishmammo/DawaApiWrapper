using Dawa.ExtensionMethods;
using System;
using System.Collections.Generic;

namespace Dawa.Models.Parameters;

public class VejstykkerAutocompleteQueryParams : BaseDawaRequest
{
    public Guid? Id { get; set; }
    public string? Query { get; set; }
    public string? Fuzzy { get; set; }
    public int? Kommunekode { get; set; }
    public int? Kode { get; set; }
    public Guid? Navngivenvej_id { get; set; }
    public string? Navn { get; set; }
    public string? Regex { get; set; }
    public bool? Medtagnedlagte { get; set; }
    public int? Postnr { get; set; }
    public double[][]? Polygon { get; set; }
    public Cirkel? Cirkel { get; set; }
    public SRID? SRID { get; set; }
    public int? Side { get; set; }
    public int? Per_side { get; set; }

    public override Dictionary<string, string?> ToQueryParameters()
    {
        var dict = new Dictionary<string, string?>();

        if (Id is not null) dict["id"] = Id.ToString();
        if (Query is not null) dict["q"] = Query;
        if (Fuzzy is not null) dict["fuzzy"] = Fuzzy;
        if (Kommunekode is not null) dict["kommunekode"] = Kommunekode.Value.ToString();
        if (Kode is not null) dict["kode"] = Kode.ToString();
        if (Navngivenvej_id is not null) dict["navngivenvej_id"] = Navngivenvej_id.ToString();
        if (Navn is not null) dict["navn"] = Navn;
        if (Regex is not null) dict["regex"] = Regex;
        if (Medtagnedlagte is not null) dict["medtagnedlagte"] = Medtagnedlagte.Value ? "true" : "false";
        if (Postnr is not null) dict["postnr"] = Postnr.Value.ToString();
        if (Polygon is not null) dict["polygon"] = Polygon.ToPolygonString();
        if (Cirkel is not null) dict["cirkel"] = Cirkel.ToString();
        if (SRID is not null) dict["srid"] = SRID.Value.ToSRIDString();
        if (Side is not null) dict["side"] = Side.Value.ToString();
        if (Per_side is not null) dict["per_side"] = Per_side.Value.ToString();

        return dict;
    }
}
