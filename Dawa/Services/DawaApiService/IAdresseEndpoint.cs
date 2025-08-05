using Dawa.Models;
using Dawa.Models.Parameters;
using System.Collections.Generic;

namespace Dawa.Services.DawaApiService;

public interface IAdresseEndpoint
{
    IAllFormatsRequestBuilder<List<Adresse>, GeoJSONAddress> Søge(AdresseQueryParams request);
    IAllFormatsRequestBuilder<Adresse, GeoJSONAddress> Opslag(AdresseOpslagQueryParams request);
    IJsonRequestBuilder<List<AdresseAutocompleteResponse>> Autocomplete(AdresseQueryParams request);
}