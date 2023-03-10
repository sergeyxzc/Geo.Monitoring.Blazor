using System;
using System.Linq;
using Geo.Monitoring.Blazor.Components.Common;
using Geo.Monitoring.Blazor.Services.Geo;
using Geo.Monitoring.Blazor.Services;
using Microsoft.AspNetCore.Components;

namespace Geo.Monitoring.Blazor.Components.Projects;


public partial class ProjectsPage : BaseApplicationComponent
{
    public class ProjectViewModel
    {
        private readonly CompanyProjectDesc _projectDesc;

        public ProjectViewModel(CompanyProjectDesc projectDesc)
        {
            _projectDesc = projectDesc;
        }

        public int Id => _projectDesc.Id;
        public string Name => _projectDesc.Name;
        public string EmployeeCount => _projectDesc.EmployeeCount.ToString();
    }

    [Inject] public IUserService UserService { get; set; }
    [Inject] public IGeoServiceClient GeoService { get; set; }
    [Inject] public NavigationManager NavigationManager { get; set; }

    public bool CreateProjectVisible {get; set; }
    public bool Busy { get; set; }
    public List<ProjectViewModel> Projects { get; set; }

    private void OnCreateProject(CompanyProjectDesc companyProjectDesc)
    {
        Projects.Add(new ProjectViewModel(companyProjectDesc));
    }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            Busy = true;
            var projects = await GeoService.GetCompanyProjectsAsync(ComponentCancellationToken);
            Projects = projects.Projects.Select(x => new ProjectViewModel(x)).ToList();
        }
        finally
        {
            Busy = false;
        }
    }

    private void OnGoToProjectClick(ProjectViewModel vm)
    {
        if (vm == null)
            return;

        NavigationManager.NavigateTo($"/project/{vm.Id}");
    }
}