using Geo.Monitoring.Blazor.Components.Common;
using Geo.Monitoring.Blazor.Services;
using Geo.Monitoring.Blazor.Services.Geo;
using Microsoft.AspNetCore.Components;

namespace Geo.Monitoring.Blazor.Components.Dashboards;

public partial class DashboardPage : BaseBusyApplicationComponent
{
    [Inject] public IUserService UserService { get; set; }
    [Inject] public IGeoServiceClient GeoService { get; set; }
    public UserContext UserContext { get; set; }
    public CompanyDetails CompanyDetails { get; set; }

    protected override async Task DoOnInitializedAsync(MessageSubscriptionHolder messageSubscriptionHolder)
    {
        try
        {
            UserContext = await UserService.GetUserContextAsync(ComponentCancellationToken);
            CompanyDetails = await GeoService.GetCompanyInfoAsync(ComponentCancellationToken);
        }
        catch
        {
            // ignored
        }
    }
}