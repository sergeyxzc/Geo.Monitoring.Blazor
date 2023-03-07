using System.Text.Json.Serialization;
using Refit;

namespace Geo.Monitoring.Blazor.Services.Geo;

public class LoginRequest
{
    public string LoginName { get; set; }
    public string Password { get; set; }
}

public class LoginResponse
{
    public bool Successes { get; set; }
    public string ErrorMessage { get; set; }
    public int EmployeeId { get; set; }
    public int CompanyId { get; set; }
}

public enum SensorType
{
    Unknown = 0,
    Pressure = 1,
    Gyroscope = 2
}

public class SensorPoint
{
    [JsonPropertyName("sensorData")]
    public double Value { get; set; }

    [JsonPropertyName("timeStamp")]
    [JsonConverter(typeof(GeoTimeConverter))]
    public DateTime Timestamp { get; set; }
}

public class SensorValuesRequest
{
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }
}

public class SensorDesc
{
    public int Id { get; set; }

    [JsonPropertyName("sensortype")]
    public SensorType Type { get; set; }

    [JsonPropertyName("min")]
    public double? MinLimit { get; set; }

    [JsonPropertyName("max")]
    public double? MaxLimit { get; set; }

    [JsonPropertyName("larstData")]
    public double LastValue { get; set; }

    [JsonPropertyName("sensorkey")]
    public string SensorKey { get; set; }

    [JsonPropertyName("larstupdate")]
    [JsonConverter(typeof(GeoTimeConverter))]
    public DateTime UpdateTimestamp { get; set; }
}

public class SensorLoggerDesc
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int SensorCount { get; set; }
    //public IReadOnlyList<SensorDesc> Sensors { get; set; }
}

public class SensorLogger
{
    public SensorLoggerDesc Logger { get; set; }
    public IReadOnlyList<SensorDesc> Sensors { get; set; }
}

public class UpdateSensorLimitsRequest
{
    public int SensorId { get; set; }
    public double? MinLimit { get; set; }
    public double? MaxLimit { get; set; }
}

public class GetLoggersResponse
{
    public IReadOnlyList<SensorLoggerDesc> Loggers { get; set; }
}

public class CompanyDetails
{
    public int Id { get; set; }
    public string Name { get; set; }
    public Address Address { get; set; }
}

public class Address
{
    public string Country { get; set; }
    public string City { get; set; }
    public string Region { get; set; }
    public string PostalCode { get; set; }
    public string AddressLine { get; set; }
}

public interface IGeoServiceClient
{
    [Post("/api/v1/login")]
    Task<LoginResponse> LoginAsync([Body(BodySerializationMethod.Serialized)] LoginRequest request, CancellationToken cancellationToken);

    [Get("/api/v1/company")]
    Task<CompanyDetails> GetCompanyInfoAsync(CancellationToken cancellationToken);

    [Get("/api/v1/logger")]
    Task<GetLoggersResponse> GetLoggersAsync(CancellationToken cancellationToken);

    [Get("/api/v1/logger/{id}")]
    Task<SensorLogger> GetLoggerAsync([Query] int id, CancellationToken cancellationToken);

    [Get("/api/v1/sensor/{id}")]
    Task<SensorDesc> GetSensorAsync([Query] int id, CancellationToken cancellationToken);

    [Get("/api/v1/sensor/{id}/values")]
    Task<IReadOnlyList<SensorPoint>> GetSensorValuesAsync([Query] int id, CancellationToken cancellationToken);
}
