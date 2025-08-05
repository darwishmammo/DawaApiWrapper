using Dawa.Models;
using Dawa.Models.Parameters;
using System.Collections.Generic;

namespace Dawa.Services.DawaApiService;

public interface IJordstykkeEndpoint
{
    IAllFormatsRequestBuilder<List<Jordstykke>, GeoJSONAddress> Søge(JordstykkeQueryParams request);
    IAllFormatsRequestBuilder<Jordstykke, GeoJSONAddress> Opslag(JordstykkeOpslagQueryParams request);
    IJsonRequestBuilder<List<JordstykkeAutocompleteResponse>> Autocomplete(JordstykkeAutocompleteQueryParams request);
}