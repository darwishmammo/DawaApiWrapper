using Dawa.ExtensionMethods;
using System.Collections.Generic;

namespace Dawa.Models.Parameters;

public class PostnummerQueryParams : BaseDawaRequest
{
    public string? Query { get; set; }
    public string? AutoComplete { get; set; }
    public int? Nr { get; set; }
    public string? Navn { get; set; }
    public int? Kommunekode { get; set; }
    public string? Landpostnumre { get; set; }
    public int? Srid { get; set; }
    public Cirkel? Cirkel { get; set; }
    public double[][]? Polygon { get; set; }
    public string? Noformat { get; set; }
    public string? Ndjson { get; set; }
    public string? Side { get; set; }
    public string? PerSide { get; set; }
    //public string? Struktur { get; set; }

    public override Dictionary<string, string?> ToQueryParameters()
    {
        var dict = new Dictionary<string, string?>();
        if (Query is not null) dict["q"] = Query;
        if (AutoComplete is not null) dict["autocomplete"] = AutoComplete;
        if (Nr is not null) dict["nr"] = $"{Nr}";
        if (Navn is not null) dict["navn"] = Navn;
        if (Kommunekode is not null) dict["kommunekode"] = Kommunekode.Value.ToString("D4");
        if (Landpostnumre is not null) dict["landpostnumre"] = Landpostnumre;
        if (Srid is not null) dict["srid"] = $"{Srid}";
        if (Cirkel is not null) dict["cirkel"] = $"{Cirkel}";
        if (Polygon is not null) dict["polygon"] = Polygon.ToPolygonString();
        if (Noformat is not null) dict["noformat"] = Noformat;
        if (Ndjson is not null) dict["ndjson"] = Ndjson;
        if (Side is not null) dict["side"] = Side;
        if (PerSide is not null) dict["per_side"] = PerSide;
        //if (Struktur is not null) dict["struktur"] = Struktur;

        return dict;
    }
}