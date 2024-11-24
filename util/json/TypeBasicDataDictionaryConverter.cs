using MyGame.Entity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MyGame.Util
{
    public class TypeBasicDataDictionaryConverter : JsonConverter<Dictionary<Type, BasicData>>
    {
        public override Dictionary<Type, BasicData> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using var jsonDoc = JsonDocument.ParseValue(ref reader);
            var root = jsonDoc.RootElement;

            var result = new Dictionary<Type, BasicData>();

            var basicDataType = typeof(BasicData);
            var derivedTypes = Assembly.GetExecutingAssembly()
                                       .GetTypes()
                                       .Where(t => t.IsSubclassOf(basicDataType));

            foreach (var property in root.EnumerateObject())
            {
                var typeName = property.Name;

                var targetType = derivedTypes.FirstOrDefault(t => t.FullName == typeName);
                if (targetType == null) continue;

                var instance = (BasicData)JsonSerializer.Deserialize(property.Value.GetRawText(), targetType, options);

                result.Add(targetType, instance);
            }

            return result;
        }

        public override void Write(Utf8JsonWriter writer, Dictionary<Type, BasicData> value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            foreach (var kvp in value)
            {
                var type = kvp.Key;
                var data = kvp.Value;

                writer.WritePropertyName(type.FullName);

                JsonSerializer.Serialize(writer, data, type, options);
            }

            writer.WriteEndObject();
        }
    }
}
