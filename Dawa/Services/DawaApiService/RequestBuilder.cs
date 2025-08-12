using Dawa.Services.DawaApiService.ResponseFormats;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Dawa.Services.DawaApiService;

internal class RequestBuilder<TJson, TGeoJson> : IJsonCsvRequestBuilder<TJson>, IAllFormatsRequestBuilder<TJson, TGeoJson>
{
    private readonly Func<CancellationToken, Task<TJson?>> _jsonExecutor;
    private readonly Func<CancellationToken, Task<TGeoJson?>>? _geoJsonExecutor;
    private readonly Func<CancellationToken, Task<byte[]?>>? _csvExecutor;

    public RequestBuilder(
    Func<CancellationToken, Task<TJson?>> jsonExecutor)
    {
        _jsonExecutor = jsonExecutor;
    }

    public RequestBuilder(
    Func<CancellationToken, Task<TJson?>> jsonExecutor,
    Func<CancellationToken, Task<byte[]?>> csvExecutor)
    {
        _jsonExecutor = jsonExecutor;
        _csvExecutor = csvExecutor;
    }

    public RequestBuilder(
    Func<CancellationToken, Task<TJson?>> jsonExecutor,
    Func<CancellationToken, Task<TGeoJson?>> geoJsonExecutor,
    Func<CancellationToken, Task<byte[]?>> csvExecutor)
    {
        _jsonExecutor = jsonExecutor;
        _geoJsonExecutor = geoJsonExecutor;
        _csvExecutor = csvExecutor;
    }

    public Task<TJson?> AsJsonAsync(CancellationToken cancellationToken) => _jsonExecutor(cancellationToken);
    public Task<TGeoJson?> AsGeoJsonAsync(CancellationToken cancellationToken) => _geoJsonExecutor is not null ? _geoJsonExecutor(cancellationToken) : throw new NullReferenceException();
    public Task<byte[]?> AsCsvAsync(CancellationToken cancellationToken) => _csvExecutor is not null ? _csvExecutor(cancellationToken) : throw new NullReferenceException();
}