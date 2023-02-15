﻿using Geo.Monitoring.Blazor.Services;
using Microsoft.AspNetCore.Components;

namespace Geo.Monitoring.Blazor.Components
{
    public record SensorPointViewModel(double Value, DateTime Timestamp);

    public partial class Sensor
    {
        [Parameter] public int SensorId { get; set; }
        [Inject] public IGeoService GeoService { get; set; }
        public List<SensorPointViewModel> SensorPoints { get; set; }
        public Services.Sensor SensorInfo { get; set; }

        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public bool Busy { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                Busy = true;
                SensorInfo = await GeoService.GetSensorAsync(new SensorRequest(SensorId), CancellationToken.None);

                var sensorPoints = await GeoService.GetSensorValuesAsync(new SensorValuesRequest() { SensorId = SensorId }, CancellationToken.None);
                SensorPoints = sensorPoints
                    .OrderBy(x => x.Timestamp)
                    .Select(x => new SensorPointViewModel(x.Value, x.Timestamp))
                    .ToList();

                StartDateTime = SensorPoints.First().Timestamp;
                EndDateTime = SensorPoints.Last().Timestamp;

                //if (sensorInfo.Max.HasValue)
                //{
                //    SensorPoints.Add(new SensorPointViewModel(sensorInfo.Max.Value, StartDateTime, SensorPointType.Max));
                //    SensorPoints.Add(new SensorPointViewModel(sensorInfo.Max.Value, EndDateTime, SensorPointType.Max));
                //}

                //if (sensorInfo.Min.HasValue)
                //{
                //    SensorPoints.Add(new SensorPointViewModel(sensorInfo.Min.Value, StartDateTime, SensorPointType.Min));
                //    SensorPoints.Add(new SensorPointViewModel(sensorInfo.Min.Value, EndDateTime, SensorPointType.Min));
                //}
            }
            finally
            {
                Busy = false;
            }
        }
    }
}
