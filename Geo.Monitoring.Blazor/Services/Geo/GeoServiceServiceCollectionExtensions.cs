using System.Net.Http.Headers;
using System.Text;
using Microsoft.Extensions.Options;
using Refit;

namespace Geo.Monitoring.Blazor.Services.Geo;

public static class GeoServiceServiceCollectionExtensions
{
    public static IServiceCollection AddGeoServiceClientMock(this IServiceCollection services)
    {
        services.AddScoped<IGeoServiceClient, GeoServiceClientMock>();
        return services;
    }

    public static IServiceCollection AddGeoServiceOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<GeoServiceOptions>(configuration.GetSection("GeoService"));
        return services;
    }

    public static IServiceCollection AddGeoServiceClient(this IServiceCollection services,
        Func<IServiceProvider, RefitSettings> settingsAction = null,
        Action<HttpClient, GeoServiceOptions> afterConfigure = null,
        Action<IHttpClientBuilder> httpClientConfigure = null)
    {
        services.AddScoped<GeoAuthHeaderHandler>();

        var httpClientBuilder = services.AddRefitClient<IGeoServiceClient>(settingsAction)
            .ConfigureHttpClient((sp, httpClient) =>
            {
                var options = sp.GetService<IOptions<GeoServiceOptions>>()!.Value;

                httpClient.BaseAddress = new Uri(options.Address);

                if (options.AuthorizationEnabled)
                {
                    var authString = $"{options.Username}:{options.Password}";
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(authString)));
                }

                afterConfigure?.Invoke(httpClient, options);
            })
            .AddHttpMessageHandler<GeoAuthHeaderHandler>(); ;

        httpClientConfigure?.Invoke(httpClientBuilder);

        return services;
    }
}