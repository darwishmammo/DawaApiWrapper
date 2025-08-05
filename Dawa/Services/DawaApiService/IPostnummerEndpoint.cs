using Dawa.Models;
using Dawa.Models.Parameters;
using System.Collections.Generic;

namespace Dawa.Services.DawaApiService;

public interface IPostnummerEndpoint
{
    IAllFormatsRequestBuilder<List<Postnummer>, GeoJSONAddress> Søge(PostnummerQueryParams request);
    IAllFormatsRequestBuilder<Postnummer, GeoJSONAddress> Opslag(PostnummerOpslagQueryParams request);
    IJsonRequestBuilder<List<PostnummerAutocompleteResponse>> Autocomplete(PostnummerAutocompleteQueryParams request);
    IAllFormatsRequestBuilder<Postnummer, GeoJSONAddress> ReverseGeocode(PostnummerReverseGeoCodeQueryParams request);
}