using Geo.Monitoring.Blazor.Services;
using Geo.Monitoring.Blazor.Services.Geo;
using Microsoft.AspNetCore.Components;

namespace Geo.Monitoring.Blazor.Components
{
    public class GeoLoggerViewModel
    {
        //private readonly Services.Geo.SensorLoggerDesc _logger;

        public GeoLoggerViewModel(/*Services.Geo.SensorLoggerDesc logger*/)
        {
            //_logger = logger;
            //Sensors = logger?.SensorCount ?? 0;
        }

        public int Id => 1;//_logger.Id;
        public string Name => "";//_logger.Name;
        public int Sensors { get; }
    }

    public partial class SensorLoggers
    {
        public bool Busy { get; set; }
        public IReadOnlyList<GeoLoggerViewModel> GeoLoggers { get; set; }
        [Inject] public IGeoServiceClient GeoService { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }


        protected override async Task OnInitializedAsync()
        {
            try
            {
                Busy = true;
                //var loggers = await GeoService.GetLoggersAsync(ComponentCancellationToken);
                //GeoLoggers = loggers?.Loggers?.Select(x => new GeoLoggerViewModel(x)).ToList();
            }
            finally
            {
                Busy = false;
            }
        }

        private void OnGoToLoggerClick(GeoLoggerViewModel vm)
        {
            if(vm == null)
                return;

            NavigationManager.NavigateTo($"/logger/{vm.Id}");
        }
    }
}
