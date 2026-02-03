using System.Text.Json;
using System.Text.Json.Serialization;

namespace Aban360.BrdigeApi.Filters
{
    public class TruncateDoubleConverter : JsonConverter<double>
    {
        public override double Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Don't modify incoming data
            return reader.GetDouble();
        }

        public override void Write(Utf8JsonWriter writer, double value, JsonSerializerOptions options)
        {
            // Truncate outgoing data to 1 decimal place
            double truncated = Math.Truncate(value * 10) / 10;
            writer.WriteNumberValue(truncated);
        }
    }

    public class TruncateFloatConverter : JsonConverter<float>
    {
        public override float Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return reader.GetSingle();
        }

        public override void Write(Utf8JsonWriter writer, float value, JsonSerializerOptions options)
        {
            float truncated = (float)(Math.Truncate(value * 10) / 10);
            writer.WriteNumberValue(truncated);
        }
    }

    // Optional: Nullable versions
    public class TruncateNullableDoubleConverter : JsonConverter<double?>
    {
        public override double? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return reader.TokenType == JsonTokenType.Null ? null : reader.GetDouble();
        }

        public override void Write(Utf8JsonWriter writer, double? value, JsonSerializerOptions options)
        {
            if (value == null)
                writer.WriteNullValue();
            else
            {
                double truncated = Math.Truncate(value.Value * 10) / 10;
                writer.WriteNumberValue(truncated);
            }
        }
    }    
}
