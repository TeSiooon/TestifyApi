using System.Text.Json;
using System.Text.Json.Serialization;

namespace Testify.API.Converters;

public class TimeSpanJsonConverter : JsonConverter<TimeSpan?>
{
    public override TimeSpan? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if(reader.TokenType == JsonTokenType.String 
            && TimeSpan.TryParse(reader.GetString(), out var result))
            return result;

        return null;
    }

    public override void Write(Utf8JsonWriter writer, TimeSpan? value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value?.ToString());
    }
}
