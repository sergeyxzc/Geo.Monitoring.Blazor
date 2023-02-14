namespace Geo.Monitoring.Blazor.Services
{
    public enum SensorType
    {
        Unknown = 0,
        Pressure = 1,
        Gyroscope = 2
    }

    public class SensorPoint
    {
        public double Value { get; set; }
        public DateTime Timestamp { get; set; }
    }

    public class SensorValuesRequest
    {
        public int SensorId { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }

    public class SensorDesc
    {
        public int Id { get; set; }
        public SensorType Type { get; set; }
        public double? MinLimit { get; set; }
        public double? MaxLimit { get; set; }
    }

    public class SensorLogger
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IReadOnlyList<SensorDesc> Sensors { get; set; }
    }

    public class UpdateSensorLimitsRequest
    {
        public int SensorId { get; set; }
        public double? MinLimit { get; set; }
        public double? MaxLimit { get; set; }
    }

    public class GetLoggerRequest
    {
        public int LoggerId { get; set; }
    }


    public interface IGeoService
    {
        Task<IReadOnlyList<SensorLogger>> GetLoggersAsync(CancellationToken cancellationToken);
        Task<SensorLogger> GetLoggerAsync(GetLoggerRequest request, CancellationToken cancellationToken);
        Task<IReadOnlyList<SensorPoint>> GetSensorValuesAsync(SensorValuesRequest request, CancellationToken cancellationToken);
        Task UpdateSensorLimitsAsync(UpdateSensorLimitsRequest request, CancellationToken cancellationToken);
    }

    public class GeoServiceMock : IGeoService
    {
        private readonly SensorLogger[] _loggers;

        public GeoServiceMock()
        {
            var r = new Random();

            _loggers = Enumerable.Range(1, 10)
                .Select(i => new SensorLogger()
                {
                    Id = i,
                    Name = $"Logger-{i}",
                    Sensors = Enumerable.Range(1, 20)
                        .Select(s => new SensorDesc()
                        {
                            Id = s,
                            Type = r.Next(2) == 0 ? SensorType.Pressure : SensorType.Gyroscope,
                            MaxLimit = r.Next(50, 100),
                            MinLimit = r.Next(10, 40)
                        })
                        .ToArray()
                })
                .ToArray();
        }

        public async Task<IReadOnlyList<SensorLogger>> GetLoggersAsync(CancellationToken cancellationToken)
        {
            await Task.Delay(1000, cancellationToken);
            return _loggers;
        }

        public async Task<SensorLogger> GetLoggerAsync(GetLoggerRequest request, CancellationToken cancellationToken)
        {
            await Task.Delay(1000, cancellationToken);
            return _loggers.First(x => x.Id == request.LoggerId);
        }

        public async Task<IReadOnlyList<SensorPoint>> GetSensorValuesAsync(SensorValuesRequest request, CancellationToken cancellationToken)
        {
            await Task.Delay(1000, cancellationToken);

            var start = request.From ?? DateTime.UtcNow.AddYears(-1);
            var end = request.To ?? DateTime.UtcNow;

            if (start > end)
                start = end;

            var step = (end - start).TotalHours / 500;

            var r = new Random();

            var result = Enumerable.Range(1, 500).Select(i =>
            {
                var sp = new SensorPoint()
                {
                    Timestamp = start.AddHours(step),
                    Value = r.NextDouble() * 100.0
                };
                start = sp.Timestamp;
                return sp;
            }).ToArray();

            return result;
        }

        public async Task UpdateSensorLimitsAsync(UpdateSensorLimitsRequest request, CancellationToken cancellationToken)
        {
            await Task.Delay(1000, cancellationToken);
        }
    }
}
