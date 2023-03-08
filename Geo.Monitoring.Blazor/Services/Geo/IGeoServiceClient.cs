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
    public GeoPoint Position { get; set; }
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

public class LoggerDesc
{
    public int Id { get; set; }
    public string Name { get; set; }
    public GeoPoint Position { get; set; }
    public IReadOnlyList<SensorDesc> Sensors { get; set; }
}

public class UpdateSensorLimitsRequest
{
    public int SensorId { get; set; }
    public double? MinLimit { get; set; }
    public double? MaxLimit { get; set; }
}

//public class GetLoggersResponse
//{
//    public IReadOnlyList<SensorLoggerDesc> Loggers { get; set; }
//}

public class CompanyDetails
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string BrandName { get; set; }
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

public class CompanyProjectDesc
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int EmployeeCount { get; set; }
}

public class GeoPoint
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }     
}

public class GetCompanyProjectsResponse
{
    public IReadOnlyList<CompanyProjectDesc>  Projects { get; set; }
}

public class ProjectEmployee
{
    public int ProjectId { get; set; }
    public int EmployeeId { get; set; }
}

public class GetProjectResponse
{
    public CompanyProjectDesc Project { get; set; }
    public IReadOnlyList<LoggerDesc> Loggers { get; set; }
    public IReadOnlyList<ProjectEmployee> Employees { get; set; }
}

public class CreateProjectResponse
{
    public int Id { get; set; }
}

public class CreateProjectRequest
{
    public string Name { get; set; }
}

public class UpdateProjectRequest
{
    public string Name { get; set; }
}

public class UpdateProjectResponse
{
    public int Id { get; set; }
}

public class GetCompanyEmployeesResponse
{
    public IReadOnlyList<EmployeeDesc> Employees { get; set; }
}

public class EmployeeDesc
{
    public int Id { get; set; }
    public int EmployeeRoleId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string MiddleName { get; set; }
    public DateOnly? BirthDate { get; set; }
    public bool IsBeneficiary { get; set; }
}

public class AddProjectEmployeeRequest
{
    public int EmployeeId { get; set; }
}

public interface IGeoServiceClient
{
    [Post("/api/v1/login")]
    Task<LoginResponse> LoginAsync([Body(BodySerializationMethod.Serialized)] LoginRequest request, CancellationToken cancellationToken);

    [Get("/api/v1/company")]
    Task<CompanyDetails> GetCompanyInfoAsync(CancellationToken cancellationToken);

    [Get("/api/v1/company/employee/list")]
    Task<GetCompanyEmployeesResponse> GetCompanyEmployeesAsync(CancellationToken cancellationToken);

    [Get("/api/v1/company/project/list")]
    Task<GetCompanyProjectsResponse> GetCompanyProjectsAsync(CancellationToken cancellationToken);

    [Get("/api/v1/company/project/{id}")]
    Task<GetProjectResponse> GetProjectAsync([Query] int id, CancellationToken cancellationToken);

    [Post("/api/v1/company/project")]
    Task<CreateProjectResponse> CreateProjectAsync(
        [Body(BodySerializationMethod.Serialized)] CreateProjectRequest request, 
        CancellationToken cancellationToken);

    [Post("/api/v1/company/project/{id}")]
    Task<UpdateProjectResponse> UpdateProjectAsync([Query] int id, 
        [Body(BodySerializationMethod.Serialized)] UpdateProjectRequest request, 
        CancellationToken cancellationToken);

    [Post("/api/v1/company/project/{id}/employee")]
    Task<ProjectEmployee> AddProjectEmployeeAsync([Query] int id,
        [Body(BodySerializationMethod.Serialized)] AddProjectEmployeeRequest request,
        CancellationToken cancellationToken);

    //[Get("/api/v1/company/project/{projectId}/logger/list")]
    //Task<GetLoggersResponse> GetLoggersAsync([Query] CancellationToken cancellationToken);

    [Get("/api/v1/company/logger/{id}")]
    Task<LoggerDesc> GetLoggerAsync([Query] int id, CancellationToken cancellationToken);

    [Get("/api/v1/company/sensor/{id}")]
    Task<SensorDesc> GetSensorAsync([Query] int id, CancellationToken cancellationToken);

    [Get("/api/v1/company/sensor/{id}/values")]
    Task<IReadOnlyList<SensorPoint>> GetSensorValuesAsync([Query] int id, CancellationToken cancellationToken);
}
