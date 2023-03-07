namespace Geo.Monitoring.Blazor.Services.Geo;

public class GeoServiceClientMock : IGeoServiceClient
{
    private static readonly int CompanyId = 7788;
    private static readonly int EmployeeId = 4455;
    private readonly SensorLogger[] _loggers;

    public GeoServiceClientMock()
    {
        var r = new Random();

        _loggers = Enumerable.Range(1, 10)
            .Select(i =>
            {
                var sensors = Enumerable.Range(1, 20)
                    .Select(s => new SensorDesc()
                    {
                        Id = s,
                        Type = r.Next(2) == 0 ? SensorType.Pressure : SensorType.Gyroscope,
                        MaxLimit = r.Next(50, 100),
                        MinLimit = r.Next(10, 40)
                    })
                    .ToArray();

                return new SensorLogger()
                {
                    Logger = new SensorLoggerDesc()
                    {
                        Id = i,
                        Name = $"Logger-{i}",
                        SensorCount = sensors.Length
                    },
                    Sensors = sensors
                };
            })
            .ToArray();
    }

    public Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new LoginResponse()
        {
            CompanyId = CompanyId,
            EmployeeId = EmployeeId,
            ErrorMessage = "",
            Successes = true
        });
    }

    public Task<CompanyDetails> GetCompanyInfoAsync(CancellationToken cancellationToken)
    {
        return Task.FromResult(new CompanyDetails()
        {
            Id = CompanyId,
            Name = "Viola Group",
            Address = new Address()
            {
                Country = "Russia",
                City = "Nadym",
                AddressLine = "Lenin Street 1",
                PostalCode = "223344",
                Region = "Siberia"
            }
        });
    }

    public async Task<GetLoggersResponse> GetLoggersAsync(CancellationToken cancellationToken)
    {
        await Task.Delay(1000, cancellationToken);
        return new GetLoggersResponse()
        {
            Loggers = _loggers.Select(x => new SensorLoggerDesc()
            {
                Id = x.Logger.Id,
                Name = x.Logger.Name,
                SensorCount = x.Sensors?.Count ?? 0,
            }).ToArray()
        };
    }

    public async Task<SensorLogger> GetLoggerAsync(int id, CancellationToken cancellationToken)
    {
        await Task.Delay(1000, cancellationToken);
        return _loggers.First(x => x.Logger.Id == id);
    }

    public async Task<IReadOnlyList<SensorPoint>> GetSensorValuesAsync(int id, CancellationToken cancellationToken)
    {
        await Task.Delay(1000, cancellationToken);

        var start = /*request.From ?? */DateTime.UtcNow.AddYears(-1);
        var end = /*request.To ?? */DateTime.UtcNow;

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

    public async Task<SensorDesc> GetSensorAsync(int id, CancellationToken cancellationToken)
    {
        await Task.Delay(1000, cancellationToken);
        return _loggers.SelectMany(x => x.Sensors).First(x => x.Id == id);
    }
}