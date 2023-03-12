using Geo.Monitoring.Blazor.Components.Common;
using Geo.Monitoring.Blazor.Services;
using Geo.Monitoring.Blazor.Services.Geo;
using Microsoft.AspNetCore.Components;

namespace Geo.Monitoring.Blazor.Components.Projects;


public partial class ProjectPage : BaseBusyApplicationComponent
{
    private class ProjectViewModel
    {
        public CompanyProjectDesc Project { get; }
        public IReadOnlyList<LoggerDesc> Loggers { get; }

        public ProjectViewModel(CompanyProjectDesc project, IReadOnlyList<LoggerDesc> loggers)
        {
            Project = project;
            Loggers = loggers;
            ProjectDetails = new ProjectDetailsViewModel()
            {
                Name = project.Name,
                Description = project.Description
            };
        }

        public ProjectDetailsViewModel ProjectDetails { get; }
    }

    [Inject] public IUserService UserService { get; set; }
    [Inject] public IGeoServiceClient GeoService { get; set; }
    [Inject] public NavigationManager NavigationManager { get; set; }
    [Parameter] public int ProjectId { get; set; }
    private ProjectViewModel ViewModel { get; set; }

    protected override async Task DoOnInitializedAsync(MessageSubscriptionHolder messageSubscriptionHolder)
    {
        var project = await GeoService.GetProjectAsync(ProjectId, ComponentCancellationToken);
        ViewModel = new ProjectViewModel(project.Project, project.Loggers);
    }
}