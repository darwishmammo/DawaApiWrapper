
using Dawa.ExtensionMethods;
using System.Collections.Generic;

namespace Dawa.Models.Parameters;

public class JordstykkeAutocompleteQueryParams : BaseDawaRequest
{
    public string? Query { get; set; }
    public string? Matrikelnr { get; set; }
    public string? Ejerlavkode { get; set; }
    public int? Kommunekode { get; set; }
    public string? Regionskode { get; set; }
    public string? Sognekode { get; set; }
    public string? Retskredskode { get; set; }
    public string? Sfeejendomsnr { get; set; }
    public string? Featureid { get; set; }
    public string? Moderjordstykke { get; set; }
    public string? Bfenummer { get; set; }
    public SRID? SRID { get; set; }
    public Cirkel? Cirkel { get; set; }
    public double[][]? Polygon { get; set; }
    public string? Side { get; set; }
    public string? PerSide { get; set; }

    public override Dictionary<string, string?> ToQueryParameters()
    {
        var dict = new Dictionary<string, string?>();
        if (Query is not null) dict["q"] = Query;
        if (Matrikelnr is not null) dict["matrikelnr"] = Matrikelnr;
        if (Ejerlavkode is not null) dict["ejerlavkode"] = Ejerlavkode;
        if (Kommunekode is not null) dict["kommunekode"] = Kommunekode.Value.ToString("D4");
        if (Regionskode is not null) dict["regionskode"] = Regionskode;
        if (Sognekode is not null) dict["sognekode"] = Sognekode;
        if (Retskredskode is not null) dict["retskredskode"] = Retskredskode;
        if (Sfeejendomsnr is not null) dict["sfeejendomsnr"] = Sfeejendomsnr;
        if (Featureid is not null) dict["featureid"] = Featureid;
        if (Moderjordstykke is not null) dict["moderjordstykke"] = Moderjordstykke;
        if (Bfenummer is not null) dict["bfenummer"] = Bfenummer;
        if (SRID is not null) dict["srid"] = SRID.Value.ToSRIDString();
        if (Cirkel is not null) dict["cirkel"] = $"{Cirkel}";
        if (Polygon is not null) dict["polygon"] = Polygon.ToPolygonString();
        if (Side is not null) dict["side"] = Side;
        if (PerSide is not null) dict["per_side"] = PerSide;

        return dict;
    }
}