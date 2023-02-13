using System.Security.Claims;

namespace Geo.Monitoring.Blazor.Services
{
    public record PasswordSignInResult(bool Succeeded);

    public interface IIdentityUserService
    {
        Task<PasswordSignInResult> PasswordSignInAsync(string login, string password, bool rememberMe, bool lockoutOnFailure);
        bool IsSignedIn(ClaimsPrincipal claimsPrincipal);
        Task SignOutAsync();
        Task<bool> ValidateAuthenticationStateAsync(ClaimsPrincipal authenticationStateUser);
    }
}
