using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;

namespace Geo.Monitoring.Blazor.Services;

public class ReValidatingIdentityAuthenticationStateProvider : RevalidatingServerAuthenticationStateProvider
{
    //private readonly ILoginUserService _loginUserService;

    public ReValidatingIdentityAuthenticationStateProvider(
        //ILoginUserService loginUserService,
        ILoggerFactory loggerFactory)
        : base(loggerFactory)
    {
        //_loginUserService = loginUserService;
    }

    protected override TimeSpan RevalidationInterval => TimeSpan.FromMinutes(30);

    protected override async Task<bool> ValidateAuthenticationStateAsync(AuthenticationState authenticationState, CancellationToken cancellationToken)
    {
        //var res = await _loginUserService.ValidateAuthenticationStateAsync(authenticationState.User, cancellationToken);
        return true;
    }
}