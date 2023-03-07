using System.Security.Claims;

namespace Geo.Monitoring.Blazor.Services;

public static class IdentityUserExtensions
{
    public static bool IsSignedIn(this ClaimsPrincipal principal)
    {
        return principal != null && principal.Identities.Any(i => i.AuthenticationType == IdentityUserSchemes.ApplicationScheme);
    }
}