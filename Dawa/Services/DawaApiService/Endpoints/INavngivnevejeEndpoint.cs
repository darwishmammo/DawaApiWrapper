using Dawa.Models;
using Dawa.Models.Parameters;
using Dawa.Services.DawaApiService.ResponseFormats;
using System.Collections.Generic;

namespace Dawa.Services.DawaApiService.Endpoints;

public interface INavngivnevejeEndpoint
{
    IAllFormatsRequestBuilder<List<NavngivenVej>, GeoJSONAddress> Søge(NavngivenvejQueryParams queryParams);
    IAllFormatsRequestBuilder<NavngivenVej, GeoJSONAddress> Opslag(NavngivenvejOpslagQueryParams queryParams);
    IAllFormatsRequestBuilder<List<NavngivenVej>, GeoJSONAddress> Naboer(NavngivenvejsNaboerQueryParams queryParams);
}