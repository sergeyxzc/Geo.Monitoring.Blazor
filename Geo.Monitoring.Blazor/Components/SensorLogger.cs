using DevExpress.Blazor.Internal.ComponentStructureHelpers;
using Geo.Monitoring.Blazor.Services;
using Geo.Monitoring.Blazor.Services.Geo;
using Microsoft.AspNetCore.Components;

namespace Geo.Monitoring.Blazor.Components
{
    public class SensorViewModel
    {
        private readonly SensorDesc _sensor;

        public SensorViewModel(Services.Geo.SensorDesc sensor)
        {
            _sensor = sensor;
        }

        public int Id => _sensor.Id;
        public string Type => _sensor.Type.ToString("G");
        public string MinLimit => _sensor.MinLimit.HasValue ? _sensor.MinLimit.Value.ToString("N") : "N/A";
        public string MaxLimit => _sensor.MaxLimit.HasValue ? _sensor.MaxLimit.Value.ToString("N") : "N/A";
    }

    public partial class SensorLogger
    {
        [Parameter]
        public int LoggerId { get; set; }

        public bool Busy { get; set; }

        public IReadOnlyList<SensorViewModel> Sensors { get; set; }
        [Inject] public IGeoServiceClient GeoService { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        public string LoggerName { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                Busy = true;
                var logger = await GeoService.GetLoggerAsync(LoggerId, ComponentCancellationToken);
                Sensors = logger.Sensors.Select(x => new SensorViewModel(x)).ToList();
                LoggerName = logger.Logger.Name;
            }
            finally
            {
                Busy = false;
            }
        }

        private void OnGoToSensorClick(SensorViewModel vm)
        {
            if (vm == null)
                return;

            NavigationManager.NavigateTo($"/sensor/{vm.Id}");
        }
    }
}
