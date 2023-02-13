using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Geo.Monitoring.Blazor.Services;

public class IdentityUserService : IIdentityUserService
{
    private const string CookiePrefix = "Geo.Monitoring";

    /// <summary>
    /// The scheme used to identify application authentication cookies.
    /// </summary>
    public static readonly string ApplicationScheme = CookiePrefix + ".Application";

    /// <summary>
    /// The scheme used to identify external authentication cookies.
    /// </summary>
    public static readonly string ExternalScheme = CookiePrefix + ".External";

    /// <summary>
    /// The scheme used to identify Two Factor authentication cookies for saving the Remember Me state.
    /// </summary>
    public static readonly string TwoFactorRememberMeScheme = CookiePrefix + ".TwoFactorRememberMe";

    /// <summary>
    /// The scheme used to identify Two Factor authentication cookies for round tripping user identities.
    /// </summary>
    public static readonly string TwoFactorUserIdScheme = CookiePrefix + ".TwoFactorUserId";


    private readonly IHttpContextAccessor _accessor;

    public IdentityUserService(IHttpContextAccessor accessor)
    {
        _accessor = accessor;
    }

    public async Task<PasswordSignInResult> PasswordSignInAsync(string login, string password, bool rememberMe, bool lockoutOnFailure)
    {
        // TODO Сделать запрос на сервис данных-пользователей и проверить там креды.

        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Email, login),
            new Claim(ClaimTypes.Name, "Sergey"),
            new Claim(ClaimTypes.Expiration, DateTime.UtcNow.AddHours(1).ToString("O"))
        };

        var claimsIdentity = new ClaimsIdentity(claims, ApplicationScheme);
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        var properties = new AuthenticationProperties()
        {
            AllowRefresh = true,
            IsPersistent = true
        };

        await _accessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, properties);
        return new PasswordSignInResult(true);
    }

    public bool IsSignedIn(ClaimsPrincipal principal)
    {
        if (principal == null)
        {
            throw new ArgumentNullException(nameof(principal));

        }
        return principal.Identities != null &&
               principal.Identities.Any(i => i.AuthenticationType == ApplicationScheme);
    }

    public async Task SignOutAsync()
    {
        await _accessor.HttpContext.SignOutAsync();
    }

    public async Task<bool> ValidateAuthenticationStateAsync(ClaimsPrincipal authenticationStateUser)
    {
        return true;
    }
}