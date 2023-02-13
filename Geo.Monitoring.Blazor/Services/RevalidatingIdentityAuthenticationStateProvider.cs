using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;

namespace Geo.Monitoring.Blazor.Services;

public class RevalidatingIdentityAuthenticationStateProvider
    : RevalidatingServerAuthenticationStateProvider
{
    private readonly IIdentityUserService _identityUserService;

    public RevalidatingIdentityAuthenticationStateProvider(
        IIdentityUserService identityUserService,
        ILoggerFactory loggerFactory)
        : base(loggerFactory)
    {
        _identityUserService = identityUserService;
    }

    protected override TimeSpan RevalidationInterval => TimeSpan.FromMinutes(30);

    protected override async Task<bool> ValidateAuthenticationStateAsync(AuthenticationState authenticationState, CancellationToken cancellationToken)
    {
        var res = await _identityUserService.ValidateAuthenticationStateAsync(authenticationState.User);
        return res;
    }
}