using Geo.Monitoring.Blazor.Services;
using Microsoft.AspNetCore.Components;

namespace Geo.Monitoring.Blazor.Components
{
    public class GeoLoggerViewModel
    {
        private readonly Services.SensorLogger _logger;

        public GeoLoggerViewModel(Services.SensorLogger logger)
        {
            _logger = logger;
            Sensors = logger?.Sensors.Count ?? 0;
        }

        public int Id => _logger.Id;
        public string? Name => _logger.Name;
        public int Sensors { get; }
    }

    public partial class SensorLoggers
    {
        public bool Busy { get; set; }
        public IReadOnlyList<GeoLoggerViewModel> GeoLoggers { get; set; }
        [Inject] public IGeoService GeoService { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }


        protected override async Task OnInitializedAsync()
        {
            try
            {
                Busy = true;
                var loggers = await GeoService.GetLoggersAsync(CancellationToken.None);
                GeoLoggers = loggers?.Select(x => new GeoLoggerViewModel(x)).ToList();
            }
            finally
            {
                Busy = false;
            }
        }

        private void OnGoToLoggerClick(GeoLoggerViewModel? vm)
        {
            if(vm == null)
                return;

            NavigationManager.NavigateTo($"/logger/{vm.Id}");
        }
    }
}
