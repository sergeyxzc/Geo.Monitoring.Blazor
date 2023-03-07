using Geo.Monitoring.Blazor.Services.Geo;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace Geo.Monitoring.Blazor.Services;

public record PasswordSignInResult(bool Succeeded);

public interface ILoginUserService
{
    Task<PasswordSignInResult> PasswordSignInAsync(
        string login, string password, 
        bool rememberMe, bool lockoutOnFailure, 
        CancellationToken cancellationToken);

    public Task SignOutAsync();

    Task<bool> ValidateAuthenticationStateAsync(
        ClaimsPrincipal authenticationStateUser,
        CancellationToken cancellationToken);
}

public class LoginUserService : ILoginUserService
{
    private readonly IHttpContextAccessor _accessor;
    private readonly IGeoServiceClient _geoServiceClient;

    public LoginUserService(IHttpContextAccessor accessor, IGeoServiceClient geoServiceClient)
    {
        _accessor = accessor;
        _geoServiceClient = geoServiceClient;
    }

    public async Task<PasswordSignInResult> PasswordSignInAsync(string login, string password, bool rememberMe, bool lockoutOnFailure, CancellationToken cancellationToken)
    {
        if (_accessor.HttpContext == null)
            return new PasswordSignInResult(false);

        // Делаем проверку на сервере данных для пользователя
        var loginResponse = await _geoServiceClient.LoginAsync(new LoginRequest()
        {
            LoginName = login,
            Password = password,
        }, cancellationToken);

        if (!loginResponse.Successes)
        {
            return new PasswordSignInResult(loginResponse.Successes);
        }

        var claims = new List<Claim>()
        {
            new(ClaimTypes.Email, login),
            new(ClaimTypes.Expiration, DateTime.UtcNow.AddHours(1).ToString("O")),
            GeoUserPrincipalClaims.CreateCompanyClaim(loginResponse.CompanyId),
            GeoUserPrincipalClaims.CreateEmployeeClaim(loginResponse.EmployeeId)
        };

        var claimsIdentity = new ClaimsIdentity(claims, IdentityUserSchemes.ApplicationScheme);
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        var properties = new AuthenticationProperties()
        {
            AllowRefresh = true,
            IsPersistent = true
        };

        await _accessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, properties);
        return new PasswordSignInResult(true);
    }

    public async Task SignOutAsync()
    {
        if (_accessor.HttpContext == null)
            return;

        if (_accessor.HttpContext.User.IsSignedIn())
        {
            await _accessor.HttpContext.SignOutAsync();
        }
    }

    public Task<bool> ValidateAuthenticationStateAsync(ClaimsPrincipal authenticationStateUser, CancellationToken cancellationToken)
    {
        return Task.FromResult(true);
    }
}