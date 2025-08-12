using Dawa.Models;
using Dawa.Models.Parameters;
using Dawa.Services.DawaApiService.ResponseFormats;
using System.Collections.Generic;

namespace Dawa.Services.DawaApiService.Endpoints;

public interface IAutocompleteEndpoint
{
    IJsonRequestBuilder<List<AutocompleteResponse>> Søge(AutocompleteQueryParams queryParams);
}