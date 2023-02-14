using Geo.Monitoring.Blazor.Services;
using Microsoft.AspNetCore.Components;

namespace Geo.Monitoring.Blazor.Components
{
    public partial class Sensor
    {
        [Parameter] public int SensorId { get; set; }
        [Inject] public IGeoService GeoService { get; set; }
        public IReadOnlyList<SensorPoint> SensorPoints { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public bool Busy { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                Busy = true;
                var sensorPoints = await GeoService.GetSensorValuesAsync(new SensorValuesRequest() { SensorId = SensorId }, CancellationToken.None);
                SensorPoints = sensorPoints.OrderBy(x => x.Timestamp).ToList();

                StartDateTime = SensorPoints.First().Timestamp;
                EndDateTime = SensorPoints.Last().Timestamp;
            }
            finally
            {
                Busy = false;
            }
        }
    }
}
