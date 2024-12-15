using Godot;
using MyGame.Entity.Save;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MyGame.Util
{
    internal class ISaveComponentConverter : JsonConverter<ISaveComponent>
    {
        public override ISaveComponent Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartArray)
            {
                GD.PrintErr("Expected start of JSON array.");
                return default;
            }

            ISaveComponent head = null;
            ISaveComponent current = null;

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndArray)
                {
                    break;
                }

                if (reader.TokenType == JsonTokenType.StartObject)
                {
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

                    Type saveComponentType = Type.GetType(typeName);
                    if (saveComponentType == null)
                    {
                        GD.PrintErr($"Type {typeName} not found.");
                        return default;
                    }

                    ISaveComponent instance = (ISaveComponent)Activator.CreateInstance(saveComponentType);
                    if (instance == null)
                    {
                        GD.PrintErr($"Cannot create instance of type {typeName}.");
                        return default;
                    }

                    foreach (var property in properties)
                    {
                        PropertyInfo propertyInfo = saveComponentType.GetProperty(property.Key);
                        if (propertyInfo != null && propertyInfo.CanWrite)
                        {
                            propertyInfo.SetValue(instance, property.Value);
                        }
                        else
                        {
                            GD.PrintErr($"Property {property.Key} not found or not writable in type {typeName}.");
                        }
                    }

                    if (head == null)
                    {
                        head = instance;
                        current = instance;
                    }
                    else
                    {
                        current.Next = instance;
                        current = instance;
                    }
                }
            }

            return head;
        }


        public override void Write(Utf8JsonWriter writer, ISaveComponent value, JsonSerializerOptions options)
        {
            ISaveComponent cur = value;

            writer.WriteStartArray();

            while (cur != null)
            {
                Type type = cur.GetType();
                PropertyInfo[] propertyInfos = type.GetProperties();
                writer.WriteStartObject();
                writer.WriteString("Type", type.FullName);
                writer.WritePropertyName("Property");
                writer.WriteStartObject();
                foreach (var propertyInfo in propertyInfos)
                {
                    if (propertyInfo.Name != nameof(ISaveComponent.Next))
                    {
                        writer.WritePropertyName(propertyInfo.Name);
                        JsonSerializer.Serialize(writer, propertyInfo.GetValue(cur), options);
                    }
                }
                writer.WriteEndObject();
                writer.WriteEndObject();
                cur = cur.Next;
            }
            writer.WriteEndArray();
        }
    }
}
