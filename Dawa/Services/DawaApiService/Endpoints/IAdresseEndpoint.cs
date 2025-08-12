using Dawa.Models;
using Dawa.Models.Parameters;
using Dawa.Services.DawaApiService.ResponseFormats;
using System.Collections.Generic;

namespace Dawa.Services.DawaApiService.Endpoints;

public interface IAdresseEndpoint
{
    IAllFormatsRequestBuilder<List<Adresse>, GeoJSONAddress> Søge(AdresseQueryParams request);
    IAllFormatsRequestBuilder<Adresse, GeoJSONAddress> Opslag(AdresseOpslagQueryParams request);
    IJsonRequestBuilder<List<AdresseAutocompleteResponse>> Autocomplete(AdresseQueryParams request);
    IJsonRequestBuilder<AdresseDatavaskResponse> Datavask(string betegnelse);
    IJsonCsvRequestBuilder<List<AdresseHistorikResponse>> Historik(AdresseHistorikQueryParams request);
}