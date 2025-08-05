using Dawa.ExtensionMethods;
using System.Collections.Generic;

namespace Dawa.Models.Parameters;

public class AdresseQueryParams : BaseDawaRequest
{
    public string? Query { get; set; }
    public string? AutoComplete { get; set; }
    public string? Fuzzy { get; set; }
    public string? KVH { get; set; }
    public string? Id { get; set; }
    public string? Vejnavn { get; set; }
    public string? HusNr { get; set; }
    public int? PostNr { get; set; }
    public int? Kommunekode { get; set; }
    public string? MatrikelNr { get; set; }
    public int? Ejerlavkode { get; set; }
    public int? SRID { get; set; }
    public double[][]? Polygon { get; set; }
    public string? Status { get; set; }
    public string? HusNrFra { get; set; }
    public string? HusNrTil { get; set; }
    public Cirkel? Cirkel { get; set; }
    public string? Nøjagtighed { get; set; }
    public string? Regionskode { get; set; }
    public string? Landsdelsnuts3 { get; set; }
    public string? Sognekode { get; set; }
    public string? Afstemningsområdenummer { get; set; }
    public string? Opstillingskredskode { get; set; }
    public string? Storkredsnummer { get; set; }
    public string? Valglandsdelsbogstav { get; set; }
    public string? Retskredskode { get; set; }
    public string? Politikredskode { get; set; }
    public string? Stednavnid { get; set; }
    public string? Stedid { get; set; }
    public string? Stednavnafstand { get; set; }
    public string? Stedafstand { get; set; }
    public string? Bebyggelsesid { get; set; }
    public string? Bebyggelsestype { get; set; }
    public string? Adgangspunktid { get; set; }
    public string? VejpunktId { get; set; }
    public string? NavngivenvejId { get; set; }
    public Geometri? Geometri { get; set; }
    public string? Noformat { get; set; }
    public string? Side { get; set; }
    public string? PerSide { get; set; }
    //public string? Struktur { get; set; }

    public override Dictionary<string, string?> ToQueryParameters()
    {
        var dict = new Dictionary<string, string?>();

        if (Query is not null) dict["q"] = Query;
        if (AutoComplete is not null) dict["autocomplete"] = AutoComplete;
        if (Fuzzy is not null) dict["fuzzy"] = Fuzzy;
        if (KVH is not null) dict["kvh"] = KVH;
        if (Id is not null) dict["id"] = Id;
        if (Status is not null) dict["status"] = Status;
        if (Vejnavn is not null) dict["vejnavn"] = Vejnavn;
        if (HusNr is not null) dict["husnr"] = HusNr;
        if (HusNrFra is not null) dict["husnrfra"] = HusNrFra;
        if (HusNrTil is not null) dict["husnrtil"] = HusNrTil;
        if (PostNr is not null) dict["postnr"] = $"{PostNr}";
        if (Kommunekode is not null) dict["kommunekode"] = Kommunekode.Value.ToString("D4");
        if (MatrikelNr is not null) dict["matrikelnr"] = MatrikelNr;
        if (Ejerlavkode is not null) dict["ejerlavkode"] = $"{Ejerlavkode}";
        if (SRID is not null) dict["srid"] = $"{SRID}";
        if (Polygon is not null) dict["polygon"] = Polygon.ToPolygonString();
        if (Cirkel is not null) dict["cirkel"] = $"{Cirkel}";
        if (Nøjagtighed is not null) dict["nøjagtighed"] = Nøjagtighed;
        if (Regionskode is not null) dict["regionskode"] = Regionskode;
        if (Landsdelsnuts3 is not null) dict["landsdelsnuts3"] = Landsdelsnuts3;
        if (Sognekode is not null) dict["sognekode"] = Sognekode;
        if (Afstemningsområdenummer is not null) dict["afstemningsområdenummer"] = Afstemningsområdenummer;
        if (Opstillingskredskode is not null) dict["opstillingskredskode"] = Opstillingskredskode;
        if (Storkredsnummer is not null) dict["storkredsnummer"] = Storkredsnummer;
        if (Valglandsdelsbogstav is not null) dict["valglandsdelsbogstav"] = Valglandsdelsbogstav;
        if (Retskredskode is not null) dict["retskredskode"] = Retskredskode;
        if (Politikredskode is not null) dict["politikredskode"] = Politikredskode;
        if (Stednavnid is not null) dict["stednavnid"] = Stednavnid;
        if (Stedid is not null) dict["stedid"] = Stedid;
        if (Stednavnafstand is not null) dict["stednavnafstand"] = Stednavnafstand;
        if (Stedafstand is not null) dict["stedafstand"] = Stedafstand;
        if (Bebyggelsesid is not null) dict["bebyggelsesid"] = Bebyggelsesid;
        if (Bebyggelsestype is not null) dict["bebyggelsestype"] = Bebyggelsestype;
        if (Adgangspunktid is not null) dict["adgangspunktid"] = Adgangspunktid;
        if (VejpunktId is not null) dict["vejpunkt_id"] = VejpunktId;
        if (NavngivenvejId is not null) dict["navngivenvej_id"] = NavngivenvejId;
        if (Geometri is not null) dict["geometri"] = $"{Geometri}";
        if (Noformat is not null) dict["noformat"] = Noformat;
        if (Side is not null) dict["side"] = Side;
        if (PerSide is not null) dict["per_side"] = PerSide;
        //if (Struktur is not null) dict["struktur"] = Struktur;

        return dict;
    }
}