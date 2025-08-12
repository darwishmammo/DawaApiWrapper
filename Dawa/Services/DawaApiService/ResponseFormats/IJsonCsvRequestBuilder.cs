using System.Threading;
using System.Threading.Tasks;

namespace Dawa.Services.DawaApiService.ResponseFormats;

public interface IJsonCsvRequestBuilder<TJson> : IJsonRequestBuilder<TJson>
{
    Task<byte[]?> AsCsvAsync(CancellationToken cancellationToken = default);
}