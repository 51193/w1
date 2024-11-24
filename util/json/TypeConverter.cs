using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MyGame.Util
{
    public class TypeConverter : JsonConverter<Type>
    {
        public override Type Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string typeName = reader.GetString();
            if (string.IsNullOrWhiteSpace(typeName))
            {
                return null;
            }

            return Type.GetType(typeName) ?? throw new JsonException($"Cannot resolve type '{typeName}'");

        }

        public override void Write(Utf8JsonWriter writer, Type value, JsonSerializerOptions options)
        {
            if (value == null)
            {
                writer.WriteNullValue();
            }
            else
            {
                writer.WriteStringValue(value.FullName);
            }
        }
    }
}
