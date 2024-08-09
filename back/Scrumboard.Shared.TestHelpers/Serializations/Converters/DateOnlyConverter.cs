using System.Globalization;
using Newtonsoft.Json;

namespace Scrumboard.Shared.TestHelpers.Serializations.Converters;

public sealed class TimeOnlyConverter : JsonConverter<TimeOnly>
{
    private const string TimeFormat = "O";

    public override void WriteJson(JsonWriter writer, TimeOnly value, JsonSerializer serializer)
        => writer.WriteValue(value.ToString(TimeFormat, CultureInfo.InvariantCulture));

    public override TimeOnly ReadJson(JsonReader reader, Type objectType, TimeOnly existingValue, bool hasExistingValue, JsonSerializer serializer)
        => TimeOnly.ParseExact((string)reader.Value!, TimeFormat, CultureInfo.InvariantCulture);
}
