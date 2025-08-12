using Dawa.Models;
using Dawa.Models.Parameters;
using Dawa.Services.DawaApiService.Endpoints;
using Dawa.Services.DawaApiService.ResponseFormats;
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
    IAutocompleteEndpoint,
    INavngivnevejeEndpoint,
    IVejstykkeEndpoint
{
    private readonly HttpClient _client = client;
    private readonly ILogger<DawaClient> _logger = logger;
    private const string _adgangsadresser = "adgangsadresser";
    private const string _adresser = "adresser";
    private const string _postnumre = "postnumre";
    private const string _navngivneveje = "navngivneveje";
    private const string _jordstykker = "jordstykker";
    private const string _vejstykker = "vejstykker";

    private static readonly JsonSerializerOptions _defaultJsonOptions = new() { PropertyNameCaseInsensitive = true };

    IAutocompleteEndpoint IDawa.Autocomplete => this;
    IAdresseEndpoint IDawa.Adresser => this;
    IAdgangsadresseEndpoint IDawa.Adgangsadresser => this;
    IPostnummerEndpoint IDawa.Postnumre => this;
    IJordstykkeEndpoint IDawa.Jordstykker => this;
    INavngivnevejeEndpoint IDawa.Navngivneveje => this;
    IVejstykkeEndpoint IDawa.Vejstykker => this;

    IAllFormatsRequestBuilder<List<Adgangsadresse>, GeoJSONAddress> IAdgangsadresseEndpoint.Søge(AdresseQueryParams queryParams)
    {
        return new RequestBuilder<List<Adgangsadresse>, GeoJSONAddress>(
            jsonExecutor: (ct) => AdgangsadresseSøgningAsJson(queryParams, ct),
            geoJsonExecutor: (ct) => AdgangsadresseSøgningAsGeoJson(queryParams, ct),
            csvExecutor: (ct) => AdgangsadresseSøgningAsCsv(queryParams, ct)
        );
    }

    IAllFormatsRequestBuilder<Adgangsadresse, GeoJSONAddress> IAdgangsadresseEndpoint.Opslag(AdresseOpslagQueryParams queryParams)
    {
        return new RequestBuilder<Adgangsadresse, GeoJSONAddress>(
            jsonExecutor: (ct) => AdgangsadresseOpslagAsJson(queryParams, ct),
            geoJsonExecutor: (ct) => AdgangsadresseOpslagAsGeoJson(queryParams, ct),
            csvExecutor: (ct) => AdgangsadresseOpslagAsCsv(queryParams, ct)
        );
    }

    IJsonRequestBuilder<List<AdgangsadresseAutocompleteResponse>> IAdgangsadresseEndpoint.Autocomplete(AdresseQueryParams queryParams)
    {
        return new RequestBuilder<List<AdgangsadresseAutocompleteResponse>, object>(
            jsonExecutor: (ct) => AdgangsadresseAutocompleteAsJson(queryParams, ct)
        );
    }

    IAllFormatsRequestBuilder<Adgangsadresse, GeoJSONAddress> IAdgangsadresseEndpoint.ReverseGeocode(AdgangsadresseReverseGeocodeParams queryParams)
    {
        return new RequestBuilder<Adgangsadresse, GeoJSONAddress>(
            jsonExecutor: (ct) => AdgangsadresseReverseGeocodeAsJson(queryParams, ct),
            geoJsonExecutor: (ct) => AdgangsadresseReverseGeocodeAsGeoJson(queryParams, ct),
            csvExecutor: (ct) => AdgangsadresseReverseGeocodeAsCsv(queryParams, ct)
        );
    }

    IAllFormatsRequestBuilder<List<Adresse>, GeoJSONAddress> IAdresseEndpoint.Søge(AdresseQueryParams queryParams)
    {
        return new RequestBuilder<List<Adresse>, GeoJSONAddress>(
            jsonExecutor: (ct) => AdresseSøgningAsJson(queryParams, ct),
            geoJsonExecutor: (ct) => AdresseSøgningAsGeoJson(queryParams, ct),
            csvExecutor: (ct) => AdresseSøgningAsCsv(queryParams, ct)
        );
    }

    IAllFormatsRequestBuilder<Adresse, GeoJSONAddress> IAdresseEndpoint.Opslag(AdresseOpslagQueryParams queryParams)
    {
        return new RequestBuilder<Adresse, GeoJSONAddress>(
            jsonExecutor: (ct) => AdresseOpslagAsJson(queryParams, ct),
            geoJsonExecutor: (ct) => AdresseOpslagAsGeoJson(queryParams, ct),
            csvExecutor: (ct) => AdresseOpslagAsCsv(queryParams, ct)
        );
    }

    IJsonRequestBuilder<List<AdresseAutocompleteResponse>> IAdresseEndpoint.Autocomplete(AdresseQueryParams queryParams)
    {
        return new RequestBuilder<List<AdresseAutocompleteResponse>, object>(
            jsonExecutor: (ct) => AdresseAutocompleteAsJson(queryParams, ct)
        );
    }

    IJsonRequestBuilder<AdresseDatavaskResponse> IAdresseEndpoint.Datavask(string betegnelse)
    {
        return new RequestBuilder<AdresseDatavaskResponse, object>(
            jsonExecutor: (ct) => AdresseDatavask(betegnelse, ct)
        );
    }

    IAllFormatsRequestBuilder<List<Postnummer>, GeoJSONAddress> IPostnummerEndpoint.Søge(PostnummerQueryParams queryParams)
    {
        return new RequestBuilder<List<Postnummer>, GeoJSONAddress>(
            jsonExecutor: (ct) => PostnummersøgningAsJson(queryParams, ct),
            geoJsonExecutor: (ct) => PostnummersøgningAsGeoJson(queryParams, ct),
            csvExecutor: (ct) => PostnummersøgningAsCsv(queryParams, ct)
        );
    }

    IAllFormatsRequestBuilder<Postnummer, GeoJSONAddress> IPostnummerEndpoint.Opslag(PostnummerOpslagQueryParams queryParams)
    {
        return new RequestBuilder<Postnummer, GeoJSONAddress>(
            jsonExecutor: (ct) => PostnummerOpslagAsJson(queryParams, ct),
            geoJsonExecutor: (ct) => PostnummerOpslagAsGeoJson(queryParams, ct),
            csvExecutor: (ct) => PostnummerOpslagAsCsv(queryParams, ct)
        );
    }

    IJsonRequestBuilder<List<PostnummerAutocompleteResponse>> IPostnummerEndpoint.Autocomplete(PostnummerAutocompleteQueryParams queryParams)
    {
        return new RequestBuilder<List<PostnummerAutocompleteResponse>, object>(
            jsonExecutor: (ct) => PostnummerAutocompleteAsJson(queryParams, ct)
        );
    }

    IAllFormatsRequestBuilder<Postnummer, GeoJSONAddress> IPostnummerEndpoint.ReverseGeocode(PostnummerReverseGeoCodeQueryParams queryParams)
    {
        return new RequestBuilder<Postnummer, GeoJSONAddress>(
            jsonExecutor: (ct) => PostnummerReverseGeocodeAsJson(queryParams, ct),
            geoJsonExecutor: (ct) => PostnummerReverseGeocodeAsGeoJson(queryParams, ct),
            csvExecutor: (ct) => PostnummerReverseGeocodeAsCsv(queryParams, ct)
        );
    }

    IAllFormatsRequestBuilder<List<Jordstykke>, GeoJSONAddress> IJordstykkeEndpoint.Søge(JordstykkeQueryParams queryParams)
    {
        return new RequestBuilder<List<Jordstykke>, GeoJSONAddress>(
            jsonExecutor: (ct) => JordstykkeSøgningAsJson(queryParams, ct),
            geoJsonExecutor: (ct) => JordstykkeSøgningAsGeoJson(queryParams, ct),
            csvExecutor: (ct) => JordstykkeSøgningAsCsv(queryParams, ct)
        );
    }

    IAllFormatsRequestBuilder<Jordstykke, GeoJSONAddress> IJordstykkeEndpoint.Opslag(JordstykkeOpslagQueryParams queryParams)
    {
        return new RequestBuilder<Jordstykke, GeoJSONAddress>(
            jsonExecutor: (ct) => JordstykkeOpslagAsJson(queryParams, ct),
            geoJsonExecutor: (ct) => JordstykkeOpslagAsGeoJson(queryParams, ct),
            csvExecutor: (ct) => JordstykkeOpslagAsCsv(queryParams, ct)
        );
    }

    IJsonRequestBuilder<List<JordstykkeAutocompleteResponse>> IJordstykkeEndpoint.Autocomplete(JordstykkeAutocompleteQueryParams queryParams)
    {
        return new RequestBuilder<List<JordstykkeAutocompleteResponse>, object>(
            jsonExecutor: (ct) => JordstykkeAutocompleteAsJson(queryParams, ct)
        );
    }

    IJsonRequestBuilder<List<AutocompleteResponse>> IAutocompleteEndpoint.Søge(AutocompleteQueryParams queryParams)
    {
        return new RequestBuilder<List<AutocompleteResponse>, object>(
            jsonExecutor: (ct) => AutocompleteSøgningAsJson(queryParams, ct)
        );
    }

    IAllFormatsRequestBuilder<List<NavngivenVej>, GeoJSONAddress> INavngivnevejeEndpoint.Søge(NavngivenvejQueryParams queryParams)
    {
        return new RequestBuilder<List<NavngivenVej>, GeoJSONAddress>(
            jsonExecutor: (ct) => NavngivenvejSøgningAsJson(queryParams, ct),
            geoJsonExecutor: (ct) => NavngivenvejSøgningAsGeoJson(queryParams, ct),
            csvExecutor: (ct) => NavngivenvejSøgningAsCsv(queryParams, ct)
        );
    }

    IAllFormatsRequestBuilder<NavngivenVej, GeoJSONAddress> INavngivnevejeEndpoint.Opslag(NavngivenvejOpslagQueryParams queryParams)
    {
        return new RequestBuilder<NavngivenVej, GeoJSONAddress>(
            jsonExecutor: (ct) => NavngivenvejOpslagAsJson(queryParams, ct),
            geoJsonExecutor: (ct) => NavngivenvejOpslagAsGeoJson(queryParams, ct),
            csvExecutor: (ct) => NavngivenvejOpslagAsCsv(queryParams, ct)
        );
    }

    IAllFormatsRequestBuilder<List<NavngivenVej>, GeoJSONAddress> INavngivnevejeEndpoint.Naboer(NavngivenvejsNaboerQueryParams queryParams)
    {
        return new RequestBuilder<List<NavngivenVej>, GeoJSONAddress>(
            jsonExecutor: (ct) => NavngivenvejNaboerAsJson(queryParams, ct),
            geoJsonExecutor: (ct) => NavngivenvejNaboerAsGeoJson(queryParams, ct),
            csvExecutor: (ct) => NavngivenvejNaboerAsCsv(queryParams, ct)
        );
    }

    IAllFormatsRequestBuilder<List<Vejstykke>, GeoJSONAddress> IVejstykkeEndpoint.Søge(VejstykkeQueryParams queryParams)
    {
        return new RequestBuilder<List<Vejstykke>, GeoJSONAddress>(
            jsonExecutor: (ct) => VejstykkeSøgningAsJson(queryParams, ct),
            geoJsonExecutor: (ct) => VejstykkeSøgningAsGeoJson(queryParams, ct),
            csvExecutor: (ct) => VejstykkeSøgningAsCsv(queryParams, ct)
        );
    }

    IAllFormatsRequestBuilder<Vejstykke, GeoJSONAddress> IVejstykkeEndpoint.Opslag(VejstykkeOpslagQueryParams queryParams)
    {
        return new RequestBuilder<Vejstykke, GeoJSONAddress>(
            jsonExecutor: (ct) => VejstykkeOpslagAsJson(queryParams, ct),
            geoJsonExecutor: (ct) => VejstykkeOpslagAsGeoJson(queryParams, ct),
            csvExecutor: (ct) => VejstykkeOpslagAsCsv(queryParams, ct)
        );
    }

    IJsonRequestBuilder<List<VejstykkeAutocompleteReponse>> IVejstykkeEndpoint.Autocomplete(VejstykkerAutocompleteQueryParams queryParams)
    {
        return new RequestBuilder<List<VejstykkeAutocompleteReponse>, object>(
            jsonExecutor: (ct) => VejstykkeAutocompleteAsJson(queryParams, ct)
        );
    }

    IAllFormatsRequestBuilder<Vejstykke, GeoJSONAddress> IVejstykkeEndpoint.ReverseGeocode(VejstykkerReverseGeocodQueryParams queryParams)
    {
        return new RequestBuilder<Vejstykke, GeoJSONAddress>(
            jsonExecutor: (ct) => VejstykkeReverseGeocodeAsJson(queryParams, ct),
            geoJsonExecutor: (ct) => VejstykkeReverseGeocodeGeoJson(queryParams, ct),
            csvExecutor: (ct) => VejstykkeReverseGeocodeCsv(queryParams, ct)
        );
    }

    IAllFormatsRequestBuilder<List<Vejstykke>, GeoJSONAddress> IVejstykkeEndpoint.Naboer(VejstykkeNaboerQueryParams queryParams)
    {
        return new RequestBuilder<List<Vejstykke>, GeoJSONAddress>(
            jsonExecutor: (ct) => VejstykkeNaboerAsJson(queryParams, ct),
            geoJsonExecutor: (ct) => VejstykkeNaboerAsGeoJson(queryParams, ct),
            csvExecutor: (ct) => VejstykkeNaboerAsCsv(queryParams, ct)
        );
    }

    IJsonCsvRequestBuilder<List<AdresseHistorikResponse>> IAdresseEndpoint.Historik(AdresseHistorikQueryParams queryParams)
    {
        return new RequestBuilder<List<AdresseHistorikResponse>, object>(
            jsonExecutor: (ct) => AdresseHistorikAsJson(queryParams, ct),
            csvExecutor: (ct) => AdresseHistorikAsCsv(queryParams, ct)
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

    private async Task<AdresseDatavaskResponse?> AdresseDatavask(string betegnelse, CancellationToken ct)
    {
        var url = QueryHelpers.AddQueryString($"datavask/{_adresser}", new Dictionary<string, string> { ["betegnelse"] = betegnelse });
        return await _client.GetFromJsonAsync<AdresseDatavaskResponse>(url, _defaultJsonOptions, ct);
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

    private async Task<List<AutocompleteResponse>?> AutocompleteSøgningAsJson(AutocompleteQueryParams request, CancellationToken ct)
    {
        var queryParameters = new Dictionary<string, string>(request.ToQueryParameters()!) { [DawaFormats.FormatKey] = DawaFormats.Json.Value };
        var url = QueryHelpers.AddQueryString("autocomplete", queryParameters!);

        return await GetAsJsonAsync<List<AutocompleteResponse>>(url, ct);
    }

    private async Task<List<NavngivenVej>?> NavngivenvejSøgningAsJson(NavngivenvejQueryParams queryParams, CancellationToken ct)
    {
        var queryParameters = new Dictionary<string, string>(queryParams.ToQueryParameters()!) { [DawaFormats.FormatKey] = DawaFormats.Json.Value };
        var url = QueryHelpers.AddQueryString(_navngivneveje, queryParameters!);

        return await GetAsJsonAsync<List<NavngivenVej>>(url, ct);
    }

    private async Task<GeoJSONAddress?> NavngivenvejSøgningAsGeoJson(NavngivenvejQueryParams queryParams, CancellationToken ct)
    {
        var queryParameters = new Dictionary<string, string>(queryParams.ToQueryParameters()!) { [DawaFormats.FormatKey] = DawaFormats.GeoJson.Value };
        var url = QueryHelpers.AddQueryString(_navngivneveje, queryParameters!);

        return await GetAsGeoJsonAddressAsync(url, ct);
    }

    private async Task<byte[]?> NavngivenvejSøgningAsCsv(NavngivenvejQueryParams queryParams, CancellationToken ct)
    {
        var queryParameters = new Dictionary<string, string>(queryParams.ToQueryParameters()!) { [DawaFormats.FormatKey] = DawaFormats.Csv.Value };
        var url = QueryHelpers.AddQueryString(_navngivneveje, queryParameters!);

        return await GetAsByteArrayAsync(url, ct);
    }

    private async Task<NavngivenVej?> NavngivenvejOpslagAsJson(NavngivenvejOpslagQueryParams queryParams, CancellationToken ct)
    {
        var queryParameters = new Dictionary<string, string>(queryParams.ToQueryParameters()!) { [DawaFormats.FormatKey] = DawaFormats.Json.Value };
        var url = QueryHelpers.AddQueryString($"{_navngivneveje}/{queryParams.Id}", queryParameters!);

        return await GetAsJsonAsync<NavngivenVej>(url, ct);
    }

    private async Task<GeoJSONAddress?> NavngivenvejOpslagAsGeoJson(NavngivenvejOpslagQueryParams queryParams, CancellationToken ct)
    {
        var queryParameters = new Dictionary<string, string>(queryParams.ToQueryParameters()!) { [DawaFormats.FormatKey] = DawaFormats.GeoJson.Value };
        var url = QueryHelpers.AddQueryString($"{_navngivneveje}/{queryParams.Id}", queryParameters!);

        return await GetAsGeoJsonAddressAsync(url, ct);
    }

    private async Task<byte[]?> NavngivenvejOpslagAsCsv(NavngivenvejOpslagQueryParams queryParams, CancellationToken ct)
    {
        var queryParameters = new Dictionary<string, string>(queryParams.ToQueryParameters()!) { [DawaFormats.FormatKey] = DawaFormats.Csv.Value };
        var url = QueryHelpers.AddQueryString($"{_navngivneveje}/{queryParams.Id}", queryParameters!);

        return await GetAsByteArrayAsync(url, ct);
    }

    private async Task<List<NavngivenVej>?> NavngivenvejNaboerAsJson(NavngivenvejsNaboerQueryParams queryParams, CancellationToken ct)
    {
        var queryParameters = new Dictionary<string, string>(queryParams.ToQueryParameters()!) { [DawaFormats.FormatKey] = DawaFormats.Json.Value };
        var url = QueryHelpers.AddQueryString($"{_navngivneveje}/{queryParams.Id}/naboer", queryParameters!);

        return await GetAsJsonAsync<List<NavngivenVej>>(url, ct);
    }

    private async Task<GeoJSONAddress?> NavngivenvejNaboerAsGeoJson(NavngivenvejsNaboerQueryParams queryParams, CancellationToken ct)
    {
        var queryParameters = new Dictionary<string, string>(queryParams.ToQueryParameters()!) { [DawaFormats.FormatKey] = DawaFormats.GeoJson.Value };
        var url = QueryHelpers.AddQueryString($"{_navngivneveje}/{queryParams.Id}/naboer", queryParameters!);

        return await GetAsGeoJsonAddressAsync(url, ct);
    }

    private async Task<byte[]?> NavngivenvejNaboerAsCsv(NavngivenvejsNaboerQueryParams queryParams, CancellationToken ct)
    {
        var queryParameters = new Dictionary<string, string>(queryParams.ToQueryParameters()!) { [DawaFormats.FormatKey] = DawaFormats.Csv.Value };
        var url = QueryHelpers.AddQueryString($"{_navngivneveje}/{queryParams.Id}/naboer", queryParameters!);

        return await GetAsByteArrayAsync(url, ct);
    }

    private async Task<List<Vejstykke>?> VejstykkeSøgningAsJson(VejstykkeQueryParams queryParams, CancellationToken ct)
    {
        var queryParameters = new Dictionary<string, string>(queryParams.ToQueryParameters()!) { [DawaFormats.FormatKey] = DawaFormats.Json.Value };
        var url = QueryHelpers.AddQueryString(_vejstykker, queryParameters!);

        return await GetAsJsonAsync<List<Vejstykke>>(url, ct);
    }

    private async Task<GeoJSONAddress?> VejstykkeSøgningAsGeoJson(VejstykkeQueryParams queryParams, CancellationToken ct)
    {
        var queryParameters = new Dictionary<string, string>(queryParams.ToQueryParameters()!) { [DawaFormats.FormatKey] = DawaFormats.GeoJson.Value };
        var url = QueryHelpers.AddQueryString(_vejstykker, queryParameters!);

        return await GetAsGeoJsonAddressAsync(url, ct);
    }

    private async Task<byte[]?> VejstykkeSøgningAsCsv(VejstykkeQueryParams queryParams, CancellationToken ct)
    {
        var queryParameters = new Dictionary<string, string>(queryParams.ToQueryParameters()!) { [DawaFormats.FormatKey] = DawaFormats.Csv.Value };
        var url = QueryHelpers.AddQueryString(_vejstykker, queryParameters!);

        return await GetAsByteArrayAsync(url, ct);
    }

    private async Task<Vejstykke?> VejstykkeOpslagAsJson(VejstykkeOpslagQueryParams queryParams, CancellationToken ct)
    {
        var queryParameters = new Dictionary<string, string>(queryParams.ToQueryParameters()!) { [DawaFormats.FormatKey] = DawaFormats.Json.Value };
        var url = QueryHelpers.AddQueryString($"{_vejstykker}/{queryParams.Kommunekode}/{queryParams.Vejkode}", queryParameters!);

        return await GetAsJsonAsync<Vejstykke>(url, ct);
    }

    private async Task<GeoJSONAddress?> VejstykkeOpslagAsGeoJson(VejstykkeOpslagQueryParams queryParams, CancellationToken ct)
    {
        var queryParameters = new Dictionary<string, string>(queryParams.ToQueryParameters()!) { [DawaFormats.FormatKey] = DawaFormats.GeoJson.Value };
        var url = QueryHelpers.AddQueryString($"{_vejstykker}/{queryParams.Kommunekode}/{queryParams.Vejkode}", queryParameters!);

        return await GetAsGeoJsonAddressAsync(url, ct);
    }

    private async Task<byte[]?> VejstykkeOpslagAsCsv(VejstykkeOpslagQueryParams queryParams, CancellationToken ct)
    {
        var queryParameters = new Dictionary<string, string>(queryParams.ToQueryParameters()!) { [DawaFormats.FormatKey] = DawaFormats.Csv.Value };
        var url = QueryHelpers.AddQueryString($"{_vejstykker}/{queryParams.Kommunekode}/{queryParams.Vejkode}", queryParameters!);

        return await GetAsByteArrayAsync(url, ct);
    }

    private async Task<List<VejstykkeAutocompleteReponse>?> VejstykkeAutocompleteAsJson(VejstykkerAutocompleteQueryParams queryParams, CancellationToken ct)
    {
        var queryParameters = new Dictionary<string, string>(queryParams.ToQueryParameters()!) { [DawaFormats.FormatKey] = DawaFormats.Json.Value };
        var url = QueryHelpers.AddQueryString($"{_vejstykker}/autocomplete", queryParameters!);

        return await GetAsJsonAsync<List<VejstykkeAutocompleteReponse>>(url, ct);
    }

    private async Task<Vejstykke?> VejstykkeReverseGeocodeAsJson(VejstykkerReverseGeocodQueryParams queryParams, CancellationToken ct)
    {
        var queryParameters = new Dictionary<string, string>(queryParams.ToQueryParameters()!) { [DawaFormats.FormatKey] = DawaFormats.Json.Value };
        var url = QueryHelpers.AddQueryString($"{_vejstykker}/reverse", queryParameters!);

        return await GetAsJsonAsync<Vejstykke>(url, ct);
    }

    private async Task<GeoJSONAddress?> VejstykkeReverseGeocodeGeoJson(VejstykkerReverseGeocodQueryParams queryParams, CancellationToken ct)
    {
        var queryParameters = new Dictionary<string, string>(queryParams.ToQueryParameters()!) { [DawaFormats.FormatKey] = DawaFormats.GeoJson.Value };
        var url = QueryHelpers.AddQueryString($"{_vejstykker}/reverse", queryParameters!);

        return await GetAsGeoJsonAddressAsync(url, ct);
    }

    private async Task<byte[]?> VejstykkeReverseGeocodeCsv(VejstykkerReverseGeocodQueryParams queryParams, CancellationToken ct)
    {
        var queryParameters = new Dictionary<string, string>(queryParams.ToQueryParameters()!) { [DawaFormats.FormatKey] = DawaFormats.Csv.Value };
        var url = QueryHelpers.AddQueryString($"{_vejstykker}/reverse", queryParameters!);

        return await GetAsByteArrayAsync(url, ct);
    }

    private async Task<List<Vejstykke>?> VejstykkeNaboerAsJson(VejstykkeNaboerQueryParams queryParams, CancellationToken ct)
    {
        var queryParameters = new Dictionary<string, string>(queryParams.ToQueryParameters()!) { [DawaFormats.FormatKey] = DawaFormats.Json.Value };
        var url = QueryHelpers.AddQueryString($"{_vejstykker}/{queryParams.Kommunekode}/{queryParams.Vejkode}/naboer", queryParameters!);

        return await GetAsJsonAsync<List<Vejstykke>>(url, ct);
    }

    private async Task<GeoJSONAddress?> VejstykkeNaboerAsGeoJson(VejstykkeNaboerQueryParams queryParams, CancellationToken ct)
    {
        var queryParameters = new Dictionary<string, string>(queryParams.ToQueryParameters()!) { [DawaFormats.FormatKey] = DawaFormats.GeoJson.Value };
        var url = QueryHelpers.AddQueryString($"{_vejstykker}/{queryParams.Kommunekode}/{queryParams.Vejkode}/naboer", queryParameters!);

        return await GetAsGeoJsonAddressAsync(url, ct);
    }

    private async Task<byte[]?> VejstykkeNaboerAsCsv(VejstykkeNaboerQueryParams queryParams, CancellationToken ct)
    {
        var queryParameters = new Dictionary<string, string>(queryParams.ToQueryParameters()!) { [DawaFormats.FormatKey] = DawaFormats.Csv.Value };
        var url = QueryHelpers.AddQueryString($"{_vejstykker}/{queryParams.Kommunekode}/{queryParams.Vejkode}/naboer", queryParameters!);

        return await GetAsByteArrayAsync(url, ct);
    }

    private async Task<List<AdresseHistorikResponse>?> AdresseHistorikAsJson(AdresseHistorikQueryParams queryParams, CancellationToken ct)
    {
        var queryParameters = new Dictionary<string, string>(queryParams.ToQueryParameters()!) { [DawaFormats.FormatKey] = DawaFormats.Json.Value };
        var url = QueryHelpers.AddQueryString($"historik/{_adresser}", queryParameters!);

        return await GetAsJsonAsync<List<AdresseHistorikResponse>>(url, ct);
    }

    private async Task<byte[]?> AdresseHistorikAsCsv(AdresseHistorikQueryParams queryParams, CancellationToken ct)
    {
        var queryParameters = new Dictionary<string, string>(queryParams.ToQueryParameters()!) { [DawaFormats.FormatKey] = DawaFormats.Csv.Value };
        var url = QueryHelpers.AddQueryString($"historik/{_adresser}", queryParameters!);

        return await GetAsByteArrayAsync(url, ct);
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
            return await _client.GetFromJsonAsync<GeoJSONAddress>(url, _defaultJsonOptions, ct);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP request failed for URL: {Url}", url);
            throw;
        }
    }
}