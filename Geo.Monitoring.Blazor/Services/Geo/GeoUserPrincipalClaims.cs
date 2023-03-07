using System.Security.Claims;

namespace Geo.Monitoring.Blazor.Services.Geo;

public static class GeoUserPrincipalClaims
{
    public static readonly string ClaimCompanyId = "CompanyId";
    public static readonly string ClaimEmployeeId = "EmployeeId";


    public static Claim CreateCompanyClaim(int companyId)
    {
        return new Claim(ClaimCompanyId, companyId.ToString("D"));
    }

    public static Claim CreateEmployeeClaim(int employeeId)
    {
        return new Claim(ClaimEmployeeId, employeeId.ToString("D"));
    }

    public static Claim FindCompanyClaim(IEnumerable<Claim> claims)
    {
        return claims.FirstOrDefault(x => x.Type == ClaimCompanyId);
    }

    public static Claim FindEmployeeClaim(IEnumerable<Claim> claims)
    {
        return claims.FirstOrDefault(x => x.Type == ClaimEmployeeId);
    }
}