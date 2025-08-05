using System.Collections.Generic;

namespace Dawa.Models.Parameters;

public class NavngivenvejQueryParams : BaseDawaRequest
{
    public string? Query { get; set; }
    public string? AutoComplete { get; set; }
    public string? Id { get; set; }
    public string? Navn { get; set; }
    public string? Postnummer { get; set; }
    public string? Kommune { get; set; }

    public override Dictionary<string, string?> ToQueryParameters()
    {
        var dict = new Dictionary<string, string?>();
        if (Query is not null) dict["q"] = Query;
        if (AutoComplete is not null) dict["autocomplete"] = AutoComplete;
        if (Id is not null) dict["id"] = Id;
        if (Navn is not null) dict["navn"] = Navn;
        if (Postnummer is not null) dict["postnr"] = Postnummer;
        if (Kommune is not null) dict["kommune"] = Kommune;

        return dict;
    }
}