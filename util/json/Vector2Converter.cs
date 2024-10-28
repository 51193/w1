using Godot;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MyGame.Util
{
    namespace MyGame.Util
    {
        internal class Vector2Converter : JsonConverter<Vector2>
        {
            public override Vector2 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType != JsonTokenType.StartObject)
                {
                    GD.PrintErr("Expected start of JSON object for Vector2.");
                    return default;
                }

                float X = 0;
                float Y = 0;

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

                        if (propertyName == "X")
                        {
                            X = reader.GetSingle();
                        }
                        else if (propertyName == "Y")
                        {
                            Y = reader.GetSingle();
                        }
                    }
                }

                return new Vector2(X, Y);
            }

            public override void Write(Utf8JsonWriter writer, Vector2 value, JsonSerializerOptions options)
            {
                writer.WriteStartObject();
                writer.WriteNumber("X", value.X);
                writer.WriteNumber("Y", value.Y);
                writer.WriteEndObject();
            }
        }
    }
}
