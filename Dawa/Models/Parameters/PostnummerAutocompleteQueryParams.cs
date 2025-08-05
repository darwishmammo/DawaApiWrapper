using System.Collections.Generic;

namespace Dawa.Models.Parameters;

public class PostnummerAutocompleteQueryParams : BaseDawaRequest
{
    public string? Nr { get; set; }
    public string? Navn { get; set; }
    public int? Kommunekode { get; set; }
    public string? Query { get; set; }
    public string? Noformat { get; set; }
    public string? Side { get; set; }
    public string? PerSide { get; set; }

    public override Dictionary<string, string?> ToQueryParameters()
    {
        var dict = new Dictionary<string, string?>();

        if (Nr is not null) dict["nr"] = Nr;
        if (Navn is not null) dict["navn"] = Navn;
        if (Kommunekode is not null) dict["kommunekode"] = Kommunekode.Value.ToString("D4");
        if (Query is not null) dict["q"] = Query;
        if (Noformat is not null) dict["noformat"] = Noformat;
        if (Side is not null) dict["side"] = Side;
        if (PerSide is not null) dict["per_side"] = PerSide;

        return dict;
    }
}