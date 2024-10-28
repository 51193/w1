using Godot;
using MyGame.Manager;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MyGame.Util
{
    internal class VariableLengthParameterConverter : JsonConverter<VariableLengthParameter>
    {
        public override VariableLengthParameter Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartArray)
            {
                GD.PrintErr("Expected start of JSON array.");
                return default;
            }

            var parameters = new List<object>();

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndArray)
                {
                    break;
                }

                if (reader.TokenType == JsonTokenType.StartObject)
                {
                    string typeName = null;
                    object parameterInstance = null;

                    while (reader.Read())
                    {
                        if (reader.TokenType == JsonTokenType.EndObject)
                        {
                            break;
                        }

                        if (reader.TokenType == JsonTokenType.PropertyName)
                        {
                            string propertyName = reader.GetString();
                            reader.Read();

                            if (propertyName == "Type")
                            {
                                typeName = reader.GetString();
                            }
                            else if (propertyName == "Property")
                            {
                                parameterInstance = JsonSerializer.Deserialize(ref reader, Type.GetType(typeName), options);
                            }
                        }
                    }

                    if (typeName == null)
                    {
                        GD.PrintErr("Missing Type property in JSON.");
                        return default;
                    }

                    Type parameterType = Type.GetType(typeName);
                    if (parameterType == null)
                    {
                        GD.PrintErr($"Type {typeName} not found.");
                        return default;
                    }

                    if (parameterInstance != null && parameterType.IsAssignableFrom(parameterInstance.GetType()))
                    {
                        parameters.Add(parameterInstance);
                    }
                    else
                    {
                        GD.PrintErr($"Invalid parameter instance for type {typeName}.");
                    }
                }
            }

            return new VariableLengthParameter(parameters);
        }


        public override void Write(Utf8JsonWriter writer, VariableLengthParameter value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            foreach (var parameter in value.Parameters)
            {
                writer.WriteStartObject();
                Type type = parameter.GetType();
                writer.WriteString("Type", type.FullName);
                writer.WritePropertyName("Property");
                JsonSerializer.Serialize(writer, parameter, options);
                writer.WriteEndObject();
            }
            writer.WriteEndArray();
        }
    }
}
