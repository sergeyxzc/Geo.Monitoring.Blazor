using System.Globalization;
using System.Text.Json;

namespace Geo.Monitoring.Blazor.Services.Geo;

public class GeoTimeConverter : System.Text.Json.Serialization.JsonConverter<DateTime>
{
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var s = reader.GetString();
        if (string.IsNullOrEmpty(s))
            return DateTime.MinValue;

        if (!DateTime.TryParse(s, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out var dt))
            return DateTime.MinValue;
        return dt;
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}