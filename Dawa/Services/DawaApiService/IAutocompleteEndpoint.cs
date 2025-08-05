using Dawa.Models;
using Dawa.Models.Parameters;
using System.Collections.Generic;

namespace Dawa.Services.DawaApiService;

public interface IAutocompleteEndpoint
{
    IJsonRequestBuilder<List<AutocompleteResponse>> Søge(AutocompleteQueryParams queryParams);
}