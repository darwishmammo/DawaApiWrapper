using System.Threading;
using System.Threading.Tasks;

namespace Dawa.Services.DawaApiService;

public interface IJsonRequestBuilder<TJson>
{
    Task<TJson?> AsJsonAsync(CancellationToken cancellationToken = default);
}
