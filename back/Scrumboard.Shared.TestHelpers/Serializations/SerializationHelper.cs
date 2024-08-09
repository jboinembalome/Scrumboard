using Newtonsoft.Json;
using Scrumboard.Shared.TestHelpers.Serializations.Converters;

namespace Scrumboard.Shared.TestHelpers.Serializations;

public static class SerializationHelper
{
    private static readonly JsonSerializerSettings SerializerSettings = new()
    {
        Converters = new JsonConverter[]
        {
            new DateOnlyConverter(),
            new TimeOnlyConverter()
        }
    };

    public static T? Deserialize<T>(string json)
        => JsonConvert.DeserializeObject<T>(json, SerializerSettings);

    public static string Serialize(object value)
        => JsonConvert.SerializeObject(value, SerializerSettings);

    public static string Reserialize(string json)
    {
        var obj = Deserialize<object>(json) ?? throw new InvalidOperationException($"{nameof(json)} should not deserialize to null");

        return Serialize(obj);
    }
}
