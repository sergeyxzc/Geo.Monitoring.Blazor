namespace Geo.Monitoring.Blazor.Services;

public static class IdentityUserSchemes
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
}