using System.Threading;
using System.Threading.Tasks;

namespace Dawa.Services.DawaApiService.ResponseFormats;

public interface IJsonRequestBuilder<TJson>
{
    Task<TJson?> AsJsonAsync(CancellationToken cancellationToken = default);
}