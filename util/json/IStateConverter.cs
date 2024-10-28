using Godot;
using MyGame.Component;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MyGame.Util
{
    public class IStateConverter : JsonConverter<IState>
    {
        public override IState Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                GD.PrintErr("Expected start of JSON object.");
                return default;
            }

            string typeName = null;
            Dictionary<string, object> properties = new();

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
                        while (reader.Read())
                        {
                            if (reader.TokenType == JsonTokenType.EndObject)
                            {
                                break;
                            }

                            if (reader.TokenType == JsonTokenType.PropertyName)
                            {
                                string propName = reader.GetString();
                                Type propType = Type.GetType(typeName).GetProperty(propName).PropertyType;
                                reader.Read();
                                var value = JsonSerializer.Deserialize(ref reader, propType, options);
                                properties[propName] = value;
                            }
                        }
                    }
                }
            }

            if (typeName == null)
            {
                GD.PrintErr("Missing Type property in JSON.");
                return default;
            }

            Type stateType = Type.GetType(typeName);
            if (stateType == null)
            {
                GD.PrintErr($"Type {typeName} not found.");
                return default;
            }

            IState instance = (IState)Activator.CreateInstance(stateType);
            if (instance == null)
            {
                GD.PrintErr($"Cannot create instance of type {typeName}.");
                return default;
            }

            foreach (var property in properties)
            {
                PropertyInfo propertyInfo = stateType.GetProperty(property.Key);
                if (propertyInfo != null && propertyInfo.CanWrite)
                {
                    propertyInfo.SetValue(instance, property.Value);
                }
                else
                {
                    GD.PrintErr($"Property {property.Key} not found or not writable in type {typeName}.");
                }
            }

            return instance;
        }


        public override void Write(Utf8JsonWriter writer, IState value, JsonSerializerOptions options)
        {
            Type type = value.GetType();
            PropertyInfo[] propertyInfos = type.GetProperties();

            writer.WriteStartObject();
            writer.WriteString("Type", type.FullName);
            writer.WritePropertyName("Property");
            writer.WriteStartObject();
            foreach (var propertyInfo in propertyInfos)
            {
                writer.WritePropertyName(propertyInfo.Name);
                JsonSerializer.Serialize(writer, propertyInfo.GetValue(value), options);
            }
            writer.WriteEndObject();
            writer.WriteEndObject();
        }
    }
}
