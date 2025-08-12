using Dawa.Models;
using Dawa.Models.Parameters;
using Dawa.Services.DawaApiService.ResponseFormats;
using System.Collections.Generic;

namespace Dawa.Services.DawaApiService.Endpoints;

public interface IAdgangsadresseEndpoint
{
    IAllFormatsRequestBuilder<List<Adgangsadresse>, GeoJSONAddress> Søge(AdresseQueryParams request);
    IAllFormatsRequestBuilder<Adgangsadresse, GeoJSONAddress> Opslag(AdresseOpslagQueryParams request);
    IJsonRequestBuilder<List<AdgangsadresseAutocompleteResponse>> Autocomplete(AdresseQueryParams request);
    IAllFormatsRequestBuilder<Adgangsadresse, GeoJSONAddress> ReverseGeocode(AdgangsadresseReverseGeocodeParams request);
}