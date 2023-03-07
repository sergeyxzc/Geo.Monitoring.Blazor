using System.Security.Claims;
using Geo.Monitoring.Blazor.Services.Geo;
using Microsoft.AspNetCore.Components.Authorization;

namespace Geo.Monitoring.Blazor.Services;

public record UserContext(string Login, string CompanyId, string EmployeeId);

public interface IUserService
{
    Task<UserContext> GetUserContextAsync(CancellationToken cancellationToken);
}

public class UserService : IUserService
{
    private readonly AuthenticationStateProvider _stateProvider;

    public UserService(AuthenticationStateProvider stateProvider)
    {
        _stateProvider = stateProvider;
    }

    public async Task<UserContext> GetUserContextAsync(CancellationToken cancellationToken)
    {
        var authenticationState = await _stateProvider.GetAuthenticationStateAsync();

        var claims = authenticationState.User.Claims;

        return new UserContext(
            claims.First(x => x.Type == ClaimTypes.Email)!.Value,
            GeoUserPrincipalClaims.FindCompanyClaim(claims).Value,
            GeoUserPrincipalClaims.FindEmployeeClaim(claims).Value);
    }
}