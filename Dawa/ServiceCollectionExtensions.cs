using Dawa.Services.DawaApiService;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;

namespace Dawa;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDawa(this IServiceCollection services, Action<HttpClient> configureClient)
    {
        services.AddHttpClient<IDawa, DawaClient>(configureClient);

        return services;
    }

    public static IServiceCollection AddDawa(this IServiceCollection services, Uri baseAddress)
    {
        return AddDawa(services, client =>
        {
            client.BaseAddress = baseAddress;
        });
    }

    public static IServiceCollection AddDawa(this IServiceCollection services)
    {
        return AddDawa(services, new Uri("https://api.dataforsyningen.dk"));
    }
}
