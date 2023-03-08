using Microsoft.AspNetCore.Components.Authorization;

namespace Geo.Monitoring.Blazor.Services.Geo;

public class GeoAuthHeaderHandler : DelegatingHandler
{
    private readonly AuthenticationStateProvider _authenticationStateProvider;

    public GeoAuthHeaderHandler(AuthenticationStateProvider authenticationStateProvider)
    {
        _authenticationStateProvider = authenticationStateProvider;
        //InnerHandler = new HttpClientHandler();
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var authenticationState = await _authenticationStateProvider.GetAuthenticationStateAsync();

        if (authenticationState.User.IsSignedIn())
        {
            var companyClaim = GeoUserPrincipalClaims.FindCompanyClaim(authenticationState.User.Claims);
            var employeeClaim = GeoUserPrincipalClaims.FindEmployeeClaim(authenticationState.User.Claims);
            request.Headers.Add("X-Geo-Company-Id", companyClaim?.Value ?? string.Empty);
            request.Headers.Add("X-Geo-Employee-Id", employeeClaim?.Value ?? string.Empty);
        }

        return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
    }
}