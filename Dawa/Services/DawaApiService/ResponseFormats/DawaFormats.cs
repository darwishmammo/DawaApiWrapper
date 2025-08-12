using System.Collections.Generic;

namespace Dawa.Services.DawaApiService.ResponseFormats;

public static class DawaFormats
{
    public const string FormatKey = "format";

    public static readonly KeyValuePair<string, string> Json = new(FormatKey, "json");
    public static readonly KeyValuePair<string, string> GeoJson = new(FormatKey, "geojson");
    public static readonly KeyValuePair<string, string> Csv = new(FormatKey, "csv");
}