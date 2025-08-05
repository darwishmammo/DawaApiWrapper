using Dawa.Models;
using Dawa.Models.Parameters;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Dawa.Services.DawaApiService;

public class DawaClient(HttpClient client, ILogger<DawaClient> logger) :
    IDawa,
    IAdgangsadresseEndpoint,
    IAdresseEndpoint,
    IPostnummerEndpoint,
    IJordstykkeEndpoint,
    IAutocompleteEndpoint
{
    private readonly HttpClient _client = client;
    private readonly ILogger<DawaClient> _logger = logger;
    private const string _adgangsadresser = "adgangsadresser";
    private const string _adresser = "adresser";
    private const string _postnumre = "postnumre";
    private const string _navngivneveje = "navngivneveje";
    private const string _jordstykker = "jordstykker";

    private static readonly JsonSerializerOptions _defaultJsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private static readonly JsonSerializerOptions _geoJsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters = { new GeometryJsonConverter() }
    };

    IAutocompleteEndpoint IDawa.Autocomplete => this;
    IAdresseEndpoint IDawa.Adresser => this;
    IAdgangsadresseEndpoint IDawa.Adgangsadresser => this;
    IPostnummerEndpoint IDawa.Postnumre => this;
    IJordstykkeEndpoint IDawa.Jordstykker => this;

    IAllFormatsRequestBuilder<List<Adgangsadresse>, GeoJSONAddress> IAdgangsadresseEndpoint.Søge(AdresseQueryParams request)
    {
        return new RequestBuilder<List<Adgangsadresse>, GeoJSONAddress>(
            jsonExecutor: (ct) => AdgangsadresseSøgningAsJson(request, ct),
            geoJsonExecutor: (ct) => AdgangsadresseSøgningAsGeoJson(request, ct),
            csvExecutor: (ct) => AdgangsadresseSøgningAsCsv(request, ct)
        );
    }

    IAllFormatsRequestBuilder<Adgangsadresse, GeoJSONAddress> IAdgangsadresseEndpoint.Opslag(AdresseOpslagQueryParams request)
    {
        return new RequestBuilder<Adgangsadresse, GeoJSONAddress>(
            jsonExecutor: (ct) => AdgangsadresseOpslagAsJson(request, ct),
            geoJsonExecutor: (ct) => AdgangsadresseOpslagAsGeoJson(request, ct),
            csvExecutor: (ct) => AdgangsadresseOpslagAsCsv(request, ct)
        );
    }

    IJsonRequestBuilder<List<AdgangsadresseAutocompleteResponse>> IAdgangsadresseEndpoint.Autocomplete(AdresseQueryParams request)
    {
        return new RequestBuilder<List<AdgangsadresseAutocompleteResponse>, object>(
            jsonExecutor: (ct) => AdgangsadresseAutocompleteAsJson(request, ct)
        );
    }

    IAllFormatsRequestBuilder<Adgangsadresse, GeoJSONAddress> IAdgangsadresseEndpoint.ReverseGeocode(AdgangsadresseReverseGeocodeParams request)
    {
        return new RequestBuilder<Adgangsadresse, GeoJSONAddress>(
            jsonExecutor: (ct) => AdgangsadresseReverseGeocodeAsJson(request, ct),
            geoJsonExecutor: (ct) => AdgangsadresseReverseGeocodeAsGeoJson(request, ct),
            csvExecutor: (ct) => AdgangsadresseReverseGeocodeAsCsv(request, ct)
        );
    }

    IAllFormatsRequestBuilder<List<Adresse>, GeoJSONAddress> IAdresseEndpoint.Søge(AdresseQueryParams request)
    {
        return new RequestBuilder<List<Adresse>, GeoJSONAddress>(
            jsonExecutor: (ct) => AdresseSøgningAsJson(request, ct),
            geoJsonExecutor: (ct) => AdresseSøgningAsGeoJson(request, ct),
            csvExecutor: (ct) => AdresseSøgningAsCsv(request, ct)
        );
    }

    IAllFormatsRequestBuilder<Adresse, GeoJSONAddress> IAdresseEndpoint.Opslag(AdresseOpslagQueryParams request)
    {
        return new RequestBuilder<Adresse, GeoJSONAddress>(
            jsonExecutor: (ct) => AdresseOpslagAsJson(request, ct),
            geoJsonExecutor: (ct) => AdresseOpslagAsGeoJson(request, ct),
            csvExecutor: (ct) => AdresseOpslagAsCsv(request, ct)
        );
    }

    IJsonRequestBuilder<List<AdresseAutocompleteResponse>> IAdresseEndpoint.Autocomplete(AdresseQueryParams request)
    {
        return new RequestBuilder<List<AdresseAutocompleteResponse>, object>(
            jsonExecutor: (ct) => AdresseAutocompleteAsJson(request, ct)
        );
    }

    IAllFormatsRequestBuilder<List<Postnummer>, GeoJSONAddress> IPostnummerEndpoint.Søge(PostnummerQueryParams request)
    {
        return new RequestBuilder<List<Postnummer>, GeoJSONAddress>(
            jsonExecutor: (ct) => PostnummersøgningAsJson(request, ct),
            geoJsonExecutor: (ct) => PostnummersøgningAsGeoJson(request, ct),
            csvExecutor: (ct) => PostnummersøgningAsCsv(request, ct)
        );
    }

    IAllFormatsRequestBuilder<Postnummer, GeoJSONAddress> IPostnummerEndpoint.Opslag(PostnummerOpslagQueryParams request)
    {
        return new RequestBuilder<Postnummer, GeoJSONAddress>(
            jsonExecutor: (ct) => PostnummerOpslagAsJson(request, ct),
            geoJsonExecutor: (ct) => PostnummerOpslagAsGeoJson(request, ct),
            csvExecutor: (ct) => PostnummerOpslagAsCsv(request, ct)
        );
    }

    IJsonRequestBuilder<List<PostnummerAutocompleteResponse>> IPostnummerEndpoint.Autocomplete(PostnummerAutocompleteQueryParams request)
    {
        return new RequestBuilder<List<PostnummerAutocompleteResponse>, object>(
            jsonExecutor: (ct) => PostnummerAutocompleteAsJson(request, ct)
        );
    }

    IAllFormatsRequestBuilder<Postnummer, GeoJSONAddress> IPostnummerEndpoint.ReverseGeocode(PostnummerReverseGeoCodeQueryParams request)
    {
        return new RequestBuilder<Postnummer, GeoJSONAddress>(
            jsonExecutor: (ct) => PostnummerReverseGeocodeAsJson(request, ct),
            geoJsonExecutor: (ct) => PostnummerReverseGeocodeAsGeoJson(request, ct),
            csvExecutor: (ct) => PostnummerReverseGeocodeAsCsv(request, ct)
        );
    }

    IAllFormatsRequestBuilder<List<Jordstykke>, GeoJSONAddress> IJordstykkeEndpoint.Søge(JordstykkeQueryParams request)
    {
        return new RequestBuilder<List<Jordstykke>, GeoJSONAddress>(
            jsonExecutor: (ct) => JordstykkeSøgningAsJson(request, ct),
            geoJsonExecutor: (ct) => JordstykkeSøgningAsGeoJson(request, ct),
            csvExecutor: (ct) => JordstykkeSøgningAsCsv(request, ct)
        );
    }

    IAllFormatsRequestBuilder<Jordstykke, GeoJSONAddress> IJordstykkeEndpoint.Opslag(JordstykkeOpslagQueryParams request)
    {
        return new RequestBuilder<Jordstykke, GeoJSONAddress>(
            jsonExecutor: (ct) => JordstykkeOpslagAsJson(request, ct),
            geoJsonExecutor: (ct) => JordstykkeOpslagAsGeoJson(request, ct),
            csvExecutor: (ct) => JordstykkeOpslagAsCsv(request, ct)
        );
    }

    IJsonRequestBuilder<List<JordstykkeAutocompleteResponse>> IJordstykkeEndpoint.Autocomplete(JordstykkeAutocompleteQueryParams request)
    {
        return new RequestBuilder<List<JordstykkeAutocompleteResponse>, object>(
            jsonExecutor: (ct) => JordstykkeAutocompleteAsJson(request, ct)
        );
    }

    IJsonRequestBuilder<List<AutocompleteResponse>> IAutocompleteEndpoint.Søge(AutocompleteQueryParams request)
    {
        var queryParameters = new Dictionary<string, string>(request.ToQueryParameters()!) { [DawaFormats.FormatKey] = DawaFormats.Json.Value };
        var url = QueryHelpers.AddQueryString("autocomplete", queryParameters!);

        return new RequestBuilder<List<AutocompleteResponse>, object>(
            jsonExecutor: (ct) => _client.GetFromJsonAsync<List<AutocompleteResponse>>(url, _defaultJsonOptions, ct)
        );
    }

    private async Task<List<Adgangsadresse>?> AdgangsadresseSøgningAsJson(AdresseQueryParams request, CancellationToken ct)
    {
        var queryParameters = new Dictionary<string, string>(request.ToQueryParameters()!) { [DawaFormats.FormatKey] = DawaFormats.Json.Value };
        var url = QueryHelpers.AddQueryString(_adgangsadresser, queryParameters!);

        return await GetAsJsonAsync<List<Adgangsadresse>>(url, ct);
    }

    private async Task<GeoJSONAddress?> AdgangsadresseSøgningAsGeoJson(AdresseQueryParams request, CancellationToken ct)
    {
        var queryParameters = new Dictionary<string, string>(request.ToQueryParameters()!) { [DawaFormats.FormatKey] = DawaFormats.GeoJson.Value };
        var url = QueryHelpers.AddQueryString(_adgangsadresser, queryParameters!);

        return await GetAsGeoJsonAddressAsync(url, ct);
    }

    private async Task<byte[]?> AdgangsadresseSøgningAsCsv(AdresseQueryParams request, CancellationToken ct)
    {
        var queryParameters = new Dictionary<string, string>(request.ToQueryParameters()!) { [DawaFormats.FormatKey] = DawaFormats.Csv.Value };
        var url = QueryHelpers.AddQueryString(_adgangsadresser, queryParameters!);

        return await GetAsByteArrayAsync(url, ct);
    }

    private async Task<Adgangsadresse?> AdgangsadresseOpslagAsJson(AdresseOpslagQueryParams request, CancellationToken ct)
    {
        var queryParameters = new Dictionary<string, string>(request.ToQueryParameters()!) { [DawaFormats.FormatKey] = DawaFormats.Json.Value };
        var url = QueryHelpers.AddQueryString($"{_adgangsadresser}/{request.Id}", queryParameters!);

        return await GetAsJsonAsync<Adgangsadresse>(url, ct);
    }

    private async Task<GeoJSONAddress?> AdgangsadresseOpslagAsGeoJson(AdresseOpslagQueryParams request, CancellationToken ct)
    {
        var queryParameters = new Dictionary<string, string>(request.ToQueryParameters()!) { [DawaFormats.FormatKey] = DawaFormats.GeoJson.Value };
        var url = QueryHelpers.AddQueryString($"{_adgangsadresser}/{request.Id}", queryParameters!);

        return await GetAsGeoJsonAddressAsync(url, ct);
    }

    private async Task<byte[]?> AdgangsadresseOpslagAsCsv(AdresseOpslagQueryParams request, CancellationToken ct)
    {
        var queryParameters = new Dictionary<string, string>(request.ToQueryParameters()!) { [DawaFormats.FormatKey] = DawaFormats.Csv.Value };
        var url = QueryHelpers.AddQueryString($"{_adgangsadresser}/{request.Id}", queryParameters!);

        return await GetAsByteArrayAsync(url, ct);
    }

    private async Task<List<AdgangsadresseAutocompleteResponse>?> AdgangsadresseAutocompleteAsJson(AdresseQueryParams request, CancellationToken ct)
    {
        var queryParameters = new Dictionary<string, string>(request.ToQueryParameters()!) { [DawaFormats.FormatKey] = DawaFormats.Json.Value };
        var url = QueryHelpers.AddQueryString($"{_adgangsadresser}/autocomplete", queryParameters!);

        return await GetAsJsonAsync<List<AdgangsadresseAutocompleteResponse>>(url, ct);
    }

    private async Task<Adgangsadresse?> AdgangsadresseReverseGeocodeAsJson(AdgangsadresseReverseGeocodeParams request, CancellationToken ct)
    {
        var queryParameters = new Dictionary<string, string>(request.ToQueryParameters()!) { [DawaFormats.FormatKey] = DawaFormats.Json.Value };
        var url = QueryHelpers.AddQueryString($"{_adgangsadresser}/reverse", queryParameters!);

        return await GetAsJsonAsync<Adgangsadresse>(url, ct);
    }

    private async Task<GeoJSONAddress?> AdgangsadresseReverseGeocodeAsGeoJson(AdgangsadresseReverseGeocodeParams request, CancellationToken ct)
    {
        var queryParameters = new Dictionary<string, string>(request.ToQueryParameters()!) { [DawaFormats.FormatKey] = DawaFormats.GeoJson.Value };
        var url = QueryHelpers.AddQueryString($"{_adgangsadresser}/reverse", queryParameters!);

        return await GetAsGeoJsonAddressAsync(url, ct);
    }

    private async Task<byte[]?> AdgangsadresseReverseGeocodeAsCsv(AdgangsadresseReverseGeocodeParams request, CancellationToken ct)
    {
        var queryParameters = new Dictionary<string, string>(request.ToQueryParameters()!) { [DawaFormats.FormatKey] = DawaFormats.Csv.Value };
        var url = QueryHelpers.AddQueryString($"{_adgangsadresser}/reverse", queryParameters!);

        return await GetAsByteArrayAsync(url, ct);
    }

    private async Task<List<Adresse>?> AdresseSøgningAsJson(AdresseQueryParams request, CancellationToken ct)
    {
        var queryParameters = new Dictionary<string, string>(request.ToQueryParameters()!) { [DawaFormats.FormatKey] = DawaFormats.Json.Value };
        var url = QueryHelpers.AddQueryString(_adresser, queryParameters!);

        return await GetAsJsonAsync<List<Adresse>>(url, ct);
    }

    private async Task<GeoJSONAddress?> AdresseSøgningAsGeoJson(AdresseQueryParams request, CancellationToken ct)
    {
        var queryParameters = new Dictionary<string, string>(request.ToQueryParameters()!) { [DawaFormats.FormatKey] = DawaFormats.GeoJson.Value };
        var url = QueryHelpers.AddQueryString(_adresser, queryParameters!);

        return await GetAsGeoJsonAddressAsync(url, ct);
    }

    private async Task<byte[]?> AdresseSøgningAsCsv(AdresseQueryParams request, CancellationToken ct)
    {
        var queryParameters = new Dictionary<string, string>(request.ToQueryParameters()!) { [DawaFormats.FormatKey] = DawaFormats.Csv.Value };
        var url = QueryHelpers.AddQueryString(_adresser, queryParameters!);

        return await GetAsByteArrayAsync(url, ct);
    }

    private async Task<Adresse?> AdresseOpslagAsJson(AdresseOpslagQueryParams request, CancellationToken ct)
    {
        var queryParameters = new Dictionary<string, string>(request.ToQueryParameters()!) { [DawaFormats.FormatKey] = DawaFormats.Json.Value };
        var url = QueryHelpers.AddQueryString($"{_adresser}/{request.Id}", queryParameters!);

        return await GetAsJsonAsync<Adresse>(url, ct);
    }

    private async Task<GeoJSONAddress?> AdresseOpslagAsGeoJson(AdresseOpslagQueryParams request, CancellationToken ct)
    {
        var queryParameters = new Dictionary<string, string>(request.ToQueryParameters()!) { [DawaFormats.FormatKey] = DawaFormats.GeoJson.Value };
        var url = QueryHelpers.AddQueryString($"{_adresser}/{request.Id}", queryParameters!);

        return await GetAsGeoJsonAddressAsync(url, ct);
    }

    private async Task<byte[]?> AdresseOpslagAsCsv(AdresseOpslagQueryParams request, CancellationToken ct)
    {
        var queryParameters = new Dictionary<string, string>(request.ToQueryParameters()!) { [DawaFormats.FormatKey] = DawaFormats.Csv.Value };
        var url = QueryHelpers.AddQueryString($"{_adresser}/{request.Id}", queryParameters!);

        return await GetAsByteArrayAsync(url, ct);
    }

    private async Task<List<AdresseAutocompleteResponse>?> AdresseAutocompleteAsJson(AdresseQueryParams request, CancellationToken ct)
    {
        var queryParameters = new Dictionary<string, string>(request.ToQueryParameters()!) { [DawaFormats.FormatKey] = DawaFormats.Json.Value };
        var url = QueryHelpers.AddQueryString($"{_adresser}/autocomplete", queryParameters!);

        return await GetAsJsonAsync<List<AdresseAutocompleteResponse>>(url, ct);
    }

    private async Task<List<Postnummer>?> PostnummersøgningAsJson(PostnummerQueryParams request, CancellationToken ct)
    {
        var queryParameters = new Dictionary<string, string>(request.ToQueryParameters()!) { [DawaFormats.FormatKey] = DawaFormats.Json.Value };
        var url = QueryHelpers.AddQueryString(_postnumre, queryParameters!);

        return await GetAsJsonAsync<List<Postnummer>>(url, ct);
    }

    private async Task<GeoJSONAddress?> PostnummersøgningAsGeoJson(PostnummerQueryParams request, CancellationToken ct)
    {
        var queryParameters = new Dictionary<string, string>(request.ToQueryParameters()!) { [DawaFormats.FormatKey] = DawaFormats.GeoJson.Value };
        var url = QueryHelpers.AddQueryString(_postnumre, queryParameters!);

        return await GetAsGeoJsonAddressAsync(url, ct);
    }

    private async Task<byte[]?> PostnummersøgningAsCsv(PostnummerQueryParams request, CancellationToken ct)
    {
        var queryParameters = new Dictionary<string, string>(request.ToQueryParameters()!) { [DawaFormats.FormatKey] = DawaFormats.Csv.Value };
        var url = QueryHelpers.AddQueryString(_postnumre, queryParameters!);

        return await GetAsByteArrayAsync(url, ct);
    }

    private async Task<Postnummer?> PostnummerOpslagAsJson(PostnummerOpslagQueryParams request, CancellationToken ct)
    {
        var queryParameters = new Dictionary<string, string>(request.ToQueryParameters()!) { [DawaFormats.FormatKey] = DawaFormats.Json.Value };
        var url = QueryHelpers.AddQueryString($"{_postnumre}/{request.Nr}", queryParameters!);

        return await GetAsJsonAsync<Postnummer>(url, ct);
    }

    private async Task<GeoJSONAddress?> PostnummerOpslagAsGeoJson(PostnummerOpslagQueryParams request, CancellationToken ct)
    {
        var queryParameters = new Dictionary<string, string>(request.ToQueryParameters()!) { [DawaFormats.FormatKey] = DawaFormats.GeoJson.Value };
        var url = QueryHelpers.AddQueryString($"{_postnumre}/{request.Nr}", queryParameters!);

        return await GetAsGeoJsonAddressAsync(url, ct);
    }

    private async Task<byte[]?> PostnummerOpslagAsCsv(PostnummerOpslagQueryParams request, CancellationToken ct)
    {
        var queryParameters = new Dictionary<string, string>(request.ToQueryParameters()!) { [DawaFormats.FormatKey] = DawaFormats.Csv.Value };
        var url = QueryHelpers.AddQueryString($"{_postnumre}/{request.Nr}", queryParameters!);

        return await GetAsByteArrayAsync(url, ct);
    }

    private async Task<List<PostnummerAutocompleteResponse>?> PostnummerAutocompleteAsJson(PostnummerAutocompleteQueryParams request, CancellationToken ct)
    {
        var queryParameters = new Dictionary<string, string>(request.ToQueryParameters()!) { [DawaFormats.FormatKey] = DawaFormats.Json.Value };
        var url = QueryHelpers.AddQueryString($"{_postnumre}/autocomplete", queryParameters!);

        return await GetAsJsonAsync<List<PostnummerAutocompleteResponse>>(url, ct);
    }

    private async Task<Postnummer?> PostnummerReverseGeocodeAsJson(PostnummerReverseGeoCodeQueryParams request, CancellationToken ct)
    {
        var queryParameters = new Dictionary<string, string>(request.ToQueryParameters()!) { [DawaFormats.FormatKey] = DawaFormats.Json.Value };
        var url = QueryHelpers.AddQueryString($"{_postnumre}/reverse", queryParameters!);

        return await GetAsJsonAsync<Postnummer>(url, ct);
    }

    private async Task<GeoJSONAddress?> PostnummerReverseGeocodeAsGeoJson(PostnummerReverseGeoCodeQueryParams request, CancellationToken ct)
    {
        var queryParameters = new Dictionary<string, string>(request.ToQueryParameters()!) { [DawaFormats.FormatKey] = DawaFormats.GeoJson.Value };
        var url = QueryHelpers.AddQueryString($"{_postnumre}/reverse", queryParameters!);

        return await GetAsGeoJsonAddressAsync(url, ct);
    }

    private async Task<byte[]?> PostnummerReverseGeocodeAsCsv(PostnummerReverseGeoCodeQueryParams request, CancellationToken ct)
    {
        var queryParameters = new Dictionary<string, string>(request.ToQueryParameters()!) { [DawaFormats.FormatKey] = DawaFormats.Csv.Value };
        var url = QueryHelpers.AddQueryString($"{_postnumre}/reverse", queryParameters!);

        return await GetAsByteArrayAsync(url, ct);
    }

    private async Task<List<Jordstykke>?> JordstykkeSøgningAsJson(JordstykkeQueryParams request, CancellationToken ct)
    {
        var queryParameters = new Dictionary<string, string>(request.ToQueryParameters()!) { [DawaFormats.FormatKey] = DawaFormats.Json.Value };
        var url = QueryHelpers.AddQueryString(_jordstykker, queryParameters!);

        return await GetAsJsonAsync<List<Jordstykke>>(url, ct);
    }

    private async Task<GeoJSONAddress?> JordstykkeSøgningAsGeoJson(JordstykkeQueryParams request, CancellationToken ct)
    {
        var queryParameters = new Dictionary<string, string>(request.ToQueryParameters()!) { [DawaFormats.FormatKey] = DawaFormats.GeoJson.Value };
        var url = QueryHelpers.AddQueryString(_jordstykker, queryParameters!);

        return await GetAsGeoJsonAddressAsync(url, ct);
    }

    private async Task<byte[]?> JordstykkeSøgningAsCsv(JordstykkeQueryParams request, CancellationToken ct)
    {
        var queryParameters = new Dictionary<string, string>(request.ToQueryParameters()!) { [DawaFormats.FormatKey] = DawaFormats.Csv.Value };
        var url = QueryHelpers.AddQueryString(_jordstykker, queryParameters!);

        return await GetAsByteArrayAsync(url, ct);
    }

    private async Task<Jordstykke?> JordstykkeOpslagAsJson(JordstykkeOpslagQueryParams request, CancellationToken ct)
    {
        var queryParameters = new Dictionary<string, string>(request.ToQueryParameters()!) { [DawaFormats.FormatKey] = DawaFormats.Json.Value };
        var url = QueryHelpers.AddQueryString($"{_jordstykker}/{request.Ejerlavkode}/{request.Matrikelnr}", queryParameters!);

        return await GetAsJsonAsync<Jordstykke>(url, ct);
    }

    private async Task<GeoJSONAddress?> JordstykkeOpslagAsGeoJson(JordstykkeOpslagQueryParams request, CancellationToken ct)
    {
        var queryParameters = new Dictionary<string, string>(request.ToQueryParameters()!) { [DawaFormats.FormatKey] = DawaFormats.GeoJson.Value };
        var url = QueryHelpers.AddQueryString($"{_jordstykker}/{request.Ejerlavkode}/{request.Matrikelnr}", queryParameters!);

        return await GetAsGeoJsonAddressAsync(url, ct);
    }

    private async Task<byte[]?> JordstykkeOpslagAsCsv(JordstykkeOpslagQueryParams request, CancellationToken ct)
    {
        var queryParameters = new Dictionary<string, string>(request.ToQueryParameters()!) { [DawaFormats.FormatKey] = DawaFormats.Csv.Value };
        var url = QueryHelpers.AddQueryString($"{_jordstykker}/{request.Ejerlavkode}/{request.Matrikelnr}", queryParameters!);

        return await GetAsByteArrayAsync(url, ct);
    }

    private async Task<List<JordstykkeAutocompleteResponse>?> JordstykkeAutocompleteAsJson(JordstykkeAutocompleteQueryParams request, CancellationToken ct)
    {
        var queryParameters = new Dictionary<string, string>(request.ToQueryParameters()!) { [DawaFormats.FormatKey] = DawaFormats.Json.Value };
        var url = QueryHelpers.AddQueryString($"{_jordstykker}/autocomplete", queryParameters!);

        return await GetAsJsonAsync<List<JordstykkeAutocompleteResponse>>(url, ct);
    }

    private async Task<T?> GetAsJsonAsync<T>(string url, CancellationToken ct = default)
    {
        try
        {
            return await _client.GetFromJsonAsync<T>(url, _defaultJsonOptions, ct);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP request failed for URL: {Url}", url);
            throw;
        }
    }

    private async Task<byte[]> GetAsByteArrayAsync(string url, CancellationToken ct = default)
    {
        try
        {
            return await _client.GetByteArrayAsync(url);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP request failed for URL: {Url}", url);
            throw;
        }
    }

    private async Task<GeoJSONAddress?> GetAsGeoJsonAddressAsync(string url, CancellationToken ct = default)
    {
        try
        {
            return await _client.GetFromJsonAsync<GeoJSONAddress>(url, _geoJsonOptions, ct);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP request failed for URL: {Url}", url);
            throw;
        }
    }
}