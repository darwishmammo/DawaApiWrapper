using System.Threading;
using System.Threading.Tasks;

namespace Dawa.Services.DawaApiService;

public interface IAllFormatsRequestBuilder<TJson, TGeoJson> : IGeoJsonRequestBuilder<TJson, TGeoJson>
{
    Task<byte[]?> AsCsvAsync(CancellationToken cancellationToken = default);
}