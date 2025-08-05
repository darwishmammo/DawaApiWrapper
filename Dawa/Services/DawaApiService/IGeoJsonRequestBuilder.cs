using System.Threading;
using System.Threading.Tasks;

namespace Dawa.Services.DawaApiService;

public interface IGeoJsonRequestBuilder<TJson, TGeoJson> : IJsonRequestBuilder<TJson>
{
    Task<TGeoJson?> AsGeoJsonAsync(CancellationToken cancellationToken = default);
}
