using Dawa.ExtensionMethods;
using System;
using System.Collections.Generic;

namespace Dawa.Models.Parameters;

public class AutocompleteQueryParams : BaseDawaRequest
{
    public string? Type { get; set; }
    public Startfra? Startfra { get; set; }
    public string? Query { get; set; }
    public int? Caretpos { get; set; }
    public int? Postnr { get; set; }
    public int? Kommunekode { get; set; }
    public Guid? Adgangsadresseid { get; set; }
    public string? Multilinje { get; set; }
    public string? Supplerendebynavn { get; set; }
    public int? Stormodtagerpostnumre { get; set; }
    public string? Fuzzy { get; set; }
    public Guid? Id { get; set; }
    public string? Gældende { get; set; }
    public double[][]? Polygon { get; set; }
    public Geometri? Geometri { get; set; }
    public Cirkel? Cirkel { get; set; }
    public SRID? SRID { get; set; }
    public int? Side { get; set; }
    public int? Per_side { get; set; }

    public override Dictionary<string, string?> ToQueryParameters()
    {
        var dict = new Dictionary<string, string?>();

        if (Query is not null) dict["q"] = Query;
        if (Type is not null) dict["type"] = Type;
        if (Startfra is not null) dict["startfra"] = Startfra.ToString();
        if (Caretpos is not null) dict["caretpos"] = Caretpos.Value.ToString();
        if (Postnr is not null) dict["postnr"] = Postnr.Value.ToString();
        if (Kommunekode is not null) dict["kommunekode"] = Kommunekode.Value.ToString("D4");
        if (Adgangsadresseid is not null) dict["adgangsadresseid"] = Adgangsadresseid.Value.ToString();
        if (Multilinje is not null) dict["multilinje"] = Multilinje;
        if (Supplerendebynavn is not null) dict["supplerendebynavn"] = Supplerendebynavn;
        if (Stormodtagerpostnumre is not null) dict["stormodtagerpostnumre"] = Stormodtagerpostnumre.Value.ToString();
        if (Fuzzy is not null) dict["fuzzy"] = Fuzzy;
        if (Id is not null) dict["id"] = Id.Value.ToString();
        if (Gældende is not null) dict["gældende"] = Gældende;
        if (Polygon is not null) dict["polygon"] = Polygon.ToPolygonString();
        if (Geometri is not null) dict["geometri"] = Geometri.ToString();
        if (Cirkel is not null) dict["cirkel"] = Cirkel.ToString();
        if (SRID is not null) dict["srid"] = SRID.Value.ToSRIDString();
        if (Side is not null) dict["side"] = Side.Value.ToString();
        if (Per_side is not null) dict["per_side"] = Per_side.Value.ToString();

        return dict;
    }
}

public enum Startfra
{
    vejnavn = 0,
    adgangsadresse = 1,
    adresse = 2,
}