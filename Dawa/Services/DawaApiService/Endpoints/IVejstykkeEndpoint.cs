using Dawa.Models;
using Dawa.Models.Parameters;
using Dawa.Services.DawaApiService.ResponseFormats;
using System.Collections.Generic;

namespace Dawa.Services.DawaApiService.Endpoints;

public interface IVejstykkeEndpoint
{
    IAllFormatsRequestBuilder<List<Vejstykke>, GeoJSONAddress> Søge(VejstykkeQueryParams queryParams);
    IAllFormatsRequestBuilder<Vejstykke, GeoJSONAddress> Opslag(VejstykkeOpslagQueryParams queryParams);
    IJsonRequestBuilder<List<VejstykkeAutocompleteReponse>> Autocomplete(VejstykkerAutocompleteQueryParams queryParams);
    IAllFormatsRequestBuilder<Vejstykke, GeoJSONAddress> ReverseGeocode(VejstykkerReverseGeocodQueryParams queryParams);
    IAllFormatsRequestBuilder<List<Vejstykke>, GeoJSONAddress> Naboer(VejstykkeNaboerQueryParams queryParams);
}