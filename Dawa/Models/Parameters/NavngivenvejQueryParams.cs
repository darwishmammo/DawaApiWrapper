using Dawa.ExtensionMethods;
using System.Collections.Generic;
using System.Globalization;

namespace Dawa.Models.Parameters;

public class NavngivenvejQueryParams : BaseDawaRequest
{
    public string? Query { get; set; }
    public string? Id { get; set; }
    public string? Navn { get; set; }
    public string? Postnummer { get; set; }
    public string? Kommune { get; set; }
    public string? Adresseringsnavn { get; set; }
    public string? Kommunekode { get; set; }
    public string? Administrerendekommunekode { get; set; }
    public string? Regex { get; set; }
    public string? Fuzzy { get; set; }
    public string? Vejstykkeid { get; set; }
    public bool? Medtagnedlagte { get; set; }
    public Geometri? Geometri { get; set; }
    public double[][]? Polygon { get; set; }
    public Cirkel? Cirkel { get; set; }
    public double? X { get; set; }
    public double? Y { get; set; }
    public SRID? SRID { get; set; }
    public int? Side { get; set; }
    public int? Per_side { get; set; }
    //public string? struktur { get; set; }

    public override Dictionary<string, string?> ToQueryParameters()
    {
        var dict = new Dictionary<string, string?>();
        if (Query is not null) dict["q"] = Query;
        if (Id is not null) dict["id"] = Id;
        if (Navn is not null) dict["navn"] = Navn;
        if (Postnummer is not null) dict["postnr"] = Postnummer;
        if (Kommune is not null) dict["kommune"] = Kommune;
        if (Adresseringsnavn is not null) dict["adresseringsnavn"] = Adresseringsnavn;
        if (Kommunekode is not null) dict["kommunekode"] = Kommunekode;
        if (Administrerendekommunekode is not null) dict["administrerendekommunekode"] = Administrerendekommunekode;
        if (Regex is not null) dict["regex"] = Regex;
        if (Fuzzy is not null) dict["fuzzy"] = Fuzzy;
        if (Vejstykkeid is not null) dict["vejstykkeid"] = Vejstykkeid;
        if (Medtagnedlagte is not null) dict["medtagnedlagte"] = Medtagnedlagte.Value ? "true" : "false";
        if (Geometri is not null) dict["geometri"] = Geometri.ToString();
        if (Polygon is not null) dict["polygon"] = Polygon.ToPolygonString();
        if (Cirkel is not null) dict["cirkel"] = Cirkel.ToString();
        if (X is not null && Y is not null) dict["x"] = X.Value.ToString(CultureInfo.InvariantCulture);
        if (Y is not null && X is not null) dict["y"] = Y.Value.ToString(CultureInfo.InvariantCulture);
        if (SRID is not null) dict["srid"] = SRID.Value.ToSRIDString();
        if (Side is not null) dict["side"] = Side.Value.ToString();
        if (Per_side is not null) dict["per_side"] = Per_side.Value.ToString();

        return dict;
    }
}