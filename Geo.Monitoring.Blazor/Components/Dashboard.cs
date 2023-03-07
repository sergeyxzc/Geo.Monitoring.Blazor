using Geo.Monitoring.Blazor.Components.Common;
using Geo.Monitoring.Blazor.Services;
using Geo.Monitoring.Blazor.Services.Geo;
using Microsoft.AspNetCore.Components;

namespace Geo.Monitoring.Blazor.Components;

public partial class Dashboard : BaseApplicationComponent
{
    [Inject] public IUserService UserService { get; set; }
    [Inject] public IGeoServiceClient GeoService { get; set; }
    public UserContext UserContext { get; set; }
    public CompanyDetails CompanyDetails { get; set; }
    public bool Busy { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            Busy = true;
            UserContext = await UserService.GetUserContextAsync(ComponentCancellationToken);
            CompanyDetails = await GeoService.GetCompanyInfoAsync(ComponentCancellationToken);
        }
        finally
        {
            Busy = false;
        }
    }
}